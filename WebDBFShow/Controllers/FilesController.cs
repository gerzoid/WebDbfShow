using Contracts.DBF;
using Entities.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Upload([FromForm] FileModel file)
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    file.FormFile.CopyTo(stream);
                }
                return StatusCode(StatusCodes.Status201Created);
            }
            catch(Exception E)
            {
                Console.WriteLine(E.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        
        
        [HttpGet]
        public ActionResult Index()
        {
            var t = _reader.OpenFile(@"c:\1\test.dbf");
            return Ok(t);
        }
    }
}
