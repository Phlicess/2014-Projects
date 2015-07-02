using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;

namespace WHC.MVCWebMis.Controllers
{
    public class QiYeDaoShiController : BusinessController<QiYeDaoShi, QiYeDaoShiInfo>
    {
        public QiYeDaoShiController() : base()
        {
        }

        public ActionResult GetDictJson()
        {
            string sql = string.Format("SELECT * FROM QiYeDaoShi");


            List<QiYeDaoShiInfo> list = baseBLL.GetList(sql);
            List<CListItem> itemList = new List<CListItem>();
            foreach (QiYeDaoShiInfo info in list)
            {
                itemList.Add(new CListItem(info.XingMing, info.ID.ToString()));
            }
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }


    }
}
