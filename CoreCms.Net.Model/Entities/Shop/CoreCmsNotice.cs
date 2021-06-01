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
    ///     公告表
    /// </summary>
    public class CoreCmsNotice
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     公告标题
        /// </summary>
        [Display(Name = "公告标题")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string title { get; set; }

        /// <summary>
        ///     公告内容
        /// </summary>
        [Display(Name = "公告内容")]
        [Required(ErrorMessage = "请输入{0}")]
        public string contentBody { get; set; }

        /// <summary>
        ///     公告类型
        /// </summary>
        [Display(Name = "公告类型")]
        public int? type { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        [Display(Name = "排序")]
        public int? sort { get; set; }

        /// <summary>
        ///     软删除位  有时间代表已删除
        /// </summary>
        [Display(Name = "软删除位  有时间代表已删除")]
        public bool? isDel { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime? createTime { get; set; }
    }
}