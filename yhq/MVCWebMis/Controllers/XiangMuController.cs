using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aspose.Words.Lists;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using WHC.Framework.ControlUtil;
using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;

namespace WHC.MVCWebMis.Controllers
{
    public class XiangMuController : BusinessController<XiangMu, XiangMuInfo>
    {
        public XiangMuController() : base()
        {
            
        }
        public virtual ActionResult FindXiangMuWithPage()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string Usersql =  //确定当前用户身份权限
                string.Format(@"SELECT T_ACL_Role.Name FROM T_ACL_Role INNER JOIN T_ACL_User_Role ON T_ACL_Role.ID = T_ACL_User_Role.Role_ID 
                        INNER JOIN T_ACL_User ON T_ACL_User_Role.User_ID = T_ACL_User.ID  where T_ACL_User.ID = {0}", CurrentUser.ID);
            var tt = BLLFactory<Role>.Instance.SqlValueList(Usersql);
            var roles = tt.Split(',');
            if (roles.Contains("管理员"))
            {
                string Asql =
                @"SELECT
                  DISTINCT
                    dbo.XiangMu.*
                    FROM
                    dbo.XueYuan JOIN dbo.ZhiDaoJiaoShi ON dbo.ZhiDaoJiaoShi.XueYuan_Id = dbo.XueYuan.ID
                    INNER JOIN dbo.XueSheng ON dbo.XueSheng.XueYuan_Id = dbo.XueYuan.ID
                    INNER JOIN dbo.XiangMuXueSheng ON dbo.XiangMuXueSheng.XueSheng_Id = dbo.XueSheng.ID
                    INNER JOIN dbo.XiangMu ON dbo.XiangMuXueSheng.XiangMu_Id = dbo.XiangMu.ID

                    INNER JOIN dbo.JingSai ON dbo.XiangMu.JingSai_Id = dbo.JingSai.ID              

                    INNER JOIN dbo.XiangMuZhiDaoJiaoShi ON dbo.XiangMuZhiDaoJiaoShi.ZhiDaoJiaoShi_Id = dbo.ZhiDaoJiaoShi.ID
                ";

                string LeiBie = Request["WHC_LeiBie"] ?? "";
                string XueYuan = Request["WHC_XueYuan"] ?? "";
                string XingMing = Request["WHC_XingMing"] ?? "";
                if (LeiBie != "")
                    Asql += " where LeiBie ='" + LeiBie + "'";
                if (XueYuan != "")
                    Asql += " AND XueYuan ='" + XueYuan + "'";
                if (XueYuan != "")
                    Asql += " AND dbo.ZhiDaoJiaoShi.XingMing ='" + XingMing + "'";

                string where = GetPagerCondition();
                PagerInfo pagerInfo = GetPagerInfo();
                DataTable dt = baseBLL.SqlTableWithPager(Asql, "*", condition: where, sortField: "ID", isDescending: false, info: pagerInfo);
             
                //Json格式的要求{total:22,rows:{}}
                //构造成Json的格式传递               
                string dts = JsonConvert.SerializeObject(dt, new DataTableConverter());
               ;
                JArray array = JsonConvert.DeserializeObject<JArray>(dts);
                var result = new { total = pagerInfo.RecordCount, rows = array };
                
                return JsonDate(result);
            }
            else if (roles.Contains("学生"))
            {
                string Ssql = String.Format(@"SELECT
                    dbo.XiangMu.*,
                    dbo.XueSheng.User_Id
                    FROM
                    dbo.XiangMuXueSheng
                    INNER JOIN dbo.XueSheng ON dbo.XiangMuXueSheng.XueSheng_Id = dbo.XueSheng.ID
                    INNER JOIN dbo.XiangMu ON dbo.XiangMuXueSheng.XiangMu_Id = dbo.XiangMu.ID 
                    WHERE
                    dbo.XueSheng.User_Id = {0}", CurrentUser.ID
                );

                string LeiBie = Request["WHC_LeiBie"] ?? "";
                if (LeiBie != "")
                    Ssql += " where LeiBie ='" + LeiBie + "'";

                string where = GetPagerCondition();
                PagerInfo pagerInfo = GetPagerInfo();

                DataTable dt = baseBLL.SqlTableWithPager(Ssql, "*", condition: where, sortField: "ID", isDescending: false, info: pagerInfo);

                //Json格式的要求{total:22,rows:{}}
                //构造成Json的格式传递
                string dts = JsonConvert.SerializeObject(dt, new DataTableConverter());
                JArray array = JsonConvert.DeserializeObject<JArray>(dts);
                var result = new { total = pagerInfo.RecordCount, rows = array };
                return JsonDate(result);
            }
            else 
            {
                string Tsql = string.Format(@"SELECT
                    dbo.XiangMu.*
                    FROM
                    dbo.XueYuan
                    INNER JOIN dbo.XueSheng ON dbo.XueSheng.XueYuan_Id = dbo.XueYuan.ID
                    INNER JOIN dbo.XiangMuXueSheng ON dbo.XiangMuXueSheng.XueSheng_Id = dbo.XueSheng.ID 
                    INNER JOIN dbo.XiangMu ON dbo.XiangMuXueSheng.XiangMu_Id = dbo.XiangMu.ID
                    WHERE
                    dbo.XueSheng.XueYuan_Id = {0}", CurrentUser.Dept_ID
                    );

                string where = GetPagerCondition();
                PagerInfo pagerInfo = GetPagerInfo();
                //DataTable dt = BLLFactory<XiangMu>.Instance.SqlTable(Tsql);

                DataTable dt = baseBLL.SqlTableWithPager(Tsql, "*", condition: where, sortField: "ID", isDescending: false, info: pagerInfo);


                string dts = JsonConvert.SerializeObject(dt, new DataTableConverter());
                JArray array = JsonConvert.DeserializeObject<JArray>(dts);
                var result = new { total = pagerInfo.RecordCount, rows = array };
                return JsonDate(result);

            }



            
        }

