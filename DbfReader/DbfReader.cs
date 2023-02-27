using Contracts.DBF;
using DbfShowLib.DBF;
using Entities.Answer;
using Entities.Query;
using Entities.Todo;
using System.Collections.Generic;
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
            info.Columns = new Column[info.CountColumns+1];
            info.CodePageId = dbf.CodePage.code;
            info.Version = dbf.GetVersion();
            for (int i = 0; i < dbf.CountColumns; i++)
            {

                var col = new Column() { Name = dbf.GetColumnName(i), Title= dbf.GetColumnName(i), Type = TypeMapping(dbf.GetColumnType(i)), Size = dbf.GetColumnSize(i)*10 };
                //var col = new Column() { Name = dbf.GetColumnName(i), Type = dbf.GetColumnType(i), Size = dbf.GetColumnSize(i) };
                info.Columns[i] = col;
            }
            //Добавляем служебное поле, содержащее признак удаленной записи
            info.Columns[info.CountColumns] = new Column() { Name = "_IS_DELETED_", Title = "_IS_DELETED_", Type = "text", Size = 1 };

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
        public List<Dictionary<string, object>> GetData(QueryGetData data)
        {
            Dbf dbf = new Dbf();

            dbf.OpenFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", data.FileName));

            var rows = new List<Dictionary<string, object>>();

            int startRow = data.PageSize * (data.Page-1);
            startRow = startRow>= dbf.CountRows ? dbf.CountRows : startRow;

            int endRow =  startRow+data.PageSize > dbf.CountRows ? dbf.CountRows : startRow + data.PageSize;
                        
            for (int indexRow = startRow; indexRow < endRow; indexRow++)
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                for (int i = 0; i < dbf.CountColumns-1; i++)
                {
                    values.Add(dbf.GetColumnName(i), dbf.GetValue(i, indexRow));
                }
                values.Add("_IS_DELETED_", dbf.IsDeleted(indexRow));
                rows.Add(values);
            }
            return rows;
        }
        public List<AnswerModify> ModifyData(ListQueryModifyData data)
        {
            Dbf dbf = new Dbf();
            dbf.OpenFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", data.FileName));
            var result = new List<AnswerModify>();
            string? res = "";

            foreach(QueryModifyData one in data.Data)
            {
                int columnIndex = dbf.GetColumnIndex(one.Field);
                res = dbf.SetValue(columnIndex, one.Row, one.Value);
                result.Add(new AnswerModify() { Result = res!=null, Value =res });
            }
            
            return result;
        }

        public bool SetEncoding(QueryEncodingData data)
        {
            Dbf dbf = new Dbf();
            dbf.OpenFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", data.FileName));
            return dbf.SetCodePage(data.CodePageId);
        }
    }
}
