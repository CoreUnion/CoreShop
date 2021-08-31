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

namespace CoreCms.Net.Model.ViewModels.DTO
{
    public class SysMenuTreeDto
    {
        /// <summary>
        ///     菜单id
        /// </summary>
        [Display(Name = "菜单id")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     上级id,0是顶级
        /// </summary>
        [Display(Name = "上级id,0是顶级")]
        [Required(ErrorMessage = "请输入{0}")]
        public int parentId { get; set; }

        /// <summary>
        ///     菜单名称
        /// </summary>
        [Display(Name = "菜单名称")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string menuName { get; set; }

        /// <summary>
        ///     菜单图标
        /// </summary>
        [Display(Name = "菜单图标")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string menuIcon { get; set; }

        /// <summary>
        ///     菜单路由关键字
        /// </summary>
        [Display(Name = "菜单路由关键字")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string path { get; set; }

        /// <summary>
        ///     菜单组件地址
        /// </summary>
        [Display(Name = "菜单组件地址")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string component { get; set; }

        /// <summary>
        ///     类型,0菜单,1按钮
        /// </summary>
        [Display(Name = "类型,0菜单,1按钮")]
        [Required(ErrorMessage = "请输入{0}")]
        public int menuType { get; set; }

        /// <summary>
        ///     排序号
        /// </summary>
        [Display(Name = "排序号")]
        public int? sortNumber { get; set; }

        /// <summary>
        ///     权限标识
        /// </summary>
        [Display(Name = "权限标识")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string authority { get; set; }

        /// <summary>
        ///     打开位置
        /// </summary>
        [Display(Name = "打开位置")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string target { get; set; }

        /// <summary>
        ///     菜单图标颜色
        /// </summary>
        [Display(Name = "菜单图标颜色")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string iconColor { get; set; }

        /// <summary>
        ///     是否隐藏,0否,1是
        /// </summary>
        [Display(Name = "是否隐藏,0否,1是")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool hide { get; set; }

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
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? updateTime { get; set; }


        public bool @checked { get; set; } = false;
        public object children { get; set; }
        public bool open { get; set; } = true;

        public string parentName { get; set; }
    }
}