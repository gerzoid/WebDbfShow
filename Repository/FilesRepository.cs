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
    public class FilesRepository : Repository<Files>, IFilesRepository
    {
        public FilesRepository(DbContext context) : base(context)
        {
        }

        public void CreateFile(Files file)
        {
            Create(file);
        }

        public IEnumerable<Files> GetFiles()
        {
            return Get();
        }


    }
}
