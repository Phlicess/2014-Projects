using WHC.MVCWebMis.Entity;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.IDAL
{
    public interface ISystemType : IBaseDAL<SystemTypeInfo>
	{
		SystemTypeInfo FindByOID(string oid);
		bool VerifySystem(string serialNumber, string typeID, int authorizeAmount);
	}
}