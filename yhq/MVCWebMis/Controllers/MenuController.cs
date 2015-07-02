using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.Pager.Entity;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.Common;

namespace WHC.MVCWebMis.Controllers
{
    public class MenuController : BusinessController<Menu, MenuInfo>
    {
        //public override ActionResult FindWithPager()
        //{
        //    string where = GetPagerCondition();
        //    PagerInfo pagerInfo = GetPagerInfo();
        //    List<MenuInfo> list = baseBLL.FindWithPager(where, pagerInfo);
        //    list = CollectionHelper<MenuInfo>.Fill("-1", 0, list, "PID", "ID", "Name");

        //    //Json格式的要求{total:22,rows:{}}
        //    //构造成Json的格式传递
        //    var result = new { total = pagerInfo.RecordCount, rows = list };
        //    return JsonDate(result);
        //}

        /// <summary>
        /// 用作下拉列表的菜单Json数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDictJson()
        {
            List<MenuInfo> list = baseBLL.GetAll();
            list = CollectionHelper<MenuInfo>.Fill("-1", 0, list, "PID", "ID", "Name");

            List<CListItem> itemList = new List<CListItem>();
            foreach (MenuInfo info in list)
            {
                itemList.Add(new CListItem(info.Name, info.ID));
            }
            itemList.Insert(0, new CListItem("无", "-1"));
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取菜单的树形展示数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeJson()
        {
            #region 返回菜单Json格式
            //        {
            //            "default": [
            //                { "menuid": "1", "icon": "icon-computer", "menuname": "权限管理",
            //				  "menus": [
            //						    { "menuid": "13", "menuname": "用户管理", "icon": "icon-user", "url": "ListUser.aspx" },
            //						    { "menuid": "14", "menuname": "组织机构管理", "icon": "icon-organ", "url": "ListOU.aspx" },
            //						    { "menuid": "15", "menuname": "角色管理", "icon": "icon-group-key", "url": "ListRole.aspx" },
            //						    { "menuid": "16", "menuname": "功能管理", "icon": "icon-key", "url": "ListFunction.aspx" }
            //			   ]},
            //               { "menuid": "2", "icon": "icon-user", "menuname": "其他管理",
            //				 "menus": [{ "menuid": "21", "menuname": "修改密码", "icon": "icon-lock", "url": "ModifyPassword.aspx" }
            //			  ]}
            //            ],
            //            "point": [
            //                { "menuid": "3", "icon": "icon-computer", "menuname": "事务中心",
            //				  "menus": [
            //							{ "menuid": "33", "menuname": "测试菜单1", "icon": "icon-user", "url": "../Commonpage/building.htm" },
            //							{ "menuid": "34", "menuname": "测试菜单2", "icon": "icon-organ", "url": "../Commonpage/building.htm" },
            //							{ "menuid": "35", "menuname": "测试菜单3", "icon": "icon-group-key", "url": "../Commonpage/building.htm" },
            //							{ "menuid": "36", "menuname": "测试菜单4", "icon": "icon-key", "url": "../Commonpage/building.htm" }
            //				]},
            //                { "menuid": "4", "icon": "icon-user", "menuname": "其他菜单",
            //                  "menus": [{ "menuid": "41", "menuname": "测试菜单5", "icon": "icon-lock", "url": "../Commonpage/building.htm"}]
            //				}
            //            ],
            //              "1": [{ "menuid": "5", "icon": "icon-computer", "menuname": "行业动态", "menus": [{ "menuid": "1331", "menuname": "政策法规", "icon": "icon-user", "url": "../Expert/ListPolicyLaw.aspx" }, { "menuid": "1333", "menuname": "通知公告", "icon": "icon-user", "url": "../Expert/ListInformation.aspx" }, { "menuid": "1334", "menuname": "动态信息", "icon": "icon-user", "url": "../Expert/ListIndustryNews.aspx"}]}], "1000": [{ "menuid": "1641", "icon": "icon-computer", "menuname": "基础信息", "menus": [{ "menuid": "1504", "menuname": "道路信息", "icon": "icon-user", "url": "../Road/IndexRoad.aspx" }, { "menuid": "1505", "menuname": "桥梁信息", "icon": "icon-user", "url": "../Bridge/IndexBridge.aspx" }, { "menuid": "1506", "menuname": "隧道信息", "icon": "icon-user", "url": "../Tunnel/IndexTunnel.aspx"}] }, { "menuid": "1622", "icon": "icon-computer", "menuname": "路政巡查管理", "menus": [{ "menuid": "1601", "menuname": "排班计划", "icon": "icon-user", "url": "../Schedule/FormList.aspx" }, { "menuid": "1621", "menuname": "PDA终端设备信息", "icon": "icon-user", "url": "../Check/IndexTerminal.aspx" }, { "menuid": "1644", "menuname": "挖掘占道审批信息", "icon": "icon-user", "url": "../Road/ListConstruction.aspx" }, { "menuid": "1645", "menuname": "考勤信息", "icon": "icon-user", "url": "../Check/TotalOnDuty.aspx" }, { "menuid": "1662", "menuname": "责任单位信息", "icon": "icon-user", "url": "../Road/ListAddressbook.aspx" }, { "menuid": "1721", "menuname": "责任单位通讯录", "icon": "icon-user", "url": "../Road/ListAddresslist.aspx" }, { "menuid": "1741", "menuname": "投诉处理", "icon": "icon-user", "url": "../Check/ListJobComplaint.aspx"}] }, { "menuid": "1663", "icon": "icon-computer", "menuname": "巡查监督管理", "menus": [{ "menuid": "1624", "menuname": "巡查问题", "icon": "icon-user", "url": "../Check/indexProblem.aspx" }, { "menuid": "1626", "menuname": "巡查人员监控", "icon": "icon-user", "url": "../GIS/ShowGis.aspx?gettype=1" }, { "menuid": "1643", "menuname": "报警信息", "icon": "icon-user", "url": "../TaskWarning/ListTaskWarning.aspx" }, { "menuid": "1625", "menuname": "任务小结", "icon": "icon-user", "url": "../Check/ListJobSummary.aspx" }, { "menuid": "1642", "menuname": "短信通知", "icon": "icon-user", "url": "../Commonpage/ListSMS.aspx" }, { "menuid": "1761", "menuname": "短信模板", "icon": "icon-user", "url": "../Commonpage/ListSMSTemplate.aspx" }, { "menuid": "1646", "menuname": "整改通知书", "icon": "icon-user", "url": "../Check/ListRectify.aspx"}] }, { "menuid": "1664", "icon": "icon-computer", "menuname": "巡查统计分析", "menus": [{ "menuid": "1661", "menuname": "巡查问题统计", "icon": "icon-user", "url": "../Check/TotalJobProblemN.aspx" }, { "menuid": "1648", "menuname": "任务完成信息", "icon": "icon-user", "url": "../Check/ListTask.aspx" }, { "menuid": "1665", "menuname": "巡查任务统计", "icon": "icon-user", "url": "../Check/TotalTask.aspx"}]}], "3": [{ "menuid": "31", "icon": "icon-computer", "menuname": "用户管理", "menus": [{ "menuid": "32", "menuname": "用户管理", "icon": "icon-user", "url": "../Security/UserFrame.aspx" }, { "menuid": "33", "menuname": "部门管理", "icon": "icon-user", "url": "../Security/GroupFrame.aspx" }, { "menuid": "34", "menuname": "角色管理", "icon": "icon-user", "url": "../Security/ListRoles.aspx" }, { "menuid": "73", "menuname": "功能管理", "icon": "icon-user", "url": "../Security/FunctionFrame.aspx"}] }, { "menuid": "35", "icon": "icon-computer", "menuname": "系统维护", "menus": [{ "menuid": "36", "menuname": "流程设置", "icon": "icon-user", "url": "../App/ListAppForm.aspx" }, { "menuid": "37", "menuname": "申请单管理", "icon": "icon-user", "url": "../App/ListAppApply.aspx" }, { "menuid": "74", "menuname": "流程环节管理", "icon": "icon-user", "url": "../App/ListAppProc.aspx" }, { "menuid": "1285", "menuname": "流程环节用户设置", "icon": "icon-user", "url": "../App/FlowUserFrame.aspx" }, { "menuid": "76", "menuname": "菜单管理", "icon": "icon-user", "url": "../Security/MenuFrame.aspx" }, { "menuid": "75", "menuname": "系统日志", "icon": "icon-user", "url": "../Commonpage/ListSystemLog.aspx" }, { "menuid": "176", "menuname": "数据字典管理", "icon": "icon-user", "url": "../Commonpage/ListMenu.aspx" }, { "menuid": "1667", "menuname": "短信经办人", "icon": "icon-user", "url": "../Commonpage/ListSmsUser.aspx" }, { "menuid": "1422", "menuname": "临时通行口令", "icon": "icon-user", "url": "../Commonpage/SpecialPermit.aspx" }, { "menuid": "1701", "menuname": "整改通知书编码管理", "icon": "icon-user", "url": "../Security/ModifyRectifySerial.aspx"}]}]
            //        }
            #endregion

            StringBuilder sb = new StringBuilder();
            List<MenuInfo> list = BLLFactory<Menu>.Instance.GetTopMenu(MyConstants.SystemType);
            int i = 0;
            foreach (MenuInfo info in list)
            {
                if (!HasFunction(info.FunctionId))
                {
                    continue;
                }

                if (i++ == 0)
                {
                    sb.AppendFormat("\"{0}\": [", "default");
                }
                else
                {
                    sb.AppendFormat("\"{0}\": [", info.ID);
                }

                bool foundNode = false;//是否存在二级菜单
                List<MenuNodeInfo> nodeList = BLLFactory<Menu>.Instance.GetTreeByID(info.ID);
                foreach (MenuNodeInfo nodeInfo in nodeList)
                {
                    if (!HasFunction(nodeInfo.FunctionId))
                    {
                        continue;
                    }

                    foundNode = true;

                    sb.AppendFormat("{{ \"menuid\": \"{0}\", \"icon\": \"{2}\", \"menuname\": \"{1}\",",
                        nodeInfo.ID, nodeInfo.Name, string.IsNullOrEmpty(nodeInfo.WebIcon) ? "icon-computer" : nodeInfo.WebIcon);

                    string tempString = "\"menus\": [{0}] }},";
                    StringBuilder subSb = new StringBuilder();
                    foreach (MenuNodeInfo subNodeInfo in nodeInfo.Children)
                    {
                        if (!HasFunction(subNodeInfo.FunctionId))
                        {
                            continue;
                        }
                        subSb.AppendFormat("{{ \"menuid\": \"{0}\", \"menuname\": \"{1}\", \"icon\": \"{3}\", \"url\": \"{2}\" }},",
                            subNodeInfo.ID, subNodeInfo.Name, subNodeInfo.Url, string.IsNullOrEmpty(subNodeInfo.WebIcon) ? "icon-organ" : subNodeInfo.WebIcon);
                    }
                    sb.AppendFormat(tempString, subSb.ToString().Trim(','));
                }

                if (foundNode)
                {
                    sb.Remove(sb.Length - 1, 1);
                }

                sb.Append("],");
            }
            string result = string.Format("{{ {0} }}", sb.ToString().Trim(','));
            return Content(result);
        }

        /// <summary>
        /// 获取树形展示数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllTreeJson()
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
            List<MenuInfo> nodeList = BLLFactory<Menu>.Instance.Find(condition);
            StringBuilder content = new StringBuilder();
            foreach (MenuInfo model in nodeList)
            {
                string ParentID = (model.PID == "-1" ? "0" : model.PID);
                //string tempMenu = string.Format("{{ id:{0}, pId:{1}, name:\"{2}\",icon:\"{3}\" }},", model.ID, ParentID, model.Name, imgsrc);
                string subMenu = this.GetTreeJson(model.ID, folderIcon, leafIcon);
                string parentMenu = string.Format("{{ \"id\":\"{0}\", \"pId\":\"{1}\", \"name\":\"{2}\" ", model.ID, ParentID, model.Name);
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
