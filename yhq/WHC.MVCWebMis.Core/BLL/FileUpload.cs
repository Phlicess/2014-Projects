using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;
using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.BLL
{
    /// <summary>
    /// 上传文件信息
    /// </summary>
    public class FileUpload : BaseBLL<FileUploadInfo>
    {
        public FileUpload()
            : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="info">文件信息（包含流数据）</param>
        /// <returns></returns>
        public CommonResult Upload(FileUploadInfo info)
        {
            CommonResult result = new CommonResult();

            try
            {
                #region 确定相对目录，然后上传文件

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

                #endregion

                if (!string.IsNullOrEmpty(relativeSavePath))
                {
                    info.SavePath = relativeSavePath.Trim('\\');
                    info.AddTime = DateTime.Now;

                    try
                    {
                        bool success = base.Insert(info);
                        if (success)
                        {
                            result.Success = success;
                        }
                        else
                        {
                            result.ErrorMessage = "数据写入数据库出错。";
                        }
                    }
                    catch (Exception ex)
                    {
                        //FileUtil.DeleteFile(filePath);
                        result.ErrorMessage = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取指定附件组GUID的附件信息
        /// </summary>
        /// <param name="attachmentGUID">附件组GUID</param>
        /// <returns></returns>
        public List<FileUploadInfo> GetByAttachGUID(string attachmentGUID)
        {
            IFileUpload dal = baseDal as IFileUpload;
            return dal.GetByAttachGUID(attachmentGUID);
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
                return relativeSavePath;
            }
            else
            {
                return string.Empty;
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
                category = "Photo";
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
        /// 删除指定的ID记录，如果是相对目录的文件则移除文件到DeletedFiles文件夹里面
        /// </summary>
        /// <param name="key">记录ID</param>
        /// <returns></returns>
        public override bool Delete(object key)
        {
            //删除记录前，需要把文件移动到删除目录下面
            FileUploadInfo info = FindByID(key);
            if (info != null && !string.IsNullOrEmpty(info.SavePath))
            {
                string serverRealPath = Path.Combine(info.BasePath, info.SavePath.Trim('\\'));
                if (!Path.IsPathRooted(serverRealPath))
                {
                    //如果是相对目录，加上当前程序的目录才能定位文件地址
                    serverRealPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, serverRealPath);

                    //如果是相对目录的，移动到删除目录里面
                    if (File.Exists(serverRealPath))
                    {
                        try
                        {
                            string deletedPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Path.Combine(info.BasePath, "DeletedFiles"));
                            DirectoryUtil.AssertDirExist(deletedPath);

                            string newFilePath = Path.Combine(deletedPath, info.FileName);
                            newFilePath = GetRightFileName(newFilePath, 1);
                            File.Move(serverRealPath, newFilePath);
                        }
                        catch (Exception ex)
                        {
                            LogTextHelper.Error(ex);
                        }
                    }
                }
            }

            return base.Delete(key);
        }

        /// <summary>
        /// 删除指定Attachment_GUID的数据记录
        /// </summary>
        /// <param name="attachment_GUID">所属者的ID</param>
        /// <returns></returns>
        public bool DeleteByAttachGUID(string attachment_GUID)
        {
            string condition = string.Format("AttachmentGUID ='{0}' ", attachment_GUID);
            List<FileUploadInfo> list = base.Find(condition);
            foreach (FileUploadInfo info in list)
            {
                Delete(info.ID);
            }
            return true;
        }
    }
}
