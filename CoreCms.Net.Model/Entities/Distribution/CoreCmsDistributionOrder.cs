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
    ///     分销商订单记录表
    /// </summary>
    public partial class CoreCmsDistributionOrder
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     用户分销商id
        /// </summary>
        [Display(Name = "用户分销商id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     下单用户id
        /// </summary>
        [Display(Name = "下单用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int buyUserId { get; set; }

        /// <summary>
        ///     订单编号
        /// </summary>
        [Display(Name = "订单编号")]
        [StringLength(25, ErrorMessage = "{0}不能超过{1}字")]
        public string orderId { get; set; }

        /// <summary>
        ///     结算金额
        /// </summary>
        [Display(Name = "结算金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal amount { get; set; }

        /// <summary>
        ///     是否结算
        /// </summary>
        [Display(Name = "是否结算")]
        [Required(ErrorMessage = "请输入{0}")]
        public int isSettlement { get; set; }

        /// <summary>
        ///     层级
        /// </summary>
        [Display(Name = "层级")]
        [Required(ErrorMessage = "请输入{0}")]
        public int level { get; set; }

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
        ///     是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDelete { get; set; }
    }
}