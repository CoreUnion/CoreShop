/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     表单项表
    /// </summary>
    public partial class CoreCmsFormItem
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     字段名称
        /// </summary>
        [Display(Name = "字段名称")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     字段类型
        /// </summary>
        [Display(Name = "字段类型")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]
        public string type { get; set; }

        /// <summary>
        ///     验证类型
        /// </summary>
        [Display(Name = "验证类型")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]
        public string validationType { get; set; }

        /// <summary>
        ///     表单值
        /// </summary>
        [Display(Name = "表单值")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string value { get; set; }

        /// <summary>
        ///     默认值
        /// </summary>
        [Display(Name = "默认值")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string defaultValue { get; set; }

        /// <summary>
        ///     表单id
        /// </summary>
        [Display(Name = "表单id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int formId { get; set; }

        /// <summary>
        ///     是否必填
        /// </summary>
        [Display(Name = "是否必填")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool required { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }
    }
}