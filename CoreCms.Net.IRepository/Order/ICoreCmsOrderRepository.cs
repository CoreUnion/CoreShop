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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.DTO;
using SqlSugar;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     订单表 工厂接口
    /// </summary>
    public interface ICoreCmsOrderRepository : IBaseRepository<CoreCmsOrder>
    {
        /// <summary>
        ///     查询团购秒杀下单数量
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        FindLimitOrderDto FindLimitOrder(int productId, int userId, DateTime? startTime, DateTime? endTime,
            int orderType = 0);


        /// <summary>
        ///     根据用户id和商品id获取下了多少订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodId"></param>
        /// <returns></returns>
        int GetOrderNum(int userId, int goodId);

        /// <summary>
        ///     重写根据条件列表数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        Task<List<CoreCmsOrder>> QueryListAsync(Expression<Func<CoreCmsOrder, bool>> predicate,
            Expression<Func<CoreCmsOrder, object>> orderByExpression, OrderByType orderByType);

        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        new Task<IPageList<CoreCmsOrder>> QueryPageAsync(
            Expression<Func<CoreCmsOrder, bool>> predicate,
            Expression<Func<CoreCmsOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);


        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<IPageList<CoreCmsOrder>> QueryPageNewAsync(Expression<Func<CoreCmsOrder, bool>> predicate,
            Expression<Func<CoreCmsOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);
    }
}