        public ActionResult XiangMuShenPi(string Ids)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string Usersql =  //确定当前用户身份权限
                string.Format(@"SELECT T_ACL_Role.Name FROM T_ACL_Role INNER JOIN T_ACL_User_Role ON T_ACL_Role.ID = T_ACL_User_Role.Role_ID 
                        INNER JOIN T_ACL_User ON T_ACL_User_Role.User_ID = T_ACL_User.ID  where T_ACL_User.ID = {0}", CurrentUser.ID);
            var tt = BLLFactory<Role>.Instance.SqlValueList(Usersql);
            var roles = tt.Split(',');
            int i = 0;
            if (roles.Contains("管理员"))
            {
                List<XiangMuInfo> list = BLLFactory<XiangMu>.Instance.FindByIDs(Ids);
                foreach (XiangMuInfo xiangMuInfo in list)
                {
                    if (xiangMuInfo.XueXiaoShenPi == false)
                    {
                        xiangMuInfo.XueXiaoShenPi = true;

                        baseBLL.Update(xiangMuInfo, xiangMuInfo.ID.ToString());
                        i++;
                    }
                }
            }
            else
            {
                List<XiangMuInfo> list = BLLFactory<XiangMu>.Instance.FindByIDs(Ids);
                foreach (XiangMuInfo xiangMuInfo in list)
                {
                    if (xiangMuInfo.XueYuanShenPi == false)
                    {
                        xiangMuInfo.XueYuanShenPi = true;
                        baseBLL.Update(xiangMuInfo, xiangMuInfo.ID.ToString());
                        i++;
                    }
                }
            }
            return Content(i);
        }




        public virtual ActionResult FindXiangMuWithPage1()
        {

            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);


            string sql =
                @"SELECT
                    dbo.XiangMu.*
                    FROM
                    dbo.XiangMu
                    INNER JOIN dbo.JingSai ON dbo.XiangMu.JingSai_Id = dbo.JingSai.ID ";
            string JingSaiMingCheng = Request["WHC_JingSaiMingCheng"] ?? "";
            if (JingSaiMingCheng != "")
                sql += " where JingSaiMingCheng ='" + JingSaiMingCheng + "'";

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();

            DataTable dt = baseBLL.SqlTableWithPager(sql, "*", condition: where, sortField: "ID", isDescending: false, info: pagerInfo);

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            string dts = JsonConvert.SerializeObject(dt, new DataTableConverter());
            JArray array = JsonConvert.DeserializeObject<JArray>(dts);
            var result = new { total = pagerInfo.RecordCount, rows = array };
            return JsonDate(result);
        }

        public virtual ActionResult FindQiYeDaoShiWithPage()
        {

            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);


            string sql =
                @"SELECT
                dbo.XiangMu.ID,
                dbo.XiangMu.QiYeDaoShi_Id,
                dbo.QiYeDaoShi.XingMing,
                dbo.QiYeDaoShi.ZhuanYe,
                dbo.QiYeDaoShi.ZhiCheng,
                dbo.QiYeDaoShi.ShenFenZhengHao,
                dbo.QiYeDaoShi.ZhiWu,
                dbo.QiYeDaoShi.GongSi
                FROM
                dbo.XiangMu
                INNER JOIN dbo.QiYeDaoShi ON dbo.XiangMu.QiYeDaoShi_Id = dbo.QiYeDaoShi.ID ";           

           

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            DataTable dt = baseBLL.SqlTableWithPager(sql, "*", condition: where, sortField: "QiYeDaoShi_Id", isDescending: false, info: pagerInfo);

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            string dts = JsonConvert.SerializeObject(dt, new DataTableConverter());
            JArray array = JsonConvert.DeserializeObject<JArray>(dts);
            var result = new { total = pagerInfo.RecordCount, rows = array };
            return JsonDate(result);
        }





        protected override string GetPagerCondition()
        {
            #region 根据数据库字段列，对所有可能的参数进行获值，然后构建查询条件

            SearchCondition condition = new SearchCondition();
            DataTable dt = baseBLL.GetFieldTypeList();
            foreach (DataRow dr in dt.Rows)
            {
                string columnName = dr["ColumnName"].ToString();
                string dataType = dr["DataType"].ToString();

                //字段增加WHC_前缀字符，避免传递如URL这样的Request关键字冲突
                string columnValue = Request["WHC_" + columnName] ?? "";

                if (IsDateTime(dataType))
                {
                    condition.AddDateCondition(columnName, columnValue);
                }
                else if (IsNumericType(dataType))
                {
                    condition.AddNumberCondition(columnName, columnValue);
                }
                else
                {
                    condition.AddCondition(columnName, columnValue, SqlOperator.Like);
                }
            }
            #endregion

            #region MyRegion

            
            

            #endregion

            string where = condition.BuildConditionSql().Replace("Where", "");
            
            return where;
        }



  

    }
}
