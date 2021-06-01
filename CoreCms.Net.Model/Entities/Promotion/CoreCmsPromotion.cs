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
    ///     促销表
    /// </summary>
    public partial class CoreCmsPromotion
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     促销名称
        /// </summary>
        [Display(Name = "促销名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     类型
        /// </summary>
        [Display(Name = "类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int type { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sort { get; set; }

        /// <summary>
        ///     其它参数
        /// </summary>
        [Display(Name = "其它参数")]
        public string parameters { get; set; }

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
        ///     最大领取数量
        /// </summary>
        [Display(Name = "最大领取数量")]
        [Required(ErrorMessage = "请输入{0}")]
        public int maxRecevieNums { get; set; }

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
        ///     是否排他
        /// </summary>
        [Display(Name = "是否排他")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isExclusive { get; set; }

        /// <summary>
        ///     是否自动领取
        /// </summary>
        [Display(Name = "是否自动领取")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isAutoReceive { get; set; }

        /// <summary>
        ///     是否开启
        /// </summary>
        [Display(Name = "是否开启")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isEnable { get; set; }

        /// <summary>
        ///     是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDel { get; set; }

        /// <summary>
        ///     有效天数
        /// </summary>
        [Display(Name = "有效天数")]
        [Required(ErrorMessage = "请输入{0}")]
        public int effectiveDays { get; set; }

        /// <summary>
        ///     有效小时
        /// </summary>
        [Display(Name = "有效小时")]
        [Required(ErrorMessage = "请输入{0}")]
        public int effectiveHours { get; set; }
    }
}