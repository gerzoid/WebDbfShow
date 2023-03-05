using Entities.Answer;
using Entities.Models;
using Entities.Query;
using Entities.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IShowService
    {
        public Users GetOrCreateUser(string userId);
        public Task<DbfInfo> UploadFile(FileModel file);
        public DbfInfo OpenFile(string fileId);
        public List<RecordStat> CalculateStatistics(string fileName);
        public List<Dictionary<string, object>> GetData(QueryGetData data);
        public List<AnswerModify> ModifyData(ListQueryModifyData data);
        public bool SetEncoding(QueryEncodingData data);
        public List<GroupRecord> CalculateGroup(string fileName, string field);
    }
}
