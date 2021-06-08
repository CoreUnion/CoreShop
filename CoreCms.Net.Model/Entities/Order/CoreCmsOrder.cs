/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:58
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 订单表
    /// </summary>
    [SugarTable("CoreCmsOrder",TableDescription = "订单表")]
    public partial class CoreCmsOrder
    {
        /// <summary>
        /// 订单表
        /// </summary>
        public CoreCmsOrder()
        {
        }

        /// <summary>
        /// 订单号
        /// </summary>
        [Display(Name = "订单号")]
        [SugarColumn(ColumnDescription = "订单号", IsPrimaryKey = true)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String orderId { get; set; }
        /// <summary>
        /// 商品总价
        /// </summary>
        [Display(Name = "商品总价")]
        [SugarColumn(ColumnDescription = "商品总价")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal goodsAmount { get; set; }
        /// <summary>
        /// 已支付的金额
        /// </summary>
        [Display(Name = "已支付的金额")]
        [SugarColumn(ColumnDescription = "已支付的金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal payedAmount { get; set; }
        /// <summary>
        /// 订单实际销售总额
        /// </summary>
        [Display(Name = "订单实际销售总额")]
        [SugarColumn(ColumnDescription = "订单实际销售总额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal orderAmount { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        [Display(Name = "支付状态")]
        [SugarColumn(ColumnDescription = "支付状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 payStatus { get; set; }
        /// <summary>
        /// 发货状态
        /// </summary>
        [Display(Name = "发货状态")]
        [SugarColumn(ColumnDescription = "发货状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 shipStatus { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        [Display(Name = "订单状态")]
        [SugarColumn(ColumnDescription = "订单状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 status { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [Display(Name = "订单类型")]
        [SugarColumn(ColumnDescription = "订单类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 orderType { get; set; }
        /// <summary>
        /// 收货方式
        /// </summary>
        [Display(Name = "收货方式")]
        [SugarColumn(ColumnDescription = "收货方式")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 receiptType { get; set; }
        /// <summary>
        /// 支付方式代码
        /// </summary>
        [Display(Name = "支付方式代码")]
        [SugarColumn(ColumnDescription = "支付方式代码", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String paymentCode { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        [Display(Name = "支付时间")]
        [SugarColumn(ColumnDescription = "支付时间", IsNullable = true)]
        public System.DateTime? paymentTime { get; set; }
        /// <summary>
        /// 配送方式ID 关联ship.id
        /// </summary>
        [Display(Name = "配送方式ID 关联ship.id")]
        [SugarColumn(ColumnDescription = "配送方式ID 关联ship.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 logisticsId { get; set; }
        /// <summary>
        /// 配送方式名称
        /// </summary>
        [Display(Name = "配送方式名称")]
        [SugarColumn(ColumnDescription = "配送方式名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String logisticsName { get; set; }
        /// <summary>
        /// 配送费用
        /// </summary>
        [Display(Name = "配送费用")]
        [SugarColumn(ColumnDescription = "配送费用")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal costFreight { get; set; }
        /// <summary>
        /// 用户ID 关联user.id
        /// </summary>
        [Display(Name = "用户ID 关联user.id")]
        [SugarColumn(ColumnDescription = "用户ID 关联user.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 店铺ID 关联seller.id
        /// </summary>
        [Display(Name = "店铺ID 关联seller.id")]
        [SugarColumn(ColumnDescription = "店铺ID 关联seller.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sellerId { get; set; }
        /// <summary>
        /// 售后状态
        /// </summary>
        [Display(Name = "售后状态")]
        [SugarColumn(ColumnDescription = "售后状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 confirmStatus { get; set; }
        /// <summary>
        /// 确认收货时间
        /// </summary>
        [Display(Name = "确认收货时间")]
        [SugarColumn(ColumnDescription = "确认收货时间", IsNullable = true)]
        public System.DateTime? confirmTime { get; set; }
        /// <summary>
        /// 自提门店ID，0就是不门店自提
        /// </summary>
        [Display(Name = "自提门店ID，0就是不门店自提")]
        [SugarColumn(ColumnDescription = "自提门店ID，0就是不门店自提")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 storeId { get; set; }
        /// <summary>
        /// 收货地区ID
        /// </summary>
        [Display(Name = "收货地区ID")]
        [SugarColumn(ColumnDescription = "收货地区ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 shipAreaId { get; set; }
        /// <summary>
        /// 收货详细地址
        /// </summary>
        [Display(Name = "收货详细地址")]
        [SugarColumn(ColumnDescription = "收货详细地址", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String shipAddress { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        [Display(Name = "收货人姓名")]
        [SugarColumn(ColumnDescription = "收货人姓名", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String shipName { get; set; }
        /// <summary>
        /// 收货电话
        /// </summary>
        [Display(Name = "收货电话")]
        [SugarColumn(ColumnDescription = "收货电话", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String shipMobile { get; set; }
        /// <summary>
        /// 商品总重量
        /// </summary>
        [Display(Name = "商品总重量")]
        [SugarColumn(ColumnDescription = "商品总重量")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal weight { get; set; }
        /// <summary>
        /// 开发票
        /// </summary>
        [Display(Name = "开发票")]
        [SugarColumn(ColumnDescription = "开发票")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 taxType { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        [Display(Name = "税号")]
        [SugarColumn(ColumnDescription = "税号", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String taxCode { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        [Display(Name = "发票抬头")]
        [SugarColumn(ColumnDescription = "发票抬头", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String taxTitle { get; set; }
        /// <summary>
        /// 使用积分
        /// </summary>
        [Display(Name = "使用积分")]
        [SugarColumn(ColumnDescription = "使用积分")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 point { get; set; }
        /// <summary>
        /// 积分抵扣金额
        /// </summary>
        [Display(Name = "积分抵扣金额")]
        [SugarColumn(ColumnDescription = "积分抵扣金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal pointMoney { get; set; }
        /// <summary>
        /// 订单优惠金额
        /// </summary>
        [Display(Name = "订单优惠金额")]
        [SugarColumn(ColumnDescription = "订单优惠金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal orderDiscountAmount { get; set; }
        /// <summary>
        /// 商品优惠金额
        /// </summary>
        [Display(Name = "商品优惠金额")]
        [SugarColumn(ColumnDescription = "商品优惠金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal goodsDiscountAmount { get; set; }
        /// <summary>
        /// 优惠券优惠额度
        /// </summary>
        [Display(Name = "优惠券优惠额度")]
        [SugarColumn(ColumnDescription = "优惠券优惠额度")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal couponDiscountAmount { get; set; }
        /// <summary>
        /// 优惠券信息
        /// </summary>
        [Display(Name = "优惠券信息")]
        [SugarColumn(ColumnDescription = "优惠券信息", IsNullable = true)]
        public System.String coupon { get; set; }
        /// <summary>
        /// 优惠信息
        /// </summary>
        [Display(Name = "优惠信息")]
        [SugarColumn(ColumnDescription = "优惠信息", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String promotionList { get; set; }
        /// <summary>
        /// 买家备注
        /// </summary>
        [Display(Name = "买家备注")]
        [SugarColumn(ColumnDescription = "买家备注", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String memo { get; set; }
        /// <summary>
        /// 下单IP
        /// </summary>
        [Display(Name = "下单IP")]
        [SugarColumn(ColumnDescription = "下单IP", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String ip { get; set; }
        /// <summary>
        /// 卖家备注
        /// </summary>
        [Display(Name = "卖家备注")]
        [SugarColumn(ColumnDescription = "卖家备注", IsNullable = true)]
        [StringLength(510, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String mark { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        [Display(Name = "订单来源")]
        [SugarColumn(ColumnDescription = "订单来源")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 source { get; set; }
        /// <summary>
        /// 是否评论
        /// </summary>
        [Display(Name = "是否评论")]
        [SugarColumn(ColumnDescription = "是否评论")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isComment { get; set; }
        /// <summary>
        /// 删除标志
        /// </summary>
        [Display(Name = "删除标志")]
        [SugarColumn(ColumnDescription = "删除标志")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isdel { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}