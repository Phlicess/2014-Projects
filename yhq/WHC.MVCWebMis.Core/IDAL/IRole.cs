using System.Collections;
using WHC.MVCWebMis.Entity;
using System.Collections.Generic;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.IDAL
{
    public interface IRole : IBaseDAL<RoleInfo>
	{
		void AddFunction(int functionID, int roleID);
		void AddOU(int ouID, int roleID);
		void AddUser(int userID, int roleID);
        List<RoleInfo> GetRolesByFunction(int functionID);
        List<RoleInfo> GetRolesByOU(int ouID);
        List<RoleInfo> GetRolesByUser(int userID);
		void RemoveFunction(int functionID, int roleID);
		void RemoveOU(int ouID, int roleID);
		void RemoveUser(int userID, int roleID);
	}
}