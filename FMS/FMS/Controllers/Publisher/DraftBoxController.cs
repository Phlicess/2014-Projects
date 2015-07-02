using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FMS.Common;
using Webdiyer.WebControls.Mvc;

namespace FMS.Controllers.Publisher
{
    public class DraftBoxController : Controller
    {
        //
        // GET: /DraftBox/
        [CheckinLogin]
        public ActionResult Index(int? id)
        {
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            //根据当前用户登陆的昵称来查找这个接收者的ID
            //string nickName = Session["NickName"].ToString();
            //YongHu yongHuInFo = WJEn.YongHus.FirstOrDefault(YongHu => YongHu.NickName == nickName);
            //根据接收者ID查找用户任务表 并返回此接收者所有的的 用户任务 数据
            int yongHu_ID;
            int.TryParse(Session["ID"].ToString(), out yongHu_ID);
            List<RenWu> renWu = WJEn.RenWus.Where(RenWu => RenWu.YongHu_ID == yongHu_ID).Where(RenWu => RenWu.Remark == true).ToList();

            PagedList<RenWu> renWuPagedList = new PagedList<RenWu>(renWu, id ?? 1, 20);

            return View(renWuPagedList);
        }

        //赋值方法
        public string Finds(int Draft_ID)
        {
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            RenWu renWuInfo = WJEn.RenWus.FirstOrDefault(RenWu => RenWu.ID == Draft_ID);
            //string renWu = "[";
            string renWu = "";
            renWu += Utils.EntityToJson(renWuInfo);
            //renWu += "]";
            return renWu;
        }

        //查找草稿所有的接收者
        public string FindReceivers(string DraftId)
        {
            int draft_ID;
            int.TryParse(DraftId, out draft_ID);
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            List<YongHu_RenWu> yongHuRenWuList =  WJEn.YongHu_RenWu.Where(r => r.RenWu_ID == draft_ID).ToList();
            List<YongHu> yongHuList = new List<YongHu>();
            foreach (YongHu_RenWu yongHuRenWu in yongHuRenWuList)
            {
                yongHuList.Add(WJEn.YongHus.FirstOrDefault(y => y.ID == yongHuRenWu.YongHu_ID));
            }
            string yongHuStr = "[";
            for (int i = 0; i < yongHuList.Count; i++)
            {
                yongHuStr += Utils.EntityToJson(yongHuList[i]) + ",";
            }
            yongHuStr += "]";

            return yongHuStr;
        }

        //查找所有的接收者
        public string FindAllReceivers()
        {
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            List<YongHu> yongHuList = WJEn.YongHus.Where(YongHu => YongHu.State == false).ToList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string yongHuJson = "[";
            foreach (YongHu yongHu in yongHuList)
            {
                yongHuJson += serializer.Serialize(yongHu) + ',';
            }
            yongHuJson += "]";
            return yongHuJson;
        }

        //草稿发布1.(草稿的ID) 2.(草稿基本信息) 3.(接收者)
        public string PublicDraft(string DraftId, RenWu draftInfo, string receiversList = "")
        {
            int draft_ID;
            int.TryParse(DraftId, out draft_ID);
            WJEntities WJEn = new WJEntities();
            RenWu renWu = WJEn.RenWus.FirstOrDefault(r => r.ID == draft_ID);
            renWu.TaskName = draftInfo.TaskName;
            renWu.TaskDetails = draftInfo.TaskDetails;
            renWu.Remark = false;
            renWu.UploadTime = draftInfo.UploadTime;
            renWu.CloseTime = draftInfo.CloseTime;
            renWu.YongHu_ID = draftInfo.YongHu_ID;
            renWu.FilePath = draftInfo.FilePath;
            WJEn.RenWus.FirstOrDefault(r => r.ID == draft_ID).YongHu_RenWu.Clear();
            WJEn.SaveChanges();
            string[] nickNameArr = receiversList.Split(',');

            WJEn.RenWus.FirstOrDefault(r => r.ID == draft_ID).UnFinished = nickNameArr.Length;
            WJEn.RenWus.FirstOrDefault(r => r.ID == draft_ID).UnReaded = nickNameArr.Length;
            WJEn.SaveChanges();

            for (int i = 0; i < nickNameArr.Length; i++)
            {
                YongHu_RenWu yongHu_RenWu = new YongHu_RenWu();
                string nickName = nickNameArr[i];
                yongHu_RenWu.YongHu_ID = WJEn.YongHus.FirstOrDefault(YongHu => YongHu.NickName == nickName).ID;
                yongHu_RenWu.RenWu_ID = draft_ID;
                yongHu_RenWu.ReadOrNo = false;
                yongHu_RenWu.FinishOrNo = false;
                yongHu_RenWu.GUID = System.Guid.NewGuid().ToString();
                WJEn.YongHu_RenWu.Add(yongHu_RenWu);
                WJEn.SaveChanges();
            }
            return DraftId;   //返回任务的ID 以便发送邮件
        }

