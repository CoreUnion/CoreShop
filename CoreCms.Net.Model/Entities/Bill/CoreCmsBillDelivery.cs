/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/12 0:34:54
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 发货单表
    /// </summary>
    public partial class CoreCmsBillDelivery
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsBillDelivery()
        {
        }
		
        /// <summary>
        /// 发货单序列
        /// </summary>
        [Display(Name = "发货单序列")]
		
        [SugarColumn(IsPrimaryKey = true)]
        
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String deliveryId  { get; set; }
        
		
        /// <summary>
        /// 订单号
        /// </summary>
        [Display(Name = "订单号")]
		
        
        [StringLength(maximumLength:500,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String orderId  { get; set; }
        
		
        /// <summary>
        /// 物流公司编码
        /// </summary>
        [Display(Name = "物流公司编码")]
		
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String logiCode  { get; set; }
        
		
        /// <summary>
        /// 物流单号
        /// </summary>
        [Display(Name = "物流单号")]
		
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String logiNo  { get; set; }
        
		
        /// <summary>
        /// 第三方对接物流编码
        /// </summary>
        [Display(Name = "第三方对接物流编码")]
		
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String thirdPartylogiCode  { get; set; }
        
		
        /// <summary>
        /// 快递物流信息
        /// </summary>
        [Display(Name = "快递物流信息")]
		
        
        
        
        
        public System.String logiInformation  { get; set; }
        
		
        /// <summary>
        /// 快递是否不更新
        /// </summary>
        [Display(Name = "快递是否不更新")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Boolean logiStatus  { get; set; }
        
		
        /// <summary>
        /// 收货地区ID
        /// </summary>
        [Display(Name = "收货地区ID")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 shipAreaId  { get; set; }
        
		
        /// <summary>
        /// 收货详细地址
        /// </summary>
        [Display(Name = "收货详细地址")]
		
        
        [StringLength(maximumLength:200,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String shipAddress  { get; set; }
        
		
        /// <summary>
        /// 收货人姓名
        /// </summary>
        [Display(Name = "收货人姓名")]
		
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String shipName  { get; set; }
        
		
        /// <summary>
        /// 收货电话
        /// </summary>
        [Display(Name = "收货电话")]
		
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String shipMobile  { get; set; }
        
		
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 status  { get; set; }
        
		
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
		
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String memo  { get; set; }
        
		
        /// <summary>
        /// 确认收货时间
        /// </summary>
        [Display(Name = "确认收货时间")]
		
        
        
        
        
        public System.DateTime? confirmTime  { get; set; }
        
		
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
