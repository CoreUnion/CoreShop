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
    ///     用户日志
    /// </summary>
    public class CoreCmsUserLog
    {
        /// <summary>
        ///     id
        /// </summary>
        [Display(Name = "id")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     用户id
        /// </summary>
        [Display(Name = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        [Display(Name = "状态")]
        public int? state { get; set; }

        /// <summary>
        ///     参数
        /// </summary>
        [Display(Name = "参数")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string parameters { get; set; }

        /// <summary>
        ///     ip地址
        /// </summary>
        [Display(Name = "ip地址")]
        [StringLength(15, ErrorMessage = "{0}不能超过{1}字")]
        public string ip { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime? createTime { get; set; }
    }
}