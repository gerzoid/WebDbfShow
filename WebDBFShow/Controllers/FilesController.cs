using Contracts;
using Contracts.DBF;
using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using Newtonsoft.Json;

namespace WebDBFShow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        IRepositoryManager _manager;
        private ILogger<FilesController> _logger;
        IFileDbReader _reader;

        public FilesController(ILogger<FilesController> logger, IFileDbReader reader, IRepositoryManager manager)
        {
            _manager = manager;
            _logger = logger;
            _reader = reader;
        }

        [HttpPost]
        [EnableCors("Policy1")]
        [Route("download")]
        public IActionResult DownloadFile([FromForm] string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fileName.ToString());
            var fs = new FileStream(path, FileMode.Open);
            return File(fs, "application/octet-stream", fileName);
        }

        [HttpPost]
        [EnableCors("Policy1")]
        [RequestSizeLimit(50_000_000)]
        //[RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<ActionResult> Upload([FromForm] FileModel file)
        {
            try
            {
                var fileId = Guid.NewGuid();
                var newFile = new Entities.Models.Files()
                {
                    CreatedAt = DateTime.Now,
                    UserId = Guid.Parse(file.UserId),
                    OriginalName = file.FileName,
                    FilesId = fileId,
                    Size = file.FormFile.Length,
                    Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fileId.ToString() + Path.GetExtension(file.FileName))
                };

                //TODO создать новый сервисный класс для всей логики

                using (Stream stream = new FileStream(newFile.Path, FileMode.Create))
                {
                    await file.FormFile.CopyToAsync(stream);
                }
                var info = _reader.OpenFile(newFile.Path);

                _manager.FilesRepository.Create(newFile);
                _manager.Save();
                var files = _manager.FilesRepository.Get().Where(d => d.UserId == Guid.Parse(file.UserId)).OrderByDescending(d => d.CreatedAt).Skip(5).ToList();
                foreach (var item in files)
                {                   
                    System.IO.File.Delete(item.Path);
                    _manager.FilesRepository.Remove(item);
                    _manager.Save();
                }          

                return StatusCode(StatusCodes.Status201Created, info);
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [EnableCors("Policy1")]
        [Route("open")]
        public async Task<ActionResult> Open([FromForm] string fileId)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fileId + ".dbf");
            var info = _reader.OpenFile(path);    //TODO переделать, что бы формат файла либо передавался, либо из базы брать, так как может быть другой формат
            return StatusCode(StatusCodes.Status201Created, info);
        }
    }
}
