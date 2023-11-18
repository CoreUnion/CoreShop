/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *           Versions: 1.0                                             *
 *         CreateTime: 2020-02-03 22:49:41
 *   ClassDescription:
 ***********************************************************************/


using System;
using System.Transactions;
using CoreCms.Net.Configuration;
using CoreCms.Net.Utility.Extensions;
using Hangfire;
using Hangfire.MySql;
using Hangfire.Redis;
using Hangfire.Redis.StackExchange;
using Hangfire.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using StackExchange.Redis;

namespace CoreCms.Net.Core.Config
{
    /// <summary>
    /// 增加HangFire到单独配置
    /// </summary>
    public static class HangFireSetup
    {
        private static ConnectionMultiplexer _redis;


        public static void AddHangFireSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //注册Hangfire定时任务
            var isEnabledRedis = AppSettingsConstVars.RedisUseTimedTask;
            if (isEnabledRedis)
            {
                var configuration = ConfigurationOptions.Parse(AppSettingsConstVars.RedisConfigConnectionString, true);
                _redis = ConnectionMultiplexer.Connect(configuration);
                var db = _redis.GetDatabase();

                services.AddHangfire(x => x.UseRedisStorage(_redis, new RedisStorageOptions()
                {
                    Db = db.Database, //建议根据
                    SucceededListSize = 500,//后续列表中的最大可见后台作业，以防止它无限增长。
                    DeletedListSize = 500,//删除列表中的最大可见后台作业，以防止其无限增长。
                    InvisibilityTimeout = TimeSpan.FromMinutes(30),
                }));
            }
            else
            {
                string dbTypeString = AppSettingsConstVars.DbDbType;
                if (dbTypeString == DbType.MySql.ToString())
                {
                    services.AddHangfire(x => x.UseStorage(new MySqlStorage(AppSettingsConstVars.DbSqlConnection, new MySqlStorageOptions
                    {
                        TransactionIsolationLevel = IsolationLevel.ReadCommitted, // 事务隔离级别。默认是读取已提交。
                        QueuePollInterval = TimeSpan.FromSeconds(15),             //- 作业队列轮询间隔。默认值为15秒。
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- 作业到期检查间隔（管理过期记录）。默认值为1小时。
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- 聚合计数器的间隔。默认为5分钟。
                        PrepareSchemaIfNecessary = true,                          //- 如果设置为true，则创建数据库表。默认是true。
                        DashboardJobListLimit = 500,                            //- 仪表板作业列表限制。默认值为50000。
                        TransactionTimeout = TimeSpan.FromMinutes(1),             //- 交易超时。默认为1分钟。
                        TablesPrefix = "Hangfire"                                  //- 数据库中表的前缀。默认为none
                    })));
                }
                else if (dbTypeString == DbType.SqlServer.ToString())
                {
                    services.AddHangfire(x => x.UseSqlServerStorage(AppSettingsConstVars.DbSqlConnection, new SqlServerStorageOptions()
                    {
                        QueuePollInterval = TimeSpan.FromSeconds(15),             //- 作业队列轮询间隔。默认值为15秒。
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- 作业到期检查间隔（管理过期记录）。默认值为1小时。
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- 聚合计数器的间隔。默认为5分钟。
                        PrepareSchemaIfNecessary = true,                          //- 如果设置为true，则创建数据库表。默认是true。
                        DashboardJobListLimit = 500,                            //- 仪表板作业列表限制。默认值为50000。
                        TransactionTimeout = TimeSpan.FromMinutes(1),             //- 交易超时。默认为1分钟。
                    }));
                }
            }

            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { GlobalEnumVars.HangFireQueuesConfig.@default.ToString(), GlobalEnumVars.HangFireQueuesConfig.apis.ToString(), GlobalEnumVars.HangFireQueuesConfig.web.ToString(), GlobalEnumVars.HangFireQueuesConfig.recurring.ToString() };
                options.ServerTimeout = TimeSpan.FromMinutes(4);
                options.SchedulePollingInterval = TimeSpan.FromSeconds(15);//秒级任务需要配置短点，一般任务可以配置默认时间，默认15秒
                options.ShutdownTimeout = TimeSpan.FromMinutes(5); //超时时间
                options.WorkerCount = Math.Max(Environment.ProcessorCount, 20); //工作线程数，当前允许的最大线程，默认20
            });

        }
    }
}
