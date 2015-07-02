using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 附件
    /// </summary>
    [DataContract]
    public class FuJianInfo : BaseEntity
    {    
        #region Field Members

        private int m_ID = 0; //附件编号          
        private int m_XiangMu_Id = 0; //项目编号          
        private string m_LuJing; //路径          
        private string m_ShuoMing; //说明          

        #endregion

        #region Property Members
        
        /// <summary>
        /// 附件编号
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
        /// 项目编号
        /// </summary>
		[DataMember]
        public virtual int XiangMu_Id
        {
            get
            {
                return this.m_XiangMu_Id;
            }
            set
            {
                this.m_XiangMu_Id = value;
            }
        }

        /// <summary>
        /// 路径
        /// </summary>
		[DataMember]
        public virtual string LuJing
        {
            get
            {
                return this.m_LuJing;
            }
            set
            {
                this.m_LuJing = value;
            }
        }

        /// <summary>
        /// 说明
        /// </summary>
		[DataMember]
        public virtual string ShuoMing
        {
            get
            {
                return this.m_ShuoMing;
            }
            set
            {
                this.m_ShuoMing = value;
            }
        }


        #endregion

    }
}