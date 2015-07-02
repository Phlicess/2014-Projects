using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.Entity;
using SCZY.IDAL;

namespace SCZY.DAL.DALSQL
{
    public class DALProduceOutStock : FramWork.BaseDALSQL<ProduceOutStockInfo>, IProduceOutStock
    {

        #region 函数构造函数

       public static DALProduceOutStock Instance
        {
            get
            {
                return new DALProduceOutStock();
            }
        }
       public DALProduceOutStock()
           : base("DALProduceOutStock", "ID")
        {
        }
        #endregion

    }
}
