/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CoreCms.Net.Auth.Policys
{
    public class ApiResponseForClientHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public ApiResponseForClientHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            //Response.StatusCode = StatusCodes.Status401Unauthorized;
            //await Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse(StatusCode.CODE401)));

            var jm = new WebApiCallBack();
            jm.status = false;
            jm.code = 14007;
            jm.data = 14007;
            jm.msg = "很抱歉，授权失效，请重新登录!";
            await Response.WriteAsync(JsonConvert.SerializeObject(jm));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            //Response.StatusCode = StatusCodes.Status403Forbidden;
            //await Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse(StatusCode.CODE403)));

            var jm = new WebApiCallBack();
            jm.status = false;
            jm.code = 14007;
            jm.data = 14007;
            jm.msg = "很抱歉，授权失效，请重新登录!";
            await Response.WriteAsync(JsonConvert.SerializeObject(jm));

        }

    }
}
