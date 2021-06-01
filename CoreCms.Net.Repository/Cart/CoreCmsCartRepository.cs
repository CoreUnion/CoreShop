/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    ///     购物车表 接口实现
    /// </summary>
    public class CoreCmsCartRepository : BaseRepository<CoreCmsCart>, ICoreCmsCartRepository
    {
        public CoreCmsCartRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        #region 获取购物车用户数据总数

        /// <summary>
        ///     获取购物车用户数据总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetCountAsync(int userId)
        {
            var count = DbClient.Queryable<CoreCmsCart, CoreCmsProducts, CoreCmsGoods>((cart, products, goods) =>
                    new object[]
                    {
                        JoinType.Inner, cart.productId == products.id,
                        JoinType.Inner, products.goodsId == goods.id
                    })
                .Where((cart, products, goods) => cart.type == (int) GlobalEnumVars.OrderType.Common)
                .Select((cart, products, goods) => new {cart.id, cart.userId, goodId = goods.id})
                .MergeTable()
                .CountAsync(p => p.userId == userId);
            return await count;
        }

        #endregion
    }
}