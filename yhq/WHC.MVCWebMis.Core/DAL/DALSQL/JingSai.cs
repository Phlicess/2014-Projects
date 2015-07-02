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
    /// 竞赛
    /// </summary>
	public class JingSai : BaseDALSQL<JingSaiInfo>, IJingSai
	{
		#region 对象实例及构造函数

		public static JingSai Instance
		{
			get
			{
				return new JingSai();
			}
		}
		public JingSai() : base("JingSai","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override JingSaiInfo DataReaderToEntity(IDataReader dataReader)
		{
			JingSaiInfo info = new JingSaiInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetInt32("ID");
			info.LeiBie = reader.GetString("LeiBie");
			info.FanWei = reader.GetString("FanWei");
			info.JingSaiMingCheng = reader.GetString("JingSaiMingCheng");
			info.JuBanDanWei = reader.GetString("JuBanDanWei");
			info.ChengBanDanWei = reader.GetString("ChengBanDanWei");
			info.WangZhi = reader.GetString("WangZhi");
			info.JianJie = reader.GetString("JianJie");
			info.JingSaiShiJian = reader.GetString("JingSaiShiJian");
			info.DaLei = reader.GetString("DaLei");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(JingSaiInfo obj)
		{
		    JingSaiInfo info = obj as JingSaiInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("LeiBie", info.LeiBie);
 			hash.Add("FanWei", info.FanWei);
 			hash.Add("JingSaiMingCheng", info.JingSaiMingCheng);
 			hash.Add("JuBanDanWei", info.JuBanDanWei);
 			hash.Add("ChengBanDanWei", info.ChengBanDanWei);
 			hash.Add("WangZhi", info.WangZhi);
 			hash.Add("JianJie", info.JianJie);
 			hash.Add("JingSaiShiJian", info.JingSaiShiJian);
 			hash.Add("DaLei", info.DaLei);
 				
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
             dict.Add("LeiBie", "类别");
             dict.Add("FanWei", "范围");
             dict.Add("JingSaiMingCheng", "竞赛名称");
             dict.Add("JuBanDanWei", "举办单位");
             dict.Add("ChengBanDanWei", "承办单位");
             dict.Add("WangZhi", "网址");
             dict.Add("JianJie", "简介");
             dict.Add("JingSaiShiJian", "竞赛时间");
             dict.Add("DaLei", "大类");
             #endregion

            return dict;
        }

    }
}