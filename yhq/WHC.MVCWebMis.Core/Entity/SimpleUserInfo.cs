using System;
using System.Runtime.Serialization;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 可用于传递的用户简单信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class SimpleUserInfo : BaseEntity
    {
        private int m_ID = 0;
        private string m_Name = "";//用户名 
        private string m_Password = "";//用户密码
        private string m_FullName = "";//用户全名

        [DataMember]
        public virtual int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public virtual string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        [DataMember]
        public virtual string Password
        {
            get { return this.m_Password; }
            set { this.m_Password = value; }
        }

        /// <summary>
        /// 用户全名
        /// </summary>
        [DataMember]
        public virtual string FullName
        {
            get { return this.m_FullName; }
            set { this.m_FullName = value; }
        }  
    }
}