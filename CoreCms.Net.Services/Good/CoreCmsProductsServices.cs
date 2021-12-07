/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.Services
{
    /// <summary>
    /// 货品表 接口实现
    /// </summary>
    public class CoreCmsProductsServices : BaseServices<CoreCmsProducts>, ICoreCmsProductsServices
    {
        private readonly ICoreCmsProductsRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;

        public CoreCmsProductsServices(IUnitOfWork unitOfWork, ICoreCmsProductsRepository dal,
            IServiceProvider serviceProvider
            )
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 根据货品ID获取货品信息
        /// </summary>
        /// <param name="id">货品序列</param>
        /// <param name="isPromotion">是否计算促销</param>
        /// <param name="userId">用户序列</param>
        /// <param name="type">类型</param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<CoreCmsProducts> GetProductInfo(int id, bool isPromotion, int userId, string type = "goods", int groupId = 0)
        {
            using var container = _serviceProvider.CreateScope();

            var userGradeServices = container.ServiceProvider.GetService<ICoreCmsUserGradeServices>();
            var promotionServices = container.ServiceProvider.GetService<ICoreCmsPromotionServices>();
            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();
            var goodsServices = container.ServiceProvider.GetService<ICoreCmsGoodsServices>();
            var userServices = container.ServiceProvider.GetService<ICoreCmsUserServices>();
            var goodsGradeServices = container.ServiceProvider.GetService<ICoreCmsGoodsGradeServices>();
            var pinTuanRuleServices = container.ServiceProvider.GetService<ICoreCmsPinTuanRuleServices>();

            //获取货品
            var productModel = await _dal.QueryByClauseAsync(p => p.id == id);
            if (productModel == null) return null;
            //获取商品信息
            var goods = await goodsServices.QueryByIdAsync(productModel.goodsId);
            if (goods == null) return null;
            //DTO映射
            productModel.bn = goods.bn;
            productModel.images = !string.IsNullOrEmpty(productModel.images) ? GoodsHelper.GetOneImage(productModel.images) : GoodsHelper.GetOneImage(goods.images);
            productModel.totalStock = Convert.ToInt32(productModel.stock);
            productModel.stock = GoodsHelper.GetStock(productModel.stock, productModel.freezeStock);
            productModel.name = goods.name;

            var price = productModel.price;
            var gradePrice = new List<CoreCmsGoodsGrade>();
            //获取价格体系
            if (userId > 0)
            {
                //获取用户信息
                var userInfo = await userServices.QueryByIdAsync(userId);
                if (userInfo != null)
                {
                    var gradeInfo = await userGradeServices.QueryByIdAsync(userInfo.grade);
                    if (gradeInfo != null)
                    {
                        productModel.gradeInfo = gradeInfo;
                    }
                    var goodsGrades = await goodsGradeServices.QueryListByClauseAsync(p => p.goodsId == goods.id);
                    if (goodsGrades != null && goodsGrades.Count > 0)
                    {
                        var userGradeList = await userGradeServices.QueryAsync();
                        goodsGrades.ForEach(p =>
                        {
                            var userGrades = userGradeList.Find(o => o.id == p.gradeId);
                            p.gradeName = userGrades != null ? userGrades.title : "";

                            if (gradeInfo != null && gradeInfo.id == p.gradeId)
                            {
                                price = (productModel.price - p.gradePrice) > 0 ? productModel.price - p.gradePrice : 0;
                            }
                            p.gradePrice = (productModel.price - p.gradePrice) > 0 ? productModel.price - p.gradePrice : 0;
                        });
                    }
                    gradePrice = goodsGrades;
                }
            }
            productModel.gradePrice = gradePrice;
            productModel.price = price;

            //如果是多规格商品，算多规格
            if (goods.openSpec == 1 && !string.IsNullOrEmpty(goods.spesDesc))
            {
                var defaultSpec = new Dictionary<string, Dictionary<string, DefaultSpesDesc>>();
                //一级拆分
                var spesDescArr = goods.spesDesc.Split("|");
                var productSpecDescArr = productModel.spesDesc.Split(",");
                foreach (var item in spesDescArr)
                {
                    //小类拆分
                    var itemArr = item.Split(".");
                    //键值对处理
                    var keyValue = itemArr[1].Split(":");
                    var defaultSpesDesc = new DefaultSpesDesc();
                    defaultSpesDesc.name = keyValue[1];
                    foreach (var childItem in productSpecDescArr)
                    {
                        if (childItem == itemArr[1])
                        {
                            defaultSpesDesc.isDefault = true;
                        }
                    }
                    if (defaultSpec.ContainsKey(keyValue[0]))
                    {
                        defaultSpec[keyValue[0]].Add(keyValue[1], defaultSpesDesc);
                    }
                    else
                    {
                        var a = new Dictionary<string, DefaultSpesDesc> { { keyValue[1], defaultSpesDesc } };

                        defaultSpec.Add(keyValue[0], a);
                    }
                }
                //取其他货品信息
                var otherProduts = await _dal.QueryListByClauseAsync(t => t.goodsId == goods.id && t.isDel == false && t.id != productModel.id);
                if (otherProduts.Any())
                {
                    foreach (var item in defaultSpec)
                    {
                        foreach (var childItem in item.Value)
                        {
                            //如果是默认选中的，跳出本次
                            if (childItem.Value.isDefault) continue;
                            //当前主货品sku
                            var tempProductSpesDesc = productSpecDescArr;
                            //替换当前sku的当前值为当前遍历的值
                            for (var i = 0; i < tempProductSpesDesc.Length; i++)
                            {
                                if (tempProductSpesDesc[i].Contains(item.Key)) tempProductSpesDesc[i] = item.Key + ":" + childItem.Key;
                            }
                            //循环所有货品，找到对应的多规格
                            foreach (var o in otherProduts)
                            {
                                var otherProductSpesDesc = o.spesDesc.Split(",");
                                if (!tempProductSpesDesc.Except(otherProductSpesDesc).Any())
                                {
                                    childItem.Value.productId = o.id;
                                    break;
                                }
                            }
                        }
                    }
                }
                productModel.defaultSpecificationDescription = defaultSpec;
            }

            productModel.amount = productModel.price;
            productModel.promotionList = new Dictionary<int, WxNameTypeDto>();
            productModel.promotionAmount = 0;

            //开启计算促销
            if (isPromotion)
            {
                //模拟购物车数据库结构，去取促销信息
                var miniCart = new CartDto();
                miniCart.userId = userId;
                miniCart.goodsAmount = productModel.amount;
                miniCart.amount = productModel.amount;
                var listOne = new CartProducts()
                {
                    id = 0,
                    isSelect = true,
                    userId = userId,
                    productId = productModel.id,
                    nums = 1,
                    products = productModel
                };
                miniCart.list.Add(listOne);

                var cartModel = new CartDto();

                if (type == "group" || type == "skill")
                {
                    //团购秒杀默认时间过期后，不可以下单
                    var dt = DateTime.Now;
                    var promotionInfo = await promotionServices.QueryByClauseAsync(p => p.startTime < dt && p.endTime > dt && p.id == groupId);
                    if (promotionInfo != null)
                    {
                        await promotionServices.SetPromotion(promotionInfo, miniCart);
                    }
                    cartModel = miniCart;
                }
                else
                {
                    cartModel = await promotionServices.ToPromotion(miniCart);
                }

                //把促销信息和新的价格等，覆盖到这里
                var promotionList = cartModel.promotionList;

                if (cartModel.list[0].products.promotionList.Count > 0)
                {
                    //把订单促销和商品促销合并,都让他显示
                    foreach (KeyValuePair<int, WxNameTypeDto> kvp in cartModel.list[0].products.promotionList)
                    {
                        if (promotionList.ContainsKey(kvp.Key))
                        {
                            promotionList[kvp.Key] = kvp.Value;
                        }
                        else
                        {
                            promotionList.Add(kvp.Key, kvp.Value);
                        }
                    }
                }
                productModel.price = cartModel.list[0].products.price; //新的商品单价
                productModel.amount = cartModel.list[0].products.amount; //商品总价格
                productModel.promotionList = promotionList; //促销列表
                productModel.promotionAmount = cartModel.list[0].products.promotionAmount; //如果商品促销应用了，那么促销的金额
            }

            //获取活动数量
            if (type == "pinTuan")
            {
                //把拼团的一些属性等加上
                var pinTuanRule = pinTuanRuleServices.QueryMuchFirst<CoreCmsPinTuanRule, CoreCmsPinTuanGoods, CoreCmsPinTuanRule>(
                    (role, pinTuanGoods) => new object[] { JoinType.Inner, role.id == pinTuanGoods.ruleId }
                    , (role, pinTuanGoods) => role
                    , (role, pinTuanGoods) => pinTuanGoods.goodsId == productModel.goodsId);

                //var pinTuanRule = dbClient.Queryable<CoreCmsPinTuanRule, CoreCmsPinTuanGoods>(

                //    ).Where((role, pinTuanGoods) => pinTuanGoods.goodsId == productModel.goodsId)
                //    .Select((role, pinTuanGoods) => role).First();
                //调整前台显示数量
                var checkOrder = orderServices.FindLimitOrder(productModel.id, userId, pinTuanRule.startTime, pinTuanRule.endTime);
                if (pinTuanRule.maxGoodsNums != 0)
                {
                    //活动销售件数
                    productModel.stock = pinTuanRule.maxGoodsNums - checkOrder.TotalOrders;
                    productModel.buyPinTuanCount = checkOrder.TotalOrders;
                }
                else
                {
                    productModel.buyPinTuanCount = checkOrder.TotalOrders;
                }
            }
            else if (type == "group" || type == "skill")
            {
                if (!goodsServices.IsInGroup(productModel.goodsId, out var groupModel, groupId))
                {
                    return null;
                }
                //调整前台显示数量
                var checkOrder = orderServices.FindLimitOrder(productModel.id, userId, groupModel.startTime,
                    groupModel.endTime);
                if (groupModel.maxGoodsNums != 0)
                {
                    //活动销售件数
                    productModel.stock = groupModel.maxGoodsNums - checkOrder.TotalOrders;
                    productModel.buyPromotionCount = checkOrder.TotalOrders;
                }
                else
                {
                    productModel.buyPromotionCount = checkOrder.TotalOrders;
                }
            }
            return productModel;
        }

        /// <summary>
        /// 判断货品上下架状态
        /// </summary>
        /// <param name="productsId">货品序列</param>
        /// <returns></returns>
        public async Task<bool> GetShelfStatus(int productsId)
        {
            return await _dal.GetShelfStatus(productsId);
        }

        /// <summary>
        /// 获取库存报警数量
        /// </summary>
        /// <param name="goodsStocksWarn"></param>
        /// <returns></returns>
        public async Task<int> GoodsStaticsTotalWarn(int goodsStocksWarn)
        {
            return await _dal.GoodsStaticsTotalWarn(goodsStocksWarn);
        }


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
            return await _dal.QueryDetailPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }

        #endregion


        /// <summary>
        /// 修改单个货品库存并记入库存管理日志内
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> EditStock(int productId, int stock)
        {
            return await _dal.EditStock(productId, stock);
        }

        /// <summary>
        /// 获取所有货品数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoreCmsProducts>> GetProducts(int goodId = 0)
        {
            return await _dal.GetProducts(goodId);
        }

    }
}