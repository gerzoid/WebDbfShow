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
            var user = _manager.UsersRepository.GetUsers().Where(d => d.UsersId.ToString() == userId).SingleOrDefault();

            if (user == null)
            {
                var userGuid = Guid.NewGuid();
                _manager.UsersRepository.Create(new Users() { UsersId = userGuid, Name = "" });
                _manager.Save();
                userId = userGuid.ToString();
            }
            else
            {
                //TypeAdapterConfig<Users, UserDto>.NewConfig().Include<Files, FileDto>();
                var files = _manager.FilesRepository.GetFiles().Where(d => d.UserId.ToString() == userId).ToList();
                var result = new UserDto();
                //user.Adapt(result).

            }
            //_manager.UsersRepository.Create(new Users
            //{
            //UsersId = new Guid(Guid.NewGuid().ToString()),
            //Name = "gerz",
            //});
            /*_manager.FilesRepository.CreateFile(new Files()
            {
                FilesId= new Guid(Guid.NewGuid().ToString()),
                UserId = new Guid("ef4af07d-0646-494f-be3a-5dd53c70e376"),
                Name = "test",
            });
            _manager.Save();*/

            return Ok(userId);
            //return Ok(_manager.FilesRepository.GetFiles());
            //return Ok(_manager.UsersRepository.GetUsers());
        }
    }
}
