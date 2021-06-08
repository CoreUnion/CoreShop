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
    /// 消息配置表
    /// </summary>
    [SugarTable("CoreCmsMessageCenter",TableDescription = "消息配置表")]
    public partial class CoreCmsMessageCenter
    {
        /// <summary>
        /// 消息配置表
        /// </summary>
        public CoreCmsMessageCenter()
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
        /// 编码
        /// </summary>
        [Display(Name = "编码")]
        [SugarColumn(ColumnDescription = "编码")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String code { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [SugarColumn(ColumnDescription = "描述", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String description { get; set; }
        /// <summary>
        /// 启用短信
        /// </summary>
        [Display(Name = "启用短信")]
        [SugarColumn(ColumnDescription = "启用短信")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isSms { get; set; }
        /// <summary>
        /// 启用站内消息
        /// </summary>
        [Display(Name = "启用站内消息")]
        [SugarColumn(ColumnDescription = "启用站内消息")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isMessage { get; set; }
        /// <summary>
        /// 启用微信模板消息
        /// </summary>
        [Display(Name = "启用微信模板消息")]
        [SugarColumn(ColumnDescription = "启用微信模板消息")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isWxTempletMessage { get; set; }
    }
}