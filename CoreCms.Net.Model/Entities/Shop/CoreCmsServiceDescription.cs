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
    ///     商城服务说明
    /// </summary>
    public class CoreCmsServiceDescription
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]

        public int id { get; set; }

        /// <summary>
        ///     名称
        /// </summary>
        [Display(Name = "名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string title { get; set; }

        /// <summary>
        ///     类型
        /// </summary>
        [Display(Name = "类型")]
        [Required(ErrorMessage = "请输入{0}")]

        public int type { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        [Display(Name = "描述")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(250, ErrorMessage = "{0}不能超过{1}字")]
        public string description { get; set; }

        /// <summary>
        ///     是否展示
        /// </summary>
        [Display(Name = "是否展示")]
        [Required(ErrorMessage = "请输入{0}")]

        public bool isShow { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入{0}")]

        public int sortId { get; set; }
    }
}