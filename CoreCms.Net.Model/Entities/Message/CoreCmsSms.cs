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
    /// 短信发送日志
    /// </summary>
    [SugarTable("CoreCmsSms",TableDescription = "短信发送日志")]
    public partial class CoreCmsSms
    {
        /// <summary>
        /// 短信发送日志
        /// </summary>
        public CoreCmsSms()
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
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        [SugarColumn(ColumnDescription = "手机号码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(15, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String mobile { get; set; }
        /// <summary>
        /// 发送编码
        /// </summary>
        [Display(Name = "发送编码")]
        [SugarColumn(ColumnDescription = "发送编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(60, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String code { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        [Display(Name = "参数")]
        [SugarColumn(ColumnDescription = "参数")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.String parameters { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [SugarColumn(ColumnDescription = "内容")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String contentBody { get; set; }
        /// <summary>
        /// ip
        /// </summary>
        [Display(Name = "ip")]
        [SugarColumn(ColumnDescription = "ip")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String ip { get; set; }
        /// <summary>
        /// 是否使用
        /// </summary>
        [Display(Name = "是否使用")]
        [SugarColumn(ColumnDescription = "是否使用")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isUsed { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.DateTime createTime { get; set; }
    }
}