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
    ///     商品收藏表 工厂接口
    /// </summary>
    public interface ICoreCmsGoodsCollectionRepository : IBaseRepository<CoreCmsGoodsCollection>
    {
        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        Task<IPageList<CoreCmsGoodsCollection>> QueryPageAsync(
            Expression<Func<CoreCmsGoodsCollection, bool>> predicate,
            Expression<Func<CoreCmsGoodsCollection, object>> orderByExpression, OrderByType orderByType,
            int pageIndex = 1,
            int pageSize = 20);


        /// <summary>
        ///     收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ToAdd(int userId, int goodsId);
    }
}