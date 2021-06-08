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
    /// 标签表
    /// </summary>
    [SugarTable("CoreCmsLabel",TableDescription = "标签表")]
    public partial class CoreCmsLabel
    {
        /// <summary>
        /// 标签表
        /// </summary>
        public CoreCmsLabel()
        {
        }

        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(ColumnDescription = "序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        [Display(Name = "标签名称")]
        [SugarColumn(ColumnDescription = "标签名称", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 标签样式
        /// </summary>
        [Display(Name = "标签样式")]
        [SugarColumn(ColumnDescription = "标签样式", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String style { get; set; }
    }
}