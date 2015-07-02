using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FMS.Controllers.Receiver
{
    public class NewBussinessController : Controller
    {
        //
        // GET: /NewBussiness/
        [CheckinLogin]
    

        public ActionResult Receiver(int? id)
        {
            WJEntities WJEn = new WJEntities();

            //PagedList<RenWu> plUsers = WJEn.RenWus.OrderBy(model => model.ID).ToPagedList(1, 15);

            PagedList<RenWu> RenWuList = WJEn.RenWus.OrderBy(model => model.ID).ToPagedList(id ?? 1, 2);

            return View(RenWuList);
        }

        public ActionResult Delete(List<int> postData)
        {
            if (postData == null)
            {
                return Content("false");
            }
            else
            {
                //string[] arrtemp = post.Split(',');
                //int[] postData = new int[arrtemp.Length];   //用来存放将字符串转换成int[]
                //for (int i = 0; i < arrtemp.Length; i++)
                //{
                //    postData[i] = int.Parse(arrtemp[i]);
                //}

                WJEntities WJEn = new WJEntities();
                List<RenWu> RenWuList = new List<RenWu>();
                for (int t = 0; t < postData.LongCount(); t++)
                {
                    var x = postData[t];
                    RenWuList.AddRange(WJEn.RenWus.Where(RenWu => RenWu.ID == x).ToList());
                }

                for (int i = 0; i < postData.LongCount(); i++)
                {
                    WJEn.RenWus.Remove(RenWuList[i]);
                }
                WJEn.SaveChanges();
                return Content("true");
            }
        }

        //文件上传处理函数
        public ActionResult Upload(HttpPostedFileBase fileData, string Text)
        {
            if (fileData != null && Text == "1")
            {
                return Content("true");
            }
            return Content("false");
        }

    }
}
