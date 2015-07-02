using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMS
{
    public class CheckinLoginAttribute:ActionFilterAttribute  
        {  
            public override void OnActionExecuting(ActionExecutingContext filterContext)  
            {
                if (filterContext.HttpContext.Session["NickName"] == null)  
                {  
                    filterContext.HttpContext.Response.Redirect("~/Home/Index");  
                }  
            }  
        }  
}