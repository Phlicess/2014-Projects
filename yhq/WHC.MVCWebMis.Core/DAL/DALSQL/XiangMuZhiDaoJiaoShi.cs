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
    /// 项目指导教师
    /// </summary>
	public class XiangMuZhiDaoJiaoShi : BaseDALSQL<XiangMuZhiDaoJiaoShiInfo>, IXiangMuZhiDaoJiaoShi
	{
		#region 对象实例及构造函数

		public static XiangMuZhiDaoJiaoShi Instance
		{
			get
			{
				return new XiangMuZhiDaoJiaoShi();
			}
		}
		public XiangMuZhiDaoJiaoShi() : base("XiangMuZhiDaoJiaoShi","ZhiDaoJiaoShi_Id")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override XiangMuZhiDaoJiaoShiInfo DataReaderToEntity(IDataReader dataReader)
		{
			XiangMuZhiDaoJiaoShiInfo info = new XiangMuZhiDaoJiaoShiInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ZhiDaoJiaoShi_Id = reader.GetInt32("ZhiDaoJiaoShi_Id");
			info.XiangMu_Id = reader.GetInt32("XiangMu_Id");
			info.WeiCi = reader.GetInt32("WeiCi");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(XiangMuZhiDaoJiaoShiInfo obj)
		{
		    XiangMuZhiDaoJiaoShiInfo info = obj as XiangMuZhiDaoJiaoShiInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ZhiDaoJiaoShi_Id", info.ZhiDaoJiaoShi_Id);
 			hash.Add("XiangMu_Id", info.XiangMu_Id);
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
            dict.Add("ZhiDaoJiaoShi_Id", "指导教师编号");
             dict.Add("XiangMu_Id", "项目编号");
             dict.Add("WeiCi", "位次");
             #endregion

            return dict;
        }

    }
}