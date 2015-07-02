using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class ProduceInStockInfo : BaseEntity
    {

        #region Field Members

        private int m_ID = 0;//产品入库信息
        private DateTime m_Date;//产品入库时间
        private float m_Weight; //产品出库数量
        private string m_Remark;//产品入库备注

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
        /// 产品入库时间
        /// </summary>
        public virtual DateTime Date
        {
            get { return this.m_Date; }
            set { this.Date = value; }
        }

        /// <summary>
        /// 产品入库数量
        /// </summary>
        public float Weight
        {
            get { return this.m_Weight; }
            set { m_Weight = value; }
        }

        public string Remark
        {
            get { return m_Remark; }
            set { m_Remark = value; }
        }
        #endregion
    }
}
