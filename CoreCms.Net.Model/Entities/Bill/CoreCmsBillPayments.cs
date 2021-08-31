/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统
 *                Web: https://www.corecms.net
 *             Author: 大灰灰
 *              Email: jianweie@163.com
 *         CreateTime: 2021/7/10 23:15:12
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 支付单表
    /// </summary>
    public partial class CoreCmsBillPayments
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsBillPayments()
        {
        }

        /// <summary>
        /// 支付单号
        /// </summary>
        [Display(Name = "支付单号")]
        [SugarColumn(IsPrimaryKey = true)]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 20, ErrorMessage = "{0}不能超过{1}字")]
        public System.String paymentId { get; set; }

        /// <summary>
        /// 资源编号
        /// </summary>
        [Display(Name = "资源编号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 20, ErrorMessage = "{0}不能超过{1}字")]
        public System.String sourceId { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [Display(Name = "支付金额")]
        [Required(ErrorMessage = "请输入{0}")]

        public System.Decimal money { get; set; }

        /// <summary>
        /// 用户ID 关联user.id
        /// </summary>
        [Display(Name = "用户ID 关联user.id")]
        [Required(ErrorMessage = "请输入{0}")]

        public System.Int32 userId { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        [Display(Name = "单据类型")]
        [Required(ErrorMessage = "请输入{0}")]

        public System.Int32 type { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        [Display(Name = "支付状态")]
        [Required(ErrorMessage = "请输入{0}")]

        public System.Int32 status { get; set; }

        /// <summary>
        /// 支付类型编码 关联payments.code
        /// </summary>
        [Display(Name = "支付类型编码 关联payments.code")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public System.String paymentCode { get; set; }

        /// <summary>
        /// 支付单生成IP
        /// </summary>
        [Display(Name = "支付单生成IP")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public System.String ip { get; set; }

        /// <summary>
        /// 支付的时候需要的参数，存的是json格式的一维数组
        /// </summary>
        [Display(Name = "支付的时候需要的参数，存的是json格式的一维数组")]
        [StringLength(maximumLength: 200, ErrorMessage = "{0}不能超过{1}字")]
        public System.String parameters { get; set; }

        /// <summary>
        /// 支付回调后的状态描述
        /// </summary>
        [Display(Name = "支付回调后的状态描述")]
        [StringLength(maximumLength: 255, ErrorMessage = "{0}不能超过{1}字")]
        public System.String payedMsg { get; set; }

        /// <summary>
        /// 第三方平台交易流水号
        /// </summary>
        [Display(Name = "第三方平台交易流水号")]
        [StringLength(maximumLength: 50, ErrorMessage = "{0}不能超过{1}字")]
        public System.String tradeNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]

        public System.DateTime createTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]

        public System.DateTime? updateTime { get; set; }
    }
}