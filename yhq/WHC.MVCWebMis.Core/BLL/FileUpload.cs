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
    /// �ϴ��ļ���Ϣ
    /// </summary>
    public class FileUpload : BaseBLL<FileUploadInfo>
    {
        public FileUpload()
            : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// �ϴ��ļ�
        /// </summary>
        /// <param name="info">�ļ���Ϣ�����������ݣ�</param>
        /// <returns></returns>
        public CommonResult Upload(FileUploadInfo info)
        {
            CommonResult result = new CommonResult();

            try
            {
                #region ȷ�����Ŀ¼��Ȼ���ϴ��ļ�

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
                            result.ErrorMessage = "����д�����ݿ����";
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
        /// ��ȡָ��������GUID�ĸ�����Ϣ
        /// </summary>
        /// <param name="attachmentGUID">������GUID</param>
        /// <returns></returns>
        public List<FileUploadInfo> GetByAttachGUID(string attachmentGUID)
        {
            IFileUpload dal = baseDal as IFileUpload;
            return dal.GetByAttachGUID(attachmentGUID);
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
                return relativeSavePath;
            }
            else
            {
                return string.Empty;
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
                category = "Photo";
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
        /// ɾ��ָ����ID��¼����������Ŀ¼���ļ����Ƴ��ļ���DeletedFiles�ļ�������
        /// </summary>
        /// <param name="key">��¼ID</param>
        /// <returns></returns>
        public override bool Delete(object key)
        {
            //ɾ����¼ǰ����Ҫ���ļ��ƶ���ɾ��Ŀ¼����
            FileUploadInfo info = FindByID(key);
            if (info != null && !string.IsNullOrEmpty(info.SavePath))
            {
                string serverRealPath = Path.Combine(info.BasePath, info.SavePath.Trim('\\'));
                if (!Path.IsPathRooted(serverRealPath))
                {
                    //��������Ŀ¼�����ϵ�ǰ�����Ŀ¼���ܶ�λ�ļ���ַ
                    serverRealPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, serverRealPath);

                    //��������Ŀ¼�ģ��ƶ���ɾ��Ŀ¼����
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
        /// ɾ��ָ��Attachment_GUID�����ݼ�¼
        /// </summary>
        /// <param name="attachment_GUID">�����ߵ�ID</param>
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
