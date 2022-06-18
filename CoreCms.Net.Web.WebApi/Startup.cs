using Autofac;
using AutoMapper;
using CoreCms.Net.Auth;
using CoreCms.Net.Configuration;
using CoreCms.Net.Core.AutoFac;
using CoreCms.Net.Core.Config;
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.Mapping;
using CoreCms.Net.Middlewares;
using CoreCms.Net.Swagger;
using CoreCms.Net.Task;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using InitQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Qc.YilianyunSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using CoreCms.Net.RedisMQ.Subscribe;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.WeChat.Service.Mediator;
using Essensoft.Paylink.Alipay;
using Essensoft.Paylink.WeChatPay;
using MediatR;
using Microsoft.AspNetCore.HttpOverrides;

namespace CoreCms.Net.Web.WebApi
{
    /// <summary>
    /// 启动配置
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        /// <summary>
        /// 配置属性
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// web环境
        /// </summary>
        public IWebHostEnvironment Env { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //添加本地路径获取支持
            services.AddSingleton(new AppSettingsHelper(Env.ContentRootPath));
            services.AddSingleton(new LogLockHelper(Env.ContentRootPath));

            //Memory缓存
            services.AddMemoryCacheSetup();
            //Redis缓存
            services.AddRedisCacheSetup();

            //添加数据库连接SqlSugar注入支持
            services.AddSqlSugarSetup();
            //配置跨域（CORS）
            services.AddCorsSetup();


            //添加session支持(session依赖于cache进行存储)
            services.AddSession();
            // AutoMapper支持
            services.AddAutoMapper(typeof(AutoMapperConfiguration));

            //MediatR（只需要注册一个,同项目或类库下就不需要注册多个）
            services.AddMediatR(typeof(TextMessageEventCommand).Assembly);

            //使用 SignalR
            services.AddSignalR();

            //Redis消息队列
            services.AddRedisMessageQueueSetup();

            // 引入Payment 依赖注入(支付宝支付/微信支付)
            services.AddAlipay();
            services.AddWeChatPay();

            // 在 appsettings.json 中 配置选项
            services.Configure<WeChatPayOptions>(Configuration.GetSection("WeChatPay"));
            services.Configure<AlipayOptions>(Configuration.GetSection("Alipay"));


            //注册自定义微信接口配置文件
            services.Configure<WeChat.Service.Options.WeChatOptions>(Configuration.GetSection(nameof(WeChat.Service.Options.WeChatOptions)));

            // 注入工厂 HTTP 客户端
            services.AddHttpClient();
            services.AddSingleton<WeChat.Service.HttpClients.IWeChatApiHttpClientFactory, WeChat.Service.HttpClients.WeChatApiHttpClientFactory>();


            //Swagger接口文档注入
            services.AddClientSwaggerSetup();

            //配置易联云打印机
            services.AddYiLianYunSetup();

            //注册Hangfire定时任务
            services.AddHangFireSetup();


            //授权支持注入
            services.AddAuthorizationSetupForClient();
            //上下文注入
            services.AddHttpContextSetup();

            //服务配置中加入AutoFac控制器替换规则。
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //注册mvc，注册razor引擎视图
            services.AddMvc(options =>
                {
                    //实体验证
                    options.Filters.Add<RequiredErrorForClent>();
                    //异常处理
                    options.Filters.Add<GlobalExceptionsFilterForClent>();
                    //Swagger剔除不需要加入api展示的列表
                    options.Conventions.Add(new ApiExplorerIgnores());
                })
                .AddNewtonsoftJson(p =>
                {
                    //数据格式首字母小写 不使用驼峰
                    p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //不使用驼峰样式的key
                    //p.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //忽略循环引用
                    p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //设置时间格式（必须使用yyyy/MM/dd格式，因为ios系统不支持2018-03-29格式的时间，只识别2018/03/09这种格式。）
                    p.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
                });



        }

        /// <summary>
        ///     Autofac规则配置
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //获取所有控制器类型并使用属性注入
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();

            builder.RegisterModule(new AutofacModuleRegister());

        }

        // public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region 解决Ubuntu Nginx 代理不能获取IP问题
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            #endregion

            // 记录请求与返回数据 (注意开启权限，不然本地无法写入)
            app.UseRequestResponseLog();
            // 用户访问记录(必须放到外层，不然如果遇到异常，会报错，因为不能返回流)(注意开启权限，不然本地无法写入)
            app.UseRecordAccessLogsMildd();
            // 记录ip请求 (注意开启权限，不然本地无法写入)
            app.UseIpLogMildd();


            //强制显示中文
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");

            app.UseSwagger().UseSwaggerUI(c =>
            {
                //根据版本名称倒序 遍历展示
                typeof(CustomApiVersion.ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(
                    version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Doc {version}");
                    });
                //设置默认跳转到swagger-ui
                c.RoutePrefix = "doc";
                //c.RoutePrefix = string.Empty;
            });


            #region Hangfire定时任务

            //授权
            var filter = new BasicAuthAuthorizationFilter(
                new BasicAuthAuthorizationFilterOptions
                {
                    SslRedirect = false,
                    // Require secure connection for dashboard
                    RequireSsl = false,
                    // Case sensitive login checking
                    LoginCaseSensitive = false,
                    // Users
                    Users = new[]
                    {
                        new BasicAuthAuthorizationUser
                        {
                            Login = AppSettingsConstVars.HangFireLogin,
                            PasswordClear = AppSettingsConstVars.HangFirePassWord
                        }
                    }
                });
            var options = new DashboardOptions
            {
                AppPath = "/",//返回时跳转的地址
                DisplayStorageConnectionString = false,//是否显示数据库连接信息
                Authorization = new[]
                {
                    filter
                },
                IsReadOnlyFunc = Context =>
                {
                    return false;//是否只读面板
                }
            };

            app.UseHangfireDashboard("/job", options); //可以改变Dashboard的url
            HangfireDispose.HangfireService();

            #endregion




            //使用 Session
            app.UseSession();

            if (env.IsDevelopment())
            {
                // 在开发环境中，使用异常页面，这样可以暴露错误堆栈信息，所以不要放在生产环境。
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // CORS跨域
            app.UseCors(AppSettingsConstVars.CorsPolicyName);

            // Routing
            app.UseRouting();

            // 使用静态文件
            app.UseStaticFiles();
            // 先开启认证
            app.UseAuthentication();
            // 然后是授权中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "areas",
                    "{area:exists}/{controller=Default}/{action=Index}/{id?}"
                );

                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Default}/{action=Index}/{id?}");
            });



            //设置默认起始页（如default.html）
            //此处的路径是相对于wwwroot文件夹的相对路径
            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();

        }


    }
}