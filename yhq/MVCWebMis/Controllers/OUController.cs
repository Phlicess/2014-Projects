using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Common;
using WHC.MVCWebMis.Entity;

namespace WHC.MVCWebMis.Controllers
{
    public class OUController : BusinessController<OU, OUInfo>
    {
        public OUController() : base()
        {
        }

        public ActionResult GetListItems()
        {
            List<CListItem> listItem = new List<CListItem>();
            List<OUInfo> list = BLLFactory<OU>.Instance.GetAll();
            foreach (WHC.MVCWebMis.Entity.OUInfo info in list)
            {
                listItem.Add(new CListItem(info.Name, info.ID.ToString()));
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTreeList()
        {
            List<OUInfo> comboList = BLLFactory<OU>.Instance.GetAll();
            comboList = CollectionHelper<OUInfo>.Fill(-1, 0, comboList, "PID", "ID", "Name");
            return Json(comboList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUserRelation(string ouid, string addList, string removeList)
        {
            if (!string.IsNullOrEmpty(ouid) && ValidateUtil.IsValidInt(ouid))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<OU>.Instance.RemoveUser(Convert.ToInt32(id), Convert.ToInt32(ouid));
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<OU>.Instance.AddUser(Convert.ToInt32(id), Convert.ToInt32(ouid));
                        }
                    }
                }

                return Content("true");
            }
            return Content("");
        }

        public ActionResult GetOUsByRole(string roleid)
        {
            if (!string.IsNullOrEmpty(roleid) && ValidateUtil.IsValidInt(roleid))
            {
                List<OUInfo> list = BLLFactory<OU>.Instance.GetOUsByRole(Convert.ToInt32(roleid));
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }

        public ActionResult GetOUsByUser(string userid)
        {
            if (!string.IsNullOrEmpty(userid) && ValidateUtil.IsValidInt(userid))
            {
                //List<OUInfo> ouList = BLLFactory<OU>.Instance.GetOUsByUser(Convert.ToInt32(userid));
                //return Json(ouList, JsonRequestBehavior.AllowGet);
                List<XueYuanInfo> ouList = BLLFactory<XueYuan>.Instance.GetOUsByUser(Convert.ToInt32(userid));
                return Json(ouList, JsonRequestBehavior.AllowGet);
            }

            return Content("");
        }

        public override ActionResult Insert(OUInfo info)
        {
            if (info.ID > 0)
            {
                string filter = string.Format("Name='{0}' and ID <>'{1}'", info.Name, info.ID);
                bool isExist = BLLFactory<OU>.Instance.IsExistRecord(filter);
                if (isExist)
                {
                    throw new ArgumentException("指定机构名称重复，请重新输入！");
                }

                OUInfo tempInfo = BLLFactory<OU>.Instance.FindByID(info.ID);
                if (tempInfo != null)
                {
                    tempInfo.Name = info.Name;
                    tempInfo.Address = info.Address;
                    tempInfo.Note = info.Note;
                    tempInfo.PID = info.PID;

                    bool sucess = BLLFactory<OU>.Instance.Update(tempInfo, tempInfo.ID.ToString());
                    return Content(sucess);
                }
            }
            else
            {
                string filter = string.Format("Name='{0}' ", info.Name);
                bool isExist = BLLFactory<OU>.Instance.IsExistRecord(filter);
                if (isExist)
                {
                    throw new ArgumentException("指定机构名称重复，请重新输入！");
                }

                OUInfo tempInfo = new OUInfo();
                tempInfo.Name = info.Name;
                tempInfo.Address = info.Address;
                tempInfo.Note = info.Note;
                tempInfo.PID = info.PID;

                bool success = BLLFactory<OU>.Instance.Insert(tempInfo);
                return Content(success);
            }
            return Content("");
        }

        public ActionResult GetTreeJson()
        {
            string folder = "/Content/JqueryEasyUI/themes/icons/customed/" + "organ.png";
            string leaf = "/Content/JqueryEasyUI/themes/icons/customed/" + "organ.png";
            string json = GetTreeJson(-1, folder, leaf);
            json = json.Trim(',');
            return Content(string.Format("[{0}]", json));
        }

        /// <summary>
        /// 递归获取树形信息
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        private string GetTreeJson(int PID, string folderIcon, string leafIcon)
        {
            string condition = string.Format("PID={0}", PID);
            List<OUInfo> nodeList = BLLFactory<OU>.Instance.Find(condition);
            StringBuilder content = new StringBuilder();
            foreach (OUInfo model in nodeList)
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

    }
}
