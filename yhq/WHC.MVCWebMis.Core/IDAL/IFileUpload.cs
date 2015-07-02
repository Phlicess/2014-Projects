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
    /// �ϴ��ļ�����
    /// </summary>
	public interface IFileUpload : IBaseDAL<FileUploadInfo>
	{
        /// <summary>
        /// ��ȡָ��������GUID�ĸ�����Ϣ
        /// </summary>
        /// <param name="attachmentGUID">������GUID</param>
        /// <returns></returns>
        List<FileUploadInfo> GetByAttachGUID(string attachmentGUID);

    }
}