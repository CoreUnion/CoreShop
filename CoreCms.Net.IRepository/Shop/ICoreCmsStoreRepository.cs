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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     门店表 工厂接口
    /// </summary>
    public interface ICoreCmsStoreRepository : IBaseRepository<CoreCmsStore>
    {
        /// <summary>
        ///     事务重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> InsertAsync(CoreCmsStore entity);


        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(CoreCmsStore entity);


        /// <summary>
        ///     Sql根据条件查询分页数据带距离
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="latitude">纬度</param>
        /// <param name="longitude">精度</param>
        /// <returns></returns>
        Task<IPageList<CoreCmsStore>> QueryPageAsyncByCoordinate(
            Expression<Func<CoreCmsStore, bool>> predicate,
            Expression<Func<CoreCmsStore, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, decimal latitude = 0, decimal longitude = 0);

        /// <summary>
        ///     根据用户序列获取单个门店数据
        /// </summary>
        /// <param name="userId">用户序列</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<CoreCmsStore> GetStoreByUserId(int userId, bool blUseNoLock = false);
    }
}