﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IFilesRepository
    {
        public IEnumerable<Files> GetFiles();
        public void CreateFile(Files file);
        
        public void RemoveFile(Files item);

    }
}
