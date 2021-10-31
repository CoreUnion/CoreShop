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
    ///     订单表 服务工厂接口
    /// </summary>
    public interface ICoreCmsOrderServices : IBaseServices<CoreCmsOrder>
    {
        /// <summary>
        ///     查询团购秒杀下单数量
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        FindLimitOrderDto FindLimitOrder(int productId, int userId, DateTime? startTime, DateTime? endTime,
            int orderType = 0);


        /// <summary>
        ///     获取税号
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetTaxCode(string name);


        /// <summary>
        ///     创建订单
        /// </summary>
        /// <param name="userId">用户序列</param>
        /// <param name="orderType">订单类型，1是普通订单，2是拼团订单</param>
        /// <param name="cartIds">购物车货品序列</param>
        /// <param name="receiptType">收货方式,1快递物流，2同城配送，3门店自提</param>
        /// <param name="ushipId">用户地址库序列</param>
        /// <param name="storeId">门店序列</param>
        /// <param name="ladingName">提货人姓名</param>
        /// <param name="ladingMobile">提货人联系方式</param>
        /// <param name="memo">备注</param>
        /// <param name="point">积分</param>
        /// <param name="couponCode">优惠券码</param>
        /// <param name="source">来源平台</param>
        /// <param name="scene">场景值（一般小程序才有）</param>
        /// <param name="taxType">发票信息</param>
        /// <param name="taxName">发票抬头</param>
        /// <param name="taxCode">发票税务编码</param>
        /// <param name="objectId">关联非普通订单营销功能的序列</param>
        /// <param name="teamId">拼团订单分组序列</param>
        /// <returns></returns>
        Task<WebApiCallBack> ToAdd(int userId, int orderType, string cartIds, int receiptType, int ushipId, int storeId,
            string ladingName, string ladingMobile, string memo, int point, string couponCode,
            int source, int scene, int taxType, string taxName, string taxCode, int objectId, int teamId);

        /// <summary>
        ///     获取订单信息
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetOrderInfoByOrderId(string id, int userId = 0, int aftersaleLevel = 0);

        /// <summary>
        ///     获取订单不同状态的数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ids"></param>
        /// <param name="isAfterSale"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetOrderStatusNum(int userId, int[] ids, bool isAfterSale = false);

        /// <summary>
        ///     订单数量统计
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> OrderCount(int type = 0, int userId = 0);

        /// <summary>
        ///     获取订单状态反查
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns></returns>
        Expression<Func<CoreCmsOrder, bool>> GetReverseStatus(int status);

        /// <summary>
        ///     获取订单列表微信小程序
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetOrderList(int status = -1, int userId = 0, int page = 1, int limit = 5);


        /// <summary>
        ///     商家获取订单列表
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetOrderPageByMerchant(string dateType, string[] date, int status = 0, int storeId = 0,
            int page = 1, int limit = 5);

        /// <summary>
        ///     根据搜索条件商家获取订单列表
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetOrderPageByMerchantSearch(string keyword, int status = 0, int receiptType = 0, int storeId = 0,
            int page = 1, int limit = 5);


        /// <summary>
        ///     订单支付
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="paymentCode">支付方式</param>
        /// <param name="billPaymentInfo">支付单据</param>
        /// <returns></returns>
        Task<WebApiCallBack> Pay(string orderId, string paymentCode, CoreCmsBillPayments billPaymentInfo);


        /// <summary>
        ///     取消订单
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> CancelOrder(string[] ids, int userId = 0);


        /// <summary>
        ///     后端根据订单状态生成不同的操作按钮
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="payStatus">支付状态</param>
        /// <param name="shipStatus">发货状态</param>
        /// <param name="receiptType">收货方式</param>
        /// <param name="isDel">是否删除</param>
        /// <returns></returns>
        string GetOperating(string orderId, int orderStatus, int payStatus, int shipStatus, int receiptType, bool isDel);


        /// <summary>
        ///     构建需要发货的数据，和发货单密切关联
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetOrderShipInfo(string[] ids);

        /// <summary>
        /// 构建单个需要发货的数据，和发货单密切关联
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetOrderShipInfo(string orderId);

        /// <summary>
        ///     发货改状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<WebApiCallBack> EditShipStatus(string orderId, Dictionary<int, int> items);


        /// <summary>
        ///     批量订单发货
        /// </summary>
        /// <param name="ids">订单标号</param>
        /// <param name="logiCode">物流公司编码</param>
        /// <param name="logiNo">物流单号</param>
        /// <param name="items">发货明细</param>
        /// <param name="shipName">收货人姓名</param>
        /// <param name="shipMobile">收货人电话</param>
        /// <param name="shipAddress">收货地址</param>
        /// <param name="memo">发货描述</param>
        /// <param name="storeId">店铺收货地址</param>
        /// <param name="shipAreaId">省市区id</param>
        /// <param name="deliveryCompanyId">直播物流编码</param>
        /// <returns></returns>
        Task<WebApiCallBack> BatchShip(string[] ids, string logiCode, string logiNo,
            Dictionary<int, int> items, string shipName, string shipMobile, string shipAddress, string memo,
            int storeId = 0, int shipAreaId = 0, string deliveryCompanyId = "");

        /// <summary>
        ///     订单发货
        /// </summary>
        /// <param name="ids">订单标号</param>
        /// <param name="logiCode">物流公司编码</param>
        /// <param name="logiNo">物流单号</param>
        /// <param name="items">发货明细</param>
        /// <param name="shipName">收货人姓名</param>
        /// <param name="shipMobile">收货人电话</param>
        /// <param name="shipAddress">收货地址</param>
        /// <param name="memo">发货描述</param>
        /// <param name="storeId">店铺收货地址</param>
        /// <param name="shipAreaId">省市区id</param>
        /// <param name="deliveryCompanyId">直播物流编码</param>
        /// <returns></returns>
        Task<WebApiCallBack> Ship(string ids, string logiCode, string logiNo,
            Dictionary<int, int> items, string shipName, string shipMobile, string shipAddress, string memo,
            int storeId = 0, int shipAreaId = 0, string deliveryCompanyId = "");


        /// <summary>
        ///     后台完成订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="score">有序队列积分</param>
        /// <param name="remark"></param>
        /// <returns></returns>
        Task<WebApiCallBack> CompleteOrder(string orderId, int score = 0, string remark = "后台订单完成操作");

        /// <summary>
        ///     确认签收订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ConfirmOrder(string orderId, int userId = 0);


        /// <summary>
        ///     判断订单是否可以进行评论
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> IsOrderComment(string orderId, int userId);


        /// <summary>
        ///     自动取消订单（定时任务使用）
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> AutoCancelOrder();


        /// <summary>
        ///     自动完成订单（定时任务使用）
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> AutoCompleteOrder();

        /// <summary>
        ///     自动评价订单（定时任务使用）
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> AutoEvaluateOrder();


        /// <summary>
        ///     自动签收订单（定时任务使用）
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> AutoSignOrder();


        /// <summary>
        ///     催付款（定时任务使用）
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> RemindOrderPay();



        /// <summary>
        ///     重写根据条件列表数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        Task<List<CoreCmsOrder>> QueryListAsync(Expression<Func<CoreCmsOrder, bool>> predicate,
            Expression<Func<CoreCmsOrder, object>> orderByExpression, OrderByType orderByType);


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
        new Task<IPageList<CoreCmsOrder>> QueryPageAsync(
            Expression<Func<CoreCmsOrder, bool>> predicate,
            Expression<Func<CoreCmsOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);
    }
}