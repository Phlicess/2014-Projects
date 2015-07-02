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
    class ProduceOutStock:BaseBLL<ProduceOutStockInfo>
    {

        public IProduceInStock producerOutStockDal;

        public ProduceOutStock():base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            this.producerOutStockDal = (IProduceInStock)base.baseDal;
        }

    }
}
