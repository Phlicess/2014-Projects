using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Reflection;
using System.Configuration;

using System.Data.SQLite;
using WHC.Pager.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using WHC.Framework.Commons;

namespace WHC.Framework.ControlUtil
{
    /// <summary>
    /// 数据访问层的基类
    /// </summary>
    public abstract class BaseDALSQLite<T> : AbstractBaseDAL<T>, IBaseDAL<T> where T : BaseEntity, new()
    {
        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseDALSQLite() { }

        /// <summary>
        /// 指定表名以及主键,对基类进构造
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">表主键</param>
        public BaseDALSQLite(string tableName, string primaryKey)
            : base(tableName, primaryKey)
        {
            this.parameterPrefix = "$";//或者为@也可以（数据库参数化访问的占位符）
            this.safeFieldFormat = "[{0}]"; //防止和保留字、关键字同名的字段格式(尽量避免）
        } 
        #endregion

        #region 通用操作方法

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
        /// <param name="targetTable">需要操作的目标表名称</param>
        /// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        public override int Insert2(Hashtable recordField, string targetTable, DbTransaction trans)
        {
            int result = -1;
            if (recordField == null || recordField.Count < 1)
            {
                return result;
            }

            string fields = ""; // 字段名
            string vals = ""; // 字段值
            foreach (string field in recordField.Keys)
            {
                fields += string.Format("[{0}],", field);//加[]为了去除别名引起的错误
                vals += string.Format("{0}{1},", parameterPrefix, field);
            }
            fields = fields.Trim(',');//除去前后的逗号
            vals = vals.Trim(',');//除去前后的逗号
            string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2});Select LAST_INSERT_ROWID()", targetTable, fields, vals);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            foreach (string field in recordField.Keys)
            {
                object val = recordField[field];
                val = val ?? DBNull.Value;
                if (val is DateTime)
                {
                    if (Convert.ToDateTime(val) <= Convert.ToDateTime("1753-1-1"))
                    {
                        val = DBNull.Value;
                    }
                }

                db.AddInParameter(command, field, TypeToDbType(val.GetType()), val);
            }

            if (trans != null)
            {
                result = Convert.ToInt32(db.ExecuteScalar(command, trans).ToString());
            }
            else
            {
                result = Convert.ToInt32(db.ExecuteScalar(command).ToString());
            }

