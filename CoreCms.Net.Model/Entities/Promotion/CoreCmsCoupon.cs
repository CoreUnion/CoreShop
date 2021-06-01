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
    ///     优惠券表
    /// </summary>
    public partial class CoreCmsCoupon
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     优惠券编码
        /// </summary>
        [Display(Name = "优惠券编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(10, ErrorMessage = "{0}不能超过{1}字")]
        public string couponCode { get; set; }

        /// <summary>
        ///     优惠券id
        /// </summary>
        [Display(Name = "优惠券id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int promotionId { get; set; }

        /// <summary>
        ///     是否使用
        /// </summary>
        [Display(Name = "是否使用")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isUsed { get; set; }

        /// <summary>
        ///     谁领取了
        /// </summary>
        [Display(Name = "谁领取了")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     被谁用了
        /// </summary>
        [Display(Name = "被谁用了")]
        [StringLength(25, ErrorMessage = "{0}不能超过{1}字")]
        public string usedId { get; set; }

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

        /// <summary>
        ///     说明
        /// </summary>
        [Display(Name = "说明")]
        [StringLength(250, ErrorMessage = "{0}不能超过{1}字")]
        public string remark { get; set; }

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
    }
}