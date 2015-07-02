using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.Framework.Commons;

namespace WHC.MVCWebMis.DALSQL
{
    public class User :  WHC.Framework.ControlUtil.BaseDALSQL<UserInfo>, IUser
    {
        #region 对象实例及构造函数

        public static User Instance
        {
            get
            {
                return new User();
            }
        }
        public User()
            : base("T_ACL_User", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override UserInfo DataReaderToEntity(IDataReader dataReader)
        {
            UserInfo info = new UserInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetInt32("ID");
            info.PID = reader.GetInt32("PID");
            info.Name = reader.GetString("Name");
            info.Password = reader.GetString("Password");
            info.FullName = reader.GetString("FullName");
            info.IsExpire = reader.GetBoolean("IsExpire");
            info.Title = reader.GetString("Title");
            info.IdentityCard = reader.GetString("IdentityCard");
            info.MobilePhone = reader.GetString("MobilePhone");
            info.OfficePhone = reader.GetString("OfficePhone");
            info.HomePhone = reader.GetString("HomePhone");
            info.Email = reader.GetString("Email");
            info.Address = reader.GetString("Address");
            info.WorkAddr = reader.GetString("WorkAddr");
            info.Birthday = reader.GetDateTime("Birthday");
            info.Qq = reader.GetString("QQ");
            //info.Portrait = reader.GetBytes("Portrait");
            info.Note = reader.GetString("Note");
            info.CustomField = reader.GetString("CustomField");
            info.Dept_ID = reader.GetString("Dept_ID");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(UserInfo obj)
        {
            UserInfo info = obj as UserInfo;
            Hashtable hash = new Hashtable();

            hash.Add("PID", info.PID);
            hash.Add("Name", info.Name);
            hash.Add("Password", info.Password);
            hash.Add("FullName", info.FullName);
            hash.Add("IsExpire", info.IsExpire);
            hash.Add("Title", info.Title);
            hash.Add("IdentityCard", info.IdentityCard);
            hash.Add("MobilePhone", info.MobilePhone);
            hash.Add("OfficePhone", info.OfficePhone);
            hash.Add("HomePhone", info.HomePhone);
            hash.Add("Email", info.Email);
            hash.Add("Address", info.Address);
            hash.Add("WorkAddr", info.WorkAddr);
            hash.Add("Birthday", info.Birthday);
            hash.Add("QQ", info.Qq);
            //hash.Add("Portrait", info.Portrait);
            hash.Add("Note", info.Note);
            hash.Add("CustomField", info.CustomField);
            hash.Add("Dept_ID", info.Dept_ID);

            return hash;
        }

        /// <summary>
        /// 重写删除操作，删除关联的信息
        /// </summary>
        /// <param name="key">ID值</param>
        /// <returns></returns>
		public override bool  Delete(object key)
		{
            Database db = CreateDatabase();
            DbCommand command = null;

            DbTransaction transaction = null;
			try
			{
                UserInfo info = this.FindByID(key);
                if (info != null)
                {
                    using (DbConnection connection = db.CreateConnection())
                    {
                        connection.Open();
                        transaction = connection.BeginTransaction();

                        string commandText = string.Format("UPDATE {2} SET PID={0} Where PID={1}", info.PID, key, tableName);
                        command = db.GetSqlStringCommand(commandText);
                        db.ExecuteNonQuery(command, transaction);

                        commandText = string.Format("Delete From {1} Where ID ={0} ", key, tableName);
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

        /// <summary>
        /// 构造一个简单用户信息类集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
		private List<SimpleUserInfo> FillSimpleUsers(string sql)
		{
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            List<SimpleUserInfo> list = new List<SimpleUserInfo>();
			using (IDataReader reader = db.ExecuteReader(command))
			{
                SmartDataReader dr = new SmartDataReader(reader);
                while (reader.Read())
                {
                    SimpleUserInfo info = new SimpleUserInfo();
                    info.ID = dr.GetInt32("ID");
                    info.Name = dr.GetString("Name");
                    info.Password = dr.GetString("Password");
                    info.FullName = dr.GetString("FullName");
                    list.Add(info);
                }
			}
			return list;
		}

        public List<SimpleUserInfo> GetSimpleUsers()
		{
			string sql = "Select ID,Name,Password,FullName From " + base.TableName;
			return this.FillSimpleUsers(sql);
		}

        public List<SimpleUserInfo> GetSimpleUsers(string userIDs)
		{
			string sql = "Select ID,Name,Password,FullName From " + base.TableName + " Where ID In (" + userIDs + ")";
			return this.FillSimpleUsers(sql);
		}

        public List<SimpleUserInfo> GetSimpleUsersByOU(int ouID)
		{
            string sql = "Select ID,Name,Password,FullName From T_ACL_OU_User Inner Join [T_ACL_User] ON [T_ACL_User].ID=User_ID Where OU_ID = " + ouID;
			return this.FillSimpleUsers(sql);
		}

        public List<SimpleUserInfo> GetSimpleUsersByRole(int roleID)
		{
            string sql = "Select ID,Name,Password,FullName From [T_ACL_User] INNER JOIN T_ACL_User_Role ON [T_ACL_User].ID=User_ID Where Role_ID = " + roleID;
			return this.FillSimpleUsers(sql);
		}

        public List<UserInfo> GetUsersByOU(int ouID)
		{
            string sql = "SELECT * FROM [T_ACL_User] INNER JOIN T_ACL_OU_User On [T_ACL_User].ID=T_ACL_OU_User.User_ID WHERE OU_ID = " + ouID;
            return this.GetList(sql, null);
		}

        public List<UserInfo> GetUsersByRole(int roleID)
		{
            string sql = "SELECT * FROM [T_ACL_User] INNER JOIN T_ACL_User_Role On [T_ACL_User].ID=T_ACL_User_Role.User_ID WHERE Role_ID = " + roleID;
            return this.GetList(sql, null);
		}

        /// <summary>
        /// 根据个人图片枚举类型获取图片数据
        /// </summary>
        /// <param name="imagetype">图片枚举类型</param>
        /// <returns></returns>
        public byte[] GetPersonImageBytes(UserImageType imagetype, int userId)
        {
            string fieldName = GetFieldNameByImageType(imagetype);

            string sql = string.Format("Select {0} from {1} where Id = {2} ", fieldName, tableName, userId);
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);

            byte[] imageBytes = null;
            using (IDataReader reader = db.ExecuteReader(dbCommand))
            {
                if (reader.Read())
                {
                    imageBytes = (reader.IsDBNull(reader.GetOrdinal(fieldName))) ? null : (byte[])reader[0];
                }
            }

            return imageBytes;
        }

        /// <summary>
        /// 根据图片枚举类型获取对应的字段名称
        /// </summary>
        /// <param name="imageType">图片枚举类型</param>
        /// <returns></returns>
        private string GetFieldNameByImageType(UserImageType imageType)
        {
            string fieldName = "Portrait";
            switch (imageType)
            {
                case UserImageType.个人肖像:
                    fieldName = "Portrait";
                    break;
                case UserImageType.身份证照片1:
                    fieldName = "IDPhoto1";
                    break;
                case UserImageType.身份证照片2:
                    fieldName = "IDPhoto2";
                    break;
                case UserImageType.名片1:
                    fieldName = "BusinessCard1";
                    break;
                case UserImageType.名片2:
                    fieldName = "BusinessCard2";
                    break;
            }
            return fieldName;
        }

        /// <summary>
        /// 更新个人相关图片数据
        /// </summary>
        /// <param name="imagetype">图片类型</param>
        /// <param name="userId">用户ID</param>
        /// <param name="imageBytes">图片字节数组</param>
        /// <returns></returns>
        public bool UpdatePersonImageBytes(UserImageType imagetype, int userId, byte[] imageBytes)
        {
            string fieldName = GetFieldNameByImageType(imagetype);

            string sql = string.Format("update {0} set {1}=@image where Id = {2} ", tableName, fieldName, userId);
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(sql);
            db.AddInParameter(dbCommand, "image", DbType.Binary, imageBytes);
            return db.ExecuteNonQuery(dbCommand) > 0;
        }

	}
}