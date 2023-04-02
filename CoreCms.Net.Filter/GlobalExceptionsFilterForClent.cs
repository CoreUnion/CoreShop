/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *           Versions: 1.0                                             *
 *         CreateTime: 2020-02-05 19:20:08
 *        Description: 
 ***********************************************************************/


using System.Net;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreCms.Net.Filter
{
    /// <summary>
    /// 接口全局异常错误日志
    /// </summary>
    public class GlobalExceptionsFilterForClent : IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {

            NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.Web, "全局异常", "全局捕获异常", context.Exception);


            HttpStatusCode status = HttpStatusCode.InternalServerError;

            //处理各种异常
            var jm = new WebApiCallBack
            {
                status = false,
                code = (int)status,
                msg = "系统返回异常，请联系管理员进行处理！",
                data = context.Exception
            };
            context.ExceptionHandled = true;
            context.Result = new ObjectResult(jm);
        }

    }

}
