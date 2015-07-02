using System;
using System.Collections.Generic;
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
    public class XiangMuZhiDaoJiaoShiController : BusinessController<XiangMuZhiDaoJiaoShi, XiangMuZhiDaoJiaoShiInfo>
    {
        public XiangMuZhiDaoJiaoShiController() : base()
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
            dbo.XiangMuZhiDaoJiaoShi
            INNER JOIN dbo.ZhiDaoJiaoShi ON dbo.XiangMuZhiDaoJiaoShi.ZhiDaoJiaoShi_Id = dbo.ZhiDaoJiaoShi.ID";

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
    }
}
