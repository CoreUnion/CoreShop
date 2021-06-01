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
    /// 售后单明细表
    /// </summary>
    public partial class CoreCmsBillAftersalesItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsBillAftersalesItem()
        {
        }
		
        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
		[SugarColumn(IsPrimaryKey = true, IsIdentity = true)][Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id  { get; set; }
		
        /// <summary>
        /// 售后单id
        /// </summary>
        [Display(Name = "售后单id")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:32,ErrorMessage = "{0}不能超过{1}字")]
        public System.String aftersalesId  { get; set; }
		
        /// <summary>
        /// 订单明细ID 关联order_items.id
        /// </summary>
        [Display(Name = "订单明细ID 关联order_items.id")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Int32 orderItemsId  { get; set; }
		
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
		[StringLength(maximumLength:30,ErrorMessage = "{0}不能超过{1}字")]
        public System.String sn  { get; set; }
		
        /// <summary>
        /// 商品编码
        /// </summary>
        [Display(Name = "商品编码")]
		[StringLength(maximumLength:30,ErrorMessage = "{0}不能超过{1}字")]
        public System.String bn  { get; set; }
		
        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
		[StringLength(maximumLength:200,ErrorMessage = "{0}不能超过{1}字")]
        public System.String name  { get; set; }
		
        /// <summary>
        /// 图片
        /// </summary>
        [Display(Name = "图片")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        public System.String imageUrl  { get; set; }
		
        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "数量")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Int32 nums  { get; set; }
		
        /// <summary>
        /// 货品明细序列号存储
        /// </summary>
        [Display(Name = "货品明细序列号存储")]
		
        public System.String addon  { get; set; }
		
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
