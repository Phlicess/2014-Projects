using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Aspose.Words.Lists;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.Pager.Entity;
using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.BLL;

namespace WHC.MVCWebMis.Controllers
{
    /// <summary>
    /// 本控制器基类专门为访问数据业务对象而设的基类
    /// </summary>
    /// <typeparam name="B">业务对象类型</typeparam>
    /// <typeparam name="T">实体类类型</typeparam>
    public class BusinessController<B, T> : BaseController
        where B : class
        where T : WHC.Framework.ControlUtil.BaseEntity, new()
    {
        #region 构造函数及常用

        /// <summary>
        /// 业务对象所在程序集的文件名，不包括其扩展名，如: WHC.Security.Core
        /// </summary>
        protected string bllAssemblyName = null;

        /// <summary>
        /// 基础业务对象
        /// </summary>
        protected BaseBLL<T> baseBLL = null;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BusinessController()
        {
            this.bllAssemblyName = System.Reflection.Assembly.GetAssembly(typeof(B)).GetName().Name;
            this.baseBLL = Reflect<BaseBLL<T>>.Create(typeof(B).FullName, bllAssemblyName);//构造对应的 BaseBLL<T> 业务访问层的对象类

            //调用前检查baseBLL是否为空引用
            if (baseBLL == null)
            {
                throw new ArgumentNullException("baseBLL", "未能成功创建对应的BaseBLL<T> 业务访问层的对象。");
            }
        }

        /// <summary>
        /// 默认的视图控制方法
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            return View();
        }

        #endregion

        #region 对象添加、修改、查询接口

        /// <summary>
        /// 插入指定对象到数据库中
        /// </summary>
        /// <param name="info">指定的对象</param>
        /// <returns>执行操作是否成功。</returns>
        public virtual ActionResult Insert(T info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            bool result = false;
            if (info != null)
            {
                result = baseBLL.Insert(info);
            }
            return Content(result);
        }

        /// <summary>
        /// 插入指定对象到数据库中
        /// </summary>
        /// <param name="info">指定的对象</param>
        /// <returns>执行成功返回新增记录的自增长ID。</returns>
        public virtual ActionResult Insert2(T info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            int result = -1;
            if (info != null)
            {
                result = baseBLL.Insert2(info);
            }
            return Content(result);
        }

        /// <summary>
        /// 更新对象属性到数据库中
        /// </summary>
        /// <param name="info">指定的对象</param>
        /// <param name="id">主键ID的值</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual ActionResult Update(string id, FormCollection formValues)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.UpdateKey);

            T obj = baseBLL.FindByID(id);
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

