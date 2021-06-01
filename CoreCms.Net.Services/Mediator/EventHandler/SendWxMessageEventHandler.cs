/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-08-13 23:57:23
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Threading;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using MediatR;
using Newtonsoft.Json.Linq;
using Senparc.Weixin;
using Senparc.Weixin.Entities.TemplateMessage;

namespace CoreCms.Net.Services.Mediator
{
    /// <summary>
    /// 消息
    /// </summary>
    public class SendWxMessageCommand : IRequest<WebApiCallBack>
    {
        /// <summary>
        /// 用户序列
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 传递数据
        /// </summary>
        public JObject parameters { get; set; }
    }

    /// <summary>
    /// 处理器-微信模板消息【小程序，公众号都走这里】
    /// </summary>
    public class SendWxMessageEventHandler : IRequestHandler<SendWxMessageCommand, WebApiCallBack>
    {
        private readonly ICoreCmsUserWeChatInfoServices _userWeChatInfoServices;
        private readonly ICoreCmsUserWeChatMsgSubscriptionServices _userWeChatMsgSubscriptionServices;
        private readonly ICoreCmsUserWeChatMsgTemplateServices _userWeChatMsgTemplateServices;

        public static readonly string WxOpenAppId = Config.SenparcWeixinSetting.WxOpenAppId;//与微信小程序后台的AppId设置保持一致，区分大小写。


        public SendWxMessageEventHandler(ICoreCmsUserWeChatMsgSubscriptionServices userWeChatMsgSubscriptionServices, ICoreCmsUserWeChatMsgTemplateServices userWeChatMsgTemplateServices, ICoreCmsUserWeChatInfoServices userWeChatInfoServices)
        {
            _userWeChatMsgSubscriptionServices = userWeChatMsgSubscriptionServices;
            _userWeChatMsgTemplateServices = userWeChatMsgTemplateServices;
            _userWeChatInfoServices = userWeChatInfoServices;
        }


