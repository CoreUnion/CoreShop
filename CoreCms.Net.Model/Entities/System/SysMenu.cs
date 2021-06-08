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
    /// 菜单表
    /// </summary>
    [SugarTable("SysMenu",TableDescription = "菜单表")]
    public partial class SysMenu
    {
        /// <summary>
        /// 菜单表
        /// </summary>
        public SysMenu()
        {
        }

        /// <summary>
        /// 菜单id
        /// </summary>
        [Display(Name = "菜单id")]
        [SugarColumn(ColumnDescription = "菜单id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 上级id,0是顶级
        /// </summary>
        [Display(Name = "上级id,0是顶级")]
        [SugarColumn(ColumnDescription = "上级id,0是顶级")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 parentId { get; set; }
        /// <summary>
        /// 英文标识符
        /// </summary>
        [Display(Name = "英文标识符")]
        [SugarColumn(ColumnDescription = "英文标识符", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String identificationCode { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Display(Name = "菜单名称")]
        [SugarColumn(ColumnDescription = "菜单名称", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String menuName { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        [Display(Name = "菜单图标")]
        [SugarColumn(ColumnDescription = "菜单图标", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String menuIcon { get; set; }
        /// <summary>
        /// 菜单路由关键字
        /// </summary>
        [Display(Name = "菜单路由关键字")]
        [SugarColumn(ColumnDescription = "菜单路由关键字", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String path { get; set; }
        /// <summary>
        /// 菜单组件地址
        /// </summary>
        [Display(Name = "菜单组件地址")]
        [SugarColumn(ColumnDescription = "菜单组件地址", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String component { get; set; }
        /// <summary>
        /// 类型,0菜单,1按钮
        /// </summary>
        [Display(Name = "类型,0菜单,1按钮")]
        [SugarColumn(ColumnDescription = "类型,0菜单,1按钮")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 menuType { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        [SugarColumn(ColumnDescription = "排序号", IsNullable = true)]
        public System.Int32? sortNumber { get; set; }
        /// <summary>
        /// 权限标识
        /// </summary>
        [Display(Name = "权限标识")]
        [SugarColumn(ColumnDescription = "权限标识", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String authority { get; set; }
        /// <summary>
        /// 打开位置
        /// </summary>
        [Display(Name = "打开位置")]
        [SugarColumn(ColumnDescription = "打开位置", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String target { get; set; }
        /// <summary>
        /// 菜单图标颜色
        /// </summary>
        [Display(Name = "菜单图标颜色")]
        [SugarColumn(ColumnDescription = "菜单图标颜色", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String iconColor { get; set; }
        /// <summary>
        /// 是否隐藏,0否,1是
        /// </summary>
        [Display(Name = "是否隐藏,0否,1是")]
        [SugarColumn(ColumnDescription = "是否隐藏,0否,1是")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean hide { get; set; }
        /// <summary>
        /// 是否删除,0否,1是
        /// </summary>
        [Display(Name = "是否删除,0否,1是")]
        [SugarColumn(ColumnDescription = "是否删除,0否,1是")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean deleted { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}