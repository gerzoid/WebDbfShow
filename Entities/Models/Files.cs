using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Files
    {
        public Guid FilesId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Size { get; set; }
        public Guid UserId { get; set; }
        public Users? User { get; set; }
    }
}
