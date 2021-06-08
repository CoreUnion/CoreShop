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
    /// 发票表
    /// </summary>
    [SugarTable("CoreCmsInvoice",TableDescription = "发票表")]
    public partial class CoreCmsInvoice
    {
        /// <summary>
        /// 发票表
        /// </summary>
        public CoreCmsInvoice()
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
        /// 开票类型
        /// </summary>
        [Display(Name = "开票类型")]
        [SugarColumn(ColumnDescription = "开票类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 category { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        [Display(Name = "资源ID")]
        [SugarColumn(ColumnDescription = "资源ID", IsNullable = true)]
        [StringLength(32, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String sourceId { get; set; }
        /// <summary>
        /// 所属用户ID
        /// </summary>
        [Display(Name = "所属用户ID")]
        [SugarColumn(ColumnDescription = "所属用户ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 发票类型
        /// </summary>
        [Display(Name = "发票类型")]
        [SugarColumn(ColumnDescription = "发票类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 type { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        [Display(Name = "发票抬头")]
        [SugarColumn(ColumnDescription = "发票抬头")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String title { get; set; }
        /// <summary>
        /// 发票税号
        /// </summary>
        [Display(Name = "发票税号")]
        [SugarColumn(ColumnDescription = "发票税号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(32, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String taxNumber { get; set; }
        /// <summary>
        /// 发票金额
        /// </summary>
        [Display(Name = "发票金额")]
        [SugarColumn(ColumnDescription = "发票金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal amount { get; set; }
        /// <summary>
        /// 开票状态
        /// </summary>
        [Display(Name = "开票状态")]
        [SugarColumn(ColumnDescription = "开票状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 status { get; set; }
        /// <summary>
        /// 开票备注
        /// </summary>
        [Display(Name = "开票备注")]
        [SugarColumn(ColumnDescription = "开票备注", IsNullable = true)]
        [StringLength(2000, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String remarks { get; set; }
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
    }
}