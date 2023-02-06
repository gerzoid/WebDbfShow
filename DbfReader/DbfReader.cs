using Contracts.DBF;
using DbfShowLib.DBF;
using Entities.Dto;
using System.Security.Cryptography;
using System.Text;

namespace DbfFile
{
    public class DbfReader : IDbfReader
    {
        public DbfInfo OpenFile(string filename)
        {
            DbfInfo info = new DbfInfo();
            info.Name = filename;

            Dbf dbf = new Dbf();
            dbf.OpenFile(@"c:\\1\test.dbf");

            info.CountColumns = dbf.CountColumns;
            info.CountRows = dbf.CountRows;
            info.Columns = new Column[info.CountColumns];

            for (int i = 0; i < dbf.CountColumns; i++)
            {
                var col = new Column() { Name = dbf.GetColumnName(i), Type = dbf.GetColumnType(i), Size = dbf.GetColumnSize(i) };
                info.Columns[i] = col;
            }
            
            return info;
            using (Stream fos = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {

                /*info.CountColumns = reader.Fields.Count();
                info.Columns = new Column[info.CountColumns];
                info.CountRows = reader.Records.Count();
                for (int i = 0; i < reader.Fields.Count(); i++)
                {
                    var col = new Column() { Name = reader.Fields[i].Name, Type = reader.Fields[i].Type.ToString(), Size = reader.Fields[i].Length, Prec = reader.Fields[i].Precision };
                    info.Columns[i] = col;
                }
                var t = reader.Records;
                return info;*/
            }
        }
    }
}
