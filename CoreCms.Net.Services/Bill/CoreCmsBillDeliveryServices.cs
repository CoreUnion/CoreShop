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
using System.Threading.Tasks;
using CoreCms.Net.Caching.AutoMate.RedisCache;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Api;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Helper;
using Flurl.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 发货单表 接口实现
    /// </summary>
    public class CoreCmsBillDeliveryServices : BaseServices<CoreCmsBillDelivery>, ICoreCmsBillDeliveryServices
    {
        private readonly ICoreCmsBillDeliveryRepository _dal;
        private readonly ICoreCmsStoreServices _storeServices;
        private readonly ICoreCmsBillDeliveryItemServices _billDeliveryItemServices;
        private readonly ICoreCmsOrderLogServices _orderLogServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRedisOperationRepository _redisOperationRepository;



        public CoreCmsBillDeliveryServices(
            IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider
            , ICoreCmsBillDeliveryRepository dal
            , ICoreCmsStoreServices storeServices
            , ICoreCmsBillDeliveryItemServices billDeliveryItemServices
            , ICoreCmsOrderLogServices orderLogServices
            , ICoreCmsSettingServices settingServices, IRedisOperationRepository redisOperationRepository)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _storeServices = storeServices;
            _billDeliveryItemServices = billDeliveryItemServices;
            _orderLogServices = orderLogServices;
            _settingServices = settingServices;
            _redisOperationRepository = redisOperationRepository;
        }

        /// <summary>
        /// 批量发货，可以支持多个订单合并发货，单个订单拆分发货等。
        /// </summary>
        /// <param name="orderId">英文逗号分隔的订单号</param>
        /// <param name="logiCode">物流公司编码</param>
        /// <param name="logiNo">物流单号</param>
        /// <param name="items">发货明细</param>
        /// <param name="storeId">店铺收货地址</param>
        /// <param name="shipName">收货人姓名</param>
        /// <param name="shipMobile">收货人电话</param>
        /// <param name="shipAreaId">省市区id</param>
        /// <param name="shipAddress">收货地址</param>
        /// <param name="memo">发货描述</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> BatchShip(string[] orderId, string logiCode, string logiNo, Dictionary<int, int> items, int storeId = 0, string shipName = "", string shipMobile = "", int shipAreaId = 0, string shipAddress = "", string memo = "")
        {
            using var container = _serviceProvider.CreateScope();
            var jm = new WebApiCallBack();

            var orderService = container.ServiceProvider.GetService<ICoreCmsOrderServices>();
            var stockServices = container.ServiceProvider.GetService<ICoreCmsStockServices>();
            //获取订单详情
            var dInfoResult = await orderService.GetOrderShipInfo(orderId);
            if (!dInfoResult.status)
            {
                return dInfoResult;
            }
            var dInfo = dInfoResult.data as AdminOrderShipResult;
            var orders = dInfo.orders;

            //校验门店自提和普通订单收货地址是否填写
            if (storeId != 0)
            {
                var storeModel = await _storeServices.QueryByIdAsync(storeId);
                if (storeModel == null)
                {
                    jm.msg = GlobalErrorCodeVars.Code10000;
                    jm.data = 1000;
                    jm.code = 1000;
                    return jm;
                }
                shipName = storeModel.storeName;
                shipMobile = storeModel.mobile;
                shipAreaId = storeModel.areaId;
                shipAddress = storeModel.address;
            }
            if (string.IsNullOrEmpty(shipName) || string.IsNullOrEmpty(shipMobile) || string.IsNullOrEmpty(shipAddress) || shipAreaId == 0)
            {
                jm.msg = "收货地址信息不全";
                jm.otherData = new
                {
                    shipName,
                    shipMobile,
                    shipAddress,
                    shipAreaId
                };
                return jm;
            }
            var billDelivery = new CoreCmsBillDelivery();
            billDelivery.orderId = string.Join(",", orderId);
            billDelivery.deliveryId = CommonHelper.GetSerialNumberType((int)GlobalEnumVars.SerialNumberType.发货单编号);
            billDelivery.logiCode = logiCode;
            billDelivery.logiNo = logiNo;
            billDelivery.shipAreaId = shipAreaId;
            billDelivery.shipAddress = shipAddress;
            billDelivery.shipName = shipName;
            billDelivery.shipMobile = shipMobile;
            billDelivery.status = (int)GlobalEnumVars.BillDeliveryStatus.Already;
            billDelivery.memo = memo;
            billDelivery.createTime = DateTime.Now;


            //设置发货明细
            var bdRel = new List<CoreCmsBillDeliveryItem>();

            //校验发货内容
            var tNum = 0;
            foreach (var item in items)
            {
                var orderItem = dInfo.items.Find(p => p.productId == item.Key);
                if (orderItem == null)
                {
                    //发货的商品不在发货明细里，肯定有问题
                    jm.msg = GlobalErrorCodeVars.Code10000;
                    return jm;
                }
                //判断总发货数量
                tNum = tNum + item.Value;

                if ((orderItem.nums - orderItem.sendNums - (orderItem.reshipNums - orderItem.reshipedNums)) < item.Value)
                {
                    jm.msg = orderItem.name + "发超了";
                    return jm;
                }

                //构建发货单明细
                var bdItem = new CoreCmsBillDeliveryItem();
                bdItem.deliveryId = billDelivery.deliveryId;
                bdItem.productId = orderItem.productId;
                bdItem.goodsId = orderItem.goodsId;
                bdItem.bn = orderItem.bn;
                bdItem.sn = orderItem.sn;
                bdItem.weight = orderItem.weight;
                bdItem.name = orderItem.name;
                bdItem.addon = !string.IsNullOrEmpty(orderItem.addon) ? orderItem.addon : "";
                bdItem.nums = item.Value;
                bdRel.Add(bdItem);
            }
            if (tNum < 1)
            {
                jm.msg = "请至少发生一件商品!";
                return jm;
            }
            //事务处理开始


            //插入发货单主体表
            await _dal.InsertAsync(billDelivery);

            //插入发货单明细表
            await _billDeliveryItemServices.InsertAsync(bdRel);

            //订单更新发货状态，发送各种消息
            foreach (var order in orders)
            {
                await OrderShip(order, items, billDelivery, storeId);
            }

            var stock = new CoreCmsStock
            {
                manager = 0,
                id = billDelivery.deliveryId,
                createTime = DateTime.Now,
                type = (int)GlobalEnumVars.StockType.DeliverGoods,
                memo = "订单发货操作，发货单号：" + billDelivery.deliveryId
            };

            await stockServices.InsertAsync(stock);

            jm.status = true;
            jm.msg = "发货成功";


            return jm;
        }


        /// <summary>
        ///     发货，单个订单发货
        /// </summary>
        /// <param name="orderId">英文逗号分隔的订单号</param>
        /// <param name="logiCode">物流公司编码</param>
        /// <param name="logiNo">物流单号</param>
        /// <param name="items">发货明细</param>
        /// <param name="storeId">店铺收货地址</param>
        /// <param name="shipName">收货人姓名</param>
        /// <param name="shipMobile">收货人电话</param>
        /// <param name="shipAreaId">省市区id</param>
        /// <param name="shipAddress">收货地址</param>
        /// <param name="memo">发货描述</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Ship(string orderId, string logiCode, string logiNo, Dictionary<int, int> items, int storeId = 0, string shipName = "", string shipMobile = "", int shipAreaId = 0, string shipAddress = "", string memo = "")
        {
            using var container = _serviceProvider.CreateScope();
            var jm = new WebApiCallBack();

            var orderService = container.ServiceProvider.GetService<ICoreCmsOrderServices>();
            var stockServices = container.ServiceProvider.GetService<ICoreCmsStockServices>();

            //获取订单详情
            var dInfoResult = await orderService.GetOrderShipInfo(orderId);
            if (!dInfoResult.status)
            {
                return dInfoResult;
            }
            var dInfo = dInfoResult.data as AdminOrderShipOneResult;
            var orderInfo = dInfo.orderInfo;

            //校验门店自提和普通订单收货地址是否填写
            if (storeId != 0)
            {
                var storeModel = await _storeServices.QueryByIdAsync(storeId);
                if (storeModel == null)
                {
                    jm.msg = GlobalErrorCodeVars.Code10000;
                    jm.data = 1000;
                    jm.code = 1000;
                    return jm;
                }
                shipName = storeModel.storeName;
                shipMobile = storeModel.mobile;
                shipAreaId = storeModel.areaId;
                shipAddress = storeModel.address;
            }
            if (string.IsNullOrEmpty(shipName) || string.IsNullOrEmpty(shipMobile) || string.IsNullOrEmpty(shipAddress) || shipAreaId == 0)
            {
                jm.msg = "收货地址信息不全";
                jm.otherData = new
                {
                    shipName,
                    shipMobile,
                    shipAddress,
                    shipAreaId
                };
                return jm;
            }
            var billDelivery = new CoreCmsBillDelivery();
            billDelivery.orderId = string.Join(",", orderId);
            billDelivery.deliveryId = CommonHelper.GetSerialNumberType((int)GlobalEnumVars.SerialNumberType.发货单编号);
            billDelivery.logiCode = logiCode;
            billDelivery.logiNo = logiNo;
            billDelivery.shipAreaId = shipAreaId;
            billDelivery.shipAddress = shipAddress;
            billDelivery.shipName = shipName;
            billDelivery.shipMobile = shipMobile;
            billDelivery.status = (int)GlobalEnumVars.BillDeliveryStatus.Already;
            billDelivery.memo = memo;
            billDelivery.createTime = DateTime.Now;


            //设置发货明细
            var bdRel = new List<CoreCmsBillDeliveryItem>();

            //校验发货内容
            var tNum = 0;
            foreach (var item in items)
            {
                var orderItem = dInfo.items.Find(p => p.productId == item.Key);
                if (orderItem == null)
                {
                    //发货的商品不在发货明细里，肯定有问题
                    jm.msg = GlobalErrorCodeVars.Code10000;
                    return jm;
                }
                //判断总发货数量
                tNum = tNum + item.Value;

                if ((orderItem.nums - orderItem.sendNums - (orderItem.reshipNums - orderItem.reshipedNums)) < item.Value)
                {
                    jm.msg = orderItem.name + "发超了";
                    return jm;
                }

                //构建发货单明细
                var bdItem = new CoreCmsBillDeliveryItem();
                bdItem.deliveryId = billDelivery.deliveryId;
                bdItem.productId = orderItem.productId;
                bdItem.goodsId = orderItem.goodsId;
                bdItem.bn = orderItem.bn;
                bdItem.sn = orderItem.sn;
                bdItem.weight = orderItem.weight;
                bdItem.name = orderItem.name;
                bdItem.addon = !string.IsNullOrEmpty(orderItem.addon) ? orderItem.addon : "";
                bdItem.nums = item.Value;
                bdRel.Add(bdItem);
            }
            if (tNum < 1)
            {
                jm.msg = "请至少发生一件商品!";
                return jm;
            }
            //事务处理开始


            //插入发货单主体表
            await _dal.InsertAsync(billDelivery);

            //插入发货单明细表
            await _billDeliveryItemServices.InsertAsync(bdRel);

            //订单更新发货状态，发送各种消息
            await OrderShip(orderInfo, items, billDelivery, storeId);

            var stock = new CoreCmsStock
            {
                manager = 0,
                id = billDelivery.deliveryId,
                createTime = DateTime.Now,
                type = (int)GlobalEnumVars.StockType.DeliverGoods,
                memo = "订单发货操作，发货单号：" + billDelivery.deliveryId
            };

            await stockServices.InsertAsync(stock);

            jm.status = true;
            jm.msg = "发货成功";

            return jm;
        }



        /// <summary>
        /// 给订单发货
        /// </summary>
        /// <param name="orderInfo">订单信息</param>
        /// <param name="items">总的发货包裹内容</param>
        /// <param name="deliveryInfo">发货单信息</param>
        /// <param name="storeId">门店自提还是普通订单，0是普通订单，其他是门店自提</param>
        private async Task<bool> OrderShip(CoreCmsOrder orderInfo, Dictionary<int, int> items, CoreCmsBillDelivery deliveryInfo, int storeId = 0)
        {
            using var container = _serviceProvider.CreateScope();
            var orderService = container.ServiceProvider.GetService<ICoreCmsOrderServices>();
            var billLadingService = container.ServiceProvider.GetService<ICoreCmsBillLadingServices>();
            var messageCenterServices = container.ServiceProvider.GetService<ICoreCmsMessageCenterServices>();
            var logisticsServices = container.ServiceProvider.GetService<ICoreCmsLogisticsServices>();
            var stockLogServices = container.ServiceProvider.GetService<ICoreCmsStockLogServices>();
            var stockServices = container.ServiceProvider.GetService<ICoreCmsStockServices>();

            var stockLogs = new List<CoreCmsStockLog>();

            var itemList = new Dictionary<int, int>();
            foreach (var item in orderInfo.items)
            {
                if (items.ContainsKey(item.productId))
                {
                    var maxNum = item.nums - item.reshipNums - item.sendNums;
                    if (maxNum > 0) //如果此条订单明细需要发货的话
                    {
                        var sendNum = maxNum;
                        if (items[item.productId] > maxNum)
                        {
                            //足够发此条记录的话
                            itemList.Add(item.productId, maxNum);
                            items[item.productId] = items[item.productId] - maxNum;
                        }
                        else
                        {
                            //此条订单都发不满的情况下
                            itemList.Add(item.productId, items[item.productId]);
                            sendNum = items[item.productId];
                        }

                        var sLog = new CoreCmsStockLog
                        {
                            stockId = deliveryInfo.deliveryId,
                            productId = item.productId,
                            goodsId = item.goodsId,
                            nums = -sendNum,
                            sn = item.sn,
                            bn = item.bn,
                            goodsName = item.name,
                            spesDesc = item.addon
                        };
                        stockLogs.Add(sLog);
                    }
                }
            }
            //如果有发货信息，就去给订单更新发货状态
            if (itemList.Keys.Count <= 0)
            {
                return false;
            }
            var res = await orderService.EditShipStatus(orderInfo.orderId, itemList);

            //如果是门店自提，生成提货单
            if (storeId != 0)
            {
                await billLadingService.AddData(orderInfo.orderId, storeId, orderInfo.shipName, orderInfo.shipMobile);
            }

            if (res.status == true)
            {
                //添加操作日志
                var log = new CoreCmsOrderLog();
                log.orderId = orderInfo.orderId;
                log.userId = orderInfo.userId;
                log.type = (int)GlobalEnumVars.OrderLogTypes.LOG_TYPE_SHIP;
                log.msg = "订单发货操作，发货单号：" + deliveryInfo.deliveryId;
                log.data = JsonConvert.SerializeObject(deliveryInfo);
                log.createTime = DateTime.Now;
                await _orderLogServices.InsertAsync(log);

                var logistics = await logisticsServices.QueryByClauseAsync(p => p.logiCode == deliveryInfo.logiCode);
                deliveryInfo.logiName = logistics != null ? logistics.logiName : deliveryInfo.logiCode;
                //添加库存出库日志

                if (stockLogs.Any())
                {
                    //var stock = new CoreCmsStock
                    //{
                    //    manager = 0,
                    //    id = deliveryInfo.deliveryId,
                    //    createTime = DateTime.Now,
                    //    type = (int)GlobalEnumVars.StockType.DeliverGoods,
                    //    memo = log.msg
                    //};
                    //await stockServices.InsertAsync(stock);
                    await stockLogServices.InsertAsync(stockLogs);
                }

                //发送消息
                await messageCenterServices.SendMessage(orderInfo.userId, GlobalEnumVars.PlatformMessageTypes.DeliveryNotice.ToString(), JObject.FromObject(deliveryInfo));
            }
            return true;
        }


        /// <summary>
        /// 物流信息查询根据快递编码和单号查询(快递100)未使用
        /// </summary>
        /// <param name="code">查询的快递公司的编码， 一律用小写字母（如：yuantong）</param>
        /// <param name="no">查询的快递单号， 单号的最大长度是32个字符</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetLogistic(string code, string no)
        {
            var jm = new WebApiCallBack();

            //快递100分配给贵司的的公司编号, 请在企业管理后台查看
            var allConfigs = await _settingServices.GetConfigDictionaries();
            var customer = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.Kuaidi100Customer);
            //签名， 用于验证身份， 按param + key + customer 的顺序进行MD5加密（注意加密后字符串一定要转大写）， 不需要加上“+”号， 如{“com”: “yuantong”, “num”: “500306190180”, “from”: “广东省深圳市”, “to”: “北京市朝阳区”}xxxxxxxxxxxxyyyyyyyyyyy yyyyyyyyyyyyyyyyyyyyy
            var key = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.Kuaidi100Key);

            if (string.IsNullOrEmpty(customer) || string.IsNullOrEmpty(key))
            {
                jm.msg = "快递查询接口公司编码及授权key获取失败";
            }

            var param = new KuaiDi100ApiPostParam();
            param.com = code.ToLowerInvariant();
            param.num = no;
            param.phone = "";
            param.from = "";
            param.to = "";
            param.resultv2 = 1;
            var jsonParamData = JsonConvert.SerializeObject(param);

            //签名加密
            var str = jsonParamData + key + customer;
            var signStr = CommonHelper.Md5For32(str).ToUpper();
            //实时查询请求地址
            var postUrl = "http://poll.kuaidi100.com/poll/query.do";

            var postData = new
            {
                customer,
                param = jsonParamData,
                sign = signStr
            };

            var result = await postUrl.PostUrlEncodedAsync(postData).ReceiveJson<KuaiDi100ApiPostResult>();
            if (result.status == "200")
            {
                jm.status = true;
                jm.data = result;
            }
            else
            {
                jm.status = false;
                jm.msg = !string.IsNullOrEmpty(result.message) ? result.message : "暂无消息";
            }

            return jm;
        }


        /// <summary>
        /// 发货单统计7天统计
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatisticsOut>> Statistics()
        {
            return await _dal.Statistics();

        }

    }
}
