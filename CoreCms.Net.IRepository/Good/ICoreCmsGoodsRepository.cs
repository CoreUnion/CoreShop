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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     商品表 工厂接口
    /// </summary>
    public interface ICoreCmsGoodsRepository : IBaseRepository<CoreCmsGoods>
    {
        /// <summary>
        ///     事务重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> InsertAsync(FMGoodsInsertModel entity);

        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(FMGoodsInsertModel entity);

        /// <summary>
        ///     重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids);

        /// <summary>
        ///     获取商品重量
        /// </summary>
        /// <param name="productsId"></param>
        /// <returns></returns>
        Task<decimal> GetWeight(int productsId);

        /// <summary>
        ///     库存改变机制。
        ///     库存机制：商品下单 总库存不变，冻结库存加1，
        ///     商品发货：冻结库存减1，总库存减1，
        ///     订单完成但未发货：总库存不变，冻结库存减1
        ///     商品退款&取消订单：总库存不变，冻结库存减1,
        ///     商品退货：总库存加1，冻结库存不变,
        ///     可销售库存：总库存-冻结库存
        /// </summary>
        /// <returns></returns>
        WebApiCallBack ChangeStock(int productsId, string type = "order", int num = 0);

        /// <summary>
        ///     获取随机推荐数据
        /// </summary>
        /// <param name="number">数量</param>
        /// <param name="isRecommend">是否推荐</param>
        /// <returns></returns>
        Task<List<CoreCmsGoods>> GetGoodsRecommendList(int number, bool isRecommend = false);

        /// <summary>
        ///     获取数据总数
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        new Task<int> GetCountAsync(Expression<Func<CoreCmsGoods, bool>> predicate, bool blUseNoLock = false);


        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        new Task<IPageList<CoreCmsGoods>> QueryPageAsync(
            Expression<Func<CoreCmsGoods, bool>> predicate,
            Expression<Func<CoreCmsGoods, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);


        /// <summary>
        ///     重写根据条件查询一定数量数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="take">获取数量</param>
        /// <param name="orderByPredicate">排序字段</param>
        /// <param name="orderByType">排序顺序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        new Task<List<CoreCmsGoods>> QueryListByClauseAsync(Expression<Func<CoreCmsGoods, bool>> predicate, int take,
            Expression<Func<CoreCmsGoods, object>> orderByPredicate, OrderByType orderByType, bool blUseNoLock = false);


        /// <summary>
        ///     重写根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns>泛型实体集合</returns>
        new Task<List<CoreCmsGoods>> QueryListByClauseAsync(Expression<Func<CoreCmsGoods, bool>> predicate,
            string orderBy = "",
            bool blUseNoLock = false);


        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        new Task<IPageList<CoreCmsGoods>> QueryPageAsync(Expression<Func<CoreCmsGoods, bool>> predicate,
            string orderBy = "", int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);


        /// <summary>
        ///     根据条件查询代理池商品分页数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        Task<IPageList<CoreCmsGoods>> QueryAgentGoodsPageAsync(Expression<Func<CoreCmsGoods, bool>> predicate,
            string orderBy = "", int pageIndex = 1, int pageSize = 20, bool blUseNoLock = false);

        /// <summary>
        ///     获取下拉商品数据
        /// </summary>
        /// <returns></returns>
        Task<List<EnumEntity>> QueryEnumEntityAsync();
    }
}