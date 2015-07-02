using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.Pager.Entity;
using WHC.Framework.Commons;

namespace WHC.Framework.ControlUtil
{
	/// <summary>
	/// 数据访问层的接口
	/// </summary>
    public interface IBaseDAL<T> where T : BaseEntity
	{
        #region 通用操作

        /// <summary>
        /// 获取表的所有记录数量
        /// </summary>
        /// <returns></returns>
        int GetRecordCount(string condition);

        /// <summary>
        /// 获取表的所有记录数量
        /// </summary>
        /// <returns></returns>
        int GetRecordCount();

        /// <summary>
        /// 根据condition条件，判断是否存在记录
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <returns>如果存在返回True，否则False</returns>
        bool IsExistRecord(string condition);

		/// <summary>
		/// 查询数据库,检查是否存在指定键值的对象
		/// </summary>
		/// <param name="recordTable">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
		/// <returns>存在则返回<c>true</c>，否则为<c>false</c>。</returns>
		bool IsExistKey(Hashtable recordTable);

		/// <summary>
		/// 查询数据库,检查是否存在指定键值的对象
		/// </summary>
		/// <param name="fieldName">指定的属性名</param>
		/// <param name="key">指定的值</param>
		/// <returns>存在则返回<c>true</c>，否则为<c>false</c>。</returns>
		bool IsExistKey(string fieldName, object key);

		/// <summary>
		/// 获取数据库中该对象的最大ID值
		/// </summary>
		/// <returns>最大ID值</returns>
		int GetMaxID();

        /// <summary>
        /// 根据指定对象的ID,从数据库中删除指定对象
        /// </summary>
        /// <param name="key">指定对象的ID</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool Delete(object key);

		/// <summary>
		/// 根据指定对象的ID,从数据库中删除指定对象
		/// </summary>
		/// <param name="key">指定对象的ID</param>
        /// <param name="trans">事务对象</param>
		/// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool Delete(object key, DbTransaction trans);
	
		/// <summary>
		/// 根据条件,从数据库中删除指定对象
		/// </summary>
		/// <param name="condition">删除记录的条件语句</param>
		/// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
		bool DeleteByCondition(string condition);

		/// <summary>
		/// 根据条件,从数据库中删除指定对象
		/// </summary>
		/// <param name="condition">删除记录的条件语句</param>
        /// <param name="trans">事务对象</param>
		/// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
		bool DeleteByCondition(string condition, DbTransaction trans);

        /// <summary>
        /// 根据指定条件,从数据库中删除指定对象
        /// </summary>
        /// <param name="condition">删除记录的条件语句</param>
        /// <param name="trans">事务对象</param>
        /// <param name="paramList">Sql参数列表</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool DeleteByCondition(string condition, DbTransaction trans, IDbDataParameter[] paramList);

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
        /// 插入指定对象到数据库中,并返回自增长的键值
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <returns>执行成功返回True</returns>
        int Insert2(T obj);

        /// <summary>
        /// 插入指定对象到数据库中,并返回自增长的键值
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns>执行成功返回True</returns>
        int Insert2(T obj, DbTransaction trans);

        /// <summary>
        /// 更新对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool Update(T obj, object primaryKeyValue);

        /// <summary>
        /// 更新对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <param name="trans">事务</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool Update(T obj, object primaryKeyValue, DbTransaction trans);

        /// <summary>
        /// 直接使用Sql进行数据的更新
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool Update(CommandType commandType, string sql);

        /// <summary>
        /// 更新某个表一条记录(只适用于用单键,用string类型作键值的表)
        /// </summary>
        /// <param name="id">ID值</param>
        /// <param name="recordField">Hashtable:键[key]为字段名;值[value]为字段对应的值</param>
        /// <param name="targetTable">需要操作的目标表名称</param>
        /// <param name="trans">事务对象,如果使用事务,传入事务对象,否则为Null不使用事务</param>
        bool Update(object id, Hashtable recordField, string targetTable, DbTransaction trans);

        /// <summary>
        /// 插入或更新对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool InsertUpdate(T obj, object primaryKeyValue);

        /// <summary>
        /// 插入或更新对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <param name="trans">事务对象</param>
        /// <returns>执行成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool InsertUpdate(T obj, object primaryKeyValue, DbTransaction trans);

        /// <summary>
        /// 如果不存在记录，则插入对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键的值</param>
        /// <returns>执行插入成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool InsertIfNew(T obj, object primaryKeyValue);

        /// <summary>
        /// 如果不存在记录，则插入对象属性到数据库中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="primaryKeyValue">主键值</param>
        /// <param name="trans">事务对象</param>
        /// <returns>执行插入成功返回<c>true</c>，否则为<c>false</c>。</returns>
        bool InsertIfNew(T obj, object primaryKeyValue, DbTransaction trans);

