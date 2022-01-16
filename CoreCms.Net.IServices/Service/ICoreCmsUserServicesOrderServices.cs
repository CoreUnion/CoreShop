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

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     服务购买表 服务工厂接口
    /// </summary>
    public interface ICoreCmsUserServicesOrderServices : IBaseServices<CoreCmsUserServicesOrder>
    {
        /// <summary>
        ///     完成服务订单后生成兑换券
        /// </summary>
        /// <param name="serviceOrderId"></param>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> CreateUserServicesTickets(string serviceOrderId, string paymentId);



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
        new Task<IPageList<CoreCmsUserServicesOrder>> QueryPageAsync(
            Expression<Func<CoreCmsUserServicesOrder, bool>> predicate,
            Expression<Func<CoreCmsUserServicesOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

    }
}