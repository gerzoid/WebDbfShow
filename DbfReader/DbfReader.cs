using Contracts.DBF;
using DotNetDBF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbf
{
    public class DbfReader:IDbfReader
    {
        DBFReader? reader;

        public int OpenFile(string filename)
        {
            using (Stream fos = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var reader = new DBFReader(fos))
                {
                    return reader.RecordCount;
                }
            }
        }
    }
}
