using CoreCms.Net.Configuration;
using CoreCms.Net.RedisMQ.Subscribe;
using CoreCms.Net.Utility.Extensions;
using InitQ;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCms.Net.Core.Config
{
    /// <summary>
    /// Redis 消息队列 启动服务
    /// </summary>
    public static class RedisMessageQueueSetup
    {
        public static void AddRedisMessageQueueSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var dbIndex = 0;
            var ConnectionString = AppSettingsConstVars.RedisConfigConnectionString;
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                var arr = ConnectionString.ToLower().Split(',');
                if (arr.Length > 1)
                {
                    var defaultDataBaseStr = arr.FirstOrDefault(p => p.Contains("defaultdatabase"));
                    if (!string.IsNullOrEmpty(defaultDataBaseStr))
                    {
                        dbIndex = defaultDataBaseStr.Split('=').LastOrDefault("0").ObjToInt(0);
                    }
                }
            }

            services.AddInitQ(m =>
            {
                //时间间隔
                m.SuspendTime = 1000;

                //每次消费消息间隔时间(毫秒)
                m.IntervalTime = 1000;

                //redis服务器地址
                m.ConnectionString = AppSettingsConstVars.RedisConfigConnectionString;

                //对应的订阅者类，需要new一个实例对象，当然你也可以传参，比如日志对象
                m.ListSubscribe = new List<Type>() {
                    typeof(OrderAgentOrDistributionSubscribe),
                    typeof(OrderAutomaticDeliverySubscribe),
                    typeof(OrderFinishCommandSubscribe),
                    typeof(OrderPrintSubscribe),

                    typeof(LogingSubscribe),

                    typeof(UserSubscribe),
                    typeof(WeChatPayNoticeSubscribe),
                    typeof(SendWxTemplateMessageSubscribe),
                    typeof(AfterSalesReviewSubscribe),
                };
                //显示日志
                m.ShowLog = false;
                //新增自定redis驱动器
                m.DbIndex = dbIndex;
            });
        }
    }
}
