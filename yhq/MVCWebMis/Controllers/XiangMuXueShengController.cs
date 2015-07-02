using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Data;
namespace WHC.MVCWebMis.Controllers
{
    public class XiangMuXueShengController : BusinessController<XiangMuXueSheng, XiangMuXueShengInfo>
    {
        public XiangMuXueShengController() : base()
        {
        }

        public override ActionResult FindWithPager()
        {



            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string sql = @"
            SELECT
            *
            FROM
            dbo.XiangMuXueSheng
            INNER JOIN dbo.XueSheng ON dbo.XiangMuXueSheng.XueSheng_Id = dbo.XueSheng.ID";

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();


            DataTable dt = baseBLL.SqlTableWithPager(sql, "*", condition: where, sortField: "WeiCi", isDescending: false, info: pagerInfo);


            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            string dts = JsonConvert.SerializeObject(dt, new DataTableConverter());
            JArray array = JsonConvert.DeserializeObject<JArray>(dts);
            var result = new { total = pagerInfo.RecordCount, rows = array };
            return JsonDate(result);
        }

        public string WUpload(XiangMuXueShengInfo WpostData)
        {
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string sql = string.Format(@"UPDATE
                dbo.XiangMuXueSheng
                SET
                dbo.XiangMuXueSheng.WeiCi = {0}

                where 
                    dbo.XiangMuXueSheng.XiangMu_Id = {1} AND 
                    dbo.XiangMuXueSheng.XueSheng_Id = {2}", WpostData.WeiCi, WpostData.XiangMu_Id, WpostData.XueSheng_Id
            );
            var result = baseBLL.Update(CommandType.Text, sql,null);
            //SqlCommand(sql);

            if (result)
                return "true";
            return "false";

        }


    }
}
