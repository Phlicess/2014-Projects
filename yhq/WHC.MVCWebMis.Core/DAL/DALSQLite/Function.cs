using System.Collections;
using System.Data;
using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using System;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Collections.Generic;
using WHC.Framework.Commons;

namespace WHC.MVCWebMis.DALSQLite
{
    public class Function :  WHC.Framework.ControlUtil.BaseDALSQLite<FunctionInfo>, IFunction
    {
        #region 对象实例及构造函数

        public static Function Instance
        {
            get
            {
                return new Function();
            }
        }
        public Function()
            : base("T_ACL_Function", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override FunctionInfo DataReaderToEntity(IDataReader dataReader)
        {
            FunctionInfo functionInfo = new FunctionInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            functionInfo.ID = reader.GetInt32("ID");
            functionInfo.PID = reader.GetInt32("PID");
            functionInfo.Name = reader.GetString("Name");
            functionInfo.ControlID = reader.GetString("ControlID");
            functionInfo.SystemType_ID = reader.GetString("SystemType_ID");

            return functionInfo;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(FunctionInfo obj)
        {
            FunctionInfo info = obj as FunctionInfo;
            Hashtable hash = new Hashtable();

            hash.Add("PID", info.PID);
            hash.Add("Name", info.Name);
            hash.Add("ControlID", info.ControlID);
            hash.Add("SystemType_ID", info.SystemType_ID);

            return hash;
        }

        /// <summary>
        /// 重写删除操作，把下面的节点提到上一级
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool Delete(object key)
        {
            FunctionInfo info = this.FindByID(key);
            if (info != null)
            {
                Database db = CreateDatabase();
                DbCommand command = null;

                string sql = string.Format("UPDATE [T_ACL_Function] SET PID={0} Where PID={1}", info.PID, key);
                command = db.GetSqlStringCommand(sql);

                using (DbTransaction trans = CreateTransaction())
                {
                    try
                    {
                        if (trans != null)
                        {
                            db.ExecuteNonQuery(command, trans);
                            base.Delete(key, trans);
                            trans.Commit();

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogTextHelper.Error(ex);
                        if (trans != null)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }

            return false;
        }

		public List<FunctionInfo> GetFunctions(string roleIDs, string typeID)
		{
            string sql = @"SELECT DISTINCT [ID],PID,[NAME],ControlID,SystemType_ID FROM [T_ACL_Function] 
            INNER JOIN T_ACL_Role_Function On [T_ACL_Function].ID=T_ACL_Role_Function.Function_ID WHERE Role_ID IN (" + roleIDs + ")";
			if (typeID.Length > 0)
			{
				sql = sql + string.Format(" AND SystemType_ID='{0}' ", typeID);
			}
            return this.GetList(sql, null);
		}

        public List<FunctionInfo> GetFunctionsByRole(int roleID)
		{
            string sql = @"SELECT * FROM [T_ACL_Function] 
            LEFT JOIN T_ACL_Role_Function On [T_ACL_Function].ID=T_ACL_Role_Function.Function_ID WHERE Role_ID = " + roleID;
            return this.GetList(sql, null);
		}

        /// <summary>
        /// 获取树形结构的功能列表
        /// </summary>
        public List<FunctionNodeInfo> GetTree(string systemType)
        {
            string condition = !string.IsNullOrEmpty(systemType) ? string.Format("Where SystemType_ID='{0}'", systemType) : "";
            List<FunctionNodeInfo> arrReturn = new List<FunctionNodeInfo>();
            string sql = string.Format("Select * From {0} {1} Order By PID, Name ", tableName, condition);

            DataTable dt = base.SqlTable(sql);
            DataRow[] dataRows = dt.Select(string.Format(" PID = '{0}' ", -1));
            for (int i = 0; i < dataRows.Length; i++)
            {
                string id = dataRows[i]["ID"].ToString();
                FunctionNodeInfo menuNodeInfo = GetNode(id, dt);
                arrReturn.Add(menuNodeInfo);
            }

            return arrReturn;
        }

        /// <summary>
        /// 获取指定功能下面的树形列表
        /// </summary>
        /// <param name="id">指定功能ID</param>
        public List<FunctionNodeInfo> GetTreeByID(string mainID)
        {
            List<FunctionNodeInfo> arrReturn = new List<FunctionNodeInfo>();
            string sql = string.Format("Select * From {0} Order By PID, Name ", tableName);

            DataTable dt = SqlTable(sql);
            DataRow[] dataRows = dt.Select(string.Format(" PID = '{0}'", mainID));
            for (int i = 0; i < dataRows.Length; i++)
            {
                string id = dataRows[i]["ID"].ToString();
                FunctionNodeInfo menuNodeInfo = GetNode(id, dt);
                arrReturn.Add(menuNodeInfo);
            }

            return arrReturn;
        }

        private FunctionNodeInfo GetNode(string id, DataTable dt)
        {
            FunctionInfo menuInfo = this.FindByID(id);
            FunctionNodeInfo menuNodeInfo = new FunctionNodeInfo(menuInfo);

            DataRow[] dChildRows = dt.Select(string.Format(" PID='{0}'", id));

            for (int i = 0; i < dChildRows.Length; i++)
            {
                string childId = dChildRows[i]["ID"].ToString();
                FunctionNodeInfo childNodeInfo = GetNode(childId, dt);
                menuNodeInfo.Children.Add(childNodeInfo);
            }
            return menuNodeInfo;
        }
	}
}