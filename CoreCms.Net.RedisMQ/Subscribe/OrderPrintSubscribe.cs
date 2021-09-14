using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Utility.Helper;
using InitQ.Abstractions;
using InitQ.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Qc.YilianyunSdk;
using LogLevel = NLog.LogLevel;

namespace CoreCms.Net.RedisMQ.Subscribe
{
    public class OrderPrintSubscribe : IRedisSubscribe
    {
        private readonly YilianyunService _yilianyunService;
        private readonly ICoreCmsApiAccessTokenServices _accessTokenServices;
        private readonly ICoreCmsOrderItemServices _orderItemServices;
        private readonly ICoreCmsAreaServices _areaServices;

        private readonly string _machineCode = AppSettingsConstVars.YiLianYunConfigMachineCode; //设备号
        private readonly string _msign = AppSettingsConstVars.YiLianYunConfigMsign;// 终端密钥
        private readonly string _printerName = AppSettingsConstVars.YiLianYunConfigPrinterName; // 打印机名称
        private readonly string _phone = AppSettingsConstVars.YiLianYunConfigPhone; //手机卡号
        private readonly bool _enabled = AppSettingsConstVars.YiLianYunConfigEnabled; //是否开启


        public OrderPrintSubscribe(YilianyunService yilianyunService, ICoreCmsApiAccessTokenServices accessTokenServices, ICoreCmsOrderItemServices orderItemServices, ICoreCmsAreaServices areaServices)
        {
            _yilianyunService = yilianyunService;
            _accessTokenServices = accessTokenServices;
            _orderItemServices = orderItemServices;
            _areaServices = areaServices;
        }

        /// <summary>
        /// 订单打印队列
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Subscribe(RedisMessageQueueKey.OrderPrint)]
        private async Task PrintQueue(string msg)
        {
            try
            {
                if (_enabled == false)
                {
                    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "订单打印队列", "打印机未开启");
                    return;
                }
                var order = JsonConvert.DeserializeObject<CoreCmsOrder>(msg);
                if (order == null)
                {
                    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "订单打印队列", "订单获取失败");
                    return;
                }