            return result;
        }

        /// <summary>
        /// 测试数据库是否正常连接
        /// </summary>
        public override bool TestConnection(string connectionString)
        {
            bool result = false;

            using (DbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    result = true;
                }
            }

            return result;
        }

        #endregion

        #region 对象添加、修改、查询接口

        /// <summary>
        /// 查找记录表中最旧的一条记录
        /// </summary>
        /// <returns></returns>
        public override T FindFirst()
        {
            string sql = string.Format("Select {0} From {1} Order by {2} ASC LIMIT 1", selectedFields, tableName, GetSafeFileName(sortField));
            T entity = null;

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            using (IDataReader dr = db.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = DataReaderToEntity(dr);
                }
            }
            return entity;
        }

        /// <summary>
        /// 查找记录表中最新的一条记录
        /// </summary>
        /// <returns></returns>
        public override T FindLast()
        {
            string sql = string.Format("Select {0} From {1} Order by {2} DESC LIMIT 1", selectedFields, tableName, GetSafeFileName(sortField));
            T entity = null;

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            using (IDataReader dr = db.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = DataReaderToEntity(dr);
                }
            }
            return entity;
        }

        #region 下面两个覆盖基类函数，指定具体的数据库类型
        /// <summary>
        /// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="info">分页实体</param>
        /// <param name="fieldToSort">排序字段</param>
        /// <param name="desc">是否降序</param>
        /// <returns>指定对象的集合</returns>
        public override List<T> FindWithPager(string condition, PagerInfo info, string fieldToSort, bool desc)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            List<T> list = new List<T>();

            PagerHelper helper = new PagerHelper(tableName, this.selectedFields, fieldToSort,
                info.PageSize, info.CurrenetPageIndex, desc, condition);

            string countSql = helper.GetPagingSql(DatabaseType.SQLite, true);
            string strCount = SqlValueList(countSql);
            info.RecordCount = Convert.ToInt32(strCount);

            string dataSql = helper.GetPagingSql(DatabaseType.SQLite, false);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(dataSql);
            using (IDataReader dr = db.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    list.Add(this.DataReaderToEntity(dr));
                }
            }
            return list;
        }

        /// <summary>
        /// 根据条件查询数据库,并返回DataTable集合(用于分页数据显示)
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="info">分页实体</param>
        /// <param name="fieldToSort">排序字段</param>
        /// <param name="desc">是否降序</param>
        /// <returns>指定DataTable的集合</returns>
        public override DataTable FindToDataTable(string condition, PagerInfo info, string fieldToSort, bool desc)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            PagerHelper helper = new PagerHelper(tableName, this.selectedFields, fieldToSort,
                info.PageSize, info.CurrenetPageIndex, desc, condition);

            string countSql = helper.GetPagingSql(DatabaseType.SQLite, true);
            string strCount = SqlValueList(countSql);
            info.RecordCount = Convert.ToInt32(strCount);

            string dataSql = helper.GetPagingSql(DatabaseType.SQLite, false);
            return GetDataTableBySql(dataSql);
        } 
        #endregion

        /// <summary>
        /// 获取前面记录指定数量的记录
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="count">指定数量</param>
        /// <param name="orderBy">排序条件，例如order by id</param>
        /// <returns></returns>
        public override DataTable GetTopResult(string sql, int count, string orderBy)
        {
            string resultSql = string.Format("Select * From ({1} {2}) LIMIT {0} ", count, sql, orderBy);
            return SqlTable(resultSql);
        }

        #endregion

        #region 设置数据库的密码
        /// <summary>
        /// 根据配置数据库配置名称生成Database对象
        /// </summary>
        /// <returns></returns>
        protected override Database CreateDatabase()
        {
            Database db = null;
            if (string.IsNullOrEmpty(dbConfigName))
            {
                db = DatabaseFactory.CreateDatabase();
            }
            else
            {
                db = DatabaseFactory.CreateDatabase(dbConfigName);
            }

            DbConnectionStringBuilder sb = db.DbProviderFactory.CreateConnectionStringBuilder();
            sb.ConnectionString = GetConnectionString(dbConfigName);
            GenericDatabase newDb = new GenericDatabase(sb.ToString(), db.DbProviderFactory);
            db = newDb;

            return db;
        }

        /// <summary>
        /// 动态改变或者连接字符串
        /// </summary>
        /// <returns></returns>
        protected virtual string GetConnectionString(string dbConfigName)
        {
            string connectionString = "";
            DatabaseSettings setting = ConfigurationManager.GetSection("dataConfiguration") as DatabaseSettings;
            if (setting != null)
            {
                string connection = string.IsNullOrEmpty(dbConfigName) ? setting.DefaultDatabase : dbConfigName;
                connectionString = ConfigurationManager.ConnectionStrings[connection].ConnectionString;

                #region 加密解密操作

                //使用自定义加密
                //if (!connectionString.EndsWith(";"))
                //{
                //    connectionString += ";";
                //}
                //connectionString += string.Format(";Password=wuhuacong2013;");

                string password = GetSubValue(connectionString, "password");
                if (!string.IsNullOrEmpty(password))
                {
                    //尝试使用AES解密
                    string decryptStr = password;
                    try
                    {
                        decryptStr = EncodeHelper.AES_Decrypt(password);
                    }
                    catch
                    {
                        decryptStr = password;
                        //throw new InvalidOperationException("无法解密数据库");
                    }

                    connectionString += string.Format(";Password={0};", decryptStr);
                }
                #endregion
            }

            return connectionString;
        }

        #endregion

    }
}