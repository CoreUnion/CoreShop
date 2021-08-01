/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/29 21:25:25
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCms.Net.WeChat.Service.Models
{
    /// <summary>接收加密信息统一基类（同时也支持非加密信息）</summary>
    public abstract class EncryptPostModel : IEncryptPostModel
    {
        /// <summary>指定当前服务账号的唯一领域定义（主要为 APM 服务），例如 AppId</summary>
        public abstract string DomainId { get; set; }

        /// <summary>Signature</summary>
        public string Signature { get; set; }

        /// <summary>Msg_Signature</summary>
        public string Msg_Signature { get; set; }

        /// <summary>Timestamp</summary>
        public string Timestamp { get; set; }

        /// <summary>Nonce</summary>
        public string Nonce { get; set; }

        /// <summary>Token</summary>
        public string Token { get; set; }

        /// <summary>EncodingAESKey</summary>
        public string EncodingAESKey { get; set; }

        /// <summary>设置服务器内部保密信息</summary>
        /// <param name="token"></param>
        /// <param name="encodingAESKey"></param>
        public virtual void SetSecretInfo(string token, string encodingAESKey)
        {
            this.Token = token;
            this.EncodingAESKey = encodingAESKey;
        }
    }
}
