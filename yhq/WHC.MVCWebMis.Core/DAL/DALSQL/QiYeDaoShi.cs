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
    /// 企业导师
    /// </summary>
	public class QiYeDaoShi : BaseDALSQL<QiYeDaoShiInfo>, IQiYeDaoShi
	{
		#region 对象实例及构造函数

		public static QiYeDaoShi Instance
		{
			get
			{
				return new QiYeDaoShi();
			}
		}
		public QiYeDaoShi() : base("QiYeDaoShi","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override QiYeDaoShiInfo DataReaderToEntity(IDataReader dataReader)
		{
			QiYeDaoShiInfo info = new QiYeDaoShiInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetInt32("ID");
			info.XingMing = reader.GetString("XingMing");
			info.ZhuanYe = reader.GetString("ZhuanYe");
			info.ZhiCheng = reader.GetString("ZhiCheng");
			info.ShenFenZhengHao = reader.GetString("ShenFenZhengHao");
			info.ZhiWu = reader.GetString("ZhiWu");
			info.GongSi = reader.GetString("GongSi");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(QiYeDaoShiInfo obj)
		{
		    QiYeDaoShiInfo info = obj as QiYeDaoShiInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("XingMing", info.XingMing);
 			hash.Add("ZhuanYe", info.ZhuanYe);
 			hash.Add("ZhiCheng", info.ZhiCheng);
 			hash.Add("ShenFenZhengHao", info.ShenFenZhengHao);
 			hash.Add("ZhiWu", info.ZhiWu);
 			hash.Add("GongSi", info.GongSi);
 				
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
             dict.Add("XingMing", "姓名");
             dict.Add("ZhuanYe", "专业");
             dict.Add("ZhiCheng", "职称");
             dict.Add("ShenFenZhengHao", "身份证号");
             dict.Add("ZhiWu", "职务");
             dict.Add("GongSi", "公司");
             #endregion

            return dict;
        }

    }
}