using System;
using System.Runtime.Serialization;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 角色信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class RoleInfo : BaseEntity
    {
        /// <summary>
        /// 管理员名称
        /// </summary>
        public const string AdminName = "管理员";

        private int m_ID = 0; //          
        private string m_Name; //角色名称          
        private string m_Note; //备注          

        #region Property Members

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
        /// 角色名称
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
        /// 备注
        /// </summary>
        [DataMember]
        public virtual string Note
        {
            get
            {
                return this.m_Note;
            }
            set
            {
                this.m_Note = value;
            }
        }


        #endregion
    }
}