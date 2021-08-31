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
using CoreCms.Net.Model.ViewModels.DTO.Agent;
using SqlSugar;

namespace CoreCms.Net.IServices
{
    /// <summary>
    /// 代理商表 服务工厂接口
    /// </summary>
    public interface ICoreCmsAgentServices : IBaseServices<CoreCmsAgent>
    {

        /// <summary>
        /// 获取代理商信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<WebApiCallBack> GetInfo(int userId);

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="iData"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> AddData(CoreCmsAgent iData, int userId);


        /// <summary>
        /// 获取我的代理订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetMyOrderList(int userId, int page, int limit = 10, int typeId = 0);

        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetStore(int userId);

        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        Task<IPageList<CoreCmsAgentOrder>> QueryOrderPageAsync(int userId, int pageIndex = 1, int pageSize = 20);


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
        new Task<IPageList<CoreCmsAgent>> QueryPageAsync(
            Expression<Func<CoreCmsAgent, bool>> predicate,
            Expression<Func<CoreCmsAgent, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);
        #endregion


        /// <summary>
        ///     获取代理商排行
        /// </summary>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        Task<IPageList<AgentRankingDTO>> QueryRankingPageAsync(int pageIndex = 1, int pageSize = 20);

    }
}
