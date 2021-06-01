/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *           Versions: 1.0                                             *
 *         CreateTime: 2020-02-02 18:41:38
 *           FileName: SwaggerSetup
 *   ClassDescription: 
 ***********************************************************************/


using System;
using System.IO;
using System.Linq;
using CoreCms.Net.Loging;
using CoreCms.Net.Swagger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CoreCms.Net.Core.Config
{
    public static class SwaggerSetup
    {

        public static void AddAdminSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var apiName = "核心商城系统管理端";

            services.AddSwaggerGen((s) =>
            {
                //遍历出全部的版本，做文档信息展示
                typeof(CustomApiVersion.ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    s.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"{apiName} 接口文档",
                        Description = $"{apiName} HTTP API " + version,
                        Contact = new OpenApiContact { Name = apiName, Email = "JianWeie@163.com", Url = new Uri("https://CoreCms.Net") },
                    });
                    s.OrderActionsBy(o => o.RelativePath);
                });

                try
                {
                    //生成API XML文档
                    var basePath = AppContext.BaseDirectory;
                    var xmlPath = Path.Combine(basePath, "doc.xml");
                    s.IncludeXmlComments(xmlPath);
                }
                catch (Exception ex)
                {
                    NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Swagger, "Swagger", "Swagger生成失败，Doc.xml丢失，请检查并拷贝。", ex);
                }

                // 开启加权小锁
                s.OperationFilter<AddResponseHeadersFilter>();
                s.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                s.OperationFilter<SecurityRequirementsOperationFilter>();

                // 必须是 oauth2
                s.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

            });
        }


        public static void AddClientSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var apiName = "核心商城系统接口端";

            services.AddSwaggerGen((s) =>
            {
                //遍历出全部的版本，做文档信息展示
                typeof(CustomApiVersion.ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    s.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"{apiName} 接口文档",
                        Description = $"{apiName} HTTP API " + version,
                        Contact = new OpenApiContact { Name = apiName, Email = "JianWeie@163.com", Url = new Uri("https://CoreCms.Net") },
                    });
                    s.OrderActionsBy(o => o.RelativePath);
                });

                try
                {
                    //生成API XML文档
                    var basePath = AppContext.BaseDirectory;
                    var xmlPath = Path.Combine(basePath, "doc.xml");
                    s.IncludeXmlComments(xmlPath);
                }
                catch (Exception ex)
                {
                    NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Swagger, "Swagger", "Swagger生成失败，Doc.xml丢失，请检查并拷贝。", ex);
                }

                // 开启加权小锁
                s.OperationFilter<AddResponseHeadersFilter>();
                s.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                s.OperationFilter<SecurityRequirementsOperationFilter>();

                // 必须是 oauth2
                s.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

            });
        }

    }
}
