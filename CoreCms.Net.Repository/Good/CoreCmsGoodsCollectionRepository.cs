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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 商品收藏表 接口实现
    /// </summary>
    public class CoreCmsGoodsCollectionRepository : BaseRepository<CoreCmsGoodsCollection>, ICoreCmsGoodsCollectionRepository
    {
        public CoreCmsGoodsCollectionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 重写根据条件查询分页数据
        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsGoodsCollection>> QueryPageAsync(Expression<Func<CoreCmsGoodsCollection, bool>> predicate,
            Expression<Func<CoreCmsGoodsCollection, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            RefAsync<int> totalCount = 0;
            var page = await DbClient.Queryable<CoreCmsGoodsCollection>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .Mapper(p => p.goods, p => p.goodsId)
                .ToPageListAsync(pageIndex, pageSize, totalCount);

            var list = new PageList<CoreCmsGoodsCollection>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

        #region 收藏
        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ToAdd(int userId, int goodsId)
        {
            var jm = new WebApiCallBack();

            var goodsModel = await DbClient.Queryable<CoreCmsGoods>().Where(p => p.id == goodsId).FirstAsync();
            if (goodsModel == null)
            {
                jm.msg = "没有此商品";
                return jm;
            }
            var model = new CoreCmsGoodsCollection();
            model.userId = userId;
            model.goodsId = goodsId;
            model.createTime = DateTime.Now;
            model.goodsName = goodsModel.name;

            await DbClient.Insertable(model).ExecuteCommandAsync();

            jm.status = true;
            jm.msg = "收藏成功";

            return jm;

        }
        #endregion

    }
}
