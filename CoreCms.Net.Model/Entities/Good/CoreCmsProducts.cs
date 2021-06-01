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
    ///     货品表
    /// </summary>
    public partial class CoreCmsProducts
    {
        /// <summary>
        ///     货品序列
        /// </summary>
        [Display(Name = "货品序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]


        public int id { get; set; }


        /// <summary>
        ///     商品序列
        /// </summary>
        [Display(Name = "商品序列")]
        [Required(ErrorMessage = "请输入{0}")]


        public int goodsId { get; set; }


        /// <summary>
        ///     商品条码
        /// </summary>
        [Display(Name = "商品条码")]
        [StringLength(128, ErrorMessage = "{0}不能超过{1}字")]


        public string barcode { get; set; }


        /// <summary>
        ///     货品编码
        /// </summary>
        [Display(Name = "货品编码")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]


        public string sn { get; set; }


        /// <summary>
        ///     货品价格
        /// </summary>
        [Display(Name = "货品价格")]
        [Required(ErrorMessage = "请输入{0}")]


        public decimal price { get; set; }


        /// <summary>
        ///     货品成本价
        /// </summary>
        [Display(Name = "货品成本价")]
        [Required(ErrorMessage = "请输入{0}")]


        public decimal costprice { get; set; }


        /// <summary>
        ///     货品市场价
        /// </summary>
        [Display(Name = "货品市场价")]
        [Required(ErrorMessage = "请输入{0}")]


        public decimal mktprice { get; set; }


        /// <summary>
        ///     是否上架
        /// </summary>
        [Display(Name = "是否上架")]
        [Required(ErrorMessage = "请输入{0}")]


        public bool marketable { get; set; }


        /// <summary>
        ///     重量(千克)
        /// </summary>
        [Display(Name = "重量(千克)")]
        [Required(ErrorMessage = "请输入{0}")]


        public decimal weight { get; set; }


        /// <summary>
        ///     库存
        /// </summary>
        [Display(Name = "库存")]
        [Required(ErrorMessage = "请输入{0}")]


        public int stock { get; set; }


        /// <summary>
        ///     冻结库存
        /// </summary>
        [Display(Name = "冻结库存")]
        [Required(ErrorMessage = "请输入{0}")]


        public int freezeStock { get; set; }


        /// <summary>
        ///     规格值
        /// </summary>
        [Display(Name = "规格值")]
        public string spesDesc { get; set; }


        /// <summary>
        ///     是否默认货品
        /// </summary>
        [Display(Name = "是否默认货品")]
        [Required(ErrorMessage = "请输入{0}")]


        public bool isDefalut { get; set; }


        /// <summary>
        ///     规格图片
        /// </summary>
        [Display(Name = "规格图片")]
        public string images { get; set; }


        /// <summary>
        ///     是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]


        public bool isDel { get; set; }
    }
}