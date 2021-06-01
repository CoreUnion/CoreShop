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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     代理商订单记录表 接口实现
    /// </summary>
    public class CoreCmsAgentOrderServices : BaseServices<CoreCmsAgentOrder>, ICoreCmsAgentOrderServices
    {
        private readonly ICoreCmsAgentGoodsServices _agentGoodsServices;
        private readonly ICoreCmsAgentProductsServices _agentProductsServices;
        private readonly ICoreCmsUserBalanceServices _balanceServices;
        private readonly ICoreCmsAgentOrderRepository _dal;
        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly ICoreCmsOrderItemServices _orderItemServices;
        private readonly ICoreCmsOrderServices _orderServices;
        private readonly ICoreCmsProductsServices _productsServices;


        private readonly IServiceProvider _serviceProvider;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICoreCmsUserServices _userServices;


        public CoreCmsAgentOrderServices(IUnitOfWork unitOfWork, ICoreCmsAgentOrderRepository dal,
            ICoreCmsUserServices userServices, ICoreCmsOrderItemServices orderItemServices,
            ICoreCmsProductsServices productsServices, ICoreCmsGoodsServices goodsServices,
            ICoreCmsAgentProductsServices agentProductsServices, ICoreCmsSettingServices settingServices,
            ICoreCmsAgentGoodsServices agentGoodsServices, IServiceProvider serviceProvider,
            ICoreCmsOrderServices orderServices, ICoreCmsUserBalanceServices balanceServices)
        {
            _dal = dal;
            BaseDal = dal;
            _unitOfWork = unitOfWork;
            _userServices = userServices;
            _orderItemServices = orderItemServices;
            _productsServices = productsServices;
            _goodsServices = goodsServices;
            _agentProductsServices = agentProductsServices;
            _settingServices = settingServices;
            _agentGoodsServices = agentGoodsServices;
            _serviceProvider = serviceProvider;
            _orderServices = orderServices;
            _balanceServices = balanceServices;
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
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<IPageList<CoreCmsAgentOrder>> QueryPageAsync(
            Expression<Func<CoreCmsAgentOrder, bool>> predicate,
            Expression<Func<CoreCmsAgentOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize,
                blUseNoLock);
        }

        #endregion

        #region 添加代理订单关联记录

        /// <summary>
        ///     添加代理订单关联记录
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> AddData(CoreCmsOrder order)
        {
            var jm = new WebApiCallBack();

            using var container = _serviceProvider.CreateScope();

            var agentServices = container.ServiceProvider.GetService<ICoreCmsAgentServices>();


            var allConfigs = await _settingServices.GetConfigDictionaries();
            var isAllowProcurementService = CommonHelper
                .GetConfigDictionary(allConfigs, SystemSettingConstVars.IsAllowProcurementService).ObjectToInt(0);
            CoreCmsUser user = null;
            CoreCmsAgent agentModel = null;
            //判断是否支持代理代购，支持的话就直接判断当前订单用户是否是代理商，是的话传代理商数据
            if (isAllowProcurementService == 1)
            {
                agentModel = await agentServices.QueryByClauseAsync(p =>
                    p.userId == order.userId && p.verifyStatus == (int) GlobalEnumVars.AgentVerifyStatus.VerifyYes);
                if (agentModel != null) user = await _userServices.QueryByClauseAsync(p => p.id == order.userId);
            }

            //如果当前用户不是代理，则找上级
            if (user == null)
            {
                var userChild = await _userServices.QueryByClauseAsync(p => p.id == order.userId);
                if (userChild.parentId > 0)
                {
                    agentModel = await agentServices.QueryByClauseAsync(p =>
                        p.userId == userChild.parentId &&
                        p.verifyStatus == (int) GlobalEnumVars.AgentVerifyStatus.VerifyYes);
                    if (agentModel != null)
                        user = await _userServices.QueryByClauseAsync(p => p.id == userChild.parentId);
                }
            }

            //查询获取几级返利

            if (user != null)
            {
                //获取购物明细
                var orderItems = await _orderItemServices.QueryListByClauseAsync(p => p.orderId == order.orderId);
                var goodIds = orderItems.Select(p => p.goodsId).ToList();
                var productIds = orderItems.Select(p => p.productId).ToList();
                //获取商品数据
                var goods = await _goodsServices.QueryListByClauseAsync(p => goodIds.Contains(p.id));
                //获取货品数据
                var products = await _productsServices.QueryListByClauseAsync(p => productIds.Contains(p.id));
                //获取当前订单包含的商品在代理商货品池启用商品数据
                var agentGoods =
                    await _agentGoodsServices.QueryListByClauseAsync(p => goodIds.Contains(p.goodId) && p.isEnable);
                //获取货品关联的分销数据
                var agentProducts = await _agentProductsServices.QueryListByClauseAsync(p =>
                    productIds.Contains(p.productId) && p.agentGradeId == agentModel.gradeId);

                if (agentGoods.Any() && agentProducts.Any())
                    await AddOther(order, orderItems, goods, products, agentGoods, agentProducts, agentModel, user);
                else
                    jm.msg = "代理商商品池或货品池为空";
                jm.status = true;
            }
            else
            {
                jm.status = false;
            }

            return jm;
        }

        #endregion

        #region 订单结算处理事件

        /// <summary>
        ///     订单结算处理事件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> FinishOrder(string orderId)
        {
            var jm = new WebApiCallBack();

            var order = await _orderServices.QueryByClauseAsync(p =>
                p.orderId == orderId && p.status == (int) GlobalEnumVars.OrderStatus.Complete);
            if (order == null)
            {
                jm.msg = "订单查询失败";
                return jm;
            }

            //更新
            var list = await _dal.QueryListByClauseAsync(p =>
                p.orderId == orderId && p.isSettlement == (int) GlobalEnumVars.AgentOrderSettlementStatus.SettlementNo);
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    //钱挪到会员余额里面
                    var result = await _balanceServices.Change(item.userId,
                        (int) GlobalEnumVars.UserBalanceSourceTypes.Agent,
                        item.amount, item.orderId);
                    if (!result.status)
                    {
                    }
                }

                await _dal.UpdateAsync(
                    p => new CoreCmsAgentOrder
                    {
                        isSettlement = (int) GlobalEnumVars.AgentOrderSettlementStatus.SettlementYes,
                        updateTime = DateTime.Now
                    },
                    p => p.orderId == orderId &&
                         p.isSettlement == (int) GlobalEnumVars.AgentOrderSettlementStatus.SettlementNo);
            }

            return jm;
        }

        #endregion

        #region 作废订单

        /// <summary>
        ///     作废订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CancleOrderByOrderId(string orderId)
        {
            var jm = new WebApiCallBack();

            var res = await _dal.UpdateAsync(
                p => new CoreCmsAgentOrder
                    {isSettlement = (int) GlobalEnumVars.AgentOrderSettlementStatus.SettlementCancel},
                p => p.orderId == orderId &&
                     p.isSettlement == (int) GlobalEnumVars.AgentOrderSettlementStatus.SettlementNo);
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

        #region 循环插入上级

        /// <summary>
        ///     循环插入上级
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="orderItems"></param>
        /// <param name="goods"></param>
        /// <param name="products">订单货品</param>
        /// <param name="agentProducts">商品池货品价格体系数据</param>
        /// <param name="agentGoods">商品池数据</param>
        /// <param name="agent">代理商数据</param>
        /// <param name="user">用户数据</param>
        /// <returns></returns>
        private async Task AddOther(CoreCmsOrder order, List<CoreCmsOrderItem> orderItems, List<CoreCmsGoods> goods,
            List<CoreCmsProducts> products, List<CoreCmsAgentGoods> agentGoods,
            List<CoreCmsAgentProducts> agentProducts, CoreCmsAgent agent, CoreCmsUser user)
        {
            //直返本级
            decimal amount = 0;
            foreach (var item in orderItems)
            {
                //判断是否存在商品内
                var good = goods.Find(p => p.id == item.goodsId);
                if (good == null) continue;
                //判断是否存在货品类
                var product = products.Find(p => p.id == item.productId);
                if (product == null) continue;
                //判断代理商代理池是否包含此商品数据
                var agentGood = agentGoods.Find(p => p.goodId == item.goodsId);
                if (agentGood == null) continue;

                //判断代理商代理池是否包含此货品数据
                var agentProduct = agentProducts.Find(p => p.productId == item.productId);
                if (agentProduct == null) continue;

                //获取实际当前单个商品应获得利润
                var price = item.price - agentProduct.agentGradePrice;
                //如果销售价减去代理商价格负了，就不计算了。
                if (price < 0) continue;

                //如果利润减去优惠负了，就不计算了。
                var mathMoney = Math.Round(price * item.nums - item.promotionAmount, 2);
                if (mathMoney < 0) mathMoney = 0;

                //单个商品利润*数量，再减去优惠金额
                amount += mathMoney;
            }

            if (amount > 0)
            {
                var iData = new CoreCmsAgentOrder();
                iData.userId = user.id;
                iData.buyUserId = order.userId;
                iData.orderId = order.orderId;
                iData.amount = amount;
                iData.isSettlement = (int) GlobalEnumVars.AgentOrderSettlementStatus.SettlementNo; //默认未结算
                iData.isDelete = false;
                //判断是否返利过,有历史记录直接更新
                var agentOrder = await _dal.QueryByClauseAsync(p => p.userId == user.id && p.orderId == order.orderId);
                if (agentOrder != null)
                {
                    agentOrder.updateTime = DateTime.Now;
                    agentOrder.userId = iData.userId;
                    agentOrder.buyUserId = iData.buyUserId;
                    agentOrder.orderId = iData.orderId;
                    agentOrder.amount = iData.amount;
                    agentOrder.isSettlement = iData.isSettlement;
                    agentOrder.isDelete = iData.isDelete;
                    await _dal.UpdateAsync(agentOrder);
                }
                else
                {
                    iData.createTime = DateTime.Now;
                    iData.updateTime = DateTime.Now;
                    await _dal.InsertAsync(iData);
                }
            }
        }

        #endregion

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        ///     重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsAgentOrder entity)
        {
            return await _dal.InsertAsync(entity);
        }

        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsAgentOrder entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsAgentOrder> entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            return await _dal.DeleteByIdAsync(id);
        }

        /// <summary>
        ///     重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            return await _dal.DeleteByIdsAsync(ids);
        }

        #endregion
    }
}