/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:59
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 角色菜单关联表
    /// </summary>
    [SugarTable("SysRoleMenu",TableDescription = "角色菜单关联表")]
    public partial class SysRoleMenu
    {
        /// <summary>
        /// 角色菜单关联表
        /// </summary>
        public SysRoleMenu()
        {
        }

        /// <summary>
        /// 主键
        /// </summary>
        [Display(Name = "主键")]
        [SugarColumn(ColumnDescription = "主键", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        [Display(Name = "角色id")]
        [SugarColumn(ColumnDescription = "角色id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 roleId { get; set; }
        /// <summary>
        /// 菜单id
        /// </summary>
        [Display(Name = "菜单id")]
        [SugarColumn(ColumnDescription = "菜单id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 menuId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [SugarColumn(ColumnDescription = "修改时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}