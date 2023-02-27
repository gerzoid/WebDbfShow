using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Query
{
    public class QueryModifyData
    {
        public string Field { get; set; }
        public int Row { get; set; }
        public string? Old { get; set; }
        [AllowNull]
        public string? Value { get; set; }
    }
}
