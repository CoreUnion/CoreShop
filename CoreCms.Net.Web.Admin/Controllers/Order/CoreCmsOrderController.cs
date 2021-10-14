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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Caching.AccressToken;
using CoreCms.Net.Caching.AutoMate.RedisCache;
using CoreCms.Net.Configuration;
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Excel;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.WeChat.Service.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 订单表
    ///</summary>
    [Description("订单表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsOrderController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsOrderServices _coreCmsOrderServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsAreaServices _areaServices;
        private readonly ICoreCmsBillAftersalesServices _aftersalesServices;
        private readonly ICoreCmsBillPaymentsServices _billPaymentsServices;
        private readonly ICoreCmsBillDeliveryServices _billDeliveryServices;
        private readonly ICoreCmsStoreServices _storeServices;
        private readonly ICoreCmsLogisticsServices _logisticsServices;
        private readonly ICoreCmsPaymentsServices _paymentsServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsUserWeChatInfoServices _userWeChatInfoServices;
        private readonly IRedisOperationRepository _redisOperationRepository;
        private readonly WeChat.Service.HttpClients.IWeChatApiHttpClientFactory _weChatApiHttpClientFactory;


        private readonly ICoreCmsOrderItemServices _orderItemServices;



        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsOrderController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsOrderServices coreCmsOrderServices
            , ICoreCmsUserServices userServices
            , ICoreCmsAreaServices areaServices
            , ICoreCmsBillAftersalesServices aftersalesServices
            , ICoreCmsStoreServices storeServices
            , ICoreCmsLogisticsServices logisticsServices
            , ICoreCmsBillPaymentsServices billPaymentsServices
            , ICoreCmsPaymentsServices paymentsServices
            , ICoreCmsSettingServices settingServices, ICoreCmsUserWeChatInfoServices userWeChatInfoServices, IRedisOperationRepository redisOperationRepository, ICoreCmsBillDeliveryServices billDeliveryServices, IWeChatApiHttpClientFactory weChatApiHttpClientFactory, ICoreCmsOrderItemServices orderItemServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsOrderServices = coreCmsOrderServices;
            _userServices = userServices;
            _areaServices = areaServices;
            _aftersalesServices = aftersalesServices;
            _storeServices = storeServices;
            _logisticsServices = logisticsServices;
            _billPaymentsServices = billPaymentsServices;
            _paymentsServices = paymentsServices;
            _settingServices = settingServices;
            _userWeChatInfoServices = userWeChatInfoServices;
            _redisOperationRepository = redisOperationRepository;
            _billDeliveryServices = billDeliveryServices;
            _weChatApiHttpClientFactory = weChatApiHttpClientFactory;
            _orderItemServices = orderItemServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsOrder/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsOrder>();
            //获取排序字段

            //订单号 nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId))
            {
                where = where.And(p => p.orderId.Contains(orderId));
            }

            //订单状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0)
            {
                where = where.And(p => p.status == status);
            }
            //订单类型 int
            var orderType = Request.Form["orderType"].FirstOrDefault().ObjectToInt(0);
            if (orderType > 0)
            {
                where = where.And(p => p.orderType == orderType);
            }
            //发货状态 int
            var shipStatus = Request.Form["shipStatus"].FirstOrDefault().ObjectToInt(0);
            if (shipStatus > 0)
            {
                where = where.And(p => p.shipStatus == shipStatus);
            }
            //支付状态 int
            var payStatus = Request.Form["payStatus"].FirstOrDefault().ObjectToInt(0);
            if (payStatus > 0)
            {
                where = where.And(p => p.payStatus == payStatus);
            }
            //支付方式代码 nvarchar
            var paymentCode = Request.Form["paymentCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentCode))
            {
                where = where.And(p => p.paymentCode.Contains(paymentCode));
            }
            //售后状态 int
            var confirmStatus = Request.Form["confirmStatus"].FirstOrDefault().ObjectToInt(0);
            if (confirmStatus > 0)
            {
                where = where.And(p => p.confirmStatus == confirmStatus);
            }
            //订单来源 int
            var source = Request.Form["source"].FirstOrDefault().ObjectToInt(0);
            if (source > 0)
            {
                where = where.And(p => p.source == source);
            }
            //收货方式 int
            var receiptType = Request.Form["receiptType"].FirstOrDefault().ObjectToInt(0);
            if (receiptType > 0)
            {
                where = where.And(p => p.receiptType == receiptType);
            }

            //收货人姓名 nvarchar
            var shipName = Request.Form["shipName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipName))
            {
                where = where.And(p => p.shipName.Contains(shipName));
            }
            //收货人地址 nvarchar
            var shipAddress = Request.Form["shipAddress"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipAddress))
            {
                where = where.And(p => p.shipAddress.Contains(shipAddress));
            }

            //收货电话 nvarchar
            var shipMobile = Request.Form["shipMobile"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipMobile))
            {
                where = where.And(p => p.shipMobile.Contains(shipMobile));
            }

            //付款单号 nvarchar
            var paymentId = Request.Form["paymentId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentId))
            {
                where = where.And(p => p.shipMobile.Contains(paymentId));
            }

            // datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                if (createTime.Contains("到"))
                {
                    var dts = createTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.createTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.createTime < dtEnd);
                }
                else
                {
                    var dt = createTime.ObjectToDate();
                    where = where.And(p => p.createTime > dt);
                }
            }


            //订单状态 int
            var orderUnifiedStatus = Request.Form["orderUnifiedStatus"].FirstOrDefault().ObjectToInt(0);
            if (orderUnifiedStatus > 0)
            {
                if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.payment)
                {
                    //待支付
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_PAYMENT));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.delivered)
                {
                    //待发货
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_DELIVERY));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.receive)
                {
                    //待收货
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_RECEIPT));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.evaluated)
                {
                    //已评价
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_COMPLETED_EVALUATE));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.noevaluat)
                {
                    //待评价
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_EVALUATE));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.complete)
                {
                    //已完成
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_COMPLETED));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.cancel)
                {
                    //已取消
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_CANCEL));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.delete)
                {
                    //已取消
                    where = where.And(p => p.isdel == true);
                }
            }
            else
            {
                where = where.And(p => p.isdel == false);
            }

            //获取数据
            var list = await _coreCmsOrderServices.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, pageCurrent, pageSize);
            if (list != null && list.Any())
            {
                var areaCache = await _areaServices.GetCaChe();
                foreach (var item in list)
                {
                    item.operating = _coreCmsOrderServices.GetOperating(item.orderId, item.status, item.payStatus, item.shipStatus, item.receiptType, item.isdel);
                    item.afterSaleStatus = "";
                    if (item.aftersalesItem != null && item.aftersalesItem.Any())
                    {
                        foreach (var sale in item.aftersalesItem)
                        {
                            item.afterSaleStatus += EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillAftersalesStatus>(sale.status) + "<br>";
                        }
                    }
                    var areas = await _areaServices.GetAreaFullName(item.shipAreaId, areaCache);
                    item.shipAreaName = areas.status ? areas.data + "-" + item.shipAddress : item.shipAddress;
                }
            }

            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsOrder/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public async Task<AdminUiCallBack> GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            //全部
            var all = await _coreCmsOrderServices.GetCountAsync(p => p.isdel == false);
            //待支付
            var paymentWhere = _coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_PAYMENT);
            var payment = await _coreCmsOrderServices.GetCountAsync(paymentWhere);
            //待发货
            var deliveredWhere = _coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_DELIVERY);
            var delivered = await _coreCmsOrderServices.GetCountAsync(deliveredWhere);
            //待收货
            var receiveWhere = _coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_RECEIPT);
            var receive = await _coreCmsOrderServices.GetCountAsync(receiveWhere);
            //已评价
            var evaluatedWhere = _coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_COMPLETED_EVALUATE);
            var evaluated = await _coreCmsOrderServices.GetCountAsync(evaluatedWhere);
            //待评价
            var noevaluatWhere = _coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_EVALUATE);
            var noevaluat = await _coreCmsOrderServices.GetCountAsync(noevaluatWhere);
            //已完成
            var completeWhere = _coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_COMPLETED);
            var complete = await _coreCmsOrderServices.GetCountAsync(completeWhere);
            //已取消
            var cancelWhere = _coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_CANCEL);
            var cancel = await _coreCmsOrderServices.GetCountAsync(cancelWhere);
            //删除
            var delete = await _coreCmsOrderServices.GetCountAsync(p => p.isdel == true);


            //订单状态说明
            var orderStatus = EnumHelper.EnumToList<GlobalEnumVars.OrderStatus>();
            //付款状态
            var payStatus = EnumHelper.EnumToList<GlobalEnumVars.OrderPayStatus>();
            //发货状态
            var shipStatus = EnumHelper.EnumToList<GlobalEnumVars.OrderShipStatus>();
            //订单来源
            var source = EnumHelper.EnumToList<GlobalEnumVars.Source>();
            //订单类型
            var orderType = EnumHelper.EnumToList<GlobalEnumVars.OrderType>();
            //订单支付方式
            var paymentCode = EnumHelper.EnumToList<GlobalEnumVars.PaymentsTypes>();
            //收货状态
            var confirmStatus = EnumHelper.EnumToList<GlobalEnumVars.OrderConfirmStatus>();
            //订单收货方式
            var receiptType = EnumHelper.EnumToList<GlobalEnumVars.OrderReceiptType>();

            jm.data = new
            {
                all,
                payment,
                delivered,
                receive,
                evaluated,
                noevaluat,
                complete,
                cancel,
                delete,
                orderStatus,
                payStatus,
                shipStatus,
                orderType,
                source,
                paymentCode,
                confirmStatus,
                receiptType
            };

            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsOrder/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var storeList = await _storeServices.QueryAsync();
            var result = await _coreCmsOrderServices.GetOrderInfoByOrderId(entity.id);
            if (!result.status)
            {
                jm.msg = result.msg;
                return jm;
            }
            jm.code = 0;
            jm.data = new
            {
                orderModel = result.data,
                storeList
            };

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Admins/CoreCmsOrder/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] AdminEditOrderPost entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsOrderServices.QueryByIdAsync(entity.orderId);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            if (entity.editType == 1)
            {
                oldModel.shipName = entity.shipName;
                oldModel.shipMobile = entity.shipMobile;
                oldModel.shipAreaId = entity.shipAreaId;
                oldModel.shipAddress = entity.shipAddress;
            }
            else if (entity.editType == 2)
            {
                oldModel.storeId = entity.storeId;
                oldModel.shipName = entity.shipName;
                oldModel.shipMobile = entity.shipMobile;
            }

            if (oldModel.orderAmount != entity.orderAmount && entity.orderAmount > 0)
            {
                oldModel.orderAmount = entity.orderAmount;
            }
            //事物处理过程结束
            var bl = await _coreCmsOrderServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 发货============================================================
        // POST: Api/CoreCmsOrder/GetShip
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("发货")]
        public async Task<AdminUiCallBack> GetShip([FromBody] FMArrayStringIds entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.id.Length == 0)
            {
                jm.msg = "请选择需要发货的数据";
                return jm;
            }



            var storeList = await _storeServices.QueryAsync();

            var logistics = await _logisticsServices.QueryListByClauseAsync(p => p.isDelete == false);

            var result = await _coreCmsOrderServices.GetOrderShipInfo(entity.id);
            if (!result.status)
            {
                jm.msg = result.msg;
                return jm;
            }

            if (storeList.Any())
            {
                foreach (var store in storeList)
                {
                    var getfullName = await _areaServices.GetAreaFullName(store.areaId);
                    if (getfullName.status)
                    {
                        store.allAddress = getfullName.data + store.address;
                    }
                }
            }
            jm.code = 0;
            jm.msg = result.msg;
            jm.data = new
            {
                orderModel = result.data,
                storeList,
                logistics,
            };

            return jm;
        }
        #endregion

        #region 发货提交============================================================
        // POST: Admins/CoreCmsOrder/Edit
        /// <summary>
        /// 发货提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("发货提交")]
        public async Task<AdminUiCallBack> DoShip([FromBody] AdminOrderShipPost entity)
        {
            var jm = new AdminUiCallBack();

            WebApiCallBack result;
            if (entity.orderId.Contains(","))
            {
                var ids = entity.orderId.Split(",");
                result = await _coreCmsOrderServices.BatchShip(ids, entity.logiCode, entity.logiNo, entity.items, entity.shipName, entity.shipMobile, entity.shipAddress, entity.memo, entity.storeId, entity.shipAreaId, entity.deliveryCompanyId);
            }
            else
            {
                result = await _coreCmsOrderServices.Ship(entity.orderId, entity.logiCode, entity.logiNo, entity.items, entity.shipName, entity.shipMobile, entity.shipAddress, entity.memo, entity.storeId, entity.shipAreaId, entity.deliveryCompanyId);
            }

            jm.code = result.status ? 0 : 1;
            jm.msg = result.msg;
            jm.data = result.data;
            jm.otherData = entity;

            return jm;
        }
        #endregion

        #region 秒发货============================================================
        // POST: Admins/CoreCmsOrder/Edit
        /// <summary>
        /// 秒发货
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("秒发货")]
        public async Task<AdminUiCallBack> DoSecondsShip([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var order = await _coreCmsOrderServices.QueryByIdAsync(entity.id);
            if (order == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var goodItems = await _orderItemServices.QueryListByClauseAsync(p => p.orderId == entity.id);
            if (!goodItems.Any())
            {
                jm.msg = "明细获取失败";
                return jm;
            }

            Dictionary<int, int> items = new Dictionary<int, int>();

            goodItems.ForEach(p =>
            {
                items.Add(p.productId, p.nums);
            });

            var result = new WebApiCallBack();

            if (order.receiptType == (int)GlobalEnumVars.OrderReceiptType.SelfDelivery)
            {
                result = await _coreCmsOrderServices.Ship(order.orderId, "shangmenziti", "无", items, order.shipName, order.shipMobile, order.shipAddress, order.memo, order.storeId, order.shipAreaId, "OTHERS");
            }
            else if (order.receiptType == (int)GlobalEnumVars.OrderReceiptType.IntraCityService)
            {
                result = await _coreCmsOrderServices.Ship(order.orderId, "benditongcheng", "无", items, order.shipName, order.shipMobile, order.shipAddress, order.memo, order.storeId, order.shipAreaId, "OTHERS");
            }

            jm.code = result.status ? 0 : 1;
            jm.msg = result.msg;
            jm.data = result.data;
            jm.otherData = entity;

            return jm;
        }
        #endregion

        #region 支付============================================================
        // POST: Api/CoreCmsOrder/GetPay
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("支付")]
        public async Task<AdminUiCallBack> GetPay([FromBody] FMArrayStringIds entity)
        {
            var jm = new AdminUiCallBack();

            var type = entity.data.ObjectToInt();
            if (type == 0 || entity.id.Length == 0)
            {
                jm.msg = "请提交合法的数据";
                return jm;
            }

            var result = await _billPaymentsServices.BatchFormatPaymentRel(entity.id, type, null);
            if (result.status == false)
            {
                jm.msg = result.msg;
                jm.data = result.data;
                return jm;
            }
            //取支付方式
            var payments = await _paymentsServices.QueryListByClauseAsync(p => p.isEnable, p => p.sort, OrderByType.Asc);
            jm.code = 0;
            jm.msg = "获取数据成功";
            jm.data = new
            {
                orderId = entity.id,
                type = entity.data,
                payments,
                rel = result.data
            };

            return jm;
        }
        #endregion

        #region 提交支付============================================================
        // POST: Admins/CoreCmsOrder/DoToPay
        /// <summary>
        /// 提交支付
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("提交支付")]
        public async Task<AdminUiCallBack> DoToPay([FromBody] AdminOrderDoPayPost entity)
        {
            var jm = new AdminUiCallBack();

            //事物处理过程结束
            var ids = entity.orderId.Split(",");
            var result = await _billPaymentsServices.ToPay(entity.orderId, entity.type, entity.paymentCode);

            jm.code = result.status ? 0 : 1;
            jm.msg = result.msg;

            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsOrder/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsOrderServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            //假删除
            var bl = await _coreCmsOrderServices.UpdateAsync(p => new CoreCmsOrder() { isdel = true }, p => p.orderId == model.orderId);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }
        #endregion

        #region 还原订单============================================================
        // POST: Api/CoreCmsOrder/DoRestore/10
        /// <summary>
        /// 还原订单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("还原订单")]
        public async Task<AdminUiCallBack> DoRestore([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsOrderServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            //还原
            var bl = await _coreCmsOrderServices.UpdateAsync(p => new CoreCmsOrder() { isdel = false }, p => p.orderId == model.orderId);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            return jm;

        }
        #endregion

        #region 判断是否存在售后============================================================
        // POST: Api/CoreCmsOrder/GetDoHaveAfterSale/10
        /// <summary>
        /// 判断是否存在售后
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("判断是否存在售后")]
        public async Task<AdminUiCallBack> GetDoHaveAfterSale([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            //等待售后审核的订单，不自动操作完成。
            var billAftersalesCount = await _aftersalesServices.GetCountAsync(p => p.orderId == entity.id && p.status == (int)GlobalEnumVars.BillAftersalesStatus.WaitAudit);

            bool bl = billAftersalesCount > 0;

            jm.code = bl ? 0 : 1;
            jm.msg = "存在未处理的售后";

            return jm;

        }
        #endregion

        #region 完成订单============================================================
        // POST: Api/CoreCmsOrder/DoComplete/10
        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("完成订单")]
        public async Task<AdminUiCallBack> DoComplete([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var result = await _coreCmsOrderServices.CompleteOrder(entity.id);
            jm.code = result.status ? 0 : 1;
            jm.msg = result.msg;
            jm.data = result.data;
            jm.otherData = result.otherData;

            return jm;

        }
        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsOrder/GetDetails/10
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var result = await _coreCmsOrderServices.GetOrderInfoByOrderId(entity.id);
            if (result == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = result.status ? 0 : 1;
            jm.data = result.data;

            return jm;
        }
        #endregion

        #region 订单打印============================================================
        // POST: Api/CoreCmsOrder/GetPrintTpl/10
        /// <summary>
        /// 订单打印
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("订单打印")]
        public async Task<AdminUiCallBack> GetPrintTpl([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var result = await _coreCmsOrderServices.GetOrderInfoByOrderId(entity.id);
            if (result == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = result.status ? 0 : 1;


            var allConfigs = await _settingServices.GetConfigDictionaries();

            var shopName = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopName);
            var shopMobile = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopMobile);

            jm.data = new
            {
                order = result.data,
                shopName,
                shopMobile
            };

            return jm;
        }
        #endregion

        #region 选择导出============================================================
        // POST: Api/CoreCmsOrder/SelectExportExcel/10
        /// <summary>
        /// 选择导出
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("选择导出")]
        public async Task<AdminUiCallBack> SelectExportExcel([FromBody] FMArrayStringIds entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.id.Length == 0)
            {
                jm.msg = "请选择要导出的数据";
                return jm;
            }


            //获取数据
            var list = await _coreCmsOrderServices.QueryListAsync(p => entity.id.Contains(p.orderId), p => p.createTime, OrderByType.Desc);
            if (list != null && list.Any())
            {
                var areaCache = await _areaServices.GetCaChe();
                foreach (var item in list)
                {
                    //item.operating = _coreCmsOrderServices.GetOperating(item.orderId, item.status, item.payStatus, item.shipStatus, item.isdel);
                    //item.afterSaleStatus = "";
                    //if (item.aftersalesItem != null && item.aftersalesItem.Any())
                    //{
                    //    foreach (var sale in item.aftersalesItem)
                    //    {
                    //        item.afterSaleStatus += EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillAftersalesStatus>(sale.status) + "<br>";
                    //    }
                    //}
                    var areas = await _areaServices.GetAreaFullName(item.shipAreaId, areaCache);
                    item.shipAreaName = areas.status ? areas.data + "-" + item.shipAddress : item.shipAddress;
                }
            }


            //订单状态说明
            var orderStatusEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderStatusDescription>();
            //付款状态
            var orderPayStatusEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderPayStatus>();
            //发货状态
            var shipStatusEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderShipStatus>();
            //订单来源
            var sourceEntities = EnumHelper.EnumToList<GlobalEnumVars.Source>();
            //订单类型
            var orderTypeEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderType>();
            //订单支付方式
            var paymentsTypesEntities = EnumHelper.EnumToList<GlobalEnumVars.PaymentsTypes>();
            //收货状态
            var orderConfirmStatusEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderConfirmStatus>();
            //订单收货方式
            var orderReceiptTypeEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderReceiptType>();


            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");

            //获取list数据
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);

            var items = new List<CellValueItem>();
            items.Add(new CellValueItem() { name = "序号", width = 10 });
            items.Add(new CellValueItem() { name = "订单号", width = 20 });
            items.Add(new CellValueItem() { name = "商品总价", width = 12 });
            items.Add(new CellValueItem() { name = "支付金额", width = 12 });
            items.Add(new CellValueItem() { name = "订单总额", width = 12 });
            items.Add(new CellValueItem() { name = "支付状态", width = 12 });
            items.Add(new CellValueItem() { name = "发货状态", width = 12 });
            items.Add(new CellValueItem() { name = "订单状态", width = 12 });
            items.Add(new CellValueItem() { name = "订单类型", width = 12 });
            items.Add(new CellValueItem() { name = "支付方式", width = 12 });
            items.Add(new CellValueItem() { name = "支付时间", width = 20 });

            items.Add(new CellValueItem() { name = "货品名称", width = 40 });
            items.Add(new CellValueItem() { name = "数量", width = 12 });
            items.Add(new CellValueItem() { name = "单价", width = 12 });
            items.Add(new CellValueItem() { name = "优惠", width = 12 });
            items.Add(new CellValueItem() { name = "合计", width = 12 });


            items.Add(new CellValueItem() { name = "收货人姓名", width = 12 });
            items.Add(new CellValueItem() { name = "收货电话", width = 12 });
            items.Add(new CellValueItem() { name = "收货详细地址", width = 40 });


            items.Add(new CellValueItem() { name = "配送方式名称", width = 20 });
            items.Add(new CellValueItem() { name = "配送费用", width = 12 });
            items.Add(new CellValueItem() { name = "用户ID", width = 12 });

            items.Add(new CellValueItem() { name = "是否收货", width = 20 });
            items.Add(new CellValueItem() { name = "确认收货时间", width = 20 });


            items.Add(new CellValueItem() { name = "商品总重量", width = 20 });
            items.Add(new CellValueItem() { name = "是否开发票", width = 20 });
            items.Add(new CellValueItem() { name = "税号", width = 20 });
            items.Add(new CellValueItem() { name = "发票抬头", width = 20 });
            items.Add(new CellValueItem() { name = "使用积分", width = 20 });
            items.Add(new CellValueItem() { name = "积分抵扣金额", width = 20 });
            items.Add(new CellValueItem() { name = "订单优惠金额", width = 20 });
            items.Add(new CellValueItem() { name = "商品优惠金额", width = 20 });
            items.Add(new CellValueItem() { name = "优惠券优惠额度", width = 20 });
            items.Add(new CellValueItem() { name = "优惠券信息", width = 20 });
            items.Add(new CellValueItem() { name = "优惠信息", width = 20 });
            items.Add(new CellValueItem() { name = "买家备注", width = 20 });
            items.Add(new CellValueItem() { name = "下单IP", width = 20 });
            items.Add(new CellValueItem() { name = "卖家备注", width = 20 });
            items.Add(new CellValueItem() { name = "订单来源", width = 20 });
            items.Add(new CellValueItem() { name = "是否评论", width = 20 });
            items.Add(new CellValueItem() { name = "删除标志", width = 20 });
            items.Add(new CellValueItem() { name = "订单时间", width = 20 });
            items.Add(new CellValueItem() { name = "更新时间", width = 20 });

            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            for (int i = 0; i < items.Count; i++)
            {
                var cell = row1.CreateCell(i);
                cell.SetCellValue(items[i].name);
                cell.CellStyle = headerStyle;
                sheet1.SetColumnWidth(i, items[i].width * 256);
            }

            row1.Height = 30 * 20;

            var commonCellStyle = ExcelHelper.GetCommonStyle(book);


            var detailsStartNumber = 0;
            var listStartNumber = 0;
            foreach (var order in list)
            {
                listStartNumber++;
                //当前开始行
                var nowNumber = detailsStartNumber;
                //将数据逐步写入sheet1各个行
                foreach (var t in order.items)
                {
                    var rowTemp = sheet1.CreateRow(detailsStartNumber + 1);

                    rowTemp.CreateCell(0).SetCellValue(listStartNumber);
                    rowTemp.CreateCell(1).SetCellValue(order.orderId);
                    rowTemp.CreateCell(2).SetCellValue(order.goodsAmount.ToString());
                    rowTemp.CreateCell(3).SetCellValue(order.payedAmount.ToString());
                    rowTemp.CreateCell(4).SetCellValue(order.orderAmount.ToString());

                    var payModel = orderPayStatusEntities.Find(p => p.value == order.payStatus);
                    rowTemp.CreateCell(5).SetCellValue(payModel != null ? payModel.description : "");

                    var shipStatusModel = shipStatusEntities.Find(p => p.value == order.shipStatus);
                    rowTemp.CreateCell(6).SetCellValue(shipStatusModel != null ? shipStatusModel.description : "");

                    var statusModel = orderStatusEntities.Find(p => p.value == order.status);
                    rowTemp.CreateCell(7).SetCellValue(statusModel != null ? statusModel.description : "");

                    var orderTypeModel = orderTypeEntities.Find(p => p.value == order.orderType);
                    rowTemp.CreateCell(8).SetCellValue(orderTypeModel != null ? orderTypeModel.description : "");

                    var paymentCodeModel = paymentsTypesEntities.Find(p => p.title == order.paymentCode);
                    rowTemp.CreateCell(9).SetCellValue(paymentCodeModel != null ? paymentCodeModel.description : "");

                    rowTemp.CreateCell(10).SetCellValue(order.paymentTime.ToString());


                    rowTemp.CreateCell(11).SetCellValue(!string.IsNullOrEmpty(t.addon) ? t.addon : t.name);
                    rowTemp.CreateCell(12).SetCellValue(t.nums);
                    rowTemp.CreateCell(13).SetCellValue(t.price + "元");
                    rowTemp.CreateCell(14).SetCellValue(t.promotionAmount + "元");
                    rowTemp.CreateCell(15).SetCellValue(t.amount + "元");


                    rowTemp.CreateCell(16).SetCellValue(order.shipName);
                    rowTemp.CreateCell(17).SetCellValue(order.shipMobile);
                    rowTemp.CreateCell(18).SetCellValue(order.shipAddress);

                    rowTemp.CreateCell(19).SetCellValue(!string.IsNullOrEmpty(order.logisticsName) ? order.logisticsName : "自提配送");
                    rowTemp.CreateCell(20).SetCellValue(order.costFreight.ToString());
                    rowTemp.CreateCell(21).SetCellValue(order.userId.ToString());

                    var confirmStatusModel = orderConfirmStatusEntities.Find(p => p.value == order.confirmStatus);
                    rowTemp.CreateCell(22).SetCellValue(confirmStatusModel != null ? confirmStatusModel.description : "");

                    rowTemp.CreateCell(23).SetCellValue(order.confirmTime.ToString());

                    rowTemp.CreateCell(24).SetCellValue(order.weight.ToString());
                    rowTemp.CreateCell(25).SetCellValue(order.taxType.ToString());
                    rowTemp.CreateCell(26).SetCellValue(order.taxCode);
                    rowTemp.CreateCell(27).SetCellValue(order.taxTitle);
                    rowTemp.CreateCell(28).SetCellValue(order.point.ToString());
                    rowTemp.CreateCell(29).SetCellValue(order.pointMoney.ToString());
                    rowTemp.CreateCell(30).SetCellValue(order.orderDiscountAmount.ToString());
                    rowTemp.CreateCell(31).SetCellValue(order.goodsDiscountAmount.ToString());
                    rowTemp.CreateCell(32).SetCellValue(order.couponDiscountAmount.ToString());
                    rowTemp.CreateCell(33).SetCellValue(order.coupon);
                    rowTemp.CreateCell(34).SetCellValue(order.promotionList);
                    rowTemp.CreateCell(35).SetCellValue(order.memo);
                    rowTemp.CreateCell(36).SetCellValue(order.ip);
                    rowTemp.CreateCell(37).SetCellValue(order.mark);
                    rowTemp.CreateCell(38).SetCellValue(order.source.ToString());
                    rowTemp.CreateCell(39).SetCellValue(order.isComment.ToString());
                    rowTemp.CreateCell(40).SetCellValue(order.isdel.ToString());
                    rowTemp.CreateCell(41).SetCellValue(order.createTime.ToString());
                    rowTemp.CreateCell(42).SetCellValue(order.updateTime.ToString());


                    rowTemp.Cells.ForEach(p =>
                    {
                        p.CellStyle = commonCellStyle;
                    });
                    rowTemp.Height = 20 * 20;


                    detailsStartNumber++;
                }
                //合并单元格（第几行，到第几行，第几列，到第几列）
                var marId = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42 };
                foreach (var id in marId)
                {
                    sheet1.AddMergedRegion(new CellRangeAddress(nowNumber + 1, detailsStartNumber, id, id));
                }

            }

            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-订单导出(选择结果).xls";
            string filePath = webRootPath + tpath;
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return jm;
        }
        #endregion

        #region 查询导出============================================================
        // POST: Api/CoreCmsOrder/QueryExportExcel/10
        /// <summary>
        /// 查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {

            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsOrder>();
            //获取排序字段

            //订单号 nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId))
            {
                where = where.And(p => p.orderId.Contains(orderId));
            }

            //订单状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0)
            {
                where = where.And(p => p.status == status);
            }
            //订单类型 int
            var orderType = Request.Form["orderType"].FirstOrDefault().ObjectToInt(0);
            if (orderType > 0)
            {
                where = where.And(p => p.orderType == orderType);
            }
            //发货状态 int
            var shipStatus = Request.Form["shipStatus"].FirstOrDefault().ObjectToInt(0);
            if (shipStatus > 0)
            {
                where = where.And(p => p.shipStatus == shipStatus);
            }
            //支付状态 int
            var payStatus = Request.Form["payStatus"].FirstOrDefault().ObjectToInt(0);
            if (payStatus > 0)
            {
                where = where.And(p => p.payStatus == payStatus);
            }
            //支付方式代码 nvarchar
            var paymentCode = Request.Form["paymentCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentCode))
            {
                where = where.And(p => p.paymentCode.Contains(paymentCode));
            }
            //售后状态 int
            var confirmStatus = Request.Form["confirmStatus"].FirstOrDefault().ObjectToInt(0);
            if (confirmStatus > 0)
            {
                where = where.And(p => p.confirmStatus == confirmStatus);
            }
            //订单来源 int
            var source = Request.Form["source"].FirstOrDefault().ObjectToInt(0);
            if (source > 0)
            {
                where = where.And(p => p.source == source);
            }
            //收货方式 int
            var receiptType = Request.Form["receiptType"].FirstOrDefault().ObjectToInt(0);
            if (receiptType > 0)
            {
                where = where.And(p => p.receiptType == receiptType);
            }

            //收货人姓名 nvarchar
            var shipName = Request.Form["shipName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipName))
            {
                where = where.And(p => p.shipName.Contains(shipName));
            }
            //收货人地址 nvarchar
            var shipAddress = Request.Form["shipAddress"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipAddress))
            {
                where = where.And(p => p.shipAddress.Contains(shipAddress));
            }

            //收货电话 nvarchar
            var shipMobile = Request.Form["shipMobile"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipMobile))
            {
                where = where.And(p => p.shipMobile.Contains(shipMobile));
            }

            //付款单号 nvarchar
            var paymentId = Request.Form["paymentId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentId))
            {
                where = where.And(p => p.shipMobile.Contains(paymentId));
            }

            // datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                if (createTime.Contains("到"))
                {
                    var dts = createTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.createTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.createTime < dtEnd);
                }
                else
                {
                    var dt = createTime.ObjectToDate();
                    where = where.And(p => p.createTime > dt);
                }
            }


            //订单状态 int
            var orderUnifiedStatus = Request.Form["orderUnifiedStatus"].FirstOrDefault().ObjectToInt(0);
            if (orderUnifiedStatus > 0)
            {
                if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.payment)
                {
                    //待支付
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_PAYMENT));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.delivered)
                {
                    //待发货
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_DELIVERY));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.receive)
                {
                    //待收货
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_RECEIPT));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.evaluated)
                {
                    //已评价
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_COMPLETED_EVALUATE));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.noevaluat)
                {
                    //待评价
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_EVALUATE));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.complete)
                {
                    //已完成
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_COMPLETED));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.cancel)
                {
                    //已取消
                    where = where.And(_coreCmsOrderServices.GetReverseStatus((int)GlobalEnumVars.OrderAllStatusType.ALL_CANCEL));
                }
                else if (orderUnifiedStatus == (int)GlobalEnumVars.OrderCountType.delete)
                {
                    //已取消
                    where = where.And(p => p.isdel == true);
                }
            }
            else
            {
                where = where.And(p => p.isdel == false);
            }

            //获取数据
            var list = await _coreCmsOrderServices.QueryListAsync(where, p => p.createTime, OrderByType.Desc);
            if (list != null && list.Any())
            {
                var areaCache = await _areaServices.GetCaChe();
                foreach (var item in list)
                {
                    //item.operating = _coreCmsOrderServices.GetOperating(item.orderId, item.status, item.payStatus, item.shipStatus, item.isdel);
                    //item.afterSaleStatus = "";
                    //if (item.aftersalesItem != null && item.aftersalesItem.Any())
                    //{
                    //    foreach (var sale in item.aftersalesItem)
                    //    {
                    //        item.afterSaleStatus += EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillAftersalesStatus>(sale.status) + "<br>";
                    //    }
                    //}
                    var areas = await _areaServices.GetAreaFullName(item.shipAreaId, areaCache);
                    item.shipAreaName = areas.status ? areas.data + "-" + item.shipAddress : item.shipAddress;
                }
            }


            //订单状态说明
            var orderStatusEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderStatusDescription>();
            //付款状态
            var orderPayStatusEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderPayStatus>();
            //发货状态
            var shipStatusEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderShipStatus>();
            //订单来源
            var sourceEntities = EnumHelper.EnumToList<GlobalEnumVars.Source>();
            //订单类型
            var orderTypeEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderType>();
            //订单支付方式
            var paymentsTypesEntities = EnumHelper.EnumToList<GlobalEnumVars.PaymentsTypes>();
            //收货状态
            var orderConfirmStatusEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderConfirmStatus>();
            //订单收货方式
            var orderReceiptTypeEntities = EnumHelper.EnumToList<GlobalEnumVars.OrderReceiptType>();


            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");

            //获取list数据
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);

            var items = new List<CellValueItem>();
            items.Add(new CellValueItem() { name = "序号", width = 10 });
            items.Add(new CellValueItem() { name = "订单号", width = 20 });
            items.Add(new CellValueItem() { name = "商品总价", width = 12 });
            items.Add(new CellValueItem() { name = "支付金额", width = 12 });
            items.Add(new CellValueItem() { name = "订单总额", width = 12 });
            items.Add(new CellValueItem() { name = "支付状态", width = 12 });
            items.Add(new CellValueItem() { name = "发货状态", width = 12 });
            items.Add(new CellValueItem() { name = "订单状态", width = 12 });
            items.Add(new CellValueItem() { name = "订单类型", width = 12 });
            items.Add(new CellValueItem() { name = "支付方式", width = 12 });
            items.Add(new CellValueItem() { name = "支付时间", width = 20 });

            items.Add(new CellValueItem() { name = "货品名称", width = 40 });
            items.Add(new CellValueItem() { name = "数量", width = 12 });
            items.Add(new CellValueItem() { name = "单价", width = 12 });
            items.Add(new CellValueItem() { name = "优惠", width = 12 });
            items.Add(new CellValueItem() { name = "合计", width = 12 });


            items.Add(new CellValueItem() { name = "收货人姓名", width = 12 });
            items.Add(new CellValueItem() { name = "收货电话", width = 12 });
            items.Add(new CellValueItem() { name = "收货详细地址", width = 40 });


            items.Add(new CellValueItem() { name = "配送方式名称", width = 20 });
            items.Add(new CellValueItem() { name = "配送费用", width = 12 });
            items.Add(new CellValueItem() { name = "用户ID", width = 12 });

            items.Add(new CellValueItem() { name = "是否收货", width = 20 });
            items.Add(new CellValueItem() { name = "确认收货时间", width = 20 });


            items.Add(new CellValueItem() { name = "商品总重量", width = 20 });
            items.Add(new CellValueItem() { name = "是否开发票", width = 20 });
            items.Add(new CellValueItem() { name = "税号", width = 20 });
            items.Add(new CellValueItem() { name = "发票抬头", width = 20 });
            items.Add(new CellValueItem() { name = "使用积分", width = 20 });
            items.Add(new CellValueItem() { name = "积分抵扣金额", width = 20 });
            items.Add(new CellValueItem() { name = "订单优惠金额", width = 20 });
            items.Add(new CellValueItem() { name = "商品优惠金额", width = 20 });
            items.Add(new CellValueItem() { name = "优惠券优惠额度", width = 20 });
            items.Add(new CellValueItem() { name = "优惠券信息", width = 20 });
            items.Add(new CellValueItem() { name = "优惠信息", width = 20 });
            items.Add(new CellValueItem() { name = "买家备注", width = 20 });
            items.Add(new CellValueItem() { name = "下单IP", width = 20 });
            items.Add(new CellValueItem() { name = "卖家备注", width = 20 });
            items.Add(new CellValueItem() { name = "订单来源", width = 20 });
            items.Add(new CellValueItem() { name = "是否评论", width = 20 });
            items.Add(new CellValueItem() { name = "删除标志", width = 20 });
            items.Add(new CellValueItem() { name = "订单时间", width = 20 });
            items.Add(new CellValueItem() { name = "更新时间", width = 20 });

            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            for (int i = 0; i < items.Count; i++)
            {
                var cell = row1.CreateCell(i);
                cell.SetCellValue(items[i].name);
                cell.CellStyle = headerStyle;
                sheet1.SetColumnWidth(i, items[i].width * 256);
            }

            row1.Height = 30 * 20;

            var commonCellStyle = ExcelHelper.GetCommonStyle(book);


            var detailsStartNumber = 0;
            var listStartNumber = 0;
            foreach (var order in list)
            {
                listStartNumber++;
                //当前开始行
                var nowNumber = detailsStartNumber;
                //将数据逐步写入sheet1各个行
                foreach (var t in order.items)
                {
                    var rowTemp = sheet1.CreateRow(detailsStartNumber + 1);

                    rowTemp.CreateCell(0).SetCellValue(listStartNumber);
                    rowTemp.CreateCell(1).SetCellValue(order.orderId);
                    rowTemp.CreateCell(2).SetCellValue(order.goodsAmount.ToString());
                    rowTemp.CreateCell(3).SetCellValue(order.payedAmount.ToString());
                    rowTemp.CreateCell(4).SetCellValue(order.orderAmount.ToString());

                    var payModel = orderPayStatusEntities.Find(p => p.value == order.payStatus);
                    rowTemp.CreateCell(5).SetCellValue(payModel != null ? payModel.description : "");

                    var shipStatusModel = shipStatusEntities.Find(p => p.value == order.shipStatus);
                    rowTemp.CreateCell(6).SetCellValue(shipStatusModel != null ? shipStatusModel.description : "");

                    var statusModel = orderStatusEntities.Find(p => p.value == order.status);
                    rowTemp.CreateCell(7).SetCellValue(statusModel != null ? statusModel.description : "");

                    var orderTypeModel = orderTypeEntities.Find(p => p.value == order.orderType);
                    rowTemp.CreateCell(8).SetCellValue(orderTypeModel != null ? orderTypeModel.description : "");

                    var paymentCodeModel = paymentsTypesEntities.Find(p => p.title == order.paymentCode);
                    rowTemp.CreateCell(9).SetCellValue(paymentCodeModel != null ? paymentCodeModel.description : "");

                    rowTemp.CreateCell(10).SetCellValue(order.paymentTime.ToString());


                    rowTemp.CreateCell(11).SetCellValue(!string.IsNullOrEmpty(t.addon) ? t.addon : t.name);
                    rowTemp.CreateCell(12).SetCellValue(t.nums);
                    rowTemp.CreateCell(13).SetCellValue(t.price + "元");
                    rowTemp.CreateCell(14).SetCellValue(t.promotionAmount + "元");
                    rowTemp.CreateCell(15).SetCellValue(t.amount + "元");


                    rowTemp.CreateCell(16).SetCellValue(order.shipName);
                    rowTemp.CreateCell(17).SetCellValue(order.shipMobile);
                    rowTemp.CreateCell(18).SetCellValue(order.shipAddress);

                    rowTemp.CreateCell(19).SetCellValue(!string.IsNullOrEmpty(order.logisticsName) ? order.logisticsName : "自提配送");
                    rowTemp.CreateCell(20).SetCellValue(order.costFreight.ToString());
                    rowTemp.CreateCell(21).SetCellValue(order.userId.ToString());

                    var confirmStatusModel = orderConfirmStatusEntities.Find(p => p.value == order.confirmStatus);
                    rowTemp.CreateCell(22).SetCellValue(confirmStatusModel != null ? confirmStatusModel.description : "");

                    rowTemp.CreateCell(23).SetCellValue(order.confirmTime.ToString());

                    rowTemp.CreateCell(24).SetCellValue(order.weight.ToString());
                    rowTemp.CreateCell(25).SetCellValue(order.taxType.ToString());
                    rowTemp.CreateCell(26).SetCellValue(order.taxCode);
                    rowTemp.CreateCell(27).SetCellValue(order.taxTitle);
                    rowTemp.CreateCell(28).SetCellValue(order.point.ToString());
                    rowTemp.CreateCell(29).SetCellValue(order.pointMoney.ToString());
                    rowTemp.CreateCell(30).SetCellValue(order.orderDiscountAmount.ToString());
                    rowTemp.CreateCell(31).SetCellValue(order.goodsDiscountAmount.ToString());
                    rowTemp.CreateCell(32).SetCellValue(order.couponDiscountAmount.ToString());
                    rowTemp.CreateCell(33).SetCellValue(order.coupon);
                    rowTemp.CreateCell(34).SetCellValue(order.promotionList);
                    rowTemp.CreateCell(35).SetCellValue(order.memo);
                    rowTemp.CreateCell(36).SetCellValue(order.ip);
                    rowTemp.CreateCell(37).SetCellValue(order.mark);
                    rowTemp.CreateCell(38).SetCellValue(order.source.ToString());
                    rowTemp.CreateCell(39).SetCellValue(order.isComment.ToString());
                    rowTemp.CreateCell(40).SetCellValue(order.isdel.ToString());
                    rowTemp.CreateCell(41).SetCellValue(order.createTime.ToString());
                    rowTemp.CreateCell(42).SetCellValue(order.updateTime.ToString());


                    rowTemp.Cells.ForEach(p =>
                    {
                        p.CellStyle = commonCellStyle;
                    });
                    rowTemp.Height = 20 * 20;


                    detailsStartNumber++;
                }
                //合并单元格（第几行，到第几行，第几列，到第几列）
                var marId = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42 };
                foreach (var id in marId)
                {
                    sheet1.AddMergedRegion(new CellRangeAddress(nowNumber + 1, detailsStartNumber, id, id));
                }

            }

            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-订单导出(查询结果).xls";
            string filePath = webRootPath + tpath;
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return jm;
        }
        #endregion

        #region 设置是否开发票============================================================
        // POST: Api/CoreCmsOrder/DoSettaxType/10
        /// <summary>
        /// 设置是否开发票
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否开发票")]
        public async Task<AdminUiCallBack> DoSettaxType([FromBody] FMUpdateIntegerDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsOrderServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.taxType = entity.data;

            var bl = await _coreCmsOrderServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 设置是否评论============================================================
        // POST: Api/CoreCmsOrder/DoSetisComment/10
        /// <summary>
        /// 设置是否评论
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否评论")]
        public async Task<AdminUiCallBack> DoSetisComment([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsOrderServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isComment = (bool)entity.data;

            var bl = await _coreCmsOrderServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 更新备注============================================================
        // POST: Api/CoreCmsOrder/DoSetisdel/10
        /// <summary>
        /// 更新备注
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("更新备注")]
        public async Task<AdminUiCallBack> DoUpdateMark([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsOrderServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.mark = entity.data.ToString();
            var bl = await _coreCmsOrderServices.UpdateAsync(p => new CoreCmsOrder() { mark = oldModel.mark }, p => p.orderId == oldModel.orderId);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 取消订单============================================================
        // POST: Api/CoreCmsOrder/CancelOrder/10
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("取消订单")]
        public async Task<AdminUiCallBack> CancelOrder([FromBody] FMArrayStringIds entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.id.Length == 0)
            {
                jm.msg = "请提交要取消的订单号";
                return jm;
            }

            var result = await _coreCmsOrderServices.CancelOrder(entity.id);
            jm.code = result.status ? 0 : 1;
            jm.msg = result.msg;

            return jm;
        }
        #endregion


        #region 批量删除订单============================================================
        // POST: Api/CoreCmsOrder/DeleteOrder/10
        /// <summary>
        /// 批量删除订单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除订单")]
        public async Task<AdminUiCallBack> DeleteOrder([FromBody] FMArrayStringIds entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.id.Length == 0)
            {
                jm.msg = "请提交要批量删除的订单号";
                return jm;
            }

            var result = await _coreCmsOrderServices.UpdateAsync(p => new CoreCmsOrder() { isdel = true }, p => entity.id.Contains(p.orderId));
            jm.code = result ? 0 : 1;
            jm.msg = result ? "删除成功" : "删除失败";

            return jm;
        }
        #endregion

        #region 重新同步发货============================================================
        // POST: Api/CoreCmsOrder/DeleteOrder/10
        /// <summary>
        /// 批量删除订单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除订单")]
        public async Task<AdminUiCallBack> RefreshDelivery([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(entity.id))
            {
                jm.msg = "请提交要取消的订单号";
                return jm;
            }

            var delivery = await _billDeliveryServices.QueryByClauseAsync(p => p.deliveryId == entity.id);
            if (delivery == null)
            {
                jm.msg = "发货单获取失败";
                return jm;
            }

            jm.code = 0;
            jm.msg = "提交任务成功,请核实远端状态";

            return jm;
        }
        #endregion


        #region 预览快递进度============================================================
        // POST: Api/CoreCmsOrder/GetDetails/10
        /// <summary>
        /// 预览快递进度
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览快递进度")]
        public async Task<AdminUiCallBack> GetOrderLogistics([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var result = await _coreCmsOrderServices.GetOrderInfoByOrderId(entity.id);
            if (result == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = result.status ? 0 : 1;
            jm.data = result.data;

            return jm;
        }
        #endregion
    }
}
