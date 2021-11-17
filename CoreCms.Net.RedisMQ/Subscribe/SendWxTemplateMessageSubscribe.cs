using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCms.Net.Caching.AccressToken;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.WeChat.Service.Enums;
using CoreCms.Net.WeChat.Service.HttpClients;
using CoreCms.Net.WeChat.Service.Models;
using InitQ.Abstractions;
using InitQ.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;

namespace CoreCms.Net.RedisMQ.Subscribe
{
    /// <summary>
    /// 微信模板消息【小程序，公众号都走这里】
    /// </summary>
    public class SendWxTemplateMessageSubscribe : IRedisSubscribe
    {
        private readonly ICoreCmsUserWeChatInfoServices _userWeChatInfoServices;
        private readonly ICoreCmsUserWeChatMsgSubscriptionServices _userWeChatMsgSubscriptionServices;
        private readonly ICoreCmsUserWeChatMsgTemplateServices _userWeChatMsgTemplateServices;
        private readonly IWeChatApiHttpClientFactory _weChatApiHttpClientFactory;


        public SendWxTemplateMessageSubscribe(ICoreCmsUserWeChatInfoServices userWeChatInfoServices, ICoreCmsUserWeChatMsgSubscriptionServices userWeChatMsgSubscriptionServices, ICoreCmsUserWeChatMsgTemplateServices userWeChatMsgTemplateServices, IWeChatApiHttpClientFactory weChatApiHttpClientFactory)
        {
            _userWeChatInfoServices = userWeChatInfoServices;
            _userWeChatMsgSubscriptionServices = userWeChatMsgSubscriptionServices;
            _userWeChatMsgTemplateServices = userWeChatMsgTemplateServices;
            _weChatApiHttpClientFactory = weChatApiHttpClientFactory;
        }

        /// <summary>
        /// 微信模板消息【小程序，公众号都走这里】
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Subscribe(RedisMessageQueueKey.SendWxTemplateMessage)]
        private async Task SendWxTemplateMessage(string msg)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<SendWxTemplateMessage>(msg);
                if (request != null)
                {
                    if (!request.parameters.ContainsKey("parameters"))
                    {
                        NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "微信模板消息", "参数获取失败");

                        return;
                    }
                    var parameters = (JObject)request.parameters["parameters"];

