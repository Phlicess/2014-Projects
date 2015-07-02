using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.Framework.Commons;

namespace WHC.MVCWebMis.DALSQLite
{
    public class OU :  WHC.Framework.ControlUtil.BaseDALSQLite<OUInfo>, IOU
    {
        #region 系统所需函数
        #region 对象实例及构造函数

        public static OU Instance
        {
            get
            {
                return new OU();
            }
        }
        public OU()
            : base("T_ACL_OU", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override OUInfo DataReaderToEntity(IDataReader dataReader)
        {
            OUInfo oUInfo = new OUInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            oUInfo.ID = reader.GetInt32("ID");
            oUInfo.PID = reader.GetInt32("PID");
            oUInfo.Name = reader.GetString("Name");
            oUInfo.Address = reader.GetString("Address");
            oUInfo.Note = reader.GetString("Note");

            return oUInfo;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(OUInfo obj)
        {
            OUInfo info = obj as OUInfo;
            Hashtable hash = new Hashtable();

            hash.Add("PID", info.PID);
            hash.Add("Name", info.Name);
            hash.Add("Address", info.Address);
            hash.Add("Note", info.Note);

            return hash;
        } 
        #endregion

		public void AddUser(int userID, int ouID)
		{
            string commandText = string.Format("INSERT INTO T_ACL_OU_User(User_ID, OU_ID) VALUES({0},{1})", userID, ouID);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(commandText);
            db.ExecuteNonQuery(command);           
		}

        public void RemoveUser(int userID, int ouID)
        {
            string commandText = string.Format("DELETE FROM T_ACL_OU_User WHERE User_ID={0} AND OU_ID={1}", userID, ouID);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(commandText);
            db.ExecuteNonQuery(command);
        }

		public override bool  Delete(object key)
        {
            Database db = CreateDatabase();
            DbCommand command = null;

            DbTransaction transaction = null;
			try
			{
				OUInfo info = this.FindByID(key);
                if (info != null)
                {
                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();
                        transaction = connection.BeginTransaction();

                        string commandText = string.Format("UPDATE [T_ACL_OU] SET PID={0} Where PID={1}", info.PID, key);
                        command = db.GetSqlStringCommand(commandText);
                        db.ExecuteNonQuery(command, transaction);

                        commandText = string.Format("Delete From [T_ACL_OU] Where ID={0}", key);
                        command = db.GetSqlStringCommand(commandText);
                        db.ExecuteNonQuery(command, transaction);

                        transaction.Commit();
                    }
                }
			}
			catch
			{
                if (transaction != null)
                {
                    transaction.Rollback();
                    throw;
                }
			}

            return true;
		}

        public List<OUInfo> GetOUsByRole(int roleID)
		{
            string sql = "SELECT * FROM T_ACL_OU INNER JOIN T_ACL_OU_Role On [T_ACL_OU].ID=T_ACL_OU_Role.OU_ID WHERE Role_ID = " + roleID;
            return this.GetList(sql, null);
		}

        public List<OUInfo> GetOUsByUser(int userID)
		{
            string sql = "SELECT * FROM T_ACL_OU INNER JOIN T_ACL_OU_User On [T_ACL_OU].ID=T_ACL_OU_User.OU_ID WHERE User_ID = " + userID;
            return this.GetList(sql, null);
		}

	}
}