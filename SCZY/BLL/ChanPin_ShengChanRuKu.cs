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
    class ChanPin_ShengChanRuKu: BaseBLL<ChanPin_ShengChanRuKuInfo>
    {
        public IChanPin_ShengChanRuKu chanPin_ShengChanRuKuDal;

           public ChanPin_ShengChanRuKu()
              : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            this.chanPin_ShengChanRuKuDal = (IChanPin_ShengChanRuKu)base.baseDal;
        }
    }
}
