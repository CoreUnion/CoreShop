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

namespace CoreCms.Net.Model.Entities.Entities
{
    /// <summary>
    ///     地区表
    /// </summary>
    public class CoreCmsArea
    {
        /// <summary>
        ///     地区ID
        /// </summary>
        [Display(Name = "地区ID")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     父级ID
        /// </summary>
        [Display(Name = "父级ID")]
        public int? parentId { get; set; }

        /// <summary>
        ///     地区深度
        /// </summary>
        [Display(Name = "地区深度")]
        public int? depth { get; set; }

        /// <summary>
        ///     地区名称
        /// </summary>
        [Display(Name = "地区名称")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     邮编
        /// </summary>
        [Display(Name = "邮编")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(10, ErrorMessage = "{0}不能超过{1}字")]
        public string postalCode { get; set; }

        /// <summary>
        ///     地区排序
        /// </summary>
        [Display(Name = "地区排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }
    }
}