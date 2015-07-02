using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCZY.Common
{
    static class DictionaryClass
    {
        /// <summary>
        /// 计算单位字典
        /// </summary>
        static Dictionary<string, string> unitDic = new Dictionary<string, string>()
            {
                {"张", "张"},
                {"令", "令"},
                {"吨", "吨"}
            };

        /// <summary>
        /// 获取计算单位字典
        /// </summary>
        /// <returns></returns>
        static public Dictionary<string, string> GetUnitDic()
        {
            return unitDic;
        }


        /// <summary>
        /// 送货方式下拉列表数据字典
        /// </summary>
        static Dictionary<string, string> disDic = new Dictionary<string, string>()
            {
                {"上门自提", "上门自提"},
                {"委托物流", "委托物流"}
            };

        /// <summary>
        /// 获取送货方式下拉列表数据字典
        /// </summary>
        /// <returns></returns>
        static public Dictionary<string, string> GetDisDic()
        {
            return disDic;
        }
    }
}
