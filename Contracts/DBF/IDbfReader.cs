using Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DBF
{
    public interface IDbfReader
    {
        public DbfInfo OpenFile(string filename);
    }
}
