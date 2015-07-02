using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 竞赛
    /// </summary>
    [DataContract]
    public class JingSaiInfo : BaseEntity
    {    
        #region Field Members

        private int m_ID = 0; //竞赛编号          
        private string m_LeiBie; //类别          
        private string m_FanWei; //范围          
        private string m_JingSaiMingCheng; //竞赛名称          
        private string m_JuBanDanWei; //举办单位          
        private string m_ChengBanDanWei; //承办单位          
        private string m_WangZhi; //网址          
        private string m_JianJie; //简介          
        private string m_JingSaiShiJian; //竞赛时间          
        private string m_DaLei; //大类          

        #endregion

        #region Property Members
        
        /// <summary>
        /// 竞赛编号
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
        /// 类别
        /// </summary>
		[DataMember]
        public virtual string LeiBie
        {
            get
            {
                return this.m_LeiBie;
            }
            set
            {
                this.m_LeiBie = value;
            }
        }

        /// <summary>
        /// 范围
        /// </summary>
		[DataMember]
        public virtual string FanWei
        {
            get
            {
                return this.m_FanWei;
            }
            set
            {
                this.m_FanWei = value;
            }
        }

        /// <summary>
        /// 竞赛名称
        /// </summary>
		[DataMember]
        public virtual string JingSaiMingCheng
        {
            get
            {
                return this.m_JingSaiMingCheng;
            }
            set
            {
                this.m_JingSaiMingCheng = value;
            }
        }

        /// <summary>
        /// 举办单位
        /// </summary>
		[DataMember]
        public virtual string JuBanDanWei
        {
            get
            {
                return this.m_JuBanDanWei;
            }
            set
            {
                this.m_JuBanDanWei = value;
            }
        }

        /// <summary>
        /// 承办单位
        /// </summary>
		[DataMember]
        public virtual string ChengBanDanWei
        {
            get
            {
                return this.m_ChengBanDanWei;
            }
            set
            {
                this.m_ChengBanDanWei = value;
            }
        }

        /// <summary>
        /// 网址
        /// </summary>
		[DataMember]
        public virtual string WangZhi
        {
            get
            {
                return this.m_WangZhi;
            }
            set
            {
                this.m_WangZhi = value;
            }
        }

        /// <summary>
        /// 简介
        /// </summary>
		[DataMember]
        public virtual string JianJie
        {
            get
            {
                return this.m_JianJie;
            }
            set
            {
                this.m_JianJie = value;
            }
        }

        /// <summary>
        /// 竞赛时间
        /// </summary>
		[DataMember]
        public virtual string JingSaiShiJian
        {
            get
            {
                return this.m_JingSaiShiJian;
            }
            set
            {
                this.m_JingSaiShiJian = value;
            }
        }

        /// <summary>
        /// 大类
        /// </summary>
		[DataMember]
        public virtual string DaLei
        {
            get
            {
                return this.m_DaLei;
            }
            set
            {
                this.m_DaLei = value;
            }
        }


        #endregion

    }
}