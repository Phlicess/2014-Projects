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
    public class DALPaidDetails:BaseDALSQL<PaidDetailsInfo>,IPaidDetails
    {

        #region 函数构造函数

        public static DALPaidDetails Instance
        {
            get { return new DALPaidDetails();}
        }

        public DALPaidDetails() : base("PaidDetails","ID")
        {
            
        }

        #endregion

    }
}
