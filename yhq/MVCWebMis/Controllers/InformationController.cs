﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;
using WHC.Framework.Commons;
using WHC.MVCWebMis.Entity;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;
using WHC.Pager.Entity;
using System.IO;
using System.Web.UI;
using Aspose.Words;

namespace WHC.MVCWebMis.Controllers
{
    public class InformationController : BusinessController<Information, InformationInfo>
    {
        /// <summary>
        /// 默认构造函数，并初始化权限控制ID
        /// </summary>
        public InformationController() : base()
        {
            base.AuthorizeKey.InsertKey = "Information/Add";
            //base.AuthorizeKey.UpdateKey = "Information/Edit";  
            //base.AuthorizeKey.DeleteKey = "Information/Delete";
        }


        public override ActionResult FindWithPager()
        {
            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            List<InformationInfo> list = baseBLL.FindWithPager(where, pagerInfo);

            //把List里面的对象附件信息转义
            for (int i = 0; i < list.Count; i++)
            {
                InformationInfo info = list[i];
                if (info != null)
                {
                    info.Attachment_GUID = GetAttachmentHtml(info.Attachment_GUID);
                }
            }

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = list };
            return JsonDate(result);
        }

        /// <summary>
        /// 获取附件的HTML代码
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private string GetAttachmentHtml(string guid)
        {
            string html = @"<li>附件暂无</li>";

            if (string.IsNullOrEmpty(guid))
                return html;

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

            return sb.ToString();
        }

        private string GetFilePath(FileUploadInfo info)
        {
            string filePath = BLLFactory<FileUpload>.Instance.GetFilePath(info);
            return HttpUtility.UrlPathEncode(filePath);
        }

        [ValidateInput(false)]
        public override ActionResult Insert(InformationInfo info)
        {
            info.Editor = CurrentUser.Name;
            info.EditTime = DateTime.Now;

            return base.Insert(info);
        }

        [ValidateInput(false)]
        public override ActionResult Update(string id, FormCollection formValues)
        {
            return base.Update(id, formValues);
        }

        /// <summary>
        /// 重写删除函数，并把相关附件删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public override ActionResult DeleteByIds(string ids)
        {
            //删除关联的附件
            if (!string.IsNullOrEmpty(ids))
            {
                string[] idArray = ids.Split(',');
                foreach (string id in idArray)
                {
                    InformationInfo info = baseBLL.FindByID(id);
                    if (info != null && !string.IsNullOrEmpty(info.Attachment_GUID))
                    {
                        BLLFactory<FileUpload>.Instance.DeleteByAttachGUID(info.Attachment_GUID);
                    }
                }
            }

            return base.DeleteByIds(ids);
        }

        public override ActionResult Index()
        {
            return Information();
        }

        public ActionResult PolicyLaw()
        {
            ViewBag.Category = "政策法规";
            return View("PolicyLaw");
        }

        public ActionResult Information()
        {
            ViewBag.Category = "通知公告";
            return View("Information");
        }

        public ActionResult IndustryNews()
        {
            ViewBag.Category = "动态信息";
            return View("IndustryNews");
        }

        [ValidateInput(false)]
        public FileStreamResult ExportExcel(string htmlString, string fileName)
        {
            //确保加上后缀名
            if (fileName.IndexOf(".xls", StringComparison.OrdinalIgnoreCase) < 0)
            {
                fileName = fileName + ".xls";
            }

            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.Charset = "GB2312";
            response.ContentEncoding = Encoding.GetEncoding("GB2312");
            response.ContentType = "application/ms-excel/msword";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
            StringWriter writer = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);

            writer2.WriteFullBeginTag("html");
            writer2.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=GB2312\">");
            writer2.WriteLine(htmlString);
            writer2.WriteEndTag("html");

            response.Write(writer.ToString());
            response.Flush();
            response.End();
            return new FileStreamResult(response.OutputStream, "application/ms-excel/msword");
        }

        [ValidateInput(false)]
        public FileStreamResult ExportWord(string htmlString, string fileName)
        {
            //确保加上后缀名
            if (fileName.IndexOf(".doc", StringComparison.OrdinalIgnoreCase) < 0)
            {
                fileName = fileName + ".doc";
            }

            HttpResponseBase response = ControllerContext.HttpContext.Response;
            response.Charset = "GB2312";
            response.ContentEncoding = Encoding.GetEncoding("GB2312");
            response.ContentType = "application/ms-word";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
            StringWriter writer = new StringWriter();
            HtmlTextWriter writer2 = new HtmlTextWriter(writer);

            writer2.WriteFullBeginTag("html");
            writer2.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=GB2312\">");
            writer2.WriteLine(htmlString);
            writer2.WriteEndTag("html");

            response.Write(writer.ToString());
            response.Flush();
            response.End();

            return new FileStreamResult(Response.OutputStream, "application/ms-word");
        }

        public FileStreamResult ExportWordById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            InformationInfo info = BLLFactory<Information>.Instance.FindByID(id);
            if (info != null)
            {


                string template = "~/Content/Template/政策法规模板.doc";
                string templateFile = Server.MapPath(template);
                Aspose.Words.Document doc = new Aspose.Words.Document(templateFile);

                #region 使用文本方式替换
                //Dictionary<string, string> dictSource = new Dictionary<string, string>();
                //dictSource.Add("Title", info.Title);
                //dictSource.Add("Content", info.Content);
                //dictSource.Add("Editor", info.Editor);
                //dictSource.Add("EditTime", info.EditTime.ToString());
                //dictSource.Add("SubType", info.SubType);  
                //foreach (string name in dictSource.Keys)
                //{
                //    doc.Range.Replace(name, dictSource[name], true, true);
                //} 
                #endregion

                //使用书签方式替换
                SetBookmark(ref doc, "Title", info.Title);
                SetBookmark(ref doc, "Editor", info.Editor);
                SetBookmark(ref doc, "EditTime", info.EditTime.ToString());
                SetBookmark(ref doc, "SubType", info.SubType);
                
                //SetBookmark(ref doc, "Content", info.Content);
                //对于HTML内容，需要通过InsertHtml方式进行写入
                DocumentBuilder builder = new DocumentBuilder(doc);
                Aspose.Words.Bookmark bookmark = doc.Range.Bookmarks["Content"];
                if (bookmark != null)
                {
                    builder.MoveToBookmark(bookmark.Name);
                    builder.InsertHtml(info.Content);
                }                
                
                doc.Save(System.Web.HttpContext.Current.Response, info.Title, Aspose.Words.ContentDisposition.Attachment,
                    Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));

                HttpResponseBase response = ControllerContext.HttpContext.Response;
                response.Flush();
                response.End();
                return new FileStreamResult(Response.OutputStream, "application/ms-word");
            }
            return null;
        }

        private void SetBookmark(ref Aspose.Words.Document doc, string title, string value)
        {
            Aspose.Words.Bookmark bookmark = doc.Range.Bookmarks[title];
            if (bookmark != null)
            {
                bookmark.Text = value;
            }
        }
    }
}
