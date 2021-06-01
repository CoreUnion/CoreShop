/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     用户对表的提交记录
    /// </summary>
    public partial class CoreCmsFormSubmit
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     表单id
        /// </summary>
        [Display(Name = "表单id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int formId { get; set; }

        /// <summary>
        ///     表单名称
        /// </summary>
        [Display(Name = "表单名称")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string formName { get; set; }

        /// <summary>
        ///     会员id
        /// </summary>
        [Display(Name = "会员id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     总金额
        /// </summary>
        [Display(Name = "总金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal money { get; set; }

        /// <summary>
        ///     是否支付
        /// </summary>
        [Display(Name = "是否支付")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool payStatus { get; set; }

        /// <summary>
        ///     是否处理
        /// </summary>
        [Display(Name = "是否处理")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool status { get; set; }

        /// <summary>
        ///     表单反馈
        /// </summary>
        [Display(Name = "表单反馈")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string feedback { get; set; }

        /// <summary>
        ///     提交人ip
        /// </summary>
        [Display(Name = "提交人ip")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string ip { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? updateTime { get; set; }
    }
}