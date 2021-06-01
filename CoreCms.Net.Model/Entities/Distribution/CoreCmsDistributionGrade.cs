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
    ///     分销商等级设置表
    /// </summary>
    public class CoreCmsDistributionGrade
    {
        /// <summary>
        ///     等级序列
        /// </summary>
        [Display(Name = "等级序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     等级名称
        /// </summary>
        [Display(Name = "等级名称")]
        [StringLength(25, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     是否默认等级
        /// </summary>
        [Display(Name = "是否默认等级")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDefault { get; set; }

        /// <summary>
        ///     是否自动升级
        /// </summary>
        [Display(Name = "是否自动升级")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isAutoUpGrade { get; set; }

        /// <summary>
        ///     等级排序
        /// </summary>
        [Display(Name = "等级排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sortId { get; set; }

        /// <summary>
        ///     等级说明
        /// </summary>
        [Display(Name = "等级说明")]
        [StringLength(250, ErrorMessage = "{0}不能超过{1}字")]
        public string description { get; set; }
    }
}