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
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using SqlSugar;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     商品评价表 服务工厂接口
    /// </summary>
    public interface ICoreCmsGoodsCommentServices : IBaseServices<CoreCmsGoodsComment>
    {
        /// <summary>
        ///     添加一条评论
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="items">评价数据</param>
        /// <param name="userId">用户序列</param>
        /// <returns></returns>
        Task<WebApiCallBack> AddComment(string orderId, List<OrderEvaluatePostItems> items, int userId);

        /// <summary>
        ///     商家回复评价
        /// </summary>
        /// <param name="id">序列</param>
        /// <param name="sellerContent">回复内容</param>
        /// <returns></returns>
        Task<AdminUiCallBack> Reply(int id, string sellerContent);

        /// <summary>
        ///     获取单个详情数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        Task<CoreCmsGoodsComment> DetailsByIdAsync(Expression<Func<CoreCmsGoodsComment, bool>> predicate,
            Expression<Func<CoreCmsGoodsComment, object>> orderByExpression, OrderByType orderByType);


        #region 重写根据条件查询分页数据

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
        new Task<IPageList<CoreCmsGoodsComment>> QueryPageAsync(
            Expression<Func<CoreCmsGoodsComment, bool>> predicate,
            Expression<Func<CoreCmsGoodsComment, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        #endregion

        #region 重写增删改查操作===========================================================

        /// <summary>
        ///     重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> InsertAsync(CoreCmsGoodsComment entity);

        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(CoreCmsGoodsComment entity);

        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(List<CoreCmsGoodsComment> entity);

        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> DeleteByIdAsync(object id);

        /// <summary>
        ///     重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids);

        #endregion
    }
}