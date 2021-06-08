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
            var sqlStr = string.Empty;
            string dbTypeString = AppSettingsConstVars.DbDbType;
            if (dbTypeString == DbType.SqlServer.ToString())
            {
                sqlStr = @"select top 1000 sum(oi.nums) as nums,sum(oi.amount) as amount,oi.sn,oi.name,oi.imageUrl,oi.addon 
                        from CoreCmsOrderItem oi
                                        left join CoreCmsOrder o on oi.orderId = o.orderId
                                        where o.payStatus <> 1
                                        and o.paymentTime > '" + start + @"' and o.paymentTime <= '" + end + @"' 
                                        group by oi.sn,oi.name,oi.imageUrl,oi.addon 
                                        order by sum(oi." + filter + @") " + thesort + @",sum(oi." + filterSed + @") " + thesort + @"";
            }
            else if (dbTypeString == DbType.MySql.ToString())
            {
                sqlStr = @"select sum(oi.nums) as nums,sum(oi.amount) as amount,oi.sn,oi.name,oi.imageUrl,oi.addon 
                        from CoreCmsOrderItem oi
                                        left join CoreCmsOrder o on oi.orderId = o.orderId
                                        where o.payStatus <> 1
                                        and o.paymentTime > '" + start + @"' and o.paymentTime <= '" + end + @"' 
                                        group by oi.sn,oi.name,oi.imageUrl,oi.addon 
                                        order by sum(oi." + filter + @") " + thesort + @",sum(oi." + filterSed + @") " + thesort + @" LIMIT 1000";
            }

            if (string.IsNullOrEmpty(sqlStr))
            {
                return null;
            }


            RefAsync<int> totalCount = 0;
            var page = await DbClient.SqlQueryable<GoodsSalesVolume>(sqlStr).ToPageListAsync(pageIndex, pageSize, totalCount);
            var list = new PageList<GoodsSalesVolume>(page, pageIndex, pageSize, totalCount);
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

            var sqlStr = string.Empty;
            string dbTypeString = AppSettingsConstVars.DbDbType;
            if (dbTypeString == DbType.SqlServer.ToString())
            {
                sqlStr = @"select top 1000 count(gc.goodsId) as nums,gc.goodsId,gc.goodsName,g.images 
                         from CoreCmsGoodsCollection gc
                                left join CoreCmsGoods g on gc.goodsId = g.id
                                where gc.createTime > '" + start + @"' and gc.createTime <= '" + end + @"'  
                                group by gc.goodsId,gc.goodsName,g.images
                                order by sum(gc.goodsId) " + thesort + @"";
            }
            else if (dbTypeString == DbType.MySql.ToString())
            {
                sqlStr = @"select count(gc.goodsId) as nums,gc.goodsId,gc.goodsName,g.images 
                         from CoreCmsGoodsCollection gc
                                left join CoreCmsGoods g on gc.goodsId = g.id
                                where gc.createTime > '" + start + @"' and gc.createTime <= '" + end + @"'  
                                group by gc.goodsId,gc.goodsName,g.images
                                order by sum(gc.goodsId) " + thesort + @" LIMIT 1000";
            }

            if (string.IsNullOrEmpty(sqlStr))
            {
                return null;
            }

            RefAsync<int> totalCount = 0;
            var page = await DbClient.SqlQueryable<GoodsCollection>(sqlStr).ToPageListAsync(pageIndex, pageSize, totalCount);
            var list = new PageList<GoodsCollection>(page, pageIndex, pageSize, totalCount);
            return list;
        }


    }
}
