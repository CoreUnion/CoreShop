/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 发货单表 接口实现
    /// </summary>
    public class CoreCmsBillDeliveryRepository : BaseRepository<CoreCmsBillDelivery>, ICoreCmsBillDeliveryRepository
    {
        public CoreCmsBillDeliveryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 发货单统计7天统计
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatisticsOut>> Statistics()
        {
            var dt = DateTime.Now.AddDays(-8);

            var list = await DbClient.Queryable<CoreCmsBillDelivery>()
                .Where(p => p.createTime >= dt)
                .Select(it => new
                {
                    it.deliveryId,
                    createTime = it.createTime.Date
                })
                .MergeTable()
                .GroupBy(it => it.createTime)
                .Select(it => new StatisticsOut { day = it.createTime.ToString("yyyy-MM-dd"), nums = SqlFunc.AggregateCount(it.deliveryId) })
                .ToListAsync();

            return list;
        }

    }
}
