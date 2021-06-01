/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 退款单表
    /// </summary>
    public partial class CoreCmsBillRefund
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsBillRefund()
        {
        }
		
        /// <summary>
        /// 退款单ID
        /// </summary>
        [Display(Name = "退款单ID")]
		[SugarColumn(IsPrimaryKey = true)][Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        public System.String refundId  { get; set; }
		
        /// <summary>
        /// 售后单id
        /// </summary>
        [Display(Name = "售后单id")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        public System.String aftersalesId  { get; set; }
		
        /// <summary>
        /// 退款金额
        /// </summary>
        [Display(Name = "退款金额")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Decimal money  { get; set; }
		
        /// <summary>
        /// 用户ID 关联user.id
        /// </summary>
        [Display(Name = "用户ID 关联user.id")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId  { get; set; }
		
        /// <summary>
        /// 资源id，根据type不同而关联不同的表
        /// </summary>
        [Display(Name = "资源id，根据type不同而关联不同的表")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        public System.String sourceId  { get; set; }
		
        /// <summary>
        /// 资源类型1=订单,2充值单
        /// </summary>
        [Display(Name = "资源类型1=订单,2充值单")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Int32 type  { get; set; }
		
        /// <summary>
        /// 退款支付类型编码 默认原路返回 关联支付单表支付编码
        /// </summary>
        [Display(Name = "退款支付类型编码 默认原路返回 关联支付单表支付编码")]
		[StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        public System.String paymentCode  { get; set; }
		
        /// <summary>
        /// 第三方平台交易流水号
        /// </summary>
        [Display(Name = "第三方平台交易流水号")]
		[StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        public System.String tradeNo  { get; set; }
		
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Int32 status  { get; set; }
		
        /// <summary>
        /// 退款失败原因
        /// </summary>
        [Display(Name = "退款失败原因")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:100,ErrorMessage = "{0}不能超过{1}字")]
        public System.String memo  { get; set; }
		
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime  { get; set; }
		
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
		
        public System.DateTime? updateTime  { get; set; }
		
    }
}
