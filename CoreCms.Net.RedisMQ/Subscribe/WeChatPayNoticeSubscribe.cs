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
    /// 微信支付成功后推送到接口进行数据处理
    /// </summary>
    public class WeChatPayNoticeSubscribe : IRedisSubscribe
    {
        private readonly ICoreCmsBillPaymentsServices _billPaymentsServices;

        private readonly ICoreCmsDistributionOrderServices _distributionOrderServices;
        private readonly ICoreCmsDistributionServices _distributionServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsAgentOrderServices _agentOrderServices;


        public WeChatPayNoticeSubscribe(ICoreCmsBillPaymentsServices billPaymentsServices, ICoreCmsDistributionOrderServices distributionOrderServices, ICoreCmsDistributionServices distributionServices, ICoreCmsSettingServices settingServices, ICoreCmsUserServices userServices, ICoreCmsAgentOrderServices agentOrderServices)
        {
            _billPaymentsServices = billPaymentsServices;
            _distributionOrderServices = distributionOrderServices;
            _distributionServices = distributionServices;
            _settingServices = settingServices;
            _userServices = userServices;
            _agentOrderServices = agentOrderServices;
        }

        /// <summary>
        /// 微信支付成功后推送到接口进行数据处理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Subscribe(RedisMessageQueueKey.WeChatPayNotice)]
        private async Task WeChatPayNotice(string msg)
        {
            try
            {
                var notify = JsonConvert.DeserializeObject<WeChatPayUnifiedOrderNotify>(msg);
                if (notify is { ReturnCode: WeChatPayCode.Success })
                {
                    if (notify.ResultCode == WeChatPayCode.Success)
                    {
                        var money = Math.Round((decimal)notify.TotalFee / 100, 2);
                        await _billPaymentsServices.ToUpdate(notify.OutTradeNo,
                            (int)GlobalEnumVars.BillPaymentsStatus.Payed,
                            GlobalEnumVars.PaymentsTypes.wechatpay.ToString(), money, notify.ResultCode,
                            notify.TransactionId);
                    }
                    else
                    {
                        var money = Math.Round((decimal)notify.TotalFee / 100, 2);
                        var message = notify.ErrCode + ":" + notify.ErrCodeDes;
                        await _billPaymentsServices.ToUpdate(notify.OutTradeNo,
                            (int)GlobalEnumVars.BillPaymentsStatus.Other,
                            GlobalEnumVars.PaymentsTypes.wechatpay.ToString(), money, notify.ReturnMsg, notify.TransactionId);
                    }
                }
                NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "微信支付成功后推送到接口进行数据处理", msg);
            }
            catch (Exception ex)
            {
                NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "微信支付成功后推送到接口进行数据处理", msg, ex);
                throw;
            }
            await Task.CompletedTask;
        }

    }
}
