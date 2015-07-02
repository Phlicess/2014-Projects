using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 项目
    /// </summary>
    [DataContract]
    public class XiangMuInfo : BaseEntity
    {    
        #region Field Members

        private int m_ID = 0; //项目编号          
        private int m_QiYeDaoShi_Id = 0; //企业导师编号          
        private int m_JingSai_Id = 0; //竞赛编号          
        private string m_XiangMuMingCheng; //项目名称          
        private string m_XiangMuJianJie; //项目简介          
        private DateTime m_CanSaiShiJian; //参赛时间          
        private DateTime m_HuoJiangShiJian; //获奖时间          
        private string m_HuoJiangJiBie; //获奖级别          
        private string m_HuoJiangMingCheng; //获奖名称          
        private string m_BeiZhu; //备注          
        private bool m_XueYuanShenPi = false; //学院审批          
        private bool m_XueXiaoShenPi = false; //学校审批          

        #endregion

        #region Property Members
        
        /// <summary>
        /// 项目编号
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
        /// 企业导师编号
        /// </summary>
		[DataMember]
        [DisplayName("企业导师")]
        public virtual int QiYeDaoShi_Id
        {
            get
            {
                return this.m_QiYeDaoShi_Id;
            }
            set
            {
                this.m_QiYeDaoShi_Id = value;
            }
        }

        /// <summary>
        /// 竞赛编号
        /// </summary>
		[DataMember]
        [DisplayName("竞赛编号")]
        public virtual int JingSai_Id
        {
            get
            {
                return this.m_JingSai_Id;
            }
            set
            {
                this.m_JingSai_Id = value;
            }
        }

        /// <summary>
        /// 项目名称
        /// </summary>
		[DataMember]
        [DisplayName("项目名称")]
        public virtual string XiangMuMingCheng
        {
            get
            {
                return this.m_XiangMuMingCheng;
            }
            set
            {
                this.m_XiangMuMingCheng = value;
            }
        }

        /// <summary>
        /// 项目简介
        /// </summary>
		[DataMember]
        [DisplayName("项目简介")]
        public virtual string XiangMuJianJie
        {
            get
            {
                return this.m_XiangMuJianJie;
            }
            set
            {
                this.m_XiangMuJianJie = value;
            }
        }

        /// <summary>
        /// 参赛时间
        /// </summary>
		[DataMember]
        [DisplayName("参赛时间")]
        public virtual DateTime CanSaiShiJian
        {
            get
            {
                return this.m_CanSaiShiJian;
            }
            set
            {
                this.m_CanSaiShiJian = value;
            }
        }

        /// <summary>
        /// 获奖时间
        /// </summary>
		[DataMember]
        [DisplayName("获奖时间")]
        public virtual DateTime HuoJiangShiJian
        {
            get
            {
                return this.m_HuoJiangShiJian;
            }
            set
            {
                this.m_HuoJiangShiJian = value;
            }
        }

        /// <summary>
        /// 获奖级别
        /// </summary>
		[DataMember]
        [DisplayName("获奖级别")]
        public virtual string HuoJiangJiBie
        {
            get
            {
                return this.m_HuoJiangJiBie;
            }
            set
            {
                this.m_HuoJiangJiBie = value;
            }
        }

        /// <summary>
        /// 获奖名称
        /// </summary>
		[DataMember]
        [DisplayName("获奖名称")]
        public virtual string HuoJiangMingCheng
        {
            get
            {
                return this.m_HuoJiangMingCheng;
            }
            set
            {
                this.m_HuoJiangMingCheng = value;
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
		[DataMember]
        [DisplayName("备注")]
        public virtual string BeiZhu
        {
            get
            {
                return this.m_BeiZhu;
            }
            set
            {
                this.m_BeiZhu = value;
            }
        }

        /// <summary>
        /// 学院审批
        /// </summary>
		[DataMember]
        [DisplayName("学院审批")]
        public virtual bool XueYuanShenPi
        {
            get
            {
                return this.m_XueYuanShenPi;
            }
            set
            {
                this.m_XueYuanShenPi = value;
            }
        }

        /// <summary>
        /// 学校审批
        /// </summary>
		[DataMember]
        [DisplayName("学校审批")]
        public virtual bool XueXiaoShenPi
        {
            get
            {
                return this.m_XueXiaoShenPi;
            }
            set
            {
                this.m_XueXiaoShenPi = value;
            }
        }


        #endregion

    }
}