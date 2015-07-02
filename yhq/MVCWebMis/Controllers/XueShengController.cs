using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    public class XueShengController : BusinessController<XueSheng, XueShengInfo>
    {
        public XueShengController() : base()
        {
        }

        public virtual ActionResult FindXueShengWithPager()
        {
            //����û��Ƿ���Ȩ�ޣ������׳�MyDenyAccessException�쳣
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string sql =
                @"SELECT
                dbo.XueSheng.ID,
                dbo.XueSheng.XueYuan_Id,
                dbo.XueSheng.XueHao,
                dbo.XueSheng.XingMing,
                dbo.XueSheng.ZhuanYe,
                dbo.XueSheng.XingBie,
                dbo.XueSheng.ShouJi,
                dbo.XueYuan.XueYuan
                FROM
                dbo.XueSheng
                INNER JOIN dbo.XueYuan ON dbo.XueSheng.XueYuan_Id = dbo.XueYuan.ID";

           
            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            DataTable dt = baseBLL.SqlTableWithPager(sql, "*", condition: where, sortField: "ID", isDescending: false, info: pagerInfo);

            //Json��ʽ��Ҫ��{total:22,rows:{}}
            //�����Json�ĸ�ʽ����
            string dts = JsonConvert.SerializeObject(dt, new DataTableConverter());
            JArray array = JsonConvert.DeserializeObject<JArray>(dts);
            var result = new { total = pagerInfo.RecordCount, rows = array };
            return JsonDate(result);
        }
    }
}
