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
    /// 提交表单保存大文本值表
    /// </summary>
    [SugarTable("CoreCmsFormSubmitDetail",TableDescription = "提交表单保存大文本值表")]
    public partial class CoreCmsFormSubmitDetail
    {
        /// <summary>
        /// 提交表单保存大文本值表
        /// </summary>
        public CoreCmsFormSubmitDetail()
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
        /// 提交表单id
        /// </summary>
        [Display(Name = "提交表单id")]
        [SugarColumn(ColumnDescription = "提交表单id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 submitId { get; set; }
        /// <summary>
        /// 表单id
        /// </summary>
        [Display(Name = "表单id")]
        [SugarColumn(ColumnDescription = "表单id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 formId { get; set; }
        /// <summary>
        /// 表单项id
        /// </summary>
        [Display(Name = "表单项id")]
        [SugarColumn(ColumnDescription = "表单项id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 formItemId { get; set; }
        /// <summary>
        /// 表单项名称
        /// </summary>
        [Display(Name = "表单项名称")]
        [SugarColumn(ColumnDescription = "表单项名称", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String formItemName { get; set; }
        /// <summary>
        /// 表单项值
        /// </summary>
        [Display(Name = "表单项值")]
        [SugarColumn(ColumnDescription = "表单项值", IsNullable = true)]
        public System.String formItemValue { get; set; }
    }
}