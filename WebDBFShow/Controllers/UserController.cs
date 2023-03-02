using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Mapster;
using Microsoft.AspNetCore.Cors;
using Contracts.Repository;
using Contracts;

namespace WebDBFShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IShowService _service;
        ILogger<UsersController> _logger;

        public UsersController(IShowService service, ILogger<UsersController> logger)
        {
            _logger = logger;
            _service= service;
        }
        
        [HttpPost]
        [EnableCors("Policy1")]
        [Route("check")]
        public ActionResult Check([FromForm] string? userId)
        {
            _logger.LogInformation($"Received a new request to verify the user userId={userId}");
            var user = _service.GetOrCreateUser(userId);
            return Ok(user);
        }
    }
}
