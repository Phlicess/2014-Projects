using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class PaidDetailsInfo : BaseEntity
    {
        #region Field Members

        private int m_ID = 0; //支付明细ID
        private int m_InStock_ID; //入库ID
        private DateTime m_Date;//时间
        private float m_Paid;//款额
        private float m_Remark;//备注

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
        /// 入库ID
        /// </summary>
        public virtual int InStock_ID
        {
            get { return this.m_InStock_ID; }
            set { m_InStock_ID = value; }
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
        public virtual float Remark
        {
            get { return this.m_Remark; }
            set { m_Remark = value; }
        }
        #endregion
    }
}
