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
    ///     服务券核验日志
    /// </summary>
    public partial class CoreCmsUserServicesTicketVerificationLog
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     核销门店id
        /// </summary>
        [Display(Name = "核销门店id")]
        [Required(ErrorMessage = "请输入{0}")]
        public int storeId { get; set; }

        /// <summary>
        ///     关联服务
        /// </summary>
        [Display(Name = "关联服务")]
        [Required(ErrorMessage = "请输入{0}")]
        public int serviceId { get; set; }

        /// <summary>
        ///     核验人
        /// </summary>
        [Display(Name = "核验人")]
        [Required(ErrorMessage = "请输入{0}")]
        public int verificationUserId { get; set; }

        /// <summary>
        ///     服务券序列
        /// </summary>
        [Display(Name = "服务券序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public int ticketId { get; set; }

        /// <summary>
        ///     核验码
        /// </summary>
        [Display(Name = "核验码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string ticketRedeemCode { get; set; }

        /// <summary>
        ///     核验时间
        /// </summary>
        [Display(Name = "核验时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime verificationTime { get; set; }

        /// <summary>
        ///     是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isDel { get; set; }
    }
}