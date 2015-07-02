using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FMS.Controllers.Admin
{
    public class AdminInformationController : Controller
    {
        //
        // GET: /AdminInformation/
        [CheckinLogin]
        public ActionResult Index()
        {
            return View();
        }

        [CheckinLogin]
        public string Find()
        {
            WJEntities WJEn = new WJEntities();
            string Name = Session["Name"].ToString();
            GuanLiYuan admin = WJEn.GuanLiYuans.FirstOrDefault(GuanLiYuan => GuanLiYuan.Name == Name);

            string Info = EntityToJson(admin);
            return Info;
        }


        /// <summary>
        /// 实体类型转换成json格式
        /// </summary>
        /// <param name="obj">要转换的实体对象</param>
        /// <returns>string</returns>
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

        //用户修改个人信息方法
        [CheckinLogin]
        public ActionResult EditInformation(GuanLiYuan Info)
        {
            if (Info == null)
            {
                return Content("false");
            }
            WJEntities WJEn = new WJEntities();
            string OldName = Session["Name"].ToString();
            GuanLiYuan admin = WJEn.GuanLiYuans.FirstOrDefault(GuanLiYuan => GuanLiYuan.Name == OldName);

            admin.Name = Info.Name;
            admin.Phone = Info.Phone;
            admin.Department = Info.Department;
            admin.Email = Info.Email;
            WJEn.SaveChanges();
            Session["Name"] = Info.Name;
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
            string Name = Session["Name"].ToString();
            GuanLiYuan admin = WJEn.GuanLiYuans.FirstOrDefault(GuanLiYuan => GuanLiYuan.Name == Name);

            if (admin.Password != null && admin.Password == OldPassword)
            {
                admin.Password = NewPassword;
                WJEn.SaveChanges();
                return Content("已经更新为你的新密码了!");
            }
            else if (admin.Password == null && OldPassword == "")
            {
                admin.Password = NewPassword;
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
