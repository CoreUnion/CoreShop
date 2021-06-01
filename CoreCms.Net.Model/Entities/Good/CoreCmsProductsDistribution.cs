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
    ///     货品三级佣金表
    /// </summary>
    public class CoreCmsProductsDistribution
    {
        /// <summary>
        ///     序号
        /// </summary>
        [Display(Name = "序号")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     货品序列
        /// </summary>
        [Display(Name = "货品序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int productsId { get; set; }

        /// <summary>
        ///     货品货号
        /// </summary>
        [Display(Name = "货品货号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string productsSN { get; set; }

        /// <summary>
        ///     一级佣金
        /// </summary>
        [Display(Name = "一级佣金")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal levelOne { get; set; }

        /// <summary>
        ///     二级佣金
        /// </summary>
        [Display(Name = "二级佣金")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal levelTwo { get; set; }

        /// <summary>
        ///     三级佣金
        /// </summary>
        [Display(Name = "三级佣金")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal levelThree { get; set; }

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