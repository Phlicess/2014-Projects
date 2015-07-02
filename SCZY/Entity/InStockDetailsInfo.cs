using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class InStockDetailsInfo : BaseEntity
    {
        #region Field Members

        private int m_ID;//ID
        private int m_Product_ID;//产品ID
        private int m_InStock_ID;//入库ID
        private float m_Price;//单价
        private float m_Weight;

        #endregion

        #region Property Member

        /// <summary>
        /// ID
        /// </summary>
        public virtual int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        /// <summary>
        /// 产品ID
        /// </summary>
        public virtual int Product_ID
        {
            get { return m_Product_ID; }
            set { m_Product_ID = value; }
        }

        /// <summary>
        ///入库 ID
        /// </summary>
        public virtual int InStock_ID
        {
            get { return m_InStock_ID; }
            set { m_InStock_ID = value; }
        }

        /// <summary>
        /// 单价
        /// </summary>
        public virtual float Price
        {
            get { return m_Price; }
            set { m_Price = value; }
        }

        /// <summary>
        /// 重量
        /// </summary>
        public virtual float Weight
        {
            get { return m_Weight; }
            set { m_Weight = value; }
        }
        #endregion
    }
}
