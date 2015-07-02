using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.BLL
{
    /// <summary>
    /// 系统标识信息
    /// </summary>
    public class SystemType : BaseBLL<SystemTypeInfo>
	{
		private ISystemType systemTypeDal;

        /// <summary>
        /// 构造函数
        /// </summary>
		public SystemType() :base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
			this.systemTypeDal = (ISystemType) baseDal;
		}

        /// <summary>
        /// 根据系统OID获取系统标识信息
        /// </summary>
        /// <param name="oid">系统OID</param>
        /// <returns></returns>
		public SystemTypeInfo FindByOID(string oid)
		{
			return this.systemTypeDal.FindByOID(oid);
		}


        /// <summary>
        /// 验证系统是否被授权注册
        /// </summary>
        /// <param name="serialNumber">序列号</param>
        /// <param name="typeID">类型ID</param>
        /// <param name="authorizeAmount">授权数量</param>
        /// <returns></returns>
		public bool VerifySystem(string serialNumber, string typeID, int authorizeAmount)
		{
			return this.systemTypeDal.VerifySystem(serialNumber, typeID, authorizeAmount);
		}
	}
}