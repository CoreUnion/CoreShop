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

            FlurlHttp.GlobalSettings.FlurlClientFactory = new DelegatingFlurlClientFactory(_httpClientFactory);
        }

        /// <summary>
        /// 微信公众号请求
        /// </summary>
        /// <returns></returns>
        public WechatApiClient CreateWeXinClient()
        {
            if (string.IsNullOrEmpty(_weChatOptions.WeiXinAppId) || string.IsNullOrEmpty(_weChatOptions.WeiXinAppSecret))
                throw new Exception("未在配置项中找到微信公众号配置讯息。");

            var wechatApiClient = new WechatApiClient(new WechatApiClientOptions()
            {
                AppId = _weChatOptions.WeiXinAppId,
                AppSecret = _weChatOptions.WeiXinAppSecret,
            });

            wechatApiClient.Configure(settings =>
            {
                settings.JsonSerializer = new FlurlNewtonsoftJsonSerializer();
            });

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

            var WechatApiClient = new WechatApiClient(new WechatApiClientOptions()
            {
                AppId = _weChatOptions.WxOpenAppId,
                AppSecret = _weChatOptions.WxOpenAppSecret
            });

            WechatApiClient.Configure(settings =>
            {
                settings.JsonSerializer = new FlurlNewtonsoftJsonSerializer();
            });

            return WechatApiClient;
        }
    }

    public partial class WeChatApiHttpClientFactory
    {
        internal class DelegatingFlurlClientFactory : IFlurlClientFactory
        {
            private readonly System.Net.Http.IHttpClientFactory _httpClientFactory;

            public DelegatingFlurlClientFactory(System.Net.Http.IHttpClientFactory httpClientFactory)
            {
                _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            }

            public IFlurlClient Get(Url url)
            {
                return new FlurlClient(_httpClientFactory.CreateClient(url.ToUri().Host));
            }

            public void Dispose()
            {
                // Do Nothing
            }
        }
    }
}
