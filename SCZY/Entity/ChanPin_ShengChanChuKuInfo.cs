using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class ChanPin_ShengChanChuKuInfo : BaseEntity
    {
        #region Field Members

        private int m_Product_ID;//产品ID
        private int m_ProduceOutStock_ID;//产品入库信息

        #endregion

        #region Property Member

        /// <summary>
        /// 产品出库ID
        /// </summary>
        public virtual int ProduceOutStock_ID
        {
            get { return m_ProduceOutStock_ID; }
            set { m_ProduceOutStock_ID = value; }

        }

        /// <summary>
        /// 产品ID
        /// </summary>
        public virtual int Product_ID
        {
            get { return m_Product_ID; }
            set { m_Product_ID = value; }

        }
        #endregion
    }
}