                    if (parameters == null)
                    {
                        NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "微信模板消息", "参数实例化失败");
                        return;
                    }
                    else if (string.IsNullOrEmpty(request.code))
                    {
                        NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "微信模板消息", "消息类型获取失败");
                        return;
                    }

                    if (request.code == GlobalEnumVars.PlatformMessageTypes.SellerOrderNotice.ToString())
                    {
                        NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "微信模板消息", "商家不通知");
                        return;
                    }

                    if (request.userId == 0 || string.IsNullOrEmpty(request.code))
                    {
                        NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "微信模板消息", "用户编码获取失败");
                        return;
                    }

                    var templateData = await GetUserIsTip(request.userId, request.code);
                    if (templateData == null)
                    {
                        NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "微信模板消息", "未查询到订阅编码");
                        return;
                    }
                    var weChatUserInfo = await _userWeChatInfoServices.QueryByClauseAsync(p => p.userId == request.userId);
                    if (weChatUserInfo == null)
                    {
                        NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "微信模板消息", "微信用户数据查询失败");
                        return;
                    }


                    var templateMessageData = new Dictionary<string, CgibinMessageSubscribeSendRequest.Types.DataItem>();
                    var pageUrl = string.Empty;

                    if (request.code == GlobalEnumVars.PlatformMessageTypes.CreateOrder.ToString())
                    {
                        templateMessageData[templateData.data01] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["orderId"].ToString() };
                        templateMessageData[templateData.data02] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["orderAmount"].ToString() };
                        templateMessageData[templateData.data03] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["shipName"].ToString() };
                        templateMessageData[templateData.data04] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["shipMobile"].ToString() };
                        templateMessageData[templateData.data05] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["shipAddress"].ToString() };
                        pageUrl = "/pages/member/order/detail/detail?orderId=" + parameters["orderId"];
                    }
                    else if (request.code == GlobalEnumVars.PlatformMessageTypes.RemindOrderPay.ToString())
                    {
                        templateMessageData[templateData.data01] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["orderId"].ToString() };
                        templateMessageData[templateData.data02] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["orderAmount"].ToString() };
                        templateMessageData[templateData.data03] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["createTime"].ToString() };
                        templateMessageData[templateData.data04] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = "订单即将失效，请及时付款！" };
                        pageUrl = "/pages/member/order/detail/detail?orderId=" + parameters["orderId"];
                    }
                    else if (request.code == GlobalEnumVars.PlatformMessageTypes.OrderPayed.ToString())
                    {
                        templateMessageData[templateData.data01] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["orderId"].ToString() };
                        templateMessageData[templateData.data02] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["orderAmount"].ToString() };
                        templateMessageData[templateData.data03] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["paymentTime"].ToString() };
                        pageUrl = "/pages/member/order/detail/detail?orderId=" + parameters["orderId"];
                    }
                    else if (request.code == GlobalEnumVars.PlatformMessageTypes.DeliveryNotice.ToString())
                    {
                        templateMessageData[templateData.data01] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["orderId"].ToString() };
                        templateMessageData[templateData.data02] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["logiName"].ToString() };
                        templateMessageData[templateData.data03] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["logiNo"].ToString() };
                        pageUrl = "/pages/member/order/detail/detail?orderId=" + parameters["orderId"];
                    }
                    else if (request.code == GlobalEnumVars.PlatformMessageTypes.AfterSalesPass.ToString())
                    {
                        templateMessageData[templateData.data01] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["orderId"].ToString() };
                        templateMessageData[templateData.data02] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["orderAmount"].ToString() };
                        templateMessageData[templateData.data03] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["aftersalesId"].ToString() };
                        templateMessageData[templateData.data04] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["aftersalesStatus"].ToString() };
                        pageUrl = "/pages/member/order/detail/detail?orderId=" + parameters["orderId"];
                    }
                    else if (request.code == GlobalEnumVars.PlatformMessageTypes.RefundSuccess.ToString())
                    {
                        templateMessageData[templateData.data01] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["sourceId"].ToString() };
                        templateMessageData[templateData.data02] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["aftersalesId"].ToString() };
                        templateMessageData[templateData.data03] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["money"].ToString() };
                        templateMessageData[templateData.data04] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["paymentCode"].ToString() };
                        templateMessageData[templateData.data05] = new CgibinMessageSubscribeSendRequest.Types.DataItem() { Value = parameters["createTime"].ToString() };
                    }
                    var result = await Send(weChatUserInfo.openid, templateData.templateId, templateMessageData, pageUrl);

                    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "微信模板消息", JsonConvert.SerializeObject(result));
                }
                else
                {
                    NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "微信模板消息", "模板消息推送数据为空");
                }
            }
            catch (Exception ex)
            {
                NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "微信模板消息", msg, ex);
                throw;
            }
            await Task.CompletedTask;
        }


        #region 判断是否需要通知用户返回 null或者 模板数据
        /// <summary>
        /// 判断是否需要通知用户返回 null或者 模板数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<CoreCmsUserWeChatMsgTemplate> GetUserIsTip(int userId, string code)
        {
            var newCode = string.Empty;
            if (code == GlobalEnumVars.PlatformMessageTypes.CreateOrder.ToString())
            {
                newCode = GlobalEnumVars.WeChatMsgTemplateType.order.ToString();
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.RemindOrderPay.ToString())
            {
                newCode = GlobalEnumVars.WeChatMsgTemplateType.cancel.ToString();
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.OrderPayed.ToString())
            {
                newCode = GlobalEnumVars.WeChatMsgTemplateType.pay.ToString();
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.DeliveryNotice.ToString())
            {
                newCode = GlobalEnumVars.WeChatMsgTemplateType.ship.ToString();
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.AfterSalesPass.ToString())
            {
                newCode = GlobalEnumVars.WeChatMsgTemplateType.aftersale.ToString();
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.RefundSuccess.ToString())
            {
                newCode = GlobalEnumVars.WeChatMsgTemplateType.refund.ToString();
            }
            else
            {
                return null;
            }

            var info = await _userWeChatMsgSubscriptionServices.QueryByClauseAsync(p => p.userId == userId && p.type == newCode);
            if (info != null)
            {
                return await _userWeChatMsgTemplateServices.QueryByClauseAsync(p => p.templateId == info.templateId);
            }
            return await _userWeChatMsgTemplateServices.QueryByClauseAsync(p => p.templateTitle == newCode);
            //return null;
        }

        #endregion

        #region 异步发送微信模板消息
        /// <summary>
        /// 异步发送微信模板消息
        /// </summary>
        /// <param name="openId">openId</param>
        /// <param name="templateId">模板编号</param>
        /// <param name="tmpData">发送数据</param>
        /// <param name="pageUrl">路径（如：pages/index/index）</param>
        /// <returns></returns>
        private async Task<WebApiCallBack> Send(string openId, string templateId, Dictionary<string, CgibinMessageSubscribeSendRequest.Types.DataItem> tmpData, string pageUrl)
        {
            var jm = new WebApiCallBack();
            var accessToken = WeChatCacheAccessTokenHelper.GetWxOpenAccessToken();
            var client = _weChatApiHttpClientFactory.CreateWxOpenClient();
            var request = new CgibinMessageSubscribeSendRequest();

            request.AccessToken = accessToken;
            request.MiniProgramPagePath = pageUrl;
            request.TemplateId = templateId;
            request.ToUserOpenId = openId;
            request.Data = tmpData;
            request.MiniProgramState = "formal";

            var response = await client.ExecuteCgibinMessageSubscribeSendAsync(request);
            if (response.IsSuccessful())
            {
                jm.status = true;
                jm.msg = "消息已发送，请注意查收";
            }
            else
            {
                jm.status = false;
                jm.msg = response.ErrorMessage;
            }

            return jm;
        }
        #endregion


    }
}
