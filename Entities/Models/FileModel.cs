using Microsoft.AspNetCore.Http;

namespace Entities.Models
{
    public class FileModel
    {        
        public string FileName { get; set; }
        public string UserId {  get; set; }
        public IFormFile FormFile { get; set; }
    }
}
