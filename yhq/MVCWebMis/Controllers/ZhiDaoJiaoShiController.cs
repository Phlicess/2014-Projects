using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;


namespace WHC.MVCWebMis.Controllers
{
    public class ZhiDaoJiaoShiController : BusinessController<ZhiDaoJiaoShi, ZhiDaoJiaoShiInfo>
    {
        public ZhiDaoJiaoShiController() : base()
        {
        }
    }
}
