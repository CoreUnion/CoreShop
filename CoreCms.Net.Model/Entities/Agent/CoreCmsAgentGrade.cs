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
    /// 代理商等级设置表
    /// </summary>
    [SugarTable("CoreCmsAgentGrade",TableDescription = "代理商等级设置表")]
    public partial class CoreCmsAgentGrade
    {
        /// <summary>
        /// 代理商等级设置表
        /// </summary>
        public CoreCmsAgentGrade()
        {
        }

        /// <summary>
        /// 等级序列
        /// </summary>
        [Display(Name = "等级序列")]
        [SugarColumn(ColumnDescription = "等级序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 等级名称
        /// </summary>
        [Display(Name = "等级名称")]
        [SugarColumn(ColumnDescription = "等级名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 是否默认等级
        /// </summary>
        [Display(Name = "是否默认等级")]
        [SugarColumn(ColumnDescription = "是否默认等级")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDefault { get; set; }
        /// <summary>
        /// 是否自动升级
        /// </summary>
        [Display(Name = "是否自动升级")]
        [SugarColumn(ColumnDescription = "是否自动升级")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isAutoUpGrade { get; set; }
        /// <summary>
        /// 价格加成方式
        /// </summary>
        [Display(Name = "价格加成方式")]
        [SugarColumn(ColumnDescription = "价格加成方式")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 defaultSalesPriceType { get; set; }
        /// <summary>
        /// 价格加成值
        /// </summary>
        [Display(Name = "价格加成值")]
        [SugarColumn(ColumnDescription = "价格加成值")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 defaultSalesPriceNumber { get; set; }
        /// <summary>
        /// 等级排序
        /// </summary>
        [Display(Name = "等级排序")]
        [SugarColumn(ColumnDescription = "等级排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sortId { get; set; }
        /// <summary>
        /// 等级说明
        /// </summary>
        [Display(Name = "等级说明")]
        [SugarColumn(ColumnDescription = "等级说明", IsNullable = true)]
        [StringLength(500, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String description { get; set; }
    }
}