using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;

namespace WHC.MVCWebMis.Controllers
{
    public class FileUploadController : BaseController
    {
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase fileData, string guid, string folder)
        {
            if (fileData != null)
            {
                try
                {
                    ControllerContext.HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.Charset = "UTF-8";

                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/UploadFiles/");
                    DirectoryUtil.AssertDirExist(filePath);

                    string fileName = Path.GetFileName(fileData.FileName);      //原始文件名称
                    string fileExtension = Path.GetExtension(fileName);         //文件扩展名
                    string saveName = Guid.NewGuid().ToString() + fileExtension; //保存文件名称

                    FileUploadInfo info = new FileUploadInfo();
                    info.FileData = ReadFileBytes(fileData);
                    if (info.FileData != null)
                    {
                        info.FileSize = info.FileData.Length;
                    }
                    info.Category = folder;
                    info.FileName = fileName;
                    info.FileExtend = fileExtension;
                    info.AttachmentGUID = guid;
                    info.AddTime = DateTime.Now;
                    info.Editor = CurrentUser.Name;//登录人
                    //info.Owner_ID = OwerId;//所属主表记录ID

                    CommonResult result = BLLFactory<FileUpload>.Instance.Upload(info);
                    if (!result.Success)
                    {
                        LogTextHelper.Error("上传文件失败:" + result.ErrorMessage);
                    }
                    return Content(result.Success.ToString());
                }
                catch (Exception ex)
                {
                    LogTextHelper.Error(ex);
                    return Content("false");
                }
            }
            else
            {
                return Content("false");
            }
        }

        private byte[] ReadFileBytes(HttpPostedFileBase fileData)
        {
            byte[] data;
            using (Stream inputStream = fileData.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            return data;
        }

        /// <summary>
        /// 删除单个附件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(id))
            {
                result = BLLFactory<FileUpload>.Instance.Delete(id);
            }
            return Content(result);
        }

        /// <summary>
        /// 获取附件的HTML代码
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult GetViewAttachmentHtml(string guid)
        {
            string html = @"<li>附件暂无</li>";

            if (string.IsNullOrEmpty(guid))
                return Content(html);

            StringBuilder sb = new StringBuilder();
            int seq = 1;
            List<FileUploadInfo> fileList = BLLFactory<FileUpload>.Instance.GetByAttachGUID(guid);
            if (fileList != null && fileList.Count > 0)
            {
                foreach (FileUploadInfo info in fileList)
                {
                    string fileName = info.FileName.Trim();
                    fileName = System.Web.HttpContext.Current.Server.UrlEncode(fileName);

                    sb.AppendFormat(@"<li><span>[ 附件{0} ]</span>", seq++);
                    sb.AppendFormat(@"<img border='0' width='16px' height='16px' src='/Content/Themes/Default/file_extension/{0}.png' />", info.FileExtend.Trim('.'));
                    sb.AppendFormat(@"<a href='/{0}?ext={1}' target='_blank'>&nbsp;{2}</a></li>", GetFilePath(info), info.FileExtend.Trim('.'), info.FileName);
                }
            }
            else
            {
                sb.Append(html);
            }

            return Content(sb.ToString());
        }

        private string GetFilePath(FileUploadInfo info)
        {
            string filePath = BLLFactory<FileUpload>.Instance.GetFilePath(info);
            return HttpUtility.UrlPathEncode(filePath);
        }

        /// <summary>
        /// 获取流程附件html,添加可删除附件链接
        /// </summary>
        /// <param name="strGuid">附件guid</param>
        /// <returns>string</returns>
        public ActionResult GetAttachmentHtml(string guid)
        {
            string html = @"<li>附件暂无</li>";

            if (string.IsNullOrEmpty(guid))
                return Content(html);

            int seq = 1;
            StringBuilder sb = new StringBuilder();

            List<FileUploadInfo> fileList = BLLFactory<FileUpload>.Instance.GetByAttachGUID(guid);
            if (fileList != null && fileList.Count > 0)
            {
                foreach (FileUploadInfo info in fileList)
                {
                    string fileName = info.FileName.Trim();
                    fileName = System.Web.HttpContext.Current.Server.UrlEncode(fileName);

                    sb.Append("<tr>");
                    sb.AppendFormat("<td style='width:20px'> <img border='0' width='16px' height='16px' src='/Content/images/delete.gif' onclick=\"deleteAttach('{0}')\"/> </td> ", info.ID);
                    sb.AppendFormat(@"<td style='width:300px'> <li><span>[ 附件{0} ]</span>", seq++);
                    sb.AppendFormat(@"<img border='0' width='16px' height='16px' src='/Content/Themes/Default/file_extension/{0}.png' />", info.FileExtend.Trim('.'));
                    sb.AppendFormat(@"<a href='/{0}?ext={1}' target='_blank'>&nbsp;{2}</a></li> </td> ", GetFilePath(info), info.FileExtend.Trim('.'), info.FileName);
                    sb.Append("</tr>");
                }

                string result = string.Format("<table style='border:0px solid #A8CFEB;'>{0}</table>", sb.ToString());
                return Content(result);
            }
            else
            {
                return Content(html);
            }
        }
    }
}
