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
    /// 项目学生
    /// </summary>
	public class XiangMuXueSheng : BaseDALSQL<XiangMuXueShengInfo>, IXiangMuXueSheng
	{
		#region 对象实例及构造函数

		public static XiangMuXueSheng Instance
		{
			get
			{
				return new XiangMuXueSheng();
			}
		}
		public XiangMuXueSheng() : base("XiangMuXueSheng","XiangMu_Id")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override XiangMuXueShengInfo DataReaderToEntity(IDataReader dataReader)
		{
			XiangMuXueShengInfo info = new XiangMuXueShengInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.XiangMu_Id = reader.GetInt32("XiangMu_Id");
			info.XueSheng_Id = reader.GetInt32("XueSheng_Id");
			info.WeiCi = reader.GetInt32("WeiCi");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(XiangMuXueShengInfo obj)
		{
		    XiangMuXueShengInfo info = obj as XiangMuXueShengInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("XiangMu_Id", info.XiangMu_Id);
 			hash.Add("XueSheng_Id", info.XueSheng_Id);
 			hash.Add("WeiCi", info.WeiCi);
 				
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
            dict.Add("XiangMu_Id", "项目编号");
             dict.Add("XueSheng_Id", "学生编号");
             dict.Add("WeiCi", "位次");
             #endregion

            return dict;
        }

    }
}