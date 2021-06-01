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
    ///     短信发送日志
    /// </summary>
    public class CoreCmsSms
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(15, ErrorMessage = "{0}不能超过{1}字")]
        public string mobile { get; set; }

        /// <summary>
        ///     发送编码
        /// </summary>
        [Display(Name = "发送编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(60, ErrorMessage = "{0}不能超过{1}字")]
        public string code { get; set; }

        /// <summary>
        ///     参数
        /// </summary>
        [Display(Name = "参数")]
        [Required(ErrorMessage = "请输入{0}")]
        public string parameters { get; set; }

        /// <summary>
        ///     内容
        /// </summary>
        [Display(Name = "内容")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "{0}不能超过{1}字")]
        public string contentBody { get; set; }

        /// <summary>
        ///     ip
        /// </summary>
        [Display(Name = "ip")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string ip { get; set; }

        /// <summary>
        ///     是否使用
        /// </summary>
        [Display(Name = "是否使用")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isUsed { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }
    }
}