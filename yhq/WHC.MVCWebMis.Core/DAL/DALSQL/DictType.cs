using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.DALSQL
{
	/// <summary>
	/// DictType 的摘要说明。
	/// </summary>
    public class DictType : BaseDALSQL<DictTypeInfo>, IDictType
	{
		#region 对象实例及构造函数

		public static DictType Instance
		{
			get
			{
				return new DictType();
			}
		}
		public DictType() : base("tb_DictType","ID")
		{
            sortField = "Seq";
            IsDescending = false;
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override DictTypeInfo DataReaderToEntity(IDataReader dataReader)
		{
			DictTypeInfo dictTypeInfo = new DictTypeInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);

            dictTypeInfo.ID = reader.GetString("ID");
			dictTypeInfo.Name = reader.GetString("Name");
			dictTypeInfo.Remark = reader.GetString("Remark");
			dictTypeInfo.Seq = reader.GetString("Seq");
            dictTypeInfo.Editor = reader.GetString("Editor");
			dictTypeInfo.LastUpdated = reader.GetDateTime("LastUpdated");
            dictTypeInfo.PID = reader.GetString("PID");
			
			return dictTypeInfo;
		}
        
		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(DictTypeInfo obj)
		{
		    DictTypeInfo info = obj as DictTypeInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("ID", info.ID);
 			hash.Add("Name", info.Name);
 			hash.Add("Remark", info.Remark);
 			hash.Add("Seq", info.Seq);
 			hash.Add("Editor", info.Editor);
 			hash.Add("LastUpdated", info.LastUpdated);
            hash.Add("PID", info.PID);
 				
			return hash;
		}
    }
}