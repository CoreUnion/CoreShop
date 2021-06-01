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
    ///     登录日志表
    /// </summary>
    public class SysLoginRecord
    {
        /// <summary>
        ///     主键
        /// </summary>
        [Display(Name = "主键")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     用户账号
        /// </summary>
        [Display(Name = "用户账号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string username { get; set; }

        /// <summary>
        ///     操作系统
        /// </summary>
        [Display(Name = "操作系统")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string os { get; set; }

        /// <summary>
        ///     设备名
        /// </summary>
        [Display(Name = "设备名")]
        [StringLength(100, ErrorMessage = "{0}不能超过{1}字")]
        public string device { get; set; }

        /// <summary>
        ///     浏览器类型
        /// </summary>
        [Display(Name = "浏览器类型")]
        [StringLength(250, ErrorMessage = "{0}不能超过{1}字")]
        public string browser { get; set; }

        /// <summary>
        ///     ip地址
        /// </summary>
        [Display(Name = "ip地址")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string ip { get; set; }

        /// <summary>
        ///     操作类型
        /// </summary>
        [Display(Name = "操作类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int operType { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string comments { get; set; }

        /// <summary>
        ///     登录时间
        /// </summary>
        [Display(Name = "登录时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }

        /// <summary>
        ///     修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        public DateTime? updateTime { get; set; }
    }
}