using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebDBFShow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MessageController : Controller
    {
        [HttpPost]
        [Route("send")]
        [EnableCors("Policy1")]
        public IActionResult Send([FromForm]string text)
        {
            System.IO.File.AppendAllText("messages.txt", DateTime.Now.ToString()+" : ");
            System.IO.File.AppendAllText("messages.txt", text);
            System.IO.File.AppendAllText("messages.txt", Environment.NewLine);
            return Ok();
        }
    }
}