            bool result = baseBLL.Update(obj, id);
            return Content(result);
        }

        ///// <summary>
        ///// 更新对象属性到数据库中
        ///// </summary>
        ///// <param name="info">指定的对象</param>
        ///// <param name="id">主键ID的值</param>
        ///// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        //public virtual ActionResult Update(T info, string id)
        //{
        //    bool result = baseBLL.Update(info, id);
        //    return Content(result);
        //}

        /// <summary>
        /// 插入或更新对象属性到数据库中
        /// </summary>
        /// <param name="info">指定的对象</param>
        /// <param name="id">主键的值</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual ActionResult InsertUpdate(T info, string id)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            bool result = baseBLL.InsertUpdate(info, id);
            return Content(result);
        }

        /// <summary>
        /// 如果不存在记录，则插入对象属性到数据库中
        /// </summary>
        /// <param name="info">指定的对象</param>
        /// <param name="id">主键的值</param>
        /// <returns>执行插入成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual ActionResult InsertIfNew(T info, string id)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            bool result = baseBLL.InsertIfNew(info, id);
            return Content(result);
        }

        /// <summary>
        /// 查询数据库,检查是否存在指定ID的对象
        /// </summary>
        /// <param name="id">对象的ID值</param>
        /// <returns>存在则返回指定的对象,否则返回Null</returns>
        public virtual ActionResult FindByID(string id)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ViewKey);

            ActionResult result = Content("");
            T info = baseBLL.FindByID(id);
            if (info != null)
            {
                result = JsonDate(info);
            }

            return result;
        }

        /// <summary>
        /// 根据条件查询数据库,如果存在返回第一个对象
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <returns>指定的对象</returns>
        public virtual ActionResult FindSingle(string condition)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ViewKey);

            ActionResult result = Content("");
            T info = baseBLL.FindSingle(condition);
            if (info != null)
            {
                result = JsonDate(info);
            }
            return result;
        }

        /// <summary>
        /// 根据条件查询数据库,如果存在返回第一个对象
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns>指定的对象</returns>
        public virtual ActionResult FindSingle2(string condition, string orderBy)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ViewKey);

            ActionResult result = Content("");
            T info = baseBLL.FindSingle(condition, orderBy);
            if (info != null)
            {
                result = JsonDate(info);
            }
            return result;
        }

        /// <summary>
        /// 查找记录表中最旧的一条记录
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult FindFirst()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ViewKey);

            ActionResult result = Content("");
            T info = baseBLL.FindFirst();
            if (info != null)
            {
                result = JsonDate(info);
            }
            return result;
        }

        /// <summary>
        /// 查找记录表中最新的一条记录
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult FindLast()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ViewKey);

            ActionResult result = Content("");
            T info = baseBLL.FindLast();
            if (info != null)
            {
                result = JsonDate(info);
            }
            return result;
        }



        #endregion

        #region 返回集合的接口

        /// <summary>
        /// 根据ID字符串(逗号分隔)获取对象列表
        /// </summary>
        /// <param name="ids">ID字符串(逗号分隔)</param>
        /// <returns>符合条件的对象列表</returns>
        public virtual ActionResult FindByIDs(string ids)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(ids))
            {
                List<T> list = baseBLL.FindByIDs(ids);
                result = JsonDate(list);
            }
            return result;
        }

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <returns>指定对象的集合</returns>
        public virtual ActionResult Find(string condition)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(condition))
            {
                List<T> list = baseBLL.Find(condition);
                result = JsonDate(list);
            }
            return result;
        }

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns>指定对象的集合</returns>
        public virtual ActionResult Find2(string condition, string orderBy)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(orderBy))
            {
                List<T> list = baseBLL.Find(condition, orderBy);
                result = JsonDate(list);
            }
            return result;
        }

        /// <summary>
        /// 根据Request参数获取分页对象数据
        /// </summary>
        /// <returns></returns>
        protected virtual PagerInfo GetPagerInfo()
        {
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);

            PagerInfo pagerInfo = new PagerInfo();
            pagerInfo.CurrenetPageIndex = pageIndex;
            pagerInfo.PageSize = pageSize;

            return pagerInfo;
        }

        /// <summary>
        /// 获取分页操作的查询条件
        /// </summary>
        /// <returns></returns>
        protected virtual string GetPagerCondition()
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
            //string SystemType_ID = Request["SystemType_ID"] ?? "";
            //string LoginName = Request["LoginName"] ?? "";
            //string FullName = Request["FullName"] ?? "";
            //string Note = Request["Note"] ?? "";
            //string IPAddress = Request["IPAddress"] ?? "";
            //string MacAddress = Request["MacAddress"] ?? "";
            //string LastUpdated = Request["LastUpdated"] ?? "";

            //SearchCondition condition = new SearchCondition();
            //condition.AddCondition("SystemType_ID", SystemType_ID, SqlOperator.Like);
            //condition.AddCondition("LoginName", LoginName, SqlOperator.Like);
            //condition.AddCondition("FullName", FullName, SqlOperator.Like);
            //condition.AddCondition("Note", Note, SqlOperator.Like);
            //condition.AddCondition("IPAddress", IPAddress, SqlOperator.Like);
            //condition.AddCondition("MacAddress", MacAddress, SqlOperator.Like);

            //condition.AddDateCondition("LastUpdated", LastUpdated); 
            #endregion

            string where = condition.BuildConditionSql().Replace("Where", "");
            return where;
        }

        /// <summary>
        /// 判断数据类型是否为数值类型
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        protected bool IsNumericType(string dataType)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(dataType))
            {
                dataType = dataType.ToLower();
                if (dataType.Contains("int") || dataType.Contains("decimal") || dataType.Contains("double") ||
                    dataType.Contains("single") || dataType.Contains("byte") || dataType.Contains("short") ||
                    dataType.Contains("float"))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 判断数据类型是否为日期类型
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        protected bool IsDateTime(string dataType)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(dataType))
            {
                dataType = dataType.ToLower();
                if (dataType.ToLower().Contains("datetime"))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
        /// </summary>
        /// <returns>指定对象的集合</returns>
        public virtual ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            List<T> list = baseBLL.FindWithPager(where, pagerInfo);

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = list };
            return JsonDate(result);
        }

        /// <summary>
        /// 返回数据库所有的对象集合
        /// </summary>
        /// <returns>指定对象的集合</returns>
        public virtual ActionResult GetAll()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            List<T> list = baseBLL.GetAll();
            return JsonDate(list);
        }

        /// <summary>
        /// 返回数据库所有的对象集合
        /// </summary>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <returns>指定对象的集合</returns>
        public virtual ActionResult GetAll2(string orderBy)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(orderBy))
            {
                List<T> list = baseBLL.GetAll(orderBy);
                result = JsonDate(list);
            }
            return result;
        }

        /// <summary>
        /// 返回数据库所有的对象集合(用于分页数据显示)
        /// </summary>
        /// <param name="info">分页实体信息</param>
        /// <returns>指定对象的集合</returns>
        public virtual ActionResult GetAllWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            PagerInfo pagerInfo = GetPagerInfo();
            List<T> list = baseBLL.GetAll(pagerInfo);

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = list };
            return JsonDate(result);
        }

        /// <summary>
        /// 获取字段列表
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public virtual ActionResult GetFieldList(string fieldName)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(fieldName))
            {
                List<string> list = baseBLL.GetFieldList(fieldName);
                result = Json(list, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        #endregion

        #region 基础接口

        /// <summary>
        /// 获取表的所有记录数量
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetRecordCount()
        {
            int result = baseBLL.GetRecordCount();
            return Content(result);
        }

        /// <summary>
        /// 获取表的所有记录数量
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetRecordCount2(string condition)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(condition))
            {
                result = baseBLL.GetRecordCount(condition);
            }
            return Content(result);
        }

        /// <summary>
        /// 根据condition条件，判断是否存在记录
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <returns>如果存在返回True，否则False</returns>
        public virtual ActionResult IsExistRecord(string condition)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(condition))
            {
                result = baseBLL.IsExistRecord(condition);
            }
            return Content(result);
        }

        /// <summary>
        /// 查询数据库,检查是否存在指定键值的对象
        /// </summary>
        /// <param name="fieldName">指定的属性名</param>
        /// <param name="key">指定的值</param>
        /// <returns>存在则返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual ActionResult IsExistKey(string fieldName, string key)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(fieldName) && !string.IsNullOrEmpty(key))
            {
                result = baseBLL.IsExistKey(fieldName, key);
            }
            return Content(result);
        }

        /// <summary>
        /// 根据指定对象的ID,从数据库中删除指定对象
        /// </summary>
        /// <param name="id">指定对象的ID</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual ActionResult Delete(string id)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.DeleteKey);

            bool result = false;
            if (!string.IsNullOrEmpty(id))
            {
                result = baseBLL.Delete(id);
            }
            return Content(result);
        }

        /// <summary>
        /// 删除多个ID的记录
        /// </summary>
        /// <param name="ids">多个id组合，逗号分开（1,2,3,4,5）</param>
        /// <returns></returns>
        public virtual ActionResult DeleteByIds(string ids)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.DeleteKey);

            bool result = false;
            if (!string.IsNullOrEmpty(ids))
            {
                string[] idArray = ids.Split(new char[] { ',' });
                foreach (string strId in idArray)
                {
                    if (!string.IsNullOrEmpty(strId))
                    {
                        baseBLL.Delete(strId);
                    }
                }
                result = true;
            }
            return Content(result);
        }

        /// <summary>
        /// 根据指定条件,从数据库中删除指定对象
        /// </summary>
        /// <param name="condition">删除记录的条件语句</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual ActionResult DeleteByCondition(string condition)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.DeleteKey);

            bool result = false;
            if (!string.IsNullOrEmpty(condition))
            {
                result = baseBLL.DeleteByCondition(condition);
            }
            return Content(result);
        }

        #endregion

        #region 其他接口

        /// <summary>
        /// 初始化数据库表名
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        public virtual void InitTableName(string tableName)
        {
            baseBLL.InitTableName(tableName);
        }

        /// <summary>
        /// 获取表的字段名称和数据类型列表
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetFieldTypeList()
        {
            ActionResult result = Content("");
            DataTable dt = baseBLL.GetFieldTypeList();
            if (dt != null && dt.Rows.Count > 0)
            {
                //构造Json
                List<CListItem> list = new List<CListItem>();
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new CListItem(dr[0].ToString(), dr[1].ToString()));
                }
                result = Json(list, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        /// <summary>
        /// 获取字段中文别名（用于界面显示）的字典集合
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GetColumnNameAlias()
        {
            ActionResult result = Content("");
            Dictionary<string, string> dict = baseBLL.GetColumnNameAlias();
            if (dict != null && dict.Keys.Count > 0)
            {
                List<CListItem> list = new List<CListItem>();
                foreach (string key in dict.Keys)
                {
                    list.Add(new CListItem(key, dict[key]));
                }
                result = Json(list, JsonRequestBehavior.AllowGet);
            }
            return result;
        }










        #endregion

    }





}