        //发送邮件提醒函数
        //参数 接收者的邮件信息
        public ActionResult SendEmail(string receiverNickName, string Group_ID, string Task_ID)
        {
            //MailAddress from = new MailAddress("2233907468@qq.com");
            //MailAddress to = new MailAddress("");

            //根据选中的群组ID 去查找此群组包含的接收者
            WJEntities WJEn = new WJEntities();
            string[] group_ID;
            string receiversList = "";
            group_ID = Group_ID.Split(',');
            int[] GroupIDInt = new int[group_ID.Length];
            for (int t = 0; t < group_ID.Length; t++)
            {
                int.TryParse(group_ID[t], out GroupIDInt[t]);
            }
            //查找群组的里包含的接收者
            List<YongHu> yongHuList = new List<YongHu>();
            for (int i = 0; i < group_ID.Length && group_ID.Length > 0 && group_ID[i] != ""; i++)
            {
                int ID = GroupIDInt[i];
                yongHuList.AddRange(WJEn.QunZus.FirstOrDefault(model => model.ID == ID).YongHus.ToList());
            }

            List<string> nickNameList = new List<string>();
            nickNameList.AddRange(receiverNickName.Split(','));
            //将得到所有不重复的接收者邮箱地址 emaiList
            for (int t = 0; t < yongHuList.Count; t++)
            {
                if (!nickNameList.Contains(yongHuList[t].NickName))
                {
                    nickNameList.Add(yongHuList[t].NickName);
                }
                //receiversList += yingHuList[t].Email + ",";
            }

            foreach (string singleNickName in nickNameList)
            {
                //接收者
                YongHu receiver = WJEn.YongHus.FirstOrDefault(model => model.NickName == singleNickName);
                //任务
                int task_ID = int.Parse(Task_ID);
                //RenWu renwuInfo = WJEn.RenWus.FirstOrDefault(model => model.ID == task_ID);
                //任务-接收者-GUID
                string GUID = WJEn.YongHu_RenWu.Where(model => model.RenWu_ID == task_ID).FirstOrDefault(model => model.YongHu_ID == receiver.ID).GUID;

                MailMessage messages = new MailMessage();
                //邮箱标题
                messages.Subject = Session["Name"] + "给你发送了一个新的任务:";
                messages.IsBodyHtml = true;
                //邮件内容
                messages.Body = Task_ID +
                    "你收到了一个新任务!<a href=\"http://localhost:12321/Backlog?id=" +
                    GUID + "\" style='color: #3c3c3c; font-wight: border;'>点击查看任务详细信息</a>";
                //内容的编码格式
                messages.BodyEncoding = System.Text.Encoding.UTF8;

                messages.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                //messages.CC.Add(to);
                messages.From = new MailAddress("15069545783@163.com");
                messages.To.Add(receiver.Email);

                SmtpClient sc = new SmtpClient();
                NetworkCredential nc = new NetworkCredential();
                nc.UserName = "15069545783@163.com";//邮箱地址
                nc.Password = "wkn12345678";      //邮箱密码
                sc.UseDefaultCredentials = true;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.Credentials = nc;
                //如果这里报mail from address must be same as authorization user这个错误，是你的QQ邮箱没有开启SMTP，
                //到自己的邮箱设置一下就可以啦！在帐户下面,如果是163邮箱的话，下面该成smtp.163.com
                sc.Host = "smtp.163.com";
                sc.Send(messages);
            }
            
            return Content("邮件发送成功！");
        }
    }
}
