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
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SCZY.Comments;
using SCZY.FramWork;

namespace SCZY.FramWorkf
{
    public abstract class AbstractBaseDAL<T> where T : BaseEntity,new()
    {
        #region 构造函数

        //protected string dbConfigName = "server=210.44.125.58; Database=SCZY;user id=yhq; password=yhq123321";//数据库配置名称

        protected string dbConfigName = ConfigurationManager.ConnectionStrings["SCZYEntities1"].ConnectionString;
        protected string constring = ConfigurationManager.ConnectionStrings["SCZYEntities1"].ConnectionString;
        protected string parameterPrefix = "@";//数据库参数化访问的占位符
        protected string tableName;//需要初始化的对象表名
        protected string sortField;//排序字段
        protected string safeFieldFormat = "[{0}]";//防止和保留字、关键字同名的字段格式，如[value]
        protected bool isDescending = true;//是否为降序
        protected string primaryKey;//数据库的主键字段名
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
        /// 默认构造函数
        /// </summary>
        public AbstractBaseDAL()
        { }

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

      

        ///// <summary>
        ///// 添加记录
        ///// </summary>
        ///// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
        ///// <param name="targetTable">需要操作的目标表名称</param>
        ///// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        //public virtual bool Insert(Hashtable recordField, string targetTable, DbTransaction trans)
        //{
        //    bool result = false;
        //    if (recordField == null || recordField.Count < 1)
        //    {
        //        return result;
        //    }

        //    SqlConnection conn = new SqlConnection(dbConfigName);

        //    conn.Open();

        //    SqlCommand sqlcmd = new SqlCommand();
        //   // string sql = string.Format("insert into {0} ({1}) values {2}",targetTable,fields,vals);
        //    sqlcmd.CommandText = "select * from Product";
        //    sqlcmd.Connection = conn;
        //    SqlDataAdapter adp = new SqlDataAdapter(sqlcmd);
        //    DataTable dt = new DataTable();
        //    var str = adp.Fill(dt);

        //    SqlDataReader reader = sqlcmd.ExecuteReader(CommandBehavior.SingleRow);
        //    return true;

        //}
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

                //db = DatabaseFactory.CreateDatabase(dbConfigName);
                db = DatabaseFactory.CreateDatabase("SCZYEntities1");
            }
            return db;
        }
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
        /// 生成防止和保留字、关键字同名的字段格式，如[value]。
        /// </summary>
        /// <param name="fieldName">字段名，如value</param>
        protected string GetSafeFileName(string fieldName)
        {
            return string.Format(safeFieldFormat, fieldName);
        }


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


        #endregion



        /// <summary>
        /// 插入指定对象到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <returns>执行成功返回新增记录的自增长ID。</returns>
        public virtual bool Insert(T obj)
        {
            var k = obj;
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
    }
}
