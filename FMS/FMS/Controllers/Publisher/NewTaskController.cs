using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Win32;


namespace FMS.Controllers.Publisher
{
    public class NewTaskController : Controller
    {
        //
        // GET: /NewTask/
        [CheckinLogin]
        public ActionResult Index()
        {
            return View();
        }
        
        //返回所有接收者的信息
        public string FindReceiver()
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
        
        //添加一个任务
        public ActionResult AddTask(RenWu renWuInfo, string Group_ID, string receiversList)
        {
            if (renWuInfo == null)
            {
                return Content("false");
            }
            
            WJEntities WJEn = new WJEntities();
            //renWuInfo.FilePath = ;
            WJEn.RenWus.Add(renWuInfo);
            WJEn.SaveChanges();
            if (receiversList != "")
            {
                receiversList += ",";
            }
            
            //根据选中的群组ID 去查找此群组包含的接收者
            string[] group_ID;
            group_ID = Group_ID.Split(',');
            int[] GroupIDInt = new int[group_ID.Length];
            for (int t = 0; t < group_ID.Length; t++)
            {
                int.TryParse(group_ID[t], out GroupIDInt[t]);
            }
            //查找群组的里包含的接收者
            List<YongHu> yingHuList = new List<YongHu>();
            for (int i = 0; i < group_ID.Length && group_ID.Length > 0 && group_ID[i] != ""; i++)
            {
                int ID = GroupIDInt[i];
                yingHuList.AddRange(WJEn.QunZus.FirstOrDefault(model => model.ID == ID).YongHus.ToList());
            }
            
            for (int t = 0; t < yingHuList.Count; t++)
            {
                receiversList += yingHuList[t].NickName + ",";
            }
            receiversList = receiversList.Substring(0, receiversList.Length - 1);  //去掉最后一个","字符

            string[] nickNameArr;
            nickNameArr = receiversList.Split(',');

            //为了去除掉 群组里面的接收者和自己选择的接收者的重复问题
            List<string> nickNameList = new List<string>();
            foreach (string midstr in nickNameArr)
            {
                if (!nickNameList.Contains(midstr))
                {
                    nickNameList.Add(midstr);
                }
            }

            int NewID = renWuInfo.ID;

            WJEn.RenWus.FirstOrDefault(r => r.ID == NewID).UnFinished = nickNameList.Count;
            WJEn.RenWus.FirstOrDefault(r => r.ID == NewID).UnReaded = nickNameList.Count;
            WJEn.SaveChanges();
            for (int i = 0; i < nickNameList.Count; i++)
            {
                YongHu_RenWu yongHu_RenWu = new YongHu_RenWu();
                string nickName = nickNameList[i];
                yongHu_RenWu.YongHu_ID = WJEn.YongHus.FirstOrDefault(YongHu => YongHu.NickName == nickName).ID;
                yongHu_RenWu.RenWu_ID = NewID;
                yongHu_RenWu.ReadOrNo = false;
                yongHu_RenWu.FinishOrNo = false;
                yongHu_RenWu.GUID = System.Guid.NewGuid().ToString();
                WJEn.YongHu_RenWu.Add(yongHu_RenWu);
                WJEn.SaveChanges();
            }
            return Content(NewID.ToString());
        }

        //添加草稿
        public string AddDraft(RenWu renWuInfo, string Group_ID, string receiversList)
        {
            if (renWuInfo == null)
            {
                return "false";
            }

            WJEntities WJEn = new WJEntities();
            //renWuInfo.FilePath = ;
            WJEn.RenWus.Add(renWuInfo);
            WJEn.SaveChanges();

            //根据选中的群组ID 去查找此群组包含的接收者
            string[] group_ID;
            group_ID = Group_ID.Split(',');
            int[] GroupIDInt = new int[group_ID.Length];
            for (int t = 0; t < group_ID.Length; t++)
            {
                 int.TryParse(group_ID[t], out GroupIDInt[t]);
            }
            //查找群组的里包含的接收者
            List<YongHu> yingHuList = new List<YongHu>();
            for (int i = 0; i < group_ID.Length && group_ID.Length > 0 && group_ID[i] != ""; i++)
            {
                int ID = GroupIDInt[i];
                yingHuList.AddRange(WJEn.QunZus.FirstOrDefault(model => model.ID == ID).YongHus.ToList());
            }

            for (int t = 0; t < yingHuList.Count; t++)
            {
                receiversList += "," + yingHuList[t].NickName;
            }
            //receiversList = receiversList.Substring(0, receiversList.Length - 1);  //去掉最后一个","字符


            string[] nickNameArr;
            nickNameArr = receiversList.Split(',');

            //为了去除掉 群组里面的接收者和自己选择的接收者的重复问题
            List<string> nickNameList = new List<string>();
            foreach (string midstr in nickNameArr)
            {
                if (!nickNameList.Contains(midstr))
                {
                    nickNameList.Add(midstr);
                }
            }

            int NewID = renWuInfo.ID;
            
            for (int i = 0; i < nickNameList.Count; i++)
            {
                YongHu_RenWu yongHu_RenWu = new YongHu_RenWu();
                string nickName = nickNameList[i];
                yongHu_RenWu.YongHu_ID = WJEn.YongHus.FirstOrDefault(YongHu => YongHu.NickName == nickName).ID;
                yongHu_RenWu.RenWu_ID = NewID;
                yongHu_RenWu.ReadOrNo = false;
                yongHu_RenWu.FinishOrNo = false;
                WJEn.YongHu_RenWu.Add(yongHu_RenWu);
                WJEn.SaveChanges();
            }           
            return "true";
        }


