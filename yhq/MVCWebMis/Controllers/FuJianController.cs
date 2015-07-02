using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WHC.Framework.ControlUtil;
using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;

using System.Data;


namespace WHC.MVCWebMis.Controllers
{
    public class FuJianController : BusinessController<FuJian, FuJianInfo>
    {
        public FuJianController() : base()
        {
        }

        [AcceptVerbs(HttpVerbs.Post)]

        public string Upload(HttpPostedFileBase fileData, int XiangMu_ID, string ShuoMing)
        {
            if (fileData != null)
            {
                try
                {
                    ControllerContext.HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.Charset = "UTF-8";

                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/UploadFiles/FuJian");
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
                    info.Category = "FuJian";
                    info.FileName = fileName;
                    info.FileExtend = fileExtension;
                    info.AddTime = DateTime.Now;
                    info.Editor = CurrentUser.Name;//登录人
                    //info.Owner_ID = OwerId;//所属主表记录ID

                    //CommonResult result = BLLFactory<FileUpload>.Instance.Upload(info);
                    CommonResult result = new CommonResult() { Success = false };
                    string relativeSavePath = "";

                    //如果上传的时候 ，指定了基础路径，那么就不需修改
                    if (string.IsNullOrEmpty(info.BasePath))
                    {
                        //如果没指定基础路径，则以配置为主，如果没有配置项AttachmentBasePath，默认一个相对目录
                        AppConfig config = new AppConfig();
                        string AttachmentBasePath = config.AppConfigGet("AttachmentBasePath");//配置的基础路径
                        if (string.IsNullOrEmpty(AttachmentBasePath))
                        {
                            //默认以根目录下的UploadFiles目录为上传目录， 例如"C:\SPDTPatientMisService\UploadFiles";
                            info.BasePath = "UploadFiles"; //Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "UploadFiles");
                        }
                        else
                        {
                            info.BasePath = AttachmentBasePath;
                        }

                        //如果没指定基础路径,就表明文件须上传
                        relativeSavePath = UploadFile(info);
                    }
                    else
                    {
                        //如果指定了基础路径，那么属于Winform本地程序复制链接，不需要文件上传,相对路径就是文件名
                        relativeSavePath = info.FileName;
                    }
                    result.Success = true;

                    FuJianInfo postData = new FuJianInfo();
                    postData.LuJing = relativeSavePath;
                    postData.ShuoMing = ShuoMing;
                    postData.XiangMu_Id = XiangMu_ID;


                    BLLFactory<FuJian>.Instance.Insert(postData);

                    if (!result.Success)
                    {
                        LogTextHelper.Error("上传文件失败:" + result.ErrorMessage);
                    }
                    return relativeSavePath;
                }
                catch (Exception ex)
                {
                    LogTextHelper.Error(ex);
                    return "false";
                }
            }
            else
            {
                return "false";
            }
        }



        /// <summary>
        /// 把文件保存到指定目录,并返回相对基础目录的路径
        /// </summary>
        /// <param name="info">文件上传信息</param>
        /// <returns>成功返回相对基础目录的路径，否则返回空字符</returns>
        private string UploadFile(FileUploadInfo info)
        {
            //检查输入及组合路径
            string filePath = GetFilePath(info);
            string relativeSavePath = filePath.Replace(info.BasePath, "").Trim('\\');//替换掉起始目录即为相对路径

            string serverRealPath = filePath;
            if (!Path.IsPathRooted(filePath))
            {
                serverRealPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filePath);
            }

            //通过实际文件名去查找对应的文件名称
            serverRealPath = GetRightFileName(serverRealPath, 1);
            //根据实际文件名创建文件
            FileUtil.CreateFile(serverRealPath, info.FileData);

            bool success = FileUtil.IsExistFile(serverRealPath);
            if (success)
            {
                return serverRealPath;
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 查找文件名，如果存在则在文件名后面加(i)，i从1开始计算
        /// </summary>
        /// <param name="originalFileName">原文件名</param>
        /// <param name="i">计数值</param>
        /// <returns></returns>
        private string GetRightFileName(string originalFilePath, int i)
        {
            bool fileExist = FileUtil.IsExistFile(originalFilePath);
            if (fileExist)
            {
                string onlyFileName = FileUtil.GetFileName(originalFilePath, true);
                int idx = originalFilePath.LastIndexOf(onlyFileName);
                string firstPath = originalFilePath.Substring(0, idx);
                string onlyExt = FileUtil.GetExtension(originalFilePath);
                string newFileName = string.Format("{0}{1}({2}){3}", firstPath, onlyFileName, i, onlyExt);
                if (FileUtil.IsExistFile(newFileName))
                {
                    i++;
                    return GetRightFileName(originalFilePath, i);
                }
                else
                {
                    return newFileName;
                }
            }
            else
            {
                return originalFilePath;
            }
        }



        /// <summary>
        /// 检查输入及组合路径
        /// </summary>
        /// <param name="info">上传文件信息</param>
        /// <returns></returns>
        public string GetFilePath(FileUploadInfo info)
        {
            string fileName = info.FileName;
            string category = info.Category;

            if (string.IsNullOrEmpty(category))
            {
                category = "FuJian";
            }

            //以类别进行目录区分
            string uploadFolder = Path.Combine(info.BasePath, category);
            string realFolderPath = uploadFolder;

            //如果目录为相对目录，那么转换为实际目录，并创建
            if (!Path.IsPathRooted(uploadFolder))
            {
                realFolderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, uploadFolder);
            }
            if (!Directory.Exists(realFolderPath))
            {
                Directory.CreateDirectory(realFolderPath);
            }

            //返回相对目录
            string filePath = Path.Combine(uploadFolder, fileName);
            return filePath;
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

    }
}