                var accessModel = await _accessTokenServices.QueryByClauseAsync(p => p.code == GlobalEnumVars.ThirdPartyEquipment.YiLianYun.ToString() && p.machineCode == _machineCode);
                string accessToken = string.Empty;
                if (accessModel == null)
                {
                    var onPostAuthTerminal = _yilianyunService.AuthTerminal(_machineCode, _msign, _phone, _printerName);
                    if (onPostAuthTerminal.IsSuccess())
                    {
                        accessToken = onPostAuthTerminal.Body.Access_Token;

                        accessModel = new CoreCmsApiAccessToken();
                        accessModel.code = GlobalEnumVars.ThirdPartyEquipment.YiLianYun.ToString();
                        accessModel.name = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.ThirdPartyEquipment>(GlobalEnumVars.ThirdPartyEquipment.YiLianYun.ToString());
                        accessModel.machineCode = _machineCode;
                        accessModel.accessToken = onPostAuthTerminal.Body.Access_Token;
                        accessModel.refreshToken = onPostAuthTerminal.Body.Refresh_Token;
                        accessModel.expiresIn = onPostAuthTerminal.Body.Expires_In != null ? Convert.ToInt32(onPostAuthTerminal.Body.Expires_In) : 0;
                        accessModel.expiressEndTime = onPostAuthTerminal.Body.ExpiressEndTime;
                        accessModel.parameters = JsonConvert.SerializeObject(onPostAuthTerminal.Body);
                        accessModel.createTime = DateTime.Now;

                        await _accessTokenServices.InsertAsync(accessModel);
                    }
                    else
                    {
                        NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "易联云重新获取Token失败", JsonConvert.SerializeObject(onPostAuthTerminal));
                        return;
                    }
                }
                else
                {
                    //判断是否过期
                    var dt = DateTime.Now;
                    if (dt >= accessModel.expiressEndTime)
                    {
                        var result = _yilianyunService.RefreshToken(accessModel.machineCode, accessModel.refreshToken);
                        if (result.IsSuccess())
                        {
                            accessToken = result.Body.Access_Token;

                            accessModel.accessToken = result.Body.Access_Token;
                            accessModel.refreshToken = result.Body.Refresh_Token;
                            accessModel.expiresIn = result.Body.Expires_In != null ? Convert.ToInt32(result.Body.Expires_In) : 0;
                            accessModel.expiressEndTime = result.Body.ExpiressEndTime;
                            await _accessTokenServices.UpdateAsync(accessModel);
                        }
                        else
                        {
                            NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "易联云更新Token失败", JsonConvert.SerializeObject(result));
                            return;
                        }
                    }
                    else
                    {
                        accessToken = accessModel.accessToken;
                    }
                }
                //获取打印机是否在线
                var printerStatus = string.Empty;
                var resultOnline = _yilianyunService.PrinterGetStatus(accessModel.accessToken, accessModel.machineCode);
                if (resultOnline.IsSuccess())
                {
                    printerStatus = resultOnline.Body.State.ToString();
                }
                if (printerStatus == "在线")
                {
                    var payStr = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.PaymentsTypes>(order.paymentCode);
                    var items = await _orderItemServices.QueryListByClauseAsync(p => p.orderId == order.orderId);
                    var areas = await _areaServices.GetAreaFullName(order.shipAreaId);
                    order.shipAreaName = areas.status ? areas.data + "" : "";

                    var receiptType = string.Empty;
                    if (order.receiptType == (int)GlobalEnumVars.OrderReceiptType.IntraCityService)
                    {
                        receiptType = "同城配送";
                    }
                    else if (order.receiptType == (int)GlobalEnumVars.OrderReceiptType.Logistics)
                    {
                        receiptType = "物流快递";
                    }
                    else if (order.receiptType == (int)GlobalEnumVars.OrderReceiptType.SelfDelivery)
                    {
                        receiptType = "门店自提";
                    }

                    var printStr = new StringBuilder();
                    printStr.Append("<center>--" + receiptType + "订单--</center>\r");
                    printStr.Append("................................\r");
                    printStr.Append("<center>--" + payStr + "--</center>\r");
                    printStr.Append("下单时间：" + order.createTime.ToString("yyyy-MM-dd HH:mm:ss") + "\r");
                    printStr.Append("订单编号：" + order.orderId + "\r");
                    printStr.Append("**************商品**************\r");
                    printStr.Append("<center>--购买明细--</center>\r");

                    if (items != null && items.Any())
                    {
                        foreach (var item in items)
                        {
                            printStr.Append(item.name + "*" + item.nums + "\r");
                        }
                    }
                    //printStr.Append("<center>--其他消费--</center>\r");
                    //printStr.Append("餐盒 1 2\r");
                    printStr.Append("................................\r");
                    printStr.Append("积分抵扣：￥" + order.pointMoney + "\r");
                    printStr.Append("订单优惠：￥" + order.orderDiscountAmount + "\r");
                    printStr.Append("商品优惠：￥" + order.goodsDiscountAmount + "\r");
                    printStr.Append("优惠券：￥" + order.couponDiscountAmount + "\r");
                    printStr.Append("总价：￥" + order.orderAmount + "\r");
                    printStr.Append("*******************************\r");
                    printStr.Append("区域：" + order.shipAreaName + "\r");
                    printStr.Append("地址：" + order.shipAddress + "\r");
                    printStr.Append("联系：" + order.shipName + " " + order.shipMobile + "\r");
                    printStr.Append("***************完结*************\r");
                    var onPostPrintText = _yilianyunService.PrintText(accessToken, _machineCode, printStr.ToString());

                    NLogUtil.WriteAll(NLog.LogLevel.Trace, LogType.RedisMessageQueue, "易联云打印结果", JsonConvert.SerializeObject(onPostPrintText));
                }
                else
                {
                    NLogUtil.WriteAll(LogLevel.Trace, LogType.RedisMessageQueue, "易联云机器离线", JsonConvert.SerializeObject(resultOnline));
                }
            }
            catch (Exception ex)
            {
                NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "订单打印队列", msg, ex);
                throw;
            }
            await Task.CompletedTask;
        }

    }
}
