using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.RedisMQ.Subscribe;
using CoreCms.Net.Utility.Extensions;
using InitQ;
using Microsoft.Extensions.DependencyInjection;
using Qc.YilianyunSdk;

namespace CoreCms.Net.Core.Config
{
    /// <summary>
    /// 易联云打印机 启动服务
    /// </summary>
    public static class YiLianYunSetup
    {
        public static void AddYiLianYunSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddYilianyunSdk<DefaultYilianyunSdkHook>(opt =>
            {
                // 应用ID请自行前往 dev.10ss.net 获取
                opt.ClientId = AppSettingsConstVars.YiLianYunConfigClientId;
                opt.ClientSecret = AppSettingsConstVars.YiLianYunConfigClientSecret;
                opt.YilianyunClientType = YilianyunClientType.自有应用;
                opt.SaveTokenDirPath = "./App_Data/YiLianYunLogs";
            });
        }
    }
}
