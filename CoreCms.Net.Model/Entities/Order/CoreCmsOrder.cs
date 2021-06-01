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
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     订单表
    /// </summary>
    public partial class CoreCmsOrder
    {
        /// <summary>
        ///     订单号
        /// </summary>
        [Display(Name = "订单号")]
        [SugarColumn(IsPrimaryKey = true)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string orderId { get; set; }

        /// <summary>
        ///     商品总价
        /// </summary>
        [Display(Name = "商品总价")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal goodsAmount { get; set; }

        /// <summary>
        ///     已支付的金额
        /// </summary>
        [Display(Name = "已支付的金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal payedAmount { get; set; }

        /// <summary>
        ///     订单实际销售总额
        /// </summary>
        [Display(Name = "订单实际销售总额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal orderAmount { get; set; }

        /// <summary>
        ///     支付状态
        /// </summary>
        [Display(Name = "支付状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int payStatus { get; set; }

        /// <summary>
        ///     发货状态
        /// </summary>
        [Display(Name = "发货状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int shipStatus { get; set; }

        /// <summary>
        ///     订单状态
        /// </summary>
        [Display(Name = "订单状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int status { get; set; }

        /// <summary>
        ///     订单类型
        /// </summary>
        [Display(Name = "订单类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int orderType { get; set; }

        /// <summary>
        ///     收货方式
        /// </summary>
        [Display(Name = "收货方式")]
        [Required(ErrorMessage = "请输入{0}")]
        public int receiptType { get; set; }

        /// <summary>
        ///     支付方式代码
        /// </summary>
        [Display(Name = "支付方式代码")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string paymentCode { get; set; }

        /// <summary>
        ///     支付时间
        /// </summary>
        [Display(Name = "支付时间")]
        public DateTime? paymentTime { get; set; }

        /// <summary>
        ///     配送方式ID 关联ship.id
        /// </summary>
        [Display(Name = "配送方式ID 关联ship.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int logisticsId { get; set; }

        /// <summary>
        ///     配送方式名称
        /// </summary>
        [Display(Name = "配送方式名称")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string logisticsName { get; set; }

        /// <summary>
        ///     配送费用
        /// </summary>
        [Display(Name = "配送费用")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal costFreight { get; set; }

        /// <summary>
        ///     用户ID 关联user.id
        /// </summary>
        [Display(Name = "用户ID 关联user.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     店铺ID 关联seller.id
        /// </summary>
        [Display(Name = "店铺ID 关联seller.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sellerId { get; set; }

        /// <summary>
        ///     售后状态
        /// </summary>
        [Display(Name = "售后状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int confirmStatus { get; set; }

        /// <summary>
        ///     确认收货时间
        /// </summary>
        [Display(Name = "确认收货时间")]
        public DateTime? confirmTime { get; set; }

        /// <summary>
        ///     自提门店ID，0就是不门店自提
        /// </summary>
        [Display(Name = "自提门店ID，0就是不门店自提")]
        [Required(ErrorMessage = "请输入{0}")]
        public int storeId { get; set; }

        /// <summary>
        ///     收货地区ID
        /// </summary>
        [Display(Name = "收货地区ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public int shipAreaId { get; set; }

        /// <summary>
        ///     收货详细地址
        /// </summary>
        [Display(Name = "收货详细地址")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string shipAddress { get; set; }

        /// <summary>
        ///     收货人姓名
        /// </summary>
        [Display(Name = "收货人姓名")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string shipName { get; set; }

        /// <summary>
        ///     收货电话
        /// </summary>
        [Display(Name = "收货电话")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string shipMobile { get; set; }

        /// <summary>
        ///     商品总重量
        /// </summary>
        [Display(Name = "商品总重量")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal weight { get; set; }

        /// <summary>
        ///     开发票
        /// </summary>
        [Display(Name = "开发票")]
        [Required(ErrorMessage = "请输入{0}")]
        public int taxType { get; set; }

        /// <summary>
        ///     税号
        /// </summary>
        [Display(Name = "税号")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string taxCode { get; set; }

        /// <summary>
        ///     发票抬头
        /// </summary>
        [Display(Name = "发票抬头")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string taxTitle { get; set; }

        /// <summary>
        ///     使用积分
        /// </summary>
        [Display(Name = "使用积分")]
        [Required(ErrorMessage = "请输入{0}")]
        public int point { get; set; }

        /// <summary>
        ///     积分抵扣金额
        /// </summary>
        [Display(Name = "积分抵扣金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal pointMoney { get; set; }

        /// <summary>
        ///     订单优惠金额
        /// </summary>
        [Display(Name = "订单优惠金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal orderDiscountAmount { get; set; }

        /// <summary>
        ///     商品优惠金额
        /// </summary>
        [Display(Name = "商品优惠金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal goodsDiscountAmount { get; set; }

        /// <summary>
        ///     优惠券优惠额度
        /// </summary>
        [Display(Name = "优惠券优惠额度")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal couponDiscountAmount { get; set; }

        /// <summary>
        ///     优惠券信息
        /// </summary>
        [Display(Name = "优惠券信息")]
        public string coupon { get; set; }

        /// <summary>
        ///     优惠信息
        /// </summary>
        [Display(Name = "优惠信息")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string promotionList { get; set; }

        /// <summary>
        ///     买家备注
        /// </summary>
        [Display(Name = "买家备注")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string memo { get; set; }

        /// <summary>
        ///     下单IP
        /// </summary>
        [Display(Name = "下单IP")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string ip { get; set; }

        /// <summary>
        ///     卖家备注
        /// </summary>
        [Display(Name = "卖家备注")]
        [StringLength(510, ErrorMessage = "{0}不能超过{1}字")]
        public string mark { get; set; }

        /// <summary>
        ///     订单来源
        /// </summary>
        [Display(Name = "订单来源")]
        [Required(ErrorMessage = "请输入{0}")]
        public int source { get; set; }

        /// <summary>
        ///     是否评论
        /// </summary>
        [Display(Name = "是否评论")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isComment { get; set; }

        /// <summary>
        ///     删除标志
        /// </summary>
        [Display(Name = "删除标志")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isdel { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? updateTime { get; set; }
    }
}