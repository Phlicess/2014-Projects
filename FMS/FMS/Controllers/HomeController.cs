using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using Microsoft.Ajax.Utilities;

namespace FMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Session.Clear();
            return View();
        }

        //验证用户登录信息
        public ActionResult Login(UserInfo UserInfo)
        {
            if (UserInfo.NickName != null)
            {
                var WJEn = new WJEntities();
                //查找管理员表
                GuanLiYuan Admin = WJEn.GuanLiYuans.FirstOrDefault(GuanLiYuan => GuanLiYuan.Name == UserInfo.NickName);
                //如果不是管理员
                if (Admin == null)
                {
                    //查找用户表
                    YongHu User = WJEn.YongHus.FirstOrDefault(YongHu => YongHu.NickName == UserInfo.NickName);
                    {
                        if (User != null){
                            //判断用户类型 发布者or接收者
                            if (User.State == true)
                            {
                                
                                if (UserInfo.Password == User.Password)
                                {
                                    Session["User"] = "Publisher";
                                    Session["ID"] = User.ID;
                                    //Session["Name"] = User.Name;
                                    Session["NickName"] = User.NickName;
                                    Session["Name"] = User.Name;
                                    return Content("Publisher");
                                }
                                else
                                {
                                    return Content("PasswordError");
                                }
                            }
                            else if (User.State == false)
                            {
                                if (UserInfo.Password == User.Password)
                                {
                                    Session["User"] = "Receiver";
                                    Session["ID"] = User.ID;
                                    //Session["Name"] = User.Name;
                                    Session["NickName"] = User.NickName;
                                    Session["Name"] = User.Name;
                                    return Content("Receiver");
                                }
                                else
                                {
                                    return Content("PasswordError");
                                }
                            }
                        }
                        else
                        {
                            return Content("false");
                        }
                    }
                }
                else
                {
                    if (UserInfo.Password == Admin.Password||""==Admin.Password)
                    {
                        Session["User"] = "Admin";
                        Session["ID"] = Admin.ID;
                        Session["NickName"] = Admin.Name;
                        Session["Name"] = Admin.Name;
                        return Content("Admin");
                    }
                    else
                    {
                        return Content("PasswordError");
                    } 
                }
            }          
            return Content("false");
        }



        /// <summary>
        /// 获取MD5值
        /// </summary>
        /// <param name="str">加密的字符串</param>
        /// <returns>返回MD5值</returns>
        public static string GetMD5(string str)
        {
            if (str == null)
            {
                return null;
            }
            string password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            return password;
        }

    }

}
