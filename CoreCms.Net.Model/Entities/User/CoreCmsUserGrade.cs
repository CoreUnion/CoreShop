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
    ///     用户等级表
    /// </summary>
    public class CoreCmsUserGrade
    {
        /// <summary>
        ///     id
        /// </summary>
        [Display(Name = "id")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     标题
        /// </summary>
        [Display(Name = "标题")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(60, ErrorMessage = "{0}不能超过{1}字")]
        public string title { get; set; }

        /// <summary>
        ///     是否默认
        /// </summary>
        [Display(Name = "是否默认")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDefault { get; set; }
    }
}