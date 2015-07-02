using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WHC.Pager.Entity;
using WHC.Framework.ControlUtil;
using WHC.Framework.Commons;
using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;
using System.Net;
using System.Net.Mime;

namespace WHC.MVCWebMis.Controllers
{
    /// <summary>
    /// 所有需要进行登录控制的控制器基类
    /// </summary>
    public class BaseController : Controller 
    {
        #region 属性变量

        /// <summary>
        /// 当前登录的用户属性
        /// </summary>
        public UserInfo CurrentUser = new UserInfo();

        /// <summary>
        /// 定义常用功能的控制ID，方便基类控制器对用户权限的控制
        /// </summary>
        protected AuthorizeKey AuthorizeKey = new AuthorizeKey(); 

        #endregion

        #region 权限控制内容

        /// <summary>
        /// 获取用户的能使用的功能集合
        /// </summary>
        protected virtual Dictionary<string, string> Functions
        {
            get
            {
                Dictionary<string, string> functionDict = Session["Functions"] as Dictionary<string, string>;
                if (functionDict == null)
                {
                    functionDict = new Dictionary<string, string>();
                }
                return functionDict;
            }
        }

        /// <summary>
        /// 判断当前用户是否拥有某功能点的权限
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        public virtual bool HasFunction(string functionId)
        {
            return Permission.HasFunction(functionId);
        }

        /// <summary>
        /// 判断是否为系统管理员
        /// </summary>
        /// <returns>true:系统管理员,false:不是系统管理员</returns>
        public virtual bool IsAdmin()
        {
            return Permission.IsAdmin();
        }

        /// <summary>
        /// 用于检查方法执行前的权限，如果未授权，返回MyDenyAccessException异常
        /// </summary>
        /// <param name="functionId"></param>
        protected virtual void CheckAuthorized(string functionId)
        {
            if(!HasFunction(functionId))
            {
                string errorMessage = "您未被授权使用该功能，请重新登录测试或联系管理员进行处理。";
                throw new MyDenyAccessException(errorMessage);
            }
        }

        /// <summary>
        /// 对AuthorizeKey对象里面的操作权限进行赋值，用于页面判断
        /// </summary>
        protected virtual void ConvertAuthorizedInfo()
        {
            //判断用户权限
            AuthorizeKey.CanInsert = HasFunction(AuthorizeKey.InsertKey);
            AuthorizeKey.CanUpdate = HasFunction(AuthorizeKey.UpdateKey);
            AuthorizeKey.CanDelete = HasFunction(AuthorizeKey.DeleteKey);
            AuthorizeKey.CanView = HasFunction(AuthorizeKey.ViewKey);
            AuthorizeKey.CanList = HasFunction(AuthorizeKey.ListKey);
            AuthorizeKey.CanExport = HasFunction(AuthorizeKey.ExportKey);
        }

        #endregion

        #region 异常处理及记录
        /// <summary>
        /// 重新基类在Action执行之前的事情
        /// </summary>
        /// <param name="filterContext">重写方法的参数</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //得到用户登录的信息
            CurrentUser = Session["UserInfo"] as UserInfo;            
            if (CurrentUser == null)
            {
                Response.Redirect("/Login/Index");//如果用户为空跳转到登录界面
            }

            //设置授权属性，然后赋值给ViewBag保存
            ConvertAuthorizedInfo();
            ViewBag.AuthorizeKey = AuthorizeKey;
        }

        /// <summary>
        /// 覆盖基类控制器的异常处理
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is MyDenyAccessException)
            {
                base.OnException(filterContext);

                //自定义非授权的异常处理，可记录用户操作

                // 当自定义显示错误 mode = On，显示友好错误页面
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    filterContext.ExceptionHandled = true;
                    this.View("Error").ExecuteResult(this.ControllerContext);
                    Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
            else
            {
                base.OnException(filterContext);
                WHC.Framework.Commons.LogTextHelper.Error(filterContext.Exception);//错误记录

                // 当自定义显示错误 mode = On，显示友好错误页面
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    filterContext.ExceptionHandled = true;
                    this.View("Error").ExecuteResult(this.ControllerContext);
                    //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
        } 
        #endregion

        #region 辅助函数
        /// <summary>
        /// 返回处理过的时间的Json字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ContentResult JsonDate(object date)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return Content(JsonConvert.SerializeObject(date, Formatting.Indented, timeConverter));
        }

        public ContentResult Content(bool result)
        {
            return Content(result.ToString().ToLower());//小写方便脚本处理
        }

        public ContentResult Content(int result)
        {
            return Content(result.ToString());
        } 
        #endregion
    }
}
