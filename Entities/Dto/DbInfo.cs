using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class DbInfo
    {
        public string? FileName { get; set; }//Имя  файла на сервере
        public int CountColumns { get; set; }
        public int CountRows { get; set; }
        public string? CodePage { get; set; }
        public string? Version { get; set; }
        public Column[]? Columns { get; set; }
    }
}
