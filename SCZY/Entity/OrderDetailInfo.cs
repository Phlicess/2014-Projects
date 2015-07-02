using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class OrderDetailInfo:BaseEntity
    {
        #region Field Members

        private int m_ID;//ID
        private int m_Order_ID;//订单ID
        private int m_Product_ID;//产品ID
        private float m_Price;//单价
        private float m_Weight;//数量

        #endregion

        #region Property Member

        /// <summary>
        /// ID
        /// </summary>
        public virtual int ID
        {
            get { return this.m_ID; }
            set { m_ID = value; }
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public virtual int Order_ID
        {
            get { return this.m_Order_ID; }
            set { m_Order_ID = value; }
        }

        /// <summary>
        /// 产品ID
        /// </summary>
        public virtual int Product_ID
        {
            get { return this.m_Product_ID; }
            set { m_Product_ID = value; }
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual float Price
        {
            get { return this.m_Price; }
            set { m_Price = value; }
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual float Weight
        {
            get { return this.m_Weight; }
            set { m_Weight = value; }
        }

        #endregion
    }
}
