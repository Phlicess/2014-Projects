using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections.Generic;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 功能菜单
    /// </summary>
    [Serializable]
    [DataContract]
    public class MenuInfo : BaseEntity
    {    
        #region Field Members

        private string m_ID = System.Guid.NewGuid().ToString(); //          
        private string m_PID = "-1"; //父ID          
        private string m_Name; //显示名称          
        private string m_Icon; //图标          
        private string m_Seq; //排序          
        private string m_FunctionId; //功能ID          
        private bool m_Visible = true; //是否可见          
        private string m_WinformType; //Winform窗体类型          
        private string m_Url; //Web界面Url地址          
        private string m_SystemType_ID = ""; //系统编号       
        private string m_WebIcon; //Web界面的菜单图标         

        #endregion

        #region Property Members
        
		[DataMember]
        public virtual string ID
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
        /// 父ID
        /// </summary>
		[DataMember]
        public virtual string PID
        {
            get
            {
                return this.m_PID;
            }
            set
            {
                this.m_PID = value;
            }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
		[DataMember]
        public virtual string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }

        /// <summary>
        /// 图标
        /// </summary>
		[DataMember]
        public virtual string Icon
        {
            get
            {
                return this.m_Icon;
            }
            set
            {
                this.m_Icon = value;
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
		[DataMember]
        public virtual string Seq
        {
            get
            {
                return this.m_Seq;
            }
            set
            {
                this.m_Seq = value;
            }
        }

        /// <summary>
        /// 功能ID
        /// </summary>
		[DataMember]
        public virtual string FunctionId
        {
            get
            {
                return this.m_FunctionId;
            }
            set
            {
                this.m_FunctionId = value;
            }
        }

        /// <summary>
        /// 是否可见
        /// </summary>
		[DataMember]
        public virtual bool Visible
        {
            get
            {
                return this.m_Visible;
            }
            set
            {
                this.m_Visible = value;
            }
        }

        /// <summary>
        /// Winform窗体类型
        /// </summary>
		[DataMember]
        public virtual string WinformType
        {
            get
            {
                return this.m_WinformType;
            }
            set
            {
                this.m_WinformType = value;
            }
        }

        /// <summary>
        /// Web界面Url地址
        /// </summary>
		[DataMember]
        public virtual string Url
        {
            get
            {
                return this.m_Url;
            }
            set
            {
                this.m_Url = value;
            }
        }

        /// <summary>
        /// Web界面的菜单图标
        /// </summary>
        [DataMember]
        public virtual string WebIcon
        {
            get
            {
                return this.m_WebIcon;
            }
            set
            {
                this.m_WebIcon = value;
            }
        }

        /// <summary>
        /// 系统编号
        /// </summary>
        [DataMember]
        public virtual string SystemType_ID
        {
            get
            {
                return this.m_SystemType_ID;
            }
            set
            {
                this.m_SystemType_ID = value;
            }
        }
        #endregion
    }

    [Serializable]
    [DataContract]
    public class MenuNodeInfo : MenuInfo
    {
        private List<MenuNodeInfo> m_Children = new List<MenuNodeInfo>();

        /// <summary>
        /// 子菜单实体类对象集合
        /// </summary>
        [DataMember]
        public List<MenuNodeInfo> Children
        {
            get { return m_Children; }
            set { m_Children = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MenuNodeInfo()
        {
            this.m_Children = new List<MenuNodeInfo>();
        }

        /// <summary>
        /// 参数构造函数
        /// </summary>
        /// <param name="menuInfo">MenuInfo对象</param>
        public MenuNodeInfo(MenuInfo menuInfo)
        {
            base.ID = menuInfo.ID;
            base.Name = menuInfo.Name;
            base.PID = menuInfo.PID;
            base.Seq = menuInfo.Seq;
            base.Visible = menuInfo.Visible;
            base.FunctionId = menuInfo.FunctionId;
            base.Icon = menuInfo.Icon;
            base.WebIcon = menuInfo.WebIcon;
            base.WinformType = menuInfo.WinformType;
            base.Url = menuInfo.Url;
            base.SystemType_ID = menuInfo.SystemType_ID;
        }
    }
}