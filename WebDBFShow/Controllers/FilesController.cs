using Contracts;
using Contracts.DBF;
using Contracts.Repository;
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
        private ILogger<FilesController> _logger;
        IShowService _service;

        public FilesController(ILogger<FilesController> logger, IShowService service)
        {            
            _logger = logger;
            _service = service;
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
                var info = await _service.UploadFile(file);
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
            var info = _service.OpenFile(fileId); 
            return StatusCode(StatusCodes.Status201Created, info);
        }
    }
}
