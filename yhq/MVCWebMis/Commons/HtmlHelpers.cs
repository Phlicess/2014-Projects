using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHC.MVCWebMis.Entity;

namespace WHC.MVCWebMis.Common
{
    public static class HtmlHelpers
    {
        public static bool HasFunction(this HtmlHelper helper, string functionId)
        {
            return Permission.HasFunction(functionId);
        }

        public static bool IsAdmin()
        {
            return Permission.IsAdmin();
        }
    }
}