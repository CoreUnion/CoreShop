using Hangfire.States;
using Hangfire.Storage;
using System;

namespace CoreCms.Net.Web.WebApi.Infrastructure
{
    /// <summary>
    /// 已完成的job设置过期，防止数据无限增长
    /// </summary>
    public class SucceededStateExpireHandler : IStateHandler
    {
        /// <summary>
        /// 数据过期时间
        /// </summary>
        public TimeSpan JobExpirationTimeout;

        /// <summary>
        /// 完成的项目过期状态处理
        /// </summary>
        /// <param name="jobExpirationTimeout"></param>
        public SucceededStateExpireHandler(int jobExpirationTimeout)
        {
            JobExpirationTimeout = TimeSpan.FromMinutes(jobExpirationTimeout);
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StateName => SucceededState.StateName;

        /// <summary>
        /// 应用状态
        /// </summary>
        /// <param name="context"></param>
        /// <param name="transaction"></param>
        public void Apply(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = JobExpirationTimeout;
        }

        /// <summary>
        /// 不应用状态
        /// </summary>
        /// <param name="context"></param>
        /// <param name="transaction"></param>
        public void Unapply(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }

}
