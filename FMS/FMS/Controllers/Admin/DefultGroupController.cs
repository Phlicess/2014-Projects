using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Webdiyer.WebControls.Mvc;

namespace FMS.Controllers.Admin
{
    public class DefultGroupController : Controller
    {
        //
        // GET: /DefultGroup/

        [CheckinLogin]
        public bool Insert(QunZu qunzuInfo)
        {
            if (qunzuInfo != null)
            {
                WJEntities wjen = new WJEntities();
                wjen.QunZus.Add(qunzuInfo);
                wjen.SaveChanges();
                return true;
            }
            return false;
        }

        [CheckinLogin]
        public int Insert_getId(QunZu qunzuInfo)
        {            
                WJEntities wjen = new WJEntities();
                var qunzu = wjen.QunZus.Add(qunzuInfo);
                wjen.SaveChanges();
                return qunzu.ID;  
        }

        [CheckinLogin]
        public string GetAllReceiver()
        {
            WJEntities wjen = new WJEntities();
            wjen.Configuration.LazyLoadingEnabled = false;
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            List<YongHu> list = wjen.YongHus.Where(YongHu => YongHu.State == false).ToList();
            string Info = "[";

            foreach (var item in list)
            {
                Info += serializer.Serialize(item) + ",";
            }

            Info += "]";
            return Info;
        }


        //获得剩余接受者（不是群组内的）
        [CheckinLogin]
        public string LeftReciever(string qunzuId)
        {
            int id = Convert.ToInt32(qunzuId);
            

            WJEntities wjen = new WJEntities();
            WJEntities confi_false_wjen = new WJEntities();
            List<int> intList = new List<int>();

            confi_false_wjen.Configuration.LazyLoadingEnabled = false;
            List<YongHu> beforeOrder_list = wjen.YongHus.Where(YongHu => YongHu.State == false).ToList();
            List<YongHu> list = beforeOrder_list.OrderBy(YongHu => YongHu.Name).ToList();

            string Info = "[";
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            foreach (var yonghu in list)
            {
                int num = 0;
                //qunZuInfo += EntityToJson(qunZu);
                var qunzuList = yonghu.QunZus1;
                foreach (var qunZu in qunzuList)
                {
                    if (qunZu.ID == id)
                    {
                        break;
                    }
                    num++;                   
                }

                if (num >= qunzuList.Count)
                {
                    intList.Add(yonghu.ID);
                }

            }

            foreach (var item in intList)
            {
                YongHu yonghu = confi_false_wjen.YongHus.Where(YongHu => YongHu.ID == item).FirstOrDefault();
                Info += serializer.Serialize(yonghu) + ",";
            }

            Info += "]";
            return Info;
     }

        [CheckinLogin]
        public string RightReciver(string qunzuId)
        {
            int id = Convert.ToInt32(qunzuId);


            WJEntities wjen = new WJEntities();
            WJEntities confi_false_wjen = new WJEntities();
            List<int> intList = new List<int>();

            confi_false_wjen.Configuration.LazyLoadingEnabled = false;
            List<YongHu> beforeOrder_list = wjen.YongHus.Where(YongHu => YongHu.State == false).ToList();
            List<YongHu> list = beforeOrder_list.OrderBy(YongHu => YongHu.Name).ToList();

            string Info = "[";
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            foreach (var yonghu in list)
            {
                int num = 0;
                //qunZuInfo += EntityToJson(qunZu);
                var qunzuList = yonghu.QunZus1;
                foreach (var qunZu in qunzuList)
                {
                    if (qunZu.ID == id)
                    {
                        break;
                    }
                    num++;
                }

                if (num < qunzuList.Count)
                {
                    intList.Add(yonghu.ID);
                }

            }

            foreach (var item in intList)
            {
                YongHu yonghu = confi_false_wjen.YongHus.Where(YongHu => YongHu.ID == item).FirstOrDefault();
                Info += serializer.Serialize(yonghu) + ",";
            }

            Info += "]";
            return Info;
        }

        [CheckinLogin]
        public ActionResult Index(int? id)
        {

            WJEntities WJEn = new WJEntities();

            //PagedList<RenWu> plUsers = WJEn.RenWus.OrderBy(model => model.ID).ToPagedList(1, 15);

            PagedList<QunZu> qunZulist = WJEn.QunZus.Where(QunZu => QunZu.PublicGroup == true).OrderBy(model => model.ID).ToPagedList(id ?? 1, 9);


            return View(qunZulist);
        }
        //未完成。。。 群组表有问题


        [CheckinLogin]
        public string ReviseGroup(QunZu info)
        {

            WJEntities wjen = new WJEntities();
            wjen.Configuration.LazyLoadingEnabled = false;
            QunZu qunzu = wjen.QunZus.Where(QunZu => QunZu.ID == info.ID).FirstOrDefault();
            qunzu.GroupName = info.GroupName;
            qunzu.GroupExplain = info.GroupExplain;
            wjen.SaveChanges();
            return "true";
        }


        [CheckinLogin]
        public bool ReviseGroupMember(List<int> array)
        {
           
            WJEntities wjen = new WJEntities();
            var groupId = array[0];
            QunZu qunzu = wjen.QunZus.Where(QunZu => QunZu.ID == groupId).FirstOrDefault();

            //清除所有
            //List<YongHu> yonghu = wjen.YongHus.Where(YongHu =>YongHu.QunZus1.ElementAt() ==  qunzu );         
            qunzu.YongHus.Clear();           
           
            for (int i = 1;i < array.Count; i++)
            {
                int id = array[i];
                YongHu user = wjen.YongHus.Where(YongHu => YongHu.ID == id).FirstOrDefault();
                qunzu.YongHus.Add(user);
            }            
            wjen.SaveChanges();
            return true;
        }


        [CheckinLogin]
        public bool Delete(List<int> list )
        {
            if (list != null)
            {
                WJEntities wjen = new WJEntities();
                foreach (var item in list)
                {
                    QunZu qunzu = wjen.QunZus.Where(QunZu => QunZu.ID == item).FirstOrDefault();
                    qunzu.YongHus.Clear();
                    wjen.QunZus.Remove(qunzu);
                }
                wjen.SaveChanges();
                return true;
            }       
            return false;
        }
    }
}
