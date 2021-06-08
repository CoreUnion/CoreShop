/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com
 *         CreateTime: 2021-06-08 22:14:58
 *        Description: 暂无
***********************************************************************/ 
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 登录日志
    /// </summary>
    [SugarTable("CoreCmsLoginLog",TableDescription = "登录日志")]
    public partial class CoreCmsLoginLog
    {
        /// <summary>
        /// 登录日志
        /// </summary>
        public CoreCmsLoginLog()
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
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        [SugarColumn(ColumnDescription = "用户id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 userId { get; set; }
        /// <summary>
        /// 登录类型
        /// </summary>
        [Display(Name = "登录类型")]
        [SugarColumn(ColumnDescription = "登录类型", IsNullable = true)]
        public System.Int32? state { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        [Display(Name = "时间")]
        [SugarColumn(ColumnDescription = "时间", IsNullable = true)]
        public System.DateTime? logTime { get; set; }
        /// <summary>
        /// 地点城市
        /// </summary>
        [Display(Name = "地点城市")]
        [SugarColumn(ColumnDescription = "地点城市", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String city { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        [Display(Name = "ip地址")]
        [SugarColumn(ColumnDescription = "ip地址", IsNullable = true)]
        [StringLength(15, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String ip { get; set; }
    }
}