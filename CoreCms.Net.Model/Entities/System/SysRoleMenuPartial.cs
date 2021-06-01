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
    ///     用户权限和菜单关系表扩展
    /// </summary>
    public partial class SysRoleMenu
    {
        /// <summary>
        ///     菜单
        /// </summary>
        [Display(Name = "菜单")]
        [SugarColumn(IsIgnore = true)]
        public SysMenu menu { get; set; }


        /// <summary>
        ///     权限
        /// </summary>
        [Display(Name = "权限")]
        [SugarColumn(IsIgnore = true)]
        public SysRole role { get; set; }
    }
}