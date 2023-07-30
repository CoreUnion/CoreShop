/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Model.ViewModels.Basics;
using SqlSugar;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     仓储通用接口类
    /// </summary>
    /// <typeparam name="T">泛型实体类</typeparam>
    public interface IBaseRepository<T> where T : class, new()
    {
        /// <summary>
        ///     根据主值查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体</returns>
        T QueryById(object pkValue, bool blUseNoLock = false);

        /// <summary>
        ///     根据主值查询单条数据
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<T> QueryByIdAsync(object objId, bool blUseNoLock = false);

        /// <summary>
        ///     根据主值列表查询单条数据
        /// </summary>
        /// <param name="lstIds"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        List<T> QueryByIDs(object[] lstIds, bool blUseNoLock = false);

        /// <summary>
        ///     根据主值列表查询单条数据
        /// </summary>
        /// <param name="lstIds"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<List<T>> QueryByIDsAsync(object[] lstIds, bool blUseNoLock = false);

        /// <summary>
        ///     根据主值列表查询单条数据
        /// </summary>
        /// <param name="lstIds"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        List<T> QueryByIDs(int[] lstIds, bool blUseNoLock = false);

        /// <summary>
        ///     根据主值列表查询单条数据
        /// </summary>
        /// <param name="lstIds"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<List<T>> QueryByIDsAsync(int[] lstIds, bool blUseNoLock = false);

        /// <summary>
        ///     查询所有数据(无分页,请慎用)
        /// </summary>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        List<T> Query(bool blUseNoLock = false);

        /// <summary>
        ///     查询所有数据(无分页,请慎用)
        /// </summary>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<List<T>> QueryAsync(bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        List<T> QueryListByClause(string strWhere, string orderBy = "", bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        Task<List<T>> QueryListByClauseAsync(string strWhere, string orderBy = "", bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, string orderBy = "", bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, string orderBy = "",
            bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate,
            OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询一定数量数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="take">获取数量</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, int take,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询一定数量数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="take">获取数量</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, int take,
            Expression<Func<T, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询一定数量数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="take">获取数量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        List<T> QueryListByClause(Expression<Func<T, bool>> predicate, int take, string strOrderByFileds = "",
            bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询一定数量数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="take">获取数量</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<List<T>> QueryListByClauseAsync(Expression<Func<T, bool>> predicate, int take,
            string strOrderByFileds = "", bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        T QueryByClause(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <param name="blUseTranLock">是否使用事务锁</param>
        /// <param name="dbLockType">事务锁类型</param>
        /// <returns></returns>
        Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false,bool blUseTranLock = false,
            DbLockType dbLockType = DbLockType.Wait);

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        T QueryByClause(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate,
            OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<T> QueryByClauseAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByPredicate,
            OrderByType orderByType, bool blUseNoLock = false);

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        int Insert(T entity);

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<int> InsertAsync(T entity);

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        int Insert(T entity, Expression<Func<T, object>> insertColumns = null);

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<int> InsertAsync(T entity, Expression<Func<T, object>> insertColumns = null);

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        bool InsertGuid(T entity, Expression<Func<T, object>> insertColumns = null);

        /// <summary>
        ///     写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<bool> InsertGuidAsync(T entity, Expression<Func<T, object>> insertColumns = null);

        /// <summary>
        ///     批量写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        int Insert(List<T> entity);

        /// <summary>
        ///     批量写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<int> InsertAsync(List<T> entity);

        /// <summary>
        ///     批量写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<int> InsertCommandAsync(List<T> entity);

        /// <summary>
        ///     批量更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(List<T> entity);

        /// <summary>
        ///     批量更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(List<T> entity);

        /// <summary>
        ///     更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        ///     更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        ///     根据手写条件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        bool Update(T entity, string strWhere);

        /// <summary>
        ///     根据手写条件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity, string strWhere);

        /// <summary>
        ///     根据手写sql语句更新数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool Update(string strSql, SugarParameter[] parameters = null);

        /// <summary>
        ///     根据手写sql语句更新数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(string strSql, SugarParameter[] parameters = null);

        /// <summary>
        ///     更新某个字段
        /// </summary>
        /// <param name="columns">lamdba表达式,如it => new Student() { Name = "a", CreateTime = DateTime.Now }</param>
        /// <param name="where">lamdba判断</param>
        /// <returns></returns>
        bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where);

        /// <summary>
        ///     更新某个字段
        /// </summary>
        /// <param name="columns">lamdba表达式,如it => new Student() { Name = "a", CreateTime = DateTime.Now }</param>
        /// <param name="where">lamdba判断</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Expression<Func<T, T>> columns, Expression<Func<T, bool>> where);

        /// <summary>
        ///     根据条件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstColumns"></param>
        /// <param name="lstIgnoreColumns"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null,
            string strWhere = "");

        /// <summary>
        ///     根据条件更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstColumns"></param>
        /// <param name="lstIgnoreColumns"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        bool Update(T entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null,
            string strWhere = "");

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        bool Delete(T entity);

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="entity">实体类集合</param>
        /// <returns></returns>
        bool Delete(IEnumerable<T> entity);

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="entity">实体类集合</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(IEnumerable<T> entity);

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> where);

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Expression<Func<T, bool>> where);

        /// <summary>
        ///     删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteById(object id);

        /// <summary>
        ///     删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(object id);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(int[] ids);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(int[] ids);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(long[] ids);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(long[] ids);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(Guid[] ids);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(Guid[] ids);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(string[] ids);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(string[] ids);



        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(List<int> ids);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(List<int> ids);


        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(List<string> ids);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(List<string> ids);


        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(List<Guid> ids);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(List<Guid> ids);


        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(List<long> ids);

        /// <summary>
        ///     删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIdsAsync(List<long> ids);

        /// <summary>
        ///     判断数据是否存在
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        bool Exists(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);

        /// <summary>
        ///     判断数据是否存在
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);

        /// <summary>
        ///     获取数据总数
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        int GetCount(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);

        /// <summary>
        ///     获取数据总数
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate, bool blUseNoLock = false);

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        int GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field, bool blUseNoLock = false);

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<int> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> field,
            bool blUseNoLock = false);

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        decimal GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field,
            bool blUseNoLock = false);

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<decimal> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> field,
            bool blUseNoLock = false);

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        float GetSum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field, bool blUseNoLock = false);

        /// <summary>
        ///     获取数据某个字段的合计
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="field">字段</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<float> GetSumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> field,
            bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        IPageList<T> QueryPage(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        IPageList<T> QueryPage(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderByExpression,
            OrderByType orderByType, int pageIndex = 1, int pageSize = 20, bool blUseNoLock = false);

        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<IPageList<T>> QueryPageAsync(Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        /// <summary>
        ///     查询-多表查询
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        List<TResult> QueryMuch<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     查询-多表查询
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        Task<List<TResult>> QueryMuchAsync<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     查询-多表查询
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        TResult QueryMuchFirst<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     查询-多表查询
        /// </summary>
        /// <typeparam name="T1">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        Task<TResult> QueryMuchFirstAsync<T1, T2, TResult>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, TResult>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     查询-三表查询
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        List<TResult> QueryMuch<T1, T2, T3, TResult>(
            Expression<Func<T1, T2, T3, object[]>> joinExpression,
            Expression<Func<T1, T2, T3, TResult>> selectExpression,
            Expression<Func<T1, T2, T3, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     查询-三表查询
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体2</typeparam>
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param>
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>值</returns>
        Task<List<TResult>> QueryMuchAsync<T1, T2, T3, TResult>(
            Expression<Func<T1, T2, T3, object[]>> joinExpression,
            Expression<Func<T1, T2, T3, TResult>> selectExpression,
            Expression<Func<T1, T2, T3, bool>> whereLambda = null, bool blUseNoLock = false) where T1 : class, new();

        /// <summary>
        ///     执行sql语句并返回List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<T> SqlQuery(string sql, List<SugarParameter> parameters);

        /// <summary>
        ///     执行sql语句并返回List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<T>> SqlQueryable(string sql);
    }
}