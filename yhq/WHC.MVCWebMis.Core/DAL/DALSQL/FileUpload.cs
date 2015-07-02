using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace WHC.MVCWebMis.DALSQL
{
    /// <summary>
    /// 相关附件信息
    /// </summary>
	public class FileUpload : BaseDALSQL<FileUploadInfo>, IFileUpload
	{
		#region 对象实例及构造函数

		public static FileUpload Instance
		{
			get
			{
				return new FileUpload();
			}
		}
		public FileUpload() : base("TB_FileUpload","ID")
        {
            this.sortField = "AddTime";

		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override FileUploadInfo DataReaderToEntity(IDataReader dataReader)
		{
			FileUploadInfo info = new FileUploadInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
            info.Owner_ID = reader.GetString("Owner_ID");
			info.AttachmentGUID = reader.GetString("AttachmentGUID");
			info.FileName = reader.GetString("FileName");
			info.BasePath = reader.GetString("BasePath");
			info.SavePath = reader.GetString("SavePath");
			info.Category = reader.GetString("Category");
			info.FileSize = reader.GetInt32("FileSize");
			info.FileExtend = reader.GetString("FileExtend");
            info.Editor = reader.GetString("Editor");
			info.AddTime = reader.GetDateTime("AddTime");
			info.DeleteFlag = reader.GetInt32("DeleteFlag");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(FileUploadInfo obj)
		{
		    FileUploadInfo info = obj as FileUploadInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
            hash.Add("Owner_ID", info.Owner_ID);
 			hash.Add("AttachmentGUID", info.AttachmentGUID);
 			hash.Add("FileName", info.FileName);
 			hash.Add("BasePath", info.BasePath);
 			hash.Add("SavePath", info.SavePath);
 			hash.Add("Category", info.Category);
 			hash.Add("FileSize", info.FileSize);
 			hash.Add("FileExtend", info.FileExtend);
 			hash.Add("Editor", info.Editor);
 			hash.Add("AddTime", info.AddTime);
 			hash.Add("DeleteFlag", info.DeleteFlag);
 				
			return hash;
		}


        /// <summary>
        /// 获取指定附件组GUID的附件信息
        /// </summary>
        /// <param name="attachmentGUID">附件组GUID</param>
        /// <returns></returns>
        public List<FileUploadInfo> GetByAttachGUID(string attachmentGUID)
        {
            if (string.IsNullOrEmpty(attachmentGUID))
            {
                throw new ArgumentException("附件组GUID不能为空", attachmentGUID);
            }
            else
            {
                string condition = string.Format("AttachmentGUID='{0}' ", attachmentGUID);
                return Find(condition);
            }
        }
    }
}