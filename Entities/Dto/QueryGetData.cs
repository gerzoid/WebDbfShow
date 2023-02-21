using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class QueryGetData
    {
        public string FileName { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
