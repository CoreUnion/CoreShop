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
    ///     用户提现记录表
    /// </summary>
    public partial class CoreCmsUserTocash
    {
        /// <summary>
        ///     id
        /// </summary>
        [Display(Name = "id")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     提现金额
        /// </summary>
        [Display(Name = "提现金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal money { get; set; }

        /// <summary>
        ///     银行名称
        /// </summary>
        [Display(Name = "银行名称")]
        [StringLength(60, ErrorMessage = "{0}不能超过{1}字")]
        public string bankName { get; set; }

        /// <summary>
        ///     银行缩写
        /// </summary>
        [Display(Name = "银行缩写")]
        [StringLength(12, ErrorMessage = "{0}不能超过{1}字")]
        public string bankCode { get; set; }

        /// <summary>
        ///     账号地区ID
        /// </summary>
        [Display(Name = "账号地区ID")]
        public int? bankAreaId { get; set; }

        /// <summary>
        ///     开户行
        /// </summary>
        [Display(Name = "开户行")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string accountBank { get; set; }

        /// <summary>
        ///     账户名
        /// </summary>
        [Display(Name = "账户名")]
        [StringLength(60, ErrorMessage = "{0}不能超过{1}字")]
        public string accountName { get; set; }

        /// <summary>
        ///     卡号
        /// </summary>
        [Display(Name = "卡号")]
        [StringLength(30, ErrorMessage = "{0}不能超过{1}字")]
        public string cardNumber { get; set; }

        /// <summary>
        ///     提现服务费
        /// </summary>
        [Display(Name = "提现服务费")]
        [Required(ErrorMessage = "请输入{0}")]
        public decimal withdrawals { get; set; }

        /// <summary>
        ///     提现状态
        /// </summary>
        [Display(Name = "提现状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int status { get; set; }

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