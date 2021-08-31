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
using Newtonsoft.Json.Linq;
using SqlSugar;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     支付单表 服务工厂接口
    /// </summary>
    public interface ICoreCmsBillPaymentsServices : IBaseServices<CoreCmsBillPayments>
    {
        /// <summary>
        ///     单个生成支付单的时候，格式化支付单明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="type"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        Task<WebApiCallBack> FormatPaymentRel(string orderId, int type, JObject @params);


        /// <summary>
        ///     批量生成支付单的时候，格式化支付单明细
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <param name="type"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        Task<WebApiCallBack> BatchFormatPaymentRel(string[] sourceStr, int type, JObject @params);


        /// <summary>
        ///     支付，先生成支付单，然后去支付
        /// </summary>
        /// <param name="sourceStr">来源，一般是订单号或者用户id，比如充值</param>
        /// <param name="paymentCode">支付方式</param>
        /// <param name="userId">用户序列</param>
        /// <param name="type">订单/充值/服务项目</param>
        /// <param name="params">支付的时候用到的参数，如果是微信支付的话，这里可以传trade_type=>'JSAPI'(小程序支付),或者'MWEB'(h5支付),当是JSPI的时候，可以不传其他参数了，默认就可以，默认的这个值就是JSAPI，如果是MWEB的话，需要传wap_url(网站url地址)参数和wap_name（网站名称）参数，其他支付方式需要传什么参数这个以后再说</param>
        /// <returns></returns>
        Task<WebApiCallBack> Pay(string sourceStr, string paymentCode, int userId, int type, JObject @params);

        /// <summary>
        ///     获取支付单详情
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetInfo(string paymentId, int userId = 0);

        /// <summary>
        ///     支付成功后，更新支付单状态
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="paymentCode"></param>
        /// <param name="money"></param>
        /// <param name="status"></param>
        /// <param name="payedMsg"></param>
        /// <param name="tradeNo"></param>
        Task<WebApiCallBack> ToUpdate(string paymentId, int status, string paymentCode, decimal money,
            string payedMsg = "", string tradeNo = "");

        /// <summary>
        ///     卖家直接支付操作
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="type">支付类型</param>
        /// <param name="paymentCode">支付类型编码</param>
        /// <returns></returns>
        Task<WebApiCallBack> ToPay(string orderId, int type, string paymentCode);

        /// <summary>
        ///     支付单7天统计
        /// </summary>
        /// <returns></returns>
        Task<List<StatisticsOut>> Statistics();


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
        new Task<IPageList<CoreCmsBillPayments>> QueryPageAsync(
            Expression<Func<CoreCmsBillPayments, bool>> predicate,
            Expression<Func<CoreCmsBillPayments, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        #endregion
    }
}