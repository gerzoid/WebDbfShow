using Entities.Dto;
using Entities.Models;

namespace Contracts.DBF
{
    public interface IFileDbReader
    {
        public DbfInfo OpenFile(string filename);

        public IEnumerable<Dictionary<string, object>> GetData(QueryGetData data);

    }
}
