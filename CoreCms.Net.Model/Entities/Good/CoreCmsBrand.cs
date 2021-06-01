/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     品牌表
    /// </summary>
    public class CoreCmsBrand
    {
        /// <summary>
        ///     品牌ID
        /// </summary>
        [Display(Name = "品牌ID")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     品牌名称
        /// </summary>
        [Display(Name = "品牌名称")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     品牌LOGO
        /// </summary>
        [Display(Name = "品牌LOGO")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string logoImageUrl { get; set; }

        /// <summary>
        ///     品牌排序
        /// </summary>
        [Display(Name = "品牌排序")]
        public int? sort { get; set; }

        /// <summary>
        ///     是否显示
        /// </summary>
        [Display(Name = "是否显示")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isShow { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? createTime { get; set; }
    }
}