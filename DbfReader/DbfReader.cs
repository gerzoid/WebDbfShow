using Contracts.DBF;
using DbfShowLib.DBF;
using Entities.Dto;
using Entities.Models;
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
            info.Columns = new ColumnHandsonTable[info.CountColumns];
            info.CodePage = dbf.CodePage.codePage;
            info.Version = dbf.GetVersion();
            for (int i = 0; i < dbf.CountColumns; i++)
            {

                var col = new ColumnHandsonTable() { Data = dbf.GetColumnName(i), Title= dbf.GetColumnName(i), Type = TypeMapping(dbf.GetColumnType(i)), Width = dbf.GetColumnSize(i)*10 };
                //var col = new Column() { Name = dbf.GetColumnName(i), Type = dbf.GetColumnType(i), Size = dbf.GetColumnSize(i) };
                info.Columns[i] = col;
            }
            dbf.Close();
            //var tt = GetRow(1);
            return info;
        }
        public string TypeMapping(string type)
        {
            switch (type)
            {
                /* case "DATETIME": return "date";
                 case "DATE": return "date";
                 case "BOOL": return "text";
                 case "CHAR": return "text";
                 case "NUMERIC": return "numeric";
                 case "INTEGER": return "numeric";
                 case "CURRENCY": return "numeric";
                 case "DOUBLE": return "numeric";
                 case "FLOAT": return "numeric";*/
                default:
                    return "text";
            }
        }

        public IEnumerable<string[]> GetData(QueryGetData data)
        {
            Dbf dbf = new Dbf();
            
            dbf.OpenFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", data.FileName));

            IEnumerable<string[]> rows = new List<string[]>();
            for (int indexRow = 0; indexRow < data.PageSize; indexRow++)
            {
                string[] row = new string[dbf.CountColumns];
                for (int i = 0; i < dbf.CountColumns; i++)
                {
                    row[i] = dbf.GetValue(i, indexRow);
                }
                ((List<string[]>)rows).Add(row);
            }
            return rows;
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
