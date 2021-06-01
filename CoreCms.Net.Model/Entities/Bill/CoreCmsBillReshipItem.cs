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
    ///     退货单明细表
    /// </summary>
    public class CoreCmsBillReshipItem
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     退款单单id
        /// </summary>
        [Display(Name = "退款单单id")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string reshipId { get; set; }

        /// <summary>
        ///     订单明细ID 关联order_items.id
        /// </summary>
        [Display(Name = "订单明细ID 关联order_items.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int orderItemsId { get; set; }

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
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     图片
        /// </summary>
        [Display(Name = "图片")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string imageUrl { get; set; }

        /// <summary>
        ///     数量
        /// </summary>
        [Display(Name = "数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public int nums { get; set; }

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
        /// </summary>
        [Display(Name = "")]
        public DateTime? updateTime { get; set; }
    }
}