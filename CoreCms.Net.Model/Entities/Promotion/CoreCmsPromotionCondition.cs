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
    /// 促销条件表
    /// </summary>
    [SugarTable("CoreCmsPromotionCondition",TableDescription = "促销条件表")]
    public partial class CoreCmsPromotionCondition
    {
        /// <summary>
        /// 促销条件表
        /// </summary>
        public CoreCmsPromotionCondition()
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
        /// 促销ID
        /// </summary>
        [Display(Name = "促销ID")]
        [SugarColumn(ColumnDescription = "促销ID", IsNullable = true)]
        public System.Int32? promotionId { get; set; }
        /// <summary>
        /// 促销条件编码
        /// </summary>
        [Display(Name = "促销条件编码")]
        [SugarColumn(ColumnDescription = "促销条件编码", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String code { get; set; }
        /// <summary>
        /// 支付配置参数序列号存储
        /// </summary>
        [Display(Name = "支付配置参数序列号存储")]
        [SugarColumn(ColumnDescription = "支付配置参数序列号存储", IsNullable = true)]
        public System.String parameters { get; set; }
    }
}