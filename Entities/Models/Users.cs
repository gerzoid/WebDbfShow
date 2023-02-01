using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Users
    {
        public Guid UsersId { get; set; }
        public string? Name { get; set; }
        public ICollection<Files>? Files { get; set; }
    }
}
