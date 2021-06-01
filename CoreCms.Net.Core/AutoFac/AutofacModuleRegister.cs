using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Core.AOP;
using CoreCms.Net.Utility.Extensions;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace CoreCms.Net.Core.AutoFac
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;

            #region 带有接口层的服务注入

            var servicesDllFile = Path.Combine(basePath, "CoreCms.Net.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, "CoreCms.Net.Repository.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                var msg = "Repository.dll和Services.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                throw new Exception(msg);
            }

            // AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
            //var cacheType = new List<Type>();
            //if (AppSettingsConstVars.RedisConfigEnabled)
            //{
            //    builder.RegisterType<RedisCacheAop>();
            //    cacheType.Add(typeof(RedisCacheAop));
            //}
            //else
            //{
            //    builder.RegisterType<MemoryCacheAop>();
            //    cacheType.Add(typeof(MemoryCacheAop));
            //}

            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            //支持属性注入依赖重复
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces().InstancePerDependency()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            //支持属性注入依赖重复
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces().InstancePerDependency()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);


            // 获取 Service.dll 程序集服务，并注册
            //var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            //builder.RegisterAssemblyTypes(assemblysServices)
            //    .AsImplementedInterfaces()
            //    .InstancePerDependency()
            //    .PropertiesAutowired()
            //    .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
            //    .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。

            //// 获取 Repository.dll 程序集服务，并注册
            //var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            //builder.RegisterAssemblyTypes(assemblysRepository)
            //    .AsImplementedInterfaces()
            //    .PropertiesAutowired()
            //    .InstancePerDependency();


            #endregion

        }
    }
}
