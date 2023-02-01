﻿using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FilesRepository : Repository<Files>
    {
        public FilesRepository(DbContext context) : base(context)
        {
        }
    }
}
