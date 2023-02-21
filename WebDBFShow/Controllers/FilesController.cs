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
        private ILogger<FilesController> _logger;
        IFileDbReader _reader;

        public FilesController(ILogger<FilesController> logger, IFileDbReader reader)
        {
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
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", Guid.NewGuid().ToString() +Path.GetExtension(file.FileName));
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await file.FormFile.CopyToAsync(stream);
                }
                var info = _reader.OpenFile(path);

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
