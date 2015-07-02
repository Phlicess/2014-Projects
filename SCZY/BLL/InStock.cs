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
    class InStock: BaseBLL<InStockInfo>
    {
        public IInStock inStockDal;

           public InStock()
              : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            this.inStockDal = (IInStock)base.baseDal;
        }
    }
}
