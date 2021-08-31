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
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     订单表
    /// </summary>
    public partial class CoreCmsOrder
    {
        /// <summary>
        ///     订单详情
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsOrderItem> items { get; set; }

        /// <summary>
        ///     用户信息
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public CoreCmsUser user { get; set; }

        /// <summary>
        ///     支付单关系
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsBillPayments> paymentItem { get; set; }

        /// <summary>
        ///     退款单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsBillRefund> refundItem { get; set; }

        /// <summary>
        ///     提货单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsBillLading> ladingItem { get; set; }

        /// <summary>
        ///     退货单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsBillReship> returnItem { get; set; }

        /// <summary>
        ///     售后单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsBillAftersales> aftersalesItem { get; set; }

        /// <summary>
        ///     发货单
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsBillDelivery> delivery { get; set; }

        /// <summary>
        ///     门店
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public CoreCmsStore store { get; set; }

        /// <summary>
        ///     配送方式
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public CoreCmsShip logistics { get; set; }

        /// <summary>
        ///     获取订单全局状态
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int globalStatus { get; set; } = 0;

        /// <summary>
        ///     获取订单全局状态描述
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string globalStatusText { get; set; }

        /// <summary>
        ///     收货地区三级地址
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string shipAreaName { get; set; }


        /// <summary>
        ///     支付方式中文描述
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string paymentName { get; set; }

        /// <summary>
        ///     优惠券列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsCoupon> couponObj { get; set; } = new();

        /// <summary>
        ///     促销信息
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public object promotionObj { get; set; }

        /// <summary>
        ///     倒计时标准时间
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public DateTime? remainingTime { get; set; }

        /// <summary>
        ///     倒计时文字说明
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string remaining { get; set; }


        /// <summary>
        ///     发票信息
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public object invoice { get; set; }

        /// <summary>
        ///     售后单号
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string billAftersalesId { get; set; }

        /// <summary>
        ///     已经退过款的金额
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public decimal refunded { get; set; } = 0;

        /// <summary>
        ///     是否能发起售后
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool addAftersalesStatus { get; set; } = false;


        /// <summary>
        ///     操作日志
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsOrderLog> orderLog { get; set; }


        /// <summary>
        ///     状态说明
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string statusText { get; set; }

        /// <summary>
        ///     支付状态说明
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string payStatusText { get; set; }

        /// <summary>
        ///     发货状态说明
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string shipStatusText { get; set; }

        /// <summary>
        ///     来源状态说明
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string sourceText { get; set; }

        /// <summary>
        ///     订单类型状态说明
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string typeText { get; set; }

        /// <summary>
        ///     发票类型
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string taxTypeText { get; set; }

        /// <summary>
        ///     支付方式说明
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string paymentCodeText { get; set; }

        /// <summary>
        ///     确认收货状态说明
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string confirmStatusText { get; set; }

        /// <summary>
        ///     操作码
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string operating { get; set; }

        /// <summary>
        ///     售后情况
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string afterSaleStatus { get; set; }

        /// <summary>
        ///     用户昵称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string userNickName { get; set; }
    }
}