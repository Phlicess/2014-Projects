using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.Entity;
using SCZY.IDAL;

namespace SCZY.DAL.DALSQL
{
    public class DALProduct :FramWork.BaseDALSQL<ProductInfo>, IProduct
    {
          #region 对象实例及构造函数

        public static DALProduct Instance
        {
            get
            {
                return new DALProduct();
            }
        }
        public DALProduct()
            : base("Product", "ID")
        {
        }

        #endregion
    }
}
