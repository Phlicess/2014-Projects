using System.Collections;
using WHC.MVCWebMis.Entity;
using System.Collections.Generic;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.IDAL
{
    public interface IOU : IBaseDAL<OUInfo>
	{
		void AddUser(int userID, int ouID);
        List<OUInfo> GetOUsByRole(int roleID);
        List<OUInfo> GetOUsByUser(int userID);
		void RemoveUser(int userID, int ouID);
	}
}