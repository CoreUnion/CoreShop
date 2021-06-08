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
    /// 文章表
    /// </summary>
    [SugarTable("CoreCmsArticle",TableDescription = "文章表")]
    public partial class CoreCmsArticle
    {
        /// <summary>
        /// 文章表
        /// </summary>
        public CoreCmsArticle()
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
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        [SugarColumn(ColumnDescription = "标题")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(200, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String title { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [Display(Name = "简介")]
        [SugarColumn(ColumnDescription = "简介", IsNullable = true)]
        [StringLength(100, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String brief { get; set; }
        /// <summary>
        /// 封面图
        /// </summary>
        [Display(Name = "封面图")]
        [SugarColumn(ColumnDescription = "封面图", IsNullable = true)]
        [StringLength(255, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String coverImage { get; set; }
        /// <summary>
        /// 文章内容
        /// </summary>
        [Display(Name = "文章内容")]
        [SugarColumn(ColumnDescription = "文章内容")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.String contentBody { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        [Display(Name = "分类id")]
        [SugarColumn(ColumnDescription = "分类id")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 typeId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [SugarColumn(ColumnDescription = "排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort { get; set; }
        /// <summary>
        /// 是否发布
        /// </summary>
        [Display(Name = "是否发布")]
        [SugarColumn(ColumnDescription = "是否发布")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isPub { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [SugarColumn(ColumnDescription = "是否删除", IsNullable = true)]
        public System.Boolean? isDel { get; set; }
        /// <summary>
        /// 访问量
        /// </summary>
        [Display(Name = "访问量")]
        [SugarColumn(ColumnDescription = "访问量")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 pv { get; set; }
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