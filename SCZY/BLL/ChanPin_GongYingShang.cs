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
    class ChanPin_GongYingShang: BaseBLL<ChanPin_GongYingShangInfo>
    {
        public IChanPin_GongYingShang chanPin_GongYingShangDal;

           public ChanPin_GongYingShang()
              : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            this.chanPin_GongYingShangDal = (IChanPin_GongYingShang)base.baseDal;
        }


        /// <summary>
        /// 根据供应商ID 查找
        /// </summary>
        /// <param name="providerId">供应商ID</param>
        /// <returns></returns>
        public ChanPin_GongYingShangInfo FindChanPin_GongYingShangInfoByProvider_ID(string providerId)
        {

            ChanPin_GongYingShangInfo chanPin_GongYingShangInfo = new ChanPin_GongYingShangInfo();

            if (!string.IsNullOrEmpty(providerId))
            {
                string condition = string.Format("Provider_ID = '{0}'", providerId);

                chanPin_GongYingShangInfo = this.chanPin_GongYingShangDal.FindSingle(condition);
            }

            return chanPin_GongYingShangInfo;
        } 


    }
}
