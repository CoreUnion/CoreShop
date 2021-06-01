/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     库存操作详情表
    /// </summary>
    public partial class CoreCmsStockLog
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]


        public int id { get; set; }


        /// <summary>
        ///     库存单号
        /// </summary>
        [Display(Name = "库存单号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]


        public string stockId { get; set; }


        /// <summary>
        ///     货品序列
        /// </summary>
        [Display(Name = "货品序列")]
        [Required(ErrorMessage = "请输入{0}")]


        public int productId { get; set; }


        /// <summary>
        ///     商品序列
        /// </summary>
        [Display(Name = "商品序列")]
        [Required(ErrorMessage = "请输入{0}")]


        public int goodsId { get; set; }


        /// <summary>
        ///     数量
        /// </summary>
        [Display(Name = "数量")]
        [Required(ErrorMessage = "请输入{0}")]


        public int nums { get; set; }


        /// <summary>
        ///     货品编码
        /// </summary>
        [Display(Name = "货品编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string sn { get; set; }


        /// <summary>
        ///     商品条码
        /// </summary>
        [Display(Name = "商品条码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string bn { get; set; }


        /// <summary>
        ///     商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]


        public string goodsName { get; set; }


        /// <summary>
        ///     货品明细序列号存储
        /// </summary>
        [Display(Name = "货品明细序列号存储")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]


        public string spesDesc { get; set; }
    }
}