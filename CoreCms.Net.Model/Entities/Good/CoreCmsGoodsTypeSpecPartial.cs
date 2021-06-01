/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     商品类型属性表
    /// </summary>
    public partial class CoreCmsGoodsTypeSpec
    {
        /// <summary>
        ///     子类
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CoreCmsGoodsTypeSpecValue> specValues { get; set; } = new();
    }
}