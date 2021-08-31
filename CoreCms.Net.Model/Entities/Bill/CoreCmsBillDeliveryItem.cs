/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/12 0:52:34
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 发货单详情表
    /// </summary>
    public partial class CoreCmsBillDeliveryItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsBillDeliveryItem()
        {
        }
		
        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
		
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 id  { get; set; }
        
		
        /// <summary>
        /// 订单编号
        /// </summary>
        [Display(Name = "订单编号")]
		
        
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String orderId  { get; set; }
        
		
        /// <summary>
        /// 发货单号 关联bill_delivery.id
        /// </summary>
        [Display(Name = "发货单号 关联bill_delivery.id")]
		
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String deliveryId  { get; set; }
        
		
        /// <summary>
        /// 商品ID 关联goods.id
        /// </summary>
        [Display(Name = "商品ID 关联goods.id")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 goodsId  { get; set; }
        
		
        /// <summary>
        /// 货品ID 关联products.id
        /// </summary>
        [Display(Name = "货品ID 关联products.id")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 productId  { get; set; }
        
		
        /// <summary>
        /// 货品编码
        /// </summary>
        [Display(Name = "货品编码")]
		
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength:30,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String sn  { get; set; }
        
		
        /// <summary>
        /// 商品编码
        /// </summary>
        [Display(Name = "商品编码")]
		
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength:30,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String bn  { get; set; }
        
		
        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
		
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength:200,ErrorMessage = "{0}不能超过{1}字")]
        
        
        public System.String name  { get; set; }
        
		
        /// <summary>
        /// 发货数量
        /// </summary>
        [Display(Name = "发货数量")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 nums  { get; set; }
        
		
        /// <summary>
        /// 重量
        /// </summary>
        [Display(Name = "重量")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Decimal weight  { get; set; }
        
		
        /// <summary>
        /// 货品明细序列号存储
        /// </summary>
        [Display(Name = "货品明细序列号存储")]
		
        
        
        
        
        public System.String addon  { get; set; }
        
		
    }
}
