using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 学生
    /// </summary>
    [DataContract]
    public class XueShengInfo : BaseEntity
    {    
        #region Field Members

        private int m_ID = 0; //学生编号          
        private int m_XueYuan_Id = 0; //学院编号          
        private string m_XueHao; //学号          
        private string m_XingMing; //姓名          
        private string m_ZhuanYe; //专业          
        private string m_XingBie; //性别          
        private string m_ShouJi; //手机       
        private int? m_User_Id;

        #endregion

        #region Property Members
        
        /// <summary>
        /// 学生编号
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
        /// 学院编号
        /// </summary>
		[DataMember]
        public virtual int XueYuan_Id
        {
            get
            {
                return this.m_XueYuan_Id;
            }
            set
            {
                this.m_XueYuan_Id = value;
            }
        }

        /// <summary>
        /// 学号
        /// </summary>
		[DataMember]
        public virtual string XueHao
        {
            get
            {
                return this.m_XueHao;
            }
            set
            {
                this.m_XueHao = value;
            }
        }

        /// <summary>
        /// 姓名
        /// </summary>
		[DataMember]
        public virtual string XingMing
        {
            get
            {
                return this.m_XingMing;
            }
            set
            {
                this.m_XingMing = value;
            }
        }

        /// <summary>
        /// 专业
        /// </summary>
		[DataMember]
        public virtual string ZhuanYe
        {
            get
            {
                return this.m_ZhuanYe;
            }
            set
            {
                this.m_ZhuanYe = value;
            }
        }

        /// <summary>
        /// 性别
        /// </summary>
		[DataMember]
        public virtual string XingBie
        {
            get
            {
                return this.m_XingBie;
            }
            set
            {
                this.m_XingBie = value;
            }
        }

        /// <summary>
        /// 手机
        /// </summary>
		[DataMember]
        public virtual string ShouJi
        {
            get
            {
                return this.m_ShouJi;
            }
            set
            {
                this.m_ShouJi = value;
            }
        }

        /// <summary>
        /// 学院编号
        /// </summary>
        [DataMember]
        public virtual int? User_Id
        {
            get
            {
                return this.m_User_Id;
            }
            set
            {
                this.m_User_Id = value;
            }
        }

        #endregion

    }
}