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
    /// 项目
    /// </summary>
	public class XiangMu : BaseDALSQL<XiangMuInfo>, IXiangMu
	{
		#region 对象实例及构造函数

		public static XiangMu Instance
		{
			get
			{
				return new XiangMu();
			}
		}
		public XiangMu() : base("XiangMu","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override XiangMuInfo DataReaderToEntity(IDataReader dataReader)
		{
			XiangMuInfo info = new XiangMuInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetInt32("ID");
			info.QiYeDaoShi_Id = reader.GetInt32("QiYeDaoShi_Id");
			info.JingSai_Id = reader.GetInt32("JingSai_Id");
			info.XiangMuMingCheng = reader.GetString("XiangMuMingCheng");
			info.XiangMuJianJie = reader.GetString("XiangMuJianJie");
			info.CanSaiShiJian = reader.GetDateTime("CanSaiShiJian");
			info.HuoJiangShiJian = reader.GetDateTime("HuoJiangShiJian");
			info.HuoJiangJiBie = reader.GetString("HuoJiangJiBie");
			info.HuoJiangMingCheng = reader.GetString("HuoJiangMingCheng");
			info.BeiZhu = reader.GetString("BeiZhu");
			info.XueYuanShenPi = reader.GetBoolean("XueYuanShenPi");
			info.XueXiaoShenPi = reader.GetBoolean("XueXiaoShenPi");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(XiangMuInfo obj)
		{
		    XiangMuInfo info = obj as XiangMuInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("QiYeDaoShi_Id", info.QiYeDaoShi_Id);
 			hash.Add("JingSai_Id", info.JingSai_Id);
 			hash.Add("XiangMuMingCheng", info.XiangMuMingCheng);
 			hash.Add("XiangMuJianJie", info.XiangMuJianJie);
 			hash.Add("CanSaiShiJian", info.CanSaiShiJian);
 			hash.Add("HuoJiangShiJian", info.HuoJiangShiJian);
 			hash.Add("HuoJiangJiBie", info.HuoJiangJiBie);
 			hash.Add("HuoJiangMingCheng", info.HuoJiangMingCheng);
 			hash.Add("BeiZhu", info.BeiZhu);
 			hash.Add("XueYuanShenPi", info.XueYuanShenPi);
 			hash.Add("XueXiaoShenPi", info.XueXiaoShenPi);
 				
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
             dict.Add("QiYeDaoShi_Id", "企业导师编号");
             dict.Add("JingSai_Id", "竞赛编号");
             dict.Add("XiangMuMingCheng", "项目名称");
             dict.Add("XiangMuJianJie", "项目简介");
             dict.Add("CanSaiShiJian", "参赛时间");
             dict.Add("HuoJiangShiJian", "获奖时间");
             dict.Add("HuoJiangJiBie", "获奖级别");
             dict.Add("HuoJiangMingCheng", "获奖名称");
             dict.Add("BeiZhu", "备注");
             dict.Add("XueYuanShenPi", "学院审批");
             dict.Add("XueXiaoShenPi", "学校审批");
             #endregion

            return dict;
        }

    }
}