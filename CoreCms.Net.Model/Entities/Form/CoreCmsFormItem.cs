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
    /// 表单项表
    /// </summary>
    [SugarTable("CoreCmsFormItem",TableDescription = "表单项表")]
    public partial class CoreCmsFormItem
    {
        /// <summary>
        /// 表单项表
        /// </summary>
        public CoreCmsFormItem()
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
        /// 字段名称
        /// </summary>
        [Display(Name = "字段名称")]
        [SugarColumn(ColumnDescription = "字段名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        [Display(Name = "字段类型")]
        [SugarColumn(ColumnDescription = "字段类型", IsNullable = true)]
        [StringLength(30, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String type { get; set; }
        /// <summary>
        /// 验证类型
        /// </summary>
        [Display(Name = "验证类型")]
        [SugarColumn(ColumnDescription = "验证类型", IsNullable = true)]
        [StringLength(30, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String validationType { get; set; }
        /// <summary>
        /// 表单值
        /// </summary>
        [Display(Name = "表单值")]
        [SugarColumn(ColumnDescription = "表单值", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String value { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        [Display(Name = "默认值")]
        [SugarColumn(ColumnDescription = "默认值", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String defaultValue { get; set; }
        /// <summary>
        /// 表单id
        /// </summary>
        [Display(Name = "表单id")]
        [SugarColumn(ColumnDescription = "表单id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 formId { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        [Display(Name = "是否必填")]
        [SugarColumn(ColumnDescription = "是否必填")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean required { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
    }
}