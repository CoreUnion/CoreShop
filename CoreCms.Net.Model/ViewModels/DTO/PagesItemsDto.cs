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

namespace CoreCms.Net.Model.ViewModels.DTO
{
    /// <summary>
    ///     单页内容
    /// </summary>
    public class PagesItemsDto
    {
        /// <summary>
        /// </summary>
        [Display(Name = "")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     组件编码
        /// </summary>
        [Display(Name = "组件编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string widgetCode { get; set; }

        /// <summary>
        ///     页面编码
        /// </summary>
        [Display(Name = "页面编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string pageCode { get; set; }

        /// <summary>
        ///     布局位置
        /// </summary>
        [Display(Name = "布局位置")]
        [Required(ErrorMessage = "请输入{0}")]
        public int positionId { get; set; }

        /// <summary>
        ///     排序，越小越靠前
        /// </summary>
        [Display(Name = "排序，越小越靠前")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }

        /// <summary>
        ///     组件配置内容
        /// </summary>
        [Display(Name = "组件配置内容")]
        public object parameters { get; set; }
    }
}