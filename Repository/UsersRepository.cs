using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UsersRepository : Repository<Users>
    {
        public UsersRepository(DbContext context) : base(context)
        {
        }
    }
}
