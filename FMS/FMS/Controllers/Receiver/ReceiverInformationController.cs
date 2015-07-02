using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace FMS.Controllers.Receiver
{
    public class ReceiverInformationController : Controller
    {
        //
        // GET: /Personal/
        [CheckinLogin]
        public ActionResult Index()
        {
            return View();
        }
        public string Find()
        {
            WJEntities WJEn = new WJEntities();
            WJEn.Configuration.LazyLoadingEnabled = false;
            string nickName = Session["NickName"].ToString();
            YongHu YongHuInfo = WJEn.YongHus.FirstOrDefault(YongHu => YongHu.NickName == nickName);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string Info = "";
            Info += serializer.Serialize(YongHuInfo);
            
            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递               
            //string dts = JsonConvert.SerializeObject(YongHuInfo);
            
            //JArray array = JsonConvert.DeserializeObject<JArray>(dts);
            //var result = new { total = 1, rows = array };
            return Info;
        }


        /// <summary>
        /// 实体类型转换成json格式
        /// </summary>
        /// <param name="obj">要转换的实体对象</param>
        /// <returns>string</returns>
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

        //用户修改个人信息方法
        public ActionResult EditInformation(YongHu Info)
        {
            if (Info == null)
            {
                return Content("false");
            }
            WJEntities WJEn = new WJEntities();
            string OldNickName = Session["NickName"].ToString();
            YongHu yongHu = WJEn.YongHus.FirstOrDefault(YongHu => YongHu.NickName == OldNickName);

            yongHu.Name = Info.Name;
            yongHu.NickName = Info.NickName;
            yongHu.Phone = Info.Phone;
            yongHu.Position = Info.Position;
            yongHu.Email = Info.Email;
            WJEn.SaveChanges();
            Session["NickName"] = Info.NickName;
            return Content("true");
        }


        //修改密码的方法
        //参数 1.（原密码） 2.（新密码）
        public ActionResult EidtPassword(string OldPassword, string NewPassword)
        {
            //if (OldPassword == null)
            //{
            //    return Content("原密码不能为空!");
            //}
            
            WJEntities WJEn = new WJEntities();
            string nickName = Session["NickName"].ToString();
            YongHu yongHuInfo = WJEn.YongHus.FirstOrDefault(YongHu => YongHu.NickName == nickName);

            if (yongHuInfo.Password != null && yongHuInfo.Password == OldPassword)
            {
                yongHuInfo.Password = NewPassword;
                WJEn.SaveChanges();
                return Content("已经更新为你的新密码了!");
            }
            else if (yongHuInfo.Password == null && OldPassword == "")
            {
                yongHuInfo.Password = NewPassword;
                WJEn.SaveChanges();
                return Content("已经更新为你的新密码了!");
            }
            else
            {
                return Content("你输入的原密码不正确!");
            }   
        }
    }
}
