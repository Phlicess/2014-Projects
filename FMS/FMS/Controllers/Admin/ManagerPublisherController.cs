using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using Microsoft.Ajax.Utilities;
using Webdiyer.WebControls.Mvc;
using WebGrease.Css.Extensions;

namespace FMS.Controllers.Admin
{

    [CheckinLogin]
    public class ManagerPublisherController : Controller
    {
        //

        // insert 验证用户名是否已经存在
        public string FindbyNickName(string nickName)
        {
            WJEntities wjen = new WJEntities();
            YongHu user = wjen.YongHus.Where(YongHu => YongHu.NickName == nickName).FirstOrDefault();
            if (user == null)
            {
                return "False";
            }
            return "True";
        }

        // revise 验证用户名是否已经存在
        public string Revise_FindbyNickName(YongHu yonghu)
        {
           WJEntities ejen = new WJEntities();
            YongHu user = ejen.YongHus.Where(YongHu => YongHu.ID == yonghu.ID).FirstOrDefault();          
            if (user.NickName == yonghu.NickName)
            {
                return "False";
            }
            YongHu user_else = ejen.YongHus.Where(YongHu => YongHu.NickName == yonghu.NickName).FirstOrDefault();
            if (user_else == null)
            {
                return "False";
            }
            return "True";
        }
        public string secrity(string str)
        {
            return  System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }


        [CheckinLogin]
        //insert /ManagerPulisher/
        public bool Insert(YongHu userInfo)
        {
            if (userInfo != null)
            {
                WJEntities wJEn = new WJEntities();
                wJEn.YongHus.Add(userInfo);
                //if (value != null) {
                //    return true;
                //   }
                //}
                wJEn.SaveChanges();
                return true;

            }
            return false;
        }


        // 更新数据

        [CheckinLogin]
        public bool RevisePubInfo(YongHu info)
        {         
             var wjen = new WJEntities();
             YongHu user = wjen.YongHus.Find(info.ID);
            user.NickName = info.NickName;
             user.Name = info.Name;
             user.Password = info.Password;
             user.Phone = info.Phone;
             user.Position = info.Position;
             user.Email = info.Email;
             wjen.SaveChanges();
             return true;
     
        }

        [CheckinLogin]
        public string Delete(List<int> array)
        {
            WJEntities wjen = new WJEntities();
            foreach (int item in array)
            {       
                //删除相关群组  
                List<QunZu> qunzuList = wjen.QunZus.Where(QunZu => QunZu.YongHu_ID == item).ToList();
                foreach (QunZu qunZu in qunzuList)
                {
                    wjen.QunZus.Remove(qunZu);
                }

                //删除相关任务
                List<RenWu> renList = wjen.RenWus.Where(RenWu => RenWu.ID == item).ToList();
                foreach (RenWu renWu in renList)
                {
                    wjen.RenWus.Remove(renWu);
                }

                //删除发布者
                YongHu publisher = wjen.YongHus.Find(item);
                wjen.YongHus.Remove(publisher);
            }
            wjen.SaveChanges();
            return "true";
        }
        //给ManagerPublisher的section部分 传递需要显示的数据 

        [CheckinLogin]
        public ActionResult Index(int? id)
        {

            WJEntities WJEn = new WJEntities();

            //PagedList<RenWu> plUsers = WJEn.RenWus.OrderBy(model => model.ID).ToPagedList(1, 15);

            PagedList<YongHu> YongHuList = WJEn.YongHus.Where(YongHu => YongHu.State == true).OrderBy(model => model.ID).ToPagedList(id ?? 1, 5);


            return View(YongHuList);
        }
    }
}
