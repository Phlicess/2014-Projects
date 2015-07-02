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
    /// 角色信息业务管理类
    /// </summary>
    public class Role : BaseBLL<RoleInfo>
	{
        /// <summary>
        /// 该ID实际为一个无效ID，当调用FillAdminID会初始化为真是的管理员ID，以后以该实际ID作为管理员的凭证
        /// </summary>
        private static int m_AdminID = -99;
		private IRole roleDal;

        /// <summary>
        /// 构造函数
        /// </summary>
		public Role() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
			this.roleDal = baseDal as IRole;
		}

        /// <summary>
        /// 为角色添加操作权限
        /// </summary>
        /// <param name="functionID">功能ID</param>
        /// <param name="roleID">角色ID</param>
		public void AddFunction(int functionID, int roleID)
		{
			this.roleDal.AddFunction(functionID, roleID);
		}

        /// <summary>
        /// 为角色添加机构
        /// </summary>
        /// <param name="ouID">机构ID</param>
        /// <param name="roleID">角色ID</param>
		public void AddOU(int ouID, int roleID)
		{
			roleDal.AddOU(ouID, roleID);
		}

        /// <summary>
        /// 为角色添加用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="roleID">角色ID</param>
		public void AddUser(int userID, int roleID)
		{
			this.FillAdminID();
			if (roleID == m_AdminID)
			{
				new User().CancelExpire(userID);
			}

			roleDal.AddUser(userID, roleID);
		}

        /// <summary>
        /// 判断Admin用户是否包含用户
        /// </summary>
        /// <returns></returns>
		internal bool AdminHasUser()
		{
			this.FillAdminID();

			User user = new User();
			return (user.GetSimpleUsersByRole(m_AdminID).Count > 0);
		}

        /// <summary>
        /// 检查管理员角色不被移除
        /// </summary>
        /// <param name="roleID"></param>
		private void CanRemoveFromAdmin(int roleID)
		{
			this.FillAdminID();

			if ((roleID == m_AdminID) && (this.GetAdminSimpleUsers().Count <= 1))
			{
				throw new MyException("管理员角色至少需要包含一个用户！");
			}
		}

		public override bool Delete(object key)
        {
            this.FillAdminID();

            if (Convert.ToInt32(key) == m_AdminID)
			{
				throw new MyException("管理员角色不能被删除！");
			}
			return baseDal.Delete(key);
		}

        /// <summary>
        /// 找到对应的角色名称（管理员），获取其对应的ID作为今后比较
        /// </summary>
		private void FillAdminID()
		{
			if (m_AdminID == -99)
			{
				RoleInfo roleByName = this.GetRoleByName(RoleInfo.AdminName);
				if (roleByName != null)
				{
					m_AdminID = roleByName.ID;//保存ID作为管理员角色参考
				}
			}
		}

        /// <summary>
        /// 获取管理员包含的机构ID列表
        /// </summary>
        /// <returns></returns>
		internal List<int> GetAdminOUIDs()
		{
			this.FillAdminID();

			List<OUInfo> oUsByRole = new OU().GetOUsByRole(m_AdminID);
            List<int> list = new List<int>();
			foreach (OUInfo info in oUsByRole)
			{
				list.Add(info.ID);
			}
			return list;
		}

        /// <summary>
        /// 获取管理员包含的用户基础信息列表
        /// </summary>
        /// <returns></returns>
        internal List<SimpleUserInfo> GetAdminSimpleUsers()
		{
			this.FillAdminID();

			User user = new User();
			List<SimpleUserInfo> simpleUsersByRole = user.GetSimpleUsersByRole(m_AdminID);
			int count = simpleUsersByRole.Count;
			if (count <= 1)
			{
				OU ou = new OU();
				foreach (OUInfo info in ou.GetOUsByRole(m_AdminID))
				{
                    List<SimpleUserInfo> simpleUsersByOU = user.GetSimpleUsersByOU(info.ID);
					if (simpleUsersByOU.Count > 0)
					{
						simpleUsersByRole.Add(simpleUsersByOU[0]);
						count++;
						if (simpleUsersByOU.Count > 1)
						{
							simpleUsersByRole.Add(simpleUsersByOU[1]);
							count++;
						}
						if (count > 1)
						{
							return simpleUsersByRole;
						}
					}
				}
			}
			return simpleUsersByRole;
		}

        /// <summary>
        /// 根据角色名称查找角色对象
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
		public RoleInfo GetRoleByName(string roleName)
		{
            string condition = string.Format("Name='{0}'", roleName);
			return this.roleDal.FindSingle(condition);
		}

        /// <summary>
        /// 获取对应功能的相关角色列表
        /// </summary>
        /// <param name="functionID">对应功能ID</param>
        /// <returns></returns>
		public List<RoleInfo> GetRolesByFunction(int functionID)
		{
			return this.roleDal.GetRolesByFunction(functionID);
		}

        /// <summary>
        /// 根据机构的ID获取对应的角色列表
        /// </summary>
        /// <param name="ouID">机构的ID</param>
        /// <returns></returns>
        public List<RoleInfo> GetRolesByOU(int ouID)
		{
			return this.roleDal.GetRolesByOU(ouID);
		}

        /// <summary>
        /// 根据用户的ID获取对应的角色列表
        /// </summary>
        /// <param name="userID">用户的ID</param>
        /// <returns></returns>
        public List<RoleInfo> GetRolesByUser(int userID)
		{
			List<RoleInfo> rolesByUser = this.roleDal.GetRolesByUser(userID);
			ArrayList list2 = new ArrayList();
			foreach (RoleInfo info in rolesByUser)
			{
				list2.Add(info.ID);
			}

			OU ou = new OU();
			foreach (OUInfo info2 in ou.GetOUsByUser(userID))
			{
				foreach (RoleInfo info3 in this.roleDal.GetRolesByOU(info2.ID))
				{
					if (list2.IndexOf(info3.ID) < 0)
					{
						rolesByUser.Add(info3);
						list2.Add(info3.ID);
					}
				}
			}
			return rolesByUser;
		}

        /// <summary>
        /// 从角色操作功能列表中，移除对应的功能
        /// </summary>
        /// <param name="functionID">功能ID</param>
        /// <param name="roleID">角色ID</param>
		public void RemoveFunction(int functionID, int roleID)
		{
			this.roleDal.RemoveFunction(functionID, roleID);
		}

        /// <summary>
        /// 从角色机构列表中，移除指定的机构
        /// </summary>
        /// <param name="ouID">机构ID</param>
        /// <param name="roleID">角色ID</param>
		public void RemoveOU(int ouID, int roleID)
		{
			this.FillAdminID();
			if (roleID == m_AdminID)
			{
				User user = new User();
				List<SimpleUserInfo> simpleUsersByRole = user.GetSimpleUsersByRole(m_AdminID);
				if (simpleUsersByRole.Count < 1)
				{
					simpleUsersByRole.Clear();
					List<UserInfo> usersByOU = user.GetUsersByOU(ouID);
					if (usersByOU.Count > 0)
					{
						usersByOU.Clear();
						bool flag = false;
						List<OUInfo> oUsByRole = new OU().GetOUsByRole(m_AdminID);
						foreach (OUInfo info in oUsByRole)
						{
							if ((info.ID != ouID) && (user.GetSimpleUsersByOU(info.ID).Count > 0))
							{
								flag = true;
								break;
							}
						}
						oUsByRole.Clear();
						if (!flag)
						{
							throw new MyException("管理员角色至少需要包含一个用户！");
						}
					}
				}
			}
			roleDal.RemoveOU(ouID, roleID);
		}

        /// <summary>
        /// 从角色的用户列表中移除指定的用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="roleID">角色ID</param>
		public void RemoveUser(int userID, int roleID)
		{
			this.CanRemoveFromAdmin(roleID);
			this.roleDal.RemoveUser(userID, roleID);
		}

        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="obj">角色对象</param>
        /// <param name="primaryKeyValue">主键</param>
        /// <returns></returns>
		public override bool  Update(RoleInfo obj, object primaryKeyValue)
		{
			RoleInfo info = (RoleInfo) obj;
			if (info.ID == m_AdminID)
			{
				info.Name = RoleInfo.AdminName;
			}
            return base.Update(info, primaryKeyValue);
		}
	}
}