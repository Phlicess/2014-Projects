using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FMS.Common;
using Webdiyer.WebControls.Mvc;
using WebGrease.Css.Ast.Selectors;

namespace FMS.Controllers.Admin
{

    [CheckinLogin]
    public class ManagerReceiverController : Controller
    {
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
        [CheckinLogin]
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
        /// <summary>
        // 转换成json格式
        /// </summary>
        /// <param name="obj">要转换的实体对象</param>
        /// <returns></returns>
           [CheckinLogin]
        public string EntityToJson(object obj)
        {
            StringBuilder jsonStr = new StringBuilder();
            PropertyInfo[] pInfos = obj.GetType().GetProperties();
            string pValue = string.Empty;
            jsonStr.Append("{");
            foreach (PropertyInfo p in pInfos)
            {
                if (!(p.GetValue(obj, null) == null))
                {
                    //转义掉Json格式特殊字符 ‘\’,‘"’
                    pValue = p.GetValue(obj, null).ToString().Replace("\\", "\\\\").Replace("\"", "\\\"");
                }
                else
                {
                    pValue = string.Empty;
                }
                jsonStr.Append(string.Format("\"{0}\":\"{1}\",", p.Name, pValue));

            }
            jsonStr.Remove(jsonStr.Length - 1, 1);
            jsonStr.Append("}");
            return jsonStr.ToString();
        }

 
        //insert 发布者
        [CheckinLogin]
        public bool insert(YongHu yonghu)
        {
            WJEntities wjen = new WJEntities();
            wjen.YongHus.Add(yonghu);
           
            return true;
        }
        //获取默认的群组
        [CheckinLogin]
        public string FindGroup()
        {

            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            List<QunZu> qunZuList = WJEn.QunZus.Where(QunZu => QunZu.PublicGroup == true).ToList();
            string qunZuInfo = "[";
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            foreach (var qunZu in qunZuList)
            {
                //qunZuInfo += EntityToJson(qunZu);
                qunZuInfo += serializer.Serialize(qunZu) + ",";
            }
            qunZuInfo += "]";
            return qunZuInfo;
        }
        
        //改变用户信息
        [CheckinLogin]
        public bool RevisePubInfo(YongHu info)
        {
          
            var wjen = new WJEntities();
            YongHu user = wjen.YongHus.Find(info.ID);
                    
           // user.QunZu_ID = info.QunZu_ID;
            user.NickName = info.NickName;
            user.Name = info.Name;
            user.Password = info.Password;
            user.Phone = info.Phone;
            user.Position = info.Position;
            user.Email = info.Email;
            wjen.SaveChanges();
            return true;           
        }

        //delete 接受者
        [CheckinLogin]
        public string Delete(List<int> array)
        {
            if (array.Count != 0)
            {
                var wjen = new WJEntities();
                
                foreach (var item in array)
                {
                    YongHu yongHu =wjen.YongHus.Where(YongHu => YongHu.ID == item).FirstOrDefault();
                    yongHu.QunZus1.Clear();
                    yongHu.RenWus.Clear();
                    wjen.YongHus.Remove(yongHu);
                }
                wjen.SaveChanges();
                return "true";
            }
            return "false";
        }

        [CheckinLogin]
        public ActionResult Index(int? id)
        {

            WJEntities WJEn = new WJEntities();

            //PagedList<RenWu> plUsers = WJEn.RenWus.OrderBy(model => model.ID).ToPagedList(1, 15);

            PagedList<YongHu> YongHuList = WJEn.YongHus.Where(YongHu => YongHu.State == false).OrderBy(model => model.ID).ToPagedList(id ?? 1, 5);


            return View(YongHuList);
        }
    }
}
