/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-08-13 23:57:23
 *        Description: 暂无
 ***********************************************************************/


using System.Threading;
using System.Threading.Tasks;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.WeChat.Service.HttpClients;
using CoreCms.Net.WeChat.Service.Models;
using MediatR;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Events;

namespace CoreCms.Net.WeChat.Service.Mediator
{
    /// <summary>
    /// 表示 TEXT 事件的数据
    /// </summary>
    public class ImageMessageEventCommand : IRequest<WeChatApiCallBack>
    {
        public ImageMessageEvent EventObj { get; set; }
    }

    /// <summary>
    /// 处理TEXT 事件的数据-以被动回复文本消息为例
    /// </summary>
    public class ImageMessageEventCommandHandler : IRequestHandler<ImageMessageEventCommand, WeChatApiCallBack>
    {
        private readonly WeChat.Service.HttpClients.IWeChatApiHttpClientFactory _weChatApiHttpClientFactory;



        public ImageMessageEventCommandHandler(IWeChatApiHttpClientFactory weChatApiHttpClientFactory)
        {
            _weChatApiHttpClientFactory = weChatApiHttpClientFactory;
        }

        public async Task<WeChatApiCallBack> Handle(ImageMessageEventCommand request, CancellationToken cancellationToken)
        {

            var jm = new WeChatApiCallBack() { Status = true };

            if (request.EventObj != null)
            {
                var client = _weChatApiHttpClientFactory.CreateWxOpenClient();
                var replyModel = new SKIT.FlurlHttpClient.Wechat.Api.Events.TransferCustomerServiceReply()
                {
                    ToUserName = request.EventObj.FromUserName,
                    FromUserName = request.EventObj.ToUserName,
                    CreateTimestamp = CommonHelper.GetTimeStampByTotalSeconds()
                };
                var replyXml = client.SerializeEventToXml(replyModel);
                jm.Data = replyXml;
            }

            return await Task.FromResult(jm);
        }
    }

}
