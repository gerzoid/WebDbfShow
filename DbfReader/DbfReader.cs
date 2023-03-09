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
            DbfInfo info = new();
            info.Name = Path.GetFileName(fileName);

            Dbf dbf = new();
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
            var res = "";
            var result = new List<AnswerModify>();
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
            Dbf dbf = new();
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
                        tmpRecordStat.min ??= (float)0;
                        tmpRecordStat.max ??= (float)0;
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
                        tmpRecordStat.min ??= "";
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

        public List<GroupRecord>? CalculateGroup(string fileName, string field)
        {
            Dbf dbf = new Dbf();
            dbf.OpenFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fileName));

            List<GroupRecord> records;

            int columnPosition = dbf.GetColumnIndex(field);
            if (columnPosition < 0)
                return null;

            //Сортируем dbf, для дальнейшей группировки
            dbf.SortValue(field, DbfShowLib.Sorting.SortingType.ASC);            
            
            records = new List<GroupRecord>();
            for (int i = 0; i< dbf.CountRows; i++)            
                records.Add(new GroupRecord() { name = field, count= 0});
            

            Dictionary<string, double> digitColumns_ = new System.Collections.Generic.Dictionary<string, double>();

            for (int y = 0; y <= dbf.CountColumns - 1; y++)
            { 
                string columnType = dbf.GetColumnType(y);
                if ((columnType == "NUMERIC") || (columnType == "FLOAT") || (columnType == "INTEGER") || (columnType == "DOUBLE") || (columnType == "CURRENCY"))

                    digitColumns_[dbf.GetColumnName(y)] = 0;                
            }

            string temp = dbf.GetValueWithFilter(columnPosition, 0);
            string tempOld = temp;
            int count = 0, count2 = 0; ;

            //records[0].summ = new double[countNumericColumns];
            records[0].summ = digitColumns_.ToDictionary(entry => entry.Key, entry => entry.Value);

            var digitColumnNames = digitColumns_.Keys.ToArray();
            var digitColumnIndex = digitColumns_.Keys.Select(x => dbf.GetColumnIndex(x)).ToArray();

            for (int x = 0; x < dbf.CountRows; x++)
            {
                temp = dbf.GetValueWithFilter(columnPosition, x);
                if (tempOld != temp)
                {
                    records[count].value = tempOld;
                    count2 = 0;
                    count++;
                    tempOld = temp;
                    records[count].summ = digitColumns_.ToDictionary(entry => entry.Key, entry => entry.Value);
                }
                count2++;
                records[count].count++;
                double doub = 0;
                for (int t = 0; t < digitColumnNames.Count(); t++)
                {
                    string TMP = dbf.GetValueWithFilter(digitColumnIndex[t], x);
                    if (TMP == "") TMP = "0";
                    //records[count].summ[digitColumnNames[t]] += Convert.ToDouble(TMP);

                    records[count].summ[digitColumnNames[t]] += double.TryParse(TMP, out doub) ? doub : 0;
                }     
            }
            records[count].value = temp;
            count++;
            records.RemoveRange(count, records.Count - count);
            dbf.Close();
            return records;
        }

        public int? CountUniqueRecordsInColumn(string fileName, string fieldName) 
        {
            Dbf dbf = new Dbf();
            dbf.OpenFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fileName));

            /*int[] stat;
            if (dbf.filteredRecords != null) {
                stat = new int[filteredRecords.Length];
                Array.Copy(filteredRecords, stat, filteredRecords.Length);
            }
            else
                stat = null;*/

            int columnPosition = dbf.GetColumnIndex(fieldName);
            if (columnPosition < 0) return null;
            dbf.SortValue(fieldName, DbfShowLib.Sorting.SortingType.ASC);
            string temp = "";
            string tempOld = "";
            int count = 0;
            for (int x = 0; x < dbf.CountRows; x++) {
                temp = dbf.GetValueWithFilter(columnPosition, x);
                if (tempOld != temp) {
                    count++;
                    tempOld = temp;
                }
            }
            if (tempOld != temp)
                count++;
            dbf.Close();
            return count;
        }

        public int? CountValueRecordsInColumn(string fileName, string fieldName, string value)
        {
            Dbf dbf = new Dbf();
            dbf.OpenFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fileName));

            /*int[] stat;
            if (dbf.filteredRecords != null) {
                stat = new int[filteredRecords.Length];
                Array.Copy(filteredRecords, stat, filteredRecords.Length);
            }
            else
                stat = null;*/

            int columnPosition = dbf.GetColumnIndex(fieldName);
            if (columnPosition < 0) return null;

            int count = 0;
            for (int x = 0; x < dbf.CountRows; x++)
            {
                if (dbf.GetValueWithFilter(columnPosition, x) == value)
                    count++;
            }
            dbf.Close();
            return count;
        }

    }
}
