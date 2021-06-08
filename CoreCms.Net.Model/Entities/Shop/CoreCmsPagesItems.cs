/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:58
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 单页内容
    /// </summary>
    [SugarTable("CoreCmsPagesItems",TableDescription = "单页内容")]
    public partial class CoreCmsPagesItems
    {
        /// <summary>
        /// 单页内容
        /// </summary>
        public CoreCmsPagesItems()
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
        /// 组件编码
        /// </summary>
        [Display(Name = "组件编码")]
        [SugarColumn(ColumnDescription = "组件编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String widgetCode { get; set; }
        /// <summary>
        /// 页面编码
        /// </summary>
        [Display(Name = "页面编码")]
        [SugarColumn(ColumnDescription = "页面编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String pageCode { get; set; }
        /// <summary>
        /// 布局位置
        /// </summary>
        [Display(Name = "布局位置")]
        [SugarColumn(ColumnDescription = "布局位置")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 positionId { get; set; }
        /// <summary>
        /// 排序，越小越靠前
        /// </summary>
        [Display(Name = "排序，越小越靠前")]
        [SugarColumn(ColumnDescription = "排序，越小越靠前")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
        /// <summary>
        /// 组件配置内容
        /// </summary>
        [Display(Name = "组件配置内容")]
        [SugarColumn(ColumnDescription = "组件配置内容", IsNullable = true)]
        public System.String parameters { get; set; }
    }
}