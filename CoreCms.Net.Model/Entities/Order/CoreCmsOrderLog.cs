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
    ///     订单记录表
    /// </summary>
    public partial class CoreCmsOrderLog
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     订单ID
        /// </summary>
        [Display(Name = "订单ID")]
        [StringLength(20, ErrorMessage = "{0}不能超过{1}字")]
        public string orderId { get; set; }

        /// <summary>
        ///     用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [Required(ErrorMessage = "请输入{0}")]
        public int userId { get; set; }

        /// <summary>
        ///     类型
        /// </summary>
        [Display(Name = "类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public int type { get; set; }

        /// <summary>
        ///     描述介绍
        /// </summary>
        [Display(Name = "描述介绍")]
        [StringLength(100, ErrorMessage = "{0}不能超过{1}字")]
        public string msg { get; set; }

        /// <summary>
        ///     请求的数据json
        /// </summary>
        [Display(Name = "请求的数据json")]
        public string data { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }
    }
}