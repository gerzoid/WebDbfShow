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
        IFileDbReader _reader;

        public ReportController(IFileDbReader reader, ILogger<ReportController> logger)
        {
            _logger = logger;
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
    }
}
