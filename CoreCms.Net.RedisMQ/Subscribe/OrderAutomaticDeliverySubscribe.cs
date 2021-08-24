using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Essensoft.Paylink.WeChatPay.V2;
using Essensoft.Paylink.WeChatPay.V2.Notify;
using InitQ.Abstractions;
using InitQ.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoreCms.Net.RedisMQ.Subscribe
{
    /// <summary>
    /// 门店订单自动发货
    /// </summary>
    public class OrderAutomaticDeliverySubscribe : IRedisSubscribe
    {

        private readonly ICoreCmsOrderServices _orderServices;
        private readonly ICoreCmsOrderItemServices _orderItemServices;
        private readonly ICoreCmsSettingServices _settingServices;


        public OrderAutomaticDeliverySubscribe(ICoreCmsSettingServices settingServices, ICoreCmsAgentOrderServices agentOrderServices, ICoreCmsOrderServices orderServices, ICoreCmsOrderItemServices orderItemServices)
        {
            _settingServices = settingServices;
            _orderServices = orderServices;
            _orderItemServices = orderItemServices;
        }

        /// <summary>
        /// 订单完成时，门店订单自动发货
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Subscribe(RedisMessageQueueKey.OrderAutomaticDelivery)]

        private async Task OrderAutomaticDelivery(string msg)
        {
            try
            {
                var order = JsonConvert.DeserializeObject<CoreCmsOrder>(msg);
                if (order != null)
                {
                    var goodItems = await _orderItemServices.QueryListByClauseAsync(p => p.orderId == order.orderId);
                    if (!goodItems.Any())
                    {
                        NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "订单自动发货", "明细获取失败");
                        return;
                    }

                    Dictionary<int, int> items = new Dictionary<int, int>();

                    goodItems.ForEach(p =>
                    {
                        items.Add(p.productId, p.nums);
                    });

                    var result = new WebApiCallBack();

                    if (order.receiptType == (int)GlobalEnumVars.OrderReceiptType.SelfDelivery)
                    {
                        result = await _orderServices.Ship(order.orderId, "shangmenziti", "无", items, order.shipName, order.shipMobile, order.shipAddress, order.memo, order.storeId, order.shipAreaId);
                    }
                    else if (order.receiptType == (int)GlobalEnumVars.OrderReceiptType.IntraCityService)
                    {
                        result = await _orderServices.Ship(order.orderId, "benditongcheng", "无", items, order.shipName, order.shipMobile, order.shipAddress, order.memo, order.storeId, order.shipAreaId);
                    }

                    NLogUtil.WriteAll(result.status ? NLog.LogLevel.Info : NLog.LogLevel.Error,
                        LogType.RedisMessageQueue, "订单自动发货", JsonConvert.SerializeObject(result));
                }
                else
                {
                    NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "订单自动发货", "订单获取失败");
                }

            }
            catch (Exception ex)
            {
                NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "订单自动发货", msg, ex);
                throw;
            }
            await Task.CompletedTask;
        }




    }
}
