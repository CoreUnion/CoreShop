/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 文章表
    /// </summary>
    public partial class CoreCmsArticle
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsArticle()
        {
        }
		
        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
		[SugarColumn(IsPrimaryKey = true, IsIdentity = true)][Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id  { get; set; }
		
        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:200,ErrorMessage = "{0}不能超过{1}字")]
        public System.String title  { get; set; }
		
        /// <summary>
        /// 简介
        /// </summary>
        [Display(Name = "简介")]
		[StringLength(maximumLength:100,ErrorMessage = "{0}不能超过{1}字")]
        public System.String brief  { get; set; }
		
        /// <summary>
        /// 封面图
        /// </summary>
        [Display(Name = "封面图")]
		[StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        public System.String coverImage  { get; set; }
		
        /// <summary>
        /// 文章内容
        /// </summary>
        [Display(Name = "文章内容")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.String contentBody  { get; set; }
		
        /// <summary>
        /// 分类id
        /// </summary>
        [Display(Name = "分类id")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Int32 typeId  { get; set; }
		
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort  { get; set; }
		
        /// <summary>
        /// 是否发布
        /// </summary>
        [Display(Name = "是否发布")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Boolean isPub  { get; set; }
		
        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
		
        public System.Boolean? isDel  { get; set; }
		
        /// <summary>
        /// 访问量
        /// </summary>
        [Display(Name = "访问量")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Int32 pv  { get; set; }
		
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
		
        public System.DateTime? createTime  { get; set; }
		
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
		
        public System.DateTime? updateTime  { get; set; }
		
    }
}
