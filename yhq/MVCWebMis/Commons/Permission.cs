using System;
using System.Web;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;

using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.BLL;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis
{
	/// <summary>
    /// Ȩ�޿�����
	/// </summary>
	public class Permission
	{		
		/// <summary>
		/// �жϵ�ǰ�û��Ƿ�ӵ��ĳ���ܵ��Ȩ��
		/// </summary>
		/// <param name="functionId"></param>
		/// <returns></returns>
		public static bool HasFunction(string functionId)
		{
            bool hasFunction = false;

            UserInfo CurrentUser = HttpContext.Current.Session["UserInfo"] as UserInfo;
            if (CurrentUser != null && CurrentUser.Name == "admin")
            {
                hasFunction = true;
            }
            else
            {
                if (string.IsNullOrEmpty(functionId))
                {
                    hasFunction = true;
                }
                else
                {
                    Dictionary<string, string> functionDict = HttpContext.Current.Session["Functions"] as Dictionary<string, string>;
                    if (functionDict != null && functionDict.ContainsKey(functionId))
                    {
                        hasFunction = true;
                    }
                }
            }
            return hasFunction;
		}

        /// <summary>
        /// �ж��Ƿ�Ϊϵͳ����Ա
        /// </summary>
        /// <returns>true:ϵͳ����Ա,false:����ϵͳ����Ա</returns>
        public static bool IsAdmin()
        {
            bool blnIsAdmin = false;
            UserInfo CurrentUser = HttpContext.Current.Session["UserInfo"] as UserInfo;
            if (CurrentUser != null)
            {
                //int groupID = Permission.CurrentUser.Dept_id;
                //string topGroupName = BLLFactory<Group>.Instance.GetTopGroupName(groupID);
                //if (topGroupName == "ͣ��������")
                //{
                //    blnIsAdmin = true;
                //}
                //else
                {
                    List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByUser(CurrentUser.ID);
                    if (roleList != null)
                    {
                        foreach (RoleInfo info in roleList)
                        {
                            if (info.Name == "ϵͳ����Ա")
                            {
                                blnIsAdmin = true;
                                break;
                            }
                        }
                    }
                }
            }
            return blnIsAdmin;
        }
	}

    public class TempPermit
    {
        public string LoginName { get; set; }
        public string CipherText { get; set; }
        public string PlainText { get; set; }
        public DateTime LoginTime { get; set; }
    }
}