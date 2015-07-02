using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.Entity;
using SCZY.FramWork;
using SCZY.IDAL;

namespace SCZY.DAL.DALSQL
{
    public class DALChanPin_ShengChanRuKu:BaseDALSQL<ChanPin_ShengChanRuKuInfo>,IChanPin_ShengChanRuKu
    {

        #region 函数构造函数

        public static DALChanPin_ShengChanRuKu Instance
        {
            get { return new DALChanPin_ShengChanRuKu();}
        }

        public DALChanPin_ShengChanRuKu() : base("ChanPin_ShengChanRuKU", "Product_ID")
        {
            
        }

        #endregion

    }
}
