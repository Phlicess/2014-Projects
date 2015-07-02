using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCZY.FramWork
{
    /// <summary>
    /// 数据访问层的接口
    /// </summary>
    public interface IBaseDAL<T> where T : BaseEntity
    {
        /// <summary>
        /// 插入指定对象到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <returns>执行成功返回True</returns>
        bool Insert(T obj);

        /// <summary>
        /// 插入指定对象到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns>执行成功返回True</returns>
        bool Insert(T obj, DbTransaction trans);


        /// <summary>
        /// 更新对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool Update(T obj, object primaryKeyValue);


        /// <summary>
        /// 查询数据库,检查是否存在指定ID的对象
        /// </summary>
        /// <param name="key">对象的ID值</param>
        /// <returns>存在则返回指定的对象,否则返回Null</returns>
        T FindByID(object key);


        /// <summary>
        /// 根据指定对象的ID,从数据库中删除指定对象
        /// </summary>
        /// <param name="key">指定对象的ID</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool Delete(object key);


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        T FindSingle(string condition);

        /// <summary>
        /// 根据条件参数 查找所有实体
        /// </summary>
        /// <param name="condition">条件参数</param>
        /// <returns></returns>
        List<T> Find(string condition);

    }
}
