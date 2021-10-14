/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.WeChat.Service.Configuration;
using CoreCms.Net.WeChat.Service.Enums;
using CoreCms.Net.WeChat.Service.HttpClients;
using CoreCms.Net.WeChat.Service.Mediator;
using CoreCms.Net.WeChat.Service.Models;
using CoreCms.Net.WeChat.Service.Options;
using CoreCms.Net.WeChat.Service.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetTaste;
using Newtonsoft.Json;
using NLog;
using SKIT.FlurlHttpClient.Wechat.Api;


namespace CoreCms.Net.Web.WebApi.Controllers.WeChatOAuth
{
    /// <summary>
    /// 微信小程序Controller
    /// </summary>
    public class WxOpenController : ControllerBase
    {

        private readonly WeChat.Service.HttpClients.IWeChatApiHttpClientFactory _weChatApiHttpClientFactory;
        private readonly WeChatOptions _weChatOptions;
        private readonly IMediator _mediator;



        /// <summary>
        /// 原始的加密请求（如果不加密则为null）
        /// </summary>
        public XDocument? EcryptRequestDocument { get; set; } = null;

        /// <summary>
        /// 是否使用加密
        /// </summary>
        public bool UsingEncryptMessage = false;

        /// <summary>
        /// 是否取消执行
        /// </summary>
        public bool CancelExecute = false;
        /// <summary>
        /// 是否使用兼容模式
        /// </summary>
        public bool UsingCompatibilityModelEncryptMessage = false;

        /// <summary>
        /// 微信小程序服务器交互
        /// </summary>
        /// <param name="weChatApiHttpClientFactory"></param>
        /// <param name="weChatOptions"></param>
        /// <param name="mediator"></param>
        public WxOpenController(IWeChatApiHttpClientFactory weChatApiHttpClientFactory, IOptions<WeChatOptions> weChatOptions, IMediator mediator)
        {
            _weChatApiHttpClientFactory = weChatApiHttpClientFactory;
            _mediator = mediator;
            _weChatOptions = weChatOptions.Value;
        }

