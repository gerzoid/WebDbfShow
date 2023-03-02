using Contracts.Repository;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private DatabaseContext _context;
        
        IFilesRepository? _filesRepository;
        IUsersRepository? _usersRepository;
        
        public IFilesRepository FilesRepository
        {
            get {
                if (_filesRepository == null)
                    _filesRepository = new FilesRepository(_context);
                return _filesRepository;
                 }
        }
        public IUsersRepository UsersRepository
        {
            get
            {
                if (_usersRepository == null)
                    _usersRepository = new UsersRepository(_context);
                return _usersRepository;
            }
        }

        public RepositoryManager(DatabaseContext context)
        {
            _context = context;
        }        

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
