using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHC.Framework.ControlUtil;
using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;
using System.Runtime.Serialization;


namespace WHC.MVCWebMis.Controllers
{

    

    public class JingSaiController : BusinessController<JingSai, JingSaiInfo>
    {
        public JingSaiController() : base()
        {
        }




        public ActionResult GetDictJson()
        {
            string sql = string.Format("SELECT * FROM JingSai WHERE id  IN (select MAX(id) from JingSai GROUP BY LeiBie);");
      
            List<JingSaiInfo> list = baseBLL.GetList(sql);
            List<CListItem> itemList = new List<CListItem>();
            foreach (JingSaiInfo info in list)
            {
                itemList.Add(new CListItem(info.LeiBie, info.ID.ToString()));
            }
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDictJson1()
        {
            string sql = string.Format("SELECT * FROM JingSai WHERE id  IN (select MAX(id) from JingSai GROUP BY JingSaiMingCheng);");

            List<JingSaiInfo> list = baseBLL.GetList(sql);
            List<CListItem> itemList = new List<CListItem>();
            foreach (JingSaiInfo info in list)
            {
                itemList.Add(new CListItem(info.JingSaiMingCheng, info.ID.ToString()));
            }
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }


    }
}
