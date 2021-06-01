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
    ///     支付方式表
    /// </summary>
    public class CoreCmsPayments
    {
        /// <summary>
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     支付类型名称
        /// </summary>
        [Display(Name = "支付类型名称")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     支付类型编码
        /// </summary>
        [Display(Name = "支付类型编码")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string code { get; set; }

        /// <summary>
        ///     是否线上支付
        /// </summary>
        [Display(Name = "是否线上支付")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isOnline { get; set; }

        /// <summary>
        ///     参数
        /// </summary>
        [Display(Name = "参数")]
        public string parameters { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }

        /// <summary>
        ///     方式描述
        /// </summary>
        [Display(Name = "方式描述")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string memo { get; set; }

        /// <summary>
        ///     是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isEnable { get; set; }
    }
}