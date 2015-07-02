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
    public class XueYuanController : BusinessController<XueYuan, XueYuanInfo>
    {
        public XueYuanController() : base()
        {
        }

        public ActionResult GetDictJson()
        {
            string sql = string.Format("SELECT * FROM XueYuan");


            List<XueYuanInfo> list = baseBLL.GetList(sql);
            List<CListItem> itemList = new List<CListItem>();
            foreach (XueYuanInfo info in list)
            {
                itemList.Add(new CListItem(info.XueYuan, info.ID.ToString()));
            }
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }



    }
}
