using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCZY.Common
{
    public class ViewProduct : Product
    {
        public string Unit { get; set; }
        public Nullable<double> Sheet { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Count { get; set; }
        public Nullable<double> Money { get; set; }
        //public Nullable<double> Count { get; set; }
    }
}
