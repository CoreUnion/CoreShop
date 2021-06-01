/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     用户表扩展
    /// </summary>
    public partial class SysUser
    {
        /// <summary>
        ///     权限序列
        /// </summary>
        [Display(Name = "权限序列")]
        [SugarColumn(IsIgnore = true)]
        public string roleIds { get; set; }


        /// <summary>
        ///     权限列表
        /// </summary>
        [Display(Name = "权限序列")]
        [SugarColumn(IsIgnore = true)]
        public List<SysRole> roles { get; set; }


        /// <summary>
        ///     组织机构名称
        /// </summary>
        [Display(Name = "组织机构名称")]
        [SugarColumn(IsIgnore = true)]
        public string organizationName { get; set; }
    }
}