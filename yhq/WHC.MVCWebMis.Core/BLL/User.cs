using System;
using System.Collections;

using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using System.Collections.Generic;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.BLL
{
    /// <summary>
    /// 用户信息业务管理类
    /// </summary>
    public class User : BaseBLL<UserInfo>
    {
        private IUser userDal;

        public User() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            this.userDal = (IUser)base.baseDal;
        }

        /// <summary>
        /// 重写删除操作，检查保留管理员用户
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool Delete(object key)
        {
            List<SimpleUserInfo> adminSimpleUsers = new Role().GetAdminSimpleUsers();
            if (adminSimpleUsers.Count == 1)
            {
                SimpleUserInfo info = (SimpleUserInfo)adminSimpleUsers[0];
                if (Convert.ToInt32(key) == info.ID)
                {
                    throw new MyException("管理员角色至少需要包含一个用户！");
                }
            }
            return baseDal.Delete(key);
        }

        /// <summary>
        /// 取消用户的过期设置，变为正常状态
        /// </summary>
        /// <param name="userID"></param>
        internal void CancelExpire(int userID)
        {
            UserInfo info = (UserInfo)this.FindByID(userID.ToString());
            if (info.IsExpire)
            {
                info.IsExpire = false;
                this.Update(info, info.ID.ToString());
            }
        }

        /// <summary>
        /// 获取所有用户的基本信息
        /// </summary>
        /// <returns></returns>
        public List<SimpleUserInfo> GetSimpleUsers()
        {
            return this.userDal.GetSimpleUsers();
        }

        /// <summary>
        /// 获取指定ID字符串的用户基本信息
        /// </summary>
        /// <param name="userIds">ID字符串,逗号分开</param>
        /// <returns></returns>
        public List<SimpleUserInfo> GetSimpleUsers(string userIds)
        {
            return this.userDal.GetSimpleUsers(userIds);
        }

        /// <summary>
        /// 通过用户机构ID方式获取对应的用户基本信息列表
        /// </summary>
        /// <param name="ouID">用户机构ID方式</param>
        /// <returns></returns>
        public List<SimpleUserInfo> GetSimpleUsersByOU(int ouID)
        {
            return this.userDal.GetSimpleUsersByOU(ouID);
        }

        /// <summary>
        /// 通过用户角色ID方式获取对应的用户基本信息列表
        /// </summary>
        /// <param name="roleID">用户角色ID</param>
        /// <returns></returns>
        public List<SimpleUserInfo> GetSimpleUsersByRole(int roleID)
        {
            return this.userDal.GetSimpleUsersByRole(roleID);
        }

        /// <summary>
        /// 通过机构ID获取对应的用户列表
        /// </summary>
        /// <param name="ouID">机构ID</param>
        /// <returns></returns>
        public List<UserInfo> GetUsersByOU(int ouID)
        {
            return this.userDal.GetUsersByOU(ouID);
        }

        /// <summary>
        /// 通过角色ID获取对应的用户列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public List<UserInfo> GetUsersByRole(int roleID)
        {
            return this.userDal.GetUsersByRole(roleID);
        }

        /// <summary>
        /// 通过用户登陆名称获取对应的用户信息
        /// </summary>
        /// <param name="userName">用户登陆名称</param>
        /// <returns></returns>
        public UserInfo GetUserByName(string userName)
        {
            UserInfo info = null;
            if (!string.IsNullOrEmpty(userName))
            {
                string condition = string.Format("Name ='{0}' ", userName);
                info = this.userDal.FindSingle(condition);
            }
            return info;
        }


        public List<FunctionInfo> GetUserFunctions(string identity, string sessionID, string typeID)
        {
            string userName = this.GetUserName(identity, sessionID);
            UserInfo userByName = this.GetUserByName(userName);
            List<FunctionInfo> functionsByUser = null;
            if (userByName != null)
            {
                functionsByUser = new Function().GetFunctionsByUser(userByName.ID, typeID);
            }
            return functionsByUser;
        }

        public string GetUserName(string identity, string sessionID)
        {
            if ((sessionID == null) || (sessionID == string.Empty))
            {
                return "";
            }

            string text = Convert.ToString(Convert.ToChar(1));
            identity = EncryptHelper.UnEncryptStr(identity, sessionID);
            int length = identity.IndexOf(text);
            return identity.Substring(0, length);
        }

        public override bool Insert(UserInfo obj)
        {
            UserInfo info = (UserInfo)obj;
            info.Password = EncryptHelper.ComputeHash(info.Password, info.Name.ToLower());
            return base.Insert(obj);
        }

        public bool ModifyPassword(string userName, string userPassword)
        {
            return ModifyPassword(userName, userPassword, "", "", "");
        }

        public bool ModifyPassword(string userName, string userPassword, string systemType, string ip, string macAddr)
        {
            bool result = false;
            UserInfo userByName = this.GetUserByName(userName);
            if (userByName != null)
            {
                userPassword = EncryptHelper.ComputeHash(userPassword, userName.ToLower());
                userByName.Password = userPassword;

                result = userDal.Update(userByName, userByName.ID.ToString());
                if (result)
                {
                    //记录用户修改密码日志
                    BLLFactory<LoginLog>.Instance.AddLoginLog(userByName, systemType, ip, macAddr, "用户修改密码");
                }
            }
            return result;
        }

        public override bool Update(UserInfo obj, object primaryKeyValue)
        {
            UserInfo info = (UserInfo)obj;
            if (info.Password.Length < 50)
            {
                info.Password = EncryptHelper.ComputeHash(info.Password, info.Name.ToLower());
            }
            return this.userDal.Update(info, primaryKeyValue);
        }

        /// <summary>
        /// 判断用户是否在指定的角色名称中
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public bool UserInRole(string userName, string roleName)
        {
            UserInfo userByName = this.GetUserByName(userName);
            Role role = new Role();
            foreach (RoleInfo info in role.GetRolesByUser(userByName.ID))
            {
                if (info.Name == roleName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据用户名、密码验证用户身份有效性
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="systemType">系统类型ID</param>
        /// <returns></returns>
        public string VerifyUser(string userName, string userPassword, string systemType)
        {
            return VerifyUser(userName, userPassword, systemType, "", "");
        }

        /// <summary>
        /// 根据用户名、密码验证用户身份有效性
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="systemType">系统类型ID</param>
        /// <param name="ip">IP地址</param>
        /// <param name="macAddr">Mac网卡地址</param>
        /// <returns></returns>
        public string VerifyUser(string userName, string userPassword, string systemType, string ip, string macAddr)
        {
            if (string.IsNullOrEmpty(systemType))
            {
                return "";
            }
            string identity = "";
            UserInfo userByName = this.GetUserByName(userName);
            if ((userByName != null) && !userByName.IsExpire)
            {
                userPassword = EncryptHelper.ComputeHash(userPassword, userName.ToLower());
                if (userPassword == userByName.Password)
                {
                    identity = EncryptHelper.EncryptStr(userName + Convert.ToString(Convert.ToChar(1)) + userPassword, systemType);

                    //记录用户登录日志
                    BLLFactory<LoginLog>.Instance.AddLoginLog(userByName, systemType, ip, macAddr, "用户登录");
                }
            }
            return identity;
        }

        /// <summary>
        /// 根据个人图片枚举类型获取图片数据
        /// </summary>
        /// <param name="imagetype">图片枚举类型</param>
        /// <returns></returns>
        public byte[] GetPersonImageBytes(UserImageType imagetype, int userId)
        {
            IUser dal = baseDal as IUser;
            return dal.GetPersonImageBytes(imagetype, userId);
        }

        /// <summary>
        /// 更新个人相关图片数据
        /// </summary>
        /// <param name="imagetype">图片类型</param>
        /// <param name="userId">用户ID</param>
        /// <param name="imageBytes">图片字节数组</param>
        /// <returns></returns>
        public bool UpdatePersonImageBytes(UserImageType imagetype, int userId, byte[] imageBytes)
        {
            IUser dal = baseDal as IUser;
            return dal.UpdatePersonImageBytes(imagetype, userId, imageBytes);
        }
    }
}