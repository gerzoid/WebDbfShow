using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UsersRepository : Repository<Users>, IUsersRepository
    {
        public UsersRepository(DatabaseContext context) : base(context)
        {
        }

        public IEnumerable<Users> GetUsers()
        {
            return Get();
        }
    }
}
