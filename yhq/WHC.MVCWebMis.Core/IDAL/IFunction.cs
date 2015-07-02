using System.Collections;
using WHC.MVCWebMis.Entity;
using System.Collections.Generic;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.IDAL
{
    public interface IFunction : IBaseDAL<FunctionInfo>
	{
        List<FunctionInfo> GetFunctions(string roleIDs, string typeID);
        List<FunctionInfo> GetFunctionsByRole(int roleID);

        List<FunctionNodeInfo> GetTree(string systemType);
        List<FunctionNodeInfo> GetTreeByID(string mainID);
	}
}