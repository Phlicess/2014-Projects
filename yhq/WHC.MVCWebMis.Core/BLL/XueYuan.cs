using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using WHC.Pager.Entity;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.BLL
{
    /// <summary>
    /// 学院
    /// </summary>
	public class XueYuan : BaseBLL<XueYuanInfo>
    {
        private IXueYuan xueYuanDal;
        public XueYuan() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            this.xueYuanDal = baseDal as IXueYuan;
        }

        /// <summary>
        /// 获取指定用户的机构列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public List<XueYuanInfo> GetOUsByUser(int userID)
        {
            return this.xueYuanDal.GetOUsByUser(userID);
        }

    }
}
