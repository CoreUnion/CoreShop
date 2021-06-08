/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:59
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 服务券核验日志
    /// </summary>
    [SugarTable("CoreCmsUserServicesTicketVerificationLog",TableDescription = "服务券核验日志")]
    public partial class CoreCmsUserServicesTicketVerificationLog
    {
        /// <summary>
        /// 服务券核验日志
        /// </summary>
        public CoreCmsUserServicesTicketVerificationLog()
        {
        }

        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(ColumnDescription = "序列", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 核销门店id
        /// </summary>
        [Display(Name = "核销门店id")]
        [SugarColumn(ColumnDescription = "核销门店id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 storeId { get; set; }
        /// <summary>
        /// 关联服务
        /// </summary>
        [Display(Name = "关联服务")]
        [SugarColumn(ColumnDescription = "关联服务")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 serviceId { get; set; }
        /// <summary>
        /// 核验人
        /// </summary>
        [Display(Name = "核验人")]
        [SugarColumn(ColumnDescription = "核验人")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 verificationUserId { get; set; }
        /// <summary>
        /// 服务券序列
        /// </summary>
        [Display(Name = "服务券序列")]
        [SugarColumn(ColumnDescription = "服务券序列")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 ticketId { get; set; }
        /// <summary>
        /// 核验码
        /// </summary>
        [Display(Name = "核验码")]
        [SugarColumn(ColumnDescription = "核验码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String ticketRedeemCode { get; set; }
        /// <summary>
        /// 核验时间
        /// </summary>
        [Display(Name = "核验时间")]
        [SugarColumn(ColumnDescription = "核验时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime verificationTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [SugarColumn(ColumnDescription = "是否删除")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isDel { get; set; }
    }
}