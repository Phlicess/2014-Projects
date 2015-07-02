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
    class PaidDetails: BaseBLL<PaidDetailsInfo>
    {
        public IPaidDetails paidDetailsDal;

           public PaidDetails()
              : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            this.paidDetailsDal = (IPaidDetails)base.baseDal;
        }
    }
}
