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
    /// 用户提现记录表
    /// </summary>
    [SugarTable("CoreCmsUserTocash",TableDescription = "用户提现记录表")]
    public partial class CoreCmsUserTocash
    {
        /// <summary>
        /// 用户提现记录表
        /// </summary>
        public CoreCmsUserTocash()
        {
        }

        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
        [SugarColumn(ColumnDescription = "id", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [SugarColumn(ColumnDescription = "用户ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        [Display(Name = "提现金额")]
        [SugarColumn(ColumnDescription = "提现金额")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal money { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        [Display(Name = "银行名称")]
        [SugarColumn(ColumnDescription = "银行名称", IsNullable = true)]
        [StringLength(60, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String bankName { get; set; }
        /// <summary>
        /// 银行缩写
        /// </summary>
        [Display(Name = "银行缩写")]
        [SugarColumn(ColumnDescription = "银行缩写", IsNullable = true)]
        [StringLength(12, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String bankCode { get; set; }
        /// <summary>
        /// 账号地区ID
        /// </summary>
        [Display(Name = "账号地区ID")]
        [SugarColumn(ColumnDescription = "账号地区ID", IsNullable = true)]
        public System.Int32? bankAreaId { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        [Display(Name = "开户行")]
        [SugarColumn(ColumnDescription = "开户行", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String accountBank { get; set; }
        /// <summary>
        /// 账户名
        /// </summary>
        [Display(Name = "账户名")]
        [SugarColumn(ColumnDescription = "账户名", IsNullable = true)]
        [StringLength(60, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String accountName { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        [Display(Name = "卡号")]
        [SugarColumn(ColumnDescription = "卡号", IsNullable = true)]
        [StringLength(30, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String cardNumber { get; set; }
        /// <summary>
        /// 提现服务费
        /// </summary>
        [Display(Name = "提现服务费")]
        [SugarColumn(ColumnDescription = "提现服务费")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Decimal withdrawals { get; set; }
        /// <summary>
        /// 提现状态
        /// </summary>
        [Display(Name = "提现状态")]
        [SugarColumn(ColumnDescription = "提现状态")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 status { get; set; }
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