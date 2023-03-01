using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbfFile
{
    public static class Utils
    {
        public static int CompareDate(string x, string y)
        {
            if ((y == null) && (x == null)) return 0;
            if ((y == null) && (x != null)) return -1;

            if ((y == "") && (x == "")) return 0;
            if ((y == "") && (x != "")) return -1;

            if ((y.Length < 8) && (x.Length < 8)) return 0;

            DateTime d1 = DateTime.Parse(x);
            DateTime d2 = DateTime.Parse(y);
            if (d1 < d2)
                return 1;
            else
                if (d1 > d2)
                return -1;
            else
                return 0;
        }

    }
}
