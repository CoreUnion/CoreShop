/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using System;
using System.Threading.Tasks;
using CoreCms.Net.Caching.AutoMate.RedisCache;
using Essensoft.Paylink.WeChatPay;
using Essensoft.Paylink.WeChatPay.V2;
using Essensoft.Paylink.WeChatPay.V2.Notify;

namespace CoreCms.Net.Web.WebApi.Controllers.PayNotify
{
    /// <summary>
    ///     微信支付异步通知
    /// </summary>
    [Route("Notify/[controller]/[action]")]
    public class WeChatPayController : ControllerBase
    {
        private readonly ICoreCmsBillPaymentsServices _billPaymentsServices;
        private readonly ICoreCmsBillRefundServices _billRefundServices;
        private readonly IWeChatPayNotifyClient _client;
        private readonly IOptions<WeChatPayOptions> _optionsAccessor;
        private readonly IRedisOperationRepository _redisOperationRepository;
        /// <summary>
        ///     构造函数
        /// </summary>
        public WeChatPayController(
            IWeChatPayNotifyClient client
            , IOptions<WeChatPayOptions> optionsAccessor
            , ICoreCmsBillPaymentsServices billPaymentsServices, ICoreCmsBillRefundServices billRefundServices, IRedisOperationRepository redisOperationRepository)
        {
            _client = client;
            _optionsAccessor = optionsAccessor;
            _billPaymentsServices = billPaymentsServices;
            _billRefundServices = billRefundServices;
            _redisOperationRepository = redisOperationRepository;
        }

        /// <summary>
        ///     统一下单支付结果通知
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Unifiedorder()
        {
            try
            {
                var notify = await _client.ExecuteAsync<WeChatPayUnifiedOrderNotify>(Request, _optionsAccessor.Value);
                if (notify.ReturnCode == WeChatPayCode.Success)
                {
                    await _redisOperationRepository.ListLeftPushAsync(RedisMessageQueueKey.WeChatPayNotice, JsonConvert.SerializeObject(notify));
                    return WeChatPayNotifyResult.Success;
                }
                NLogUtil.WriteAll(LogLevel.Trace, LogType.Order, "微信支付成功回调", JsonConvert.SerializeObject(notify));
                return NoContent();
            }
            catch (Exception ex)
            {
                NLogUtil.WriteAll(LogLevel.Trace, LogType.Order, "微信支付成功回调", "统一下单支付结果通知", ex);
                return NoContent();
            }
        }

        /// <summary>
        ///     退款结果通知
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Refund()
        {
            try
            {
                var notify = await _client.ExecuteAsync<WeChatPayRefundNotify>(Request, _optionsAccessor.Value);
                NLogUtil.WriteAll(LogLevel.Trace, LogType.Refund, "微信退款结果通知", JsonConvert.SerializeObject(notify));

                if (notify.ReturnCode == WeChatPayCode.Success)
                    if (notify.RefundStatus == WeChatPayCode.Success)
                    {
                        //Console.WriteLine("OutTradeNo: " + notify.OutTradeNo);
                        var memo = JsonConvert.SerializeObject(notify);
                        await _billRefundServices.UpdateAsync(p => new CoreCmsBillRefund { memo = memo }, p => p.refundId == notify.OutTradeNo);
                        return WeChatPayNotifyResult.Success;
                    }
                return NoContent();
            }
            catch (Exception ex)
            {
                NLogUtil.WriteAll(LogLevel.Trace, LogType.Refund, "微信退款结果通知", "退款结果通知", ex);

                return NoContent();
            }
        }
    }
}