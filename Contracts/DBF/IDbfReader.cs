using Entities.Dto;

namespace Contracts.DBF
{
    public interface IDbfReader
    {
        public DbfInfo OpenFile(string filename);
    }
}
