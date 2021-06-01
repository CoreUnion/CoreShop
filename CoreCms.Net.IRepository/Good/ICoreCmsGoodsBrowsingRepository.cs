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
using SqlSugar;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     商品浏览记录表 工厂接口
    /// </summary>
    public interface ICoreCmsGoodsBrowsingRepository : IBaseRepository<CoreCmsGoodsBrowsing>
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
        Task<IPageList<CoreCmsGoodsBrowsing>> QueryPageAsync(Expression<Func<CoreCmsGoodsBrowsing, bool>> predicate,
            Expression<Func<CoreCmsGoodsBrowsing, object>> orderByExpression, OrderByType orderByType,
            int pageIndex = 1,
            int pageSize = 20);
    }
}