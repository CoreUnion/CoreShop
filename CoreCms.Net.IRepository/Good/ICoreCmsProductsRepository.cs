using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     货品表 工厂接口
    /// </summary>
    public interface ICoreCmsProductsRepository : IBaseRepository<CoreCmsProducts>
    {
        /// <summary>
        ///     判断货品上下架状态
        /// </summary>
        /// <param name="productsId">货品序列</param>
        /// <returns></returns>
        Task<bool> GetShelfStatus(int productsId);


        /// <summary>
        ///     获取库存报警数量
        /// </summary>
        /// <param name="goodsStocksWarn"></param>
        /// <returns></returns>
        Task<int> GoodsStaticsTotalWarn(int goodsStocksWarn);


        /// <summary>
        ///     获取关联商品的货品列表数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<IPageList<CoreCmsProducts>> QueryDetailPageAsync(Expression<Func<CoreCmsProducts, bool>> predicate,
            Expression<Func<CoreCmsProducts, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);


        /// <summary>
        ///     修改单个货品库存并记入库存管理日志内
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> EditStock(int productId, int stock);


        /// <summary>
        ///     获取所有货品数据
        /// </summary>
        /// <returns></returns>
        Task<List<CoreCmsProducts>> GetProducts(int goodId = 0);
    }
}