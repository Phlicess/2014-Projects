using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.FramWork;

namespace SCZY.Entity
{
    public class UserInfo :BaseEntity
    {
        #region Field Members

        private int m_ID = 0; //用户编号
        private string m_Name; //用户名字
        private string m_UserName; //用户登录名
        private string m_PassWord; //密码
        private string m_Role; //角色（管理员，财务人员，仓库录入人员）

        #endregion

        #region Property Members


        /// <summary>
        /// 用户编号
        /// </summary>
        public virtual int ID
        {
            get { return this.m_ID; }
            set { this.m_ID = value; }
        }

        /// <summary>
        /// 用户名字
        /// </summary>
        public virtual string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }

        ///<summary>
        ///用户登录名
        ///</summary>
        public virtual string UserName
        {
            get { return this.m_UserName; }
            set { this.m_UserName = value; }
        }

        ///<summary>
        ///密码
        ///</summary>
        public virtual string PassWord
        {
            get { return this.m_PassWord; }
            set { this.m_PassWord = value; }
        }

        ///<summary>
        ///用户角色
        ///</summary>
        public virtual string Role
        {
            get { return this.m_Role; }
            set { this.m_Role = value; }
        }
     
        //public User(int m_ID, string m_Name, string m_UserName, string m_PassWord, string m_Role)
        //{
        //    ID = m_ID;
        //    Name = m_Name;
        //    UserName = m_UserName;
        //    PassWord = m_PassWord;
        //    Role = m_Role;
        //}
        #endregion
    }
}
