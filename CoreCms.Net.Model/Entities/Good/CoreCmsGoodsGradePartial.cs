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
    ///     商品分类
    /// </summary>
    public partial class CoreCmsGoodsGrade
    {
        /// <summary>
        ///     名称
        /// </summary>
        [Display(Name = "名称")]
        [SugarColumn(IsIgnore = true)]
        public string gradeName { get; set; }
    }
}