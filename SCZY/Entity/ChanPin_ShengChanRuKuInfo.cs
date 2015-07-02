using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class ChanPin_ShengChanRuKuInfo : BaseEntity
    {
        #region Field Members

        private int m_Product_ID;//产品ID
        private int m_ProduceInStock_ID;//产品入库信息

        #endregion

        #region Property Member

        /// <summary>
        /// 产品入库ID
        /// </summary>
        public virtual int ProduceInStock_ID
        {
            get { return m_ProduceInStock_ID; }
            set { m_ProduceInStock_ID = value; }

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
