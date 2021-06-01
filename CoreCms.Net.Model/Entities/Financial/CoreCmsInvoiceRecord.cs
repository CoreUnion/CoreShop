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
    ///     发票信息记录
    /// </summary>
    public class CoreCmsInvoiceRecord
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     发票抬头
        /// </summary>
        [Display(Name = "发票抬头")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(80, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     发票税号
        /// </summary>
        [Display(Name = "发票税号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]
        public string code { get; set; }

        /// <summary>
        ///     被使用次数
        /// </summary>
        [Display(Name = "被使用次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public int frequency { get; set; }
    }
}