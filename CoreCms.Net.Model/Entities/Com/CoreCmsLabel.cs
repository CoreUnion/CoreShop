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
    ///     标签表
    /// </summary>
    public class CoreCmsLabel
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     标签名称
        /// </summary>
        [Display(Name = "标签名称")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     标签样式
        /// </summary>
        [Display(Name = "标签样式")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string style { get; set; }
    }
}