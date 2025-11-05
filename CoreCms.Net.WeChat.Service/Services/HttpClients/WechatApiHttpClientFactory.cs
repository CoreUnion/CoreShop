using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Options;
using SKIT.FlurlHttpClient;
using SKIT.FlurlHttpClient.Wechat;
using SKIT.FlurlHttpClient.Wechat.Api;

namespace CoreCms.Net.WeChat.Service.HttpClients
{
    public partial class WeChatApiHttpClientFactory : IWeChatApiHttpClientFactory
    {
        private readonly System.Net.Http.IHttpClientFactory _httpClientFactory;
        private readonly Options.WeChatOptions _weChatOptions;

        public WeChatApiHttpClientFactory(
            System.Net.Http.IHttpClientFactory httpClientFactory,
            IOptions<Options.WeChatOptions> weChatOptions)
        {
            _httpClientFactory = httpClientFactory;
            _weChatOptions = weChatOptions.Value;
        }

        /// <summary>
        /// 微信公众号请求
        /// </summary>
        /// <returns></returns>
        public WechatApiClient CreateWeXinClient()
        {
            if (string.IsNullOrEmpty(_weChatOptions.WeiXinAppId) || string.IsNullOrEmpty(_weChatOptions.WeiXinAppSecret))
                throw new Exception("未在配置项中找到微信公众号配置讯息。");

            var wechatApiClientOptions = new WechatApiClientOptions()
            {
	            AppId = _weChatOptions.WeiXinAppId,
	            AppSecret = _weChatOptions.WeiXinAppSecret,
	            PushEncodingAESKey = _weChatOptions.WeiXinEncodingAESKey,
	            PushToken = _weChatOptions.WeiXinToken
            };


			var wechatApiClient = WechatApiClientBuilder.Create(wechatApiClientOptions)
	            .UseHttpClient(_httpClientFactory.CreateClient(Microsoft.Extensions.Options.Options.DefaultName), disposeClient: false)
	            .Build();


			return wechatApiClient;
        }

        /// <summary>
        /// 微信小程序请求
        /// </summary>
        /// <returns></returns>
        public WechatApiClient CreateWxOpenClient()
        {
            if (string.IsNullOrEmpty(_weChatOptions.WxOpenAppId) || string.IsNullOrEmpty(_weChatOptions.WxOpenAppSecret))
                throw new Exception("未在配置项中找到微信小程序配置讯息。");

            var wechatApiClientOptions = new WechatApiClientOptions()
            {
	            AppId = _weChatOptions.WxOpenAppId,
	            AppSecret = _weChatOptions.WxOpenAppSecret,
	            PushEncodingAESKey = _weChatOptions.WxOpenEncodingAESKey,
	            PushToken = _weChatOptions.WxOpenToken
            };

			var wechatApiClient = WechatApiClientBuilder.Create(wechatApiClientOptions)
	            .UseHttpClient(_httpClientFactory.CreateClient(Microsoft.Extensions.Options.Options.DefaultName), disposeClient: false)
	            .Build();

			return wechatApiClient;
        }
    }

}
