using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.BLL;
using SCZY.Entity;
using SCZY.FramWork;
using SCZY.IDAL;

namespace SCZY.DAL.DALSQL
{
    public class DALInStock:BaseDALSQL<InStockInfo>,IInStock
    {

        #region 函数构造函数

        public static DALInStock Instance
        {
            get
            {
                return new DALInStock();
            }
        }

        public  DALInStock() : base("InStock", "ID")
        {    
        }

        #endregion

    }
}
