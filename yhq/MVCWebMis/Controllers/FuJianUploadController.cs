using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebGrease.Css;



namespace WHC.MVCWebMis.Controllers
{
    public class FuJianUploadController : Controller
    {


        public void uploadnow(object sender, EventArgs e)
        {
            if (FileUpload.HasFile)
            {
                string filename = FileUpload.PostedFile.FileName;
                string extension = (new FileInfo(filename)).Extension;
                string newfilename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                Response.Write("原文件名" + filename + "<br/>");
                Response.Write("新文件名" + newfilename + "<br/>");
                string path = Server.MapPath("/userloadfile/");
                FileUpload.PostedFile.SaveAs(path + newfilename);
                Response.Write("<script>alert('上传成功！');</script>");

            }
        }



    }

   
}
