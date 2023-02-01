using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Users
    {
        
        public Guid UsersId { get; set; }
        
        [Required]
        [MinLength(5)]
        public string? Name { get; set; }
        public ICollection<Files>? Files { get; set; }
    }
}
