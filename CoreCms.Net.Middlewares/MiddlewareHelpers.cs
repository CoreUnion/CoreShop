/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *           Versions: 1.0                                             *
 *         CreateTime: 2020-02-03 23:29:27
 *           FileName: MiddlewareHelpers
 *   ClassDescription:
 ***********************************************************************/

using CoreCms.Net.Core;
using Microsoft.AspNetCore.Builder;

namespace CoreCms.Net.Middlewares
{
    /// <summary>
    /// 中间件
    /// </summary>
    public static class MiddlewareHelpers
    {
        /// <summary>
        /// 请求响应中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestResponseLog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequRespLogMildd>();
        }


        /// <summary>
        /// 异常处理中间件（后端模式）
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandlerMiddForAdmin(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddForAdmin>();
        }

        /// <summary>
        /// 异常处理中间件（客户端）
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandlerMiddForClent(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddForClent>();
        }


        /// <summary>
        /// IP请求中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIpLogMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<IPLogMildd>();
        }


        /// <summary>
        /// 用户访问接口日志中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRecordAccessLogsMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RecordAccessLogsMildd>();
        }

    }
}
