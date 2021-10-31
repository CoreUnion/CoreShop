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
using CoreCms.Net.Model.Entities;
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    /// <summary>
    ///     查询团购秒杀下单数量返回实体
    /// </summary>
    public class FindLimitOrderDto
    {
        /// <summary>
        ///     总单数
        /// </summary>
        public int TotalOrders { get; set; } = 0;

        /// <summary>
        ///     用户总单数
        /// </summary>
        public int TotalUserOrders { get; set; } = 0;
    }

    /// <summary>
    ///     发票模糊查询提交实体
    /// </summary>
    public class GetTaxCodePost
    {
        /// <summary>
        ///     发票抬头
        /// </summary>
        public string name { get; set; }
    }

    /// <summary>
    ///     创建订单提交参数
    /// </summary>
    public class CreateOrder
    {
        /// <summary>
        ///     区域序列
        /// </summary>
        public int areaId { get; set; }

        /// <summary>
        ///     购物车货品数据
        /// </summary>
        public string cartIds { get; set; }

        /// <summary>
        ///     优惠券码
        /// </summary>
        public string couponCode { get; set; }

        /// <summary>
        ///     买家留言
        /// </summary>
        public string memo { get; set; }

        /// <summary>
        ///     积分
        /// </summary>
        public int point { get; set; } = 0;

        /// <summary>
        ///     收货方式,1快递物流，2同城配送，3门店自提
        /// </summary>
        public int receiptType { get; set; } = 1;

        /// <summary>
        ///     来源平台
        /// </summary>
        public int source { get; set; } = 2;

        /// <summary>
        ///     发票税务编号
        /// </summary>
        public string taxCode { get; set; }

        /// <summary>
        ///     发票抬头
        /// </summary>
        public string taxName { get; set; }

        /// <summary>
        ///     发票类型
        /// </summary>
        public int taxType { get; set; } = 1;

        /// <summary>
        ///     用户地址库序列
        /// </summary>
        public int ushipId { get; set; } = 0;

        /// <summary>
        ///     门店序列
        /// </summary>
        public int storeId { get; set; } = 0;

        /// <summary>
        ///     订单类型，1是普通订单，2是拼团订单
        /// </summary>
        public int orderType { get; set; } = 1;

        /// <summary>
        ///     提货人姓名
        /// </summary>
        public string ladingName { get; set; }

        /// <summary>
        ///     提货人联系方式
        /// </summary>
        public string ladingMobile { get; set; }

        /// <summary>
        ///     非普通订单关联营销对象序列
        /// </summary>
        public int objectId { get; set; } = 0;

        /// <summary>
        ///     拼团订单分组序列
        /// </summary>
        public int teamId { get; set; } = 0;

        /// <summary>
        ///     场景值
        /// </summary>
        public int scene { get; set; } = 0;
    }

    /// <summary>
    ///     支付确认页面取信息提交参数集合
    /// </summary>
    public class CheckPayPost
    {
        /// <summary>
        ///     订单号集合
        /// </summary>
        public string ids { get; set; }

        /// <summary>
        ///     //支付的时候，有一些特殊的参数需要传递到支付里面，这里就是干这个事情的,key=>value格式的一维数组
        /// </summary>
        public JObject @params { get; set; } = null;

        /// <summary>
        ///     付款方式
        /// </summary>
        public int paymentType { get; set; }
    }

    /// <summary>
    ///     获取订单不同状态的数量提交参数
    /// </summary>
    public class GetOrderStatusNumPost
    {
        /// <summary>
        ///     类型集合
        /// </summary>
        public string ids { get; set; }

        /// <summary>
        ///     是否进行售后
        /// </summary>
        public bool isAfterSale { get; set; }
    }

    /// <summary>
    ///     获取订单列表提交参数
    /// </summary>
    public class GetOrderListPost
    {
        /// <summary>
        ///     每页数量
        /// </summary>
        public int limit { get; set; } = 5;

        /// <summary>
        ///     页码
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        ///     状态
        /// </summary>
        public int status { get; set; } = 0;
    }

    /// <summary>
    ///     获取订单列表提交参数
    /// </summary>
    public class GetOrderPageByMerchantPost
    {
        /// <summary>
        ///     日期类型
        /// </summary>
        public string dateType { get; set; }

        /// <summary>
        ///     自定义日期
        /// </summary>
        public string[] date { get; set; }

        /// <summary>
        ///     每页数量
        /// </summary>
        public int limit { get; set; } = 5;

        /// <summary>
        ///     页码
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        ///     状态
        /// </summary>
        public int status { get; set; } = 0;

        /// <summary>
        ///     收货类型
        /// </summary>
        public int receiptType { get; set; } = 0;


        /// <summary>
        ///     门店序列
        /// </summary>
        public int storeId { get; set; } = 0;
    }


    /// <summary>
    ///     搜索获取订单列表提交参数
    /// </summary>
    public class GetOrderPageByMerchantSearcgPost
    {
        /// <summary>
        ///     查询关键词
        /// </summary>
        public string keyword { get; set; }

        /// <summary>
        ///     每页数量
        /// </summary>
        public int limit { get; set; } = 5;

        /// <summary>
        ///     页码
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        ///     状态
        /// </summary>
        public int status { get; set; } = 0;

        /// <summary>
        ///     收货类型
        /// </summary>
        public int receiptType { get; set; } = 0;

        /// <summary>
        ///     门店序列
        /// </summary>
        public int storeId { get; set; } = 0;
    }


    /// <summary>
    ///     后端订单管理列表返回实体dto
    /// </summary>
    public class OrderListUIResult
    {
        /// <summary>
        ///     订单编号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        ///     订单状态
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        ///     用户用户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     收货人手机
        /// </summary>
        public string ShipMobile { get; set; }

        /// <summary>
        ///     操作html代码
        /// </summary>
        public string Operating { get; set; }

        /// <summary>
        ///     收货区域
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        ///     支付状态说明
        /// </summary>
        public string PayStatus { get; set; }

        /// <summary>
        ///     发货状态说明
        /// </summary>
        public string ShipStatus { get; set; }

        /// <summary>
        ///     订单来源说明
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        ///     订单类型说明(团购,普通)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     订单售后状态
        /// </summary>
        public string AfterSaleStatus { get; set; }

        /// <summary>
        ///     获取订单打印状态
        /// </summary>
        public bool Print { get; set; }

        /// <summary>
        ///     订单号(备注醒目)
        /// </summary>
        public string OrderIdK { get; set; }

        /// <summary>
        ///     订单金额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        ///     支付方式
        /// </summary>
        public string PaymentCode { get; set; }

        /// <summary>
        ///     数据创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }


    /// <summary>
    ///     后端订单发货返回集合实体
    /// </summary>
    public class AdminOrderShipResult
    {
        public string[] orderId { get; set; }
        public decimal weight { get; set; } = 0;
        public List<string> memo { get; set; }
        public decimal costFreight { get; set; } = 0;
        public int storeId { get; set; } = 0;
        public int shipAreaId { get; set; } = 0;
        public string shipAddress { get; set; }
        public string shipName { get; set; }
        public string shipMobile { get; set; }
        public int logisticsId { get; set; } = 0;
        public CoreCmsShip ship { get; set; } = null;
        public string logisticsName { get; set; }
        public List<CoreCmsOrderItem> items { get; set; }
        public List<CoreCmsOrder> orders { get; set; }
    }

    /// <summary>
    ///     后端订单发货返回单个实体
    /// </summary>
    public class AdminOrderShipOneResult
    {
        public string orderId { get; set; }
        public decimal weight { get; set; } = 0;
        public string memo { get; set; }
        public decimal costFreight { get; set; } = 0;
        public int storeId { get; set; } = 0;
        public int shipAreaId { get; set; } = 0;
        public string shipAddress { get; set; }
        public string shipName { get; set; }
        public string shipMobile { get; set; }
        public int logisticsId { get; set; } = 0;
        public CoreCmsShip ship { get; set; } = null;
        public string logisticsName { get; set; }
        public List<CoreCmsOrderItem> items { get; set; }
        public CoreCmsOrder orderInfo { get; set; }
    }


    /// <summary>
    ///     前端提交售后单提交参数
    /// </summary>
    public class ToAddBillAfterSalesPost
    {
        /// <summary>
        ///     订单编号
        /// </summary>
        public string orderId { get; set; }

        /// <summary>
        ///     是否收到退货，1未收到退货，不会创建退货单，2收到退货，会创建退货单,只有未发货的商品才能选择未收到货，只有已发货的才能选择已收到货
        /// </summary>
        public int type { get; set; } = 0;

        /// <summary>
        ///     如果是退款退货，退货的明细 以 [[order_item_id=>nums]]的二维数组形式传值
        /// </summary>
        public JArray items { get; set; }

        /// <summary>
        ///     上传图集
        /// </summary>
        public string[] images { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string reason { get; set; }

        /// <summary>
        ///     金额
        /// </summary>
        public decimal refund { get; set; } = 0;
    }
}