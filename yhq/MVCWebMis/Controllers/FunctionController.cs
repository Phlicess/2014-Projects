using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;
using WHC.Framework.Commons;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Common;
using WHC.MVCWebMis.Entity;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Controllers
{
    public class FunctionController : BusinessController<Function, FunctionInfo>
    {
        public FunctionController() :base()
        {
        }

        protected override void ConvertAuthorizedInfo()
        {
            //base.ConvertAuthorizedInfo();
        }

        public ActionResult GetTreeList()
        {
            List<FunctionInfo> comboList = BLLFactory<Function>.Instance.GetAll();
            comboList = CollectionHelper<FunctionInfo>.Fill(-1, 0, comboList, "PID", "ID", "Name");
            return Json(comboList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取功能的树形展示数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeJson()
        {
            string json = GetTreeJson(-1, "", "");
            json = json.Trim(',');
            return Content(string.Format("[{0}]", json));
        }
               
        /// <summary>
        /// 递归获取树形信息
        /// </summary>
        /// <returns></returns>
        private string GetTreeJson(int PID, string folderIcon, string leafIcon)
        {
            string condition = string.Format("PID={0}", PID);
            List<FunctionInfo> nodeList = BLLFactory<Function>.Instance.Find(condition);
            StringBuilder content = new StringBuilder();
            foreach (FunctionInfo model in nodeList)
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

        /// <summary>
        /// 获取指定角色的功能集合
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <returns></returns>
        public ActionResult GetFunctions(string roleid)
        {
            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(roleid) && ValidateUtil.IsValidInt(roleid))
            {
                List<FunctionInfo> roleList = BLLFactory<Function>.Instance.GetFunctionsByRole(Convert.ToInt32(roleid));
                result = Json(roleList, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        /// <summary>
        /// 新增或更新功能信息
        /// </summary>
        /// <param name="info">功能信息实体类</param>
        /// <returns></returns>
        public override ActionResult Insert(FunctionInfo info)
        {
            FunctionInfo parentInfo = BLLFactory<Function>.Instance.FindByID(info.PID);
            if (parentInfo != null)
            {
                if (info.ID > 0)
                {
                    string filter = string.Format("ControlID='{0}' and SystemType_ID='{1}' and ID <>'{2}'",
                        info.ControlID, parentInfo.SystemType_ID, info.ID);
                    bool isExist = BLLFactory<Function>.Instance.IsExistRecord(filter);
                    if (isExist)
                    {
                        throw new ArgumentException("指定功能控制ID重复，请重新输入！");
                    }

                    FunctionInfo tmpInfo = BLLFactory<Function>.Instance.FindByID(info.ID);
                    if (tmpInfo != null)
                    {
                        tmpInfo.Name = info.Name;
                        tmpInfo.ControlID = info.ControlID;
                        tmpInfo.PID = info.PID;
                        tmpInfo.SystemType_ID = parentInfo.SystemType_ID;

                        bool sucess = BLLFactory<Function>.Instance.Update(tmpInfo, tmpInfo.ID.ToString());
                        return Content(sucess);
                    }
                }
                else
                {
                    string filter = string.Format("ControlID='{0}' and SystemType_ID='{1}'", info.ControlID, parentInfo.SystemType_ID);
                    bool isExist = BLLFactory<Function>.Instance.IsExistRecord(filter);
                    if (isExist)
                    {
                        throw new ArgumentException("指定功能控制ID重复，请重新输入！");
                    }

                    FunctionInfo tmpInfo = new FunctionInfo();
                    tmpInfo.Name = info.Name;
                    tmpInfo.ControlID = info.ControlID;
                    tmpInfo.PID = info.PID;
                    tmpInfo.SystemType_ID = parentInfo.SystemType_ID;

                    bool sucess = BLLFactory<Function>.Instance.Insert(tmpInfo);
                    return Content(sucess);
                }
            }
            return Content("");
        }
    }
}