        /// <summary>
        /// 执行SQL查询语句，返回查询结果的所有记录的第一个字段,用逗号分隔。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>
        /// 返回查询结果的所有记录的第一个字段,用逗号分隔。
        /// </returns>
        string SqlValueList(string sql);

        /// <summary>
        /// 执行一些特殊的语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        int SqlExecute(string sql);
                        
        /// <summary>
        /// 执行一些特殊的语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="trans">事务对象</param>
        int SqlExecute(string sql, DbTransaction trans);
                       
        /// <summary>
        /// 执行存储过程函数。
        /// </summary>
        /// <param name="storeProcName">存储过程函数</param>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        int StoreProcExecute(string storeProcName, DbParameter[] parameters);

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataTable集合。
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <returns></returns>
        DataTable SqlTable(string sql);

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="fieldsToReturn">需要返回的列</param>
        /// <param name="condition">查询的条件</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="isDescending">是否降序</param>
        /// <param name="info">分页实体</param>
        /// <returns>指定对象的集合</returns>
        DataTable SqlTableWithPager(string sql, string fieldsToReturn, string condition, string sortField,
            bool isDescending, PagerInfo info);

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataTable集合。
        /// </summary>
        /// <param name="sql">SQL查询语句</param>
        /// <param name="parameters">SQL查询参数</param>
        /// <returns></returns>
        DataTable SqlTable(string sql, DbParameter[] parameters);

        /// <summary>
        /// 打开数据库连接，并创建事务对象
        /// </summary>
        DbTransaction CreateTransaction();
                        
        /// <summary>
        /// 测试数据库是否正常连接
        /// </summary>
        bool TestConnection(string connectionString);

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="sql"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        bool Update(CommandType commandType, string sql, DbTransaction trans); 

        #endregion

        #region 返回实体类操作

        /// <summary>
        /// 查询数据库,检查是否存在指定ID的对象
        /// </summary>
        /// <param name="key">对象的ID值</param>
        /// <returns>存在则返回指定的对象,否则返回Null</returns>
        T FindByID(object key);

        /// <summary>
        /// 根据条件查询数据库,如果存在返回第一个对象
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <returns>指定的对象</returns>
        T FindSingle(string condition);

        /// <summary>
        /// 根据条件查询数据库,如果存在返回第一个对象
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <returns>指定的对象</returns>
        T FindSingle(string condition, string orderBy);

        /// <summary>
        /// 根据条件查询数据库,如果存在返回第一个对象
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <param name="paramList">参数列表</param>
        /// <returns>指定的对象</returns>
        T FindSingle(string condition, string orderBy, IDbDataParameter[] paramList);

        /// <summary>
        /// 查找记录表中最新的一条记录
        /// </summary>
        /// <returns></returns>
        T FindLast();

        /// <summary>
        /// 查找记录表中最旧的一条记录
        /// </summary>
        /// <returns></returns>
        T FindFirst(); 

        #endregion

        #region 返回集合的接口

        /// <summary>
        /// 根据ID字符串(逗号分隔)获取对象列表
        /// </summary>
        /// <param name="idString">ID字符串(逗号分隔)</param>
        /// <returns>符合条件的对象列表</returns>
        List<T> FindByIDs(string idString);

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <returns>指定对象的集合</returns>
        List<T> Find(string condition);

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <returns>指定对象的集合</returns>
        List<T> Find(string condition, string orderBy);

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <param name="paramList">参数列表</param>
        /// <returns>指定对象的集合</returns>
        List<T> Find(string condition, string orderBy, IDbDataParameter[] paramList);
                        
        /// <summary>
        /// 通用获取集合对象方法
        /// </summary>
        /// <param name="sql">查询的Sql语句</param>
        /// <param name="paramList">参数列表，如果没有则为null</param>
        /// <returns></returns>
        List<T> GetList(string sql, IDbDataParameter[] paramList);

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="info">分页实体</param>
        /// <returns>指定对象的集合</returns>
        List<T> FindWithPager(string condition, PagerInfo info);

        /// <summary>
		/// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
		/// </summary>
		/// <param name="condition">查询的条件</param>
		/// <param name="info">分页实体</param>
        /// <param name="fieldToSort">排序字段</param>
		/// <returns>指定对象的集合</returns>
        List<T> FindWithPager(string condition, PagerInfo info, string fieldToSort);
                      
