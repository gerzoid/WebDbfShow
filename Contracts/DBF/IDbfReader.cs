using Entities.Dto;

namespace Contracts.DBF
{
    public interface IFileDbReader
    {
        public DbfInfo OpenFile(string filename);

        public List<Dictionary<string, object>> GetData(QueryGetData data);

        public bool ModifyData(ListQueryModifyData modifyData);
    }
}
