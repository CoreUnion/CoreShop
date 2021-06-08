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
    /// 服务消费券
    /// </summary>
    [SugarTable("CoreCmsUserServicesTicket",TableDescription = "服务消费券")]
    public partial class CoreCmsUserServicesTicket
    {
        /// <summary>
        /// 服务消费券
        /// </summary>
        public CoreCmsUserServicesTicket()
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
        /// 关联购买订单
        /// </summary>
        [Display(Name = "关联购买订单")]
        [SugarColumn(ColumnDescription = "关联购买订单")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String serviceOrderId { get; set; }
        /// <summary>
        /// 安全码
        /// </summary>
        [Display(Name = "安全码")]
        [SugarColumn(ColumnDescription = "安全码")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Guid securityCode { get; set; }
        /// <summary>
        /// 兑换码
        /// </summary>
        [Display(Name = "兑换码")]
        [SugarColumn(ColumnDescription = "兑换码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String redeemCode { get; set; }
        /// <summary>
        /// 关联服务项目id
        /// </summary>
        [Display(Name = "关联服务项目id")]
        [SugarColumn(ColumnDescription = "关联服务项目id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 serviceId { get; set; }
        /// <summary>
        /// 关联用户id
        /// </summary>
        [Display(Name = "关联用户id")]
        [SugarColumn(ColumnDescription = "关联用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        [SugarColumn(ColumnDescription = "状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 status { get; set; }
        /// <summary>
        /// 核销有效期类型
        /// </summary>
        [Display(Name = "核销有效期类型")]
        [SugarColumn(ColumnDescription = "核销有效期类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 validityType { get; set; }
        /// <summary>
        /// 核销开始时间
        /// </summary>
        [Display(Name = "核销开始时间")]
        [SugarColumn(ColumnDescription = "核销开始时间", IsNullable = true)]
        public System.DateTime? validityStartTime { get; set; }
        /// <summary>
        /// 核销结束时间
        /// </summary>
        [Display(Name = "核销结束时间")]
        [SugarColumn(ColumnDescription = "核销结束时间", IsNullable = true)]
        public System.DateTime? validityEndTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 是否核销
        /// </summary>
        [Display(Name = "是否核销")]
        [SugarColumn(ColumnDescription = "是否核销")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isVerification { get; set; }
        /// <summary>
        /// 核销时间
        /// </summary>
        [Display(Name = "核销时间")]
        [SugarColumn(ColumnDescription = "核销时间", IsNullable = true)]
        public System.DateTime? verificationTime { get; set; }
    }
}