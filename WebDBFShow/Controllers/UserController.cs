using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace WebDBFShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IRepositoryManager _manager;
        public UsersController(IRepositoryManager manager)
        {
            _manager = manager;
        }
        
        [HttpGet]
        public ActionResult Get()
        {
            throw new NotImplementedException();
            return Ok(_manager.UsersRepository.GetUsers());
        }
    }
}
