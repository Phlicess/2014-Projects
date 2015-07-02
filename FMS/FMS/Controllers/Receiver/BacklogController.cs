using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FMS.Common;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using Webdiyer.WebControls.Mvc;


namespace FMS.Controllers.Receiver
{
    public class BacklogController : Controller
    {
        //
        // GET: /NewBussiness/
        //[CheckinLogin]

        //此函数实现了根据当前 登陆的任务接收者ID 返回这个用户的对应的任务列表
        public ActionResult Index(int? id)
        {
            //判断Session 和 GUID
            if (Session["NickName"] == null)
            {
                string url = Request.RawUrl;
                if (!url.Contains("="))
                {
                    HttpContext.Response.Redirect(Url.Action("Index", "Home"));
                    //return Content("Sorry..你输入的url有问题哟~ 重新登陆一下吧..");
                }
                string GUID = url.Split('=')[1];  //（已解决）这里可能会出现异常  原因：在没有Session的情况下 用户在地址栏输入的url没有 '=' 符号
                if (GUID != null)
                {
                    WJEntities WJ_G = new WJEntities();
                    YongHu_RenWu yongHuRenWu = WJ_G.YongHu_RenWu.FirstOrDefault(model => model.GUID == GUID);
                    if (yongHuRenWu != null)
                    {
                        Session["NickName"] = yongHuRenWu.YongHu.NickName;
                        Session["ID"] = yongHuRenWu.YongHu.ID;
                        Session["Name"] = yongHuRenWu.YongHu.Name;
                        Session["User"] = "Receiver";
                    }
                    else
                    {
                        HttpContext.Response.Redirect(Url.Action("Index", "Home"));
                        //return Content("Sorry..你的身份认证有问题额~  重新登陆一下吧..");
                    }
                }
            }
            WJEntities WJEn = new WJEntities();
            //根据当前用户登陆的昵称来查找这个接收者的ID
            string nickName = Session["NickName"].ToString();
            YongHu yongHuInFo = WJEn.YongHus.FirstOrDefault(YongHu => YongHu.NickName == nickName);
            //根据接收者ID查找用户任务表 并返回此接收者所有的的 用户任务 数据
            List<YongHu_RenWu> yongHu_renWu = WJEn.YongHu_RenWu.Where(yongHu_RenWu => yongHu_RenWu.YongHu_ID == yongHuInFo.ID && yongHu_RenWu.FinishOrNo == false).OrderBy(yongHu_RenWu => yongHu_RenWu.RenWu.UploadTime).ToList();
            PagedList<YongHu_RenWu> yongHu_renWuList = new PagedList<YongHu_RenWu>(yongHu_renWu, id ?? 1,18);

            return View(yongHu_renWuList);
        }
        [CheckinLogin]
        public ActionResult Delete(string TaskIds)
        {
            if (TaskIds == "")
            {
                return Content("false");
            }
            else
            {
                string[] task_IDs = TaskIds.Split(',');
                int yongHu_ID;
                int.TryParse(Session["ID"].ToString(), out yongHu_ID);
                WJEntities WJEn = new WJEntities();

                for (int i = 0; i < task_IDs.Length; i++)
                {
                    int taskId;
                    int.TryParse(task_IDs[i], out taskId);
                    WJEn.YongHu_RenWu.Remove(WJEn.YongHu_RenWu.Where(yr => yr.RenWu_ID == taskId).FirstOrDefault(yr => yr.YongHu_ID == yongHu_ID));
                }
                WJEn.SaveChanges();
                return Content("true");
            }
        }
        
        //查找任务函数 根据ID值  返回值是Json类型的实体对象列表
        //ID是一个数组类型
        public string Finds(List<int> ID)
        {
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.ProxyCreationEnabled = false;
            WJEn.Configuration.LazyLoadingEnabled = false;
            int yongHu_ID;
            int.TryParse(Session["ID"].ToString(), out yongHu_ID);
            int renWu_ID = ID[0];
            YongHu_RenWu User_TaskInfo = WJEn.YongHu_RenWu.Where(yr => yr.RenWu_ID == renWu_ID).FirstOrDefault(r => r.YongHu_ID == yongHu_ID);

            User_TaskInfo.ReadOrNo = true;
            WJEn.SaveChanges();
            //设置未完成任务人数
            WJEn.RenWus.FirstOrDefault(r => r.ID == renWu_ID).UnReaded =
                WJEn.YongHu_RenWu.Where(yr => yr.RenWu_ID == renWu_ID && yr.ReadOrNo == false).ToList().Count;

            RenWu TaskInfo = WJEn.RenWus.FirstOrDefault(r => r.ID == renWu_ID);
            string TaskStr = Utils.EntityToJson(TaskInfo);
            WJEn.SaveChanges();
            return TaskStr;
        }

        //上传文件处理函数

