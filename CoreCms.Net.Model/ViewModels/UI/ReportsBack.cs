/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;

namespace CoreCms.Net.Model.ViewModels.UI
{
    /// <summary>
    ///     根据时间查询报表截断获取时间段返回数据
    /// </summary>
    public class ReportsBackForGetDate
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int section { get; set; } = 0;
        public int num { get; set; } = 0;
    }

    /// <summary>
    ///     商品销量统计返回dataTable转实体
    /// </summary>
    public class GoodsSalesVolume
    {
        /// <summary>
        ///     数量
        /// </summary>
        public int nums { get; set; } = 0;

        /// <summary>
        ///     金额
        /// </summary>
        public decimal amount { get; set; } = 0;

        /// <summary>
        ///     名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///     缩略图
        /// </summary>
        public string imageUrl { get; set; }

        /// <summary>
        ///     规格说明
        /// </summary>
        public string addon { get; set; }

        /// <summary>
        ///     货号
        /// </summary>
        public string sn { get; set; }
    }


    /// <summary>
    ///     商品销量统计返回dataTable转实体
    /// </summary>
    public class GoodsCollection
    {
        /// <summary>
        ///     数量
        /// </summary>
        public int nums { get; set; } = 0;

        /// <summary>
        ///     商品编号
        /// </summary>
        public decimal goodId { get; set; } = 0;

        /// <summary>
        ///     名称
        /// </summary>
        public string goodsName { get; set; }

        /// <summary>
        ///     缩略图
        /// </summary>
        public string images { get; set; }
    }


    /// <summary>
    ///     后端首页统计返回UI
    /// </summary>
    public class StatisticsOut
    {
        /// <summary>
        ///     日期
        /// </summary>
        public string day { get; set; }

        /// <summary>
        ///     数量
        /// </summary>
        public int nums { get; set; }
    }
}