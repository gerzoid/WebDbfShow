using Contracts.DBF;
using dBASE.NET;
using Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbfFile
{
    public class DbfReader : IDbfReader
    {
        public DbfInfo OpenFile(string filename)
        {
            DbfInfo info = new DbfInfo();
            info.Name = filename;

            Dbf reader = new Dbf();
            reader.Read(filename);


            using (Stream fos = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                dBASE.NET.Dbf reader2 = new dBASE.NET.Dbf();
                reader.Read(fos);

                info.CountColumns = reader.Fields.Count();
                info.Columns = new Column[info.CountColumns];
                info.CountRows = reader.Records.Count();
                for (int i = 0; i < reader.Fields.Count(); i++)
                {
                    var col = new Column() { Name = reader.Fields[i].Name, Type = reader.Fields[i].Type.ToString(), Size = reader.Fields[i].Length, Prec = reader.Fields[i].Precision };
                    info.Columns[i] = col;
                }
                var t = reader.Records;
                return info;
            }
        }
    }
}
