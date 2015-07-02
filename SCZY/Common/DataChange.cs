using System.Linq;

namespace SCZY
{
    static class DataChange
    {
        /// <summary>
        ///ID 转换
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>"SC" + 八位订单号</returns>
        static public string IDToOrderID(decimal ID)
        {
            var orderID = ID.ToString();
            for (int i = orderID.Length; i < 8; i++)
            {
                orderID =  "0" + orderID;
            }
            orderID = "SC" + orderID;

            return orderID;
        }

        /// <summary>
        /// 将double?类型数据截取小数点后两位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public double CutDouble(double? data)
        {            
            if (data.ToString().Split('.').Count() > 1 && (data.ToString().Split('.')[1].Length >= 3))
            {
                double ret;
                string str = data.ToString().Split('.')[0] + "." + data.ToString().Split('.')[1].Substring(0, 2);
                
                double.TryParse(str, out ret);
                return ret;
            }
            string str1 = data.ToString();
            double ret1;
            double.TryParse(str1, out ret1);
            return ret1;
        }


        /// <summary>
        /// 取整函数 向上取整
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public int DoubleToInt(double? data)
        {
            int value; 
            int reservation;//余数
            string ty = data.ToString().Split('.')[0];
            int.TryParse(ty, out value);
            reservation = value % 10;
            if (reservation >= 5)
            {
                value = value - reservation + 10;
            }

            return value;
        }

        /// <summary>
        /// 计算产品的吨数  (宽幅*长度 /100/100 * 克重 * 张数 /1000 /1000)
        /// </summary>
        /// <param name="gramWeight">克重（克/平方米）</param>
        /// <param name="width">宽幅（厘米）</param>
        /// <param name="length">长度（厘米）</param>
        /// <param name="sheet">张数（张）</param>
        /// <returns>吨数(吨)</returns>
        static public double GetWeight(string gramWeight,string width,string length,string sheet)
        {
            double value;
            double doubleGramWeight, doublewidth, doublelength, doublesheet;
            double.TryParse(gramWeight, out doubleGramWeight);
            double.TryParse(width, out doublewidth);
            double.TryParse(length, out doublelength);
            double.TryParse(sheet, out doublesheet);
            value = doublewidth * doublelength / 100 / 100 * doubleGramWeight * doublesheet / 1000 / 1000;
            return value;
        }
    }
}
