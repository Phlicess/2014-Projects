using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Common;

namespace WHC.MVCWebMis.Controllers
{
    /// <summary>
    /// 数据字典大类的控制器
    /// </summary>
    public class DictTypeController :  BusinessController<DictType, DictTypeInfo>
    {
        /// <summary>
        /// 用作下拉列表的菜单Json数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDictJson()
        {
            List<DictTypeInfo> list = baseBLL.GetAll();
            list = CollectionHelper<DictTypeInfo>.Fill("-1", 0, list, "PID", "ID", "Name");

            List<CListItem> itemList = new List<CListItem>();
            foreach (DictTypeInfo info in list)
            {
                itemList.Add(new CListItem(info.Name, info.ID));
            }
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取树形展示数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeJson()
        {
            string json = GetTreeJson("-1", "", "");
            json = json.Trim(',');
            return Content(string.Format("[{0}]", json));
        }

        /// <summary>
        /// 递归获取树形信息
        /// </summary>
        /// <returns></returns>
        private string GetTreeJson(string PID, string folderIcon, string leafIcon)
        {
            string condition = string.Format("PID='{0}' ", PID);
            List<DictTypeInfo> nodeList = BLLFactory<DictType>.Instance.Find(condition);
            StringBuilder content = new StringBuilder();
            foreach (DictTypeInfo model in nodeList)
            {
                string ParentID = (model.PID == "-1" ? "0" : model.PID);
                //string tempDictType = string.Format("{{ id:{0}, pId:{1}, name:\"{2}\",icon:\"{3}\" }},", model.ID, ParentID, model.Name, imgsrc);
                string subDictType = this.GetTreeJson(model.ID, folderIcon, leafIcon);
                string parentDictType = string.Format("{{ \"id\":\"{0}\", \"pId\":\"{1}\", \"name\":\"{2}\" ", model.ID, ParentID, model.Name);
                if (string.IsNullOrEmpty(subDictType))
                {
                    if (!string.IsNullOrEmpty(leafIcon))
                    {
                        parentDictType += string.Format(",\"icon\":\"{0}\" }},", leafIcon);
                    }
                    else
                    {
                        parentDictType += "},";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(folderIcon))
                    {
                        parentDictType += string.Format(",\"icon\":\"{0}\" }},", folderIcon);
                    }
                    else
                    {
                        parentDictType += "},";
                    }
                }

                content.AppendLine(parentDictType.Trim());
                content.AppendLine(subDictType.Trim());
            }
            return content.ToString().Trim();
        }

    }
}
