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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    ///     商品浏览记录表 接口实现
    /// </summary>
    public class CoreCmsGoodsBrowsingRepository : BaseRepository<CoreCmsGoodsBrowsing>, ICoreCmsGoodsBrowsingRepository
    {
        public CoreCmsGoodsBrowsingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsGoodsBrowsing>> QueryPageAsync(
            Expression<Func<CoreCmsGoodsBrowsing, bool>> predicate,
            Expression<Func<CoreCmsGoodsBrowsing, object>> orderByExpression, OrderByType orderByType,
            int pageIndex = 1,
            int pageSize = 20)
        {
            RefAsync<int> totalCount = 0;
            var page = await DbClient.Queryable<CoreCmsGoodsBrowsing, CoreCmsGoods>((gb, goods) =>
                    new JoinQueryInfos(JoinType.Left, gb.goodsId == goods.id))
                .Select((gb, goods) => new CoreCmsGoodsBrowsing
                {
                    id = gb.id,
                    goodsId = gb.goodsId,
                    userId = gb.userId,
                    goodsName = gb.goodsName,
                    createTime = gb.createTime,
                    isdel = gb.isdel,
                    goodImage = goods.image
                    //isCollection = SqlFunc.Subqueryable<CoreCmsGoodsCollection>().Where(p => p.userId == gb.userId && p.goodsId == gb.goodsId).Any()
                })
                .With(SqlWith.NoLock)
                .MergeTable()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).ToPageListAsync(pageIndex, pageSize, totalCount);

            var list = new PageList<CoreCmsGoodsBrowsing>(page, pageIndex, pageSize, totalCount);
            return list;
        }
    }
}