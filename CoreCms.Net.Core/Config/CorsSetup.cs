/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *           Versions: 1.0                                             *
 *         CreateTime: 2020-02-03 22:49:41
 *          NameSpace: CoreCms.Net.Framework
 *           FileName: Cors
 *   ClassDescription:
 ***********************************************************************/


using System;
using CoreCms.Net.Configuration;
using CoreCms.Net.Utility.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCms.Net.Core.Config
{
    /// <summary>
    /// 配置跨域（CORS）
    /// </summary>
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddCors(c =>
            {
                if (!AppSettingsConstVars.CorsEnableAllIPs)
                {
                    c.AddPolicy(AppSettingsConstVars.CorsPolicyName, policy =>
                        {
                            policy.WithOrigins(AppSettingsConstVars.CorsIPs.Split(','));
                            policy.AllowAnyHeader();//Ensures that the policy allows any header.
                            policy.AllowAnyMethod();
                            policy.AllowCredentials();
                        });
                }
                else
                {
                    //允许任意跨域请求
                    c.AddPolicy(AppSettingsConstVars.CorsPolicyName, policy =>
                        {
                            policy.SetIsOriginAllowed((host) => true)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                        });
                }
            });
        }
    }
}
