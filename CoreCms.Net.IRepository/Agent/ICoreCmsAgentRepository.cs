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
using CoreCms.Net.Model.ViewModels.DTO.Agent;
using SqlSugar;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     代理商表 工厂接口
    /// </summary>
    public interface ICoreCmsAgentRepository : IBaseRepository<CoreCmsAgent>
    {
        /// <summary>
        ///     根据条件查询订单分页数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        Task<IPageList<CoreCmsAgentOrder>> QueryOrderPageAsync(int userId, int pageIndex = 1, int pageSize = 20,
            int typeId = 0);


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
        new Task<IPageList<CoreCmsAgent>> QueryPageAsync(
            Expression<Func<CoreCmsAgent, bool>> predicate,
            Expression<Func<CoreCmsAgent, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);


        /// <summary>
        ///     获取代理商排行
        /// </summary>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        Task<IPageList<AgentRankingDTO>> QueryRankingPageAsync(int pageIndex = 1, int pageSize = 20);
    }
}