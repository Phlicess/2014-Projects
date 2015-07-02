using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.Pager.Entity;
using WHC.MVCWebMis.Entity;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.IDAL
{
    /// <summary>
    /// 上传文件操作
    /// </summary>
	public interface IFileUpload : IBaseDAL<FileUploadInfo>
	{
        /// <summary>
        /// 获取指定附件组GUID的附件信息
        /// </summary>
        /// <param name="attachmentGUID">附件组GUID</param>
        /// <returns></returns>
        List<FileUploadInfo> GetByAttachGUID(string attachmentGUID);

    }
}