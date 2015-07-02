using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.Entity;
using SCZY.FramWork;
using SCZY.IDAL;

namespace SCZY.BLL
{
    class ChanPin_ShengChanChuKu : BaseBLL<ChanPin_ShengChanChuKuInfo>
    {
        public IChanPin_ShengChanChuKu chanPin_ShengChanChuKuDal;

           public ChanPin_ShengChanChuKu()
              : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            this.chanPin_ShengChanChuKuDal = (IChanPin_ShengChanChuKu)base.baseDal;
        }
    }
}
