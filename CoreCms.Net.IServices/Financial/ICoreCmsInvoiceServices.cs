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
    ///     发票表 服务工厂接口
    /// </summary>
    public interface ICoreCmsInvoiceServices : IBaseServices<CoreCmsInvoice>
    {
        /// <summary>
        ///     获取订单的发票信息
        /// </summary>
        /// <param name="orderId">订单字符串编号</param>
        /// <returns></returns>
        Task<WebApiCallBack> GetOrderInvoiceInfo(string orderId);


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
        new Task<IPageList<CoreCmsInvoice>> QueryPageAsync(
            Expression<Func<CoreCmsInvoice, bool>> predicate,
            Expression<Func<CoreCmsInvoice, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        #endregion
    }
}