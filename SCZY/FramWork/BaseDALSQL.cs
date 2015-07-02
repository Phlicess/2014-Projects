using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SCZY.Comments;
using SCZY.FramWorkf;

namespace SCZY.FramWork
{
    public abstract class BaseDALSQL<T> : AbstractBaseDAL<T>, IBaseDAL<T> where T : BaseEntity, new()
    {
        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseDALSQL()
        {
        }

        /// <summary>
        /// 指定表名以及主键,对基类进构造
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">表主键</param>
        public BaseDALSQL(string tableName, string primaryKey)
            : base(tableName, primaryKey)
        {
            parameterPrefix = "@";     //数据库参数化访问的占位符
            safeFieldFormat = "[{0}]"; //防止和保留字、关键字同名的字段格式，如[value]
        }


        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
        /// <param name="targetTable">需要操作的目标表名称</param>
        /// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        public override bool Insert(T obj)
        {
            string targetTable = null;
            DbTransaction trans = null;


            if (!tableName.Contains("["))
            {
                tableName = "[" + tableName + "]";
            }       
            
            Hashtable recordField = GetHashByEntity(obj);
            int result = -1;
            if (recordField == null || recordField.Count < 1)
            {
                return true;
            }

            string fields = ""; // 字段名
            string vals = ""; // 字段值
            foreach (string field in recordField.Keys)
            {

                if (field != "ID")
                {
                    fields += string.Format("{0},", GetSafeFileName(field));
                    vals += string.Format("{0}{1},", parameterPrefix, field);
                }                                                               
            }
            fields = fields.Trim(',');//除去前后的逗号
            //fields = fields.Replace("[","");//除去前后的逗号
            //fields = fields.Replace("]", "");//除去前后的逗号

            vals = vals.Trim(',');//除去前后的逗号
            string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, fields, vals);
            //sql =
            //    "INSERT INTO User ([Role],[Name],[UserName],[PassWord]) VALUES ('sadf','afaf','af','asdfaf')";
        //     sql =
        //"INSERT INTO User (Name) VALUES (@Name); "
        //+ "SELECT CAST(scope_identity() AS int)";
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
                result = Convert.ToInt32(db.ExecuteScalar(command, trans));
            }
            else
            {
                db.ExecuteScalar(command);
            }

            return true;
       
          
        }


        /// <summary>
        /// 更新某个表一条记录(只适用于用单键,用int类型作键值的表)
        /// </summary>
        /// <param name="id">ID值</param>
        /// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
        /// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        public virtual bool Update(T obj, object id)
        {
            try
            {
                //与系统表冲突
                if (!tableName.Contains("["))
                {
                    tableName = "[" + tableName + "]";
                }


                if (obj == null || id == null)
                {
                    return false;
                }

                Hashtable recordFields = GetHashByEntity(obj);
                DbTransaction trans = null;

                string setValue = "";

                foreach (string field in recordFields.Keys)
                {
                    if (field != "ID")
                    {
                        setValue += string.Format("{0} = {1}{2},", GetSafeFileName(field), parameterPrefix, field);
                    }
                }

                string sql = string.Format("UPDATE {0} SET {1} WHERE {2} = {3}{2}", tableName,
                    setValue.Substring(0, setValue.Length - 1), primaryKey, parameterPrefix);
                Database db = CreateDatabase();

                DbCommand command = db.GetSqlStringCommand(sql);

                bool foundID = false;

                foreach (string field in recordFields.Keys)
                {
                    object val = recordFields[field];
                    val = val ?? DBNull.Value;
                    if (val is DateTime)
                    {
                        if (Convert.ToDateTime(val) <= Convert.ToDateTime("1753-1-1"))
                        {
                            val = DBNull.Value;
                        }
                        db.AddInParameter(command, field, DbType.DateTime, val);
                    }
                    else if (field == "ID")
                    {
                        db.AddInParameter(command, field, TypeToDbType(val.GetType()), id);
                    }
                    else
                    {
                        db.AddInParameter(command,field,TypeToDbType(val.GetType()),val);
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
        /// 提供对FindByID的私有方法实现
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T FindByID(object key)
        {
         
            if (!tableName.Contains("["))
            {
                tableName = "[" + tableName + "]";
            }
                
            string sql = string.Format("Select {0} From {1} Where ({2} = {3}ID)", selectedFields, tableName, primaryKey, parameterPrefix);
          //  sql = "select * from [User] where ID = 1";
            //SqlDataReader reader = sqlcmd.ExecuteReader(CommandBehavior.SingleRow);
            Database db = CreateDatabase(); 
            DbCommand command = db.GetSqlStringCommand(sql);
            db.AddInParameter(command, "ID", DbType.String, key);
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

            if (!tableName.Contains("["))
            {
                tableName = "[" + tableName + "]";
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


            if (!tableName.Contains("["))
            {
                tableName = "[" + tableName + "]";
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
        #endregion

    }
}
