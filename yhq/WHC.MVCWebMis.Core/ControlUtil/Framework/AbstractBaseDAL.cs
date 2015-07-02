using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using WHC.Pager.Entity;
using WHC.Framework.Commons;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace WHC.Framework.ControlUtil
{
	/// <summary>
	/// 数据访问层的超级基类，所有数据库的数据访问基类都继承自这个超级基类，包括Oracle、SqlServer、Sqlite、MySql、Access等
	/// </summary>
    public abstract class AbstractBaseDAL<T> where T : BaseEntity, new()
	{
		#region 构造函数

        protected string dbConfigName = ""; //数据库配置名称
        protected string parameterPrefix = "@";//数据库参数化访问的占位符
        protected string safeFieldFormat = "[{0}]";//防止和保留字、关键字同名的字段格式，如[value]
        protected string tableName;//需要初始化的对象表名
        protected string primaryKey;//数据库的主键字段名
        protected string sortField;//排序字段
        protected bool isDescending = true;//是否为降序
        protected string selectedFields = " * ";//选择的字段，默认为所有(*)
        
        /// <summary>
        /// 数据库配置名称，默认为空。
        /// 可在子类指定不同的配置名称，用于访问不同的数据库
        /// </summary>
        public string DbConfigName
        {
            get { return dbConfigName; }
            set { dbConfigName = value; }
        }

        /// <summary>
        /// 数据库参数化访问的占位符
        /// </summary>
        public string ParameterPrefix
        {
            get { return parameterPrefix; }
            set { parameterPrefix = value; }
        }

        /// <summary>
        /// 防止和保留字、关键字同名的字段格式，如[value]。
        /// 不同数据库类型的BaseDAL需要进行修改
        /// </summary>
        public string SafeFieldFormat
        {
            get { return safeFieldFormat; }
            set { safeFieldFormat = value; }
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField
        {
            get 
            {
                return sortField; 
            }
            set 
            {
                sortField = value; 
            }
        }

        /// <summary>
        /// 是否为降序
        /// </summary>
        public bool IsDescending
        {
            get { return isDescending; }
            set { isDescending = value; }
        }       

        /// <summary>
        /// 选择的字段，默认为所有(*)
        /// </summary>
        public string SelectedFields
        {
            get { return selectedFields; }
            set { selectedFields = value; }
        }

		/// <summary>
		/// 数据库访问对象的表名
		/// </summary>
		public string TableName
		{
			get
			{
				return tableName;
			}
		}

		/// <summary>
		/// 数据库访问对象的外键约束
		/// </summary>
		public string PrimaryKey
		{
			get
			{
				return primaryKey;
			}
		}
		
        /// <summary>
        /// 默认构造函数
        /// </summary>
		public AbstractBaseDAL()
		{}

		/// <summary>
		/// 指定表名以及主键,对基类进构造
		/// </summary>
		/// <param name="tableName">表名</param>
		/// <param name="primaryKey">表主键</param>
        public AbstractBaseDAL(string tableName, string primaryKey)
		{
			this.tableName = tableName;
			this.primaryKey = primaryKey;
            this.sortField = primaryKey;//默认为主键排序
		}

        /// <summary>
        /// 根据配置数据库配置名称生成Database对象
        /// </summary>
        /// <returns></returns>
        protected virtual Database CreateDatabase()
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
            return db;
        }

        /// <summary>
        /// 获取指定字符串中的子项的值
        /// </summary>
        /// <param name="connectionString">字符串值</param>
        /// <param name="subKeyName">以分号(;)为分隔符的子项名称</param>
        /// <returns>对应子项名称的值（即是=号后面的值）</returns>
        protected string GetSubValue(string connectionString, string subKeyName)
        {
            string[] item = connectionString.Split(new char[] { ';' });
            for (int i = 0; i < item.Length; i++)
            {
                string itemValue = item[i].ToLower();
                if (itemValue.IndexOf(subKeyName, StringComparison.OrdinalIgnoreCase) >= 0) //如果含有指定的关键字
                {
                    int startIndex = item[i].IndexOf("="); //等号开始的位置
                    return item[i].Substring(startIndex + 1).Trim(); //获取等号后面的值即为Value
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 生成防止和保留字、关键字同名的字段格式，如[value]。
        /// </summary>
        /// <param name="fieldName">字段名，如value</param>
        protected string GetSafeFileName(string fieldName)
        {
            return string.Format(safeFieldFormat, fieldName);
        }

		#endregion    

        #region 通用操作方法

        /// <summary>
		/// 添加记录
		/// </summary>
		/// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
		/// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        public virtual bool Insert(Hashtable recordField, DbTransaction trans)
		{
			return this.Insert(recordField, tableName, trans);
		}

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
        /// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        public virtual int Insert2(Hashtable recordField, DbTransaction trans)
        {
            return this.Insert2(recordField, tableName, trans);
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
        /// <param name="targetTable">需要操作的目标表名称</param>
        /// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        public virtual bool Insert(Hashtable recordField, string targetTable, DbTransaction trans)
        {
            bool result = false;
            if (recordField == null || recordField.Count < 1)
            {
                return result;
            }

            string fields = ""; // 字段名
            string vals = ""; // 字段值
            foreach (string field in recordField.Keys)
            {
                fields += string.Format("{0},", GetSafeFileName(field));
                vals += string.Format("{0}{1},", parameterPrefix, field);
            }
            fields = fields.Trim(',');//除去前后的逗号
            vals = vals.Trim(',');//除去前后的逗号
            string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", targetTable, fields, vals);

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
                result = db.ExecuteNonQuery(command, trans) > 0;
            }
            else
            {
                result = db.ExecuteNonQuery(command) > 0;
            }

            return result;
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
        /// <param name="targetTable">需要操作的目标表名称</param>
        /// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        public virtual int Insert2(Hashtable recordField, string targetTable, DbTransaction trans)
        {
            throw new NotSupportedException();
        }
		
		/// <summary>
		/// 更新某个表一条记录(只适用于用单键,用int类型作键值的表)
		/// </summary>
		/// <param name="id">ID值</param>
		/// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
		/// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        public virtual bool Update(object id, Hashtable recordField, DbTransaction trans)
		{
            return this.PrivateUpdate(id, recordField, tableName, trans);
		}

		/// <summary>
		/// 更新某个表一条记录(只适用于用单键,用int类型作键值的表)
		/// </summary>
		/// <param name="id">ID值</param>
		/// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
		/// <param name="targetTable">需要操作的目标表名称</param>
		/// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        public virtual bool Update(object id, Hashtable recordField, string targetTable, DbTransaction trans)
		{
            return PrivateUpdate(id, recordField, targetTable, trans);
		}

        /// <summary>
        /// 更新某个表一条记录
        /// </summary>
        /// <param name="id">ID值</param>
        /// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
        /// <param name="targetTable">需要操作的目标表名称</param>
        /// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        public virtual bool PrivateUpdate(object id, Hashtable recordField, string targetTable, DbTransaction trans)
        {
            try
            {
                if (recordField == null || recordField.Count < 1)
                {
                    return false;
                }

                string setValue = "";
                foreach (string field in recordField.Keys)
                {
                    setValue += string.Format("{0} = {1}{2},", GetSafeFileName(field), parameterPrefix, field);
                }
                string sql = string.Format("UPDATE {0} SET {1} WHERE {2} = {3}{2} ",
                    targetTable, setValue.Substring(0, setValue.Length - 1), primaryKey, parameterPrefix);
                Database db = CreateDatabase();
                DbCommand command = db.GetSqlStringCommand(sql);

                bool foundID = false;
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
                        db.AddInParameter(command, field, DbType.DateTime, val);
                    }
                    else
                    {
                        db.AddInParameter(command, field, TypeToDbType(val.GetType()), val);
                    }

                    if (field == primaryKey)
                    {
                        foundID = true;
                    }
                }

                if (!foundID)
                {
                    db.AddInParameter(command, primaryKey, TypeToDbType(id.GetType()), id);
                }

                bool result = false;
                if (trans != null)
                {
                    result = db.ExecuteNonQuery(command, trans) > 0;
                }
                else
                {
                    result = db.ExecuteNonQuery(command) > 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                LogTextHelper.WriteLine(ex.ToString());
                throw;
            }
        }
        
        /// <summary>
        /// 执行SQL查询语句，返回查询结果的所有记录的第一个字段,用逗号分隔。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>
        /// 返回查询结果的所有记录的第一个字段,用逗号分隔。
        /// </returns>
        public virtual string SqlValueList(string sql)
        {
            StringBuilder result = new StringBuilder();
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            using (IDataReader dr = db.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    result.AppendFormat("{0},", dr[0].ToString());
                }
            }
            string strResult = result.ToString().Trim(',');
            return strResult;
        }

        /// <summary>
        /// 执行一些特殊的语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        public virtual int SqlExecute(string sql)
        {
            Database db = CreateDatabase();
            return db.ExecuteNonQuery(CommandType.Text, sql);
        }

        /// <summary>
        /// 执行一些特殊的语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="trans">事务对象</param>
        public virtual int SqlExecute(string sql, DbTransaction trans)
        {
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            if (trans != null)
            {
                return db.ExecuteNonQuery(command, trans);
            }
            else
            {
                return db.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 执行存储过程函数。
        /// </summary>
        /// <param name="storeProcName">存储过程函数</param>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        public virtual int StoreProcExecute(string storeProcName, DbParameter[] parameters)
        {
            Database db = CreateDatabase();
            DbCommand command = db.GetStoredProcCommand(storeProcName);
            foreach (DbParameter param in parameters)
            {
                db.AddInParameter(command, param.ParameterName, param.DbType, param.Value);
            } 
            return db.ExecuteNonQuery(command);
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataTable集合。
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <returns></returns>
        public virtual DataTable SqlTable(string sql)
        {
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(command).Tables[0];
        }

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="fieldsToReturn">需要返回的列</param>
        /// <param name="condition">查询的条件</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="isDescending">是否降序</param>
        /// <param name="info">分页实体</param>
        /// <returns>指定对象的集合</returns>
        public virtual DataTable SqlTableWithPager(string sql, string fieldsToReturn, string condition, string sortField, bool isDescending, PagerInfo info)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            PagerHelper helper = new PagerHelper(sql, fieldsToReturn, sortField,
                info.PageSize, info.CurrenetPageIndex, isDescending, condition);

            string countSql = helper.GetPagingSql(true);
            string strCount = SqlValueList(countSql);
            info.RecordCount = Convert.ToInt32(strCount);

            string dataSql = helper.GetPagingSql(false);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(dataSql);
            return db.ExecuteDataSet(command).Tables[0];
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataTable集合。
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        public virtual DataTable SqlTable(string sql, DbParameter[] parameters)
        {
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            foreach (DbParameter param in parameters)
            {
                db.AddInParameter(command, param.ParameterName, param.DbType, param.Value);
            }
            return db.ExecuteDataSet(command).Tables[0];
        }

        /// <summary>
        /// 打开数据库连接，并创建事务对象
        /// </summary>
        public virtual DbTransaction CreateTransaction()
        {
            Database db = CreateDatabase();
            DbConnection connection = db.CreateConnection();
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }

            return connection.BeginTransaction();
        }

        /// <summary>
        /// 测试数据库是否正常连接
        /// </summary>
        public virtual bool TestConnection(string connectionString)
        {
            throw new NotSupportedException();
        }

		#endregion

		#region 对象添加、修改

		/// <summary>
		/// 插入指定对象到数据库中
		/// </summary>
		/// <param name="obj">指定的对象</param>
		/// <returns>执行成功返回新增记录的自增长ID。</returns>
        public virtual bool Insert(T obj)
		{
            return Insert(obj, null);
		}

        /// <summary>
        /// 插入指定对象到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="trans">事务</param>
        /// <returns>执行成功返回true或false</returns>
        public virtual bool Insert(T obj, DbTransaction trans)
        {
            ArgumentValidation.CheckForNullReference(obj, "传入的对象obj为空");

            Hashtable hash = GetHashByEntity(obj);
            return Insert(hash, trans);
        }

        /// <summary>
        /// 插入指定对象到数据库中,并返回自增长的键值
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <returns>执行成功返回True</returns>
        public virtual int Insert2(T obj)
        {
            ArgumentValidation.CheckForNullReference(obj, "传入的对象obj为空");

            Hashtable hash = GetHashByEntity(obj);
            return Insert2(hash, null);
        }

        /// <summary>
        /// 插入指定对象到数据库中,并返回自增长的键值
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns>执行成功返回True</returns>
        public virtual int Insert2(T obj, DbTransaction trans)
        {
            ArgumentValidation.CheckForNullReference(obj, "传入的对象obj为空");

            Hashtable hash = GetHashByEntity(obj);
            return Insert2(hash, trans);
        }
		
		/// <summary>
		/// 更新对象属性到数据库中
		/// </summary>
		/// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
		/// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool Update(T obj, string primaryKeyValue)
		{
            return Update(obj, primaryKeyValue, null);
		}
		
		/// <summary>
		/// 更新对象属性到数据库中
		/// </summary>
		/// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool Update(T obj, object primaryKeyValue)
		{
            return Update(obj, primaryKeyValue, null);
		}

        /// <summary>
        /// 更新对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <param name="trans">事务对象</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool Update(T obj, object primaryKeyValue, DbTransaction trans)
        {
            ArgumentValidation.CheckForNullReference(obj, "传入的对象obj为空");

            Hashtable hash = GetHashByEntity(obj);
            return Update(primaryKeyValue, hash, trans);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="commandType">数据类型</param>
        /// <param name="sql">sql</param>
        /// <returns>bool</returns>
        public virtual bool Update(CommandType commandType, string sql)
        {
            return Update(commandType, sql, null);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="commandType">数据类型</param>
        /// <param name="sql">sql</param>
        /// <param name="trans">事务对象</param>
        /// <returns>bool</returns>
        public virtual bool Update(CommandType commandType, string sql, DbTransaction trans)
        {
            Database db = CreateDatabase();
            if (trans != null)
            {
                return db.ExecuteNonQuery(trans, CommandType.Text, sql) > 0;
            }
            else
            {
                return db.ExecuteNonQuery(CommandType.Text, sql) > 0;
            }
        }
                       
        /// <summary>
        /// 插入或更新对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool InsertUpdate(T obj, object primaryKeyValue)
        {
            return InsertUpdate(obj, primaryKeyValue, null);
        }

        /// <summary>
        /// 插入或更新对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <param name="trans">事务对象</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool InsertUpdate(T obj, object primaryKeyValue, DbTransaction trans)
        {
            bool isExist = IsExistKey(primaryKey, primaryKeyValue);
            if (isExist)
            {
                return Update(obj, primaryKey, trans);
            }
            else
            {
                return Insert(obj, trans);
            }
        }

        /// <summary>
        /// 如果不存在记录，则插入对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <returns>执行插入成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool InsertIfNew(T obj, object primaryKeyValue)
        {
            return InsertIfNew(obj, primaryKeyValue, null);
        }

        /// <summary>
        /// 如果不存在记录，则插入对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <param name="trans">事务对象</param>
        /// <returns>执行插入成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool InsertIfNew(T obj, object primaryKeyValue, DbTransaction trans)
        {
            bool isExist = IsExistKey(primaryKey, primaryKeyValue);
            if (!isExist)
            {
                return Insert(obj, trans);
            }
            return false;
        }
 
		#endregion

        #region 返回实体类操作

        /// <summary>
        /// 查询数据库,检查是否存在指定ID的对象
        /// </summary>
        /// <param name="key">对象的ID值</param>
        /// <returns>存在则返回指定的对象,否则返回Null</returns>
        public virtual T FindByID(object key)
        {
            return PrivateFindByID(key);
        }

        /// <summary>
        /// 提供对FindByID的私有方法实现
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private T PrivateFindByID(object key)
        {
            string sql = string.Format("Select {0} From {1} Where ({2} = {3}ID)", selectedFields, tableName, primaryKey, parameterPrefix);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            db.AddInParameter(command, "ID", TypeToDbType(key.GetType()), key);

            T entity = null;
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
        /// 根据条件查询数据库,如果存在返回第一个对象
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <returns>指定的对象</returns>
        public virtual T FindSingle(string condition)
        {
            return FindSingle(condition, null);
        }

        /// <summary>
        /// 根据条件查询数据库,如果存在返回第一个对象
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <returns>指定的对象</returns>
        public virtual T FindSingle(string condition, string orderBy)
        {
            return FindSingle(condition, orderBy, null);
        }

        /// <summary>
        /// 根据条件查询数据库,如果存在返回第一个对象
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <param name="paramList">参数列表</param>
        /// <returns>指定的对象</returns>
        public virtual T FindSingle(string condition, string orderBy, IDbDataParameter[] paramList)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }
            if (HasInjectionData(orderBy))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", orderBy));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            string sql = string.Format("Select {0} From {1} ", selectedFields, tableName);
            if (!string.IsNullOrEmpty(condition))
            {
                sql += string.Format("Where {0} ", condition);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql += " " + orderBy;
            }
            else
            {
                sql += string.Format(" Order by {0} {1}", GetSafeFileName(sortField), isDescending ? "DESC" : "ASC");
            }

            #region 获取单条记录
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            if (paramList != null)
            {
                command.Parameters.AddRange(paramList);
            }
            
            T entity = null;
            using (IDataReader dr = db.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = DataReaderToEntity(dr);
                }
            }
            return entity;
            #endregion
        }

        /// <summary>
        /// 查找记录表中最旧的一条记录
        /// </summary>
        /// <returns></returns>
        public virtual T FindFirst()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 查找记录表中最新的一条记录
        /// </summary>
        /// <returns></returns>
        public virtual T FindLast()
        {
            throw new NotSupportedException();
        } 

        #endregion

		#region 返回集合的接口
		
		/// <summary>
		/// 根据ID字符串(逗号分隔)获取对象列表
		/// </summary>
		/// <param name="idString">ID字符串(逗号分隔)</param>
		/// <returns>符合条件的对象列表</returns>
        public virtual List<T> FindByIDs(string idString)
		{
			string condition = string.Format("{0} in({1})", primaryKey, idString);
			return this.Find(condition);
		}		
		
		/// <summary>
		/// 根据条件查询数据库,并返回对象集合
		/// </summary>
		/// <param name="condition">查询的条件</param>
		/// <returns>指定对象的集合</returns>
        public virtual List<T> Find(string condition)
		{
            return Find(condition, null);
		}

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns>指定对象的集合</returns>
        public virtual List<T> Find(string condition, string orderBy)
        {
            return Find(condition, orderBy, null);
        }

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <param name="paramList">参数列表</param>
        /// <returns>指定对象的集合</returns>
        public virtual List<T> Find(string condition, string orderBy, IDbDataParameter[] paramList)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            //串连条件语句为一个完整的Sql语句
            string sql = string.Format("Select {0} From {1} ", selectedFields, tableName);
            if (!string.IsNullOrEmpty(condition))
            {
                sql += string.Format("Where {0}", condition);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql += " " + orderBy;
            }
            else
            {
                sql += string.Format(" Order by {0} {1}", GetSafeFileName(sortField), isDescending ? "DESC" : "ASC");
            }

            List<T> list = GetList(sql, paramList);
            return list;
        }

        /// <summary>
        /// 通用获取集合对象方法
        /// </summary>
        /// <param name="sql">查询的Sql语句</param>
        /// <param name="paramList">参数列表，如果没有则为null</param>
        /// <returns></returns>
        public virtual List<T> GetList(string sql, IDbDataParameter[] paramList)
        {
            T entity = null;
            List<T> list = new List<T>();

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            if (paramList != null)
            {
                command.Parameters.AddRange(paramList);
            }

            using (IDataReader dr = db.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = DataReaderToEntity(dr);

                    list.Add(entity);
                }
            }
            return list;
        }

		
		/// <summary>
		/// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
		/// </summary>
		/// <param name="condition">查询的条件</param>
		/// <param name="info">分页实体</param>
		/// <returns>指定对象的集合</returns>
        public virtual List<T> FindWithPager(string condition, PagerInfo info)
		{
            return FindWithPager(condition, info, this.sortField);
        }
        		
        /// <summary>
		/// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
		/// </summary>
		/// <param name="condition">查询的条件</param>
		/// <param name="info">分页实体</param>
        /// <param name="fieldToSort">排序字段</param>
		/// <returns>指定对象的集合</returns>
        public virtual List<T> FindWithPager(string condition, PagerInfo info, string fieldToSort)
        {
            return FindWithPager(condition, info, this.sortField, this.isDescending);
		}
             
        /// <summary>
		/// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
		/// </summary>
		/// <param name="condition">查询的条件</param>
		/// <param name="info">分页实体</param>
        /// <param name="fieldToSort">排序字段</param>
        /// <param name="desc">是否降序</param>
        /// <returns>指定对象的集合</returns>
        public virtual List<T> FindWithPager(string condition, PagerInfo info, string fieldToSort, bool desc)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            List<T> list = new List<T>();

            PagerHelper helper = new PagerHelper(tableName, this.selectedFields, fieldToSort,
                info.PageSize, info.CurrenetPageIndex, desc, condition);

            string countSql = helper.GetPagingSql(true);
            string strCount = SqlValueList(countSql);
            info.RecordCount = Convert.ToInt32(strCount);

            string dataSql = helper.GetPagingSql(false);
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
		/// 返回数据库所有的对象集合
		/// </summary>
		/// <returns>指定对象的集合</returns>
        public virtual List<T> GetAll()
		{
            return GetAll("");
		}
                
        /// <summary>
		/// 返回数据库所有的对象集合
		/// </summary>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <returns>指定对象的集合</returns>
        public virtual List<T> GetAll(string orderBy)
        {
            if (HasInjectionData(orderBy))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", orderBy));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            string sql = string.Format("Select {0} From {1} ", selectedFields, tableName);
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql += orderBy;
            }
            else
            {
                sql += string.Format(" Order by {0} {1}", GetSafeFileName(sortField), isDescending ? "DESC" : "ASC");
            }

            List<T> list = GetList(sql, null);
            return list;
        }

        /// <summary>
		/// 返回数据库所有的对象集合(用于分页数据显示)
		/// </summary>
        /// <param name="info">分页实体信息</param>
        /// <returns>指定对象的集合</returns>
        public virtual List<T> GetAll(PagerInfo info)
        {
            return FindWithPager("", info, this.sortField, this.isDescending);
        }
		
		/// <summary>
		/// 返回数据库所有的对象集合(用于分页数据显示)
		/// </summary>
        /// <param name="info">分页实体信息</param>
        /// <param name="fieldToSort">排序字段</param>
        /// <param name="desc">是否降序</param>
        /// <returns>指定对象的集合</returns>
        public virtual List<T> GetAll(PagerInfo info, string fieldToSort, bool desc)
		{
            return FindWithPager("", info, fieldToSort, desc);
		}


        /// <summary>
        /// 返回所有记录到DataTable集合中
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetAllToDataTable()
        {
            return GetAllToDataTable("");
        }

        /// <summary>
        /// 返回所有记录到DataTable集合中
        /// </summary>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <returns></returns>
        public virtual DataTable GetAllToDataTable(string orderBy)
        {
            if (HasInjectionData(orderBy))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", orderBy));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            string sql = string.Format("Select {0} From {1} ", selectedFields, tableName);
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql += orderBy;
            }
            else
            {
                sql += string.Format(" Order by {0} {1}", GetSafeFileName(sortField), isDescending ? "DESC" : "ASC");
            }

            return GetDataTableBySql(sql);
        }
                 
        /// <summary>
        /// 根据分页条件，返回DataTable对象
        /// </summary>
        /// <param name="info">分页条件</param>
        /// <returns></returns>
        public virtual DataTable GetAllToDataTable(PagerInfo info)
        {
            return FindToDataTable("", info, this.sortField, this.isDescending);
        }

        /// <summary>
        /// 根据分页条件，返回DataTable对象
        /// </summary>
        /// <param name="info">分页条件</param>
        /// <param name="fieldToSort">排序字段</param>
        /// <param name="desc">是否降序</param>
        /// <returns></returns>
        public virtual DataTable GetAllToDataTable(PagerInfo info, string fieldToSort, bool desc)
        {
            return FindToDataTable("", info, fieldToSort, desc);
        }


        /// <summary>
        /// 根据查询条件，返回记录到DataTable集合中
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public virtual DataTable FindToDataTable(string condition)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            //串连条件语句为一个完整的Sql语句
            string sql = string.Format("Select {0} From {1} ", selectedFields, tableName);
            if (!string.IsNullOrEmpty(condition))
            {
                sql += string.Format("Where {0}", condition);
            }
            sql += string.Format(" Order by {0} {1}", GetSafeFileName(sortField), isDescending ? "DESC" : "ASC");

            return GetDataTableBySql(sql);
        }
                
        /// <summary>
        /// 根据查询条件，返回记录到DataTable集合中
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="info">分页条件</param>
        /// <returns></returns>
        public virtual DataTable FindToDataTable(string condition, PagerInfo info)
        {
            return FindToDataTable(condition, info, this.sortField, this.isDescending);
        }

        /// <summary>
        /// 根据条件查询数据库,并返回DataTable集合(用于分页数据显示)
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="info">分页实体</param>
        /// <param name="fieldToSort">排序字段</param>
        /// <param name="desc">是否降序</param>
        /// <returns>指定DataTable的集合</returns>
        public virtual DataTable FindToDataTable(string condition, PagerInfo info, string fieldToSort, bool desc)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            PagerHelper helper = new PagerHelper(tableName, this.selectedFields, fieldToSort,
                info.PageSize, info.CurrenetPageIndex, desc, condition);

            string countSql = helper.GetPagingSql(true);
            string strCount = SqlValueList(countSql);
            info.RecordCount = Convert.ToInt32(strCount);

            string dataSql = helper.GetPagingSql(false);
            return GetDataTableBySql(dataSql);
        }

        /// <summary>
        /// 操根据条件返回DataTable记录辅助类
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected DataTable GetDataTableBySql(string sql)
        {
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(command).Tables[0];
        }

        /// <summary>
        /// 获取某字段数据字典列表
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public virtual List<string> GetFieldList(string fieldName)
        {
            string sql = string.Format("Select distinct {0} From {1} order by {0}", GetSafeFileName(fieldName), tableName);

            List<string> list = new List<string>();

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            string number = string.Empty;
            using (IDataReader dr = db.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    number = dr[fieldName].ToString();
                    if (!string.IsNullOrEmpty(number))
                    {
                        list.Add(number);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 根据条件，从视图里面获取记录
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataTable FindByView(string viewName, string condition)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            //串连条件语句为一个完整的Sql语句
            string sql = string.Format("Select * From {0} Where ", viewName);
            sql += condition;
            //sql += string.Format(" Order by {0} {1}", SortField, IsDescending ? "DESC" : "ASC");

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            DataTable dt = db.ExecuteDataSet(command).Tables[0];
            return dt;
        }
              
        /// <summary>
        /// 根据条件，从视图里面获取记录
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="condition">查询条件</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="isDescending">是否为降序</param>
        /// <returns></returns>
        public DataTable FindByView(string viewName, string condition, string sortField, bool isDescending)
        {
            //串连条件语句为一个完整的Sql语句
            string sql = string.Format("Select * From {0} Where ", viewName);
            sql += condition;
            sql += string.Format(" Order by {0} {1}", sortField, isDescending ? "DESC" : "ASC");

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            DataTable dt = db.ExecuteDataSet(command).Tables[0];
            return dt;
        }

        /// <summary>
        /// 获取前面记录指定数量的记录
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="count">指定数量</param>
        /// <param name="orderBy">排序条件，例如order by id</param>
        /// <returns></returns>
        public abstract DataTable GetTopResult(string sql, int count, string orderBy);

        /// <summary>
        /// 根据条件，从视图里面获取记录
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="condition">查询条件</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="isDescending">是否为降序</param>
        /// <param name="info">分页条件</param>
        /// <returns></returns>
        public DataTable FindByViewWithPager(string viewName, string condition, string sortField, bool isDescending, PagerInfo info)
        {
            //从视图中获取数据
            PagerHelper helper = new PagerHelper(viewName, "*", sortField,
                            info.PageSize, info.CurrenetPageIndex, isDescending, condition);
            string countSql = helper.GetPagingSql(true);
            string strCount = SqlValueList(countSql);
            info.RecordCount = Convert.ToInt32(strCount);

            string dataSql = helper.GetPagingSql(false);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(dataSql);
            return db.ExecuteDataSet(command).Tables[0];
        }

		#endregion
		
		#region 子类必须实现的函数(用于更新或者插入)

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// (提供了默认的反射机制获取信息，为了提高性能，建议重写该函数)
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
        protected virtual T DataReaderToEntity(IDataReader dr)
        {
            T obj = new T();
            PropertyInfo[] pis = obj.GetType().GetProperties();

            foreach (PropertyInfo pi in pis)
            {
                try
                {
                    if (dr[pi.Name].ToString() != "")
                    {
                        pi.SetValue(obj, dr[pi.Name] ?? "", null);
                    }
                }
                catch { }
            }
            return obj;
        }

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值(用于插入或者更新操作)
        /// (提供了默认的反射机制获取信息，为了提高性能，建议重写该函数)
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected virtual Hashtable GetHashByEntity(T obj)
        {
            Hashtable ht = new Hashtable();
            PropertyInfo[] pis = obj.GetType().GetProperties();
            for (int i = 0; i < pis.Length; i++)
            {
                //if (pis[i].Name != PrimaryKey)
                {
                    object objValue = pis[i].GetValue(obj, null);
                    objValue = (objValue == null) ? DBNull.Value : objValue;

                    if (!ht.ContainsKey(pis[i].Name))
                    {
                        ht.Add(pis[i].Name, objValue);

                        EntityTypeHash.Add(pis[i].Name, pis[i].GetType());
                    }                    
                }
            }
            return ht;
        }

        private Hashtable EntityTypeHash = new Hashtable();

		#endregion
		
		#region IBaseDAL接口

        /// <summary>
        /// 获取表的所有记录数量
        /// </summary>
        /// <returns></returns>
        public virtual int GetRecordCount(string condition)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            string sql = string.Format("Select Count(*) from {0} WHERE {1} ", tableName, condition);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            return Convert.ToInt32(db.ExecuteScalar(command));
        }

        /// <summary>
        /// 获取表的所有记录数量
        /// </summary>
        /// <returns></returns>
        public virtual int GetRecordCount()
        {
            string sql = string.Format("Select Count(*) from {0} ", tableName);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            return Convert.ToInt32(db.ExecuteScalar(command));
        }

        /// <summary>
        /// 根据condition条件，判断是否存在记录
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <returns>如果存在返回True，否则False</returns>
        public virtual bool IsExistRecord(string condition)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            string sql = string.Format("Select Count(*) from {0} WHERE {1} ", tableName, condition);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            return Convert.ToInt32(db.ExecuteScalar(command)) > 0;
        }

        /// <summary>
        /// 查询数据库,检查是否存在指定键值的对象
        /// </summary>
        /// <param name="recordTable">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
        /// <returns>存在则返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool IsExistKey(Hashtable recordTable)
        {
            string fields = "";// 字段名
            foreach (string field in recordTable.Keys)
            {
                fields += string.Format(" {0} = {1}{2} AND", GetSafeFileName(field), parameterPrefix, field);
            }
            fields = fields.Substring(0, fields.Length - 3);//除去最后的AND

            string sql = string.Format("SELECT COUNT(*) FROM {0} WHERE {1}", tableName, fields);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            foreach (string field in recordTable.Keys)
            {
                object objValue = recordTable[field];
                db.AddInParameter(command, field, TypeToDbType(objValue.GetType()), objValue);
            }

            return Convert.ToInt32(db.ExecuteScalar(command)) > 0;
        }

        /// <summary>
        /// 查询数据库,检查是否存在指定键值的对象
        /// </summary>
        /// <param name="fieldName">指定的属性名</param>
        /// <param name="key">指定的值</param>
        /// <returns>存在则返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool IsExistKey(string fieldName, object key)
        {
            Hashtable table = new Hashtable();
            table.Add(fieldName, key);

            return IsExistKey(table);
        }
		
		/// <summary>
		/// 获取数据库中该对象的最大ID值
		/// </summary>
		/// <returns>最大ID值</returns>
        public virtual int GetMaxID()
		{
			string sql = string.Format("SELECT MAX({0}) AS MaxID FROM {1}", primaryKey, tableName);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

			object obj = db.ExecuteScalar(command);
			if(Convert.IsDBNull(obj))
			{
				return 0;//没有记录的时候为0
			}
			return Convert.ToInt32(obj);
		}

        /// <summary>
        /// 根据指定对象的ID,从数据库中删除指定对象
        /// </summary>
        /// <param name="key">指定对象的ID</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool Delete(object key)
        {
            return Delete(key, null);
        }	

        /// <summary>
		/// 根据指定对象的ID,从数据库中删除指定对象
		/// </summary>
		/// <param name="key">指定对象的ID</param>
        /// <param name="trans">事务对象</param>
		/// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool Delete(object key, DbTransaction trans)
        {
            string condition = string.Format("{0} = {1}{0}", primaryKey, parameterPrefix);
            string sql = string.Format("DELETE FROM {0} WHERE {1} ", tableName, condition);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            db.AddInParameter(command, primaryKey, TypeToDbType(key.GetType()), key);

            bool result = false;
            if (trans != null)
            {
                result = db.ExecuteNonQuery(command, trans) > 0;
            }
            else
            {
                result = db.ExecuteNonQuery(command) > 0;
            }
            return result;
		}				
		
		/// <summary>
		/// 根据指定条件,从数据库中删除指定对象
		/// </summary>
		/// <param name="condition">删除记录的条件语句</param>
		/// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool DeleteByCondition(string condition)
		{
            return DeleteByCondition(condition, null);
		}

		/// <summary>
		/// 根据指定条件,从数据库中删除指定对象
		/// </summary>
		/// <param name="condition">删除记录的条件语句</param>
        /// <param name="trans">事务对象</param>
		/// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool DeleteByCondition(string condition, DbTransaction trans)
		{
			return DeleteByCondition(condition, trans, null);
		} 
		
		/// <summary>
        /// 根据指定条件,从数据库中删除指定对象
        /// </summary>
        /// <param name="condition">删除记录的条件语句</param>
        /// <param name="trans">事务对象</param>
        /// <param name="paramList">Sql参数列表</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool DeleteByCondition(string condition, DbTransaction trans, IDbDataParameter[] paramList)
        {
            if (HasInjectionData(condition))
            {
                LogTextHelper.Error(string.Format("检测出SQL注入的恶意数据, {0}", condition));
                throw new Exception("检测出SQL注入的恶意数据");
            }

            string sql = string.Format("DELETE FROM {0} WHERE {1} ", tableName, condition);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            if(paramList != null)
            {
				command.Parameters.AddRange(paramList);
			}

            bool result = false;
            if (trans != null)
            {
                result = db.ExecuteNonQuery(command, trans) > 0;
            }
            else
            {
                result = db.ExecuteNonQuery(command) > 0;
            }

            return result;
        }
               		
		#endregion

        /// <summary>
        /// 转换.NET的对象类型到数据库类型
        /// </summary>
        /// <param name="t">.NET的对象类型</param>
        /// <returns></returns>
        public virtual DbType TypeToDbType(Type t)
        {
            DbType dbt;
            try
            {
                if (t.Name.ToLower() == "byte[]")
                {
                    dbt = DbType.Binary;
                }
                else
                {
                    dbt = (DbType)Enum.Parse(typeof(DbType), t.Name);
                }
            }
            catch
            {
                dbt = DbType.String;
            }
            return dbt;
        }

        /// <summary>
        /// 初始化数据库表名
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        public virtual void InitTableName(string tableName)
        {
            this.tableName = tableName;
        }

        /// <summary>
        /// 验证是否存在注入代码(条件语句）
        /// </summary>
        /// <param name="inputData"></param>
        public virtual bool HasInjectionData(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
                return false;

            //里面定义恶意字符集合
            //验证inputData是否包含恶意集合
            if (Regex.IsMatch(inputData.ToLower(), GetRegexString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取正则表达式
        /// </summary>
        /// <returns></returns>
        private string GetRegexString()
        {
            //构造SQL的注入关键字符
            string[] strBadChar =
            {
                //"select\\s",
                //"from\\s",
                "insert\\s",
                "delete\\s",
                "update\\s",
                "drop\\s",
                "truncate\\s",
                "exec\\s",
                "count\\(",
                "declare\\s",
				"asc\\(",
				"mid\\(",
				"char\\(",
                "net user",
                "xp_cmdshell",
                "/add\\s",
                "exec master.dbo.xp_cmdshell",
                "net localgroup administrators"
            };

            //构造正则表达式
            string str_Regex = ".*(";
            for (int i = 0; i < strBadChar.Length - 1; i++)
            {
                str_Regex += strBadChar[i] + "|";
            }
            str_Regex += strBadChar[strBadChar.Length - 1] + ").*";

            return str_Regex;
        }

        /// <summary>
        /// 获取表的字段名称和数据类型列表。
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetFieldTypeList()
        {
            DataTable dt = DataTableHelper.CreateTable("ColumnName,DataType");
            DataTable schemaTable = GetReaderSchema(tableName);
            if (schemaTable != null)
            {
                foreach (DataRow dr in schemaTable.Rows)
                {
                    string columnName = dr["ColumnName"].ToString().ToUpper();
                    string netType = dr["DataType"].ToString().ToLower();

                    DataRow row = dt.NewRow();
                    row["ColumnName"] = columnName;
                    row["DataType"] = netType;

                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取指定表的元数据，包括字段名称、类型等等
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        /// <returns></returns>
        private DataTable GetReaderSchema(string tableName)
        {
            DataTable schemaTable = null;

            string sql = string.Format("Select * FROM {0}", tableName);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            try
            {
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    schemaTable = reader.GetSchemaTable();
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);
            }

            return schemaTable;
        }

        /// <summary>
        /// 获取字段中文别名（用于界面显示）的字典集合
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, string> GetColumnNameAlias()
        {
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// 获取指定字段的报表数据
        /// </summary>
        /// <param name="fieldName">表字段</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public virtual DataTable GetReportData(string fieldName, string condition)
        {
            string where = "";
            if (!string.IsNullOrEmpty(condition))
            {
                where = string.Format("Where {0}", condition);
            }
            string sql = string.Format("select {0} as argument, count(*) as datavalue from {1} {2} group by {0} order by count(*) desc", fieldName, tableName, where);

            return SqlTable(sql);
        }
	}
}