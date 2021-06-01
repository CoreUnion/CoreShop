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
    ///     退款单表 服务工厂接口
    /// </summary>
    public interface ICoreCmsBillRefundServices : IBaseServices<CoreCmsBillRefund>
    {
        /// <summary>
        ///     创建退款单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sourceId"></param>
        /// <param name="type">1订单,2充值,5服务订单</param>
        /// <param name="money"></param>
        /// <param name="aftersalesId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ToAdd(int userId, string sourceId, int type, decimal money, string aftersalesId);


        /// <summary>
        ///     退款单去退款或者拒绝
        /// </summary>
        /// <param name="refundId">退款单id</param>
        /// <param name="status">2或者4，通过或者拒绝</param>
        /// <param name="paymentCodeStr">退款方式，如果和退款单上的一样，说明没有修改，原路返回，否则只记录状态，不做实际退款,如果为空是原路返回</param>
        /// <returns></returns>
        Task<WebApiCallBack> ToRefund(string refundId, int status, string paymentCodeStr = "");


        /// <summary>
        ///     如果是在线支付的原路退还，去做退款操作
        /// </summary>
        Task<WebApiCallBack> PaymentRefund(string refundId);


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
        new Task<IPageList<CoreCmsBillRefund>> QueryPageAsync(
            Expression<Func<CoreCmsBillRefund, bool>> predicate,
            Expression<Func<CoreCmsBillRefund, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        #endregion
    }
}