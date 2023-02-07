using Entities.Dto;

namespace Contracts.DBF
{
    public interface IFileDbReader
    {
        public DbfInfo OpenFile(string filename);
    }
}
