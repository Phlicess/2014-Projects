using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.Pager.Entity;
using WHC.Framework.Commons;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;

namespace WHC.MVCWebMis.DALSQL
{
    /// <summary>
    /// 功能菜单
    /// </summary>
	public class Menu :  WHC.Framework.ControlUtil.BaseDALSQL<MenuInfo>, IMenu
	{
		#region 对象实例及构造函数

		public static Menu Instance
		{
			get
			{
				return new Menu();
			}
		}
		public Menu() : base("T_ACL_Menu","ID")
        {
            this.sortField = "Seq";
            this.isDescending = false;
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override MenuInfo DataReaderToEntity(IDataReader dataReader)
		{
			MenuInfo info = new MenuInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.PID = reader.GetString("PID");
			info.Name = reader.GetString("Name");
			info.Icon = reader.GetString("Icon");
			info.Seq = reader.GetString("Seq");
			info.FunctionId = reader.GetString("FunctionId");
			info.Visible = reader.GetInt32("Visible") > 0;
			info.WinformType = reader.GetString("WinformType");
            info.Url = reader.GetString("Url");
            info.WebIcon = reader.GetString("WebIcon");
            info.SystemType_ID = reader.GetString("SystemType_ID");

			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(MenuInfo obj)
		{
		    MenuInfo info = obj as MenuInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("PID", info.PID);
 			hash.Add("Name", info.Name);
 			hash.Add("Icon", info.Icon);
 			hash.Add("Seq", info.Seq);
 			hash.Add("FunctionId", info.FunctionId);
 			hash.Add("Visible", info.Visible ? 1 : 0);
 			hash.Add("WinformType", info.WinformType);
            hash.Add("Url", info.Url);
            hash.Add("WebIcon", info.WebIcon);
            hash.Add("SystemType_ID", info.SystemType_ID);
		
			return hash;
		}

        /// <summary>
        /// 获取树形结构的菜单列表
        /// </summary>
        public List<MenuNodeInfo> GetTree(string systemType)
        {
            string condition = !string.IsNullOrEmpty(systemType) ? string.Format("AND SystemType_ID='{0}'", systemType) : "";
            List<MenuNodeInfo> arrReturn = new List<MenuNodeInfo>();
            string sql = string.Format("Select * From {0} Where Visible > 0 {1} Order By PID, Seq ", tableName, condition);

            DataTable dt = base.SqlTable(sql);
            DataRow[] dataRows = dt.Select(string.Format(" PID = '{0}' ", -1));
            for (int i = 0; i < dataRows.Length; i++)
            {
                string id = dataRows[i]["ID"].ToString();
                MenuNodeInfo menuNodeInfo = GetNode(id, dt);
                arrReturn.Add(menuNodeInfo);
            }

            return arrReturn;
        }

        /// <summary>
        /// 获取所有的菜单列表
        /// </summary>
        public List<MenuInfo> GetAllMenu(string systemType)
        {
            string condition = !string.IsNullOrEmpty(systemType) ? string.Format("Where SystemType_ID='{0}'", systemType) : "";
            string sql = string.Format("Select * From {0} {1} Order  By PID, Seq  ", tableName, condition);
            return GetList(sql, null);
        }

        private MenuNodeInfo GetNode(string id, DataTable dt)
        {
            MenuInfo menuInfo = this.FindByID(id);
            MenuNodeInfo menuNodeInfo = new MenuNodeInfo(menuInfo);

            DataRow[] dChildRows = dt.Select(string.Format(" PID='{0}'", id));

            for (int i = 0; i < dChildRows.Length; i++)
            {
                string childId = dChildRows[i]["ID"].ToString();
                MenuNodeInfo childNodeInfo = GetNode(childId, dt);
                menuNodeInfo.Children.Add(childNodeInfo);
            }
            return menuNodeInfo;
        }

        /// <summary>
        /// 获取第一级的菜单列表
        /// </summary>
        public List<MenuInfo> GetTopMenu(string systemType)
        {
            string condition = !string.IsNullOrEmpty(systemType) ? string.Format("AND SystemType_ID='{0}'", systemType) : "";
            string sql = string.Format("Select * From {0} Where Visible > 0 and PID='-1' {1} Order By Seq  ", tableName, condition);
            return GetList(sql, null);
        }

        /// <summary>
        /// 获取指定菜单下面的树形列表
        /// </summary>
        /// <param name="id">指定菜单ID</param>
        public List<MenuNodeInfo> GetTreeByID(string mainMenuID)
        {
            List<MenuNodeInfo> arrReturn = new List<MenuNodeInfo>();
            string sql = string.Format("Select * From {0} Where Visible > 0 Order By PID, Seq ", tableName);

            DataTable dt = SqlTable(sql);
            DataRow[] dataRows = dt.Select(string.Format(" PID = '{0}'", mainMenuID));
            for (int i = 0; i < dataRows.Length; i++)
            {
                string id = dataRows[i]["ID"].ToString();
                MenuNodeInfo menuNodeInfo = GetNode(id, dt);
                arrReturn.Add(menuNodeInfo);
            }

            return arrReturn;
        }

        /// <summary>
        /// 根据指定的父ID获取其下面一级（仅限一级）的菜单列表
        /// </summary>
        /// <param name="pid">菜单父ID</param>
        public List<MenuInfo> GetMenuByID(string PID)
        {
            string sql = string.Format(@"Select t.*,case pid when '-1' then '0' else pid end as parentId From {1} t 
                                         Where  PID='{0}' Order By Seq ", PID, tableName);
            return GetList(sql, null);
        }
    }
}