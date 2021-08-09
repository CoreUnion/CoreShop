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
    /// 单页
    /// </summary>
    [SugarTable("CoreCmsPages",TableDescription = "单页")]
    public partial class CoreCmsPages
    {
        /// <summary>
        /// 单页
        /// </summary>
        public CoreCmsPages()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(ColumnDescription = "", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 可视化区域编码
        /// </summary>
        [Display(Name = "可视化区域编码")]
        [SugarColumn(ColumnDescription = "可视化区域编码", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String code { get; set; }
        /// <summary>
        /// 可编辑区域名称
        /// </summary>
        [Display(Name = "可编辑区域名称")]
        [SugarColumn(ColumnDescription = "可编辑区域名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [SugarColumn(ColumnDescription = "描述", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String description { get; set; }
        /// <summary>
        /// 布局样式编码，1，手机端
        /// </summary>
        [Display(Name = "布局样式编码，1，手机端")]
        [SugarColumn(ColumnDescription = "布局样式编码，1，手机端", IsNullable = true)]
        public System.Int32? layout { get; set; }
        /// <summary>
        /// 1手机端，2PC端
        /// </summary>
        [Display(Name = "1手机端，2PC端")]
        [SugarColumn(ColumnDescription = "1手机端，2PC端", IsNullable = true)]
        public System.Int32? type { get; set; }
    }
}