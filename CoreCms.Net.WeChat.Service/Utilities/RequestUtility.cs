/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/29 23:24:45
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace CoreCms.Net.WeChat.Service.Utilities
{
    /// <summary>HTTP 请求工具类</summary>
    public static class RequestUtility
    {
        /// <summary>【异步方法】从 Request.Body 中读取流，并复制到一个独立的 MemoryStream 对象中</summary>
        /// <param name="request"></param>
        /// <param name="allowSynchronousIO"></param>
        /// <returns></returns>
        public static async Task<Stream> GetRequestMemoryStreamAsync(
          this HttpRequest request,
          bool? allowSynchronousIO = true)
        {
            IHttpBodyControlFeature bodyControlFeature = request.HttpContext.Features.Get<IHttpBodyControlFeature>();
            if (bodyControlFeature != null && allowSynchronousIO.HasValue)
                bodyControlFeature.AllowSynchronousIO = allowSynchronousIO.Value;
            return (Stream)new MemoryStream(Encoding.UTF8.GetBytes(await new StreamReader(request.Body).ReadToEndAsync()));
        }

        /// <summary>从 Request.Body 中读取流，并复制到一个独立的 MemoryStream 对象中</summary>
        /// <param name="request"></param>
        /// <param name="allowSynchronousIO"></param>
        /// <returns></returns>
        public static Stream GetRequestStream(
            this HttpRequest request,
            bool? allowSynchronousIO = true)
        {
            IHttpBodyControlFeature bodyControlFeature = request.HttpContext.Features.Get<IHttpBodyControlFeature>();
            if (bodyControlFeature != null && allowSynchronousIO.HasValue)
                bodyControlFeature.AllowSynchronousIO = allowSynchronousIO.Value;
            return (Stream)new MemoryStream(Encoding.UTF8.GetBytes(new StreamReader(request.Body).ReadToEnd()));
        }

        /// <summary>从 Request.Body 中读取流，并复制到一个独立的 MemoryStream 对象中</summary>
        /// <param name="request"></param>
        /// <param name="allowSynchronousIO"></param>
        /// <returns></returns>
        public static MemoryStream GetRequestMemoryStream(
            this HttpRequest request,
            bool? allowSynchronousIO = true)
        {
            IHttpBodyControlFeature bodyControlFeature = request.HttpContext.Features.Get<IHttpBodyControlFeature>();
            if (bodyControlFeature != null && allowSynchronousIO.HasValue)
                bodyControlFeature.AllowSynchronousIO = allowSynchronousIO.Value;
            return new MemoryStream(Encoding.UTF8.GetBytes(new StreamReader(request.Body).ReadToEnd()));
        }


    }

}
