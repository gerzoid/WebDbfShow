using Contracts.DBF;
using Entities.Models;
using Entities.Query;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using Newtonsoft.Json;

namespace WebDBFShow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EditorController : Controller
    {
        private ILogger<FilesController> _logger;
        IFileDbReader _reader;

        public EditorController(ILogger<FilesController> logger, IFileDbReader reader)
        {
            _logger = logger;
            _reader = reader;
        }

        [HttpPost]
        [EnableCors("Policy1")]
        [Route("getdata")]
        //public async Task<ActionResult> GetData([FromForm] QueryGetData data)
        public async Task<ActionResult> GetData(QueryGetData data)
        {
            var result = _reader.GetData(data);
            return Ok(result);
        }

        [HttpPost]
        [EnableCors("Policy1")]
        [Route("modify")]
        public async Task<ActionResult> Modify(ListQueryModifyData data)
        {            
            var result = _reader.ModifyData(data);
            return Ok(result);
        }

        [HttpPost]
        [EnableCors("Policy1")]
        [Route("encoding")]
        public async Task<ActionResult> SetEncoding([FromForm]QueryEncodingData data)
        {
            var result = _reader.SetEncoding(data);
            if (result)
                return Ok(result);
            return BadRequest();
        }
    }
}
