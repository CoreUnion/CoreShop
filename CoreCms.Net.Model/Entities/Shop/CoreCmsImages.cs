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
    /// 图片表
    /// </summary>
    [SugarTable("CoreCmsImages",TableDescription = "图片表")]
    public partial class CoreCmsImages
    {
        /// <summary>
        /// 图片表
        /// </summary>
        public CoreCmsImages()
        {
        }

        /// <summary>
        /// 图片ID
        /// </summary>
        [Display(Name = "图片ID")]
        [SugarColumn(ColumnDescription = "图片ID", IsPrimaryKey = true)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String id { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        [Display(Name = "图片名称")]
        [SugarColumn(ColumnDescription = "图片名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 绝对地址
        /// </summary>
        [Display(Name = "绝对地址")]
        [SugarColumn(ColumnDescription = "绝对地址", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String url { get; set; }
        /// <summary>
        /// 物理地址
        /// </summary>
        [Display(Name = "物理地址")]
        [SugarColumn(ColumnDescription = "物理地址", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String path { get; set; }
        /// <summary>
        /// 存储引擎
        /// </summary>
        [Display(Name = "存储引擎")]
        [SugarColumn(ColumnDescription = "存储引擎", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String type { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [SugarColumn(ColumnDescription = "是否删除", IsNullable = true)]
        public System.Boolean? isDel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
        [SugarColumn(ColumnDescription = "", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
    }
}