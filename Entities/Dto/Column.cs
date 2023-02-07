using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
    public class Column
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Size { get; set; }
        public int Prec { get; set; }
    }
}
