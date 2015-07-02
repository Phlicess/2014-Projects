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
    public class DALOrderMoneyDetails:BaseDALSQL<OrderMoneyDetailsInfo>,IOrderMoneyDetails
    {

        #region 函数构造函数

        public static DALOrderMoneyDetails Instance
        {
            get { return new DALOrderMoneyDetails();}
        }

        public DALOrderMoneyDetails() :base("OrderMoneyDetails","ID") 
        { }

        #endregion

    }
}
