using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FMS.Controllers.Receiver
{
    public class MemberListController : Controller
    {
        //
        // GET: /MemberList/
        [CheckinLogin]
        public ActionResult Index(int? id)
        {
            WJEntities WJEn = new WJEntities();
            List<YongHu> ReceiverList = WJEn.YongHus.Where(YongHu => YongHu.State == false).ToList();

            PagedList<YongHu> ReceiverPagerList = new PagedList<YongHu>(ReceiverList, id ?? 1, 18);
            return View(ReceiverPagerList);
        }

    }
}
