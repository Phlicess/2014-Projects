using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHC.Framework.ControlUtil;
using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;


namespace WHC.MVCWebMis.Controllers
{
    public class LiShiController : BaseController
    {
        //
        // GET: /LiShi/

        public ActionResult Text()
        {
            string sql =
                string.Format(@"SELECT T_ACL_Role.Name FROM T_ACL_Role INNER JOIN T_ACL_User_Role ON T_ACL_Role.ID = T_ACL_User_Role.Role_ID 
                        INNER JOIN T_ACL_User ON T_ACL_User_Role.User_ID = T_ACL_User.ID  where T_ACL_User.ID = {0}", CurrentUser.ID);
            
            var result = BLLFactory<Role>.Instance.SqlValueList(sql);
            var roles = result.Split(',');
            if (roles.Contains("管理员"))
            {
                ViewBag.Role = "Admin";
            }
            else
            {
                ViewBag.Role = "XueSheng";
            }
            
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }


    }
}
