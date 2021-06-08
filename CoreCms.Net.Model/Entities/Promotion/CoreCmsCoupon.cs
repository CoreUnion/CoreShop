/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:58
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 优惠券表
    /// </summary>
    [SugarTable("CoreCmsCoupon",TableDescription = "优惠券表")]
    public partial class CoreCmsCoupon
    {
        /// <summary>
        /// 优惠券表
        /// </summary>
        public CoreCmsCoupon()
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
        /// 优惠券编码
        /// </summary>
        [Display(Name = "优惠券编码")]
        [SugarColumn(ColumnDescription = "优惠券编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String couponCode { get; set; }
        /// <summary>
        /// 优惠券id
        /// </summary>
        [Display(Name = "优惠券id")]
        [SugarColumn(ColumnDescription = "优惠券id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 promotionId { get; set; }
        /// <summary>
        /// 是否使用
        /// </summary>
        [Display(Name = "是否使用")]
        [SugarColumn(ColumnDescription = "是否使用")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isUsed { get; set; }
        /// <summary>
        /// 谁领取了
        /// </summary>
        [Display(Name = "谁领取了")]
        [SugarColumn(ColumnDescription = "谁领取了")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 被谁用了
        /// </summary>
        [Display(Name = "被谁用了")]
        [SugarColumn(ColumnDescription = "被谁用了", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String usedId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        [Display(Name = "说明")]
        [SugarColumn(ColumnDescription = "说明", IsNullable = true)]
        [StringLength(500, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String remark { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        [SugarColumn(ColumnDescription = "开始时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        [SugarColumn(ColumnDescription = "结束时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime endTime { get; set; }
    }
}