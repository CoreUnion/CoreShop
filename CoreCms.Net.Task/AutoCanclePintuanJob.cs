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
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using Newtonsoft.Json;

namespace CoreCms.Net.Task
{
    /// <summary>
    /// 拼团自动取消到期团
    /// </summary>
    public class AutoCanclePinTuanJob
    {
        private readonly ICoreCmsPinTuanRecordServices _pinTuanRecordServices;


        public AutoCanclePinTuanJob(ICoreCmsPinTuanRecordServices pinTuanRecordServices)
        {
            _pinTuanRecordServices = pinTuanRecordServices;
        }

        public async System.Threading.Tasks.Task Execute()
        {
            await _pinTuanRecordServices.AutoCanclePinTuanOrder();
        }
    }
}