        public string UploadFile(HttpPostedFileBase fileData, string Task_ID = "")
        {
            if (fileData != null)
            {
                try
                {
                    ControllerContext.HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.Charset = "UTF-8";

                    int taskId;
                    int.TryParse(Task_ID, out taskId);
                    WJEntities WJEn = new WJEntities();

                    string GUID = WJEn.RenWus.FirstOrDefault(r => r.ID == taskId).FilePath;
                    string allFilePath = Server.MapPath("~/UploadFiles/" + GUID + "/");
                    if (allFilePath == null)
                    {
                        GUID = Guid.NewGuid().ToString();
                        allFilePath = Server.MapPath("~/UploadFiles/" + GUID + "/");
                        Directory.CreateDirectory(allFilePath);
                        WJEn.RenWus.FirstOrDefault(r => r.ID == taskId).FilePath = GUID; //在数据库保存 文件夹的GUID就可以
                        //将任务用户表的完成状态设置为已经完成..
                        //int intYongHu_ID;
                        //int.TryParse(Session["ID"].ToString(), out intYongHu_ID);
                        //WJEn.YongHu_RenWu.Where(ry => ry.RenWu_ID == taskId).FirstOrDefault(y => y.YongHu_ID == intYongHu_ID).FinishOrNo = true;
                        WJEn.SaveChanges();
                    }

                    //将任务用户表的完成状态设置为已经完成..
                    int intYongHu_ID;
                    int.TryParse(Session["ID"].ToString(), out intYongHu_ID);
                    WJEn.YongHu_RenWu.Where(ry => ry.RenWu_ID == taskId).FirstOrDefault(y => y.YongHu_ID == intYongHu_ID).FinishOrNo = true;
                    WJEn.SaveChanges();

                    //设置未完成任务人数
                    WJEn.RenWus.FirstOrDefault(r => r.ID == taskId).UnFinished =
                        WJEn.YongHu_RenWu.Where(yr => yr.RenWu_ID == taskId && yr.FinishOrNo == false).ToList().Count;
                    WJEn.SaveChanges();
                    
                    string fileName = Path.GetFileName(fileData.FileName); //原始文件名称
                    string fileExtension = Path.GetExtension(fileName); //文件扩展名
                    string newFileName = Session["Name"] + fileExtension;
                    fileData.SaveAs(allFilePath + newFileName);

                    string renWuFile = allFilePath + WJEn.RenWus.FirstOrDefault(r => r.ID == taskId).TaskName + ".zip";  //原有的文件
                    if (!System.IO.File.Exists(renWuFile))
                    {
                        ZipOutputStream p = new ZipOutputStream(System.IO.File.Create(renWuFile));  //如果没有原有的文件 就创建一个 并且 创建完成就关闭占用
                        p.Close();
                    }

                    //1.要被压缩文件的绝对路径 2.保存的目录 3.已经存在的压缩包的路径
                    RARsave(allFilePath + newFileName, allFilePath, renWuFile); //调用压缩函数 把用户上传的文件包含在压缩包里

                    System.IO.File.Delete(allFilePath + newFileName); //删除用户上传的文件

                    allFilePath = WJEn.RenWus.FirstOrDefault(r => r.ID == taskId).FilePath;

                    return allFilePath;
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
        //1.被压缩文件的绝对路径 2.保存的目录 3.保存的ZIP文件名
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
                the_Info = " a -ep " + rarName + " " + patch + " -r";
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

        ////压缩函数
        //public void RARsave(string renWuFile, string newFileName, string allFilePath)
        //{
        //    //已经存在的压缩文件 目标rar
        //    string rarFilePath = renWuFile;

        //    //被压缩的文件路径
        //    //string unrarDestPath = Path.Combine(sysTempDir, Path.GetFileNameWithoutExtension(rarFilePath));
        //    string rarDestPath = allFilePath + newFileName;

        //    //组合出需要shell的完整格式
        //    string shellArguments = string.Format("a -ep {0} {1}", rarFilePath, rarDestPath); //a file d:/*.ext  

        //    //用Process调用
        //    using (Process rar = new Process())
        //    {
        //        rar.StartInfo.FileName = "D:/Programer/WinRAR/WinRAR.exe";
        //        rar.StartInfo.WorkingDirectory = allFilePath;
        //        rar.StartInfo.Arguments = shellArguments;
        //        //隐藏rar本身的窗口
        //        rar.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //        rar.Start();
        //        //等待压缩完成
        //        rar.WaitForExit();
        //        rar.Close();
        //    }

        //}


        //下载任务说明函数 （任务ID）
        public ActionResult DownLoadFile(string TaskId)
        {
            if (TaskId == null)
            {
                return Content("请选择一个任务..");
            }
            int task_ID;
            int.TryParse(TaskId, out task_ID);
            WJEntities WJEn = new WJEntities();
            RenWu renWuInfo = WJEn.RenWus.FirstOrDefault(r => r.ID == task_ID);
            if (renWuInfo.FilePath != null)
            {

                var strFileName = Server.MapPath("~/UploadFiles/" + renWuInfo.FilePath + "/任务说明.zip");
               // string[] files = Directory.GetFiles(strFileName, "任务说明.zip", SearchOption.TopDirectoryOnly); 
                if (System.IO.File.Exists(strFileName)) //判断 目录是否含有 这个文件
                {
                    return Content(renWuInfo.FilePath + "/任务说明.zip");
                }
                return Content("false");
            }


           
            //var path = Server.MapPath("~/UploadFiles/" + renWuInfo.FilePath + "/" + renWuInfo.TaskName + ".zip");
            //return File(path, "application/x-zip-compressed", renWuInfo.TaskName + ".zip");
            return Content("false");
        }

    }
}
