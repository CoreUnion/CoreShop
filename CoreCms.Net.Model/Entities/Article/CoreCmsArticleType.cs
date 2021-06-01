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
    /// 文章分类表
    /// </summary>
    public partial class CoreCmsArticleType
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsArticleType()
        {
        }
		
        /// <summary>
        /// 序列
        /// </summary>
        [Display(Name = "序列")]
		[SugarColumn(IsPrimaryKey = true, IsIdentity = true)][Required(ErrorMessage = "请输入{0}")]
        public System.Int32 id  { get; set; }
		
        /// <summary>
        /// 分类名称
        /// </summary>
        [Display(Name = "分类名称")]
		[Required(ErrorMessage = "请输入{0}")][StringLength(maximumLength:32,ErrorMessage = "{0}不能超过{1}字")]
        public System.String name  { get; set; }
		
        /// <summary>
        /// 父id
        /// </summary>
        [Display(Name = "父id")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Int32 parentId  { get; set; }
		
        /// <summary>
        /// 排序 
        /// </summary>
        [Display(Name = "排序 ")]
		[Required(ErrorMessage = "请输入{0}")]
        public System.Int32 sort  { get; set; }
		
    }
}
