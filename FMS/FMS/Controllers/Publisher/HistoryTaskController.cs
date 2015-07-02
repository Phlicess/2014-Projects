using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace FMS.Controllers.Publisher
{
    public class HistoryTaskController : Controller
    {
        //
        // GET: /HistoryTask/
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

            for (int i = 0; i < renWu.Count; i++)
            {
                //判断当前日期是否大于任务截止日期 （任务是否过期）
                if (renWu[i].CloseTime != null && DateTime.Parse(System.DateTime.Now.ToString()) > DateTime.Parse(renWu[i].CloseTime.ToString()))
                {
                    renWuList.Add(renWu[i]);
                }
            }
            PagedList<RenWu> renWuPagedList = new PagedList<RenWu>(renWuList, id ?? 1, 20);

            return View(renWuPagedList);
        }

        //下载函数
        public string Download(string TaskId)
        {
            WJEntities WJEn = new WJEntities();
            int task_ID;
            int.TryParse(TaskId, out task_ID);
           
            RenWu renwu = WJEn.RenWus.Where(r => r.ID == task_ID).FirstOrDefault();

            if (renwu.FilePath != null)
            {
                var strFileName = Server.MapPath("~/UploadFiles/"+ renwu.FilePath + "/" + renwu.TaskName + ".zip");
                if (System.IO.File.Exists(strFileName))
                {
                    return renwu.FilePath + "/" + renwu.TaskName + ".zip"; 
                }
                return "false";
            }

            
            return "false";
        }

     
        //删除历史任务函数
        public string DeleteTask(string TaskIds)
        {
            WJEntities WJEn = new WJEntities();
            string[] task_IDS = TaskIds.Split(',');
            for (int i = 0; i < task_IDS.Length; i++)
            {
                int intTaskID;
                int.TryParse(task_IDS[i], out intTaskID);
                WJEn.RenWus.FirstOrDefault(r => r.ID == intTaskID).YongHu_RenWu.Clear();
                RenWu renWu = WJEn.RenWus.FirstOrDefault(r => r.ID == intTaskID);
                WJEn.RenWus.Remove(renWu);
                WJEn.SaveChanges();
            }
            return "任务删除成功!";
        }

    }
}
