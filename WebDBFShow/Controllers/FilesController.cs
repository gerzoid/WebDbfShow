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
        [RequestSizeLimit(10_000_000)]
        //[RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<ActionResult> Upload([FromForm] FileModel file)
        {
            try
            {
                var fileId = Guid.NewGuid();
                
                var newFile = new Entities.Models.Files() { 
                    UserId= Guid.Parse(file.UserId),                     
                    OriginalName = file.FileName, 
                    FilesId = fileId,
                    Size=file.FormFile.Length,
                    Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fileId.ToString() + Path.GetExtension(file.FileName)) 
                };
               
                using (Stream stream = new FileStream(newFile.Path, FileMode.Create))
                {
                    await file.FormFile.CopyToAsync(stream);
                }
                _manager.FilesRepository.CreateFile(newFile);
                _manager.Save();
                var info = _reader.OpenFile(newFile.Path);

                return StatusCode(StatusCodes.Status201Created, info);
            }
            catch(Exception E)
            {
                Console.WriteLine(E.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }        
    }
}
