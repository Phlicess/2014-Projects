using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;

namespace WHC.MVCWebMis.DALSQL
{
    /// <summary>
    /// 学院
    /// </summary>
	public class XueYuan : BaseDALSQL<XueYuanInfo>, IXueYuan
	{
		#region 对象实例及构造函数

		public static XueYuan Instance
		{
			get
			{
				return new XueYuan();
			}
		}
		public XueYuan() : base("XueYuan","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override XueYuanInfo DataReaderToEntity(IDataReader dataReader)
		{
			XueYuanInfo info = new XueYuanInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetInt32("ID");
			info.XueYuan = reader.GetString("XueYuan");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(XueYuanInfo obj)
		{
		    XueYuanInfo info = obj as XueYuanInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("XueYuan", info.XueYuan);
 				
			return hash;
		}

        /// <summary>
        /// 获取字段中文别名（用于界面显示）的字典集合
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> GetColumnNameAlias()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            #region 添加别名解析
            //dict.Add("ID", "编号");
             dict.Add("XueYuan", "学院");
             #endregion

            return dict;
        }

        public List<XueYuanInfo> GetOUsByUser(int userID)
        {
            //todo
            //string sql = "SELECT * FROM XueYuan INNER JOIN T_ACL_User On [XueYuan].ID=T_ACL_OU_User.Dept_Id WHERE User_ID = " + userID;
            string sql = "SELECT XueYuan.* FROM XueYuan INNER JOIN T_ACL_User On [XueYuan].ID=T_ACL_User.Dept_Id WHERE T_ACL_User.ID = " + userID; 
            return this.GetList(sql, null);
        }

    }
}