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
    ///     拼团规则表
    /// </summary>
    public partial class CoreCmsPinTuanRule
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     活动名称
        /// </summary>
        [Display(Name = "活动名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

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
        ///     人数2-10人
        /// </summary>
        [Display(Name = "人数2-10人")]
        [Required(ErrorMessage = "请输入{0}")]
        public int peopleNumber { get; set; }

        /// <summary>
        ///     单位分钟
        /// </summary>
        [Display(Name = "单位分钟")]
        [Required(ErrorMessage = "请输入{0}")]
        public int significantInterval { get; set; }

        /// <summary>
        ///     优惠金额
        /// </summary>
        [Display(Name = "优惠金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal discountAmount { get; set; }

        /// <summary>
        ///     每人限购数量
        /// </summary>
        [Display(Name = "每人限购数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public int maxNums { get; set; }

        /// <summary>
        ///     每个商品活动数量
        /// </summary>
        [Display(Name = "每个商品活动数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public int maxGoodsNums { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }

        /// <summary>
        ///     是否开启
        /// </summary>
        [Display(Name = "是否开启")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isStatusOpen { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public DateTime? createTime { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        public DateTime? updateTime { get; set; }
    }
}