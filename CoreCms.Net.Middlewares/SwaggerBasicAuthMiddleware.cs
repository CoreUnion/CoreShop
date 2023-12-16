using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using Microsoft.AspNetCore.Http;

namespace CoreCms.Net.Middlewares
{
    /// <summary>
    /// Swagger授权登录拦截
    /// </summary>
    public class SwaggerBasicAuthMiddleware
    {

        private readonly RequestDelegate next;
        public SwaggerBasicAuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var path = "/" + AppSettingsConstVars.SwaggerRoutePrefix;

            if (context.Request.Path.StartsWithSegments(path))
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the credentials from request header
                    var header = AuthenticationHeaderValue.Parse(authHeader);
                    var inBytes = Convert.FromBase64String(header.Parameter);
                    var credentials = Encoding.UTF8.GetString(inBytes).Split(':');
                    var username = credentials[0];
                    var password = credentials[1];

                    var swaggerUserName = AppSettingsConstVars.SwaggerUserName;
                    var swaggerPassWord = AppSettingsConstVars.SwaggerPassWord;

                    // validate credentials
                    if (!string.IsNullOrEmpty(swaggerUserName) && !string.IsNullOrEmpty(swaggerPassWord) && username.Equals(swaggerUserName) && password.Equals(swaggerPassWord))
                    {
                        await next.Invoke(context).ConfigureAwait(false);
                        return;
                    }
                }
                context.Response.Headers["WWW-Authenticate"] = "Basic";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await next.Invoke(context).ConfigureAwait(false);
            }
        }



    }
}
