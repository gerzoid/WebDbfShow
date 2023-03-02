using Contracts;
using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Mapster;
using Microsoft.AspNetCore.Cors;

namespace WebDBFShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IRepositoryManager _manager;
        ILogger<UsersController> _logger;

        public UsersController(IRepositoryManager manager, ILogger<UsersController> logger)
        {
            _logger = logger;
            _manager = manager;
        }
        
        [HttpPost]
        [EnableCors("Policy1")]
        [Route("check")]
        public ActionResult Check([FromForm] string? userId)
        {
            _logger.LogInformation($"Received a new request to verify the user userId={userId}");
            var user = _manager.UsersRepository.Get().Where(d => d.UsersId.ToString() == userId).SingleOrDefault();

            if (user == null)
            {
                user = new Users() { UsersId = Guid.NewGuid(), Name = "" };
                _manager.UsersRepository.Create(user);
                _manager.Save();
            }
            else
            {
                //TypeAdapterConfig<Users, UserDto>.NewConfig().Include<Files, FileDto>();
                var files = _manager.FilesRepository.Get().Where(d => d.UserId.ToString() == userId).OrderByDescending(d=>d.CreatedAt).ToList();
                user.Files = files;
            }
            return Ok(user);
        }
    }
}
