using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class ProviderInfo:BaseEntity
    {
        #region Field Members

        private int m_ID = 0; // 供应者ID
        private string m_Name; //  供应者名称
        private string m_Address;// 供应者住址
        private string m_Phone; //供应者电话
        private string m_Remark; //供应者备注

        #endregion

        #region Property Members


        /// <summary>
        /// 供应商编号
        /// </summary>
        public virtual int ID
        {
            get { return this.m_ID; }
            set { this.m_ID = value; }
        }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }

        /// <summary>
        /// 供应商地址
        /// </summary>
        public virtual string Address
        {
            get { return this.m_Address; }
            set { this.m_Address = value; }
        }

        /// <summary>
        /// 供应商电话
        /// </summary>
        public virtual string Phone
        {
            get { return this.m_Phone; }
            set { this.m_Phone = value; }
        }

        /// <summary>
        /// 供应商备注
        /// </summary>
        public virtual string Remark
        {
            get { return this.m_Remark; }
            set { this.m_Remark = value; }
        }
        #endregion
    }
}
