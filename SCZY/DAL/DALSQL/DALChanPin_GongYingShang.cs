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
    public class DALChanPin_GongYingShang : BaseDALSQL<ChanPin_GongYingShangInfo>, IChanPin_GongYingShang
    {

        #region  函数构造函数
        public static DALChanPin_GongYingShang Instance
        {
            get
            {
                return new DALChanPin_GongYingShang();
            }
        }

          public DALChanPin_GongYingShang() : base("ChanPin_GongYingShang", "Product_ID") { }

        #endregion



    }
}
