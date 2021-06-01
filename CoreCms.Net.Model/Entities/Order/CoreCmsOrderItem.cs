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
    ///     订单明细表
    /// </summary>
    public partial class CoreCmsOrderItem
    {
        /// <summary>
        ///     序号
        /// </summary>
        [Display(Name = "序号")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     订单ID 关联order.id
        /// </summary>
        [Display(Name = "订单ID 关联order.id")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string orderId { get; set; }

        /// <summary>
        ///     商品ID 关联goods.id
        /// </summary>
        [Display(Name = "商品ID 关联goods.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int goodsId { get; set; }

        /// <summary>
        ///     货品ID 关联products.id
        /// </summary>
        [Display(Name = "货品ID 关联products.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int productId { get; set; }

        /// <summary>
        ///     货品编码
        /// </summary>
        [Display(Name = "货品编码")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]
        public string sn { get; set; }

        /// <summary>
        ///     商品编码
        /// </summary>
        [Display(Name = "商品编码")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]
        public string bn { get; set; }

        /// <summary>
        ///     商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     货品价格单价
        /// </summary>
        [Display(Name = "货品价格单价")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal price { get; set; }

        /// <summary>
        ///     货品成本价单价
        /// </summary>
        [Display(Name = "货品成本价单价")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal costprice { get; set; }

        /// <summary>
        ///     市场价
        /// </summary>
        [Display(Name = "市场价")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal mktprice { get; set; }

        /// <summary>
        ///     图片
        /// </summary>
        [Display(Name = "图片")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(100, ErrorMessage = "{0}不能超过{1}字")]
        public string imageUrl { get; set; }

        /// <summary>
        ///     数量
        /// </summary>
        [Display(Name = "数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public int nums { get; set; }

        /// <summary>
        ///     总价
        /// </summary>
        [Display(Name = "总价")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal amount { get; set; }

        /// <summary>
        ///     商品优惠总金额
        /// </summary>
        [Display(Name = "商品优惠总金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal promotionAmount { get; set; }

        /// <summary>
        ///     促销信息
        /// </summary>
        [Display(Name = "促销信息")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string promotionList { get; set; }

        /// <summary>
        ///     总重量
        /// </summary>
        [Display(Name = "总重量")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal weight { get; set; }

        /// <summary>
        ///     发货数量
        /// </summary>
        [Display(Name = "发货数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sendNums { get; set; }

        /// <summary>
        ///     货品明细序列号存储
        /// </summary>
        [Display(Name = "货品明细序列号存储")]
        public string addon { get; set; }

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