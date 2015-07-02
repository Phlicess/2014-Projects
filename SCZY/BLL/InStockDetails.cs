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
    class InStockDetails: BaseBLL<InStockDetailsInfo>
    {
        public IInStockDetails inStockDetailsDal;

           public InStockDetails()
              : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            this.inStockDetailsDal = (IInStockDetails)base.baseDal;
        }
    }
}
