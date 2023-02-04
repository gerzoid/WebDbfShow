﻿using Contracts;
using Contracts.DBF;
using DbfFile;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace WebDBFShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IRepositoryManager _manager;
        IDbfReader _reader;
        public UsersController(IRepositoryManager manager, IDbfReader reader)
        {
            _manager = manager;
            _reader = reader;
        }
        
        [HttpGet]
        public ActionResult Get()
        {
            //_manager.UsersRepository.Create(new Users
            //{
            //UsersId = new Guid(Guid.NewGuid().ToString()),
            //Name = "gerz",
            //});
            /*_manager.FilesRepository.CreateFile(new Files()
            {
                FilesId= new Guid(Guid.NewGuid().ToString()),
                UserId = new Guid("ef4af07d-0646-494f-be3a-5dd53c70e376"),
                Name = "test",
            });
            _manager.Save();*/
            var t =  _reader.OpenFile(@"c:\1\test.dbf");
            return Ok(t);
            //return Ok(_manager.FilesRepository.GetFiles());
            //return Ok(_manager.UsersRepository.GetUsers());
        }
    }
}
