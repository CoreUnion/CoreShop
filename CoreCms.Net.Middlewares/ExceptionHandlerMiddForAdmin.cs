/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *           Versions: 1.0                                             *
 *         CreateTime: 2020-02-05 19:08:19
 *          NameSpace: CoreCms.Net.Framework.Middlewares
 *           FileName: ExceptionHandlerMidd
 *   ClassDescription: 
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoreCms.Net.Middlewares
{
    /// <summary>
    /// 异常错误统一返回记录
    /// </summary>
    public class ExceptionHandlerMiddForAdmin
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddForAdmin(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (ex == null) return;
            NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.Web, "全局捕获异常", "全局捕获异常", new Exception("全局捕获异常", ex));
            await WriteExceptionAsync(context, ex).ConfigureAwait(false);
        }

        private static async Task WriteExceptionAsync(HttpContext context, Exception e)
        {
            if (e is UnauthorizedAccessException) context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else if (e is Exception) context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            context.Response.ContentType = "application/json";
            var jm = new AdminUiCallBack();
            jm.code = 500;
            jm.data = e;
            jm.msg = "全局捕获异常";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(jm)).ConfigureAwait(false);
        }
    }
}
