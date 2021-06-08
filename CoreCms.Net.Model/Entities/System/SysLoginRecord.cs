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
    /// 登录日志表
    /// </summary>
    [SugarTable("SysLoginRecord",TableDescription = "登录日志表")]
    public partial class SysLoginRecord
    {
        /// <summary>
        /// 登录日志表
        /// </summary>
        public SysLoginRecord()
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
        /// 用户账号
        /// </summary>
        [Display(Name = "用户账号")]
        [SugarColumn(ColumnDescription = "用户账号")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String username { get; set; }
        /// <summary>
        /// 操作系统
        /// </summary>
        [Display(Name = "操作系统")]
        [SugarColumn(ColumnDescription = "操作系统", IsNullable = true)]
        [StringLength(400, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String os { get; set; }
        /// <summary>
        /// 设备名
        /// </summary>
        [Display(Name = "设备名")]
        [SugarColumn(ColumnDescription = "设备名", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String device { get; set; }
        /// <summary>
        /// 浏览器类型
        /// </summary>
        [Display(Name = "浏览器类型")]
        [SugarColumn(ColumnDescription = "浏览器类型", IsNullable = true)]
        [StringLength(400, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String browser { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        [Display(Name = "ip地址")]
        [SugarColumn(ColumnDescription = "ip地址", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String ip { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        [Display(Name = "操作类型")]
        [SugarColumn(ColumnDescription = "操作类型")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 operType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [SugarColumn(ColumnDescription = "备注", IsNullable = true)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String comments { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        [Display(Name = "登录时间")]
        [SugarColumn(ColumnDescription = "登录时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [SugarColumn(ColumnDescription = "修改时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}