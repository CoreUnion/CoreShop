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
    ///     定时任务日志
    /// </summary>
    public class SysTaskLog
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     任务名称
        /// </summary>
        [Display(Name = "任务名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}不能超过{1}字")]
        public string name { get; set; }

        /// <summary>
        ///     完成时间
        /// </summary>
        [Display(Name = "完成时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public DateTime createTime { get; set; }

        /// <summary>
        ///     是否完成
        /// </summary>
        [Display(Name = "是否完成")]
        [Required(ErrorMessage = "请输入{0}")]
        public bool isSuccess { get; set; }

        /// <summary>
        ///     其他数据
        /// </summary>
        [Display(Name = "其他数据")]
        public string parameters { get; set; }
    }
}