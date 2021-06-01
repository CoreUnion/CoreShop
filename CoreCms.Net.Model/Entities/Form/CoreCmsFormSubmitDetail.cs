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
    ///     提交表单保存大文本值表
    /// </summary>
    public class CoreCmsFormSubmitDetail
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     提交表单id
        /// </summary>
        [Display(Name = "提交表单id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int submitId { get; set; }

        /// <summary>
        ///     表单id
        /// </summary>
        [Display(Name = "表单id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int formId { get; set; }

        /// <summary>
        ///     表单项id
        /// </summary>
        [Display(Name = "表单项id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int formItemId { get; set; }

        /// <summary>
        ///     表单项名称
        /// </summary>
        [Display(Name = "表单项名称")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string formItemName { get; set; }

        /// <summary>
        ///     表单项值
        /// </summary>
        [Display(Name = "表单项值")]
        public string formItemValue { get; set; }
    }
}