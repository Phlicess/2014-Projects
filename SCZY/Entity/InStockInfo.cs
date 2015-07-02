using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class InStockInfo : BaseEntity
    {
        #region Field Member

        private int m_ID;//入库ID
        private int m_Provider_ID;//供应者ID
        private string m_Provider;//提供者
        private DateTime m_Date;//入库时间

        #endregion

        #region Field Members

        /// <summary>
        /// ID
        /// </summary>
        public virtual int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        /// <summary>
        /// 提供者ID
        /// </summary>
        public virtual int Provider_ID
        {
            get { return m_Provider_ID; }
            set { m_Provider_ID = value; }
        }

        /// <summary>
        /// 提供者名称
        /// </summary>
        public virtual string Provider
        {
            get { return m_Provider; }
            set { m_Provider = value; }
        }

        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime Date
        {
            get { return m_Date; }
            set { m_Date = value; }
        }

        #endregion

    }
}
