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
    ///     促销条件表
    /// </summary>
    public class CoreCmsPromotionCondition
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     促销ID
        /// </summary>
        [Display(Name = "促销ID")]
        public int? promotionId { get; set; }

        /// <summary>
        ///     促销条件编码
        /// </summary>
        [Display(Name = "促销条件编码")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string code { get; set; }

        /// <summary>
        ///     支付配置参数序列号存储
        /// </summary>
        [Display(Name = "支付配置参数序列号存储")]
        public string parameters { get; set; }
    }
}