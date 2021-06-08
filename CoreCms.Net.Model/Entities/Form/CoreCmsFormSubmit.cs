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
    /// 用户对表的提交记录
    /// </summary>
    [SugarTable("CoreCmsFormSubmit",TableDescription = "用户对表的提交记录")]
    public partial class CoreCmsFormSubmit
    {
        /// <summary>
        /// 用户对表的提交记录
        /// </summary>
        public CoreCmsFormSubmit()
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
        /// 表单id
        /// </summary>
        [Display(Name = "表单id")]
        [SugarColumn(ColumnDescription = "表单id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 formId { get; set; }
        /// <summary>
        /// 表单名称
        /// </summary>
        [Display(Name = "表单名称")]
        [SugarColumn(ColumnDescription = "表单名称", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String formName { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        [Display(Name = "会员id")]
        [SugarColumn(ColumnDescription = "会员id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        [Display(Name = "总金额")]
        [SugarColumn(ColumnDescription = "总金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal money { get; set; }
        /// <summary>
        /// 是否支付
        /// </summary>
        [Display(Name = "是否支付")]
        [SugarColumn(ColumnDescription = "是否支付")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean payStatus { get; set; }
        /// <summary>
        /// 是否处理
        /// </summary>
        [Display(Name = "是否处理")]
        [SugarColumn(ColumnDescription = "是否处理")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean status { get; set; }
        /// <summary>
        /// 表单反馈
        /// </summary>
        [Display(Name = "表单反馈")]
        [SugarColumn(ColumnDescription = "表单反馈", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String feedback { get; set; }
        /// <summary>
        /// 提交人ip
        /// </summary>
        [Display(Name = "提交人ip")]
        [SugarColumn(ColumnDescription = "提交人ip", IsNullable = true)]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String ip { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}