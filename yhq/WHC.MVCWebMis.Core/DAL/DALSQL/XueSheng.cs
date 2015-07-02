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
    /// 学生
    /// </summary>
	public class XueSheng : BaseDALSQL<XueShengInfo>, IXueSheng
	{
		#region 对象实例及构造函数

		public static XueSheng Instance
		{
			get
			{
				return new XueSheng();
			}
		}
		public XueSheng() : base("XueSheng","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override XueShengInfo DataReaderToEntity(IDataReader dataReader)
		{
			XueShengInfo info = new XueShengInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetInt32("ID");
			info.XueYuan_Id = reader.GetInt32("XueYuan_Id");
			info.XueHao = reader.GetString("XueHao");
			info.XingMing = reader.GetString("XingMing");
			info.ZhuanYe = reader.GetString("ZhuanYe");
			info.XingBie = reader.GetString("XingBie");
			info.ShouJi = reader.GetString("ShouJi");
            info.User_Id = reader.GetInt32Nullable("User_Id");

			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(XueShengInfo obj)
		{
		    XueShengInfo info = obj as XueShengInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("XueYuan_Id", info.XueYuan_Id);
 			hash.Add("XueHao", info.XueHao);
 			hash.Add("XingMing", info.XingMing);
 			hash.Add("ZhuanYe", info.ZhuanYe);
 			hash.Add("XingBie", info.XingBie);
            hash.Add("ShouJi", info.ShouJi);
            hash.Add("User_Id", info.User_Id);
 				
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
             dict.Add("XueYuan_Id", "学院编号");
             dict.Add("XueHao", "学号");
             dict.Add("XingMing", "姓名");
             dict.Add("ZhuanYe", "专业");
             dict.Add("XingBie", "性别");
             dict.Add("ShouJi", "手机");
             dict.Add("User_Id", "用户");
            #endregion

            return dict;
        }

    }
}