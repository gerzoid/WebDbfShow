using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class FileDto
    {
        public Guid FilesId { get; set; }     
        public string? Name { get; set; }
        public string? OriginalName { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Path { get; set; }
        public int? Size { get; set; }
        public Guid UserId { get; set; }
    }
}
