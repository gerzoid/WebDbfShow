using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Todo
{
    public class GroupRecord
    {
        public string name { get; set; }
        public string value { get; set; }
        public int count { get; set; }
        //public double[] summ { get; set; }
        public Dictionary<string, double> summ { get; set; }
        public bool isDigit { get; set; }
    }
}
