/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *           Versions: 1.0                                             *
 *         CreateTime: 2020-02-03 23:30:52
 *           FileName: RequRespLogMildd
 *   ClassDescription:
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Loging;
using CoreCms.Net.Utility.Extensions;
using Microsoft.AspNetCore.Http;

namespace CoreCms.Net.Middlewares
{
    /// <summary>
    /// 中间件
    /// 记录请求和响应数据
    /// </summary>
    public class RequRespLogMildd
    {
        /// <summary>
        ///
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        public RequRespLogMildd(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (AppSettingsConstVars.MiddlewareRequestResponseLogEnabled)
            {
                // 过滤，只有接口
                if (context.Request.Path.Value.Contains("api") || context.Request.Path.Value.Contains("Api"))
                {
                    context.Request.EnableBuffering();
                    Stream originalBody = context.Response.Body;
                    try
                    {
                        // 存储请求数据
                        await RequestDataLog(context);

                        using (var ms = new MemoryStream())
                        {
                            context.Response.Body = ms;

                            await _next(context);

                            // 存储响应数据
                            ResponseDataLog(context.Response, ms);

                            ms.Position = 0;
                            await ms.CopyToAsync(originalBody);
                        }
                    }
                    catch (Exception)
                    {
                        // 记录异常
                        //ErrorLogData(context.Response, ex);
                    }
                    finally
                    {
                        context.Response.Body = originalBody;
                    }
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task RequestDataLog(HttpContext context)
        {
            var request = context.Request;
            var sr = new StreamReader(request.Body);

            var content = $" QueryData:{request.Path + request.QueryString}\r\n BodyData:{await sr.ReadToEndAsync()}";

            if (!string.IsNullOrEmpty(content))
            {
                Parallel.For(0, 1, e =>
                {
                    LogLockHelper.OutSql2Log("RequestResponseLog", "RequestResponseLog" + DateTime.Now.ToString("yyyy-MM-dd-HH"), new string[] { "Request Data:", content });

                });

                request.Body.Position = 0;
            }
        }

        private void ResponseDataLog(HttpResponse response, MemoryStream ms)
        {
            ms.Position = 0;
            var ResponseBody = new StreamReader(ms).ReadToEnd();
            // 去除 Html
            var reg = "<[^>]+>";
            var isHtml = Regex.IsMatch(ResponseBody, reg);
            if (!string.IsNullOrEmpty(ResponseBody))
            {
                Parallel.For(0, 1, e =>
                {
                    LogLockHelper.OutSql2Log("RequestResponseLog", "RequestResponseLog" + DateTime.Now.ToString("yyyy-MM-dd-HH"), new string[] { "Response Data:", ResponseBody });
                });
            }
        }
    }
}
