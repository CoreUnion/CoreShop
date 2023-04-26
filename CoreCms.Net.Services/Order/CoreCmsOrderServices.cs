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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreCms.Net.Caching.AutoMate.RedisCache;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Validations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 订单表 接口实现
    /// </summary>
    public class CoreCmsOrderServices : BaseServices<CoreCmsOrder>, ICoreCmsOrderServices
    {
        private readonly ICoreCmsOrderRepository _dal;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICoreCmsShipServices _shipServices;
        private readonly ICoreCmsCartServices _cartServices;
        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly ICoreCmsCouponServices _couponServices;
        private readonly ICoreCmsUserPointLogServices _userPointLogServices;
        private readonly ICoreCmsPinTuanRecordServices _pinTuanRecordServices;
        private readonly ICoreCmsBillDeliveryServices _billDeliveryServices;
        private readonly ICoreCmsAreaServices _areaServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsLogisticsServices _logisticsServices;
        private readonly ICoreCmsInvoiceServices _invoiceServices;
        private readonly ICoreCmsBillAftersalesServices _billAftersalesServices;
        private readonly ICoreCmsOrderItemServices _orderItemServices;
        private readonly ICoreCmsInvoiceRecordServices _invoiceRecordServices;
        private readonly ICoreCmsOrderLogServices _orderLogServices;
        private readonly ICoreCmsUserShipServices _userShipServices;
        private readonly ICoreCmsStoreServices _storeServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsBillPaymentsServices _billPaymentsServices;
        private readonly ICoreCmsPaymentsServices _paymentsServices;
        private readonly ICoreCmsBillRefundServices _billRefundServices;
        private readonly ICoreCmsBillLadingServices _billLadingServices;
        private readonly ICoreCmsBillReshipServices _billReshipServices;
        private readonly ICoreCmsMessageCenterServices _messageCenterServices;
        private readonly ICoreCmsGoodsCommentServices _goodsCommentServices;
        private readonly ISysTaskLogServices _taskLogServices;
        private readonly ICoreCmsPromotionRecordServices _promotionRecordServices;
        private readonly IRedisOperationRepository _redisOperationRepository;

        public CoreCmsOrderServices(ICoreCmsOrderRepository dal
            , IHttpContextAccessor httpContextAccessor
            , ICoreCmsShipServices shipServices
            , ICoreCmsCartServices cartServices
            , ICoreCmsGoodsServices goodsServices
            , ICoreCmsCouponServices couponServices
            , ICoreCmsUserPointLogServices userPointLogServices
            , ICoreCmsPinTuanRecordServices pinTuanRecordServices
            , ICoreCmsBillDeliveryServices billDeliveryServices
            , ICoreCmsAreaServices areaServices
            , ICoreCmsSettingServices settingServices
            , ICoreCmsLogisticsServices logisticsServices
            , ICoreCmsInvoiceServices invoiceServices
            , ICoreCmsBillAftersalesServices billAftersalesServices
            , ICoreCmsOrderItemServices orderItemServices
            , ICoreCmsInvoiceRecordServices invoiceRecordServices
            , ICoreCmsOrderLogServices orderLogServices
            , ICoreCmsUserShipServices userShipServices
            , ICoreCmsStoreServices storeServices
            , ICoreCmsUserServices userServices
            , ICoreCmsBillPaymentsServices billPaymentsServices
            , ICoreCmsPaymentsServices paymentsServices
            , ICoreCmsBillRefundServices billRefundServices
            , ICoreCmsBillLadingServices billLadingServices
            , ICoreCmsBillReshipServices billReshipServices, ICoreCmsMessageCenterServices messageCenterServices, ICoreCmsGoodsCommentServices goodsCommentServices, ISysTaskLogServices taskLogServices, ICoreCmsPromotionRecordServices promotionRecordServices, IRedisOperationRepository redisOperationRepository)
        {
            this._dal = dal;
            base.BaseDal = dal;

            _httpContextAccessor = httpContextAccessor;
            _shipServices = shipServices;
            _cartServices = cartServices;
            _goodsServices = goodsServices;
            _couponServices = couponServices;
            _userPointLogServices = userPointLogServices;
            _pinTuanRecordServices = pinTuanRecordServices;
            _billDeliveryServices = billDeliveryServices;
            _areaServices = areaServices;
            _settingServices = settingServices;
            _logisticsServices = logisticsServices;
            _invoiceServices = invoiceServices;
            _billAftersalesServices = billAftersalesServices;
            _orderItemServices = orderItemServices;
            _invoiceRecordServices = invoiceRecordServices;
            _orderLogServices = orderLogServices;
            _userShipServices = userShipServices;
            _storeServices = storeServices;
            _userServices = userServices;
            _billPaymentsServices = billPaymentsServices;
            _paymentsServices = paymentsServices;
            _billRefundServices = billRefundServices;
            _billLadingServices = billLadingServices;
            _billReshipServices = billReshipServices;
            _messageCenterServices = messageCenterServices;
            _goodsCommentServices = goodsCommentServices;
            _taskLogServices = taskLogServices;
            _promotionRecordServices = promotionRecordServices;
            _redisOperationRepository = redisOperationRepository;
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
            return _dal.FindLimitOrder(productId, userId, startTime, endTime, orderType);
        }

        #endregion

        #region 获取税号
        /// <summary>
        /// 获取税号
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetTaxCode(string name)
        {
            var jm = new WebApiCallBack();

            var list = await _invoiceRecordServices.QueryPageAsync(p => p.name.Contains(name) && p.frequency >= 1, p => p.id, OrderByType.Desc, 1, 10);
            jm.data = list;
            jm.status = true;
            jm.msg = "获取成功";
            return jm;
        }

        #endregion

        #region 创建订单

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="userId">用户序列</param>
        /// <param name="orderType">订单类型，1是普通订单，2是拼团订单</param>
        /// <param name="cartIds">购物车货品序列</param>
        /// <param name="receiptType">收货方式,1快递物流，2同城配送，3门店自提</param>
        /// <param name="ushipId">用户地址库序列</param>
        /// <param name="storeId">门店序列</param>
        /// <param name="ladingName">提货人姓名</param>
        /// <param name="ladingMobile">提货人联系方式</param>
        /// <param name="memo">备注</param>
        /// <param name="point">积分</param>
        /// <param name="couponCode">优惠券码</param>
        /// <param name="source">来源平台</param>
        /// <param name="scene">场景值（一般小程序才有）</param>
        /// <param name="taxType">发票信息</param>
        /// <param name="taxName">发票抬头</param>
        /// <param name="taxCode">发票税务编码</param>
        /// <param name="objectId">关联非普通订单营销功能的序列</param>
        /// <param name="teamId">拼团订单分组序列</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ToAdd(int userId, int orderType, string cartIds, int receiptType, int ushipId, int storeId, string ladingName, string ladingMobile, string memo, int point, string couponCode, int source, int scene, int taxType, string taxName, string taxCode, int objectId, int teamId)
        {
            var jm = new WebApiCallBack() { methodDescription = "创建订单" };

            var order = new CoreCmsOrder
            {
                orderId = CommonHelper.GetSerialNumberType((int)GlobalEnumVars.SerialNumberType.订单编号),
                userId = userId,
                orderType = orderType,
                point = point,
                coupon = couponCode,
                receiptType = receiptType,
                objectId = objectId
            };

            //生成收货信息
            var areaId = 0;
            var deliveryRes = await FormatOrderDelivery(order, receiptType, ushipId, storeId, ladingName, ladingMobile);
            if (!deliveryRes.status)
            {
                return deliveryRes;
            }
            else
            {
                areaId = Convert.ToInt32(deliveryRes.data);
            }

            //通过购物车生成订单信息和订单明细信息
            List<CoreCmsOrderItem> orderItems;
            var ids = CommonHelper.StringToIntArray(cartIds);
            var orderRes = await FormatOrder(order, userId, ids, areaId, point, couponCode, false, receiptType, objectId);
            if (!orderRes.status)
            {
                return orderRes;
            }
            else
            {
                orderItems = orderRes.data as List<CoreCmsOrderItem>;
            }

            //以下值不是通过购物车得来的，是直接赋值的，就写这里吧，不写formatOrder里了。
            order.memo = memo;
            order.source = source;
            order.taxType = taxType;
            order.taxTitle = taxName;
            order.taxCode = taxCode;
            order.shipStatus = (int)GlobalEnumVars.OrderShipStatus.No;
            order.status = (int)GlobalEnumVars.OrderStatus.Normal;
            order.confirmStatus = (int)GlobalEnumVars.OrderConfirmStatus.ReceiptNotConfirmed;
            order.createTime = DateTime.Now;

            //开始事务处理
            await _dal.InsertAsync(order);
            
            //上面保存好订单表，下面保存订单的其他信息
            if (orderItems != null)
            {
                jm.msg = "更改库存";
                // foreach (var item in orderItems)
                // {
                //     var res = _goodsServices.ChangeStock(item.productId, GlobalEnumVars.OrderChangeStockType.order.ToString(), item.nums);
                //     if (res.status == false)
                //     {
                //         jm.msg = "更新库存数据失败";
                //         return jm;
                //     }
                // }
                
                //更改库存
                var avaliableOrderItems = orderItems.Where(item =>
                {
                    var res = _goodsServices.ChangeStock(item.productId, GlobalEnumVars.OrderChangeStockType.order.ToString(), item.nums);

                    if (res.status == false)
                    {
                        jm.msg += $"{item.name}库存不足";
                    }
                    
                    return res.status;

 
                }).ToList();
                
                
                if (avaliableOrderItems.Count == 0)
                {
                    await _orderItemServices.InsertCommandAsync(orderItems);
                    
                    await _dal.UpdateAsync(n => new CoreCmsOrder()
                        {
                            status = (int)GlobalEnumVars.OrderStatus.Cancel,
                            updateTime = DateTime.Now
                        },
                        m => m.orderId == order.orderId);
                   
                    //清除购物车信息
                    await _cartServices.DeleteAsync(p => ids.Contains(p.id) && p.userId == userId && p.type == orderType);

                    
                    jm.msg = "下单失败，库存不足";
                    return jm;
                }
                
                jm.msg = "订单明细更新" + avaliableOrderItems.Count;
                var outItems = await _orderItemServices.InsertCommandAsync(avaliableOrderItems);
                var outItemsBool = outItems > 0;
                if (outItemsBool == false)
                {

                    jm.msg = "订单明细更新失败";
                    jm.data = outItems;
                    return jm;
                }
                
                


                
                
                
                //优惠券核销
                if (!string.IsNullOrEmpty(couponCode))
                {
                    var arr = CommonHelper.StringToStringArray(couponCode);
                    var couponRes = await _couponServices.UsedMultipleCoupon(arr, order.orderId);
                    if (couponRes.status == false)
                    {
                        return couponRes;
                    }
                }
                //积分核销
                if (order.point > 0)
                {
                    jm.msg += "积分核销";
                    var pointLogRes = await _userPointLogServices.SetPoint(userId, 0 - order.point,
                        (int)GlobalEnumVars.UserPointSourceTypes.PointTypeDiscount, "订单" + order.orderId + "使用积分");
                    if (pointLogRes.status == false)
                    {
                        return pointLogRes;
                    }
                }
                //不同的订单类型会有不同的操作
                switch (orderType)
                {
                    case (int)GlobalEnumVars.OrderType.Common:
                        //标准模式不需要修改订单数据和商品数据
                        break;
                    case (int)GlobalEnumVars.OrderType.PinTuan:
                        //拼团模式去校验拼团是否存在，并添加拼团记录
                        var pinTuanRes = await _pinTuanRecordServices.OrderAdd(order, avaliableOrderItems, teamId);
                        if (pinTuanRes.status == false)
                        {
                            return pinTuanRes;
                        }
                        break;
                    case (int)GlobalEnumVars.OrderType.Group:
                        var groupRes = await _promotionRecordServices.OrderAdd(order, avaliableOrderItems, objectId, orderType);
                        if (groupRes.status == false)
                        {
                            return groupRes;
                        }
                        break;
                    case (int)GlobalEnumVars.OrderType.Skill:
                        var rskillRes = await _promotionRecordServices.OrderAdd(order, avaliableOrderItems, objectId, orderType);
                        if (rskillRes.status == false)
                        {
                            return rskillRes;
                        }
                        break;
                    case (int)GlobalEnumVars.OrderType.Bargain:
                        //砍价模式

                        break;

                    default:

                        jm.status = false;
                        jm.data = 10000;
                        jm.msg = GlobalErrorCodeVars.Code10000;
                        break;
                }
            }

            //清除购物车信息
            await _cartServices.DeleteAsync(p => ids.Contains(p.id) && p.userId == userId && p.type == orderType);

            //订单记录
            var orderLog = new CoreCmsOrderLog
            {
                userId = userId,
                orderId = order.orderId,
                type = (int)GlobalEnumVars.OrderLogTypes.LOG_TYPE_CREATE,
                msg = "订单创建",
                data = JsonConvert.SerializeObject(order),
                createTime = DateTime.Now
            };
            await _orderLogServices.InsertAsync(orderLog);

            //0元订单记录支付成功
            if (order.orderAmount <= 0)
            {
                orderLog = new CoreCmsOrderLog
                {
                    userId = userId,
                    orderId = order.orderId,
                    type = (int)GlobalEnumVars.OrderLogTypes.LOG_TYPE_PAY,
                    msg = "0元订单直接支付成功",
                    data = JsonConvert.SerializeObject(order),
                    createTime = DateTime.Now
                };
                await _orderLogServices.InsertAsync(orderLog);
            }

            //企业发票信息记录
            if (taxType == (int)GlobalEnumVars.OrderTaxType.Company)
            {
                var invoiceRecord = await _invoiceRecordServices.QueryByClauseAsync(p => p.code == taxCode && p.name == taxName);
                if (invoiceRecord != null)
                {
                    invoiceRecord.frequency += 1;
                    await _invoiceRecordServices.UpdateAsync(invoiceRecord);
                }
                else
                {
                    invoiceRecord = new CoreCmsInvoiceRecord { code = taxCode, name = taxName, frequency = 1 };
                    await _invoiceRecordServices.InsertAsync(invoiceRecord);
                }
            }
            order.taxTitle = taxName;
            order.taxCode = taxCode;

            //发送消息
            //推送消息
            await _messageCenterServices.SendMessage(order.userId, GlobalEnumVars.PlatformMessageTypes.CreateOrder.ToString(), JObject.FromObject(order));


            jm.status = true;
            jm.data = order;

            return jm;
        }

        #endregion

        #region 生成订单的收货信息
        /// <summary>
        /// 生成订单的收货信息
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="receiptType">收货方式,1快递物流，2同城配送，3门店自提</param>
        /// <param name="ushipId">用户地址库序列</param>
        /// <param name="storeId">门店序列</param>
        /// <param name="ladingName">提货人姓名</param>
        /// <param name="ladingMobile">提货人联系方式</param>
        /// <returns></returns>
        private async Task<WebApiCallBack> FormatOrderDelivery(CoreCmsOrder order, int receiptType, int ushipId, int storeId, string ladingName, string ladingMobile)
        {
            var res = new WebApiCallBack() { methodDescription = "生成订单的收货信息" };

            var areaId = 0;
            if (receiptType == (int)GlobalEnumVars.OrderReceiptType.Logistics || receiptType == (int)GlobalEnumVars.OrderReceiptType.IntraCityService)
            {
                //快递邮寄
                var userShipInfo = await _userShipServices.QueryByClauseAsync(p => p.userId == order.userId && p.id == ushipId);
                if (userShipInfo == null)
                {
                    res.data = 11050;
                    res.msg = GlobalErrorCodeVars.Code11050;
                    return res;
                }
                areaId = userShipInfo.areaId;

                //快递邮寄
                order.shipAreaId = userShipInfo.areaId;
                order.shipAddress = userShipInfo.address;
                order.shipName = userShipInfo.name;
                order.shipMobile = userShipInfo.mobile;

                var ship = _shipServices.GetShip(userShipInfo.areaId);
                if (ship != null)
                {
                    order.logisticsId = ship.id;
                    order.logisticsName = ship.name;
                    order.storeId = 0;
                }
            }
            else
            {
                //门店自提
                var storeInfo = await _storeServices.QueryByIdAsync(storeId);
                if (storeInfo == null)
                {
                    res.data = 11055;
                    res.msg = GlobalErrorCodeVars.Code11055;
                    return res;
                }
                areaId = storeInfo.areaId;

                //门店自提
                order.shipAreaId = storeInfo.areaId;
                order.shipAddress = storeInfo.address;
                order.shipName = ladingName;
                order.shipMobile = ladingMobile;
                order.storeId = storeId;
                order.logisticsId = 0;

            }
            res.status = true;
            res.msg = "订单的收货信息生成成功";
            res.data = areaId;

            return res;
        }
        #endregion

        #region 生成订单的时候，根据购物车信息生成订单信息及明细信息

        /// <summary>
        /// 生成订单的时候，根据购物车信息生成订单信息及明细信息
        /// </summary>
        /// <param name="order">订单数组</param>
        /// <param name="userId">用户id</param>
        /// <param name="cartIds">购物车信息</param>
        /// <param name="areaId">收货地区</param>
        /// <param name="point">使用积分</param>
        /// <param name="couponCode">使用优惠券</param>
        /// <param name="freeFreight">是否包邮</param>
        /// <param name="deliveryType">收货方式,1快递物流，2同城配送，3门店自提</param>
        /// <param name="groupId">团队明细</param>
        /// <returns>返回订单明细信息</returns>
        private async Task<WebApiCallBack> FormatOrder(CoreCmsOrder order, int userId, int[] cartIds, int areaId, int point,
            string couponCode, bool freeFreight = false, int deliveryType = (int)GlobalEnumVars.OrderReceiptType.Logistics, int groupId = 0)
        {
            var res = new WebApiCallBack() { methodDescription = "生成订单信息及明细信息" };

            var cartModel = await _cartServices.GetCartInfos(userId, cartIds, order.orderType, areaId, point, couponCode,
           freeFreight, deliveryType, groupId);
            if (!cartModel.status)
            {
                return cartModel;
            }

            if (cartModel.data is CartDto cartDto)
            {
                order.goodsAmount = cartDto.goodsAmount;
                order.orderAmount = cartDto.amount;
                if (order.orderAmount == 0)
                {
                    order.payStatus = (int)GlobalEnumVars.OrderPayStatus.Yes;
                    order.paymentTime = DateTime.Now;
                }
                else
                {
                    order.payStatus = (int)GlobalEnumVars.OrderPayStatus.No;
                }
                order.costFreight = cartDto.costFreight;
                //优惠信息存储
                var promotionList = new Dictionary<int, WxNameTypeDto>();
                foreach (var item in cartDto.promotionList)
                {
                    if (item.Value.type == 2)
                    {
                        promotionList.Add(item.Key, item.Value);
                    }
                }
                order.promotionList = promotionList.Any() ? JsonConvert.SerializeObject(promotionList) : "";
                //积分使用情况
                order.point = cartDto.point;
                order.pointMoney = cartDto.pointExchangeMoney;
                order.weight = cartDto.weight;
                order.orderDiscountAmount = cartDto.orderPromotionMoney > 0 ? cartDto.orderPromotionMoney : 0;
                order.goodsDiscountAmount = cartDto.goodsPromotionMoney > 0 ? cartDto.goodsPromotionMoney : 0;
                order.couponDiscountAmount = cartDto.couponPromotionMoney;
                order.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";
                //以上保存了订单主体表信息，以下生成订单明细表
                var items = FormatOrderItems(cartDto.list, order.orderId);
                if (!items.Any())
                {
                    res.status = false;
                    res.data = 10000;
                    res.msg = GlobalErrorCodeVars.Code10000;
                    return res;
                }
                res.status = true;
                res.data = items;
            }

            return res;
        }

        #endregion

        #region 根据购物车的明细生成订单明细

        /// <summary>
        /// 根据购物车的明细生成订单明细
        /// </summary>
        private static List<CoreCmsOrderItem> FormatOrderItems(List<CartProducts> list, string orderId)
        {
            var res = new List<CoreCmsOrderItem>();
            foreach (var item in list)
            {
                if (item.isSelect == false) continue;
                var model = new CoreCmsOrderItem
                {
                    orderId = orderId,
                    goodsId = (int)item.products.goodsId,
                    productId = item.products.id,
                    sn = item.products.sn,
                    bn = item.products.bn,
                    name = item.products.name,
                    price = (decimal)item.products.price,
                    costprice = (decimal)item.products.costprice,
                    mktprice = (decimal)item.products.mktprice,
                    imageUrl = item.products.images,
                    nums = item.nums,
                    amount = item.products.amount,
                    promotionAmount = item.products.promotionAmount > 0 ? item.products.promotionAmount : 0,
                    weight = Math.Round(item.weight * item.nums, 2),
                    sendNums = 0,
                    addon = item.products.spesDesc,
                    createTime = DateTime.Now
                };
                if (item.products.promotionList.Count > 0)
                {
                    var promotionList = new Dictionary<int, WxNameTypeDto>();
                    foreach (var proDto in item.products.promotionList)
                    {
                        if (proDto.Value.type == 2)
                        {
                            promotionList.Add(proDto.Key, proDto.Value);
                        }
                    }
                    model.promotionList = JsonConvert.SerializeObject(promotionList);
                }
                res.Add(model);
            }
            return res;
        }
        #endregion

        #region 获取单个订单所有详情
        /// <summary>
        /// 根据订单编号获取单个订单所有详情
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetOrderInfoByOrderId(string id, int userId = 0, int aftersaleLevel = 0)
        {
            var jm = new WebApiCallBack();

            var order = new CoreCmsOrder();
            order = userId > 0
                ? await _dal.QueryByClauseAsync(p => p.orderId == id && p.userId == userId)
                : await _dal.QueryByClauseAsync(p => p.orderId == id);
            if (order == null)
            {
                jm.msg = "获取订单失败";
                return jm;
            }
            //订单详情(子货品数据)
            order.items = await _orderItemServices.QueryListByClauseAsync(p => p.orderId == order.orderId);

            if (order.items.Any())
            {
                order.items.ForEach(p =>
                {
                    if (!string.IsNullOrEmpty(p.promotionList))
                    {
                        var jobj = JObject.Parse(p.promotionList);
                        if (jobj.Values().Any())
                        {
                            p.promotionObj = jobj.Values().FirstOrDefault();
                        }
                    }
                });
            }

            //获取相关状态描述说明转换
            order.statusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderStatus>(order.status);
            order.payStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderPayStatus>(order.payStatus);
            order.shipStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderShipStatus>(order.shipStatus);
            order.sourceText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.Source>(order.source);
            order.typeText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderType>(order.orderType);
            order.confirmStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderConfirmStatus>(order.confirmStatus);
            order.taxTypeText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderTaxType>(order.taxType);
            order.paymentCodeText = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.PaymentsTypes>(order.paymentCode);
            //获取日志
            order.orderLog = await _orderLogServices.QueryListByClauseAsync(p => p.orderId == order.orderId);

            if (order.orderLog.Any())
            {
                order.orderLog.ForEach(p =>
                {
                    p.typeText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderLogTypes>(p.type);
                });
            }

            //用户信息
            order.user = await _userServices.QueryByIdAsync(order.userId);
            if (order.user != null)
            {
                order.user.passWord = "";
            }
            //支付单
            order.paymentItem = await _billPaymentsServices.QueryListByClauseAsync(p => p.sourceId == order.orderId);
            //退款单
            order.refundItem = await _billRefundServices.QueryListByClauseAsync(p => p.sourceId == order.orderId);
            //提货单
            order.ladingItem = await _billLadingServices.QueryListByClauseAsync(p => p.orderId == order.orderId);
            //退货单
            order.returnItem = await _billReshipServices.QueryListByClauseAsync(p => p.orderId == order.orderId);
            //售后单
            order.aftersalesItem = await _billAftersalesServices.QueryListByClauseAsync(p => p.orderId == order.orderId);
            //发货单
            order.delivery = await _billDeliveryServices.QueryListByClauseAsync(p => p.orderId == order.orderId);
            if (order.delivery != null && order.delivery.Any())
            {
                foreach (var item in order.delivery)
                {
                    var outFirstAsync = await _logisticsServices.QueryByClauseAsync(p => p.logiCode == item.logiCode);
                    item.logiName = outFirstAsync != null ? outFirstAsync.logiName : item.logiCode;
                }
            }
            //获取提货门店
            if (order.storeId != 0)
            {
                order.store = await _storeServices.QueryByIdAsync(order.storeId);
                if (order.store != null)
                {
                    var areaBack = await _areaServices.GetAreaFullName(order.store.areaId);
                    order.store.allAddress = areaBack.status ? areaBack.data + order.store.address : order.store.address;
                }
            }
            //获取配送方式
            if (order.logisticsId > 0)
            {
                order.logistics = await _shipServices.QueryByIdAsync(order.logisticsId);
            }
            //获取订单状态及中文描述
            order.globalStatus = GetGlobalStatus(order);

            order.globalStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderAllStatusType>(order.globalStatus);
            //收货地区三级地址
            var shipAreaBack = await _areaServices.GetAreaFullName(order.shipAreaId);

            order.shipAreaName = shipAreaBack.status ? shipAreaBack.data.ToString() : "";

            //获取支付方式
            var pm = await _paymentsServices.QueryByClauseAsync(p => p.code == order.paymentCode);
            order.paymentName = pm != null ? pm.name : "未知支付方式";
            //优惠券
            //if (!string.IsNullOrEmpty(order.coupon))
            //{
            //    order.couponObj = await _couponServices.QueryWithAboutAsync(p => p.usedId == order.orderId);
            //}
            order.couponObj = await _couponServices.QueryWithAboutAsync(p => p.usedId == order.orderId);

            var allConfigs = await _settingServices.GetConfigDictionaries();
            //获取该状态截止时间
            switch (order.globalStatus)
            {
                case (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_PAYMENT: ////待付款
                    var cancelTime = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OrderCancelTime).ObjectToInt(1) * 86400;
                    var dt = order.createTime.AddSeconds(cancelTime);
                    order.remainingTime = dt;
                    order.remaining = CommonHelper.GetRemainingTime(dt);
                    break;
                case (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_RECEIPT: //待收货
                    var autoSignTime = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OrderAutoSignTime).ObjectToInt(1) * 86400;
                    var dtautoSignTime = order.createTime.AddSeconds(autoSignTime);
                    order.remainingTime = dtautoSignTime;
                    order.remaining = CommonHelper.GetRemainingTime(dtautoSignTime);
                    break;
                case (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_EVALUATE:  //待评价
                    var autoEvalTime = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OrderAutoEvalTime).ObjectToInt(1) * 86400;
                    var dtautoEvalTime = order.createTime.AddSeconds(autoEvalTime);
                    order.remainingTime = dtautoEvalTime;
                    order.remaining = CommonHelper.GetRemainingTime(dtautoEvalTime);
                    break;

                default:
                    order.remaining = string.Empty;
                    order.remainingTime = null;
                    break;

            }
            //支付单
            if (order.paymentItem != null && order.paymentItem.Any())
            {
                foreach (var item in order.paymentItem)
                {
                    item.paymentCodeName = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.PaymentsTypes>(item.paymentCode);
                    item.statusName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillPaymentsStatus>(item.status);
                }
            }
            //退款单
            if (order.refundItem != null && order.refundItem.Any())
            {
                foreach (var item in order.refundItem)
                {
                    item.paymentCodeName = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.PaymentsTypes>(item.paymentCode);
                    item.statusName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillRefundStatus>(item.status);
                }
            }
            //发货单
            if (order.delivery != null && order.delivery.Any())
            {
                foreach (var item in order.delivery)
                {
                    var logisticsModel = await _logisticsServices.GetLogiInfo(item.logiCode);
                    if (logisticsModel.status)
                    {
                        var logisticsData = logisticsModel.data as CoreCmsLogistics;
                        item.logiName = logisticsData.logiName;
                    }
                    var areaModel = await _areaServices.GetAreaFullName(item.shipAreaId);
                    if (areaModel.status)
                    {
                        item.shipAreaIdName = areaModel.data as string;
                    }
                }
            }
            //提货单
            if (order.ladingItem != null && order.ladingItem.Any())
            {
                foreach (var item in order.ladingItem)
                {
                    var storeModel = await _storeServices.QueryByIdAsync(item.storeId);
                    item.storeName = storeModel != null ? storeModel.storeName : "";
                    item.statusName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillLadingStatus>(item.status ? 2 : 1);

                    if (item.clerkId != 0)
                    {
                        var userModel = await _userServices.QueryByIdAsync(item.clerkId);
                        if (userModel != null)
                        {
                            item.clerkIdName = !string.IsNullOrEmpty(userModel.nickName) ? userModel.nickName : userModel.mobile;
                        }
                    }
                }
            }
            //退货单
            if (order.returnItem != null && order.returnItem.Any())
            {
                foreach (var item in order.returnItem)
                {
                    var logisticsModel = await _logisticsServices.GetLogiInfo(item.logiCode);
                    if (logisticsModel.status)
                    {
                        var logisticsData = logisticsModel.data as CoreCmsLogistics;
                        item.logiName = logisticsData.logiName;
                    }
                    item.statusName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillReshipStatus>(item.status);
                }
            }
            //售后单取当前活动的收货单
            if (order.aftersalesItem != null && order.aftersalesItem.Any())
            {
                foreach (var item in order.aftersalesItem)
                {
                    order.billAftersalesId = item.aftersalesId;
                    //如果售后单里面有待审核的活动售后单，那就直接拿这条
                    if (item.status == (int)GlobalEnumVars.BillAftersalesStatus.WaitAudit) break;
                }
            }
            //把退款金额和退货商品查出来
            AfterSalesVal(order, aftersaleLevel);
            //促销信息
            if (!string.IsNullOrEmpty(order.promotionList))
            {
                order.promotionObj = JsonConvert.DeserializeObject(order.promotionList);
            }

            //发票信息
            var invoiceModel = await _invoiceServices.GetOrderInvoiceInfo(order.orderId);
            if (invoiceModel != null && invoiceModel.status)
            {
                order.invoice = invoiceModel.data;
            }
            else
            {
                order.invoice = new
                {
                    type = order.taxType,
                    title = order.taxTitle,
                    taxNumber = order.taxCode
                };
            }

            jm.status = true;
            jm.data = order;
            jm.msg = GlobalConstVars.GetDataSuccess;

            return jm;
        }

        #endregion

        #region 把退款的金额和退货的商品数量保存起来
        /// <summary>
        /// 把退款的金额和退货的商品数量保存起来
        /// </summary>
        /// <param name="order"></param>
        /// <param name="aftersaleLevel">取售后单的时候，售后单的等级，0：待审核的和审核通过的售后单，1未审核的，2审核通过的</param>
        public void AfterSalesVal(CoreCmsOrder order, int aftersaleLevel)
        {
            var addAftersalesStatus = false;
            var res = _billAftersalesServices.OrderToAftersales(order.orderId, aftersaleLevel);
            var resData = res.data as OrderToAftersalesDto;
            //已经退过款的金额
            order.refunded = resData.refundMoney;
            //算退货商品数量
            foreach (var item in order.items)
            {
                if (resData.reshipGoods.ContainsKey(item.id))
                {
                    item.reshipNums = resData.reshipGoods[item.id].reshipNums;
                    item.reshipedNums = resData.reshipGoods[item.id].reshipedNums;

                    //商品总数量 - 已发货数量 - 未发货的退货数量（总退货数量减掉已发货的退货数量）
                    if (!addAftersalesStatus && (item.nums - item.reshipNums) > 0)//如果没退完，就可以再次发起售后
                    {
                        addAftersalesStatus = true;
                    }
                }
                else
                {
                    item.reshipNums = 0;  //退货商品
                    item.reshipedNums = 0;//已发货的退货商品
                    if (!addAftersalesStatus) //没退货，就能发起售后
                    {
                        addAftersalesStatus = true;
                    }
                }
            }
            //商品没退完或没退，可以发起售后，但是订单状态不对的话，也不能发起售后
            if (order.payStatus == (int)GlobalEnumVars.OrderPayStatus.No || order.status != (int)GlobalEnumVars.OrderStatus.Normal)
            {
                addAftersalesStatus = false;
            }
            order.addAftersalesStatus = addAftersalesStatus;
        }

        #endregion

        #region 获取订单不同状态的数量
        /// <summary>
        /// 获取订单不同状态的数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ids"></param>
        /// <param name="isAfterSale"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetOrderStatusNum(int userId, int[] ids, bool isAfterSale = false)
        {
            var jm = new WebApiCallBack();

            var data = new Dictionary<string, int>();
            foreach (var id in ids)
            {
                var count = await OrderCount(id, userId);
                data.Add(id.ToString(), count);
            }
            if (isAfterSale)
            {
                var number = await _billAftersalesServices.GetUserAfterSalesNum(userId,
                    (int)GlobalEnumVars.BillAftersalesStatus.WaitAudit);
                data.Add("isAfterSale", number);
            }
            else
            {
                data.Add("isAfterSale", 0);
            }
            jm.status = true;
            jm.data = data;

            return jm;
        }


        /// <summary>
        /// 订单数量统计
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> OrderCount(int type = 0, int userId = 0)
        {
            var count = 0;
            var where = GetReverseStatus(type);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }

            count = await _dal.GetCountAsync(where);
            return count;

        }


        #endregion

        #region 获取订单全局状态
        /// <summary>
        /// 获取订单全局状态
        /// </summary>
        /// <param name="orderInfo">订单数据</param>
        /// <returns></returns>
        public static int GetGlobalStatus(CoreCmsOrder orderInfo)
        {
            var status = 0;
            if (orderInfo.status == (int)GlobalEnumVars.OrderStatus.Complete)
            {
                status = (int)GlobalEnumVars.OrderAllStatusType.ALL_COMPLETED; //已完成
            }
            else if (orderInfo.status == (int)GlobalEnumVars.OrderStatus.Cancel)
            {
                status = (int)GlobalEnumVars.OrderAllStatusType.ALL_CANCEL; //已取消
            }
            else if (orderInfo.status == (int)GlobalEnumVars.OrderStatus.Normal)
            {
                if (orderInfo.payStatus == (int)GlobalEnumVars.OrderPayStatus.No)
                {
                    status = (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_PAYMENT;//待付款
                }
                else
                {
                    if (orderInfo.shipStatus == (int)GlobalEnumVars.OrderShipStatus.No || orderInfo.shipStatus == (int)GlobalEnumVars.OrderShipStatus.PartialYes)
                    {
                        status = (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_DELIVERY;//待发货

                    }
                    else if ((orderInfo.shipStatus == (int)GlobalEnumVars.OrderShipStatus.Yes || orderInfo.shipStatus == (int)GlobalEnumVars.OrderShipStatus.PartialYes) && orderInfo.confirmStatus == (int)GlobalEnumVars.OrderConfirmStatus.ReceiptNotConfirmed)
                    {
                        status = (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_RECEIPT;//待收货

                    }
                    else if (orderInfo.shipStatus != (int)GlobalEnumVars.OrderShipStatus.No && orderInfo.confirmStatus == (int)GlobalEnumVars.OrderConfirmStatus.ConfirmReceipt && orderInfo.isComment == false)
                    {
                        status = (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_EVALUATE;//待评价
                    }
                    else if (orderInfo.shipStatus != (int)GlobalEnumVars.OrderShipStatus.No && orderInfo.confirmStatus == (int)GlobalEnumVars.OrderConfirmStatus.ConfirmReceipt && orderInfo.isComment == true)
                    {
                        status = (int)GlobalEnumVars.OrderAllStatusType.ALL_COMPLETED_EVALUATE;//已评价

                    }
                }
            }
            return status;
        }
        #endregion

        #region 获取订单状态反查
        /// <summary>
        /// 获取订单状态反查
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public Expression<Func<CoreCmsOrder, bool>> GetReverseStatus(int status)
        {
            var where = PredicateBuilder.True<CoreCmsOrder>();
            switch (status)
            {
                case (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_PAYMENT: //待付款
                    where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
                    where = where.And(p => p.payStatus == (int)GlobalEnumVars.OrderPayStatus.No);
                    where = where.And(p => p.isdel == false);
                    break;
                case (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_DELIVERY: //待发货
                    where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
                    where = where.And(p => p.payStatus != (int)GlobalEnumVars.OrderPayStatus.No);
                    where = where.And(p => p.shipStatus == (int)GlobalEnumVars.OrderShipStatus.No || p.shipStatus == (int)GlobalEnumVars.OrderShipStatus.PartialYes);
                    where = where.And(p => p.isdel == false);
                    break;
                case (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_RECEIPT: //待收货
                    where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
                    where = where.And(p => p.payStatus != (int)GlobalEnumVars.OrderPayStatus.No);
                    where = where.And(p => p.shipStatus == (int)GlobalEnumVars.OrderShipStatus.Yes || p.shipStatus == (int)GlobalEnumVars.OrderShipStatus.PartialYes);
                    where = where.And(p => p.confirmStatus == (int)GlobalEnumVars.OrderConfirmStatus.ReceiptNotConfirmed);
                    where = where.And(p => p.isdel == false);
                    break;
                case (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_EVALUATE: //待评价
                    where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
                    where = where.And(p => p.payStatus != (int)GlobalEnumVars.OrderPayStatus.No);
                    where = where.And(p => p.shipStatus != (int)GlobalEnumVars.OrderShipStatus.No);
                    where = where.And(p => p.confirmStatus == (int)GlobalEnumVars.OrderConfirmStatus.ConfirmReceipt);
                    where = where.And(p => p.isComment == false);
                    where = where.And(p => p.isdel == false);
                    break;
                case (int)GlobalEnumVars.OrderAllStatusType.ALL_COMPLETED_EVALUATE: //已评价
                    where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
                    where = where.And(p => p.payStatus != (int)GlobalEnumVars.OrderPayStatus.No);
                    where = where.And(p => p.shipStatus != (int)GlobalEnumVars.OrderShipStatus.No);
                    where = where.And(p => p.confirmStatus == (int)GlobalEnumVars.OrderConfirmStatus.ConfirmReceipt);
                    where = where.And(p => p.isComment == true);
                    where = where.And(p => p.isdel == false);
                    break;
                case (int)GlobalEnumVars.OrderAllStatusType.ALL_CANCEL: //已取消
                    where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Cancel);
                    where = where.And(p => p.isdel == false);
                    break;
                case (int)GlobalEnumVars.OrderAllStatusType.ALL_COMPLETED: //已完成
                    where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Complete);
                    where = where.And(p => p.isdel == false);
                    break;
                default:
                    where = where.And(p => p.isdel == false);
                    break;
            }
            return where;
        }

        #endregion

        #region 获取订单列表微信小程序
        /// <summary>
        /// 获取订单列表微信小程序
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetOrderList(int status = -1, int userId = 0, int page = 1, int limit = 5)
        {
            var jm = new WebApiCallBack { status = true };

            var where = PredicateBuilder.True<CoreCmsOrder>();

            if (status > -1)
            {
                where = GetReverseStatus(status);
            }
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            var list = await _dal.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, page, limit);

            if (list.Any())
            {
                foreach (var order in list)
                {
                    //获取相关状态描述说明转换
                    order.statusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderStatus>(order.status);
                    order.payStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderPayStatus>(order.payStatus);
                    order.shipStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderShipStatus>(order.shipStatus);
                    order.sourceText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.Source>(order.source);
                    order.typeText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderType>(order.orderType);
                    order.confirmStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderConfirmStatus>(order.confirmStatus);
                    order.taxTypeText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderTaxType>(order.taxType);
                    order.paymentCodeText = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.PaymentsTypes>(order.paymentCode);
                }
            }
            jm.data = new
            {
                list,
                count = list.TotalCount,
                page,
                limit,
                status
            };

            return jm;
        }


        #endregion

        #region 商家获取订单列表-微信小程序
        /// <summary>
        /// 商家获取订单列表-微信小程序
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetOrderPageByMerchant(string dateType, string[] date, int status = 0, int storeId = 0, int page = 1, int limit = 5)
        {
            var jm = new WebApiCallBack { status = true };

            var where = PredicateBuilder.True<CoreCmsOrder>();
            @where = status > 0 ? GetReverseStatus(status) : @where.And(p => p.isdel == false);


            if (storeId > 0)
            {
                where = where.And(p => p.storeId == storeId);
            }


            DateTime dt = DateTime.Now;
            if (dateType == "today")
            {
                var startTime = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                var entTime = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                where = where.And(p => p.createTime > startTime && p.createTime < entTime);
            }
            else if (dateType == "yesterday")
            {
                var yesterday = dt.AddDays(-1);
                var startTime = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 0, 0, 0);
                var entTime = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59);
                where = where.And(p => p.createTime > startTime && p.createTime < entTime);
            }
            else if (dateType == "week")
            {
                int dayOfWeek = -1 * (int)dt.Date.DayOfWeek;
                DateTime weekStartTime = dt.AddDays(dayOfWeek + 1);//取本周一
                if (dayOfWeek == 0) weekStartTime = weekStartTime.AddDays(-7);//如果今天是周日，则开始时间是上周一
                var weekEndTime = weekStartTime.AddDays(7);

                var startTime = new DateTime(weekStartTime.Year, weekStartTime.Month, weekStartTime.Day, 0, 0, 0);
                var entTime = new DateTime(weekEndTime.Year, weekEndTime.Month, weekEndTime.Day, 23, 59, 59);

                where = where.And(p => p.createTime > startTime && p.createTime < entTime);
            }
            else if (dateType == "month")
            {
                //本月第一天时间      
                DateTime dtFirst = dt.AddDays(1 - (dt.Day));
                dtFirst = new DateTime(dtFirst.Year, dtFirst.Month, dtFirst.Day, 0, 0, 0);
                //获得某年某月的天数    
                int dayCount = DateTime.DaysInMonth(dt.Date.Year, dt.Date.Month);
                //本月最后一天时间    
                DateTime dtLast = dtFirst.AddDays(dayCount - 1);

                var startTime = new DateTime(dtFirst.Year, dtFirst.Month, dtFirst.Day, 0, 0, 0);
                var entTime = new DateTime(dtLast.Year, dtLast.Month, dtLast.Day, 23, 59, 59);


                where = where.And(p => p.createTime > startTime && p.createTime < entTime);
            }
            else if (dateType == "custom" && date is { Length: 2 })
            {
                var st = date[0].ObjectToDate();
                var et = date[1].ObjectToDate();

                var startTime = new DateTime(st.Year, st.Month, st.Day, 0, 0, 0);
                var entTime = new DateTime(et.Year, et.Month, et.Day, 23, 59, 59);

                where = where.And(p => p.createTime > startTime && p.createTime < entTime);
            }

            var pages = await _dal.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, page, limit);

            if (pages.Any())
            {
                foreach (var order in pages)
                {
                    //获取相关状态描述说明转换
                    order.statusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderStatus>(order.status);
                    order.payStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderPayStatus>(order.payStatus);
                    order.shipStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderShipStatus>(order.shipStatus);
                    order.sourceText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.Source>(order.source);
                    order.typeText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderType>(order.orderType);
                    order.confirmStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderConfirmStatus>(order.confirmStatus);
                    order.taxTypeText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderTaxType>(order.taxType);
                    order.paymentCodeText = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.PaymentsTypes>(order.paymentCode);
                }
            }


            var totalMoney = await _dal.GetSumAsync(where, p => p.payedAmount, true);

            jm.data = new
            {
                pages,
                pages.TotalCount,
                pages.PageSize,
                pages.HasNextPage,
                pages.HasPreviousPage,
                pages.PageIndex,
                pages.TotalPages,
                totalMoney
            };

            return jm;
        }

        #endregion


        #region 商家获取订单列表通过检索手机号码和订单号-微信小程序
        /// <summary>
        /// 商家获取订单列表通过检索手机号码和订单号-微信小程序
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetOrderPageByMerchantSearch(string keyword, int status = 0, int receiptType = 0, int storeId = 0, int page = 1, int limit = 5)
        {
            var jm = new WebApiCallBack { status = true };

            var where = PredicateBuilder.True<CoreCmsOrder>();
            @where = status > 0 ? GetReverseStatus(status) : @where.And(p => p.isdel == false);

            if (storeId > 0)
            {
                where = where.And(p => p.storeId == storeId);
            }
            if (receiptType > 0)
            {
                where = where.And(p => p.receiptType == receiptType);

            }

            if (!string.IsNullOrEmpty(keyword))
            {
                where = where.And(p =>
                    p.shipMobile.Contains(keyword) || p.shipName.Contains(keyword) || p.orderId.Contains(keyword));
            }

            var pages = await _dal.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, page, limit);

            if (pages.Any())
            {
                foreach (var order in pages)
                {
                    //获取相关状态描述说明转换
                    order.statusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderStatus>(order.status);
                    order.payStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderPayStatus>(order.payStatus);
                    order.shipStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderShipStatus>(order.shipStatus);
                    order.sourceText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.Source>(order.source);
                    order.typeText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderType>(order.orderType);
                    order.confirmStatusText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderConfirmStatus>(order.confirmStatus);
                    order.taxTypeText = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.OrderTaxType>(order.taxType);
                    order.paymentCodeText = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.PaymentsTypes>(order.paymentCode);
                }
            }

            var totalMoney = await _dal.GetSumAsync(where, p => p.payedAmount, true);

            jm.data = new
            {
                pages,
                pages.TotalCount,
                pages.PageSize,
                pages.HasNextPage,
                pages.HasPreviousPage,
                pages.PageIndex,
                pages.TotalPages,
                totalMoney
            };
            return jm;
        }

        #endregion

        #region 订单支付

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="paymentCode">支付方式</param>
        /// <param name="billPaymentInfo">支付单据</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Pay(string orderId, string paymentCode, CoreCmsBillPayments billPaymentInfo)
        {
            var jm = new WebApiCallBack() { msg = "订单支付失败" };

            //获取订单
            var order = await _dal.QueryByClauseAsync(p => p.orderId == orderId && p.status == (int)GlobalEnumVars.OrderStatus.Normal);
            if (order == null)
            {
                return jm;
            }
            if (order.payStatus == (int)GlobalEnumVars.OrderPayStatus.Yes || order.payStatus == (int)GlobalEnumVars.OrderPayStatus.PartialNo || order.payStatus == (int)GlobalEnumVars.OrderPayStatus.Refunded)
            {
                jm.msg = "订单" + orderId + "支付失败，订单已经支付";
                jm.data = order;
            }
            else
            {
                //赋值，用于传递完整数据到事件处理中
                order.payedAmount = order.orderAmount;
                order.paymentTime = DateTime.Now;
                order.updateTime = DateTime.Now;
                order.paymentCode = paymentCode;
                order.payStatus = (int)GlobalEnumVars.OrderPayStatus.Yes;

                var isUpdate = await _dal.UpdateAsync(
                    p => new CoreCmsOrder()
                    {
                        paymentCode = paymentCode,
                        payStatus = (int)GlobalEnumVars.OrderPayStatus.Yes,
                        paymentTime = order.paymentTime,
                        payedAmount = order.orderAmount,
                        updateTime = order.updateTime
                    }, p => p.orderId == order.orderId);
                jm.data = isUpdate;

                if (isUpdate)
                {
                    order.payStatus = (int)GlobalEnumVars.OrderPayStatus.Yes;
                    jm.status = true;
                    jm.msg = "订单支付成功";

                    //发票存储
                    if (order.taxType != (int)GlobalEnumVars.OrderTaxType.No)
                    {
                        //组装发票信息
                        var taxInfo = new CoreCmsInvoice
                        {
                            category = (int)GlobalEnumVars.OrderTaxCategory.Order,
                            sourceId = order.orderId,
                            userId = order.userId,
                            type = order.taxType,
                            title = order.taxTitle,
                            taxNumber = order.taxCode,
                            amount = order.orderAmount,
                            status = (int)GlobalEnumVars.OrderTaxStatus.No,
                            createTime = DateTime.Now
                        };

                        await _invoiceServices.InsertAsync(taxInfo);
                    }

                    //如果是门店自提，应该自动跳过发货，生成提货单信息，使用提货单核销。
                    if (order.receiptType == (int)GlobalEnumVars.OrderReceiptType.SelfDelivery)
                    {
                        var allConfigs = await _settingServices.GetConfigDictionaries();
                        var storeOrderAutomaticDelivery = CommonHelper
                            .GetConfigDictionary(allConfigs, SystemSettingConstVars.StoreOrderAutomaticDelivery)
                            .ObjectToInt(1);
                        if (storeOrderAutomaticDelivery == 1)
                        {
                            //订单自动发货
                            await _redisOperationRepository.ListLeftPushAsync(RedisMessageQueueKey.OrderAutomaticDelivery, JsonConvert.SerializeObject(order));
                        }
                    }

                    //结佣处理
                    await _redisOperationRepository.ListLeftPushAsync(RedisMessageQueueKey.OrderAgentOrDistribution, JsonConvert.SerializeObject(order));
                    //易联云打印机打印
                    await _redisOperationRepository.ListLeftPushAsync(RedisMessageQueueKey.OrderPrint, JsonConvert.SerializeObject(order));

                    //发送支付成功信息,增加发送内容
                    await _messageCenterServices.SendMessage(order.userId, GlobalEnumVars.PlatformMessageTypes.OrderPayed.ToString(), JObject.FromObject(order));
                    await _messageCenterServices.SendMessage(order.userId, GlobalEnumVars.PlatformMessageTypes.SellerOrderNotice.ToString(), JObject.FromObject(order));

                    //用户升级处理
                    await _redisOperationRepository.ListLeftPushAsync(RedisMessageQueueKey.UserUpGrade, JsonConvert.SerializeObject(order));

                }
            }
            //订单记录
            var orderLog = new CoreCmsOrderLog
            {
                orderId = order.orderId,
                userId = order.userId,
                type = (int)GlobalEnumVars.OrderLogTypes.LOG_TYPE_PAY,
                msg = jm.msg,
                data = JsonConvert.SerializeObject(jm),
                createTime = DateTime.Now
            };
            await _orderLogServices.InsertAsync(orderLog);

            return jm;
        }
        #endregion

        #region 取消订单
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> CancelOrder(string[] ids, int userId = 0)
        {
            var jm = new WebApiCallBack();

            var where = PredicateBuilder.True<CoreCmsOrder>();
            where = where.And(p => ids.Contains(p.orderId));
            where = where.And(p => p.payStatus == (int)GlobalEnumVars.OrderPayStatus.No);
            where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
            where = where.And(p => p.shipStatus == (int)GlobalEnumVars.OrderShipStatus.No);

            var msg = "后台订单取消操作";
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
                msg = "订单取消操作";
            }
            var orderInfo = await _dal.QueryListByClauseAsync(where);
            if (orderInfo != null && orderInfo.Any())
            {
                //更改状态和库存
                foreach (var item in orderInfo)
                {
                    //订单记录
                    var orderLog = new CoreCmsOrderLog
                    {
                        orderId = item.orderId,
                        userId = item.userId,
                        type = (int)GlobalEnumVars.OrderLogTypes.LOG_TYPE_CANCEL,
                        msg = msg,
                        data = JsonConvert.SerializeObject(orderInfo),
                        createTime = DateTime.Now
                    };
                    await _orderLogServices.InsertAsync(orderLog);

                    if (item.point > 0)
                    {
                        await _userPointLogServices.SetPoint(item.userId, item.point, (int)GlobalEnumVars.UserPointSourceTypes.PointCanCelOrder, "取消订单：" + item.orderId + "返还积分");
                    }

                    if (!string.IsNullOrEmpty(item.coupon))
                    {
                        await _couponServices.CancelReturnCoupon(item.coupon);
                    }

                }
                //状态修改
                await _dal.UpdateAsync(
                    p => new CoreCmsOrder()
                    {
                        status = (int)GlobalEnumVars.OrderStatus.Cancel,
                        updateTime = DateTime.Now
                    }, p => ids.Contains(p.orderId));

                var orderItems = await _orderItemServices.QueryListByClauseAsync(p => ids.Contains(p.orderId));
                //更改库存
                foreach (var item in orderItems)
                {
                    _goodsServices.ChangeStock(item.productId, GlobalEnumVars.OrderChangeStockType.cancel.ToString(), item.nums);
                }
                jm.status = true;
                jm.msg = "订单取消成功";
            }
            else
            {
                jm.msg = "订单取消失败";
            }

            return jm;
        }
        #endregion

        #region 后端根据订单状态生成不同的操作按钮

        /// <summary>
        /// 后端根据订单状态生成不同的操作按钮
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="payStatus">支付状态</param>
        /// <param name="shipStatus">发货状态</param>
        /// <param name="receiptType">收货方式</param>
        /// <param name="isDel">是否删除</param>
        /// <returns></returns>
        public string GetOperating(string orderId, int orderStatus, int payStatus, int shipStatus, int receiptType, bool isDel)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<button class='layui-btn layui-btn-primary layui-btn-xs view-order' lay-active='viewOrder' data-id='" + orderId + "'>查看</button><br>");
            //正常订单
            if (orderStatus == (int)GlobalEnumVars.OrderStatus.Normal)
            {
                if (payStatus == (int)GlobalEnumVars.OrderPayStatus.No)
                {
                    html.Append("<a class='layui-btn layui-btn-xs pay-order' lay-active='payOrder' data-id='" + orderId + "'>支付</a><br>");
                    html.Append("<a class='layui-btn layui-btn-xs edit-order' lay-active='editOrder' data-id='" + orderId + "'>编辑</a><br>");
                    html.Append("<a class='layui-btn layui-btn-xs cancel-order' lay-active='cancelOrder' data-id='" + orderId + "'>取消</a><br>");
                }
                else
                {
                    if ((shipStatus == (int)GlobalEnumVars.OrderShipStatus.No || shipStatus == (int)GlobalEnumVars.OrderShipStatus.PartialYes))
                    {
                        html.Append("<a class='layui-btn layui-btn-xs edit-order' lay-active='editOrder' data-id='" + orderId + "'>编辑</a><br>");
                        html.Append("<a class='layui-btn layui-btn-xs ship-order' lay-active='shipOrder' data-id='" + orderId + "'>发货</a><br>");

                        if (receiptType == (int)GlobalEnumVars.OrderReceiptType.IntraCityService || receiptType == (int)GlobalEnumVars.OrderReceiptType.SelfDelivery)
                        {
                            html.Append("<a class='layui-btn layui-btn-xs  layui-btn-normal seconds-ship-order' lay-active='secondsShipOrder' data-id='" + orderId + "'>秒发</a><br>");
                        }
                    }
                    else
                    {
                        html.Append("<a class='layui-btn layui-btn-xs complete-order' lay-active='completeOrder' data-id='" + orderId + "'>完成</a><br>");
                    }
                }
            }
            //已取消的订单
            if (orderStatus == (int)GlobalEnumVars.OrderStatus.Cancel && isDel == false)
            {
                html.Append("<a class='layui-btn layui-btn-danger layui-btn-xs del-order' lay-active='delOrder' data-id='" + orderId + "'>删除</a><br>");
            }

            //已取消的订单
            if (isDel == true)
            {
                html.Append("<a class='layui-btn layui-btn-warm layui-btn-xs restore-order' lay-active='restoreOrder' data-id='" + orderId + "'>还原</a><br>");
            }

            return html.ToString();
        }
        #endregion

        #region 构建多个需要发货的数据，和发货单密切关联
        /// <summary>
        /// 构建多个需要发货的数据，和发货单密切关联
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetOrderShipInfo(string[] ids)
        {
            var jm = new WebApiCallBack { status = true };

            var where = PredicateBuilder.True<CoreCmsOrder>();
            where = where.And(p => ids.Contains(p.orderId));

            var orderInfo = await _dal.QueryListByClauseAsync(where);
            if (orderInfo == null || !orderInfo.Any())
            {
                jm.msg = "请选择订单";
                return jm;
            }
            var orderItems = await _orderItemServices.QueryListByClauseAsync(p => ids.Contains(p.orderId));
            var isStoreId = 0;//校验是普通快递收货，还是门店自提，这两种收货方式不能混着发
                              //更改状态和库存
            foreach (var item in orderInfo)
            {
                item.items = orderItems.Where(p => p.orderId == item.orderId).ToList();

                if (item.status != (int)GlobalEnumVars.OrderStatus.Normal)
                {
                    jm.status = false;
                    jm.msg = "订单号：" + item.orderId + "非正常状态不能发货。<br />";
                }
                else if (item.payStatus == (int)GlobalEnumVars.OrderPayStatus.No)
                {
                    jm.status = false;
                    jm.msg = "订单号：" + item.orderId + "未支付不能发货。<br />";
                }
                else if (item.shipStatus != (int)GlobalEnumVars.OrderShipStatus.No && item.shipStatus != (int)GlobalEnumVars.OrderShipStatus.PartialYes)
                {
                    jm.status = false;
                    jm.msg = "订单号：" + item.orderId + "不是待发货和部分发货状态不能发货。<br />";
                }
                //校验，不能普通快递和门店自提，不能混发
                if (isStoreId != 0)
                {
                    if (isStoreId != item.storeId)
                    {
                        jm.status = false;
                        jm.msg = "门店自提订单和普通订单不能混合发货";
                        return jm;
                    }
                }
                else
                {
                    isStoreId = item.storeId;
                }
                //判断是否有未审核的售后单，如果有，就不能发货，已做拦截
                var isHaveBillAfterSales = await _billAftersalesServices.ExistsAsync(p =>
                    p.orderId == item.orderId &&
                    p.status == (int)GlobalEnumVars.BillAftersalesStatus.WaitAudit);
                if (isHaveBillAfterSales)
                {
                    jm.status = false;
                    jm.msg = "订单号：" + item.orderId + "有未审核的售后单，请先处理掉才能发货。";
                    return jm;
                }
                AfterSalesVal(item, 0);
            }

            if (!jm.status)
            {
                return jm;
            }

            var userIdArr = true;
            var userId = 0;
            var shipInfoArr = true;
            var shipInfoId = string.Empty;



            var newOrder = new AdminOrderShipResult()
            {
                orderId = ids,
                weight = 0,
                costFreight = 0,
                storeId = orderInfo[0].storeId,
                shipAreaId = orderInfo[0].shipAreaId,
                shipAddress = orderInfo[0].shipAddress,
                shipName = orderInfo[0].shipName,
                shipMobile = orderInfo[0].shipMobile,
                logisticsId = orderInfo[0].logisticsId,
                logisticsName = orderInfo[0].logisticsName,
                items = new List<CoreCmsOrderItem>(),
                orders = orderInfo  //把订单信息冗余上去
            };
            newOrder.memo = new List<string>();

            if (newOrder.logisticsId > 0)
            {
                newOrder.ship = await _shipServices.QueryByClauseAsync(p => p.id == newOrder.logisticsId);
            }

            foreach (var item in orderInfo)
            {
                //组合总重量
                newOrder.weight += item.weight;
                //组合总运费
                newOrder.costFreight += item.costFreight;
                //组合备注信息
                if (!string.IsNullOrEmpty(item.memo))
                {
                    newOrder.memo.Add(item.orderId + ":" + item.memo);
                }

                foreach (var orderItem in item.items)
                {
                    var model = newOrder.items.FirstOrDefault(p => p.productId == orderItem.productId);
                    if (model == null)
                    {
                        newOrder.items.Add(orderItem);
                    }
                    else
                    {
                        var index = newOrder.items.IndexOf(model);
                        newOrder.items[index].nums += orderItem.nums;//总数量
                        newOrder.items[index].weight += orderItem.weight;//总重量
                        newOrder.items[index].sendNums += orderItem.sendNums;//已发送数量
                        newOrder.items[index].reshipNums += orderItem.reshipNums;//退货数量
                    }
                }
                //判断是否有多个用户的订单
                if (userIdArr && userId == 0)
                {
                    userId = item.userId;
                }
                else
                {
                    if (userId != item.userId)
                    {
                        userIdArr = false;
                    }
                }
                //判断是否是多个收货地址
                if (shipInfoArr && shipInfoId == string.Empty)
                {
                    shipInfoId = item.shipAreaId + item.shipAddress;
                }
                else
                {
                    if (shipInfoId != item.shipAreaId + item.shipAddress)
                    {
                        shipInfoArr = false;
                    }
                }
            }

            //判断用户
            if (userIdArr == false) jm.msg += "多个用户订单";
            //判断多个收货地址
            if (shipInfoArr == false) jm.msg += "多个收货地址";
            //是否有警告
            if (string.IsNullOrEmpty(jm.msg))
            {
                jm.msg = "请注意！合并发货订单中存在：" + jm.msg + "。确定发货吗？";
            }
            jm.status = true;
            jm.data = newOrder;

            return jm;
        }
        #endregion

        #region 构建单个需要发货的数据，和发货单密切关联
        /// <summary>
        /// 构建单个需要发货的数据，和发货单密切关联
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetOrderShipInfo(string orderId)
        {
            var jm = new WebApiCallBack { status = true };

            var orderInfo = await _dal.QueryByClauseAsync(p => p.orderId == orderId);
            if (orderInfo == null)
            {
                jm.msg = "请选择订单";
                return jm;
            }
            orderInfo.items = await _orderItemServices.QueryListByClauseAsync(p => p.orderId == orderId);
            var isStoreId = 0;//校验是普通快递收货，还是门店自提，这两种收货方式不能混着发
                              //更改状态和库存

            if (orderInfo.status != (int)GlobalEnumVars.OrderStatus.Normal)
            {
                jm.status = false;
                jm.msg = "订单号：" + orderInfo.orderId + "非正常状态不能发货。<br />";
            }
            else if (orderInfo.payStatus == (int)GlobalEnumVars.OrderPayStatus.No)
            {
                jm.status = false;
                jm.msg = "订单号：" + orderInfo.orderId + "未支付不能发货。<br />";
            }
            else if (orderInfo.shipStatus != (int)GlobalEnumVars.OrderShipStatus.No && orderInfo.shipStatus != (int)GlobalEnumVars.OrderShipStatus.PartialYes)
            {
                jm.status = false;
                jm.msg = "订单号：" + orderInfo.orderId + "不是待发货和部分发货状态不能发货。<br />";
            }
            //校验，不能普通快递和门店自提，不能混发
            isStoreId = orderInfo.storeId;

            //判断是否有未审核的售后单，如果有，就不能发货，已做拦截
            var isHaveBillAfterSales = await _billAftersalesServices.ExistsAsync(p =>
                p.orderId == orderInfo.orderId &&
                p.status == (int)GlobalEnumVars.BillAftersalesStatus.WaitAudit);
            if (isHaveBillAfterSales)
            {
                jm.status = false;
                jm.msg = "订单号：" + orderInfo.orderId + "有未审核的售后单，请先处理掉才能发货。";
                return jm;
            }
            AfterSalesVal(orderInfo, 0);


            if (!jm.status)
            {
                return jm;
            }

            var newOrder = new AdminOrderShipOneResult()
            {
                orderId = orderId,
                weight = orderInfo.weight,
                costFreight = orderInfo.costFreight,
                storeId = orderInfo.storeId,
                shipAreaId = orderInfo.shipAreaId,
                shipAddress = orderInfo.shipAddress,
                shipName = orderInfo.shipName,
                shipMobile = orderInfo.shipMobile,
                logisticsId = orderInfo.logisticsId,
                logisticsName = orderInfo.logisticsName,
                items = new List<CoreCmsOrderItem>(),
                orderInfo = orderInfo,
                memo = orderInfo.memo
            };

            if (newOrder.logisticsId > 0)
            {
                newOrder.ship = await _shipServices.QueryByClauseAsync(p => p.id == newOrder.logisticsId);
            }

            //组合总运费
            foreach (var orderItem in orderInfo.items)
            {
                var model = newOrder.items.FirstOrDefault(p => p.productId == orderItem.productId);
                if (model == null)
                {
                    newOrder.items.Add(orderItem);
                }
                else
                {
                    var index = newOrder.items.IndexOf(model);
                    newOrder.items[index].nums += orderItem.nums;//总数量
                    newOrder.items[index].weight += orderItem.weight;//总重量
                    newOrder.items[index].sendNums += orderItem.sendNums;//已发送数量
                    newOrder.items[index].reshipNums += orderItem.reshipNums;//退货数量
                }
            }

            jm.status = true;
            jm.data = newOrder;

            return jm;
        }
        #endregion

        #region 发货改状态
        /// <summary>
        /// 发货改状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> EditShipStatus(string orderId, Dictionary<int, int> items)
        {
            var jm = new WebApiCallBack();

            //未发货，部分发货，部分退货状态(怕部分发货中的部分退货这种业务场景，所以加这个字段)
            var shipStatus = new[] { (int)GlobalEnumVars.OrderShipStatus.No, (int)GlobalEnumVars.OrderShipStatus.PartialNo, (int)GlobalEnumVars.OrderShipStatus.PartialYes };
            var orderItem = await _dal.QueryByClauseAsync(p => p.orderId == orderId && p.status == (int)GlobalEnumVars.OrderStatus.Normal && shipStatus.Contains(p.shipStatus));
            if (orderItem == null)
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }
            //更新订单明细发货数量，并校验是否发完
            var isOver = await _orderItemServices.ship(orderId, items);
            if (isOver)
            {
                await _dal.UpdateAsync(
                    p => new CoreCmsOrder() { shipStatus = (int)GlobalEnumVars.OrderShipStatus.Yes },
                    p => p.orderId == orderId);
            }
            else
            {
                await _dal.UpdateAsync(
                    p => new CoreCmsOrder() { shipStatus = (int)GlobalEnumVars.OrderShipStatus.PartialYes },
                    p => p.orderId == orderId);
            }
            jm.status = true;

            return jm;
        }
        #endregion

        #region 订单批量发货

        /// <summary>
        /// 订单批量发货
        /// </summary>
        /// <param name="ids">订单标号</param>
        /// <param name="logiCode">物流公司编码</param>
        /// <param name="logiNo">物流单号</param>
        /// <param name="items">发货明细</param>
        /// <param name="shipName">收货人姓名</param>
        /// <param name="shipMobile">收货人电话</param>
        /// <param name="shipAddress">收货地址</param>
        /// <param name="memo">发货描述</param>
        /// <param name="storeId">店铺收货地址</param>
        /// <param name="shipAreaId">省市区id</param>
        /// <param name="deliveryCompanyId">直播物流编码</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BatchShip(string[] ids, string logiCode, string logiNo,
            Dictionary<int, int> items, string shipName, string shipMobile, string shipAddress, string memo, int storeId = 0, int shipAreaId = 0, string deliveryCompanyId = "")
        {

            var result = await _billDeliveryServices.BatchShip(ids, logiCode, logiNo, items, storeId, shipName, shipMobile, shipAreaId, shipAddress, memo);
            return result;

        }
        #endregion

        #region 订单单个发货

        /// <summary>
        /// 订单单个发货
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="logiCode">物流公司编码</param>
        /// <param name="logiNo">物流单号</param>
        /// <param name="items">发货明细</param>
        /// <param name="shipName">收货人姓名</param>
        /// <param name="shipMobile">收货人电话</param>
        /// <param name="shipAddress">收货地址</param>
        /// <param name="memo">发货描述</param>
        /// <param name="storeId">店铺收货地址</param>
        /// <param name="shipAreaId">省市区id</param>
        /// <param name="deliveryCompanyId">直播物流编码</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Ship(string orderId, string logiCode, string logiNo,
            Dictionary<int, int> items, string shipName, string shipMobile, string shipAddress, string memo, int storeId = 0, int shipAreaId = 0, string deliveryCompanyId = "")
        {
            var result = await _billDeliveryServices.Ship(orderId, logiCode, logiNo, items, storeId, shipName, shipMobile, shipAreaId, shipAddress, memo);
            return result;

        }
        #endregion



        #region 完成订单

        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="score">有序队列积分</param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CompleteOrder(string orderId, int score = 0, string remark = "后台订单完成操作")
        {
            var jm = new WebApiCallBack();

            //等待售后审核的订单，不自动操作完成。
            var billAftersalesCount = await _billAftersalesServices.GetCountAsync(p => p.orderId == orderId && p.status == (int)GlobalEnumVars.BillAftersalesStatus.WaitAudit);

            if (billAftersalesCount > 0)
            {
                jm.msg = "售后单未处理";
                return jm;
            }
            var where = PredicateBuilder.True<CoreCmsOrder>();
            where = where.And(p => p.payStatus != (int)GlobalEnumVars.OrderPayStatus.No && p.orderId == orderId);
            var orderInfo = await _dal.QueryByClauseAsync(where);
            if (orderInfo != null)
            {
                await _dal.UpdateAsync(p => new CoreCmsOrder() { status = (int)GlobalEnumVars.OrderStatus.Complete, updateTime = DateTime.Now }, p => p.orderId == orderId);

                //计算订单实际支付金额（要减去售后退款的金额）
                var money = orderInfo.payedAmount;

                //查询售后单
                var baList = await _billAftersalesServices.QueryListByClauseAsync(p =>
                    p.orderId == orderId && p.status == (int)GlobalEnumVars.BillAftersalesStatus.Success);
                if (baList != null && baList.Count > 0)
                {
                    decimal refundMoney = 0;
                    foreach (var item in baList)
                    {
                        refundMoney = Math.Round(refundMoney + item.refundAmount, 2);
                    }
                    money = Math.Round(money - refundMoney, 2);
                }
                //奖励积分
                await _userPointLogServices.OrderComplete(orderInfo.userId, money, orderInfo.orderId);

                //如果订单是已完成，但是订单的未发货商品还有的话，需要解冻库存
                var orderItems = await _orderItemServices.QueryListByClauseAsync(p => p.orderId == orderId);
                foreach (var item in orderItems)
                {
                    var nums = item.nums - item.sendNums - (item.reshipNums - item.reshipedNums);//还未发货的数量
                    if (nums > 0)
                    {
                        _goodsServices.ChangeStock(item.productId, GlobalEnumVars.OrderChangeStockType.complete.ToString(), nums);
                    }
                }

                //订单记录
                var orderLog = new CoreCmsOrderLog
                {
                    userId = orderInfo.userId,
                    orderId = orderInfo.orderId,
                    type = (int)GlobalEnumVars.OrderLogTypes.LOG_TYPE_COMPLETE,
                    msg = "后台订单完成操作",
                    data = JsonConvert.SerializeObject(orderInfo),
                    createTime = DateTime.Now
                };
                await _orderLogServices.InsertAsync(orderLog);

                //订单完成结算订单
                await _redisOperationRepository.SortedSetAddAsync(RedisMessageQueueKey.OrderFinishCommand, orderInfo.orderId, score);

                jm.status = true;
                jm.msg = "订单完成";

            }
            else
            {
                jm.status = false;
                jm.msg = "未获取到对应订单数据";
            }

            return jm;
        }
        #endregion

        #region 确认签收订单
        /// <summary>
        /// 确认签收订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ConfirmOrder(string orderId, int userId = 0)
        {
            var jm = new WebApiCallBack();

            var where = PredicateBuilder.True<CoreCmsOrder>();
            where = where.And(p => p.orderId == orderId);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            where = where.And(p => p.payStatus != (int)GlobalEnumVars.OrderPayStatus.No);
            where = where.And(p => p.shipStatus != (int)GlobalEnumVars.OrderShipStatus.No);
            where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
            where = where.And(p => p.confirmStatus != (int)GlobalEnumVars.OrderConfirmStatus.ConfirmReceipt);

            var bl = await _dal.UpdateAsync(
                p => new CoreCmsOrder()
                {
                    confirmStatus = (int)GlobalEnumVars.OrderConfirmStatus.ConfirmReceipt,
                    confirmTime = DateTime.Now
                }, where);
            if (!bl)
            {
                jm.msg = "确认收货失败";
                return jm;
            }
            //修改发货单,如果有为确认收货的发货单，那么给他们回传上去确认收货时间

            //订单记录
            var orderLog = new CoreCmsOrderLog
            {
                orderId = orderId,
                userId = userId,
                type = (int)GlobalEnumVars.OrderLogTypes.LOG_TYPE_SIGN,
                msg = "确认收货成功",
                data = JsonConvert.SerializeObject(jm),
                createTime = DateTime.Now
            };
            await _orderLogServices.InsertAsync(orderLog);

            jm.status = true;
            jm.msg = "确认收货成功";



            return jm;
        }
        #endregion

        #region 判断订单是否可以进行评论
        /// <summary>
        /// 判断订单是否可以进行评论
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> IsOrderComment(string orderId, int userId)
        {
            var jm = new WebApiCallBack();

            var order = await _dal.QueryByClauseAsync(p => p.orderId == orderId && p.userId == userId);
            if (order != null)
            {
                if (order.payStatus > (int)GlobalEnumVars.OrderPayStatus.No && order.status == (int)GlobalEnumVars.OrderStatus.Normal && order.shipStatus > (int)GlobalEnumVars.OrderShipStatus.No && order.status == (int)GlobalEnumVars.OrderStatus.Normal && order.isComment == false)
                {
                    jm.status = true;
                    jm.msg = "可以评价";
                    jm.data = order;
                }
                else
                {
                    jm.status = false;
                    jm.msg = "订单状态存在问题，不能评价";
                    jm.data = order;
                }
            }
            else
            {
                jm.status = false;
                jm.msg = "不存在这个订单";
            }

            return jm;
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

            return await _dal.QueryListAsync(predicate, orderByExpression, orderByType);
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
        public new async Task<IPageList<CoreCmsOrder>> QueryPageAsync(Expression<Func<CoreCmsOrder, bool>> predicate,
            Expression<Func<CoreCmsOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion

        #region 自动取消订单（定时任务使用）
        /// <summary>
        /// 自动取消订单（定时任务使用）
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> AutoCancelOrder()
        {
            var jm = new WebApiCallBack();

            var allConfigs = await _settingServices.GetConfigDictionaries();
            var time = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OrderCancelTime).ObjectToInt(1);
            var endTime = DateTime.Now.AddMinutes(-time);

            var where = PredicateBuilder.True<CoreCmsOrder>();
            where = where.And(p => p.payStatus == (int)GlobalEnumVars.OrderPayStatus.No);
            where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
            //where = where.And(p => p.orderType == (int)GlobalEnumVars.OrderType.Common || p.orderType == (int)GlobalEnumVars.OrderType.PinTuan);
            where = where.And(p => p.createTime <= endTime);

            var orderInfos = await _dal.QueryListByClauseAsync(where);

            jm.status = true;
            jm.msg = "取消成功";


            if (orderInfos != null && orderInfos.Any())
            {
                var ids = orderInfos.Select(p => p.orderId).ToArray();
                jm = await CancelOrder(ids);
            }


            //插入日志
            var model = new SysTaskLog
            {
                createTime = DateTime.Now,
                isSuccess = jm.status,
                name = "自动取消订单",
                parameters = JsonConvert.SerializeObject(jm)
            };
            await _taskLogServices.InsertAsync(model);

            return jm;
        }
        #endregion

        #region 自动完成订单（定时任务使用）
        /// <summary>
        /// 自动完成订单（定时任务使用）
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> AutoCompleteOrder()
        {
            var jm = new WebApiCallBack();

            var allConfigs = await _settingServices.GetConfigDictionaries();
            var time = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OrderCompleteTime).ObjectToInt(30);
            var endTime = DateTime.Now.AddDays(-time);

            var where = PredicateBuilder.True<CoreCmsOrder>();
            where = where.And(p => p.payStatus == (int)GlobalEnumVars.OrderPayStatus.Yes);
            where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
            where = where.And(p => p.shipStatus == (int)GlobalEnumVars.OrderShipStatus.Yes);
            where = where.And(p => p.confirmStatus == (int)GlobalEnumVars.OrderConfirmStatus.ConfirmReceipt);
            where = where.And(p => p.paymentTime <= endTime);

            var orderInfos = await _dal.QueryListByClauseAsync(where);

            jm.status = true;
            jm.msg = "完成成功";

            if (orderInfos != null && orderInfos.Any())
            {
                for (var i = 0; i < orderInfos.Count; i++)
                {
                    var item = orderInfos[i];
                    var score = 2 * (i + 1);
                    await CompleteOrder(item.orderId, score, "定时任务操作");
                }
            }
            //插入日志
            var model = new SysTaskLog
            {
                createTime = DateTime.Now,
                isSuccess = jm.status,
                name = "订单自动完成",
                parameters = JsonConvert.SerializeObject(jm)
            };
            await _taskLogServices.InsertAsync(model);

            return jm;
        }
        #endregion

        #region 自动评价订单（定时任务使用）
        /// <summary>
        /// 自动评价订单（定时任务使用）
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> AutoEvaluateOrder()
        {
            var jm = new WebApiCallBack();

            var allConfigs = await _settingServices.GetConfigDictionaries();
            var time = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OrderAutoEvalTime).ObjectToInt(5);
            var endTime = DateTime.Now.AddDays(-time);

            var where = PredicateBuilder.True<CoreCmsOrder>();
            where = where.And(p => p.payStatus == (int)GlobalEnumVars.OrderPayStatus.Yes);
            where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
            where = where.And(p => p.shipStatus == (int)GlobalEnumVars.OrderShipStatus.Yes);
            where = where.And(p => p.confirmStatus == (int)GlobalEnumVars.OrderConfirmStatus.ConfirmReceipt);
            where = where.And(p => p.isComment == false);
            where = where.And(p => p.confirmTime <= endTime);

            var orderInfos = await _dal.QueryListByClauseAsync(where);


            if (orderInfos != null && orderInfos.Any())
            {
                //订单记录
                var logs = new List<CoreCmsOrderLog>();
                foreach (var orderInfo in orderInfos)
                {
                    var orderLog = new CoreCmsOrderLog
                    {
                        userId = orderInfo.userId,
                        orderId = orderInfo.orderId,
                        type = (int)GlobalEnumVars.OrderLogTypes.LOG_TYPE_AUTO_EVALUATION,
                        msg = "订单后台自动评价(定时任务)",
                        data = JsonConvert.SerializeObject(orderInfo),
                        createTime = DateTime.Now
                    };
                    logs.Add(orderLog);
                }
                await _orderLogServices.InsertAsync(logs);

                //更新订单
                var ids = orderInfos.Select(p => p.orderId).ToList();
                await _dal.UpdateAsync(p => new CoreCmsOrder() { isComment = true, updateTime = DateTime.Now },
                    p => ids.Contains(p.orderId));

                //查询评价商品
                var orderItems = await _orderItemServices.QueryListByClauseAsync(p => ids.Contains(p.orderId));

                var listGoodsComment = new List<CoreCmsGoodsComment>();
                foreach (var item in orderItems)
                {
                    var orderInfo = orderInfos.Find(p => p.orderId == item.orderId);
                    var commentModel = new CoreCmsGoodsComment
                    {
                        commentId = 0,
                        score = 5,
                        userId = orderInfo?.userId ?? 0,
                        goodsId = item.goodsId,
                        orderId = item.orderId,
                        contentBody = "用户" + time + "天内未对商品做出评价，已由系统自动评价。",
                        addon = item.addon,
                        isDisplay = true,
                        createTime = DateTime.Now
                    };
                    listGoodsComment.Add(commentModel);
                }

                await _goodsCommentServices.InsertAsync(listGoodsComment);
            }


            jm.status = true;
            jm.msg = "评价订单成功";


            //插入日志
            var model = new SysTaskLog
            {
                createTime = DateTime.Now,
                isSuccess = jm.status,
                name = "订单自动评价",
                parameters = JsonConvert.SerializeObject(jm)
            };
            await _taskLogServices.InsertAsync(model);

            return jm;
        }
        #endregion

        #region 自动签收订单（定时任务使用）
        /// <summary>
        /// 自动签收订单（定时任务使用）
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> AutoSignOrder()
        {
            var jm = new WebApiCallBack();

            var allConfigs = await _settingServices.GetConfigDictionaries();
            var time = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OrderAutoSignTime).ObjectToInt(20);
            var endTime = DateTime.Now.AddDays(-time);

            var where = PredicateBuilder.True<CoreCmsOrder>();
            where = where.And(p => p.payStatus == (int)GlobalEnumVars.OrderPayStatus.Yes);
            where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
            where = where.And(p => p.shipStatus == (int)GlobalEnumVars.OrderShipStatus.Yes);
            where = where.And(p => p.updateTime <= endTime);

            var orderInfos = await _dal.QueryListByClauseAsync(where);

            if (orderInfos != null && orderInfos.Any())
            {
                foreach (var item in orderInfos)
                {
                    await ConfirmOrder(item.orderId);
                }
            }

            jm.status = true;
            jm.msg = "自动签收订单成功";

            //插入日志
            var model = new SysTaskLog
            {
                createTime = DateTime.Now,
                isSuccess = jm.status,
                name = "自动签收订单",
                parameters = JsonConvert.SerializeObject(jm)
            };
            await _taskLogServices.InsertAsync(model);

            return jm;
        }
        #endregion

        #region 催付款订单（定时任务使用）
        /// <summary>
        /// 催付款订单（定时任务使用）
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> RemindOrderPay()
        {
            var jm = new WebApiCallBack();

            var allConfigs = await _settingServices.GetConfigDictionaries();
            var time = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.RemindOrderTime).ObjectToInt(1);
            var dt = DateTime.Now;
            //var endTime = DateTime.Now.AddHours(-time);

            var where = PredicateBuilder.True<CoreCmsOrder>();
            where = where.And(p => p.payStatus == (int)GlobalEnumVars.OrderPayStatus.No);
            where = where.And(p => p.status == (int)GlobalEnumVars.OrderStatus.Normal);
            where = where.And(p => dt <= SqlFunc.DateAdd(p.createTime, time, DateType.Minute));
            //where = where.And(p => p.createTime >= SqlFunc.DateAdd(p.createTime, -time, DateType.Minute));

            var orderInfos = await _dal.QueryListByClauseAsync(where);

            if (orderInfos != null && orderInfos.Any())
            {
                foreach (var item in orderInfos)
                {
                    await _messageCenterServices.SendMessage(item.userId, GlobalEnumVars.PlatformMessageTypes.RemindOrderPay.ToString(), JObject.FromObject(item));
                }
            }

            jm.status = true;
            jm.msg = "催付款订单成功";

            //插入日志
            var model = new SysTaskLog
            {
                createTime = DateTime.Now,
                isSuccess = jm.status,
                name = "催付款订单",
                parameters = JsonConvert.SerializeObject(jm)
            };
            await _taskLogServices.InsertAsync(model);

            return jm;
        }
        #endregion

    }
}
