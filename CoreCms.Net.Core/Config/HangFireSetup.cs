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
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace CoreCms.Net.Core.Config
{
    /// <summary>
    /// 增加HangFire到单独配置
    /// </summary>
    public static class HangFireSetup
    {
        public static void AddHangFireSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //注册Hangfire定时任务
            var isEnabledRedis = AppSettingsConstVars.RedisConfigEnabled;
            if (isEnabledRedis)
            {
                services.AddHangfire(x => x.UseRedisStorage(AppSettingsConstVars.RedisConfigConnectionString));
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
                        DashboardJobListLimit = 50000,                            //- 仪表板作业列表限制。默认值为50000。
                        TransactionTimeout = TimeSpan.FromMinutes(1),             //- 交易超时。默认为1分钟。
                        TablesPrefix = "Hangfire"                                  //- 数据库中表的前缀。默认为none
                    })));
                }
                else if (dbTypeString == DbType.SqlServer.ToString())
                {
                    services.AddHangfire(x => x.UseSqlServerStorage(AppSettingsConstVars.DbSqlConnection));
                }
            }
        }
    }
}
