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
    ///     分销商表
    /// </summary>
    public partial class CoreCmsDistribution
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     分销商名称
        /// </summary>
        [Display(Name = "分销商名称")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     分销等级
        /// </summary>
        [Display(Name = "分销等级")]
        [Required(ErrorMessage = "请输入{0}")]
        public int gradeId { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        [Display(Name = "手机号")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string mobile { get; set; }

        /// <summary>
        ///     微信号
        /// </summary>
        [Display(Name = "微信号")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string weixin { get; set; }

        /// <summary>
        ///     qq号
        /// </summary>
        [Display(Name = "qq号")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string qq { get; set; }

        /// <summary>
        ///     店铺名称
        /// </summary>
        [Display(Name = "店铺名称")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string storeName { get; set; }

        /// <summary>
        ///     店铺Logo
        /// </summary>
        [Display(Name = "店铺Logo")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string storeLogo { get; set; }

        /// <summary>
        ///     店铺Banner
        /// </summary>
        [Display(Name = "店铺Banner")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string storeBanner { get; set; }

        /// <summary>
        ///     店铺简介
        /// </summary>
        [Display(Name = "店铺简介")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string storeDesc { get; set; }

        /// <summary>
        ///     审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int verifyStatus { get; set; }

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
        ///     审核时间
        /// </summary>
        [Display(Name = "审核时间")]
        public DateTime? verifyTime { get; set; }

        /// <summary>
        ///     是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDelete { get; set; }
    }
}