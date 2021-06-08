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
    /// 操作日志表
    /// </summary>
    [SugarTable("SysOperRecord",TableDescription = "操作日志表")]
    public partial class SysOperRecord
    {
        /// <summary>
        /// 操作日志表
        /// </summary>
        public SysOperRecord()
        {
        }

        /// <summary>
        /// 主键
        /// </summary>
        [Display(Name = "主键")]
        [SugarColumn(ColumnDescription = "主键", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        [SugarColumn(ColumnDescription = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [SugarColumn(ColumnDescription = "用户名", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String userName { get; set; }
        /// <summary>
        /// 操作模块
        /// </summary>
        [Display(Name = "操作模块")]
        [SugarColumn(ColumnDescription = "操作模块", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String model { get; set; }
        /// <summary>
        /// 操作方法
        /// </summary>
        [Display(Name = "操作方法")]
        [SugarColumn(ColumnDescription = "操作方法", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String description { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        [Display(Name = "请求地址")]
        [SugarColumn(ColumnDescription = "请求地址", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String url { get; set; }
        /// <summary>
        /// 请求方式
        /// </summary>
        [Display(Name = "请求方式")]
        [SugarColumn(ColumnDescription = "请求方式", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String requestMethod { get; set; }
        /// <summary>
        /// 调用方法
        /// </summary>
        [Display(Name = "调用方法")]
        [SugarColumn(ColumnDescription = "调用方法", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String operMethod { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        [Display(Name = "请求参数")]
        [SugarColumn(ColumnDescription = "请求参数", IsNullable = true)]
        public System.String param { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        [Display(Name = "返回结果")]
        [SugarColumn(ColumnDescription = "返回结果", IsNullable = true)]
        public System.String result { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        [Display(Name = "ip地址")]
        [SugarColumn(ColumnDescription = "ip地址", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String ip { get; set; }
        /// <summary>
        /// 请求耗时,单位毫秒
        /// </summary>
        [Display(Name = "请求耗时,单位毫秒")]
        [SugarColumn(ColumnDescription = "请求耗时,单位毫秒")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String spendTime { get; set; }
        /// <summary>
        /// 状态,0成功,1异常
        /// </summary>
        [Display(Name = "状态,0成功,1异常")]
        [SugarColumn(ColumnDescription = "状态,0成功,1异常")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 state { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        [Display(Name = "登录时间")]
        [SugarColumn(ColumnDescription = "登录时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
    }
}