        /// <summary>
		/// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
		/// </summary>
		/// <param name="condition">查询的条件</param>
		/// <param name="info">分页实体</param>
        /// <param name="fieldToSort">排序字段</param>
        /// <param name="desc">是否降序</param>
        /// <returns>指定对象的集合</returns>
        List<T> FindWithPager(string condition, PagerInfo info, string fieldToSort, bool desc);


        /// <summary>
        /// 返回数据库所有的对象集合
        /// </summary>
        /// <returns>指定对象的集合</returns>
        List<T> GetAll();

        /// <summary>
        /// 返回数据库所有的对象集合
        /// </summary>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <returns>指定对象的集合</returns>
        List<T> GetAll(string orderBy);
                       
        /// <summary>
		/// 返回数据库所有的对象集合(用于分页数据显示)
		/// </summary>
        /// <param name="info">分页实体信息</param>
        /// <returns>指定对象的集合</returns>
        List<T> GetAll(PagerInfo info);

		/// <summary>
		/// 返回数据库所有的对象集合(用于分页数据显示)
		/// </summary>
        /// <param name="info">分页实体信息</param>
        /// <param name="fieldToSort">排序字段</param>
        /// <param name="desc">是否降序</param>
        /// <returns>指定对象的集合</returns>
        List<T> GetAll(PagerInfo info, string fieldToSort, bool desc);


        /// <summary>
        /// 返回所有记录到DataTable集合中
        /// </summary>
        /// <returns></returns>
        DataTable GetAllToDataTable();

        /// <summary>
        /// 返回所有记录到DataTable集合中
        /// </summary>
        /// <param name="orderBy">自定义排序语句，如Order By Name Desc；如不指定，则使用默认排序</param>
        /// <returns></returns>
        DataTable GetAllToDataTable(string orderBy);

        /// <summary>
        /// 根据分页条件，返回DataTable对象
        /// </summary>
        /// <param name="info">分页条件</param>
        /// <returns></returns>
        DataTable GetAllToDataTable(PagerInfo info);

        /// <summary>
        /// 根据分页条件，返回DataTable对象
        /// </summary>
        /// <param name="info">分页条件</param>
        /// <param name="fieldToSort">排序字段</param>
        /// <param name="desc">是否降序</param>
        /// <returns></returns>
        DataTable GetAllToDataTable(PagerInfo info, string fieldToSort, bool desc);


        /// <summary>
        /// 根据查询条件，返回记录到DataTable集合中
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        DataTable FindToDataTable(string condition);

        /// <summary>
        /// 根据查询条件，返回记录到DataTable集合中
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="info">分页条件</param>
        /// <returns></returns>
        DataTable FindToDataTable(string condition, PagerInfo info);
        
        /// <summary>
        /// 根据条件查询数据库,并返回DataTable集合(用于分页数据显示)
        /// </summary>
        /// <param name="condition">查询的条件</param>
        /// <param name="info">分页实体</param>
        /// <param name="fieldToSort">排序字段</param>
        /// <param name="desc">是否降序</param>
        /// <returns>指定DataTable的集合</returns>
        DataTable FindToDataTable(string condition, PagerInfo info, string fieldToSort, bool desc);

        /// <summary>
        /// 获取某字段数据字典列表
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        List<string> GetFieldList(string fieldName);

        /// <summary>
        /// 根据条件，从视图里面获取记录
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        DataTable FindByView(string viewName, string condition);
                       
        /// <summary>
        /// 根据条件，从视图里面获取记录
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="condition">查询条件</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="isDescending">是否为降序</param>
        /// <returns></returns>
        DataTable FindByView(string viewName, string condition, string sortField, bool isDescending);

        /// <summary>
        /// 获取前面记录指定数量的记录
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="count">指定数量</param>
        /// <param name="orderBy">排序条件，例如order by id</param>
        /// <returns></returns>
        DataTable GetTopResult(string sql, int count, string orderBy);
                       
        /// <summary>
        /// 根据条件，从视图里面获取记录
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="condition">查询条件</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="isDescending">是否为降序</param>
        /// <param name="info">分页条件</param>
        /// <returns></returns>
        DataTable FindByViewWithPager(string viewName, string condition, string sortField, bool isDescending, PagerInfo info);

        #endregion

        /// <summary>
        /// 初始化数据库表名
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        void InitTableName(string tableName);

        /// <summary>
        /// 获取表的字段名称和数据类型列表
        /// </summary>
        /// <returns></returns>
        DataTable GetFieldTypeList();
                        
        /// <summary>
        /// 获取字段中文别名（用于界面显示）的字典集合
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetColumnNameAlias();
                        
        /// <summary>
        /// 获取指定字段的报表数据
        /// </summary>
        /// <param name="fieldName">表字段</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        DataTable GetReportData(string fieldName, string condition);
	}
}