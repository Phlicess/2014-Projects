using System;
using System.Collections.Generic;
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
    public class UserController : BusinessController<User, UserInfo>
    {
        public UserController() : base()
        {
        }

        public ActionResult GetListItems()
        {
            List<CListItem> listItem = new List<CListItem>();
            List<UserInfo> list = BLLFactory<User>.Instance.GetAll();
            foreach (WHC.MVCWebMis.Entity.UserInfo info in list)
            {
                listItem.Add(new CListItem(info.Name, info.ID.ToString()));
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置用户密码为12345678
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public ActionResult ResetPassword(string id)
        {
            string result = "口令初始化失败";
            if (string.IsNullOrEmpty(id))
            {
                result = "用户id不能为空";
            }
            else
            {
                UserInfo info = BLLFactory<User>.Instance.FindByID(id);
                if (info != null)
                {
                    string defaultPassword = "12345678";
                    bool tempBool = BLLFactory<User>.Instance.ModifyPassword(info.Name, defaultPassword);
                    if (tempBool)
                    {
                        result = "OK";
                    }
                    else
                    {
                        result = "口令初始化失败";
                    }
                }
            }
            return Content(result);
        }

        public ActionResult ModifyPass(string name, string oldpass, string newpass)
        {
            #region MyRegion
            //string result = "";
            //if (oldpass != "123")
            //{
            //    result = "原口令错误";
            //}
            //else if (string.IsNullOrWhiteSpace(newpass))
            //{
            //    result = "新密码不能为空";
            //}
            //else
            //{
            //    result = "OK";
            //}
            //return Content(result); 
            #endregion

            string result = "";
            string identity = BLLFactory<User>.Instance.VerifyUser(name, oldpass, "WareMis");
            if (string.IsNullOrEmpty(identity))
            {
                result = "原口令错误";
            }

            bool tempBool = BLLFactory<User>.Instance.ModifyPassword(name, newpass);
            if (tempBool)
            {
                result = "OK";
            }
            else
            {
                result = "口令修改失败";
            }
            return Content(result);
        }

        /// <summary>
        /// 获取用户的树形展示数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeJson()
        {
            string folder = "/Content/JqueryEasyUI/themes/icons/customed/" + "user.png";
            string leaf = "/Content/JqueryEasyUI/themes/icons/customed/" + "user.png";
            string json = GetTreeJson(-1, folder, leaf);
            json = json.Trim(',');
            return Content(string.Format("[{0}]", json));
        }

        /// <summary>
        /// 递归获取树形信息
        /// </summary>
        private string GetTreeJson(int PID, string folderIcon, string leafIcon)
        {
            string condition = string.Format("PID={0}", PID);
            List<UserInfo> nodeList = BLLFactory<User>.Instance.Find(condition);
            StringBuilder content = new StringBuilder();
            foreach (UserInfo model in nodeList)
            {
                int ParentID = (model.PID == -1 ? 0 : model.PID);
                //string tempMenu = string.Format("{{ id:{0}, pId:{1}, name:\"{2}\",icon:\"{3}\" }},", model.ID, ParentID, model.Name, imgsrc);
                string subMenu = this.GetTreeJson(model.ID, folderIcon, leafIcon);
                string parentMenu = string.Format("{{ \"id\":{0}, \"pId\":{1}, \"name\":\"{2}\" ", model.ID, ParentID, model.Name);
                if (string.IsNullOrEmpty(subMenu))
                {
                    if (!string.IsNullOrEmpty(leafIcon))
                    {
                        parentMenu += string.Format(",\"icon\":\"{0}\" }},", leafIcon);
                    }
                    else
                    {
                        parentMenu += "},";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(folderIcon))
                    {
                        parentMenu += string.Format(",\"icon\":\"{0}\" }},", folderIcon);
                    }
                    else
                    {
                        parentMenu += "},";
                    }
                }

                content.AppendLine(parentMenu.Trim());
                content.AppendLine(subMenu.Trim());
            }

            return content.ToString().Trim();
        }

        public override ActionResult Insert(UserInfo userInfo)
        {
            if (userInfo.ID > 0)
            {
                string filter = string.Format("Name='{0}' and ID <>'{1}'", userInfo.Name, userInfo.ID);
                bool isExist = BLLFactory<User>.Instance.IsExistRecord(filter);
                if (isExist)
                {
                    throw new ArgumentException("指定用户名重复，请重新输入！");
                }

                UserInfo info = BLLFactory<User>.Instance.FindByID(userInfo.ID);
                if (info != null)
                {
                    info.Name = userInfo.Name;
                    info.FullName = userInfo.FullName;
                    if (!"admin".Equals(info.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        info.PID = userInfo.PID;
                    }
                    info.Address = userInfo.Address;
                    info.Email = userInfo.Email;
                    info.HomePhone = userInfo.HomePhone;
                    info.IdentityCard = userInfo.IdentityCard;
                    info.MobilePhone = userInfo.MobilePhone;
                    info.OfficePhone = userInfo.OfficePhone;
                    info.Title = userInfo.Title;
                    info.IsExpire = userInfo.IsExpire;
                    info.Birthday = Convert.ToDateTime("1900-1-1");
                    string password = userInfo.Password;
                    if (!string.IsNullOrEmpty(password))
                    {
                        info.Password = password;
                    }
                    info.Dept_ID = userInfo.Dept_ID;

                    bool sucess = BLLFactory<User>.Instance.Update(info, info.ID.ToString());
                    return Content(sucess);
                }
            }
            else
            {
                string filter = string.Format("Name='{0}' ", userInfo.Name);
                bool isExist = BLLFactory<User>.Instance.IsExistRecord(filter);
                if (isExist)
                {
                    throw new ArgumentException("指定用户名重复，请重新输入！");
                }

                UserInfo info = new UserInfo();
                info.Name = userInfo.Name;
                info.FullName = userInfo.FullName;
                if (!"admin".Equals(info.Name, StringComparison.OrdinalIgnoreCase))
                {
                    info.PID = userInfo.PID;
                }
                info.Address = userInfo.Address;
                info.Email = userInfo.Email;
                info.HomePhone = userInfo.HomePhone;
                info.IdentityCard = userInfo.IdentityCard;
                info.MobilePhone = userInfo.MobilePhone;
                info.OfficePhone = userInfo.OfficePhone;
                info.Title = userInfo.Title;
                info.IsExpire = userInfo.IsExpire;
                info.Birthday = Convert.ToDateTime("1900-1-1");

                bool sucess = BLLFactory<User>.Instance.Insert(info);
                return Content(sucess);
            }
            return Content("");
        }

        public ActionResult GetUsersByRole(string roleid)
        {
            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(roleid) && ValidateUtil.IsValidInt(roleid))
            {
                List<UserInfo> roleList = BLLFactory<User>.Instance.GetUsersByRole(Convert.ToInt32(roleid));
                result = JsonDate(roleList);
            }
            return result;
        }

        public ActionResult GetUsersByOU(string ouid)
        {
            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(ouid) && ValidateUtil.IsValidInt(ouid))
            {
                List<UserInfo> roleList = BLLFactory<User>.Instance.GetUsersByOU(Convert.ToInt32(ouid));
                result = JsonDate(roleList);
            }
            return result;
        }
        public virtual ActionResult DeletebyNames(string names)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.DeleteKey);

            bool result = false;
            if (!string.IsNullOrEmpty(names))
            {
                string[] nameArray = names.Split(new char[] { ',' });
                foreach (string strName in nameArray)
                {
                    if (!string.IsNullOrEmpty(strName))
                    {
                        UserInfo info = baseBLL.FindSingle(String.Format("Name = '{0}'", strName));
                        if (info != null) { 
                            baseBLL.Delete(info.ID);
                        }
                    }
                }
                result = true;
            }
            return Content(result);
        } 
    }
}
