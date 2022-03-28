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
using CoreCms.Net.Caching.AutoMate.RedisCache;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.QueryMuch;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 退货单表 接口实现
    /// </summary>
    public class CoreCmsBillAftersalesServices : BaseServices<CoreCmsBillAftersales>, ICoreCmsBillAftersalesServices
    {
        private readonly ICoreCmsBillAftersalesRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICoreCmsMessageCenterServices _messageCenterServices;
        private readonly ICoreCmsUserPointLogServices _userPointLogServices;
        private readonly IRedisOperationRepository _redisOperationRepository;
        
        public CoreCmsBillAftersalesServices(IUnitOfWork unitOfWork, ICoreCmsBillAftersalesRepository dal, IServiceProvider serviceProvider, ICoreCmsMessageCenterServices messageCenterServices, ICoreCmsUserPointLogServices userPointLogServices, IRedisOperationRepository redisOperationRepository)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _messageCenterServices = messageCenterServices;
            _userPointLogServices = userPointLogServices;
            _redisOperationRepository = redisOperationRepository;
        }

        #region 根据订单号查询已经售后的内容======================

        /// <summary>
        /// 根据订单号查询已经售后的内容
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="aftersaleLevel">取售后单的时候，售后单的等级，0：待审核的和审核通过的售后单，1未审核的，2审核通过的</param>
        /// <returns></returns>
        public WebApiCallBack OrderToAftersales(string orderId, int aftersaleLevel = 0)
        {
            var jm = new WebApiCallBack();

            List<int> statusInts = new List<int>();
            switch (aftersaleLevel)
            {
                case 0:
                    statusInts.Add((int)GlobalEnumVars.BillAftersalesStatus.Success);
                    statusInts.Add((int)GlobalEnumVars.BillAftersalesStatus.WaitAudit);
                    break;
                case 1:
                    statusInts.Add((int)GlobalEnumVars.BillAftersalesStatus.WaitAudit);
                    break;
                case 2:
                    statusInts.Add((int)GlobalEnumVars.BillAftersalesStatus.Success);
                    break;
                default:
                    jm.msg = "aftersale_level值类型不对";
                    return jm;
            }
            //算已经退过款的金额，取已经完成的售后单的金额汇总
            var where = PredicateBuilder.True<CoreCmsBillAftersales>();
            where = where.And(p => p.orderId == orderId && statusInts.Contains(p.status));  //加上待审核状态，这样申请过售后的商品和金额不会再重复申请了
                                                                                            //已经退过款的金额
            var refundMoney = base.GetSum(where, p => p.refundAmount);

            //算退货商品明细
            var list = base.QueryMuch<CoreCmsBillAftersalesItem, CoreCmsBillAftersales, QMAftersalesItems>(
                (child, parent) => new object[]
                {
                        JoinType.Inner, child.aftersalesId == parent.aftersalesId
                },
                (child, parent) => new QMAftersalesItems
                {
                    orderItemsId = child.orderItemsId,
                    nums = child.nums,
                    status = parent.status,
                    type = parent.type
                }, (child, parent) => parent.orderId == orderId && statusInts.Contains(parent.status));

            var reshipGoods = new Dictionary<int, reshipGoods>();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var reshipGoodsItem = new reshipGoods()
                    {
                        reshipNums = 0,
                        reshipedNums = 0
                    };
                    reshipGoodsItem.reshipNums += item.nums;
                    if (item.type == (int)GlobalEnumVars.BillAftersalesIsReceive.Reship)
                    {
                        reshipGoodsItem.reshipedNums += item.nums;
                    }
                    reshipGoods.Add(item.orderItemsId, reshipGoodsItem);
                }
            }

            var billAftersales = base.QueryListByClause(where);
            jm.data = new OrderToAftersalesDto
            {
                refundMoney = refundMoney,
                reshipGoods = reshipGoods,
                billAftersales = billAftersales
            };
            jm.status = true;
            jm.msg = jm.status ? GlobalConstVars.GetDataSuccess : GlobalConstVars.GetDataFailure;

            return jm;
        }

        #endregion

        #region 统计用户的售后数量============================================
        /// <summary>
        /// 统计用户的售后数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<int> GetUserAfterSalesNum(int userId, int status)
        {
            var count = await base.GetCountAsync(p => p.userId == userId && p.status == status);
            return count;
        }
        #endregion

        #region 创建售后单
        /// <summary>
        /// 创建售后单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId">发起售后的订单</param>
        /// <param name="type">是否收到退货，1未收到退货，不会创建退货单，2收到退货，会创建退货单,只有未发货的商品才能选择未收到货，只有已发货的才能选择已收到货</param>
        /// <param name="items">如果是退款退货，退货的明细 以 [[order_item_id=>nums]]的二维数组形式传值</param>
        /// <param name="images"></param>
        /// <param name="reason">售后理由</param>
        /// <param name="refund">退款金额，只在退款退货的时候用，如果是退款，直接就是订单金额</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ToAdd(int userId, string orderId, int type, JArray items, string[] images, string reason, decimal refund)
        {
            var jm = new WebApiCallBack();

            //做个简单校验，防止乱传值
            if (type != (int)GlobalEnumVars.BillAftersalesIsReceive.Refund && type != (int)GlobalEnumVars.BillAftersalesIsReceive.Reship)
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                jm.code = 10000;
                return jm;
            }

            using var container = _serviceProvider.CreateScope();
            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();
            var settingServices = container.ServiceProvider.GetService<ICoreCmsSettingServices>();
            var imagesServices = container.ServiceProvider.GetService<ICoreCmsBillAftersalesImagesServices>();
            var itemServices = container.ServiceProvider.GetService<ICoreCmsBillAftersalesItemServices>();
            var result = await orderServices.GetOrderInfoByOrderId(orderId, userId);
            if (result.status == false)
            {
                jm.msg = GlobalErrorCodeVars.Code13101;
                jm.code = 13101;
                return jm;
            }

            var orderInfo = new CoreCmsOrder();
            orderInfo = result.data as CoreCmsOrder;

            if (orderInfo.addAftersalesStatus == false)
            {
                jm.msg = GlobalErrorCodeVars.Code13200;
                jm.code = 13200;
                return jm;
            }
            //生成售后单号
            var aftersalesId = CommonHelper.GetSerialNumberType((int)GlobalEnumVars.SerialNumberType.售后单编号);

            //校验订单是否可以进行此售后，并且校验订单价格是否合理
            var verifyResult = Verify(type, orderInfo, refund, items);
            if (verifyResult.status == false)
            {
                return verifyResult;
            }

            jm.otherData = new
            {
                orderInfo,
                items,
                verifyResult
            };


            //判断图片是否大于系统限定

            //var allConfigs = await settingServices.GetConfigDictionaries();
            if (images.Length > 5)
            {
                jm.msg = GlobalErrorCodeVars.Code10006;
                jm.code = 10006;
                return jm;
            }


            var billAftersales = new CoreCmsBillAftersales();
            billAftersales.aftersalesId = aftersalesId;
            billAftersales.orderId = orderId;
            billAftersales.userId = userId;
            billAftersales.type = type;
            billAftersales.refundAmount = refund;
            billAftersales.reason = reason;
            billAftersales.status = (int)GlobalEnumVars.BillAftersalesStatus.WaitAudit;
            billAftersales.createTime = DateTime.Now;
            //保存主表数据
            await _dal.InsertAsync(billAftersales);

            if (items != null && items.Any())
            {
                //如果是退货，判断退货明细，数量是否超出可退的数量
                var aftersalesItems = formatAftersalesItems(orderInfo, items, aftersalesId);
                if (!aftersalesItems.status)
                {
                    return aftersalesItems;
                }
                //保存售后明细
                if (aftersalesItems.data != null)
                {
                    var list = aftersalesItems.data as List<CoreCmsBillAftersalesItem>;
                    await itemServices.InsertAsync(list);
                }
            }

            //保存图片
            if (images.Length > 0)
            {
                var imagesList = new List<CoreCmsBillAftersalesImages>();
                for (int i = 0; i < images.Length; i++)
                {
                    imagesList.Add(new CoreCmsBillAftersalesImages()
                    {
                        aftersalesId = aftersalesId,
                        imageUrl = images[i],
                        sortId = i
                    });
                }
                await imagesServices.InsertAsync(imagesList);
            }

            //消息模板推送给客户
            SmsHelper.SendMessage();

            jm.status = true;
            jm.data = billAftersales;

            return jm;

        }

        #endregion

        #region 校验是否可以进行售后
        /// <summary>
        /// 校验是否可以进行售后
        /// </summary>
        /// <param name="type"></param>
        /// <param name="orderInfo"></param>
        /// <param name="refund"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        private WebApiCallBack Verify(int type, CoreCmsOrder orderInfo, decimal refund, JArray items)
        {
            var jm = new WebApiCallBack();
            //判断订单是否是可以售后
            //只有活动订单才能售后
            if (orderInfo.status != (int)GlobalEnumVars.OrderStatus.Normal)
            {
                jm.msg = GlobalErrorCodeVars.Code13200;
                jm.code = 13200;
                return jm;
            }
            //未付款订单和已退款订单不能售后
            if (orderInfo.payStatus == (int)GlobalEnumVars.OrderPayStatus.No || orderInfo.payStatus == (int)GlobalEnumVars.OrderPayStatus.Refunded)
            {
                jm.msg = GlobalErrorCodeVars.Code13203;
                jm.code = 13203;
                return jm;
            }
            //如果订单未发货，那么用户不能选择已收到货
            if (type == (int)GlobalEnumVars.BillAftersalesIsReceive.Reship && orderInfo.shipStatus == (int)GlobalEnumVars.OrderShipStatus.No)
            {
                jm.msg = GlobalErrorCodeVars.Code13227;
                jm.code = 13227;
                return jm;
            }
            //判断退款金额不能超
            if (refund + orderInfo.refunded > orderInfo.payedAmount)
            {
                jm.msg = GlobalErrorCodeVars.Code13206;
                jm.code = 13206;
                return jm;
            }
            //根据是否已收到货和未收到货来判断实际可以退的数量，不能超过最大数量，已收到货的和未收到货的不能一起退，在这里做判断
            return verifyNums(type, orderInfo, items);
        }
        #endregion

        #region 判断退货数量是否超标
        /// <summary>
        /// 判断退货数量是否超标
        /// </summary>
        /// <param name="type"></param>
        /// <param name="orderInfo"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        private WebApiCallBack verifyNums(int type, CoreCmsOrder orderInfo, JArray items)
        {
            var jm = new WebApiCallBack();
            foreach (var item in items)
            {
                var id = ((JObject)item)["id"].ObjectToInt();
                var nums = ((JObject)item)["nums"].ObjectToInt();
                foreach (var orderItem in orderInfo.items)
                {
                    if (orderItem.id == id)
                    {
                        if (type == (int)GlobalEnumVars.BillAftersalesIsReceive.Refund)
                        {
                            var n = orderItem.nums - orderItem.sendNums - (orderItem.reshipNums - orderItem.reshipedNums);
                            if (n < nums)
                            {
                                jm.msg = orderItem.name + orderItem.addon + "，未发货商品，最多能退" + n + "个";
                                return jm;
                            }
                        }
                        else
                        {
                            var n = orderItem.sendNums - orderItem.reshipedNums;
                            if (n < nums)
                            {
                                jm.msg = orderItem.name + orderItem.addon + "已发货商品，最多能退" + n + "个";
                                return jm;
                            }
                        }
                    }
                }
            }
            jm.status = true;
            return jm;
        }
        #endregion

        #region 根据退货的明细，生成售后单明细表的数据
        /// <summary>
        /// 根据退货的明细，生成售后单明细表的数据
        /// </summary>
        /// <param name="orderInfo">订单的详细数据</param>
        /// <param name="items">前台选择的退货商品信息</param>
        /// <param name="aftersalesId">将要保存的售后单的单号</param>
        /// <returns></returns>
        private WebApiCallBack formatAftersalesItems(CoreCmsOrder orderInfo, JArray items, string aftersalesId)
        {
            var jm = new WebApiCallBack();
            var data = new List<CoreCmsBillAftersalesItem>();
            foreach (var item in items)
            {
                var id = ((JObject)item)["id"].ObjectToInt();
                var nums = ((JObject)item)["nums"].ObjectToInt();

                if (nums <= 0)
                {
                    continue;
                }

                foreach (var orderItem in orderInfo.items)
                {
                    if (orderItem.id == id)
                    {
                        //判断已经退过的加上本次退的，是否超过了购买的数量,具体取nums（购买数量）还是取sendnums(已发货数量),以后再说吧。要取购买数量，因为未发货的，也可以退的
                        if (nums + orderItem.reshipNums > orderItem.nums)
                        {
                            jm.msg = GlobalErrorCodeVars.Code13201;
                            jm.code = 13201;
                            return jm;
                        }
                        var billAftersalesItem = new CoreCmsBillAftersalesItem
                        {
                            aftersalesId = aftersalesId,
                            orderItemsId = orderItem.id,
                            goodsId = orderItem.goodsId,
                            productId = orderItem.productId,
                            sn = orderItem.sn,
                            bn = orderItem.bn,
                            name = orderItem.name,
                            imageUrl = orderItem.imageUrl,
                            nums = nums,
                            addon = orderItem.addon,
                            createTime = DateTime.Now
                        };
                        data.Add(billAftersalesItem);
                    }
                }
            }
            //判断生成的总记录条数，是否和前端传过来的记录条数对应上，如果没有对应上，就说明退货明细不正确
            if (data.Count != items.Count)
            {
                jm.msg = GlobalErrorCodeVars.Code13202;
                jm.data = jm.code = 13202;

                return jm;
            }
            jm.status = true;
            jm.data = data;
            return jm;
        }

        #endregion

        #region 带子集数据分页

        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsBillAftersales>> QueryPageAsync(Expression<Func<CoreCmsBillAftersales, bool>> predicate,
            Expression<Func<CoreCmsBillAftersales, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize);
        }

        #endregion

        #region 获取单个数据

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="aftersalesId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CoreCmsBillAftersales> GetInfo(string aftersalesId, int userId)
        {
            return await _dal.GetInfo(aftersalesId, userId);
        }
        #endregion

        #region Audit平台审核通过或者审核不通过

        /// <summary>
        /// 平台审核通过或者审核不通过
        /// 如果审核通过了，是退款单的话，自动生成退款单，并做订单完成状态，如果是退货的话，自动生成退款单和退货单，如果
        /// </summary>
        /// <param name="aftersalesId"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="refund"></param>
        /// <param name="mark"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Audit(string aftersalesId, int status, int type, decimal refund, string mark,
            JArray items)
        {
            var jm = new WebApiCallBack();

            var res = await PreAudit(aftersalesId);
            if (!res.status)
            {
                return jm;
            }

            var info = res.data as CoreCmsBillAftersales;
            var orderInfo = info.order;

            //校验订单是否可以进行此售后，并且校验订单价格是否合理
            var verifyData = Verify(info.type, orderInfo, refund, items);
            if (!verifyData.status && status == (int)GlobalEnumVars.BillAftersalesStatus.Success)
            {
                return verifyData;
            }


            //如果订单未发货，那么用户不能选择已收到货
            if (type == (int)GlobalEnumVars.BillAftersalesIsReceive.Reship &&
                orderInfo.shipStatus == (int)GlobalEnumVars.OrderShipStatus.No)
            {
                jm.msg = GlobalErrorCodeVars.Code13227;
                jm.code = 13227;
                return jm;
            }

            //如果是退货单，必须选择退货明细
            if (info.type == (int)GlobalEnumVars.BillAftersalesIsReceive.Reship && items.Count <= 0)
            {
                jm.msg = GlobalErrorCodeVars.Code13205;
                jm.code = 13205;
                return jm;
            }

            //如果是退货，判断退货明细，数量是否超出可退的数量
            var billAftersalesItems = new List<CoreCmsBillAftersalesItem>();
            if (items.Count > 0)
            {
                var aftersalesItems = formatAftersalesItems(orderInfo, items, aftersalesId);
                if (!aftersalesItems.status)
                {
                    return aftersalesItems;
                }

                billAftersalesItems = aftersalesItems.data as List<CoreCmsBillAftersalesItem>;
            }

            //判断退款金额不能超了
            if (refund + orderInfo.refunded > orderInfo.payedAmount)
            {
                jm.msg = GlobalErrorCodeVars.Code13206;
                jm.code = 13206;
                return jm;
            }

            using (var container = _serviceProvider.CreateScope())
            {
                var itemServices = container.ServiceProvider.GetService<ICoreCmsBillAftersalesItemServices>();
                var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();
                var refundServices = container.ServiceProvider.GetService<ICoreCmsBillRefundServices>();
                var reshipServices = container.ServiceProvider.GetService<ICoreCmsBillReshipServices>();
                var goodsServices = container.ServiceProvider.GetService<ICoreCmsGoodsServices>();
                var couponServices = container.ServiceProvider.GetService<ICoreCmsCouponServices>();
                //更新售后单
                await _dal.UpdateAsync(p => new CoreCmsBillAftersales() { status = status, mark = mark, refundAmount = refund, type = type },
                    p => p.aftersalesId == aftersalesId && p.status == (int)GlobalEnumVars.BillAftersalesStatus.WaitAudit);

                //更新售后单明细表，先删除，然后全新插入
                await itemServices.DeleteAsync(p => p.aftersalesId == aftersalesId);
                if (billAftersalesItems != null && billAftersalesItems.Any())
                {
                    await itemServices.InsertAsync(billAftersalesItems);
                }

                //审核通过的话，有退款的，生成退款单，根据最新的items生成退货单,并做订单的状态更改
                if (status == (int)GlobalEnumVars.BillAftersalesStatus.Success)
                {
                    //如果有退款，生成退款单
                    if (refund > 0)
                    {
                        var refundRes = await refundServices.ToAdd(info.userId, info.orderId,
                            (int)GlobalEnumVars.BillRefundType.Order, refund, info.aftersalesId);
                        if (!refundRes.status)
                        {
                            return refundRes;
                        }
                    }

                    //如果已经发货了，要退货，生成退货单，让用户吧商品邮回来。
                    if (info.type == (int)GlobalEnumVars.BillAftersalesIsReceive.Reship && billAftersalesItems != null && billAftersalesItems.Any())
                    {
                        var reshipRes = await reshipServices.ToAdd(info.userId, info.orderId, info.aftersalesId,
                            billAftersalesItems);
                        if (!reshipRes.status)
                        {
                            return reshipRes;
                        }
                    }
                    //更新订单状态

                    //如果是退款，退完了就变成已退款并且订单类型变成已完成，如果未退完，就是部分退款
                    if (refund > 0)
                    {
                        if (refund + orderInfo.refunded == orderInfo.payedAmount)
                        {
                            orderInfo.payStatus = (int)GlobalEnumVars.OrderPayStatus.Refunded;
                            orderInfo.status = (int)GlobalEnumVars.OrderStatus.Complete;

                            //返还积分
                            if (orderInfo.point > 0)
                            {
                                await _userPointLogServices.SetPoint(orderInfo.userId, orderInfo.point, (int)GlobalEnumVars.UserPointSourceTypes.PointRefundReturn, "售后退款：" + orderInfo.orderId + "返还积分");
                            }
                            //返还优惠券
                            if (!string.IsNullOrEmpty(orderInfo.coupon))
                            {
                                await couponServices.CancelReturnCoupon(orderInfo.coupon);
                            }

                        }
                        else
                        {
                            orderInfo.payStatus = (int)GlobalEnumVars.OrderPayStatus.PartialNo;
                        }
                    }

                    //判断货物发完没，如果货已发完了，订单发货就变成已发货,为了判断在有退款的情况下，当
                    var allDeliveryed = true;     //商品该发货状态，默认发货了,为了判断部分发货的情况下，的订单发货状态
                    var noDeliveryed = true;      //是否都没发货,默认都没发货
                    var allSened = true;          //商品退货状态（所有退货，包含已发的退货和未发的退货），默认都退货了,为了判断都退了的话，订单状态变成已完成

                    foreach (var item in orderInfo.items)
                    {
                        if (item.id > 0)
                        {
                            foreach (var jToken in items)
                            {
                                var tt = (JObject)jToken;
                                if (tt["id"].ToString() == item.id.ToString())
                                {
                                    item.reshipNums += tt["nums"].ObjectToInt(0);
                                    //判断 商品id相等，才能判断是否已发货，才能赋值给 reshipedNums 已发货的退货商品
                                    if (type == (int)GlobalEnumVars.BillAftersalesIsReceive.Reship)
                                    {
                                        item.reshipedNums += tt["nums"].ObjectToInt(0);
                                    }
                                }
                            }
                        }
                        //有任何商品发货，都不是未发货状态
                        if (noDeliveryed && item.sendNums > 0)
                        {
                            noDeliveryed = false;
                        }

                        if (allDeliveryed && (item.nums - item.sendNums - (item.reshipNums - item.reshipedNums) > 0))
                        {
                            //说明该发货的商品没发完
                            allDeliveryed = false;
                        }

                        if (allSened && (item.reshipNums < item.nums))
                        {
                            //说明未退完商品
                            allSened = false;
                        }
                    }

                    if (allDeliveryed && !noDeliveryed)
                    {
                        orderInfo.shipStatus = (int)GlobalEnumVars.OrderShipStatus.Yes;
                    }

                    if (allSened)
                    {
                        orderInfo.status = (int)GlobalEnumVars.OrderStatus.Complete;
                    }

                    //未发货的商品库存调整,如果订单未发货或者部分发货，并且用户未收到商品的情况下，需要解冻冻结库存
                    if ((orderInfo.shipStatus == (int)GlobalEnumVars.OrderShipStatus.No ||
                         orderInfo.shipStatus == (int)GlobalEnumVars.OrderShipStatus.PartialYes) &&
                        type == (int)GlobalEnumVars.BillAftersalesIsReceive.Refund &&
                        (billAftersalesItems != null && billAftersalesItems.Count > 0))
                    {
                        //未发货商品解冻库存
                        foreach (var item in billAftersalesItems)
                        {
                            goodsServices.ChangeStock(item.productId, GlobalEnumVars.OrderChangeStockType.refund.ToString(), item.nums);
                        }
                    }

                    //如果订单是已完成，但是订单的未发货商品还有的话，需要解冻库存
                    if (orderInfo.status == (int)GlobalEnumVars.OrderStatus.Complete)
                    {
                        foreach (var item in orderInfo.items)
                        {
                            var nums = item.nums - item.sendNums - (item.reshipNums - item.reshipedNums);//还未发货的数量
                            if (nums > 0)
                            {
                                goodsServices.ChangeStock(item.productId, GlobalEnumVars.OrderChangeStockType.refund.ToString(), nums);
                            }
                        }
                    }

                    //更新状态
                    await orderServices.UpdateAsync(
                        p => new CoreCmsOrder() { status = orderInfo.status, payStatus = orderInfo.payStatus },
                        p => p.orderId == orderInfo.orderId && p.status == (int)GlobalEnumVars.OrderStatus.Normal);
                }

                //售后单审核过后的事件处理
                if (status == (int)GlobalEnumVars.BillAftersalesStatus.Success)
                {
                    //售后审核通过后处理
                    await _redisOperationRepository.ListLeftPushAsync(RedisMessageQueueKey.AfterSalesReview, aftersalesId);
                }

            }

            orderInfo.addAftersalesStatus = true;
            orderInfo.billAftersalesId = aftersalesId;
            orderInfo.mark = mark;

            //发送售后审核消息
            await _messageCenterServices.SendMessage(info.userId, GlobalEnumVars.PlatformMessageTypes.AfterSalesPass.ToString(), JObject.FromObject(orderInfo));

            jm.status = true;

            return jm;
        }

        #endregion

        #region 后端进行审核的时候，前置操作，1取出页面的数据，2在提交过来的表单的时候，进行

        /// <summary>
        /// 后端进行审核的时候，前置操作，1取出页面的数据，2在提交过来的表单的时候，进行校验
        /// </summary>
        /// <param name="aftersalesId"></param>
        /// <returns></return>
        public async Task<WebApiCallBack> PreAudit(string aftersalesId)
        {
            var jm = new WebApiCallBack();

            using var container = _serviceProvider.CreateScope();

            var userServices = container.ServiceProvider.GetService<ICoreCmsUserServices>();
            var imagesServices = container.ServiceProvider.GetService<ICoreCmsBillAftersalesImagesServices>();
            var itemServices = container.ServiceProvider.GetService<ICoreCmsBillAftersalesItemServices>();
            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();

            var model = await _dal.QueryByIdAsync(aftersalesId);

            var userModel = await userServices.QueryByClauseAsync(p => p.id == model.userId);
            model.userNickName = userModel != null ? userModel.nickName : "";
            model.statusName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillAftersalesStatus>(model.status);
            model.images = await imagesServices.QueryListByClauseAsync(p => p.aftersalesId == model.aftersalesId);
            model.items = await itemServices.QueryListByClauseAsync(p => p.aftersalesId == model.aftersalesId);

            //获取订单信息
            var orderResult = await orderServices.GetOrderInfoByOrderId(model.orderId, model.userId, 2);

            if (orderResult.status)
            {
                model.order = orderResult.data as CoreCmsOrder;

                //订单上的退款金额和数量只包含已经售后的，这里要把当次售后单的商品信息保存到订单
                foreach (var orderItem in model.order.items)
                {
                    orderItem.promotionList = string.Empty;
                    orderItem.atPresentReshipNums = 0;
                    foreach (var it in model.items)
                    {
                        if (orderItem.id == it.orderItemsId)
                        {
                            orderItem.atPresentReshipNums = it.nums;
                        }
                    }
                }
            }
            jm.status = true;
            jm.data = model;


            return jm;
        }
        #endregion

    }
}
