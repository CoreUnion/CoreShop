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
    ///     角色表
    /// </summary>
    public class SysRole
    {
        /// <summary>
        ///     角色id
        /// </summary>
        [Display(Name = "角色id")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     角色名称
        /// </summary>
        [Display(Name = "角色名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string roleName { get; set; }

        /// <summary>
        ///     角色标识
        /// </summary>
        [Display(Name = "角色标识")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string roleCode { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string comments { get; set; }

        /// <summary>
        ///     是否删除,0否,1是
        /// </summary>
        [Display(Name = "是否删除,0否,1是")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool deleted { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }

        /// <summary>
        ///     修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        public DateTime? updateTime { get; set; }
    }
}