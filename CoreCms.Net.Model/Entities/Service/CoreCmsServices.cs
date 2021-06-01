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
    ///     服务项目表
    /// </summary>
    public partial class CoreCmsServices
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     项目名称
        /// </summary>
        [Display(Name = "项目名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string title { get; set; }

        /// <summary>
        ///     项目缩略图
        /// </summary>
        [Display(Name = "项目缩略图")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string thumbnail { get; set; }

        /// <summary>
        ///     项目概述
        /// </summary>
        [Display(Name = "项目概述")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string description { get; set; }

        /// <summary>
        ///     项目详细说明
        /// </summary>
        [Display(Name = "项目详细说明")]
        [Required(ErrorMessage = "请输入{0}")]
        public string contentBody { get; set; }

        /// <summary>
        ///     允许购买会员级别
        /// </summary>
        [Display(Name = "允许购买会员级别")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string allowedMembership { get; set; }

        /// <summary>
        ///     可消费门店
        /// </summary>
        [Display(Name = "可消费门店")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string consumableStore { get; set; }

        /// <summary>
        ///     项目状态
        /// </summary>
        [Display(Name = "项目状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int status { get; set; }

        /// <summary>
        ///     项目重复购买次数
        /// </summary>
        [Display(Name = "项目重复购买次数")]
        [Required(ErrorMessage = "请输入{0}")]
        public int maxBuyNumber { get; set; }

        /// <summary>
        ///     项目可销售数量
        /// </summary>
        [Display(Name = "项目可销售数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public int amount { get; set; }

        /// <summary>
        ///     项目开始时间
        /// </summary>
        [Display(Name = "项目开始时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime startTime { get; set; }

        /// <summary>
        ///     项目截止时间
        /// </summary>
        [Display(Name = "项目截止时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime endTime { get; set; }

        /// <summary>
        ///     核销有效期类型
        /// </summary>
        [Display(Name = "核销有效期类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int validityType { get; set; }

        /// <summary>
        ///     核销开始时间
        /// </summary>
        [Display(Name = "核销开始时间")]
        public DateTime? validityStartTime { get; set; }

        /// <summary>
        ///     核销结束时间
        /// </summary>
        [Display(Name = "核销结束时间")]
        public DateTime? validityEndTime { get; set; }

        /// <summary>
        ///     核销服务券数量
        /// </summary>
        [Display(Name = "核销服务券数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public int ticketNumber { get; set; }

        /// <summary>
        ///     项目创建时间
        /// </summary>
        [Display(Name = "项目创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }

        /// <summary>
        ///     项目更新时间
        /// </summary>
        [Display(Name = "项目更新时间")]
        public DateTime? updateTime { get; set; }

        /// <summary>
        ///     售价
        /// </summary>
        [Display(Name = "售价")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal money { get; set; }
    }
}