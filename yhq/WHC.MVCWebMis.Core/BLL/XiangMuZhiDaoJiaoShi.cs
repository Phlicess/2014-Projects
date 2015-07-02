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
    /// 项目指导教师
    /// </summary>
	public class XiangMuZhiDaoJiaoShi : BaseBLL<XiangMuZhiDaoJiaoShiInfo>
    {
        public XiangMuZhiDaoJiaoShi() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

    }
}
