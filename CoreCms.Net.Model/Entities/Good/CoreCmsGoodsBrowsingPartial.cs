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
    ///     商品浏览记录
    /// </summary>
    public partial class CoreCmsGoodsBrowsing
    {
        /// <summary>
        ///     商品图片
        /// </summary>
        [Display(Name = "商品图片")]
        [SugarColumn(IsIgnore = true)]
        public string goodImage { get; set; }


        /// <summary>
        ///     是否收藏
        /// </summary>
        [Display(Name = "是否收藏")]
        [SugarColumn(IsIgnore = true)]
        public bool isCollection { get; set; }
    }
}