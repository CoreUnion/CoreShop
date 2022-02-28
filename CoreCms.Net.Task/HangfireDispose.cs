/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-08-27 2:09:38
 *        Description: 暂无
 ***********************************************************************/


using System;
using Hangfire;

namespace CoreCms.Net.Task
{
    public class HangfireDispose
    {
        #region 配置服务

        public static void HangfireService()
        {
            //Fire - And - forget（发布 / 订阅）
            //这是一个主要的后台任务类型，持久化消息队列会去处理这个任务。当你创建了一个发布 / 订阅任务，该任务会被保存到默认队列里面（默认队列是"Default"，但是支持使用多队列）。多个专注的工作者（Worker）会监听这个队列，并且从中获取任务并且完成任务。
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget"));

            //延迟
            //如果想要延迟某些任务的执行，可以是用以下任务。在给定延迟时间后，任务会被排入队列，并且和发布 / 订阅任务一样执行。
            //BackgroundJob.Schedule(() => Console.WriteLine("Delayed"), TimeSpan.FromDays(1));

            //循环
            //按照周期性（小时，天等）来调用方法，请使用RecurringJob类。在复杂的场景，您可以使用CRON表达式指定计划时间来处理任务。
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Daily Job"), Cron.Daily);

            //连续
            //连续性允许您通过将多个后台任务链接在一起来定义复杂的工作流。
            //var id = BackgroundJob.Enqueue(() => Console.WriteLine("Hello, "));
            //BackgroundJob.ContinueWith(id, () => Console.WriteLine("world!"));

            //这里呢就是需要触发的方法 "0/10 * * * * ? " 可以自行搜索cron表达式 代表循环的规律很简单
            //CancelOrderJob代表你要触发的类 Execute代表你要触发的方法


            //自动取消订单任务
            RecurringJob.AddOrUpdate<AutoCancelOrderJob>(s => s.Execute(), "0 0/5 * * * ? ", TimeZoneInfo.Local); // 每5分钟取消一次订单

            //自动完成订单任务
            RecurringJob.AddOrUpdate<CompleteOrderJob>(s => s.Execute(), "0 0 0/1 * * ? ", TimeZoneInfo.Local); // 每小时自动完成订单

            //自动评价订单任务
            RecurringJob.AddOrUpdate<EvaluateOrderJob>(s => s.Execute(), "0 0 0/1 * * ? ", TimeZoneInfo.Local); // 每小时自动完成订单

            //自动签收订单任务
            RecurringJob.AddOrUpdate<AutoSignOrderJob>(s => s.Execute(), "0 0 0/1 * * ? ", TimeZoneInfo.Local); // 每小时自动完成订单

            //催付款订单
            RecurringJob.AddOrUpdate<RemindOrderPayJob>(s => s.Execute(), "0 0/5 * * * ? ", TimeZoneInfo.Local); // 每5分钟催付款订单

            //拼团自动取消到期团（每分钟执行一次）
            RecurringJob.AddOrUpdate<AutoCanclePinTuanJob>(s => s.Execute(), "0 0/2 * * * ? ", TimeZoneInfo.Local); // 每分钟取消一次订单

            //每天凌晨5点定期清理7天前操作日志
            RecurringJob.AddOrUpdate<RemoveOperationLogJob>(s => s.Execute(), "0 0 5 * * ? ", TimeZoneInfo.Local); // 每天5点固定时间清理一次

            //定时刷新获取微信AccessToken
            RecurringJob.AddOrUpdate<RefreshWeChatAccessTokenJob>(s => s.Execute(), "0 0/4 * * * ? ", TimeZoneInfo.Local); // 每2分钟刷新获取微信AccessToken

        }

        #endregion
    }
}