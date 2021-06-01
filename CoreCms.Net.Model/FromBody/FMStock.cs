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
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.Model.FromBody
{
    public class FMCreateStock
    {
        /// <summary>
        ///     广告位置
        /// </summary>
        public CoreCmsStock model { get; set; }


        public List<FMCreateStockItem> items { get; set; }
    }

    public class FMCreateStockItem
    {
        public int nums { get; set; }
        public int productId { get; set; }
    }
}