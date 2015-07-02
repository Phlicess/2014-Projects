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
    public class DALOrder:BaseDALSQL<OrderInfo>,IOrder
    {

        public static DALOrder Instance
        {
            get
            {
                return new DALOrder();
            }
        }

        public DALOrder() : base("Order", "ID")
        {
            
        }
    }
}
