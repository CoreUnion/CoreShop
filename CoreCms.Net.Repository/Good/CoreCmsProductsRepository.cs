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
using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 货品表 接口实现
    /// </summary>
    public class CoreCmsProductsRepository : BaseRepository<CoreCmsProducts>, ICoreCmsProductsRepository
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoreCmsStockRepository _stockRepository;
        private readonly IHttpContextUser _user;


        public CoreCmsProductsRepository(IUnitOfWork unitOfWork, ICoreCmsStockRepository stockRepository, IHttpContextUser user) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _stockRepository = stockRepository;
            _user = user;
        }

        #region 判断货品上下架状态

        /// <summary>
        /// 判断货品上下架状态
        /// </summary>
        /// <param name="productsId">货品序列</param>
        /// <returns></returns>
        public async Task<bool> GetShelfStatus(int productsId)
        {
            var data = await DbClient.Queryable<CoreCmsProducts, CoreCmsGoods>((products, goods) => new object[]
                  {
                    JoinType.Inner, products.goodsId == goods.id
                  })
                .Where((products, goods) => products.id == productsId)
                .Select((products, goods) => new
                {
                    productsId = products.id,
                    isMarketable = goods.isMarketable
                }).FirstAsync();
            return data != null && data.isMarketable == true;
        }
        #endregion

        #region 获取库存报警数量
        /// <summary>
        /// 获取库存报警数量
        /// </summary>
        /// <param name="goodsStocksWarn"></param>
        /// <returns></returns>
        public async Task<int> GoodsStaticsTotalWarn(int goodsStocksWarn)
        {
            var sql = @"SELECT  COUNT(*) AS number
                        FROM    ( SELECT    t.goodsId
                                  FROM      ( SELECT    goodsId ,
                                                        ( CASE WHEN stock < freezeStock THEN 0
                                                               ELSE stock - freezeStock
                                                          END ) AS number
                                              FROM      CoreCmsProducts
                                            ) t
                                  WHERE     t.number < " + goodsStocksWarn + @"
                                  GROUP BY  t.goodsId
                                ) d";

            var dt = await DbClient.Ado.GetDataTableAsync(sql);
            var number = dt.Rows[0][0].ObjectToInt(0);
            return number;
        }

        #endregion

        #region 获取关联商品的货品列表数据
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
        public async Task<IPageList<CoreCmsProducts>> QueryDetailPageAsync(Expression<Func<CoreCmsProducts, bool>> predicate,
            Expression<Func<CoreCmsProducts, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsProducts> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsProducts, CoreCmsGoods>((p, good) => new JoinQueryInfos(
                        JoinType.Left, p.goodsId == good.id))
                    .Select((p, good) => new CoreCmsProducts
                    {
                        id = p.id,
                        goodsId = p.goodsId,
                        barcode = p.barcode,
                        sn = p.sn,
                        price = p.price,
                        costprice = p.costprice,
                        mktprice = p.mktprice,
                        marketable = p.marketable,
                        weight = p.weight,
                        stock = p.stock,
                        freezeStock = p.freezeStock,
                        spesDesc = p.spesDesc,
                        isDefalut = p.isDefalut,
                        images = p.images,
                        isDel = p.isDel,
                        name = good.name,
                        bn = good.bn,
                        isMarketable = good.isMarketable,
                        unit = good.unit
                    }).With(SqlWith.NoLock)
                    .MergeTable()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsProducts, CoreCmsGoods>((p, good) => new JoinQueryInfos(
                        JoinType.Left, p.goodsId == good.id))
                    .Select((p, good) => new CoreCmsProducts
                    {
                        id = p.id,
                        goodsId = p.goodsId,
                        barcode = p.barcode,
                        sn = p.sn,
                        price = p.price,
                        costprice = p.costprice,
                        mktprice = p.mktprice,
                        marketable = p.marketable,
                        weight = p.weight,
                        stock = p.stock,
                        freezeStock = p.freezeStock,
                        spesDesc = p.spesDesc,
                        isDefalut = p.isDefalut,
                        images = p.images,
                        isDel = p.isDel,
                        name = good.name,
                        bn = good.bn,
                        isMarketable = good.isMarketable,
                        unit = good.unit
                    })
                    .MergeTable()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsProducts>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

        #region 修改单个货品库存并记入库存管理日志内

        /// <summary>
        /// 修改单个货品库存并记入库存管理日志内
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="stockNumber"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> EditStock(int productId, int stockNumber)
        {
            var jm = new AdminUiCallBack();

            var product = await DbClient.Queryable<CoreCmsProducts, CoreCmsGoods>((p, good) => new JoinQueryInfos(
                    JoinType.Left, p.goodsId == good.id))
                .Select((p, good) => new CoreCmsProducts
                {
                    id = p.id,
                    goodsId = p.goodsId,
                    barcode = p.barcode,
                    sn = p.sn,
                    price = p.price,
                    costprice = p.costprice,
                    mktprice = p.mktprice,
                    marketable = p.marketable,
                    weight = p.weight,
                    stock = p.stock,
                    freezeStock = p.freezeStock,
                    spesDesc = p.spesDesc,
                    isDefalut = p.isDefalut,
                    images = p.images,
                    isDel = p.isDel,
                    name = good.name,
                    bn = good.bn,
                    isMarketable = good.isMarketable,
                    unit = good.unit
                })
                .MergeTable()
                .Where(p => p.id == productId)
                .FirstAsync();
            if (product == null)
            {
                jm.msg = "货品数据查询失败";
                return jm;
            }


            var nums = stockNumber - product.stock;
            var msg = string.Empty;
            if (nums == 0)
            {
                jm.code = 0;
                jm.msg = "库存未修改";
                return jm;
            }
            else if (nums < 0)
            {
                jm.code = 0;
                msg = "库存盘点：库存减少" + Math.Abs(nums);
            }
            else
            {
                msg = "库存盘点：库存增加" + nums;
            }

            var stockModel = new CoreCmsStock();
            stockModel.id = await _stockRepository.CreateCode(GlobalEnumVars.StockType.CheckGoods.ToString());
            stockModel.memo = msg;
            stockModel.type = (int)GlobalEnumVars.StockType.CheckGoods;
            stockModel.manager = _user.ID;
            stockModel.createTime = DateTime.Now;

            var stockLogModel = new CoreCmsStockLog();
            stockLogModel.stockId = stockModel.id;
            stockLogModel.productId = product.id;
            stockLogModel.goodsId = product.goodsId;
            stockLogModel.nums = nums;
            stockLogModel.goodsName = product.name;
            stockLogModel.sn = product.sn;
            stockLogModel.bn = product.bn;
            stockLogModel.spesDesc = product.spesDesc;

            try
            {
                _unitOfWork.BeginTran();

                await DbClient.Updateable<CoreCmsProducts>().SetColumns(p => new CoreCmsProducts() { stock = stockNumber }).Where(p => p.id == product.id).ExecuteCommandAsync();

                await DbClient.Insertable(stockModel).ExecuteCommandAsync();
                await DbClient.Insertable(stockLogModel).ExecuteCommandAsync();

                jm.code = 0;
                jm.msg = "库存修改成功";

                _unitOfWork.CommitTran();
            }
            catch (Exception e)
            {
                jm.code = 1;
                jm.msg = "库存修改异常";
                jm.otherData = e;
                _unitOfWork.RollbackTran();
            }
            return jm;
        }


        #endregion


        #region 获取货品数据
        /// <summary>
        /// 获取货品数据
        /// </summary>
        /// <param name="goodId"></param>
        /// <returns></returns>
        public async Task<List<CoreCmsProducts>> GetProducts(int goodId = 0)
        {
            var list = await DbClient.Queryable<CoreCmsProducts, CoreCmsGoods>((p, good) => new JoinQueryInfos(
                    JoinType.Left, p.goodsId == good.id))
                   .Select((p, good) => new CoreCmsProducts
                   {
                       id = p.id,
                       goodsId = p.goodsId,
                       barcode = p.barcode,
                       sn = p.sn,
                       price = p.price,
                       costprice = p.costprice,
                       mktprice = p.mktprice,
                       marketable = p.marketable,
                       weight = p.weight,
                       stock = p.stock,
                       freezeStock = p.freezeStock,
                       spesDesc = p.spesDesc,
                       isDefalut = p.isDefalut,
                       images = p.images,
                       isDel = p.isDel,
                       name = good.name,
                       bn = good.bn,
                       isMarketable = good.isMarketable,
                       unit = good.unit
                   }).With(SqlWith.NoLock)
                   .MergeTable()
                   .OrderBy(p => p.id, OrderByType.Desc)
                   .WhereIF(goodId > 0, p => p.goodsId == goodId)
                   .Where(p => p.isDel == false)
                   .ToListAsync();

            return list;
        }

        #endregion

    }
}
