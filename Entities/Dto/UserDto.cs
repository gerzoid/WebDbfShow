using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public List<FileDto>? Files {get; set;}
    }
}
