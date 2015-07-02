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
    /// 学院
    /// </summary>
	public interface IXueYuan : IBaseDAL<XueYuanInfo>
    {
        List<XueYuanInfo> GetOUsByUser(int userID);
    }
}