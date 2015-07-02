using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.Pager.Entity;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Entity;

namespace WHC.MVCWebMis.IDAL
{
    /// <summary>
    /// 政策法规公告动态
    /// </summary>
	public interface IInformation : IBaseDAL<InformationInfo>
	{
              
        /// <summary>
        /// 获取我的通知信息
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="infoType">信息类型</param>
        /// <returns></returns>
        DataTable GetMyInformation(int userId, InformationCategory infoType);
    }
}