using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class ProduceOutStockInfo:BaseEntity
    {
        #region Field Members

        private int m_ID = 0; //ID
        private DateTime m_Date; //产品出库信息
        private float m_Weight;// 产品数量
        private string m_Remark;//产品注释

        #endregion

        #region Property Member

        /// <summary>
        /// 产品ID
        /// </summary>
        public virtual int ID
        {
            get { return this.m_ID; }
            set { this.m_ID = value; }
        }

        /// <summary>
        /// 产品出库时间
        /// </summary>
        public virtual DateTime Date
        {
            get { return this.m_Date; }
            set { this.m_Date = value; }
        }

        /// <summary>
        /// 产品数量
        /// </summary>
        public virtual float Weight
        {
            get { return this.m_Weight; }
            set { this.m_Weight = value; }
        }

        /// <summary>
        ///  
        /// </summary>
        public virtual string Remark
        {
            get { return this.m_Remark; }
            set { this.m_Remark = value; }
        }
        #endregion
    }
}
