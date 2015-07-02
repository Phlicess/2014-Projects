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
    public class DALProduceInStock:BaseDALSQL<ProduceInStockInfo>,IProduceInStock
    {
        #region 函数构造函数

        public static DALProduceInStock Instance
        {
            get
            {
                return new DALProduceInStock();
            }
        }

        public DALProduceInStock():base("ProduceInStock","ID")
        {           
        }

        #endregion
    }
}
