/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统
 *                Web: https://www.corecms.net
 *             Author: 大灰灰
 *              Email: jianweie@163.com
 *         CreateTime: 2021/6/28 22:49:48
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     接龙活动表
    /// </summary>
    public partial class CoreCmsSolitaire
    {
        /// <summary>
        ///     序列没
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     活动标题
        /// </summary>
        [Display(Name = "活动标题")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(250, ErrorMessage = "{0}不能超过{1}字")]
        public string title { get; set; }

        /// <summary>
        ///     缩略图
        /// </summary>
        [Display(Name = "缩略图")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(250, ErrorMessage = "{0}不能超过{1}字")]
        public string thumbnail { get; set; }

        /// <summary>
        ///     活动内容
        /// </summary>
        [Display(Name = "活动内容")]
        [Required(ErrorMessage = "请输入{0}")]
        public string contentBody { get; set; }

        /// <summary>
        ///     开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime startTime { get; set; }

        /// <summary>
        ///     结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime endTime { get; set; }

        /// <summary>
        ///     起购价格
        /// </summary>
        [Display(Name = "起购价格")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal startBuyPrice { get; set; }

        /// <summary>
        ///     起送价格
        /// </summary>
        [Display(Name = "起送价格")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal minDeliveryPrice { get; set; }

        /// <summary>
        ///     是否显示
        /// </summary>
        [Display(Name = "是否显示")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isShow { get; set; }

        /// <summary>
        ///     活动状态
        /// </summary>
        [Display(Name = "活动状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int status { get; set; }

        /// <summary>
        ///     标注删除
        /// </summary>
        [Display(Name = "标注删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDelete { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }
    }
}