using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Services.Description;
using FMS.Common;
using Webdiyer.WebControls.Mvc;

namespace FMS.Controllers.Publisher
{
    public class GoingTaskController : Controller
    {
        //
        // GET: /GoingTask/
        [CheckinLogin]
        public ActionResult Index(int? id)
        {
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            //根据当前用户登陆的昵称来查找这个接收者的ID
            //string nickName = Session["NickName"].ToString();
            //YongHu yongHuInFo = WJEn.YongHus.FirstOrDefault(YongHu => YongHu.NickName == nickName);
            //根据接收者ID查找用户任务表 并返回此接收者所有的的 用户任务 数据
            var yongHu_ID = int.Parse(Session["ID"].ToString());
            List<RenWu> renWu = WJEn.RenWus.Where(RenWu => RenWu.YongHu_ID == yongHu_ID).Where(RenWu => RenWu.Remark == false).ToList();
            List<RenWu> renWuList = new List<RenWu>();
            
            for (int i=0; i < renWu.Count; i++)
            {
                //判断当前日期是否大于任务截止日期 （任务是否过期）
                DateTime CloseTime;
                if (DateTime.TryParse(renWu[i].CloseTime.ToString(), out CloseTime) && DateTime.Parse(System.DateTime.Now.ToString()) < DateTime.Parse(renWu[i].CloseTime.ToString()))
                {
                    renWuList.Add(renWu[i]);
                }
            }
            PagedList<RenWu> renWuPagedList = new PagedList<RenWu>(renWuList, id ?? 1, 20);

            return View(renWuPagedList);
        }

        //Finds  查看事件详细
        public string Finds(string ID)
        {
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            int id = int.Parse(ID);
            RenWu renWu = WJEn.RenWus.FirstOrDefault(d => d.ID == id);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string renWuInfo = serializer.Serialize(renWu);;
            return renWuInfo;
        }

        //查看事项未完成人员
        public string FindUnfinished(string TaskId)
        {
            int task_ID;
            int.TryParse(TaskId, out task_ID);
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            List<YongHu_RenWu> yongHu_RenWuList = WJEn.YongHu_RenWu.Where(yr => yr.FinishOrNo == false && yr.RenWu_ID == task_ID).ToList();
            //List<YongHu> yongHuList = new List<YongHu>();
            string yongHus = "[";
            foreach (YongHu_RenWu yongHuRenWu in yongHu_RenWuList)
            {
                yongHus += Utils.EntityToJson(WJEn.YongHus.FirstOrDefault(y => y.ID == yongHuRenWu.YongHu_ID)) + ",";
            }
            yongHus += "]";
            return yongHus;
        }
  
    }
}

