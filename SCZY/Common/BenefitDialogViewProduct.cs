using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCZY.Common
{
    public class BenefitDialogViewProduct:Product
    {
        public DateTime Month { get; set; }
        public double MoneyOfStock { get; set; }
        public double MoneyOfSold { get; set; }
        public double RetainedProfits { get; set; }
    }
}
