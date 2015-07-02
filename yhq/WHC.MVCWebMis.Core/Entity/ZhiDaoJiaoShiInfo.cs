using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 指导教师
    /// </summary>
    [DataContract]
    public class ZhiDaoJiaoShiInfo : BaseEntity
    {    
        #region Field Members

        private int m_ID = 0; //指导教师编号          
        private int m_XueYuan_Id = 0; //学院编号          
        private string m_XingMing; //姓名          
        private string m_ZhuanYe; //专业          
        private string m_ZhiCheng; //职称          
        private string m_ShenFenZhengHao; //身份证号          

        #endregion

        #region Property Members
        
        /// <summary>
        /// 指导教师编号
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
        /// 职称
        /// </summary>
		[DataMember]
        public virtual string ZhiCheng
        {
            get
            {
                return this.m_ZhiCheng;
            }
            set
            {
                this.m_ZhiCheng = value;
            }
        }

        /// <summary>
        /// 身份证号
        /// </summary>
		[DataMember]
        public virtual string ShenFenZhengHao
        {
            get
            {
                return this.m_ShenFenZhengHao;
            }
            set
            {
                this.m_ShenFenZhengHao = value;
            }
        }


        #endregion

    }
}