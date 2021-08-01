using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SKIT.FlurlHttpClient.Wechat.Api;

namespace CoreCms.Net.WeChat.Service.HttpClients
{
    public interface IWeChatApiHttpClientFactory
    {
        /// <summary>
        /// 微信公众号请求
        /// </summary>
        /// <returns></returns>
        WechatApiClient CreateWeXinClient();

        /// <summary>
        /// 微信小程序请求
        /// </summary>
        /// <returns></returns>
        WechatApiClient CreateWxOpenClient();
    }
}
