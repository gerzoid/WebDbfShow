using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Query
{
    public class QueryMiniStat
    {
        public string FileName { get; set; }
        public string? Field { get; set; }
        public string FuncName{ get; set; }
        public string? Value { get; set; }
    }
}
