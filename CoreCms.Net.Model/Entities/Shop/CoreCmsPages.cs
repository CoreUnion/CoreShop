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
    ///     单页
    /// </summary>
    public class CoreCmsPages
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]

        public int id { get; set; }

        /// <summary>
        ///     区域编码
        /// </summary>
        [Display(Name = "区域编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(25, ErrorMessage = "{0}不能超过{1}字")]
        public string code { get; set; }

        /// <summary>
        ///     区域名称
        /// </summary>
        [Display(Name = "区域名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(25, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        [Display(Name = "描述")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(127, ErrorMessage = "{0}不能超过{1}字")]
        public string description { get; set; }

        /// <summary>
        ///     布局样式
        /// </summary>
        [Display(Name = "布局样式")]
        [Required(ErrorMessage = "请输入{0}")]

        public int layout { get; set; }

        /// <summary>
        ///     类型
        /// </summary>
        [Display(Name = "类型")]
        [Required(ErrorMessage = "请输入{0}")]

        public int type { get; set; }
    }
}