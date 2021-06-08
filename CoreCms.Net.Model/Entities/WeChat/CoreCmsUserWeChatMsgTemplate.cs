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
    /// 微信小程序消息模板
    /// </summary>
    [SugarTable("CoreCmsUserWeChatMsgTemplate",TableDescription = "微信小程序消息模板")]
    public partial class CoreCmsUserWeChatMsgTemplate
    {
        /// <summary>
        /// 微信小程序消息模板
        /// </summary>
        public CoreCmsUserWeChatMsgTemplate()
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
        /// 模板名称
        /// </summary>
        [Display(Name = "模板名称")]
        [SugarColumn(ColumnDescription = "模板名称", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String templateTitle { get; set; }
        /// <summary>
        /// 模板说明
        /// </summary>
        [Display(Name = "模板说明")]
        [SugarColumn(ColumnDescription = "模板说明", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String templateDes { get; set; }
        /// <summary>
        /// 模板Id
        /// </summary>
        [Display(Name = "模板Id")]
        [SugarColumn(ColumnDescription = "模板Id", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String templateId { get; set; }
        /// <summary>
        /// 字段1
        /// </summary>
        [Display(Name = "字段1")]
        [SugarColumn(ColumnDescription = "字段1", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String data01 { get; set; }
        /// <summary>
        /// 字段2
        /// </summary>
        [Display(Name = "字段2")]
        [SugarColumn(ColumnDescription = "字段2", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String data02 { get; set; }
        /// <summary>
        /// 字段3
        /// </summary>
        [Display(Name = "字段3")]
        [SugarColumn(ColumnDescription = "字段3", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String data03 { get; set; }
        /// <summary>
        /// 字段4
        /// </summary>
        [Display(Name = "字段4")]
        [SugarColumn(ColumnDescription = "字段4", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String data04 { get; set; }
        /// <summary>
        /// 字段5
        /// </summary>
        [Display(Name = "字段5")]
        [SugarColumn(ColumnDescription = "字段5", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String data05 { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [SugarColumn(ColumnDescription = "描述", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sortId { get; set; }
    }
}