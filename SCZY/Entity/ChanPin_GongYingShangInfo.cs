using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class ChanPin_GongYingShangInfo:BaseEntity
    {
        #region Field Members

        private decimal m_Product_ID;//产品ID
        private decimal m_Provider_ID;//提供者ID

        #endregion

        #region Property Member

        /// <summary>
        /// 产品ID
        /// </summary>
        public virtual decimal Provider_ID
        {
            get { return m_Provider_ID; }
            set { m_Provider_ID = value; }

        }

        /// <summary>
        /// 提供者ID
        /// </summary>
        public virtual decimal Product_ID
        {
            get { return m_Product_ID; }
            set { m_Product_ID = value; }

        }
        #endregion

    }
}
