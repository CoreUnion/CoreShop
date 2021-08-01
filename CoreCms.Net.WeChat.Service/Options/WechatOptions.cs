using System;
using Microsoft.Extensions.Options;

namespace CoreCms.Net.WeChat.Service.Options
{
    public partial class WeChatOptions : IOptions<WeChatOptions>
    {
        WeChatOptions IOptions<WeChatOptions>.Value => this;


        /// <summary>
        /// 微信公众号AppId 
        /// </summary>
        public string WeiXinAppId { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string WeiXinAppSecret { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string WeiXinEncodingAESKey { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string WeiXinToken { get; set; } = string.Empty;

        /// <summary>
        /// 微信小程序
        /// </summary>
        public string WxOpenAppId { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string WxOpenAppSecret { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string WxOpenToken { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string WxOpenEncodingAESKey { get; set; } = string.Empty;

    }


}
