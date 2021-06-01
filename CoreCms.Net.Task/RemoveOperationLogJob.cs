/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-08-25 1:25:29
 *        Description: 暂无
 ***********************************************************************/


using System;
using CoreCms.Net.IRepository;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using Newtonsoft.Json;


namespace CoreCms.Net.Task
{
    /// <summary>
    /// 定期清理7天前操作日志任务
    /// </summary>
    public class RemoveOperationLogJob
    {
        private readonly ISysTaskLogServices _taskLogServices;
        private readonly ISysNLogRecordsServices _nLogRecordsServices;
        private readonly ICoreCmsGoodsBrowsingServices _browsingServices;

        public RemoveOperationLogJob(ISysTaskLogServices taskLogServices, ISysNLogRecordsServices nLogRecordsServices, ICoreCmsGoodsBrowsingServices browsingServices)
        {
            _taskLogServices = taskLogServices;
            _nLogRecordsServices = nLogRecordsServices;
            _browsingServices = browsingServices;
        }

        public async System.Threading.Tasks.Task Execute()
        {
            var dt = DateTime.Now.AddDays(-7);
            var dt2 = DateTime.Now.AddDays(-7);
            var dt3 = DateTime.Now.AddDays(-7);
            //清理7天前的Nlog记录
            await _nLogRecordsServices.DeleteAsync(p => p.LogDate <= dt);
            //清理7天前的定时任务记录
            await _taskLogServices.DeleteAsync(p => p.createTime <= dt2);
            //清理7天前的用户足迹
            await _browsingServices.DeleteAsync(p => p.createTime <= dt3);
        }
    }
}
