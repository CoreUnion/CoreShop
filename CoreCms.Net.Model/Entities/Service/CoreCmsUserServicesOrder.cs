using System;
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     服务购买表
    /// </summary>
    public partial class CoreCmsUserServicesOrder
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     服务订单编号
        /// </summary>
        [Display(Name = "服务订单编号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string serviceOrderId { get; set; }

        /// <summary>
        ///     关联用户
        /// </summary>
        [Display(Name = "关联用户")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     关联服务
        /// </summary>
        [Display(Name = "关联服务")]
        [Required(ErrorMessage = "请输入{0}")]
        public int servicesId { get; set; }

        /// <summary>
        ///     是否支付
        /// </summary>
        [Display(Name = "是否支付")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isPay { get; set; }

        /// <summary>
        ///     支付时间
        /// </summary>
        [Display(Name = "支付时间")]
        public DateTime? payTime { get; set; }

        /// <summary>
        ///     支付单号
        /// </summary>
        [Display(Name = "支付单号")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string paymentId { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        [Display(Name = "状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public int status { get; set; }

        /// <summary>
        ///     订单创建时间
        /// </summary>
        [Display(Name = "订单创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }

        /// <summary>
        ///     截止服务时间
        /// </summary>
        [Display(Name = "截止服务时间")]
        public DateTime? servicesEndTime { get; set; }
    }
}