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
    /// Nlog记录表
    /// </summary>
    [SugarTable("SysNLogRecords",TableDescription = "Nlog记录表")]
    public partial class SysNLogRecords
    {
        /// <summary>
        /// Nlog记录表
        /// </summary>
        public SysNLogRecords()
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
        /// 时间
        /// </summary>
        [Display(Name = "时间")]
        [SugarColumn(ColumnDescription = "时间", IsNullable = true)]
        public System.DateTime? LogDate { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        [Display(Name = "级别")]
        [SugarColumn(ColumnDescription = "级别", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String LogLevel { get; set; }
        /// <summary>
        /// 事件日志上下文
        /// </summary>
        [Display(Name = "事件日志上下文")]
        [SugarColumn(ColumnDescription = "事件日志上下文", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String LogType { get; set; }
        /// <summary>
        /// 事件标题
        /// </summary>
        [Display(Name = "事件标题")]
        [SugarColumn(ColumnDescription = "事件标题", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String LogTitle { get; set; }
        /// <summary>
        /// 记录器名字
        /// </summary>
        [Display(Name = "记录器名字")]
        [SugarColumn(ColumnDescription = "记录器名字", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String Logger { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        [Display(Name = "消息")]
        [SugarColumn(ColumnDescription = "消息", IsNullable = true)]
        public System.String Message { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        [Display(Name = "异常信息")]
        [SugarColumn(ColumnDescription = "异常信息", IsNullable = true)]
        public System.String Exception { get; set; }
        /// <summary>
        /// 机器名称
        /// </summary>
        [Display(Name = "机器名称")]
        [SugarColumn(ColumnDescription = "机器名称", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String MachineName { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        [Display(Name = "ip")]
        [SugarColumn(ColumnDescription = "ip", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String MachineIp { get; set; }
        /// <summary>
        /// 请求方式
        /// </summary>
        [Display(Name = "请求方式")]
        [SugarColumn(ColumnDescription = "请求方式", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String NetRequestMethod { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        [Display(Name = "请求地址")]
        [SugarColumn(ColumnDescription = "请求地址", IsNullable = true)]
        [StringLength(500, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String NetRequestUrl { get; set; }
        /// <summary>
        /// 是否授权
        /// </summary>
        [Display(Name = "是否授权")]
        [SugarColumn(ColumnDescription = "是否授权", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String NetUserIsauthenticated { get; set; }
        /// <summary>
        /// 授权类型
        /// </summary>
        [Display(Name = "授权类型")]
        [SugarColumn(ColumnDescription = "授权类型", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String NetUserAuthtype { get; set; }
        /// <summary>
        /// 身份认证
        /// </summary>
        [Display(Name = "身份认证")]
        [SugarColumn(ColumnDescription = "身份认证", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String NetUserIdentity { get; set; }
    }
}