using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace WHC.MVCWebMis.Commons
{
    public class PageElementofRole
    {
        /// <summary>
        /// 根据用户ID，页面名字和元素名字判断当前用户是否
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageName"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static bool HasPageElement(int userId, string pageName, string elementName)
        {
            string sql = string.Format(@"SELECT
                COUNT(*)
                FROM
                dbo.ZhiDaoJiaoShi
                INNER JOIN dbo.T_ACL_User_Role ON dbo.T_ACL_User_Role.User_ID = dbo.T_ACL_User.ID
                INNER JOIN dbo.T_ACL_Role ON dbo.T_ACL_User_Role.Role_ID = dbo.T_ACL_Role.ID
                INNER JOIN dbo.T_ACL_Role_PageElement ON dbo.T_ACL_Role_PageElement.[Role_ID] = dbo.T_ACL_Role.ID
                INNER JOIN dbo.T_ACL_PageElement ON dbo.T_ACL_Role_PageElement.PageElement_ID = dbo.T_ACL_PageElement.ID
                WHERE
                dbo.XueYuanJiaoShi.ID = {0} AND
                dbo.XueYuanJiaoShi.Page = '{1}' AND
                dbo.XueYuanJiaoShi.Element = '{2}'", userId, pageName, elementName);
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            return Convert.ToInt32(db.ExecuteScalar(command)) > 0;
        }
        
    }
}
