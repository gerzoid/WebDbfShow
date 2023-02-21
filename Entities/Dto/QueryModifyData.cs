using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class QueryModifyData
    {
        public string Field { get; set; }
        public int Row { get; set; }
        public string Old { get; set; }        
        public string Value { get; set; }
    }
}
