using System;
using System.Runtime.Serialization;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class UserInfo : SimpleUserInfo
    {
        public const int IdentityLen = 50;

        private int m_PID = -1; //父ID          
        private bool m_IsExpire = false; //是否过期          
        private string m_Title; //职务头衔          
        private string m_IdentityCard; //身份证号码          
        private string m_MobilePhone; //移动电话          
        private string m_OfficePhone; //办公电话          
        private string m_HomePhone; //家庭电话          
        private string m_Email; //邮件地址          
        private string m_Address; //住址          
        private string m_WorkAddr; //工作地址          
        private DateTime m_Birthday; //出生日期          
        private string m_Qq; //QQ                  
        private string m_Note; //          
        private string m_CustomField; //自定义字段     
        private string m_Dept_ID; //默认部门     

        #region Property Members

        /// <summary>
        /// 父ID
        /// </summary>
        [DataMember]
        public virtual int PID
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
        /// 是否过期
        /// </summary>
        [DataMember]
        public virtual bool IsExpire
        {
            get
            {
                return this.m_IsExpire;
            }
            set
            {
                this.m_IsExpire = value;
            }
        }

        /// <summary>
        /// 职务头衔
        /// </summary>
        [DataMember]
        public virtual string Title
        {
            get
            {
                return this.m_Title;
            }
            set
            {
                this.m_Title = value;
            }
        }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [DataMember]
        public virtual string IdentityCard
        {
            get
            {
                return this.m_IdentityCard;
            }
            set
            {
                this.m_IdentityCard = value;
            }
        }

        /// <summary>
        /// 移动电话
        /// </summary>
        [DataMember]
        public virtual string MobilePhone
        {
            get
            {
                return this.m_MobilePhone;
            }
            set
            {
                this.m_MobilePhone = value;
            }
        }

        /// <summary>
        /// 办公电话
        /// </summary>
        [DataMember]
        public virtual string OfficePhone
        {
            get
            {
                return this.m_OfficePhone;
            }
            set
            {
                this.m_OfficePhone = value;
            }
        }

        /// <summary>
        /// 家庭电话
        /// </summary>
        [DataMember]
        public virtual string HomePhone
        {
            get
            {
                return this.m_HomePhone;
            }
            set
            {
                this.m_HomePhone = value;
            }
        }

        /// <summary>
        /// 邮件地址
        /// </summary>
        [DataMember]
        public virtual string Email
        {
            get
            {
                return this.m_Email;
            }
            set
            {
                this.m_Email = value;
            }
        }

        /// <summary>
        /// 住址
        /// </summary>
        [DataMember]
        public virtual string Address
        {
            get
            {
                return this.m_Address;
            }
            set
            {
                this.m_Address = value;
            }
        }

        /// <summary>
        /// 工作地址
        /// </summary>
        [DataMember]
        public virtual string WorkAddr
        {
            get
            {
                return this.m_WorkAddr;
            }
            set
            {
                this.m_WorkAddr = value;
            }
        }

        /// <summary>
        /// 出生日期
        /// </summary>
        [DataMember]
        public virtual DateTime Birthday
        {
            get
            {
                return this.m_Birthday;
            }
            set
            {
                this.m_Birthday = value;
            }
        }

        /// <summary>
        /// QQ
        /// </summary>
        [DataMember]
        public virtual string Qq
        {
            get
            {
                return this.m_Qq;
            }
            set
            {
                this.m_Qq = value;
            }
        }

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

        /// <summary>
        /// 自定义字段
        /// </summary>
        [DataMember]
        public virtual string CustomField
        {
            get
            {
                return this.m_CustomField;
            }
            set
            {
                this.m_CustomField = value;
            }
        }

        /// <summary>
        /// 默认部门
        /// </summary>
        [DataMember]
        public virtual string Dept_ID
        {
            get
            {
                return this.m_Dept_ID;
            }
            set
            {
                this.m_Dept_ID = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// 个人图片分类
    /// </summary>
    [Serializable]
    public enum UserImageType
    {
        个人肖像, 身份证照片1, 身份证照片2, 名片1, 名片2
    }
}