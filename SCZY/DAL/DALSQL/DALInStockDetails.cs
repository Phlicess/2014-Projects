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
    public class DALInStockDetails : BaseDALSQL<InStockDetailsInfo>, IInStockDetails
    {

        #region 函数构造函数

        public static DALInStockDetails Instance
        {
            get
            {
                return new DALInStockDetails();
            }
        }

        public DALInStockDetails() : base("InStockDetails", "ID")
        {
            
        }

        #endregion

    }
}
