using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCZY.Comments;

namespace SCZY.FramWork
{
    /// <summary>
    /// 业务基类对象
    /// </summary>
    /// <typeparam name="T">业务对象类型</typeparam>
    public class BaseBLL<T> where T : BaseEntity, new()
    {
        #region 构造函数

        private string dalName = "";

        /// <summary>
        /// BLL业务类的全名（子类必须实现），可使用this.GetType().FullName
        /// </summary>
        protected string bllFullName;

        /// <summary>
        /// 数据访问层程序集的清单文件的文件名，不包括其扩展名，可使用Assembly.GetExecutingAssembly().GetName().Name
        /// </summary>
        protected string dalAssemblyName;

        /// <summary>
        /// BLL命名空间的前缀（BLL.)
        /// </summary>
        protected string bllPrefix = "BLL.";

        /// <summary>
        /// 基础数据访问层接口对象
        /// </summary>
        protected IBaseDAL<T> baseDal = null;

        /// <summary>
        /// 默认构造函数，调用后需手动调用一次 Init() 方法进行对象初始化
        /// </summary>
        public BaseBLL() { }

        /// <summary>
        /// 参数赋值后，初始化相关对象
        /// </summary>
        /// <param name="bllFullName">BLL业务类的全名（子类必须实现）,子类构造函数传入this.GetType().FullName</param>
        /// <param name="dalAssemblyName">数据访问层程序集的清单文件的文件名，不包括其扩展名，可使用Assembly.GetExecutingAssembly().GetName().Name</param>
        /// <param name="bllPrefix">BLL命名空间的前缀（BLL.)</param>
        protected void Init(string bllFullName, string dalAssemblyName, string bllPrefix = "BLL.")
        {
            if (string.IsNullOrEmpty(bllFullName))
                throw new ArgumentNullException("子类未设置bllFullName业务类全名！");

            if (string.IsNullOrEmpty(dalAssemblyName))
                throw new ArgumentNullException("子类未设置dalAssemblyName程序集名称！");

            //赋值，准备构建对象
            this.bllFullName = bllFullName;
            this.dalAssemblyName = dalAssemblyName;
            this.bllPrefix = bllPrefix;         
            string DALPrefix = "";


            string[] sArray = bllFullName.Split('.');

            bool state = false;
            bllFullName = null;

            foreach (var item in sArray)
            {
                if (bllFullName == null)
                {
                    bllFullName = bllFullName + item;
                }
                else
                {
                    if (state == false)
                    {
                        bllFullName = bllFullName + "." + item;
                    }
                    else
                    {
                        bllFullName = bllFullName + ".DAL" + item;
                    }
                    
                }
                
                if (item == "BLL")
                {
                    state = true;
                }
            }

            DALPrefix = "DAL.DALSQL.";

            this.dalName = bllFullName.Replace(bllPrefix, DALPrefix);//替换中级的BLL.为DAL.，就是DAL类的全名
           // this.dalName = "SCZY.DAL.DALSQL.DALUser"; 
            baseDal = Reflect<IBaseDAL<T>>.Create(this.dalName, dalAssemblyName);//构造对应的DAL数据访问层的对象类
        }

        /// <summary>
        /// 调用前检查baseDal是否为空引用
        /// </summary>
        protected void CheckDAL()
        {
            if (baseDal == null)
            {
                throw new ArgumentNullException("baseDal", "未能成功创建对应的DAL对象，请在BLL业务类构造函数中调用base.Init(**,**)方法，如base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);");
            }
        }

        /// <summary>
        /// 插入指定对象到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <returns>执行操作是否成功。</returns>
        public virtual bool Insert(T obj)
        {
            CheckDAL();
            return baseDal.Insert(obj);
        }

        /// <summary>
        /// 插入指定对象到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns>执行成功返回新增记录的自增长ID。</returns>
        public virtual bool Insert(T obj, DbTransaction trans)
        {
            CheckDAL();
            return baseDal.Insert(obj, trans);

        }


        /// <summary>
        /// 更新对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool Update(T obj, object primaryKeyValue)
        {
            CheckDAL();
            return baseDal.Update(obj, primaryKeyValue);
        }

        ///// <summary>
        ///// 通用获取集合对象方法
        ///// </summary>
        ///// <param name="sql">查询的Sql语句</param>
        ///// <param name="paramList">参数列表，如果没有则为null</param>
        ///// <returns></returns>
        //public virtual List<T> GetList(string sql)
        //{
        //    CheckDAL();
        //    return baseDal.GetList(sql, null);
        //}

        public virtual T FindByID(object key)
        {
            CheckDAL();
            return baseDal.FindByID(key);
        }

        /// <summary>
        /// 根据指定对象的ID,从数据库中删除指定对象
        /// </summary>
        /// <param name="key">指定对象的ID</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        public virtual bool Delete(object key)
        {
            CheckDAL();
            return baseDal.Delete(key);
        }

        #endregion
    }
}
