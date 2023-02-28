using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Todo
{
    public class RecordStat
    {
        public string name { get; set; }
        public string type { get; set; }
        public double max { get; set; }
        public double min { get; set; }
        public string maxDate { get; set; }
        public string minDate { get; set; }
        public double avg { get; set; }
        public double sum { get; set; }
    }

}