        #region GET请求用于处理微信小程序后台的URL验证
        /// <summary>
        /// GET请求用于处理微信小程序后台的URL验证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, _weChatOptions.WxOpenToken))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, _weChatOptions.WxOpenToken) + "。" + "如果你在浏览器中看到这句话，说明此地址可以被作为微信小程序后台的Url，请注意保持Token一致。");
            }
        }
        #endregion

        #region 接收服务器推送
        /// <summary>
        /// 接收服务器推送 文档：https://developers.weixin.qq.com/miniprogram/dev/framework/server-ability/message-push.html
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Index")]

        public async Task<IActionResult> Post(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, _weChatOptions.WxOpenToken))
            {
                NLogUtil.WriteFileLog(LogLevel.Error, LogType.WxPost, "接收服务器推送（签名错误）", JsonConvert.SerializeObject(postModel));
                return Content("fail");
            }
            else
            {
                NLogUtil.WriteFileLog(LogLevel.Info, LogType.WxPost, "接收服务器推送（签名成功）", JsonConvert.SerializeObject(postModel));
            }
            postModel.Token = _weChatOptions.WxOpenToken;//根据自己后台的设置保持一致
            postModel.EncodingAESKey = _weChatOptions.WxOpenEncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = _weChatOptions.WxOpenAppId;//根据自己后台的设置保持一致（必须提供）

            var body = Request.Body;
            //获取流数据转xml流
            XDocument postDataDocument = XmlUtility.Convert(Request.GetRequestStream());

            var msgXml = string.Empty;
            var callbackXml = Init(postDataDocument, postModel, ref msgXml);

            //怕出现误判，所以将最优结果判断
            if (callbackXml != null && CancelExecute == false && !string.IsNullOrEmpty(msgXml))
            {
                /* 如果是 XML 格式的通知内容 */
                NLogUtil.WriteFileLog(LogLevel.Info, LogType.WxPost, "接收服务器推送（XML格式的通知内容）", JsonConvert.SerializeObject(callbackXml));
                var callBack = await ExecuteProcess(callbackXml, msgXml);
                NLogUtil.WriteFileLog(LogLevel.Info, LogType.WxPost, "接收服务器推送（XML通知微信服务器）", callBack.Data);
                return Content(callBack.Data);
            }
            else
            {
                NLogUtil.WriteFileLog(LogLevel.Info, LogType.WxPost, "接收服务器推送（解密失败）", JsonConvert.SerializeObject(callbackXml));
                return Content("fail");
            }
        }

        #endregion


        #region 处理xml内容
        /// <summary>
        /// 对解密后的xml数据进行筛选并分发处理结果
        /// </summary>
        public async Task<WeChatApiCallBack> ExecuteProcess(XDocument sourceXml, string msgXml)
        {
            var requestType = sourceXml.Root?.Element("MsgType")?.Value;

            WeChatApiCallBack callBack = new WeChatApiCallBack();

            if (!string.IsNullOrEmpty(requestType))
            {
                var client = _weChatApiHttpClientFactory.CreateWxOpenClient();

                switch (requestType)
                {

                    case RequestMsgType.Text:
                        var textMessageEvent = client.DeserializeEventFromXml<SKIT.FlurlHttpClient.Wechat.Api.Events.TextMessageEvent>(msgXml);
                        callBack = await _mediator.Send(new TextMessageEventCommand() { EventObj = textMessageEvent });
                        break;
                    case RequestMsgType.Location:

                        break;
                    case RequestMsgType.Image:
                        var imageMessageEvent = client.DeserializeEventFromXml<SKIT.FlurlHttpClient.Wechat.Api.Events.ImageMessageEvent>(msgXml);
                        callBack = await _mediator.Send(new ImageMessageEventCommand() { EventObj = imageMessageEvent });
                        break;
                    case RequestMsgType.Voice:
                        var voiceMessageEvent = client.DeserializeEventFromXml<SKIT.FlurlHttpClient.Wechat.Api.Events.VoiceMessageEvent>(msgXml);
                        callBack = await _mediator.Send(new VoiceMessageEventCommand() { EventObj = voiceMessageEvent });
                        break;
                    case RequestMsgType.Video:

                        break;
                    case RequestMsgType.ShortVideo:

                        break;
                    case RequestMsgType.Link:

                        break;
                    case RequestMsgType.MessageEvent:
                        var eventType = sourceXml.Root?.Element("Event")?.Value;
                        if (!string.IsNullOrEmpty(eventType))
                        {
                            switch (eventType)
                            {
                                case EventType.Subscribe:

                                    break;
                                case EventType.Unsubscribe:

                                    break;
                                case EventType.Localtion:

                                    break;

                                default:
                                    NLogUtil.WriteFileLog(LogLevel.Info, LogType.WxPost, "接收服务器推送（处理xml内容/Event无匹配）", JsonConvert.SerializeObject(sourceXml));
                                    break;
                            }
                        }
                        break;
                    default:
                        NLogUtil.WriteFileLog(LogLevel.Info, LogType.WxPost, "接收服务器推送（处理xml内容/MsgType无匹配）", JsonConvert.SerializeObject(sourceXml));
                        break;
                }
            }
            else
            {
                NLogUtil.WriteFileLog(LogLevel.Info, LogType.WxPost, "接收服务器推送（处理xml内容/获取MsgType失败）", JsonConvert.SerializeObject(sourceXml));
            }

            return callBack;

        }
        #endregion


        #region 初始化获取xml文本数据

        /// <summary>
        /// 初始化获取xml文本数据
        /// </summary>
        /// <param name="postDataDocument"></param>
        /// <param name="postModel"></param>
        /// <param name="msgXml"></param>
        /// <returns></returns>
        private XDocument? Init(XDocument postDataDocument, PostModel postModel, ref string msgXml)
        {
            //进行加密判断并处理
            var postDataStr = postDataDocument.ToString();
            XDocument decryptDoc = postDataDocument;
            if (postDataDocument.Root?.Element("Encrypt") != null && !string.IsNullOrEmpty(postDataDocument.Root.Element("Encrypt")?.Value))
            {
                //使用了加密
                UsingEncryptMessage = true;
                EcryptRequestDocument = postDataDocument;

                WXBizMsgCrypt msgCrype = new WXBizMsgCrypt(postModel.Token, postModel.EncodingAESKey, postModel.AppId);

                var result = msgCrype.DecryptMsg(postModel.Msg_Signature, postModel.Timestamp, postModel.Nonce, postDataStr, ref msgXml);
                //判断result类型
                if (result != 0)
                {
                    //验证没有通过，取消执行
                    CancelExecute = true;
                    return null;
                }
                if (postDataDocument.Root.Element("FromUserName") != null && !string.IsNullOrEmpty(postDataDocument.Root.Element("FromUserName")?.Value))
                {
                    //TODO：使用了兼容模式，进行验证即可
                    UsingCompatibilityModelEncryptMessage = true;
                }
                decryptDoc = XDocument.Parse(msgXml);//完成解密
            }
            return decryptDoc;

        }


        #endregion
    }
}