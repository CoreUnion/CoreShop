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
    ///     拼团记录表
    /// </summary>
    public partial class CoreCmsPinTuanRecord
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     团序列
        /// </summary>
        [Display(Name = "团序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int teamId { get; set; }

        /// <summary>
        ///     用户序列
        /// </summary>
        [Display(Name = "用户序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     规则表序列
        /// </summary>
        [Display(Name = "规则表序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int ruleId { get; set; }

        /// <summary>
        ///     商品序列
        /// </summary>
        [Display(Name = "商品序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int goodsId { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        [Display(Name = "状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int status { get; set; }

        /// <summary>
        ///     订单序列
        /// </summary>
        [Display(Name = "订单序列")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string orderId { get; set; }

        /// <summary>
        ///     拼团人数Json
        /// </summary>
        [Display(Name = "拼团人数Json")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string parameters { get; set; }

        /// <summary>
        ///     关闭时间
        /// </summary>
        [Display(Name = "关闭时间")]
        public DateTime closeTime { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime createTime { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? updateTime { get; set; }
    }
}