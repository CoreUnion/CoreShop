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
    ///     操作日志表
    /// </summary>
    public class SysOperRecord
    {
        /// <summary>
        ///     主键
        /// </summary>
        [Display(Name = "主键")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]


        public int id { get; set; }


        /// <summary>
        ///     用户id
        /// </summary>
        [Display(Name = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]


        public int userId { get; set; }


        /// <summary>
        ///     用户名
        /// </summary>
        [Display(Name = "用户名")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string userName { get; set; }


        /// <summary>
        ///     操作模块
        /// </summary>
        [Display(Name = "操作模块")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string model { get; set; }


        /// <summary>
        ///     操作方法
        /// </summary>
        [Display(Name = "操作方法")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string description { get; set; }


        /// <summary>
        ///     请求地址
        /// </summary>
        [Display(Name = "请求地址")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]


        public string url { get; set; }


        /// <summary>
        ///     请求方式
        /// </summary>
        [Display(Name = "请求方式")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string requestMethod { get; set; }


        /// <summary>
        ///     调用方法
        /// </summary>
        [Display(Name = "调用方法")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string operMethod { get; set; }


        /// <summary>
        ///     请求参数
        /// </summary>
        [Display(Name = "请求参数")]
        public string param { get; set; }


        /// <summary>
        ///     返回结果
        /// </summary>
        [Display(Name = "返回结果")]
        public string result { get; set; }


        /// <summary>
        ///     ip地址
        /// </summary>
        [Display(Name = "ip地址")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string ip { get; set; }


        /// <summary>
        ///     请求耗时,单位毫秒
        /// </summary>
        [Display(Name = "请求耗时,单位毫秒")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]


        public string spendTime { get; set; }


        /// <summary>
        ///     状态,0成功,1异常
        /// </summary>
        [Display(Name = "状态,0成功,1异常")]
        [Required(ErrorMessage = "请输入{0}")]


        public int state { get; set; }


        /// <summary>
        ///     登录时间
        /// </summary>
        [Display(Name = "登录时间")]
        [Required(ErrorMessage = "请输入{0}")]


        public DateTime createTime { get; set; }
    }
}