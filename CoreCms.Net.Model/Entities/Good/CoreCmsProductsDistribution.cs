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
    /// 货品三级佣金表
    /// </summary>
    [SugarTable("CoreCmsProductsDistribution",TableDescription = "货品三级佣金表")]
    public partial class CoreCmsProductsDistribution
    {
        /// <summary>
        /// 货品三级佣金表
        /// </summary>
        public CoreCmsProductsDistribution()
        {
        }

        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "序号")]
        [SugarColumn(ColumnDescription = "序号", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 货品序列
        /// </summary>
        [Display(Name = "货品序列")]
        [SugarColumn(ColumnDescription = "货品序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 productsId { get; set; }
        /// <summary>
        /// 货品货号
        /// </summary>
        [Display(Name = "货品货号")]
        [SugarColumn(ColumnDescription = "货品货号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String productsSN { get; set; }
        /// <summary>
        /// 一级佣金
        /// </summary>
        [Display(Name = "一级佣金")]
        [SugarColumn(ColumnDescription = "一级佣金")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal levelOne { get; set; }
        /// <summary>
        /// 二级佣金
        /// </summary>
        [Display(Name = "二级佣金")]
        [SugarColumn(ColumnDescription = "二级佣金")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal levelTwo { get; set; }
        /// <summary>
        /// 三级佣金
        /// </summary>
        [Display(Name = "三级佣金")]
        [SugarColumn(ColumnDescription = "三级佣金")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal levelThree { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}