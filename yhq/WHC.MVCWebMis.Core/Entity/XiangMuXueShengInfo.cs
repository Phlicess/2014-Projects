using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 项目学生
    /// </summary>
    [DataContract]
    public class XiangMuXueShengInfo : BaseEntity
    {    
        #region Field Members

        private int m_XiangMu_Id = 0; //项目编号          
        private int m_XueSheng_Id = 0; //学生编号          
        private int m_WeiCi = 0; //位次          

        #endregion

        #region Property Members
        
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
        /// 学生编号
        /// </summary>
		[DataMember]
        public virtual int XueSheng_Id
        {
            get
            {
                return this.m_XueSheng_Id;
            }
            set
            {
                this.m_XueSheng_Id = value;
            }
        }

        /// <summary>
        /// 位次
        /// </summary>
		[DataMember]
        public virtual int WeiCi
        {
            get
            {
                return this.m_WeiCi;
            }
            set
            {
                this.m_WeiCi = value;
            }
        }


        #endregion

    }
}