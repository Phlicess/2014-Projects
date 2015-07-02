using System;
using System.Collections.Generic;
using System.Collections;

using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using WHC.Framework.ControlUtil;
using WHC.Framework.Commons;

namespace WHC.MVCWebMis.BLL
{
    /// <summary>
    /// 部门机构信息
    /// </summary>
    public class OU : BaseBLL<OUInfo>
	{
		private IOU ouDal;

        /// <summary>
        /// 构造函数
        /// </summary>
		public OU() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            this.ouDal = baseDal as IOU;
		}

        /// <summary>
        /// 为机构添加相关用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="ouID">机构ID</param>
		public void AddUser(int userID, int ouID)
		{
            if (this.OUInRole(ouID, RoleInfo.AdminName))
			{
				new User().CancelExpire(userID);
			}

			this.ouDal.AddUser(userID, ouID);
		}

        /// <summary>
        /// 根据角色ID获取对应的机构列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
		public List<OUInfo> GetOUsByRole(int roleID)
		{
			return this.ouDal.GetOUsByRole(roleID);
		}

        /// <summary>
        /// 获取指定用户的机构列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public List<OUInfo> GetOUsByUser(int userID)
		{
			return this.ouDal.GetOUsByUser(userID);
		}

        /// <summary>
        /// 判断机构是否在指定的角色中
        /// </summary>
        /// <param name="ouID">机构ID</param>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
		public bool OUInRole(int ouID, string roleName)
		{
			bool result = false;
			List<RoleInfo> rolesByOU = new Role().GetRolesByOU(ouID);
			foreach (RoleInfo info in rolesByOU)
			{
				if (info.Name == roleName)
				{
					result = true;
					break;
				}
			}

			return result;
		}

        /// <summary>
        /// 在机构中移除指定的用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="ouID">机构ID</param>
		public void RemoveUser(int userID, int ouID)
		{
            if (this.OUInRole(ouID, RoleInfo.AdminName))
			{
				List<SimpleUserInfo> adminSimpleUsers = new Role().GetAdminSimpleUsers();
				if (adminSimpleUsers.Count == 1)
				{
					SimpleUserInfo info = (SimpleUserInfo) adminSimpleUsers[0];
					if (userID == info.ID)
					{
						throw new MyException("管理员角色至少需要包含一个用户！");
					}
				}
			}
			ouDal.RemoveUser(userID, ouID);
		}
	}
}