        public async Task<WebApiCallBack> Handle(SendWxMessageCommand request, CancellationToken cancellationToken)
        {
            var jm = new WebApiCallBack();

            if (!request.parameters.ContainsKey("parameters"))
            {
                jm.msg = "参数获取失败";
                return await Task.FromResult(jm);
            }
            var parameters = (JObject)request.parameters["parameters"];

            if (parameters == null)
            {
                jm.msg = "参数实例化失败";
                return await Task.FromResult(jm);
            }
            else if (string.IsNullOrEmpty(request.code))
            {
                jm.msg = "消息类型获取失败";
                return await Task.FromResult(jm);
            }

            if (request.code == GlobalEnumVars.PlatformMessageTypes.SellerOrderNotice.ToString())
            {
                jm.msg = "商家不通知";
                return await Task.FromResult(jm);
            }

            if (request.userId == 0 || string.IsNullOrEmpty(request.code))
            {
                jm.msg = "用户编码获取失败";
                return await Task.FromResult(jm);
            }

            var templateData = await GetUserIsTip(request.userId, request.code);
            if (templateData == null)
            {
                jm.msg = "未查询到订阅编码";
                return await Task.FromResult(jm);
            }
            var weChatUserInfo = await _userWeChatInfoServices.QueryByClauseAsync(p => p.userId == request.userId);
            if (weChatUserInfo == null)
            {
                jm.msg = "微信用户数据查询失败";
                return await Task.FromResult(jm);
            }


            var templateMessageData = new TemplateMessageData();
            var pageUrl = string.Empty;

            if (request.code == GlobalEnumVars.PlatformMessageTypes.CreateOrder.ToString())
            {
                templateMessageData[templateData.data01] = new TemplateMessageDataValue(parameters["orderId"].ToString());
                templateMessageData[templateData.data02] = new TemplateMessageDataValue(parameters["orderAmount"].ToString());
                templateMessageData[templateData.data03] = new TemplateMessageDataValue(parameters["shipName"].ToString());
                templateMessageData[templateData.data04] = new TemplateMessageDataValue(parameters["shipMobile"].ToString());
                templateMessageData[templateData.data05] = new TemplateMessageDataValue(parameters["shipAddress"].ToString());
                pageUrl = "/pages/member/order/orderdetail?orderId=" + parameters["orderId"];
            }
            else if (request.code == GlobalEnumVars.PlatformMessageTypes.RemindOrderPay.ToString())
            {
                templateMessageData[templateData.data01] = new TemplateMessageDataValue(parameters["orderId"].ToString());
                templateMessageData[templateData.data02] = new TemplateMessageDataValue(parameters["orderAmount"].ToString());
                templateMessageData[templateData.data03] = new TemplateMessageDataValue(parameters["createTime"].ToString());
                templateMessageData[templateData.data04] = new TemplateMessageDataValue("订单即将失效，请及时付款！");
                pageUrl = "/pages/member/order/orderdetail?orderId=" + parameters["orderId"];
            }
            else if (request.code == GlobalEnumVars.PlatformMessageTypes.OrderPayed.ToString())
            {
                templateMessageData[templateData.data01] = new TemplateMessageDataValue(parameters["orderId"].ToString());
                templateMessageData[templateData.data02] = new TemplateMessageDataValue(parameters["orderAmount"].ToString());
                templateMessageData[templateData.data03] = new TemplateMessageDataValue(parameters["paymentTime"].ToString());
                pageUrl = "/pages/member/order/orderdetail?orderId=" + parameters["orderId"];
            }
            else if (request.code == GlobalEnumVars.PlatformMessageTypes.DeliveryNotice.ToString())
            {
                templateMessageData[templateData.data01] = new TemplateMessageDataValue(parameters["orderId"].ToString());
                templateMessageData[templateData.data02] = new TemplateMessageDataValue(parameters["logiName"].ToString());
                templateMessageData[templateData.data03] = new TemplateMessageDataValue(parameters["logiNo"].ToString());
                pageUrl = "/pages/member/order/orderdetail?orderId=" + parameters["orderId"];
            }
            else if (request.code == GlobalEnumVars.PlatformMessageTypes.AfterSalesPass.ToString())
            {
                templateMessageData[templateData.data01] = new TemplateMessageDataValue(parameters["orderId"].ToString());
                templateMessageData[templateData.data02] = new TemplateMessageDataValue(parameters["orderAmount"].ToString());
                templateMessageData[templateData.data03] = new TemplateMessageDataValue(parameters["aftersalesId"].ToString());
                templateMessageData[templateData.data04] = new TemplateMessageDataValue(parameters["aftersalesStatus"].ToString());
                pageUrl = "/pages/member/order/orderdetail?orderId=" + parameters["orderId"];
            }
            else if (request.code == GlobalEnumVars.PlatformMessageTypes.RefundSuccess.ToString())
            {
                templateMessageData[templateData.data01] = new TemplateMessageDataValue(parameters["sourceId"].ToString());
                templateMessageData[templateData.data02] = new TemplateMessageDataValue(parameters["aftersalesId"].ToString());
                templateMessageData[templateData.data03] = new TemplateMessageDataValue(parameters["money"].ToString());
                templateMessageData[templateData.data04] = new TemplateMessageDataValue(parameters["paymentCode"].ToString());
                templateMessageData[templateData.data05] = new TemplateMessageDataValue(parameters["createTime"].ToString());
            }
            jm = await Send(WxOpenAppId, weChatUserInfo.openid, templateData.templateId, templateMessageData, pageUrl);

            return await Task.FromResult(jm);
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
            return null;
        }

        #endregion

        #region 异步发送微信模板消息
        /// <summary>
        /// 异步发送微信模板消息
        /// </summary>
        /// <param name="wxOpenAppId">appId</param>
        /// <param name="openId">openId</param>
        /// <param name="templateId">模板编号</param>
        /// <param name="tmpData">发送数据</param>
        /// <param name="pageUrl">路径（如：pages/index/index）</param>
        /// <returns></returns>
        private async Task<WebApiCallBack> Send(string wxOpenAppId, string openId, string templateId, TemplateMessageData tmpData, string pageUrl)
        {
            var jm = new WebApiCallBack();

            var result = await Senparc.Weixin.WxOpen.AdvancedAPIs.MessageApi.SendSubscribeAsync(wxOpenAppId, openId, templateId, tmpData, pageUrl);
            if (result.errcode == ReturnCode.请求成功)
            {
                jm.status = true;
                jm.msg = "消息已发送，请注意查收";
            }
            else
            {
                jm.status = false;
                jm.msg = result.errmsg;
            }

            return jm;
        }
        #endregion

    }

}
