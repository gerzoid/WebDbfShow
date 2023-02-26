using Entities.Todo;

namespace Contracts.DBF
{
    public interface IFileDbReader
    {
        public DbfInfo OpenFile(string filename);
        public List<Dictionary<string, object>> GetData(QueryGetData data);
        public List<AnswerModify> ModifyData(ListQueryModifyData modifyData);
        public bool SetEncoding(QueryEncodingData data);
    }
}
