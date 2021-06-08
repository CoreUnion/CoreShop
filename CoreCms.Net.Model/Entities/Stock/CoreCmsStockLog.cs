/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:59
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 库存操作详情表
    /// </summary>
    [SugarTable("CoreCmsStockLog",TableDescription = "库存操作详情表")]
    public partial class CoreCmsStockLog
    {
        /// <summary>
        /// 库存操作详情表
        /// </summary>
        public CoreCmsStockLog()
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
        /// 库存单号
        /// </summary>
        [Display(Name = "库存单号")]
        [SugarColumn(ColumnDescription = "库存单号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String stockId { get; set; }
        /// <summary>
        /// 货品序列
        /// </summary>
        [Display(Name = "货品序列")]
        [SugarColumn(ColumnDescription = "货品序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 productId { get; set; }
        /// <summary>
        /// 商品序列
        /// </summary>
        [Display(Name = "商品序列")]
        [SugarColumn(ColumnDescription = "商品序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 goodsId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "数量")]
        [SugarColumn(ColumnDescription = "数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 nums { get; set; }
        /// <summary>
        /// 货品编码
        /// </summary>
        [Display(Name = "货品编码")]
        [SugarColumn(ColumnDescription = "货品编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String sn { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        [Display(Name = "商品条码")]
        [SugarColumn(ColumnDescription = "商品条码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String bn { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [SugarColumn(ColumnDescription = "商品名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String goodsName { get; set; }
        /// <summary>
        /// 货品明细序列号存储
        /// </summary>
        [Display(Name = "货品明细序列号存储")]
        [SugarColumn(ColumnDescription = "货品明细序列号存储")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String spesDesc { get; set; }
    }
}