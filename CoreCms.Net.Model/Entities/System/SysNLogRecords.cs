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
    ///     Nlog记录表
    /// </summary>
    public class SysNLogRecords
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]


        public int id { get; set; }


        /// <summary>
        ///     时间
        /// </summary>
        [Display(Name = "时间")]
        public DateTime? LogDate { get; set; }


        /// <summary>
        ///     级别
        /// </summary>
        [Display(Name = "级别")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string LogLevel { get; set; }


        /// <summary>
        ///     事件日志上下文
        /// </summary>
        [Display(Name = "事件日志上下文")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string LogType { get; set; }


        /// <summary>
        ///     事件标题
        /// </summary>
        [Display(Name = "事件标题")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]


        public string LogTitle { get; set; }


        /// <summary>
        ///     记录器名字
        /// </summary>
        [Display(Name = "记录器名字")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]


        public string Logger { get; set; }


        /// <summary>
        ///     消息
        /// </summary>
        [Display(Name = "消息")]
        public string Message { get; set; }


        /// <summary>
        ///     异常信息
        /// </summary>
        [Display(Name = "异常信息")]
        public string Exception { get; set; }


        /// <summary>
        ///     机器名称
        /// </summary>
        [Display(Name = "机器名称")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string MachineName { get; set; }


        /// <summary>
        ///     ip
        /// </summary>
        [Display(Name = "ip")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string MachineIp { get; set; }


        /// <summary>
        ///     请求方式
        /// </summary>
        [Display(Name = "请求方式")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string NetRequestMethod { get; set; }


        /// <summary>
        ///     请求地址
        /// </summary>
        [Display(Name = "请求地址")]
        [StringLength(500, ErrorMessage = "{0}不能超过{1}字")]


        public string NetRequestUrl { get; set; }


        /// <summary>
        ///     是否授权
        /// </summary>
        [Display(Name = "是否授权")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string NetUserIsauthenticated { get; set; }


        /// <summary>
        ///     授权类型
        /// </summary>
        [Display(Name = "授权类型")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string NetUserAuthtype { get; set; }


        /// <summary>
        ///     身份认证
        /// </summary>
        [Display(Name = "身份认证")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string NetUserIdentity { get; set; }
    }
}