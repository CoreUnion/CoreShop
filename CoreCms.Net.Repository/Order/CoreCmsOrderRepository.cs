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
using System.Text;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 订单表 接口实现
    /// </summary>
    public class CoreCmsOrderRepository : BaseRepository<CoreCmsOrder>, ICoreCmsOrderRepository
    {
        public CoreCmsOrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }


        #region 查询团购秒杀下单数量
        /// <summary>
        /// 查询团购秒杀下单数量
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public FindLimitOrderDto FindLimitOrder(int productId, int userId, DateTime? startTime, DateTime? endTime, int orderType = 0)
        {
            var dto = new FindLimitOrderDto();

            var statusArr = new int[]{(int)GlobalEnumVars.OrderStatus.Normal,
                (int)GlobalEnumVars.OrderStatus.Complete};

            var payStatusArr = new int[] { (int)GlobalEnumVars.OrderPayStatus.No, (int)GlobalEnumVars.OrderPayStatus.Yes, (int)GlobalEnumVars.OrderPayStatus.PartialYes };
            var shipStatusArr = new int[] { (int)GlobalEnumVars.OrderShipStatus.No, (int)GlobalEnumVars.OrderShipStatus.Yes, (int)GlobalEnumVars.OrderShipStatus.PartialYes };


            var queryable = DbClient.Queryable<CoreCmsOrderItem, CoreCmsOrder>((orderItem, orderModel) => new object[]
                {
                    JoinType.Inner, orderItem.orderId == orderModel.orderId
                });

            //计算订单总量
            queryable.WhereIF(productId > 0, (orderItem, orderModel) => orderItem.productId == productId);
            queryable.In((orderItem, orderModel) => orderModel.status, payStatusArr);
            //在活动时间范围内
            queryable.WhereIF(startTime != null, (orderItem, orderModel) => orderModel.createTime >= startTime);
            queryable.WhereIF(endTime != null, (orderItem, orderModel) => orderModel.createTime < endTime);
            //已退款、已退货、部分退款的、部分退货的排除
            queryable.In((orderItem, orderModel) => orderModel.payStatus, payStatusArr);
            queryable.In((orderItem, orderModel) => orderModel.shipStatus, shipStatusArr);


            //订单类型
            if (orderType > 0)
            {
                queryable.Where((orderItem, orderModel) => orderModel.orderType == orderType);
            }

            dto.TotalOrders = queryable.Clone().Select((orderItem, orderModel) => new { nums = orderItem.nums }).MergeTable().With(SqlWith.Null).Sum(p => p.nums);

            if (userId <= 0) return dto;
            {
                queryable.Where((orderItem, orderModel) => orderModel.userId == userId);
                dto.TotalUserOrders = queryable.Clone().Select((orderItem, orderModel) => new { nums = orderItem.nums }).MergeTable().With(SqlWith.Null).Sum(p => p.nums);
            }
            return dto;
        }

        #endregion

        #region 根据用户id和商品id获取下了多少订单
        /// <summary>
        /// 根据用户id和商品id获取下了多少订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodId"></param>
        /// <returns></returns>
        public int GetOrderNum(int userId, int goodId)
        {
            var num = DbClient.Queryable<CoreCmsOrder, CoreCmsOrderItem>((op, ot) => new object[]
            {
                JoinType.Inner, op.orderId == ot.orderId
            }).Where((op, ot) => op.userId == userId && ot.goodsId == goodId).Count();
            return num;
        }
        #endregion

        #region 重写根据条件列表数据
        /// <summary>
        ///     重写根据条件列表数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<List<CoreCmsOrder>> QueryListAsync(Expression<Func<CoreCmsOrder, bool>> predicate,
            Expression<Func<CoreCmsOrder, object>> orderByExpression, OrderByType orderByType)
        {
            List<CoreCmsOrder> list = await DbClient.Queryable<CoreCmsOrder, CoreCmsUser>((sOrder, sUser) => new JoinQueryInfos(
                     JoinType.Left, sOrder.userId == sUser.id))
                    .Select((sOrder, sUser) => new CoreCmsOrder
                    {
                        orderId = sOrder.orderId,
                        goodsAmount = sOrder.goodsAmount,
                        payedAmount = sOrder.payedAmount,
                        orderAmount = sOrder.orderAmount,
                        payStatus = sOrder.payStatus,
                        shipStatus = sOrder.shipStatus,
                        status = sOrder.status,
                        orderType = sOrder.orderType,
                        receiptType = sOrder.receiptType,
                        paymentCode = sOrder.paymentCode,
                        paymentTime = sOrder.paymentTime,
                        logisticsId = sOrder.logisticsId,
                        logisticsName = sOrder.logisticsName,
                        costFreight = sOrder.costFreight,
                        userId = sOrder.userId,
                        sellerId = sOrder.sellerId,
                        confirmStatus = sOrder.confirmStatus,
                        confirmTime = sOrder.confirmTime,
                        storeId = sOrder.storeId,
                        shipAreaId = sOrder.shipAreaId,
                        shipAddress = sOrder.shipAddress,
                        shipName = sOrder.shipName,
                        shipMobile = sOrder.shipMobile,
                        weight = sOrder.weight,
                        taxType = sOrder.taxType,
                        taxCode = sOrder.taxCode,
                        taxTitle = sOrder.taxTitle,
                        point = sOrder.point,
                        pointMoney = sOrder.pointMoney,
                        orderDiscountAmount = sOrder.orderDiscountAmount,
                        goodsDiscountAmount = sOrder.goodsDiscountAmount,
                        couponDiscountAmount = sOrder.couponDiscountAmount,
                        coupon = sOrder.coupon,
                        promotionList = sOrder.promotionList,
                        memo = sOrder.memo,
                        ip = sOrder.ip,
                        mark = sOrder.mark,
                        source = sOrder.source,
                        isComment = sOrder.isComment,
                        isdel = sOrder.isdel,
                        createTime = sOrder.createTime,
                        updateTime = sOrder.updateTime,
                        userNickName = sUser.nickName
                    })
                    .With(SqlWith.NoLock)
                    .MergeTable()
                    .Mapper(sOrder => sOrder.aftersalesItem, sOrder => sOrder.aftersalesItem.First().orderId)
                    .Mapper(sOrder => sOrder.items, sOrder => sOrder.items.First().orderId)
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .ToListAsync();
            return list;
        }

        #endregion

        #region 重写根据条件查询分页数据-带用户数据
        /// <summary>
        ///     重写根据条件查询分页数据-带用户数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<IPageList<CoreCmsOrder>> QueryPageAsync(Expression<Func<CoreCmsOrder, bool>> predicate,
            Expression<Func<CoreCmsOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsOrder> page = await DbClient.Queryable<CoreCmsOrder, CoreCmsUser>((sOrder, sUser) => new JoinQueryInfos(
                     JoinType.Left, sOrder.userId == sUser.id))
                    .Select((sOrder, sUser) => new CoreCmsOrder
                    {
                        orderId = sOrder.orderId,
                        goodsAmount = sOrder.goodsAmount,
                        payedAmount = sOrder.payedAmount,
                        orderAmount = sOrder.orderAmount,
                        payStatus = sOrder.payStatus,
                        shipStatus = sOrder.shipStatus,
                        status = sOrder.status,
                        orderType = sOrder.orderType,
                        receiptType = sOrder.receiptType,
                        paymentCode = sOrder.paymentCode,
                        paymentTime = sOrder.paymentTime,
                        logisticsId = sOrder.logisticsId,
                        logisticsName = sOrder.logisticsName,
                        costFreight = sOrder.costFreight,
                        userId = sOrder.userId,
                        sellerId = sOrder.sellerId,
                        confirmStatus = sOrder.confirmStatus,
                        confirmTime = sOrder.confirmTime,
                        storeId = sOrder.storeId,
                        shipAreaId = sOrder.shipAreaId,
                        shipAddress = sOrder.shipAddress,
                        shipName = sOrder.shipName,
                        shipMobile = sOrder.shipMobile,
                        weight = sOrder.weight,
                        taxType = sOrder.taxType,
                        taxCode = sOrder.taxCode,
                        taxTitle = sOrder.taxTitle,
                        point = sOrder.point,
                        pointMoney = sOrder.pointMoney,
                        orderDiscountAmount = sOrder.orderDiscountAmount,
                        goodsDiscountAmount = sOrder.goodsDiscountAmount,
                        couponDiscountAmount = sOrder.couponDiscountAmount,
                        coupon = sOrder.coupon,
                        promotionList = sOrder.promotionList,
                        memo = sOrder.memo,
                        ip = sOrder.ip,
                        mark = sOrder.mark,
                        source = sOrder.source,
                        isComment = sOrder.isComment,
                        isdel = sOrder.isdel,
                        createTime = sOrder.createTime,
                        updateTime = sOrder.updateTime,
                        userNickName = sUser.nickName
                    })
                    .MergeTable()
                    .Mapper(sOrder => sOrder.aftersalesItem, sOrder => sOrder.aftersalesItem.First().orderId)
                    .Mapper(sOrder => sOrder.items, sOrder => sOrder.items.First().orderId)
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);

            var list = new PageList<CoreCmsOrder>(page, pageIndex, pageSize, totalCount);
            return list;
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
        public async Task<IPageList<CoreCmsOrder>> QueryPageNewAsync(Expression<Func<CoreCmsOrder, bool>> predicate,
            Expression<Func<CoreCmsOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsOrder> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsOrder>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsOrder
                {
                    orderId = p.orderId,
                    goodsAmount = p.goodsAmount,
                    payedAmount = p.payedAmount,
                    orderAmount = p.orderAmount,
                    payStatus = p.payStatus,
                    shipStatus = p.shipStatus,
                    status = p.status,
                    orderType = p.orderType,
                    receiptType = p.receiptType,
                    paymentCode = p.paymentCode,
                    paymentTime = p.paymentTime,
                    logisticsId = p.logisticsId,
                    logisticsName = p.logisticsName,
                    costFreight = p.costFreight,
                    userId = p.userId,
                    sellerId = p.sellerId,
                    confirmStatus = p.confirmStatus,
                    confirmTime = p.confirmTime,
                    storeId = p.storeId,
                    shipAreaId = p.shipAreaId,
                    shipAddress = p.shipAddress,
                    shipName = p.shipName,
                    shipMobile = p.shipMobile,
                    weight = p.weight,
                    taxType = p.taxType,
                    taxCode = p.taxCode,
                    taxTitle = p.taxTitle,
                    point = p.point,
                    pointMoney = p.pointMoney,
                    orderDiscountAmount = p.orderDiscountAmount,
                    goodsDiscountAmount = p.goodsDiscountAmount,
                    couponDiscountAmount = p.couponDiscountAmount,
                    coupon = p.coupon,
                    promotionList = p.promotionList,
                    memo = p.memo,
                    ip = p.ip,
                    mark = p.mark,
                    source = p.source,
                    isComment = p.isComment,
                    isdel = p.isdel,
                    createTime = p.createTime,
                    updateTime = p.updateTime,

                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsOrder>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsOrder
                {
                    orderId = p.orderId,
                    goodsAmount = p.goodsAmount,
                    payedAmount = p.payedAmount,
                    orderAmount = p.orderAmount,
                    payStatus = p.payStatus,
                    shipStatus = p.shipStatus,
                    status = p.status,
                    orderType = p.orderType,
                    receiptType = p.receiptType,
                    paymentCode = p.paymentCode,
                    paymentTime = p.paymentTime,
                    logisticsId = p.logisticsId,
                    logisticsName = p.logisticsName,
                    costFreight = p.costFreight,
                    userId = p.userId,
                    sellerId = p.sellerId,
                    confirmStatus = p.confirmStatus,
                    confirmTime = p.confirmTime,
                    storeId = p.storeId,
                    shipAreaId = p.shipAreaId,
                    shipAddress = p.shipAddress,
                    shipName = p.shipName,
                    shipMobile = p.shipMobile,
                    weight = p.weight,
                    taxType = p.taxType,
                    taxCode = p.taxCode,
                    taxTitle = p.taxTitle,
                    point = p.point,
                    pointMoney = p.pointMoney,
                    orderDiscountAmount = p.orderDiscountAmount,
                    goodsDiscountAmount = p.goodsDiscountAmount,
                    couponDiscountAmount = p.couponDiscountAmount,
                    coupon = p.coupon,
                    promotionList = p.promotionList,
                    memo = p.memo,
                    ip = p.ip,
                    mark = p.mark,
                    source = p.source,
                    isComment = p.isComment,
                    isdel = p.isdel,
                    createTime = p.createTime,
                    updateTime = p.updateTime,

                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsOrder>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion


    }
}
