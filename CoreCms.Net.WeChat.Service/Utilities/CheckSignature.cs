/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/29 21:44:44
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoreCms.Net.WeChat.Service.Models;

namespace CoreCms.Net.WeChat.Service.Utilities
{
    /// <summary>签名验证类</summary>
    public class CheckSignature
    {
        /// <summary>在网站没有提供Token（或传入为null）的情况下的默认Token，建议在网站中进行配置。</summary>
        public const string Token = "weixin";

        /// <summary>检查签名是否正确</summary>
        /// <param name="signature"></param>
        /// <param name="postModel">需要提供：Timestamp、Nonce、Token</param>
        /// <returns></returns>
        public static bool Check(string signature, PostModel postModel) => CheckSignature.Check(signature, postModel.Timestamp, postModel.Nonce, postModel.Token);

        /// <summary>检查签名是否正确</summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool Check(string signature, string timestamp, string nonce, string token = null) => signature == CheckSignature.GetSignature(timestamp, nonce, token);

        /// <summary>返回正确的签名</summary>
        /// <param name="postModel">需要提供：Timestamp、Nonce、Token</param>
        /// <returns></returns>
        public static string GetSignature(PostModel postModel) => CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, postModel.Token);

        /// <summary>返回正确的签名</summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetSignature(string timestamp, string nonce, string token = null)
        {
            token = token ?? "weixin";
            string s = string.Join("", ((IEnumerable<string>)new string[3]
            {
        token,
        timestamp,
        nonce
            }).OrderBy<string, string>((Func<string, string>)(z => z)).ToArray<string>());
            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(s));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in hash)
                stringBuilder.AppendFormat("{0:x2}", (object)num);
            return stringBuilder.ToString();
        }
    }
}
