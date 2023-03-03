using Contracts.DBF;
using DbfShowLib.DBF;
using Entities.Answer;
using Entities.Query;
using Entities.Todo;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            info.Columns = new Column[info.CountColumns + 1];
            info.CodePageId = dbf.CodePage.code;
            info.Version = dbf.GetVersion();
            for (int i = 0; i < dbf.CountColumns; i++)
            {
                var col = new Column()
                {
                    Name = dbf.GetColumnName(i),
                    Title = dbf.GetColumnName(i),
                    Type = TypeMapping(dbf.GetColumnType(i)),
                    DbType = dbf.GetColumnType(i),
                    DbSize = dbf.GetColumnSize(i),
                    Size = dbf.GetColumnSize(i) * 10 
                };
                if (col.Size < 12)
                    col.Size = 22;
                if (col.Size > 1000)
                    col.Size = 1000;
                
                info.Columns[i] = col;
            }
            //Добавляем служебное поле, содержащее признак удаленной записи
            info.Columns[info.CountColumns] = new Column() { Name = "_IS_DELETED_", Title = "_IS_DELETED_", Type = "text", Size = 1 , DbType=""};

            dbf.Close();
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

            int startRow = data.PageSize * (data.Page - 1);
            startRow = startRow >= dbf.CountRows ? dbf.CountRows : startRow;

            int endRow = startRow + data.PageSize > dbf.CountRows ? dbf.CountRows : startRow + data.PageSize;

            for (int indexRow = startRow; indexRow < endRow; indexRow++)
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                for (int i = 0; i < dbf.CountColumns - 1; i++)
                {
                    values.Add(dbf.GetColumnName(i), dbf.GetValue(i, indexRow));
                }
                values.Add("_IS_DELETED_", dbf.IsDeleted(indexRow));
                rows.Add(values);
            }
            dbf.Close();
            return rows;
        }
        public List<AnswerModify> ModifyData(ListQueryModifyData data)
        {
            Dbf dbf = new Dbf();
            dbf.OpenFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", data.FileName));
            var result = new List<AnswerModify>();
            string? res = "";

            foreach (QueryModifyData one in data.Data)
            {
                int columnIndex = dbf.GetColumnIndex(one.Field);
                res = dbf.SetValue(columnIndex, one.Row, one.Value).Result;
                result.Add(new AnswerModify() { Result = res != null, Value = res });
            }
            dbf.Close();
            return result;
        }
        public bool SetEncoding(QueryEncodingData data)
        {
            Dbf dbf = new Dbf();
            dbf.OpenFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", data.FileName));
            var result = dbf.SetCodePage(data.CodePageId);
            dbf.Close();
            return result;
        }
        public List<RecordStat> CalculateStatistics(string fileName)
        {
            Dbf dbf = new Dbf();
            dbf.OpenFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fileName));

            var countColumns = dbf.CountColumns;
            var countRows = dbf.CountRows;

            var recordsStats = new List<RecordStat>();

            for (int x = 0; x <= countColumns - 1; x++)
            {
                recordsStats.Add(new RecordStat()
                {
                    name = dbf.GetColumnName(x),
                    avg = 0,
                    type = dbf.GetColumnType(x),
                    sum = 0,
                }); ;
            }
            string value = "";

            for (int x = 0; x <= countColumns - 1; x++)
            {
                var tmpRecordStat = recordsStats[x];
                for (int y = 0; y <= countRows - 1; y++)
                {
                    if ((tmpRecordStat.type == "NUMERIC") || (tmpRecordStat.type == "FLOAT") || (tmpRecordStat.type == "INTEGER") || (tmpRecordStat.type == "CURRENCY") || (tmpRecordStat.type == "DOUBLE"))
                    {
                        value = dbf.GetValue(x, y).Trim();
                        tmpRecordStat.min = tmpRecordStat.min ?? (float)0;
                        tmpRecordStat.max = tmpRecordStat.max ?? (float)0;
                        if (value != "")
                        {
                            tmpRecordStat.sum += Convert.ToSingle(value);
                            if ((float)tmpRecordStat.min == 0)
                                tmpRecordStat.min = Convert.ToSingle(value);
                            if (Convert.ToSingle(value) > (float)tmpRecordStat.max)
                                tmpRecordStat.max = Convert.ToSingle(value);
                            if (Convert.ToSingle(value) < (float)tmpRecordStat.min)
                                tmpRecordStat.min = Convert.ToSingle(value);
                        }
                    }
                    else
                        if ((tmpRecordStat.type == "DATE") || (tmpRecordStat.type == "DATETIME"))
                    {
                        value = dbf.GetValue(x, y).Trim();
                        tmpRecordStat.min = tmpRecordStat.min ?? "";
                        tmpRecordStat.max = tmpRecordStat.max ?? "";
                        if (value != "")
                        {
                            if (((string)tmpRecordStat.min == "") || (tmpRecordStat.min == null))
                                tmpRecordStat.min = value;
                            if (Utils.CompareDate(value, (string)tmpRecordStat.max) == -1)
                                tmpRecordStat.max = value;
                            else
                                if (Utils.CompareDate(value, (string)tmpRecordStat.min) == 1)
                                tmpRecordStat.min = value;
                        }
                    }
                    else
                            if ((tmpRecordStat.type == "CHAR") /*|| (recordsStat[x].type == "MEMO")*/)
                    {
                        value = dbf.GetValue(x, y).Trim();
                        tmpRecordStat.min = tmpRecordStat.min ?? 0;
                        tmpRecordStat.max = tmpRecordStat.max ?? 0;
                        if ((int)tmpRecordStat.min == 0)
                            tmpRecordStat.min = Convert.ToInt32(value.Length);
                        if (Convert.ToInt32(value.Length) > (int)tmpRecordStat.max)
                            tmpRecordStat.max = Convert.ToInt32(value.Length);
                        if (Convert.ToInt32(value.Length) < (int)tmpRecordStat.min)
                            tmpRecordStat.min = Convert.ToInt32(value.Length);
                    }
                }
                //if (Convert.ToInt32(tmpRecordStat.min) == -1)
                //  tmpRecordStat.min = 0;
                if ((tmpRecordStat.type == "NUMERIC") || (tmpRecordStat.type == "FLOAT") || (tmpRecordStat.type == "INTEGER") || (tmpRecordStat.type == "CURRENCY") || (tmpRecordStat.type == "DOUBLE"))
                {
                    tmpRecordStat.max = Math.Round((float)(tmpRecordStat.max ?? 0), 2);
                    tmpRecordStat.min = Math.Round((float)(tmpRecordStat.min ?? 0), 2);
                    tmpRecordStat.sum = Math.Round((float)tmpRecordStat.sum, 2);
                }
                recordsStats[x] = tmpRecordStat;
            }
            dbf.Close();
            return recordsStats;
        }
    }
}
