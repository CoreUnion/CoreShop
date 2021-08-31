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

namespace CoreCms.Net.Model.ViewModels.DTO
{
    /// <summary>
    ///     OrderToAftersales返回类
    /// </summary>
    public class OrderToAftersalesDto
    {
        public decimal refundMoney { get; set; } = 0;

        public Dictionary<int, reshipGoods> reshipGoods { get; set; } = null;

        public List<CoreCmsBillAftersales> billAftersales { get; set; } = new();
    }
}