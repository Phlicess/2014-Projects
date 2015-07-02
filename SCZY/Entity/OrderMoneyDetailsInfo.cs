using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class OrderMoneyDetailsInfo : BaseEntity
    {
        #region Field Members

        private int m_ID;//ID
        private int m_Order_ID; //订单ID
        private DateTime m_Date;//时间
        private float m_Paid;//款额
        private string m_Remark;//备注

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
        /// 入库记录ID
        /// </summary>
        public virtual int Order_ID
        {
            get { return this.m_Order_ID; }
            set { m_Order_ID = value; }
        }

        /// <summary>
        /// 日期
        /// </summary>
        public virtual DateTime Date
        {
            get { return this.m_Date; }
            set { m_Date = value; }
        }

        /// <summary>
        /// 款额
        /// </summary>
        public virtual float Paid
        {
            get { return this.m_Paid; }
            set { m_Paid = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark
        {
            get { return this.m_Remark; }
            set { m_Remark = value; }
        }
        #endregion
    }
}
