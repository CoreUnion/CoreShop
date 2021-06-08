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
    /// 退货单明细表
    /// </summary>
    [SugarTable("CoreCmsBillReshipItem",TableDescription = "退货单明细表")]
    public partial class CoreCmsBillReshipItem
    {
        /// <summary>
        /// 退货单明细表
        /// </summary>
        public CoreCmsBillReshipItem()
        {
        }

        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(ColumnDescription = "序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 退款单单id
        /// </summary>
        [Display(Name = "退款单单id")]
        [SugarColumn(ColumnDescription = "退款单单id")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String reshipId { get; set; }
        /// <summary>
        /// 订单明细ID 关联order_items.id
        /// </summary>
        [Display(Name = "订单明细ID 关联order_items.id")]
        [SugarColumn(ColumnDescription = "订单明细ID 关联order_items.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 orderItemsId { get; set; }
        /// <summary>
        /// 商品ID 关联goods.id
        /// </summary>
        [Display(Name = "商品ID 关联goods.id")]
        [SugarColumn(ColumnDescription = "商品ID 关联goods.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsId { get; set; }
        /// <summary>
        /// 货品ID 关联products.id
        /// </summary>
        [Display(Name = "货品ID 关联products.id")]
        [SugarColumn(ColumnDescription = "货品ID 关联products.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 productId { get; set; }
        /// <summary>
        /// 货品编码
        /// </summary>
        [Display(Name = "货品编码")]
        [SugarColumn(ColumnDescription = "货品编码", IsNullable = true)]
        [StringLength(30, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String sn { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        [Display(Name = "商品编码")]
        [SugarColumn(ColumnDescription = "商品编码", IsNullable = true)]
        [StringLength(30, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String bn { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [SugarColumn(ColumnDescription = "商品名称", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [Display(Name = "图片")]
        [SugarColumn(ColumnDescription = "图片", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String imageUrl { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "数量")]
        [SugarColumn(ColumnDescription = "数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 nums { get; set; }
        /// <summary>
        /// 货品明细序列号存储
        /// </summary>
        [Display(Name = "货品明细序列号存储")]
        [SugarColumn(ColumnDescription = "货品明细序列号存储", IsNullable = true)]
        public System.String addon { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
        [SugarColumn(ColumnDescription = "", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}