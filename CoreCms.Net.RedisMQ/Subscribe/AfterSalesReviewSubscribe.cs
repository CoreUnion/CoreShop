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
    /// 售后审核通过后处理
    /// </summary>
    public class AfterSalesReviewSubscribe : IRedisSubscribe
    {
        private readonly ICoreCmsBillAftersalesServices _aftersalesServices;
        private readonly ICoreCmsDistributionOrderServices _distributionOrderServices;
        private readonly ICoreCmsAgentOrderServices _agentOrderServices;


        public AfterSalesReviewSubscribe(ICoreCmsBillAftersalesServices aftersalesServices, ICoreCmsDistributionOrderServices distributionOrderServices, ICoreCmsAgentOrderServices agentOrderServices)
        {
            _aftersalesServices = aftersalesServices;
            _distributionOrderServices = distributionOrderServices;
            _agentOrderServices = agentOrderServices;
        }

        /// <summary>
        /// 售后审核通过后处理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Subscribe(RedisMessageQueueKey.AfterSalesReview)]
        private async Task AfterSalesReview(string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(msg))
                {
                    NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "售后审核通过后处理", "审核单编号获取失败");
                    return;
                }
                var info = await _aftersalesServices.QueryByClauseAsync(p => p.aftersalesId == msg);
                if (info != null)
                {
                    await _distributionOrderServices.CancleOrderByOrderId(info.orderId);
                    await _agentOrderServices.CancleOrderByOrderId(info.orderId);
                    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "售后审核通过后处理", msg);
                }
                else
                {
                    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "售后审核通过后处理", "售后单查询失败");
                }
            }
            catch (Exception ex)
            {
                NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "售后审核通过后处理", msg, ex);
                throw;
            }
            await Task.CompletedTask;
        }

    }
}
