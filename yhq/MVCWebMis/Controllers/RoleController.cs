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
    /// <summary>
    /// 角色业务操作控制器
    /// </summary>
    public class RoleController : BusinessController<Role, RoleInfo>
    {
        public RoleController() : base()
        {
        }

        public ActionResult GetTreeJson()
        {
            string leaf = "/Content/JqueryEasyUI/themes/icons/customed/" + "group.png";
            string json = GetTreeJson(leaf);
            json = json.Trim(',');
            return Content(string.Format("[{0}]", json));
        }

        /// <summary>
        /// 递归获取树形信息
        /// </summary>
        /// <returns></returns>
        private string GetTreeJson(string leafIcon)
        {
            List<RoleInfo> nodeList = BLLFactory<Role>.Instance.GetAll();
            StringBuilder content = new StringBuilder();
            foreach (RoleInfo model in nodeList)
            {
                string json = string.Format("{{ \"id\":{0}, \"name\":\"{1}\" ", model.ID, model.Name);
                if (!string.IsNullOrEmpty(leafIcon))
                {
                    json += string.Format(",\"icon\":\"{0}\" }},", leafIcon);
                }
                else
                {
                    json += "},";
                }

                content.AppendLine(json.Trim());
            }

            return content.ToString().Trim();
        } 

        public ActionResult EditUserRelation(string roleId, string addList, string removeList)
        {
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<Role>.Instance.RemoveUser(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<Role>.Instance.AddUser(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }

                return Content("true");
            }
            return Content("");
        }

        public ActionResult EditOURelation(string roleId, string addList, string removeList)
        {
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<Role>.Instance.RemoveOU(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<Role>.Instance.AddOU(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }
                return Content("true");
            }
            return Content("");
        }

        public ActionResult EditFunctionRelation(string roleId, string addList, string removeList)
        {
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<Role>.Instance.RemoveFunction(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<Role>.Instance.AddFunction(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }
                return Content("true");
            }
            return Content("");
        }

        public ActionResult GetRolesByUser(string userid)
        {
            if (!string.IsNullOrEmpty(userid) && ValidateUtil.IsValidInt(userid))
            {
                List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByUser(Convert.ToInt32(userid));
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }

            return Content("");
        }

        public ActionResult GetRolesByFunction(string functionId)
        {
            if (!string.IsNullOrEmpty(functionId) && ValidateUtil.IsValidInt(functionId))
            {
                List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByFunction(Convert.ToInt32(functionId));
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }

        public ActionResult GetRolesByOU(string ouid)
        {
            if (!string.IsNullOrEmpty(ouid) && ValidateUtil.IsValidInt(ouid))
            {
                List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByOU(Convert.ToInt32(ouid));
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }

        public override  ActionResult Insert(RoleInfo roleInfo)
        {
            if (roleInfo.ID > 0)
            {
                string filter = string.Format("Name='{0}' and ID <> '{1}'", roleInfo.Name, roleInfo.ID);
                bool isExist = BLLFactory<Role>.Instance.IsExistRecord(filter);
                if (isExist)
                {
                    throw new ArgumentException("指定角色名称重复，请重新输入！");
                }

                RoleInfo info = BLLFactory<Role>.Instance.FindByID(roleInfo.ID);
                if (info != null)
                {
                    info.Name = roleInfo.Name;
                    info.Note = roleInfo.Note;

                    bool sucess = BLLFactory<Role>.Instance.Update(info, info.ID.ToString());
                    return Content(sucess);
                }
            }
            else
            {
                string filter = string.Format("Name='{0}' ", roleInfo.Name);
                bool isExist = BLLFactory<Role>.Instance.IsExistRecord(filter);
                if (isExist)
                {
                    throw new ArgumentException("指定角色名称重复，请重新输入！");
                }

                RoleInfo info = new RoleInfo();
                info.Name = roleInfo.Name;
                info.Note = roleInfo.Note;

                bool sucess = BLLFactory<Role>.Instance.Insert(info);
                return Content(sucess);
            }
            return Content("");
        }
    }
}
