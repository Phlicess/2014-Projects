using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHC.MVCWebMis.Common;

using WHC.MVCWebMis.Entity;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.BLL;

namespace WHC.MVCWebMis.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 第一种登陆界面
        /// </summary>
        public ActionResult Index()
        {
            Session.Clear();

            Office2Pdf a = new Office2Pdf();

            //a.DOCConvertToPDF("/UploadFiles/a.doc", "/UploadFiles/a.swf");

            return View();
        }

        /// <summary>
        /// 第二种登陆界面
        /// </summary>
        public ActionResult SecondIndex()
        {
            Session.Clear();

            return View();
        }

        public ActionResult CheckUser(string username, string password, string code)
        {
            string result = "";

            bool codeValidated = true;
            if (this.TempData["ValidateCode"] != null)
            {
                codeValidated = (this.TempData["ValidateCode"].ToString() == code);
            }

            if (string.IsNullOrEmpty(username))
            {
                result = "用户名不能为空";
            }
            else if (!codeValidated)
            {
                result = "验证码输入有误";
            }

            string ip = GetClientIp();
            string macAddr = "";
            //string ip = NetworkUtil.GetLocalIP();
            //string macAddr = HardwareInfoHelper.GetMacAddress();
            string identity = BLLFactory<WHC.MVCWebMis.BLL.User>.Instance.VerifyUser(username, password, MyConstants.SystemType, ip, macAddr);
            if (!string.IsNullOrEmpty(identity))
            {
                UserInfo info = BLLFactory<WHC.MVCWebMis.BLL.User>.Instance.GetUserByName(username);
                if (info != null)
                {
                    result = "OK";
                    Session["UserInfo"] = info;
                    Session["Identity"] = info.Name.Trim();

                    #region 取得用户的授权信息，并存储在Session中

                    List<FunctionInfo> functionList = BLLFactory<Function>.Instance.GetFunctionsByUser(info.ID, MyConstants.SystemType);
                    Dictionary<string, string> functionDict = new Dictionary<string, string>();
                    foreach (FunctionInfo functionInfo in functionList)
                    {
                        if (!string.IsNullOrEmpty(functionInfo.ControlID) && 
                            !functionDict.ContainsKey(functionInfo.ControlID))
                        {
                            functionDict.Add(functionInfo.ControlID, functionInfo.ControlID);
                        }
                    }
                    Session["Functions"] = functionDict; 

                    #endregion
                }
            }
            else
            {
                result = "用户名输入错误或者您已经被禁用";
            }

            return Content(result);
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        private string GetClientIp()
        {
            //可以透过代理服务器
            string userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(userIP))
            {
                //没有代理服务器,如果有代理服务器获取的是代理服务器的IP
                userIP = Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(userIP))
            {
                userIP = Request.UserHostAddress;
            }
            return userIP;
        }


        /// <summary>
        /// 验证码的实现
        /// </summary>
        /// <returns>返回验证码</returns>
        public ActionResult CheckCode()
        {
            //首先实例化验证码的类
            MyValidateCode validateCode = new MyValidateCode();
            //生成验证码指定的长度
            //string code = validateCode.CreateValidateCode(5);

            string code = WHC.Framework.Commons.RandomChinese.GetRandomNumber(4, true);
            //将验证码赋值给Session变量
            //Session["ValidateCode"] = code;
            this.TempData["ValidateCode"] = code;
            //创建验证码的图片
            byte[] bytes = validateCode.CreateValidateGraphic(code);
            //最后将验证码返回
            return File(bytes, @"image/jpeg");
        }
    }
}
