using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Webdiyer.WebControls.Mvc;

namespace FMS.Controllers.Publisher
{
    public class GroupManagerController : Controller
    {
        // GET: /GroupManager/
        [CheckinLogin]
        public ActionResult Index(int? id)
        {
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            int yongHu_ID = int.Parse(Session["ID"].ToString());
            List<QunZu> qunZuList = WJEn.QunZus.Where(d => d.YongHu_ID == yongHu_ID).ToList();
            PagedList<QunZu> qunZuPagedList = new PagedList<QunZu>(qunZuList, id ?? 1, 20);
            return View(qunZuPagedList);
        }

        //为新建群组返回所有的接收者
        public string FindReceivers()
        {
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            List<YongHu> yongHuList = WJEn.YongHus.Where(d => d.State == false).ToList();
            string yongHuStr = "[";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            foreach (YongHu yongHu in yongHuList)
            {
                yongHuStr += serializer.Serialize(yongHu) + ",";
            }
            yongHuStr += "]";
            return yongHuStr;
        }

        //新建群组
        public string AddGroup(string receivers, QunZu qunZu)
        {
            WJEntities WJEn = new WJEntities();
            int yongHu_ID = int.Parse(Session["ID"].ToString());
            qunZu.YongHu_ID = yongHu_ID;
            qunZu.PublicGroup = false;
            WJEn.QunZus.Add(qunZu);
            WJEn.SaveChanges();
            int qunZu_ID = qunZu.ID;
            List<string> receiverList = new List<string>(receivers.Split(','));
            YongHu newYongHu = new YongHu();
            string receiver_NickName;
            for (int i = 0; i < receiverList.Count; i++)
            {
                receiver_NickName = receiverList[i];
                newYongHu = WJEn.YongHus.FirstOrDefault(y => y.NickName == receiver_NickName);
                WJEn.QunZus.FirstOrDefault(q => q.ID == qunZu_ID).YongHus.Add(newYongHu);
                WJEn.SaveChanges();
            }
            return "新建群组成功!";
        }

        //修改群组信息(接收者 修改的群组的ID 新的群组的信息)
        public string EditGroup(string receivers, string Group_ID, QunZu qunZuInfo)
        {
            WJEntities WJEn = new WJEntities();
            int OldGroup_ID;
            int.TryParse(Group_ID, out OldGroup_ID);
            QunZu oldQunZu = WJEn.QunZus.FirstOrDefault(q => q.ID == OldGroup_ID);
            oldQunZu.GroupName = qunZuInfo.GroupName;
            oldQunZu.GroupExplain = qunZuInfo.GroupExplain;
            WJEn.SaveChanges();

            if (receivers == "")
            {
                return "此群组没有接收者!";
            }
            string[] receiverNickName = receivers.Split(',');
            //1. 先删除群组对应所有接收者的外键关系 
            List<YongHu> yongHuList = new List<YongHu>();
            yongHuList.AddRange(WJEn.QunZus.FirstOrDefault(q => q.ID == OldGroup_ID).YongHus);
            for (int t = 0; t < yongHuList.Count; t++)
            {
                string nickName = yongHuList[t].NickName;
                YongHu yongHu = WJEn.YongHus.FirstOrDefault(y => y.NickName == nickName);
                WJEn.QunZus.FirstOrDefault(q => q.ID == OldGroup_ID).YongHus.Remove(yongHu);
                WJEn.SaveChanges();
            }
            //2. 然后为新的接收者建立外键关联
            for (int i = 0; i < receiverNickName.Length; i++)
            {
                string nickName = receiverNickName[i];
                YongHu yongHu = WJEn.YongHus.FirstOrDefault(y => y.NickName == nickName);
                WJEn.QunZus.FirstOrDefault(q => q.ID == OldGroup_ID).YongHus.Add(yongHu);
                WJEn.SaveChanges();
            }
            return "群组修改成功!";
        }

        //根据ID查询Group的所有接收者信息
        public string FindGroups(string group_ID)
        {
            WJEntities WJEn = new WJEntities();  //禁止懒加载 接收懒加载的接收者 变为没有懒加载的
            WJEntities WJEnLa = new WJEntities();  //可以懒加载 实现用群组ID找接收者
            WJEn.Configuration.LazyLoadingEnabled = false;

            int ID = int.Parse(group_ID);
            string receivers = "[";
            List<YongHu> yongHus = WJEnLa.QunZus.FirstOrDefault(q => q.ID == ID).YongHus.ToList();
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            int yongHu_ID;
            for (int i = 0; i < yongHus.Count; i++)
            {
                yongHu_ID = yongHus[i].ID;
                receivers += serializer.Serialize(WJEn.YongHus.FirstOrDefault(y => y.ID == yongHu_ID)) + ",";
            }
            return receivers + "]";
        }

        //删除群组功能（可以删除多个）
        public string DeleteGroup(string GroupIds)
        {
            WJEntities WJEn = new WJEntities();
            string[] group_ID = GroupIds.Split(',');
            for (int i = 0; i < group_ID.Length; i++)
            {
                int intGroup_ID;
                int.TryParse(group_ID[i], out intGroup_ID);
                //List<YongHu> yongHuList = new List<YongHu>();
                //yongHuList.AddRange(WJEn.QunZus.FirstOrDefault(q => q.ID == intGroup_ID).YongHus.ToList());
                //foreach (YongHu yongHu in yongHuList)
                //{
                //    WJEn.QunZus.FirstOrDefault(q => q.ID == intGroup_ID).YongHus.Remove(yongHu);
                //    WJEn.SaveChanges();
                //}
                WJEn.QunZus.FirstOrDefault(q => q.ID == intGroup_ID).YongHus.Clear();
                QunZu qunZu = WJEn.QunZus.FirstOrDefault(q => q.ID == intGroup_ID);
                WJEn.QunZus.Remove(qunZu);
                WJEn.SaveChanges();
            }
            return "成功删除选中的群组!";
        }
    }
}
