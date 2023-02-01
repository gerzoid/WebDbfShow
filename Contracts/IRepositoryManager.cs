﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IFilesRepository FilesRepository { get; }
        IUsersRepository UsersRepository { get; }
        void Save();
    }
}
