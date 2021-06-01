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
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.View;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
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
    [Authorize]
    public class CoreCmsOrderController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsOrderServices _coreCmsOrderServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsAreaServices _areaServices;
        private readonly ICoreCmsBillAftersalesServices _aftersalesServices;
        private readonly ICoreCmsBillPaymentsServices _billPaymentsServices;
        private readonly ICoreCmsBillPaymentsRelServices _billPaymentsRelServices;
        private readonly ICoreCmsStoreServices _storeServices;
        private readonly ICoreCmsLogisticsServices _logisticsServices;
        private readonly ICoreCmsPaymentsServices _paymentsServices;
        private readonly ICoreCmsSettingServices _settingServices;

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
            , ICoreCmsSettingServices settingServices
            , ICoreCmsBillPaymentsRelServices billPaymentsRelServices
            )
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
            _billPaymentsRelServices = billPaymentsRelServices;

        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsOrder/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = ObjectExtensions.ObjectToInt(Request.Form["page"].FirstOrDefault(), 1);
            var pageSize = ObjectExtensions.ObjectToInt(Request.Form["limit"].FirstOrDefault(), 30);
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
                    var dtStart = ObjectExtensions.ObjectToDate(dts[0].Trim());
                    where = where.And(p => p.createTime > dtStart);
                    var dtEnd = ObjectExtensions.ObjectToDate(dts[1].Trim());
                    where = where.And(p => p.createTime < dtEnd);
                }
                else
                {
                    var dt = ObjectExtensions.ObjectToDate(createTime);
                    where = where.And(p => p.createTime > dt);
                }
            }


            //订单状态 int
            var orderUnifiedStatus = ObjectExtensions.ObjectToInt(Request.Form["orderUnifiedStatus"].FirstOrDefault(), 0);
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
                    item.operating = _coreCmsOrderServices.GetOperating(item.orderId, item.status, item.payStatus, item.shipStatus);
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
            return new JsonResult(jm);
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
        public async Task<JsonResult> GetIndex()
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
                orderStatus,
                payStatus,
                shipStatus,
                orderType,
                source,
                paymentCode,
                confirmStatus,
                receiptType
            };

            return new JsonResult(jm);
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
        public async Task<JsonResult> GetEdit([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var storeList = await _storeServices.QueryAsync();
            var result = await _coreCmsOrderServices.GetOrderInfoByOrderId(entity.id);
            if (!result.status)
            {
                jm.msg = result.msg;
                return new JsonResult(jm);
            }
            jm.code = 0;
            jm.data = new
            {
                orderModel = result.data,
                storeList
            };

            return new JsonResult(jm);
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
        public async Task<JsonResult> DoEdit([FromBody] AdminEditOrderPost entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsOrderServices.QueryByIdAsync(entity.orderId);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
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

            return new JsonResult(jm);
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
        public async Task<JsonResult> GetShip([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var storeList = await _storeServices.QueryAsync();

            var logistics = await _logisticsServices.QueryListByClauseAsync(p => p.isDelete == false);
            var ids = entity.id.Split(",");
            var result = await _coreCmsOrderServices.GetOrderShipInfo(ids);
            if (!result.status)
            {
                jm.msg = result.msg;
                return new JsonResult(jm);
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
                logistics
            };

            return new JsonResult(jm);
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
        public async Task<JsonResult> DoShip([FromBody] AdminOrderShipPost entity)
        {
            var jm = new AdminUiCallBack();

            var ids = entity.orderId.Split(",");

            var result = await _coreCmsOrderServices.OrderShip(ids, entity.logiCode, entity.logiNo, entity.items, entity.shipName, entity.shipMobile, entity.shipAddress, entity.memo, entity.storeId, entity.shipAreaId);

            jm.code = result.status ? 0 : 1;
            jm.msg = result.msg;
            jm.data = result.data;
            jm.otherData = entity;

            return new JsonResult(jm);
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
        public async Task<JsonResult> GetPay([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var ids = entity.id.Split(",");
            var type = entity.data.ObjectToInt();
            if (type == 0 || ids.Length == 0)
            {
                jm.msg = "请提交合法的数据";
                return new JsonResult(jm);
            }

            var result = _billPaymentsServices.FormatPaymentRel(ids, type, null);
            if (result.status == false)
            {
                jm.msg = result.msg;
                jm.data = result.data;
                return new JsonResult(jm);
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

            return new JsonResult(jm);
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
        public async Task<JsonResult> DoToPay([FromBody] AdminOrderDoPayPost entity)
        {
            var jm = new AdminUiCallBack();

            //事物处理过程结束
            var ids = entity.orderId.Split(",");
            var result = await _billPaymentsServices.ToPay(entity.orderId, entity.type, entity.paymentCode);

            jm.code = result.status ? 0 : 1;
            jm.msg = result.msg;

            return new JsonResult(jm);
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
        public async Task<JsonResult> DoDelete([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsOrderServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }
            //假删除
            var bl = await _coreCmsOrderServices.UpdateAsync(p => new CoreCmsOrder() { isdel = true }, p => p.orderId == model.orderId);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return new JsonResult(jm);

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
        public async Task<JsonResult> GetDoHaveAfterSale([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            //等待售后审核的订单，不自动操作完成。
            var billAftersalesCount = await _aftersalesServices.GetCountAsync(p => p.orderId == entity.id && p.status == (int)GlobalEnumVars.BillAftersalesStatus.WaitAudit);

            bool bl = billAftersalesCount > 0;

            jm.code = bl ? 0 : 1;
            jm.msg = "存在未处理的售后";

            return new JsonResult(jm);

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
        public async Task<JsonResult> DoComplete([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var result = await _coreCmsOrderServices.CompleteOrder(entity.id);
            jm.code = result.status ? 0 : 1;
            jm.msg = result.msg;
            jm.data = result.data;
            jm.otherData = result.otherData;

            return new JsonResult(jm);

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
        public async Task<JsonResult> GetDetails([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var result = await _coreCmsOrderServices.GetOrderInfoByOrderId(entity.id);
            if (result == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            jm.code = result.status ? 0 : 1;
            jm.data = result.data;

            return new JsonResult(jm);
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
        public async Task<JsonResult> GetPrintTpl([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var result = await _coreCmsOrderServices.GetOrderInfoByOrderId(entity.id);
            if (result == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
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

            return new JsonResult(jm);
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
        public async Task<JsonResult> SelectExportExcel([FromBody] FMArrayStringIds entity)
        {
            var jm = new AdminUiCallBack();

            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel = await _coreCmsOrderServices.QueryListByClauseAsync(p => entity.id.Contains(p.orderId), p => p.orderId, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("订单号");
            row1.CreateCell(1).SetCellValue("商品总价");
            row1.CreateCell(2).SetCellValue("已支付的金额");
            row1.CreateCell(3).SetCellValue("订单实际销售总额");
            row1.CreateCell(4).SetCellValue("支付状态");
            row1.CreateCell(5).SetCellValue("发货状态");
            row1.CreateCell(6).SetCellValue("订单状态");
            row1.CreateCell(7).SetCellValue("订单类型");
            row1.CreateCell(8).SetCellValue("支付方式代码");
            row1.CreateCell(9).SetCellValue("支付时间");
            row1.CreateCell(10).SetCellValue("配送方式ID 关联ship.id");
            row1.CreateCell(11).SetCellValue("配送方式名称");
            row1.CreateCell(12).SetCellValue("配送费用");
            row1.CreateCell(13).SetCellValue("用户ID 关联user.id");
            row1.CreateCell(14).SetCellValue("店铺ID 关联seller.id");
            row1.CreateCell(15).SetCellValue("售后状态");
            row1.CreateCell(16).SetCellValue("确认收货时间");
            row1.CreateCell(17).SetCellValue("自提门店ID，0就是不门店自提");
            row1.CreateCell(18).SetCellValue("收货地区ID");
            row1.CreateCell(19).SetCellValue("收货详细地址");
            row1.CreateCell(20).SetCellValue("收货人姓名");
            row1.CreateCell(21).SetCellValue("收货电话");
            row1.CreateCell(22).SetCellValue("商品总重量");
            row1.CreateCell(23).SetCellValue("是否开发票");
            row1.CreateCell(24).SetCellValue("税号");
            row1.CreateCell(25).SetCellValue("发票抬头");
            row1.CreateCell(26).SetCellValue("使用积分");
            row1.CreateCell(27).SetCellValue("积分抵扣金额");
            row1.CreateCell(28).SetCellValue("订单优惠金额");
            row1.CreateCell(29).SetCellValue("商品优惠金额");
            row1.CreateCell(30).SetCellValue("优惠券优惠额度");
            row1.CreateCell(31).SetCellValue("优惠券信息");
            row1.CreateCell(32).SetCellValue("优惠信息");
            row1.CreateCell(33).SetCellValue("买家备注");
            row1.CreateCell(34).SetCellValue("下单IP");
            row1.CreateCell(35).SetCellValue("卖家备注");
            row1.CreateCell(36).SetCellValue("订单来源");
            row1.CreateCell(37).SetCellValue("是否评论");
            row1.CreateCell(38).SetCellValue("删除标志");
            row1.CreateCell(39).SetCellValue("");
            row1.CreateCell(40).SetCellValue("");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].orderId.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].goodsAmount.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].payedAmount.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].orderAmount.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].payStatus.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].shipStatus.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].orderType.ToString());
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].paymentCode.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].paymentTime.ToString());
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].logisticsId.ToString());
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].logisticsName.ToString());
                rowtemp.CreateCell(12).SetCellValue(listmodel[i].costFreight.ToString());
                rowtemp.CreateCell(13).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(14).SetCellValue(listmodel[i].sellerId.ToString());
                rowtemp.CreateCell(15).SetCellValue(listmodel[i].confirmStatus.ToString());
                rowtemp.CreateCell(16).SetCellValue(listmodel[i].confirmTime.ToString());
                rowtemp.CreateCell(17).SetCellValue(listmodel[i].storeId.ToString());
                rowtemp.CreateCell(18).SetCellValue(listmodel[i].shipAreaId.ToString());
                rowtemp.CreateCell(19).SetCellValue(listmodel[i].shipAddress.ToString());
                rowtemp.CreateCell(20).SetCellValue(listmodel[i].shipName.ToString());
                rowtemp.CreateCell(21).SetCellValue(listmodel[i].shipMobile.ToString());
                rowtemp.CreateCell(22).SetCellValue(listmodel[i].weight.ToString());
                rowtemp.CreateCell(23).SetCellValue(listmodel[i].taxType.ToString());
                rowtemp.CreateCell(24).SetCellValue(listmodel[i].taxCode.ToString());
                rowtemp.CreateCell(25).SetCellValue(listmodel[i].taxTitle.ToString());
                rowtemp.CreateCell(26).SetCellValue(listmodel[i].point.ToString());
                rowtemp.CreateCell(27).SetCellValue(listmodel[i].pointMoney.ToString());
                rowtemp.CreateCell(28).SetCellValue(listmodel[i].orderDiscountAmount.ToString());
                rowtemp.CreateCell(29).SetCellValue(listmodel[i].goodsDiscountAmount.ToString());
                rowtemp.CreateCell(30).SetCellValue(listmodel[i].couponDiscountAmount.ToString());
                rowtemp.CreateCell(31).SetCellValue(listmodel[i].coupon.ToString());
                rowtemp.CreateCell(32).SetCellValue(listmodel[i].promotionList.ToString());
                rowtemp.CreateCell(33).SetCellValue(listmodel[i].memo.ToString());
                rowtemp.CreateCell(34).SetCellValue(listmodel[i].ip.ToString());
                rowtemp.CreateCell(35).SetCellValue(listmodel[i].mark.ToString());
                rowtemp.CreateCell(36).SetCellValue(listmodel[i].source.ToString());
                rowtemp.CreateCell(37).SetCellValue(listmodel[i].isComment.ToString());
                rowtemp.CreateCell(38).SetCellValue(listmodel[i].isdel.ToString());
                rowtemp.CreateCell(39).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(40).SetCellValue(listmodel[i].updateTime.ToString());

            }
            // 导出excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsOrder导出(选择结果).xls";
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

            return new JsonResult(jm);
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
        public async Task<JsonResult> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsOrder>();
            //查询筛选

            //订单号 nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId))
            {
                where = where.And(p => p.orderId.Contains(orderId));
            }
            //支付状态 int
            var payStatus = ObjectExtensions.ObjectToInt(Request.Form["payStatus"].FirstOrDefault(), 0);
            if (payStatus > 0)
            {
                where = where.And(p => p.payStatus == payStatus);
            }
            //发货状态 int
            var shipStatus = ObjectExtensions.ObjectToInt(Request.Form["shipStatus"].FirstOrDefault(), 0);
            if (shipStatus > 0)
            {
                where = where.And(p => p.shipStatus == shipStatus);
            }
            //订单状态 int
            var status = ObjectExtensions.ObjectToInt(Request.Form["status"].FirstOrDefault(), 0);
            if (status > 0)
            {
                where = where.And(p => p.status == status);
            }
            //订单类型 int
            var orderType = ObjectExtensions.ObjectToInt(Request.Form["orderType"].FirstOrDefault(), 0);
            if (orderType > 0)
            {
                where = where.And(p => p.orderType == orderType);
            }
            //支付方式代码 nvarchar
            var paymentCode = Request.Form["paymentCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentCode))
            {
                where = where.And(p => p.paymentCode.Contains(paymentCode));
            }
            //支付时间 datetime
            var paymentTime = Request.Form["paymentTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(paymentTime))
            {
                var dt = ObjectExtensions.ObjectToDate(paymentTime);
                where = where.And(p => p.paymentTime > dt);
            }
            //配送方式名称 nvarchar
            var logisticsName = Request.Form["logisticsName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logisticsName))
            {
                where = where.And(p => p.logisticsName.Contains(logisticsName));
            }
            //用户ID 关联user.id int
            var userId = ObjectExtensions.ObjectToInt(Request.Form["userId"].FirstOrDefault(), 0);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            //店铺ID 关联seller.id int
            var sellerId = ObjectExtensions.ObjectToInt(Request.Form["sellerId"].FirstOrDefault(), 0);
            if (sellerId > 0)
            {
                where = where.And(p => p.sellerId == sellerId);
            }
            //售后状态 int
            var confirmStatus = ObjectExtensions.ObjectToInt(Request.Form["confirmStatus"].FirstOrDefault(), 0);
            if (confirmStatus > 0)
            {
                where = where.And(p => p.confirmStatus == confirmStatus);
            }
            //确认收货时间 datetime
            var confirmTime = Request.Form["confirmTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(confirmTime))
            {
                var dt = ObjectExtensions.ObjectToDate(confirmTime);
                where = where.And(p => p.confirmTime > dt);
            }
            //自提门店ID，0就是不门店自提 int
            var storeId = ObjectExtensions.ObjectToInt(Request.Form["storeId"].FirstOrDefault(), 0);
            if (storeId > 0)
            {
                where = where.And(p => p.storeId == storeId);
            }
            //收货地区ID int
            var shipAreaId = ObjectExtensions.ObjectToInt(Request.Form["shipAreaId"].FirstOrDefault(), 0);
            if (shipAreaId > 0)
            {
                where = where.And(p => p.shipAreaId == shipAreaId);
            }
            //收货详细地址 nvarchar
            var shipAddress = Request.Form["shipAddress"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipAddress))
            {
                where = where.And(p => p.shipAddress.Contains(shipAddress));
            }
            //收货人姓名 nvarchar
            var shipName = Request.Form["shipName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipName))
            {
                where = where.And(p => p.shipName.Contains(shipName));
            }
            //收货电话 nvarchar
            var shipMobile = Request.Form["shipMobile"].FirstOrDefault();
            if (!string.IsNullOrEmpty(shipMobile))
            {
                where = where.And(p => p.shipMobile.Contains(shipMobile));
            }
            //税号 nvarchar
            var taxCode = Request.Form["taxCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(taxCode))
            {
                where = where.And(p => p.taxCode.Contains(taxCode));
            }
            //发票抬头 nvarchar
            var taxTitle = Request.Form["taxTitle"].FirstOrDefault();
            if (!string.IsNullOrEmpty(taxTitle))
            {
                where = where.And(p => p.taxTitle.Contains(taxTitle));
            }
            //使用积分 int
            var point = ObjectExtensions.ObjectToInt(Request.Form["point"].FirstOrDefault(), 0);
            if (point > 0)
            {
                where = where.And(p => p.point == point);
            }
            //优惠券信息 nvarchar
            var coupon = Request.Form["coupon"].FirstOrDefault();
            if (!string.IsNullOrEmpty(coupon))
            {
                where = where.And(p => p.coupon.Contains(coupon));
            }
            //优惠信息 nvarchar
            var promotionList = Request.Form["promotionList"].FirstOrDefault();
            if (!string.IsNullOrEmpty(promotionList))
            {
                where = where.And(p => p.promotionList.Contains(promotionList));
            }
            //买家备注 nvarchar
            var memo = Request.Form["memo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(memo))
            {
                where = where.And(p => p.memo.Contains(memo));
            }
            //下单IP nvarchar
            var ip = Request.Form["ip"].FirstOrDefault();
            if (!string.IsNullOrEmpty(ip))
            {
                where = where.And(p => p.ip.Contains(ip));
            }
            //卖家备注 nvarchar
            var mark = Request.Form["mark"].FirstOrDefault();
            if (!string.IsNullOrEmpty(mark))
            {
                where = where.And(p => p.mark.Contains(mark));
            }
            //订单来源 int
            var source = ObjectExtensions.ObjectToInt(Request.Form["source"].FirstOrDefault(), 0);
            if (source > 0)
            {
                where = where.And(p => p.source == source);
            }
            //是否评论 bit
            var isComment = Request.Form["isComment"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isComment) && isComment.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isComment == true);
            }
            else if (!string.IsNullOrEmpty(isComment) && isComment.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isComment == false);
            }
            //删除标志 bit
            var isdel = Request.Form["isdel"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isdel) && isdel.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isdel == true);
            }
            else if (!string.IsNullOrEmpty(isdel) && isdel.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isdel == false);
            }
            // datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                var dt = ObjectExtensions.ObjectToDate(createTime);
                where = where.And(p => p.createTime > dt);
            }
            // datetime
            var updateTime = Request.Form["updateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updateTime))
            {
                var dt = ObjectExtensions.ObjectToDate(updateTime);
                where = where.And(p => p.updateTime > dt);
            }
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var sheet1 = book.CreateSheet("Sheet1");
            //获取list数据
            var listmodel = await _coreCmsOrderServices.QueryListByClauseAsync(where, p => p.orderId, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("订单号");
            row1.CreateCell(1).SetCellValue("商品总价");
            row1.CreateCell(2).SetCellValue("已支付的金额");
            row1.CreateCell(3).SetCellValue("订单实际销售总额");
            row1.CreateCell(4).SetCellValue("支付状态");
            row1.CreateCell(5).SetCellValue("发货状态");
            row1.CreateCell(6).SetCellValue("订单状态");
            row1.CreateCell(7).SetCellValue("订单类型");
            row1.CreateCell(8).SetCellValue("支付方式代码");
            row1.CreateCell(9).SetCellValue("支付时间");
            row1.CreateCell(10).SetCellValue("配送方式ID 关联ship.id");
            row1.CreateCell(11).SetCellValue("配送方式名称");
            row1.CreateCell(12).SetCellValue("配送费用");
            row1.CreateCell(13).SetCellValue("用户ID 关联user.id");
            row1.CreateCell(14).SetCellValue("店铺ID 关联seller.id");
            row1.CreateCell(15).SetCellValue("售后状态");
            row1.CreateCell(16).SetCellValue("确认收货时间");
            row1.CreateCell(17).SetCellValue("自提门店ID，0就是不门店自提");
            row1.CreateCell(18).SetCellValue("收货地区ID");
            row1.CreateCell(19).SetCellValue("收货详细地址");
            row1.CreateCell(20).SetCellValue("收货人姓名");
            row1.CreateCell(21).SetCellValue("收货电话");
            row1.CreateCell(22).SetCellValue("商品总重量");
            row1.CreateCell(23).SetCellValue("是否开发票");
            row1.CreateCell(24).SetCellValue("税号");
            row1.CreateCell(25).SetCellValue("发票抬头");
            row1.CreateCell(26).SetCellValue("使用积分");
            row1.CreateCell(27).SetCellValue("积分抵扣金额");
            row1.CreateCell(28).SetCellValue("订单优惠金额");
            row1.CreateCell(29).SetCellValue("商品优惠金额");
            row1.CreateCell(30).SetCellValue("优惠券优惠额度");
            row1.CreateCell(31).SetCellValue("优惠券信息");
            row1.CreateCell(32).SetCellValue("优惠信息");
            row1.CreateCell(33).SetCellValue("买家备注");
            row1.CreateCell(34).SetCellValue("下单IP");
            row1.CreateCell(35).SetCellValue("卖家备注");
            row1.CreateCell(36).SetCellValue("订单来源");
            row1.CreateCell(37).SetCellValue("是否评论");
            row1.CreateCell(38).SetCellValue("删除标志");
            row1.CreateCell(39).SetCellValue("");
            row1.CreateCell(40).SetCellValue("");

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listmodel.Count; i++)
            {
                var rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(listmodel[i].orderId.ToString());
                rowtemp.CreateCell(1).SetCellValue(listmodel[i].goodsAmount.ToString());
                rowtemp.CreateCell(2).SetCellValue(listmodel[i].payedAmount.ToString());
                rowtemp.CreateCell(3).SetCellValue(listmodel[i].orderAmount.ToString());
                rowtemp.CreateCell(4).SetCellValue(listmodel[i].payStatus.ToString());
                rowtemp.CreateCell(5).SetCellValue(listmodel[i].shipStatus.ToString());
                rowtemp.CreateCell(6).SetCellValue(listmodel[i].status.ToString());
                rowtemp.CreateCell(7).SetCellValue(listmodel[i].orderType.ToString());
                rowtemp.CreateCell(8).SetCellValue(listmodel[i].paymentCode.ToString());
                rowtemp.CreateCell(9).SetCellValue(listmodel[i].paymentTime.ToString());
                rowtemp.CreateCell(10).SetCellValue(listmodel[i].logisticsId.ToString());
                rowtemp.CreateCell(11).SetCellValue(listmodel[i].logisticsName.ToString());
                rowtemp.CreateCell(12).SetCellValue(listmodel[i].costFreight.ToString());
                rowtemp.CreateCell(13).SetCellValue(listmodel[i].userId.ToString());
                rowtemp.CreateCell(14).SetCellValue(listmodel[i].sellerId.ToString());
                rowtemp.CreateCell(15).SetCellValue(listmodel[i].confirmStatus.ToString());
                rowtemp.CreateCell(16).SetCellValue(listmodel[i].confirmTime.ToString());
                rowtemp.CreateCell(17).SetCellValue(listmodel[i].storeId.ToString());
                rowtemp.CreateCell(18).SetCellValue(listmodel[i].shipAreaId.ToString());
                rowtemp.CreateCell(19).SetCellValue(listmodel[i].shipAddress.ToString());
                rowtemp.CreateCell(20).SetCellValue(listmodel[i].shipName.ToString());
                rowtemp.CreateCell(21).SetCellValue(listmodel[i].shipMobile.ToString());
                rowtemp.CreateCell(22).SetCellValue(listmodel[i].weight.ToString());
                rowtemp.CreateCell(23).SetCellValue(listmodel[i].taxType.ToString());
                rowtemp.CreateCell(24).SetCellValue(listmodel[i].taxCode.ToString());
                rowtemp.CreateCell(25).SetCellValue(listmodel[i].taxTitle.ToString());
                rowtemp.CreateCell(26).SetCellValue(listmodel[i].point.ToString());
                rowtemp.CreateCell(27).SetCellValue(listmodel[i].pointMoney.ToString());
                rowtemp.CreateCell(28).SetCellValue(listmodel[i].orderDiscountAmount.ToString());
                rowtemp.CreateCell(29).SetCellValue(listmodel[i].goodsDiscountAmount.ToString());
                rowtemp.CreateCell(30).SetCellValue(listmodel[i].couponDiscountAmount.ToString());
                rowtemp.CreateCell(31).SetCellValue(listmodel[i].coupon.ToString());
                rowtemp.CreateCell(32).SetCellValue(listmodel[i].promotionList.ToString());
                rowtemp.CreateCell(33).SetCellValue(listmodel[i].memo.ToString());
                rowtemp.CreateCell(34).SetCellValue(listmodel[i].ip.ToString());
                rowtemp.CreateCell(35).SetCellValue(listmodel[i].mark.ToString());
                rowtemp.CreateCell(36).SetCellValue(listmodel[i].source.ToString());
                rowtemp.CreateCell(37).SetCellValue(listmodel[i].isComment.ToString());
                rowtemp.CreateCell(38).SetCellValue(listmodel[i].isdel.ToString());
                rowtemp.CreateCell(39).SetCellValue(listmodel[i].createTime.ToString());
                rowtemp.CreateCell(40).SetCellValue(listmodel[i].updateTime.ToString());

            }
            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsOrder导出(查询结果).xls";
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

            return new JsonResult(jm);
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
        public async Task<JsonResult> DoSettaxType([FromBody] FMUpdateIntegerDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsOrderServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            oldModel.taxType = entity.data;

            var bl = await _coreCmsOrderServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
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
        public async Task<JsonResult> DoSetisComment([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsOrderServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            oldModel.isComment = (bool)entity.data;

            var bl = await _coreCmsOrderServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
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
        public async Task<JsonResult> DoUpdateMark([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsOrderServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            oldModel.mark = entity.data.ToString();
            var bl = await _coreCmsOrderServices.UpdateAsync(p => new CoreCmsOrder() { mark = oldModel.mark }, p => p.orderId == oldModel.orderId);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
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
        public async Task<JsonResult> CancelOrder([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(entity.id))
            {
                jm.msg = "请提交要取消的订单号";
                return new JsonResult(jm);
            }
            var ids = entity.id.Split(",");
            var result = await _coreCmsOrderServices.CancelOrder(ids);
            jm.code = result.status ? 0 : 1;
            jm.msg = result.msg;

            return new JsonResult(jm);
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
        public async Task<JsonResult> DeleteOrder([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            if (string.IsNullOrEmpty(entity.id))
            {
                jm.msg = "请提交要取消的订单号";
                return new JsonResult(jm);
            }
            var ids = entity.id.Split(",");
            var result = await _coreCmsOrderServices.UpdateAsync(p => new CoreCmsOrder() { isdel = true }, p => ids.Contains(p.orderId));
            jm.code = result ? 0 : 1;
            jm.msg = result ? "删除成功" : "删除失败";

            return new JsonResult(jm);
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
        public async Task<JsonResult> GetOrderLogistics([FromBody] FMStringId entity)
        {
            var jm = new AdminUiCallBack();

            var result = await _coreCmsOrderServices.GetOrderInfoByOrderId(entity.id);
            if (result == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }
            jm.code = result.status ? 0 : 1;
            jm.data = result.data;

            return new JsonResult(jm);
        }
        #endregion


    }
}
