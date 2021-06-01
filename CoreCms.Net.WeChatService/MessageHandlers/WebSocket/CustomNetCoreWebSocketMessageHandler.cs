//DPBMARK_FILE WebSocket
#if !NET45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Senparc.CO2NET.Extensions;
using Senparc.WebSocket;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.WxOpen.Containers;

namespace CoreCms.Net.WeChatService.MessageHandlers.WebSocket
{
    /// <summary>
    /// .NET Core 自定义 WebSocket 处理类
    /// </summary>
    public class CustomNetCoreWebSocketMessageHandler : WebSocketMessageHandler
    {
        public override Task OnConnecting(WebSocketHelper webSocketHandler)
        {
            //TODO:处理连接时的逻辑
            return base.OnConnecting(webSocketHandler);
        }

        public override Task OnDisConnected(WebSocketHelper webSocketHandler)
        {
            //TODO:处理断开连接时的逻辑
            return base.OnDisConnected(webSocketHandler);
        }


        public override async Task OnMessageReceiced(WebSocketHelper webSocketHandler, ReceivedMessage receivedMessage, string originalData)
        {
            if (receivedMessage == null || string.IsNullOrEmpty(receivedMessage.Message))
            {
                return;
            }

            var message = receivedMessage.Message;

            await webSocketHandler.SendMessage("originalData：" + originalData, webSocketHandler.WebSocket.Clients.Caller);
            await webSocketHandler.SendMessage("您发送了文字：" + message, webSocketHandler.WebSocket.Clients.Caller);
            await webSocketHandler.SendMessage("正在处理中（反转文字）...", webSocketHandler.WebSocket.Clients.Caller);

            await Task.Delay(1000);

            //处理文字
            var result = string.Concat(message.Reverse());
            await webSocketHandler.SendMessage(result, webSocketHandler.WebSocket.Clients.Caller);

            var appId = Senparc.Weixin.Config.SenparcWeixinSetting.WxOpenAppId;//与微信小程序账号后台的AppId设置保持一致，区分大小写。

            try
            {

                var sessionBag = SessionContainer.GetSession(receivedMessage.SessionId ?? "-");

                //临时演示使用固定openId
                var openId = sessionBag != null ? sessionBag.OpenId : receivedMessage.SessionId;// "用户未正确登陆小程序，或是在网页上发起";
                openId ??= "[未登录用户]";

                //await webSocketHandler.SendMessage("OpenId：" + openId, webSocketHandler.WebSocket.Clients.Caller);
                //await webSocketHandler.SendMessage("FormId：" + formId);

                //群发

                var shotOpenId = openId.Length > 10 ? $"***{openId.Substring(openId.Length - 10, 10)}" : openId;

                await webSocketHandler.SendMessage($"[群发消息] [来自 OpenId：{shotOpenId}，昵称：{(sessionBag?.DecodedUserInfo?.nickName) ?? "[未登录]"}]：{message}", webSocketHandler.WebSocket.Clients.All);

                //发送模板消息

            }
            catch (Exception ex)
            {
                var msg = ex.Message + "\r\n\r\n" + originalData + "\r\n\r\nAPPID:" + appId;

                await webSocketHandler.SendMessage(msg, webSocketHandler.WebSocket.Clients.Caller); //VS2017以下如果编译不通过，可以注释掉这一行

                Senparc.Weixin.WeixinTrace.SendCustomLog("WebSocket OnMessageReceiced()过程出错", msg);
            }
        }
    }
}
#endif
