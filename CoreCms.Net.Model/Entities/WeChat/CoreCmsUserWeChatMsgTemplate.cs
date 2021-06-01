/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     微信小程序消息模板
    /// </summary>
    public partial class CoreCmsUserWeChatMsgTemplate
    {
        /// <summary>
        ///     序列
        /// </summary>
        [Display(Name = "序列")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public int id { get; set; }

        /// <summary>
        ///     模板名称
        /// </summary>
        [Display(Name = "模板名称")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string templateTitle { get; set; }

        /// <summary>
        ///     模板说明
        /// </summary>
        [Display(Name = "模板说明")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string templateDes { get; set; }

        /// <summary>
        ///     模板Id
        /// </summary>
        [Display(Name = "模板Id")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string templateId { get; set; }

        /// <summary>
        ///     字段1
        /// </summary>
        [Display(Name = "字段1")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string data01 { get; set; }

        /// <summary>
        ///     字段2
        /// </summary>
        [Display(Name = "字段2")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string data02 { get; set; }

        /// <summary>
        ///     字段3
        /// </summary>
        [Display(Name = "字段3")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string data03 { get; set; }

        /// <summary>
        ///     字段4
        /// </summary>
        [Display(Name = "字段4")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string data04 { get; set; }

        /// <summary>
        ///     字段5
        /// </summary>
        [Display(Name = "字段5")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string data05 { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        [Display(Name = "描述")]
        [StringLength(255, ErrorMessage = "{0}不能超过{1}字")]
        public string description { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public int sortId { get; set; }
    }
}