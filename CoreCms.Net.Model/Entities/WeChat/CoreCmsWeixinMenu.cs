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
    /// 微信公众号菜单表
    /// </summary>
    [SugarTable("CoreCmsWeixinMenu",TableDescription = "微信公众号菜单表")]
    public partial class CoreCmsWeixinMenu
    {
        /// <summary>
        /// 微信公众号菜单表
        /// </summary>
        public CoreCmsWeixinMenu()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
        [SugarColumn(ColumnDescription = "", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 菜单id
        /// </summary>
        [Display(Name = "菜单id")]
        [SugarColumn(ColumnDescription = "菜单id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 menuId { get; set; }
        /// <summary>
        /// 父级菜单
        /// </summary>
        [Display(Name = "父级菜单")]
        [SugarColumn(ColumnDescription = "父级菜单")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 parentId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Display(Name = "菜单名称")]
        [SugarColumn(ColumnDescription = "菜单名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 菜单类型
        /// </summary>
        [Display(Name = "菜单类型")]
        [SugarColumn(ColumnDescription = "菜单类型")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(11, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String type { get; set; }
        /// <summary>
        /// 菜单参数
        /// </summary>
        [Display(Name = "菜单参数")]
        [SugarColumn(ColumnDescription = "菜单参数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.String parameters { get; set; }
    }
}