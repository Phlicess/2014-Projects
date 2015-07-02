using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FMS.Controllers.Receiver
{
    public class HistoryMattersController : Controller
    {
        //
        // GET: /HistoryBussiness/
        [CheckinLogin]
        public ActionResult Index(int? id)
        {
            WJEntities WJEn = new WJEntities();
            //根据当前用户登陆的昵称来查找这个接收者的ID
            string nickName = Session["NickName"].ToString();
            YongHu yongHuInFo = WJEn.YongHus.FirstOrDefault(YongHu => YongHu.NickName == nickName);
            //根据接收者ID查找用户任务表 并返回此接收者所有的的 用户任务 数据
            List<YongHu_RenWu> yongHu_renWu = WJEn.YongHu_RenWu.Where(YongHu_RenWu => YongHu_RenWu.YongHu_ID == yongHuInFo.ID).Where(YongHu_RenWu => YongHu_RenWu.FinishOrNo == true).OrderBy(y => y.RenWu.UploadTime).ToList();

            
            //List<RenWu> renWu = new List<RenWu>();
            //for (int t = 0; t < yongHu_renWu.LongCount(); t++)
            //{
            //    var x = yongHu_renWu[t].RenWu_ID;
            //    renWu.AddRange(WJEn.RenWus.Where(RenWu => RenWu.ID == x).ToList());
            //}
            //renWu.OrderBy(RenWu => RenWu.UploadTime);

            PagedList<YongHu_RenWu> yongHu_renWuList = new PagedList<YongHu_RenWu>(yongHu_renWu, id ?? 1, 18);
            return View(yongHu_renWuList);
        }

    }
}
