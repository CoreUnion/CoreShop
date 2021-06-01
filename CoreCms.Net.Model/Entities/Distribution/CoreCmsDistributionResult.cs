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
    ///     等级佣金表
    /// </summary>
    public class CoreCmsDistributionResult
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     会员等级Id
        /// </summary>
        [Display(Name = "会员等级Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int gradeId { get; set; }

        /// <summary>
        ///     佣金编码
        /// </summary>
        [Display(Name = "佣金编码")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string code { get; set; }

        /// <summary>
        ///     佣金设置序列化参数
        /// </summary>
        [Display(Name = "佣金设置序列化参数")]
        public string parameters { get; set; }
    }
}