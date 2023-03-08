using Contracts;
using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Mapster;
using Microsoft.AspNetCore.Cors;
using System.Reflection.PortableExecutable;
using Contracts.DBF;

namespace WebDBFShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        ILogger<ReportController> _logger;
        IShowService _service;
        IFileDbReader _reader;
        public ReportController(ILogger<ReportController> logger, IShowService service, IFileDbReader reader)
        {
            _logger = logger;            
            _service = service;
            _reader = reader;
        }

        [HttpPost]
        [EnableCors("Policy1")]
        [Route("statistics")]
        public ActionResult Statistics([FromForm] string? fileName)
        {
            var stat = _reader.CalculateStatistics(fileName);
            return Ok(stat);
        }
        [HttpPost]
        [EnableCors("Policy1")]
        [Route("group")]
        public ActionResult Group([FromForm] string? field, string? fileName)
        {
            
            var stat = _reader.CalculateGroup(Request.Form["fileName"].ToString(), Request.Form["field"].ToString());
            return Ok(stat);
        }

    }
}
