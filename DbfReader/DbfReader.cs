using Contracts.DBF;
using DbfShowLib.DBF;
using Entities.Dto;
using System.Security.Cryptography;
using System.Text;

namespace DbfFile
{
    public class DbfReader : IFileDbReader
    {
        public DbfInfo OpenFile(string fileName)
        {
            DbfInfo info = new DbfInfo();
            info.Name = Path.GetFileName(fileName);

            Dbf dbf = new Dbf();
            dbf.OpenFile(fileName);

            info.CountColumns = dbf.CountColumns;
            info.CountRows = dbf.CountRows;
            info.Columns = new Column[info.CountColumns];
            info.CodePage = dbf.CodePage.codePage;
            info.Version = dbf.GetVersion();
            for (int i = 0; i < dbf.CountColumns; i++)
            {
                var col = new Column() { Name = dbf.GetColumnName(i), Type = dbf.GetColumnType(i), Size = dbf.GetColumnSize(i) };
                info.Columns[i] = col;
            }
            dbf.Close();
            //var tt = GetRow(1);
            return info;
        }

        public string[] GetRow(int indexRow)
        {            
            Dbf dbf = new Dbf();
            dbf.OpenFile(@"c:\\1\test.dbf");
            string[] row = new string[dbf.CountColumns];
            for (int i = 0; i < dbf.CountColumns; i++)
            {
                row[i] = dbf.GetValue(i, indexRow);
            }
            return row;
        }
    }
}
