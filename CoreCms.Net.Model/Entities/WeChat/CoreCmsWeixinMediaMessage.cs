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
    /// 微信图文消息表
    /// </summary>
    [SugarTable("CoreCmsWeixinMediaMessage",TableDescription = "微信图文消息表")]
    public partial class CoreCmsWeixinMediaMessage
    {
        /// <summary>
        /// 微信图文消息表
        /// </summary>
        public CoreCmsWeixinMediaMessage()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "")]
        [SugarColumn(ColumnDescription = "", IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        [SugarColumn(ColumnDescription = "标题", IsNullable = true)]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String title { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [Display(Name = "作者")]
        [SugarColumn(ColumnDescription = "作者", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String author { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        [Display(Name = "摘要")]
        [SugarColumn(ColumnDescription = "摘要", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String brief { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        [Display(Name = "封面")]
        [SugarColumn(ColumnDescription = "封面", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String imageUrl { get; set; }
        /// <summary>
        /// 文章详情
        /// </summary>
        [Display(Name = "文章详情")]
        [SugarColumn(ColumnDescription = "文章详情", IsNullable = true)]
        public System.String contentBody { get; set; }
        /// <summary>
        /// 原文地址
        /// </summary>
        [Display(Name = "原文地址")]
        [SugarColumn(ColumnDescription = "原文地址", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String url { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public System.DateTime? createTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public System.DateTime? updateTime { get; set; }
    }
}