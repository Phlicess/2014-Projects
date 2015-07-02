using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;
using WHC.Pager.Entity;


using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using WHC.Framework.ControlUtil;


namespace WHC.MVCWebMis.Controllers
{
    public class UserZhuCeController : BusinessController<XueSheng, XueShengInfo>
    {
        //
        // GET: /UserZhuCe/

        public UserZhuCeController()
            : base()
        {
            string sql =
                string.Format(@"SELECT T_ACL_Role.Name FROM T_ACL_Role INNER JOIN T_ACL_User_Role ON T_ACL_Role.ID = T_ACL_User_Role.Role_ID 
                        INNER JOIN T_ACL_User ON T_ACL_User_Role.User_ID = T_ACL_User.ID  where T_ACL_User.ID = {0}", CurrentUser.ID);

            var result = BLLFactory<Role>.Instance.SqlValueList(sql);
            var roles = result.Split(',');
            //如果当前用户是管理员身份 则返回 ViewBag = 'Admin'
            //如果当前用户是学院审批项目的老师 则返回 ViewBag 值为学院老师的学院ID
            if (roles.Contains("管理员"))
            {
                ViewBag.Role = "Admin";
            }
            else if (roles.Contains("学院老师"))
            {
                ViewBag.Role = CurrentUser.Dept_ID;
            }
            
        }
       

        //用于分页查找 根据查询条件筛选用户
        //条件：XueYuan_Id  XingMing  XueHao
        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();

            List<XueShengInfo> list = baseBLL.FindWithPager(where, pagerInfo);

            string sql = string.Format(@"
                SELECT dbo.XueSheng.*,
                       dbo.XueYuan.XueYuan
                FROM dbo.XueSheng
                INNER JOIN dbo.XueYuan ON dbo.XueSheng.XueYuan_Id = dbo.XueYuan.ID
                WHERE 
                    dbo.XueSheng.User_Id IS NOT NULL
            ");
            DataTable dt = baseBLL.SqlTableWithPager(sql, "*", condition: where, sortField: "XueHao", isDescending: false, info: pagerInfo);

            string dts = JsonConvert.SerializeObject(dt, new DataTableConverter());
            JArray array = JsonConvert.DeserializeObject<JArray>(dts);
            var result = new { total = pagerInfo.RecordCount, rows = array };
            return JsonDate(result);
        }


        //重写Insert()方法 实现插入新注册用户数据到 T_ACL_User 表 并且插入到 T_ACL_Role 表和 XueSheng 表
        public ActionResult UserInsert(UserInfo postData)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            //插入信息到 User 表
            postData.Dept_ID = CurrentUser.Dept_ID;
            postData.Birthday = DateTime.Now;

            postData.Password = EncryptHelper.ComputeHash(postData.Password, postData.Name.ToLower());

            int User_Id = BLLFactory<User>.Instance.Insert2(postData);

            //插入信息到 User_Role 表
            string U_Rsql = string.Format(@"INSERT INTO
                    T_ACL_User_Role (User_ID, Role_ID)
                VALUES ({0}, (SELECT top 1 id FROM T_ACL_Role where name='学生'))", User_Id
            );
            baseBLL.Update(CommandType.Text, U_Rsql, null);
            int XueYuan_Id = int.Parse(postData.Dept_ID);
            XueShengInfo xueShengInfo = new XueShengInfo();
            xueShengInfo.User_Id = User_Id;
            xueShengInfo.XingMing = postData.Name;
            xueShengInfo.XueYuan_Id = XueYuan_Id;
            xueShengInfo.XueHao = postData.Qq;

            //插入信息到 XueSheng 表
            bool result = BLLFactory<XueSheng>.Instance.Insert(xueShengInfo);
            return Content(result);
        }

        //参数 1.User的ID 2.要更改学生表的学生学号 3.要更改的User表的信息
        [HttpPost]
        public ActionResult UserUpdate(string XueShengID, string OldXueHao, FormCollection formValues)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);


            var UserID = BLLFactory<XueSheng>.Instance.FindByID(XueShengID).User_Id;

            UserInfo obj = BLLFactory<User>.Instance.FindByID(UserID);
                //baseBLL.FindByID(userId);//这儿要UserInfo的对象
            
            if (obj != null)
            {
                //遍历提交过来的数据（可能是实体类的部分属性更新）
                foreach (string key in formValues.Keys)
                {
                    string value = formValues[key];
                    System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(key);
                    if (propertyInfo != null)
                    {
                        try
                        {
                            // obj对象有key的属性，把对应的属性值赋值给它(从字符串转换为合适的类型）
                            //如果转换失败，会抛出InvalidCastException异常
                            propertyInfo.SetValue(obj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                        }
                        catch { }
                    }
                }
            }
            if (obj != null)
            {


                UserInfo objinfo = BLLFactory<User>.Instance.FindByID(UserID);
                if (objinfo.Password != obj.Password)    //判断用户密码有没有更改（密文）
                {
                    obj.Password = EncryptHelper.ComputeHash(obj.Password, obj.Name.ToLower());
                }
                
                BLLFactory<User>.Instance.Update(obj, UserID);
                //int Dept_ID = baseBLL.FindSingle(obj.Email).XueYuan_Id;

                obj.Dept_ID = CurrentUser.Dept_ID;
                int XueYuan_Id = int.Parse(obj.Dept_ID);
                XueShengInfo xueShengInfo = new XueShengInfo();
                //xueShengInfo.User_Id = User_Id;
                xueShengInfo.XingMing = obj.Name;
                xueShengInfo.XueYuan_Id = XueYuan_Id;
                xueShengInfo.XueHao = obj.Qq;
                xueShengInfo.User_Id = UserID;

                //插入信息到 XueSheng 表
                //int xueShengId = BLLFactory<XueSheng>.Instance.FindByID(XueHao).ID;
                bool result = BLLFactory<XueSheng>.Instance.Update(xueShengInfo, XueShengID);
                return Content(result);
            }
            return Content("用户不存在");

        }

        public int UserDelet(string User_Id)
        {
            int count = 0; 
            string sql = string.Format(@"
                DELETE
                    dbo.XiangMu.*
                FROM
                dbo.XiangMu 
                    INNER JOIN dbo.XiangMuXueSheng ON dbo.XiangMuXueSheng.XiangMu_Id = dbo.XiangMu.ID
                    INNER JOIN dbo.XueSheng ON dbo.XiangMuXueSheng.XueSheng_Id = dbo.XueSheng.ID 
                    INNER JOIN dbo.T_ACL_User ON dbo.T_ACL_User.ID = dbo.XueSheng.User_Id
                WHERE
                    dbo.T_ACL_User.ID = {0}", User_Id
            );

            return count;
        }


    }
}
