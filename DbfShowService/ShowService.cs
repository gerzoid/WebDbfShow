using Contracts;
using Contracts.DBF;
using Contracts.Repository;
using Entities.Answer;
using Entities.Models;
using Entities.Query;
using Entities.Todo;
using Microsoft.Extensions.Logging;

namespace DbfShowService
{
    public class ShowService:IShowService
    {
        IRepositoryManager _manager;
        ILogger<ShowService> _logger;
        IFileDbReader _reader;
        const int COUNT_MAX_UPLOAD_FILES= 5;
        
        public ShowService(ILogger<ShowService> logger, IRepositoryManager manager, IFileDbReader reader)
        {
            _logger = logger;
            _manager = manager;
            _reader = reader;
        }

        //Загрузка файла на сервер
        public async Task<DbfInfo> UploadFile(FileModel file)
        {
            var fileId = Guid.NewGuid();
            var newFile = new Entities.Models.Files()
            {
                CreatedAt = DateTime.Now,
                UserId = Guid.Parse(file.UserId),
                OriginalName = file.FileName,
                FilesId = fileId,
                Size = file.FormFile.Length,
                Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fileId.ToString() + Path.GetExtension(file.FileName.ToLower()))
            };

            //TODO создать новый сервисный класс для всей логики

            using (Stream stream = new FileStream(newFile.Path, FileMode.Create))
            {
                await file.FormFile.CopyToAsync(stream);
            }
            var info = _reader.OpenFile(newFile.Path);

            _manager.FilesRepository.Create(newFile);
            _manager.Save();
            
            //Удаляем лишние файлы из базы и физически
            var files = _manager.FilesRepository.Find(d => d.UserId == Guid.Parse(file.UserId)).OrderByDescending(d => d.CreatedAt).Skip(COUNT_MAX_UPLOAD_FILES).ToList();
            foreach (var item in files)
            {
                System.IO.File.Delete(item.Path);
                _manager.FilesRepository.Remove(item);
                _manager.Save();
            }
            return info;

        }

        //Получить или создать пользователя
        public Users GetOrCreateUser(string userId)
        {
            var user = _manager.UsersRepository.Get().Where(d => d.UsersId.ToString() == userId).SingleOrDefault();

            if (user == null)
            {
                user = new Users() { UsersId = Guid.NewGuid(), Name = "" };
                _manager.UsersRepository.Create(user);
                _manager.Save();
            }
            else
            {
                var files = _manager.FilesRepository.Get().Where(d => d.UserId.ToString() == userId).OrderByDescending(d => d.CreatedAt).ToList();
                user.Files = files;
            }
            return user;

        }

        public DbfInfo OpenFile(string fileId)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fileId + ".dbf");
            return _reader.OpenFile(path);
        }

        public List<RecordStat> CalculateStatistics(string fileName)
        {
            //TODO переделать, что бы формат файла либо передавался, либо из базы брать, так как может быть другой формат
            return _reader.CalculateStatistics(fileName);
        }

        public GroupRecord[] CalculateGroup(string fileName, string field)
        {
            //TODO переделать, что бы формат файла либо передавался, либо из базы брать, так как может быть другой формат
            return _reader.CalculateGroup(fileName, field);
        }

        public List<Dictionary<string, object>> GetData(QueryGetData data)
        {
            //TODO Здесь нужна выборка из базы или кеша для фильтров и т.п.
            return _reader.GetData(data);
        }
        public List<AnswerModify> ModifyData(ListQueryModifyData data)
        {
            return _reader.ModifyData(data);
        }

        public bool SetEncoding(QueryEncodingData data)
        {
            return _reader.SetEncoding(data);
        }
    }
}