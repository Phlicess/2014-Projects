using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;
using WHC.Framework.Commons;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Common;
using WHC.MVCWebMis.Entity;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Controllers
{
    public class SystemTypeController : BusinessController<SystemType, SystemTypeInfo>
    {
        /// <summary>
        /// 用作下拉列表的菜单Json数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDictJson()
        {
            List<SystemTypeInfo> list = baseBLL.GetAll();
            List<CListItem> itemList = new List<CListItem>();
            foreach (SystemTypeInfo info in list)
            {
                itemList.Add(new CListItem(info.Name, info.OID));
            }
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }
    }
}
