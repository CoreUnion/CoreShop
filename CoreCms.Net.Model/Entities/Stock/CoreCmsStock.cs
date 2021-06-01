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
    ///     库存操作表
    /// </summary>
    public partial class CoreCmsStock
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string id { get; set; }

        /// <summary>
        ///     操作类型
        /// </summary>
        [Display(Name = "操作类型")]
        [Required(ErrorMessage = "请输入{0}")]

        public int type { get; set; }

        /// <summary>
        ///     操作员
        /// </summary>
        [Display(Name = "操作员")]
        [Required(ErrorMessage = "请输入{0}")]

        public int manager { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string memo { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]

        public DateTime createTime { get; set; }
    }
}