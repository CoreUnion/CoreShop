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
    ///     退货单表
    /// </summary>
    public partial class CoreCmsBillReship
    {
        /// <summary>
        ///     退货单号
        /// </summary>
        [Display(Name = "退货单号")]
        [SugarColumn(IsPrimaryKey = true)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string reshipId { get; set; }

        /// <summary>
        ///     订单序列
        /// </summary>
        [Display(Name = "订单序列")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string orderId { get; set; }

        /// <summary>
        ///     售后单序列
        /// </summary>
        [Display(Name = "售后单序列")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string aftersalesId { get; set; }

        /// <summary>
        ///     用户ID 关联user.id
        /// </summary>
        [Display(Name = "用户ID 关联user.id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     物流公司编码
        /// </summary>
        [Display(Name = "物流公司编码")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string logiCode { get; set; }

        /// <summary>
        ///     物流单号
        /// </summary>
        [Display(Name = "物流单号")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string logiNo { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        [Display(Name = "状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int status { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string memo { get; set; }

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