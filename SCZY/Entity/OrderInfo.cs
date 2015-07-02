using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class OrderInfo : BaseEntity
    {
        #region Field Members

        private int m_ID; //订单ID
        private int m_Consumer_ID;//客户ID
        private int m_User_ID;//客户ID
        private string m_ConsumerName;//客户名称
        private float m_Weight;//
        private DateTime m_OrderDate;//订单时间
        private DateTime m_TakeDate;//时间
        private DateTime m_DeliveryDate;//订单交付时间
        private string m_Dispatch;//
        private bool m_Urgent;//
        private string m_Phone;//

        #endregion

        #region Property Member

        /// <summary>
        /// 订单ID
        /// </summary>
        public virtual int ID
        {
            get { return this.m_ID; }
            set { m_ID = value; }       
        }

        /// <summary>
        /// 客户ID
        /// </summary>
        public virtual int Consumer_ID
        {
            get { return this.m_Consumer_ID; }
            set { m_Consumer_ID = value; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual int User_ID
        {
            get { return this.m_User_ID; }
            set { m_User_ID = value; }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public virtual string ConsumerName
        {
            get { return this.m_ConsumerName; }
            set { m_ConsumerName = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual float Weight
        {
            get { return this.m_Weight; }
            set { m_Weight = value; }
        }

        /// <summary>
        /// 订单日期
        /// </summary>
        public virtual DateTime OrderDate
        {
            get { return this.m_OrderDate; }
            set { m_OrderDate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime TakeDate
        {
            get { return this.m_TakeDate; }
            set { m_TakeDate = value; }
        }

        /// <summary>
        /// 订单交付时间
        /// </summary>
        public virtual DateTime DeliveryDate
        {
            get { return this.m_DeliveryDate; }
            set { m_DeliveryDate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Dispatch
        {
            get { return this.m_Dispatch; }
            set { m_Dispatch = value; }
        }

        /// <summary>
        /// 代理
        /// </summary>
        public virtual bool Urgent
        {
            get { return this.m_Urgent; }
            set { m_Urgent = value; }
        }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Phone
        {
            get { return this.m_Phone; }
            set { m_Phone = value; }
        }
        #endregion

    }
}
