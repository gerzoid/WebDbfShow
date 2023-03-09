using Entities.Answer;
using Entities.Query;
using Entities.Todo;

namespace Contracts.DBF
{
    public interface IFileDbReader
    {
        public DbfInfo OpenFile(string filename);
        public List<Dictionary<string, object>> GetData(QueryGetData data);
        public List<AnswerModify> ModifyData(ListQueryModifyData modifyData);
        public bool SetEncoding(QueryEncodingData data);
        public List<RecordStat> CalculateStatistics(string fileName);
        public List<GroupRecord> CalculateGroup(string fileName, string field);
        public int CountUniqueRecordsInColumn(string fileName, string fieldName);
    }
}
