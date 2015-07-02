using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace SCZY.Common
{
    public class ViewInStock:InStockDetail
    {
        public double? Width { get; set; }
        public double? Length { get; set; }
        public float? GramWeight { get; set; }
        public float? Reservation { get; set; }
        public string Brand { get; set; }
        public string Texture { get; set; }
        public string Remark { get; set; }
        public string Level { get; set; }
        
    }
}
