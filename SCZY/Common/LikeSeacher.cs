using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SCZY.Common
{
    static class LikeSeacher
    {
        /// <summary>
        /// 查询符合条件的数据返回
        /// </summary>
        /// <typeparam name="T">一个类</typeparam>
        /// <param name="obj">一个空对象</param>
        /// <param name="para">查询的条件</param>
        /// <param name="table">查询的表</param>
        /// <returns>指定类型数据的List</returns>
        static public List<T> SeacherLike<T>(T obj, string para, string table)
        {
            Type type = obj.GetType();
            table = "[" + table + "]";
            PropertyInfo[] ps = type.GetProperties();
            //构造sql查询的条件 
            string sql = "select * from " + table + " where ";
            foreach (PropertyInfo i in ps)
            {
                if (i.PropertyType == typeof(string))
                {
                    sql += i.Name + " like '%" + para + "%' or ";
                }
            }
            sql = sql.Substring(0, sql.Length - 3);
            SCZYEntities sczy = new SCZYEntities();
            List<T> list = new List<T>();
            list = sczy.Database.SqlQuery<T>(sql).ToList();
            return list;
        }

        /// <summary>
        /// 查询符合条件的数据返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="para"></param>
        /// <param name="beforePara"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        static public List<T> SeacherLike<T>(T obj, string para,string beforePara, string table)
        {
            Type type = obj.GetType();
            table = "[" + table + "]";
            PropertyInfo[] ps = type.GetProperties();
            //构造sql查询的条件 
            string sql = "select * from " + table + " where " ;
            if (beforePara == "已划价")
            {
                sql += " OrderState = " + "'已划价'" + "and ";
               
            }
            if (beforePara == "未划价")
            {
                sql += " OrderState = " + "'未划价'" + "and ";
              
            }

            foreach (PropertyInfo i in ps)
            {
                if (i.PropertyType == typeof(string))
                {
                    if (i.Name != "OrderState")
                    {
                        sql += i.Name + " like '%" + para + "%' or ";    
                    }                               
                }
            }
            sql = sql.Substring(0, sql.Length - 3);
            SCZYEntities sczy = new SCZYEntities();
            List<T> list = new List<T>();
            list = sczy.Database.SqlQuery<T>(sql).ToList();
            return list;
        }
    }
}
