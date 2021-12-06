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
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 分销商订单记录表 接口实现
    /// </summary>
    public class CoreCmsDistributionOrderServices : BaseServices<CoreCmsDistributionOrder>, ICoreCmsDistributionOrderServices
    {
        private readonly ICoreCmsDistributionOrderRepository _dal;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsDistributionServices _distributionServices;
        private readonly ICoreCmsOrderServices _orderServices;
        private readonly ICoreCmsOrderItemServices _orderItemServices;
        private readonly ICoreCmsProductsDistributionServices _productsDistributionServices;
        private readonly ICoreCmsProductsServices _productsServices;
        private readonly ICoreCmsUserBalanceServices _balanceServices;
        private readonly ICoreCmsGoodsServices _goodsServices;

        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsDistributionOrderServices(IUnitOfWork unitOfWork, ICoreCmsDistributionOrderRepository dal, ICoreCmsDistributionServices distributionServices, ICoreCmsUserBalanceServices balanceServices, ICoreCmsOrderServices orderServices, ICoreCmsUserServices userServices, ICoreCmsOrderItemServices orderItemServices, ICoreCmsProductsDistributionServices productsDistributionServices, ICoreCmsProductsServices productsServices, ICoreCmsGoodsServices goodsServices)
        {
            this._dal = dal;
            _distributionServices = distributionServices;
            _balanceServices = balanceServices;
            _orderServices = orderServices;
            _userServices = userServices;
            _orderItemServices = orderItemServices;
            _productsDistributionServices = productsDistributionServices;
            _productsServices = productsServices;
            _goodsServices = goodsServices;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsDistributionOrder entity)
        {
            return await _dal.InsertAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsDistributionOrder entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsDistributionOrder> entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            return await _dal.DeleteByIdAsync(id);
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            return await _dal.DeleteByIdsAsync(ids);
        }

        #endregion

        #region 重写根据条件查询分页数据
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
        public new async Task<IPageList<CoreCmsDistributionOrder>> QueryPageAsync(Expression<Func<CoreCmsDistributionOrder, bool>> predicate,
            Expression<Func<CoreCmsDistributionOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion


        #region 添加分销订单关联记录
        /// <summary>
        /// 添加分销订单关联记录
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> AddData(CoreCmsOrder order)
        {
            var jm = new WebApiCallBack();

            //查询获取几级返利
            var user = await _userServices.QueryByClauseAsync(p => p.id == order.userId);
            if (user is { parentId: > 0 })
            {
                //获取购物明细
                var orderItems = await _orderItemServices.QueryListByClauseAsync(p => p.orderId == order.orderId);
                var goodIds = orderItems.Select(p => p.goodsId).ToList();
                var productIds = orderItems.Select(p => p.productId).ToList();
                //获取货品数据
                var goods = await _goodsServices.QueryListByClauseAsync(p => goodIds.Contains(p.id));
                //获取货品数据
                var products = await _productsServices.QueryListByClauseAsync(p => productIds.Contains(p.id));
                //获取货品关联的分销数据
                var productsDistributions = await _productsDistributionServices.QueryListByClauseAsync(p => productIds.Contains(p.productsId));

                await AddOther(order, orderItems, goods, products, productsDistributions, 1, user.parentId); //本级是否返利
            }
            jm.status = true;

            return jm;
        }

        #endregion

        #region 循环插入上级

        /// <summary>
        /// 循环插入上级
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="orderItems"></param>
        /// <param name="goods"></param>
        /// <param name="products">订单货品</param>
        /// <param name="productsDistributions">货品分销数据</param>
        /// <param name="level">第几级</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        private async Task AddOther(CoreCmsOrder order, List<CoreCmsOrderItem> orderItems, List<CoreCmsGoods> goods, List<CoreCmsProducts> products, List<CoreCmsProductsDistribution> productsDistributions, int level = 0, int userId = 0)
        {
            var user = await _userServices.QueryByClauseAsync(p => p.id == userId);
            if (user != null)
            {
                var commission = await _distributionServices.GetGradeAndCommission(user.id);
                if (commission.status && commission.data != null) //不是分销商的，不返利。
                {
                    var ommissionDto = commission.data as DistributionDto;
                    //直返本级
                    decimal amount = 0;
                    foreach (var item in orderItems)
                    {
                        var good = goods.Find(p => p.id == item.goodsId);
                        if (good == null) continue;
                        var product = products.Find(p => p.id == item.productId);
                        if (product == null) continue;
                        if (good.productsDistributionType == (int)GlobalEnumVars.ProductsDistributionType.Global)
                        {
                            if (ommissionDto == null) continue;
                            //获取实际当前支付金额,减去优惠的金额
                            var itemAmount = item.amount - item.promotionAmount;
                            //如果去掉优惠需要负了，就为0
                            if (itemAmount < 0) itemAmount = 0;
                            //一级分销
                            if (level == 1 && ommissionDto.commission_1 != null)
                            {
                                if (ommissionDto.commission_1.type == (int)GlobalEnumVars.DistributionCommissiontype.COMMISSION_TYPE_FIXED)
                                {
                                    amount += ommissionDto.commission_1.discount;
                                }
                                else
                                {
                                    amount += Math.Round(ommissionDto.commission_1.discount * itemAmount / 100, 2);
                                }
                            }
                            //二级分销
                            else if (level == 2 && ommissionDto.commission_2 != null)
                            {
                                if (ommissionDto.commission_2.type == (int)GlobalEnumVars.DistributionCommissiontype.COMMISSION_TYPE_FIXED)
                                {
                                    amount += ommissionDto.commission_2.discount;
                                }
                                else
                                {
                                    amount += Math.Round(ommissionDto.commission_2.discount * itemAmount / 100, 2);
                                }
                            }
                            //三级分销
                            else if (level == 3 && ommissionDto.commission_3 != null)
                            {
                                if (ommissionDto.commission_3.type == (int)GlobalEnumVars.DistributionCommissiontype.COMMISSION_TYPE_FIXED)
                                {
                                    amount += ommissionDto.commission_3.discount;
                                }
                                else
                                {
                                    amount += Math.Round(ommissionDto.commission_3.discount * itemAmount / 100, 2);
                                }
                            }
                        }
                        else if (good.productsDistributionType == (int)GlobalEnumVars.ProductsDistributionType.Detail)
                        {
                            var productsDistribution = productsDistributions.Find(p => p.productsId == item.productId);
                            if (productsDistribution == null) continue;

                            if (level == 1 && productsDistribution.levelOne > 0)
                            {
                                amount += Math.Round(productsDistribution.levelOne * item.nums, 2);
                            }
                            else if (level == 2 && productsDistribution.levelTwo > 0)
                            {
                                amount += Math.Round(productsDistribution.levelTwo * item.nums, 2);
                            }
                            else if (level == 3 && productsDistribution.levelThree > 0)
                            {
                                amount += Math.Round(productsDistribution.levelThree * item.nums, 2);
                            }
                        }
                    }

                    if (amount > 0)
                    {
                        var iData = new CoreCmsDistributionOrder();
                        iData.userId = userId;
                        iData.buyUserId = order.userId;
                        iData.orderId = order.orderId;
                        iData.amount = amount;
                        iData.level = level;
                        iData.isSettlement = (int)GlobalEnumVars.DistributionOrderSettlementStatus.SettlementNo;  //默认未结算
                        iData.isDelete = false;
                        //判断是否返利过,有历史记录直接更新
                        var commissOrder = await _dal.QueryByClauseAsync(p => p.userId == userId && p.orderId == order.orderId);
                        if (commissOrder != null)
                        {
                            commissOrder.updateTime = DateTime.Now;
                            commissOrder.userId = iData.userId;
                            commissOrder.buyUserId = iData.buyUserId;
                            commissOrder.orderId = iData.orderId;
                            commissOrder.amount = iData.amount;
                            commissOrder.level = iData.level;
                            commissOrder.isSettlement = iData.isSettlement;
                            commissOrder.isDelete = iData.isDelete;
                            await _dal.UpdateAsync(commissOrder);
                        }
                        else
                        {
                            iData.createTime = DateTime.Now;
                            iData.updateTime = DateTime.Now;
                            await _dal.InsertAsync(iData);
                        }
                    }

                    if (user.parentId > 0 && ommissionDto != null && level < ommissionDto.DistributionLevel)
                    {
                        //返第二级
                        level++;
                        await AddOther(order, orderItems, goods, products, productsDistributions, level, user.parentId);
                    }
                }
            }
        }

        #endregion

        #region 订单结算处理事件
        /// <summary>
        /// 订单结算处理事件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> FinishOrder(string orderId)
        {
            var jm = new WebApiCallBack();

            var order = await _orderServices.QueryByClauseAsync(p => p.orderId == orderId && p.status == (int)GlobalEnumVars.OrderStatus.Complete);
            if (order == null)
            {
                jm.msg = "订单查询失败";
                return jm;
            }
            //更新
            var list = await _dal.QueryListByClauseAsync(p => p.orderId == orderId && p.isSettlement == (int)GlobalEnumVars.DistributionOrderSettlementStatus.SettlementNo);
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    //钱挪到会员余额里面
                    var result = await _balanceServices.Change(item.userId, (int)GlobalEnumVars.UserBalanceSourceTypes.Distribution,
                         item.amount, item.orderId);
                    if (!result.status)
                    {

                    }
                }
                await _dal.UpdateAsync(p => new CoreCmsDistributionOrder()
                {
                    isSettlement = (int)GlobalEnumVars.DistributionOrderSettlementStatus.SettlementYes,
                    updateTime = DateTime.Now
                }, p => p.orderId == orderId && p.isSettlement == (int)GlobalEnumVars.DistributionOrderSettlementStatus.SettlementNo);
            }

            return jm;
        }
        #endregion

        #region 作废订单
        /// <summary>
        /// 作废订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CancleOrderByOrderId(string orderId)
        {
            var jm = new WebApiCallBack();

            var res = await _dal.UpdateAsync(p => new CoreCmsDistributionOrder() { isSettlement = (int)GlobalEnumVars.DistributionOrderSettlementStatus.SettlementCancel },
                p => p.orderId == orderId && p.isSettlement == (int)GlobalEnumVars.DistributionOrderSettlementStatus.SettlementNo);
            if (res == false)
            {
                jm.msg = "该未结算的订单不存在";
                return jm;
            }
            jm.msg = "操作成功";
            jm.status = true;


            return jm;
        }
        #endregion


        #region 获取下级推广订单数量
        /// <summary>
        ///     获取下级推广订单数量
        /// </summary>
        /// <param name="parentId">父类序列</param>
        /// <param name="type">1获取1级，其他为2级,0为全部</param>
        /// <param name="thisMonth">显示当月</param>
        /// <returns></returns>
        public async Task<int> QueryChildOrderCountAsync(int parentId, int type = 1, bool thisMonth = false)
        {
            return await _dal.QueryChildOrderCountAsync(parentId, type, thisMonth);

        }
        #endregion


        #region 获取下级推广订单金额
        /// <summary>
        ///     获取下级推广订单金额
        /// </summary>
        /// <param name="parentId">父类序列</param>
        /// <param name="type">1获取1级，其他为2级,0为全部</param>
        /// <param name="thisMonth">显示当月</param>
        /// <returns></returns>
        public async Task<decimal> QueryChildOrderMoneySumAsync(int parentId, int type = 1, bool thisMonth = false)
        {

            return await _dal.QueryChildOrderMoneySumAsync(parentId, type, thisMonth);

        }
        #endregion
    }
}
