/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/29 21:25:02
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCms.Net.WeChat.Service.Models
{
    /// <summary>接收加密信息统一接口（同时也支持非加密信息）</summary>
    public interface IEncryptPostModel
    {
        /// <summary>指定当前服务账号的唯一领域定义（主要为 APM 服务），例如 AppId</summary>
        string DomainId { get; set; }

        /// <summary>Signature</summary>
        string Signature { get; set; }

        /// <summary>Msg_Signature</summary>
        string Msg_Signature { get; set; }

        /// <summary>Timestamp</summary>
        string Timestamp { get; set; }

        /// <summary>Nonce</summary>
        string Nonce { get; set; }

        /// <summary>Token</summary>
        string Token { get; set; }

        /// <summary>EncodingAESKey</summary>
        string EncodingAESKey { get; set; }
    }
}
