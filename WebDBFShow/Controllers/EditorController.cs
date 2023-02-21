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
        public async Task<ActionResult> GetData([FromForm]QueryGetData data)
        {
            var result = _reader.GetData(data);
            return Ok(result);
        }

        [HttpPost]
        [EnableCors("Policy1")]
        [Route("modify")]
        public async Task<ActionResult> Modify(ListQueryModifyData data)
        {
                //await _appService.UpdateReestrsAccount(result);
              
            //var result = _reader.GetData(data);
            return Ok();
        }
    }
}
