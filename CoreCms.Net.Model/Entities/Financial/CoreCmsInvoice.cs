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
    ///     发票表
    /// </summary>
    public partial class CoreCmsInvoice
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     开票类型
        /// </summary>
        [Display(Name = "开票类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int category { get; set; }

        /// <summary>
        ///     资源ID
        /// </summary>
        [Display(Name = "资源ID")]
        [StringLength(32, ErrorMessage = "{0}不能超过{1}字")]
        public string sourceId { get; set; }

        /// <summary>
        ///     所属用户ID
        /// </summary>
        [Display(Name = "所属用户ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     发票类型
        /// </summary>
        [Display(Name = "发票类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int type { get; set; }

        /// <summary>
        ///     发票抬头
        /// </summary>
        [Display(Name = "发票抬头")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string title { get; set; }

        /// <summary>
        ///     发票税号
        /// </summary>
        [Display(Name = "发票税号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(32, ErrorMessage = "{0}不能超过{1}字")]
        public string taxNumber { get; set; }

        /// <summary>
        ///     发票金额
        /// </summary>
        [Display(Name = "发票金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal amount { get; set; }

        /// <summary>
        ///     开票状态
        /// </summary>
        [Display(Name = "开票状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int status { get; set; }

        /// <summary>
        ///     开票备注
        /// </summary>
        [Display(Name = "开票备注")]
        [StringLength(2000, ErrorMessage = "{0}不能超过{1}字")]
        public string remarks { get; set; }

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