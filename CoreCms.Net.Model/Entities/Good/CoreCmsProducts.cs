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
    /// 货品表
    /// </summary>
    [SugarTable("CoreCmsProducts",TableDescription = "货品表")]
    public partial class CoreCmsProducts
    {
        /// <summary>
        /// 货品表
        /// </summary>
        public CoreCmsProducts()
        {
        }

        /// <summary>
        /// 货品序列
        /// </summary>
        [Display(Name = "货品序列")]
        [SugarColumn(ColumnDescription = "货品序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 商品序列
        /// </summary>
        [Display(Name = "商品序列")]
        [SugarColumn(ColumnDescription = "商品序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsId { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        [Display(Name = "商品条码")]
        [SugarColumn(ColumnDescription = "商品条码", IsNullable = true)]
        [StringLength(128, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String barcode { get; set; }
        /// <summary>
        /// 货品编码
        /// </summary>
        [Display(Name = "货品编码")]
        [SugarColumn(ColumnDescription = "货品编码", IsNullable = true)]
        [StringLength(30, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String sn { get; set; }
        /// <summary>
        /// 货品价格
        /// </summary>
        [Display(Name = "货品价格")]
        [SugarColumn(ColumnDescription = "货品价格")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal price { get; set; }
        /// <summary>
        /// 货品成本价
        /// </summary>
        [Display(Name = "货品成本价")]
        [SugarColumn(ColumnDescription = "货品成本价")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal costprice { get; set; }
        /// <summary>
        /// 货品市场价
        /// </summary>
        [Display(Name = "货品市场价")]
        [SugarColumn(ColumnDescription = "货品市场价")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal mktprice { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        [Display(Name = "是否上架")]
        [SugarColumn(ColumnDescription = "是否上架")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean marketable { get; set; }
        /// <summary>
        /// 重量(千克)
        /// </summary>
        [Display(Name = "重量(千克)")]
        [SugarColumn(ColumnDescription = "重量(千克)")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal weight { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        [Display(Name = "库存")]
        [SugarColumn(ColumnDescription = "库存")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 stock { get; set; }
        /// <summary>
        /// 冻结库存
        /// </summary>
        [Display(Name = "冻结库存")]
        [SugarColumn(ColumnDescription = "冻结库存")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 freezeStock { get; set; }
        /// <summary>
        /// 规格值
        /// </summary>
        [Display(Name = "规格值")]
        [SugarColumn(ColumnDescription = "规格值", IsNullable = true)]
        public System.String spesDesc { get; set; }
        /// <summary>
        /// 是否默认货品
        /// </summary>
        [Display(Name = "是否默认货品")]
        [SugarColumn(ColumnDescription = "是否默认货品")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDefalut { get; set; }
        /// <summary>
        /// 规格图片
        /// </summary>
        [Display(Name = "规格图片")]
        [SugarColumn(ColumnDescription = "规格图片", IsNullable = true)]
        public System.String images { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [SugarColumn(ColumnDescription = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDel { get; set; }
    }
}