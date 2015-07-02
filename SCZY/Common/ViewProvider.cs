using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCZY.Common
{
    /// <summary>
    /// 这个类是为了在管理员 “产品管理” 功能里面显示出产品的供应商的报价信息
    /// </summary>
    class ViewProvider : Provider
    {
        public double? Price { get; set; }
    }
}
