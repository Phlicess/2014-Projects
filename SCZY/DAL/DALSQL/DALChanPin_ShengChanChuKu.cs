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
    public class DALChanPin_ShengChanChuKu:BaseDALSQL<ChanPin_ShengChanChuKuInfo>,IChanPin_ShengChanChuKu
    {

        public static DALChanPin_ShengChanChuKu Instance
        {
            get
            {
                return new DALChanPin_ShengChanChuKu();
            }
        }

        public DALChanPin_ShengChanChuKu() : base("ChanPin_ShengChanChuKu", "Product_ID") { }

    }
}
