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
    /// 定时任务日志
    /// </summary>
    [SugarTable("SysTaskLog",TableDescription = "定时任务日志")]
    public partial class SysTaskLog
    {
        /// <summary>
        /// 定时任务日志
        /// </summary>
        public SysTaskLog()
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
        /// 任务名称
        /// </summary>
        [Display(Name = "任务名称")]
        [SugarColumn(ColumnDescription = "任务名称")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String name { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        [Display(Name = "完成时间")]
        [SugarColumn(ColumnDescription = "完成时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 是否完成
        /// </summary>
        [Display(Name = "是否完成")]
        [SugarColumn(ColumnDescription = "是否完成")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isSuccess { get; set; }
        /// <summary>
        /// 其他数据
        /// </summary>
        [Display(Name = "其他数据")]
        [SugarColumn(ColumnDescription = "其他数据", IsNullable = true)]
        public System.String parameters { get; set; }
    }
}