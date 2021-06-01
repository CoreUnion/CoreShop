using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     优惠券码表 工厂接口
    /// </summary>
    public interface ICoreCmsCouponRepository : IBaseRepository<CoreCmsCoupon>
    {
        /// <summary>
        ///     根据优惠券编码取优惠券的信息,并判断是否可用
        /// </summary>
        /// <param name="code"></param>
        /// <param name="check"></param>
        Task<WebApiCallBack> ToInfo(string[] code, bool check = false);


        /// <summary>
        ///     获取 我的优惠券
        /// </summary>
        /// <param name="userId">用户序列</param>
        /// <param name="promotionId">促销序列</param>
        /// <param name="display">优惠券状态编码</param>
        /// <param name="page">页码</param>
        /// <param name="limit">数量</param>
        Task<WebApiCallBack> GetMyCoupon(int userId, int promotionId = 0, string display = "all", int page = 1,
            int limit = 10);


        /// <summary>
        ///     根据条件查询分页数据及导航数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="isToPage">是否分页</param>
        /// <returns></returns>
        Task<IPageList<CoreCmsCoupon>> QueryPageMapperAsync(
            Expression<Func<CoreCmsCoupon, bool>> predicate,
            Expression<Func<CoreCmsCoupon, object>> orderByExpression, OrderByType orderByType, bool isToPage = false,
            int pageIndex = 1,
            int pageSize = 20);


        /// <summary>
        ///     重写数据并获取相关
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        Task<List<CoreCmsCoupon>> QueryWithAboutAsync(Expression<Func<CoreCmsCoupon, bool>> predicate);


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
        new Task<IPageList<CoreCmsCoupon>> QueryPageAsync(
            Expression<Func<CoreCmsCoupon, bool>> predicate,
            Expression<Func<CoreCmsCoupon, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);


        /// <summary>
        ///     获取 我的优惠券可用数量
        /// </summary>
        /// <param name="userId">用户序列</param>
        Task<int> GetMyCouponCount(int userId);
    }
}