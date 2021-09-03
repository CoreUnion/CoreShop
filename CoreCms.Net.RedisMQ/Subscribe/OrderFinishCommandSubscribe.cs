using System;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
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
    /// 订单完成时，结算该订单
    /// </summary>
    public class OrderFinishCommandSubscribe : IRedisSubscribe
    {
        private readonly ICoreCmsBillPaymentsServices _billPaymentsServices;

        private readonly ICoreCmsDistributionOrderServices _distributionOrderServices;
        private readonly ICoreCmsDistributionServices _distributionServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsAgentOrderServices _agentOrderServices;


        public OrderFinishCommandSubscribe(ICoreCmsBillPaymentsServices billPaymentsServices, ICoreCmsDistributionOrderServices distributionOrderServices, ICoreCmsDistributionServices distributionServices, ICoreCmsSettingServices settingServices, ICoreCmsUserServices userServices, ICoreCmsAgentOrderServices agentOrderServices)
        {
            _billPaymentsServices = billPaymentsServices;
            _distributionOrderServices = distributionOrderServices;
            _distributionServices = distributionServices;
            _settingServices = settingServices;
            _userServices = userServices;
            _agentOrderServices = agentOrderServices;
        }

        /// <summary>
        /// 订单完成时，结算该订单|延迟队列
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [SubscribeDelay(RedisMessageQueueKey.OrderFinishCommand)]

        private async Task OrderFinishCommand(string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(msg))
                {
                    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "订单完结结佣", "订单编号获取失败");
                    return;
                }
                else
                {
                    await _distributionOrderServices.FinishOrder(msg);
                    await _agentOrderServices.FinishOrder(msg);
                    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "订单完结结佣", "订单编号获取正常：" + msg);
                }
                
            }
            catch (Exception ex)
            {
                NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "订单完结结佣", msg, ex);
                throw;
            }
            await Task.CompletedTask;
        }




    }
}
