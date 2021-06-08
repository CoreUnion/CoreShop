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
    /// 地区表
    /// </summary>
    [SugarTable("CoreCmsArea",TableDescription = "地区表")]
    public partial class CoreCmsArea
    {
        /// <summary>
        /// 地区表
        /// </summary>
        public CoreCmsArea()
        {
        }

        /// <summary>
        /// 地区ID
        /// </summary>
        [Display(Name = "地区ID")]
        [SugarColumn(ColumnDescription = "地区ID", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        [Display(Name = "父级ID")]
        [SugarColumn(ColumnDescription = "父级ID", IsNullable = true)]
        public System.Int32? parentId { get; set; }
        /// <summary>
        /// 地区深度
        /// </summary>
        [Display(Name = "地区深度")]
        [SugarColumn(ColumnDescription = "地区深度", IsNullable = true)]
        public System.Int32? depth { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>
        [Display(Name = "地区名称")]
        [SugarColumn(ColumnDescription = "地区名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        [Display(Name = "邮编")]
        [SugarColumn(ColumnDescription = "邮编", IsNullable = true)]
        [StringLength(10, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String postalCode { get; set; }
        /// <summary>
        /// 地区排序
        /// </summary>
        [Display(Name = "地区排序")]
        [SugarColumn(ColumnDescription = "地区排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
    }
}