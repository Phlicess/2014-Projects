using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 学院
    /// </summary>
    [DataContract]
    public class XueYuanInfo : BaseEntity
    {    
        #region Field Members

        private int m_ID = 0; //学院编号          
        private string m_XueYuan; //学院          

        #endregion

        #region Property Members
        
        /// <summary>
        /// 学院编号
        /// </summary>
		[DataMember]
        public virtual int ID
        {
            get
            {
                return this.m_ID;
            }
            set
            {
                this.m_ID = value;
            }
        }

        /// <summary>
        /// 学院
        /// </summary>
		[DataMember]
        public virtual string XueYuan
        {
            get
            {
                return this.m_XueYuan;
            }
            set
            {
                this.m_XueYuan = value;
            }
        }


        #endregion

    }
}