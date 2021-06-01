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
    ///     用户余额表
    /// </summary>
    public partial class CoreCmsUserBalance
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     用户id
        /// </summary>
        [Display(Name = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     类型
        /// </summary>
        [Display(Name = "类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int type { get; set; }

        /// <summary>
        ///     金额
        /// </summary>
        [Display(Name = "金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal money { get; set; }

        /// <summary>
        ///     余额
        /// </summary>
        [Display(Name = "余额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal balance { get; set; }

        /// <summary>
        ///     资源id
        /// </summary>
        [Display(Name = "资源id")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string sourceId { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        [Display(Name = "描述")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string memo { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }
    }
}