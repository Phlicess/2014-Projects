using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.Framework.Commons;
using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace WHC.MVCWebMis.DALSQL
{
    public class Role :  WHC.Framework.ControlUtil.BaseDALSQL<RoleInfo>, IRole
    {
        #region 对象实例及构造函数

        public static Role Instance
        {
            get
            {
                return new Role();
            }
        }
        public Role() : base("T_ACL_Role", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override RoleInfo DataReaderToEntity(IDataReader dataReader)
        {
            RoleInfo roleInfo = new RoleInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            roleInfo.ID = reader.GetInt32("ID");
            roleInfo.Name = reader.GetString("Name");
            roleInfo.Note = reader.GetString("Note");

            return roleInfo;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(RoleInfo obj)
        {
            RoleInfo info = obj as RoleInfo;
            Hashtable hash = new Hashtable();

            hash.Add("Name", info.Name);
            hash.Add("Note", info.Note);

            return hash;
        }

		public void AddFunction(int functionID, int roleID)
		{
            string commandText = string.Format("INSERT INTO T_ACL_Role_Function(Function_ID, Role_ID) VALUES({0},{1})", functionID, roleID);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(commandText);
            db.ExecuteNonQuery(command);
		}

		public void AddOU(int ouID, int roleID)
		{
            string commandText = string.Format("INSERT INTO T_ACL_OU_Role(OU_ID, Role_ID) VALUES({0},{1})", ouID, roleID);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(commandText);
            db.ExecuteNonQuery(command);
        }

		public void AddUser(int userID, int roleID)
		{
            string commandText = string.Format("INSERT INTO T_ACL_User_Role(User_ID, Role_ID) VALUES({0},{1})", userID, roleID);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(commandText);
            db.ExecuteNonQuery(command);
        }

        public List<RoleInfo> GetRolesByFunction(int functionID)
		{
            string sql = "SELECT * FROM T_ACL_Role INNER JOIN T_ACL_Role_Function On T_ACL_Role.ID=T_ACL_Role_Function.Role_ID WHERE Function_ID = " + functionID;
            return this.GetList(sql, null);
		}

        public List<RoleInfo> GetRolesByOU(int ouID)
		{
            string sql = "SELECT * FROM T_ACL_Role INNER JOIN T_ACL_OU_Role ON T_ACL_Role.ID=Role_ID WHERE OU_ID = " + ouID;
            return this.GetList(sql, null);
		}

        public List<RoleInfo> GetRolesByUser(int userID)
		{
            string sql = "SELECT * FROM T_ACL_Role INNER JOIN T_ACL_User_Role On T_ACL_Role.ID=T_ACL_User_Role.Role_ID WHERE User_ID = " + userID;
            return this.GetList(sql, null);
		}

		public void RemoveFunction(int functionID, int roleID)
		{
            string commandText = string.Format("DELETE FROM T_ACL_Role_Function WHERE Function_ID={0} AND Role_ID={1}", functionID, roleID);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(commandText);
            db.ExecuteNonQuery(command);
        }

		public void RemoveOU(int ouID, int roleID)
		{
            string commandText = string.Format("DELETE FROM T_ACL_OU_Role WHERE OU_ID={0} AND Role_ID={1}", ouID, roleID);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(commandText);
            db.ExecuteNonQuery(command);
        }

		public void RemoveUser(int userID, int roleID)
		{
            string commandText = string.Format("DELETE FROM T_ACL_User_Role WHERE User_ID={0} AND Role_ID={1}", userID, roleID);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(commandText);
            db.ExecuteNonQuery(command);
        }
	}
}