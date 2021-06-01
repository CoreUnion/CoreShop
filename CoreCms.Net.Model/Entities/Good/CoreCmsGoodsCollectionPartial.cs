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
    ///     商品收藏
    /// </summary>
    public partial class CoreCmsGoodsCollection
    {
        /// <summary>
        ///     商品信息
        /// </summary>
        [Display(Name = "商品信息")]
        [SugarColumn(IsIgnore = true)]
        public CoreCmsGoods goods { get; set; }
    }
}