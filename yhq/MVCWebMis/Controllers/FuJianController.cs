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

                    // �ļ��ϴ���ı���·��
                    string filePath = Server.MapPath("~/UploadFiles/FuJian");
                    DirectoryUtil.AssertDirExist(filePath);

                    string fileName = Path.GetFileName(fileData.FileName);      //ԭʼ�ļ�����
                    string fileExtension = Path.GetExtension(fileName);         //�ļ���չ��
                    string saveName = Guid.NewGuid().ToString() + fileExtension; //�����ļ�����

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
                    info.Editor = CurrentUser.Name;//��¼��
                    //info.Owner_ID = OwerId;//���������¼ID

                    //CommonResult result = BLLFactory<FileUpload>.Instance.Upload(info);
                    CommonResult result = new CommonResult() { Success = false };
                    string relativeSavePath = "";

                    //����ϴ���ʱ�� ��ָ���˻���·������ô�Ͳ����޸�
                    if (string.IsNullOrEmpty(info.BasePath))
                    {
                        //���ûָ������·������������Ϊ�������û��������AttachmentBasePath��Ĭ��һ�����Ŀ¼
                        AppConfig config = new AppConfig();
                        string AttachmentBasePath = config.AppConfigGet("AttachmentBasePath");//���õĻ���·��
                        if (string.IsNullOrEmpty(AttachmentBasePath))
                        {
                            //Ĭ���Ը�Ŀ¼�µ�UploadFilesĿ¼Ϊ�ϴ�Ŀ¼�� ����"C:\SPDTPatientMisService\UploadFiles";
                            info.BasePath = "UploadFiles"; //Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "UploadFiles");
                        }
                        else
                        {
                            info.BasePath = AttachmentBasePath;
                        }

                        //���ûָ������·��,�ͱ����ļ����ϴ�
                        relativeSavePath = UploadFile(info);
                    }
                    else
                    {
                        //���ָ���˻���·������ô����Winform���س��������ӣ�����Ҫ�ļ��ϴ�,���·�������ļ���
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
                        LogTextHelper.Error("�ϴ��ļ�ʧ��:" + result.ErrorMessage);
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
        /// ���ļ����浽ָ��Ŀ¼,��������Ի���Ŀ¼��·��
        /// </summary>
        /// <param name="info">�ļ��ϴ���Ϣ</param>
        /// <returns>�ɹ�������Ի���Ŀ¼��·�������򷵻ؿ��ַ�</returns>
        private string UploadFile(FileUploadInfo info)
        {
            //������뼰���·��
            string filePath = GetFilePath(info);
            string relativeSavePath = filePath.Replace(info.BasePath, "").Trim('\\');//�滻����ʼĿ¼��Ϊ���·��

            string serverRealPath = filePath;
            if (!Path.IsPathRooted(filePath))
            {
                serverRealPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filePath);
            }

            //ͨ��ʵ���ļ���ȥ���Ҷ�Ӧ���ļ�����
            serverRealPath = GetRightFileName(serverRealPath, 1);
            //����ʵ���ļ��������ļ�
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
        /// �����ļ�����������������ļ��������(i)��i��1��ʼ����
        /// </summary>
        /// <param name="originalFileName">ԭ�ļ���</param>
        /// <param name="i">����ֵ</param>
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
        /// ������뼰���·��
        /// </summary>
        /// <param name="info">�ϴ��ļ���Ϣ</param>
        /// <returns></returns>
        public string GetFilePath(FileUploadInfo info)
        {
            string fileName = info.FileName;
            string category = info.Category;

            if (string.IsNullOrEmpty(category))
            {
                category = "FuJian";
            }

            //��������Ŀ¼����
            string uploadFolder = Path.Combine(info.BasePath, category);
            string realFolderPath = uploadFolder;

            //���Ŀ¼Ϊ���Ŀ¼����ôת��Ϊʵ��Ŀ¼��������
            if (!Path.IsPathRooted(uploadFolder))
            {
                realFolderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, uploadFolder);
            }
            if (!Directory.Exists(realFolderPath))
            {
                Directory.CreateDirectory(realFolderPath);
            }

            //�������Ŀ¼
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
