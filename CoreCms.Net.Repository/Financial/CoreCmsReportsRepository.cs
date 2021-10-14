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
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.Echarts;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 报表通用返回 接口实现
    /// </summary>
    public class CoreCmsReportsRepository : BaseRepository<GetOrdersReportsDbSelectOut>, ICoreCmsReportsRepository
    {
        public CoreCmsReportsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// 获取订单销量查询返回结果
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="filter"></param>
        /// <param name="filterSed"></param>
        /// <param name="thesort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IPageList<GoodsSalesVolume>> GetGoodsSalesVolumes(string start, string end, string filter, string filterSed, string thesort, int pageIndex = 1, int pageSize = 5000)
        {
            RefAsync<int> totalCount = 0;

            var startDt = DateTime.Parse(start);
            var endDt = DateTime.Parse(end);

            var orderBy = thesort switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };

            var data = await DbClient.Queryable<CoreCmsOrderItem>()
                .LeftJoin<CoreCmsOrder>((oi, o) => oi.orderId == o.orderId)
                .Where((oi, o) => o.payStatus != 1 && o.paymentTime > startDt && o.paymentTime <= endDt)
                .GroupBy((oi, o) => new
                {
                    oi.sn,
                    oi.name,
                    oi.imageUrl,
                    oi.addon
                })
                .Select((oi, o) => new GoodsSalesVolume()
                {
                    nums = SqlFunc.AggregateSum(oi.nums),
                    amount = SqlFunc.AggregateSum(oi.amount),
                    sn = oi.sn,
                    name = oi.name,
                    imageUrl = oi.imageUrl,
                    addon = oi.addon
                })
                .MergeTable()
                .OrderByIF(filter == "nums", p => p.nums, orderBy)
                .OrderByIF(filter == "amount", p => p.amount, orderBy)
                .OrderByIF(filterSed == "nums", p => p.nums, orderBy)
                .OrderByIF(filterSed == "amount", p => p.amount, orderBy)
                .ToPageListAsync(pageIndex, pageSize, totalCount);

            var list = new PageList<GoodsSalesVolume>(data, pageIndex, pageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 获取商品收藏查询返回结果
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="thesort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IPageList<GoodsCollection>> GetGoodsCollections(string start, string end, string thesort, int pageIndex = 1, int pageSize = 5000)
        {
            RefAsync<int> totalCount = 0;

            var startDt = DateTime.Parse(start);
            var endDt = DateTime.Parse(end);

            var orderBy = thesort switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };

            var data = await DbClient.Queryable<CoreCmsGoodsCollection>()
                .LeftJoin<CoreCmsGoods>((gc, g) => gc.goodsId == g.id)
                .Where((gc, g) => gc.createTime > startDt && gc.createTime <= endDt)
                .GroupBy((gc, g) => new { gc.goodsId, gc.goodsName, g.images })
                .Select((gc, g) => new GoodsCollection()
                {
                    nums = SqlFunc.AggregateCount(gc.goodsId),
                    goodsName = gc.goodsName,
                    goodId = gc.goodsId,
                    images = g.images
                })
                .MergeTable()
                .OrderBy(gc => gc.nums, orderBy)
                .ToPageListAsync(pageIndex, pageSize, totalCount);

            var list = new PageList<GoodsCollection>(data, pageIndex, pageSize, totalCount);
            return list;
        }


    }
}
