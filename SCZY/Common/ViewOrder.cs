using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCZY.Common
{
    public class ViewOrder:Order
    {
        public float Paid { get; set; }

        public double Leaving { get; set; }

        public double? Amount { get; set; }
    }
}