        //上传文件处理函数
        //保存到任务表里面的FilePath只是GUID标识的文件夹的名字 
        public string UploadFile(HttpPostedFileBase fileData, string TaskName="")
        {
            if (fileData != null)
            {
                try
                {
                    ControllerContext.HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.Charset = "UTF-8";

                    // 文件上传后的保存路径
                    //文件夹的名字 用GUID标识Guid.NewGuid().ToString()
                    string GUID = Guid.NewGuid().ToString();
                    string filePath = Server.MapPath("~/UploadFiles/" + GUID + "/"); // DateTime.Now.ToLongDateString().ToString() 这个方式的时间 是 "2008年9月4日" 格式的  存储路径中不能出现 ":"冒号等符号。。

                    string fileName = Path.GetFileName(fileData.FileName); //原始文件名称
                    string fileExtension = Path.GetExtension(fileName); //文件扩展名
                    //string saveName = Guid.NewGuid().ToString() + fileExtension; //保存文件名称

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath); //如果没有路径 则新添加一个文件路径
                    }                    
                    fileData.SaveAs(filePath + "任务说明" + fileExtension);

                    var beforeUrl = filePath + "任务说明" + fileExtension;
                    RARsave(beforeUrl, filePath, "任务说明.zip");  // 1. 要压缩文件的绝对路径 2.压缩后保存的目录 3. 压缩的名字
                    System.IO.File.Delete(beforeUrl); //删除用户上传的文件
                    return GUID;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            else
            {
                return "false";
            }
        }

        //调用 rar 压缩
        public void RARsave(string patch, string rarPatch, string rarName)
        {
            string the_rar;
            RegistryKey the_Reg;
            Object the_Obj;
            String the_Info;
            ProcessStartInfo the_StartInfo;
            Process the_Process;
            try
            {
                the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                the_Obj = the_Reg.GetValue("");

                the_rar = String.Concat(the_Obj);
                //the_rar = the_Obj.ToString();

                the_Reg.Close();
                //the_rar = the_rar.Substring(1, the_rar.Length - 7);
                Directory.CreateDirectory(rarPatch);
                //命令参数  
                //the_Info = " a    " + rarName + "  " + @"C:Test70821.txt"; //文件压缩  
                the_Info = " a -ep " + rarName + " " + patch ;
                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //打包文件存放目录  
                the_StartInfo.WorkingDirectory = rarPatch;
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        ////返回当前用户所有创建的群组
        public string FindGroups()
        {
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            string qunZuList = "[";
            JavaScriptSerializer serializer =new JavaScriptSerializer();
            List<QunZu> defaultList = WJEn.QunZus.Where(model => model.PublicGroup == true).ToList();
            foreach (QunZu defaultQunZu in defaultList)
            {
                qunZuList += serializer.Serialize(defaultQunZu) + ",";
            }

            int yongHu_ID = int.Parse(Session["ID"].ToString());
            List<QunZu> userList = WJEn.QunZus.Where(model => model.YongHu_ID == yongHu_ID).ToList();
            foreach (QunZu userQunZu in userList)
            {
                qunZuList += serializer.Serialize(userQunZu) + ",";
            }
            qunZuList += "]";
            return qunZuList;
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
                string url = Request.Url.Host;
                string VirtualPath = Url.Content("~/");
                if (Request.Url.Port.ToString() != "")
                {
                    url += ":" + Request.Url.Port;
                }
                if (VirtualPath != "/")
                {
                    url += VirtualPath;
                }
                messages.Body = Task_ID +
                    "你收到了一个新任务!<a href=\"http://" + url + "Backlog?id=" +
                    GUID + "\" style='color: #3c3c3c; font-wight: border;'>点击查看任务详细信息</a>";
                //内容的编码格式
                messages.BodyEncoding = Encoding.UTF8;

                messages.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                //messages.CC.Add(to);

                //从配置文件读取服务邮箱信息
                string userName = ConfigurationManager.AppSettings["Email"];
                string password = ConfigurationManager.AppSettings["EmailPassWord"];
                string host = ConfigurationManager.AppSettings["STMP"];
                messages.From = new MailAddress(userName);
                messages.To.Add(receiver.Email);

                SmtpClient sc = new SmtpClient();
                NetworkCredential nc = new NetworkCredential();

                nc.UserName = userName;      //邮箱地址
                nc.Password = password;      //邮箱密码
                sc.UseDefaultCredentials = true;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.Credentials = nc;
                //如果这里报mail from address must be same as authorization user这个错误，是你的QQ邮箱没有开启SMTP，
                //到自己的邮箱设置一下就可以啦！在帐户下面,如果是163邮箱的话，下面该成smtp.163.com
                sc.Host = host;
                sc.Send(messages);
            }

            

            return Content("邮件发送成功！");
        }

    }
}
