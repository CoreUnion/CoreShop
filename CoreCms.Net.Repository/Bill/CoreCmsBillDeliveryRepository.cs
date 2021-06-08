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
using CoreCms.Net.Model.ViewModels.View;
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

        #region 获取发货单列表
        /// <summary>
        /// 获取发货单列表
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetDeliveryList(string orderId)
        {
            var jm = new WebApiCallBack();

            var list = await DbClient.Queryable<CoreCmsBillDelivery, CoreCmsBillDeliveryOrderRel>((d, dor) => new object[]
             {
                    JoinType.Inner, d.deliveryId == dor.deliveryId
             }).Where((d, dor) => dor.orderId == orderId).ToListAsync();
            jm.status = true;
            jm.data = list;
            jm.msg = jm.status ? GlobalConstVars.GetDataSuccess : GlobalConstVars.GetDataFailure;

            return jm;
        }

        #endregion

        /// <summary>
        /// 发货单统计7天统计
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatisticsOut>> Statistics()
        {
            var dt = DateTime.Now.AddDays(-8).ToString("yyyy-MM-dd 00:00:00");

            var sqlStr = string.Empty;
            string dbTypeString = AppSettingsConstVars.DbDbType;
            if (dbTypeString == DbType.SqlServer.ToString())
            {
                sqlStr = @"SELECT  count(1) AS nums,CONVERT(varchar(100),createTime, 23)  AS day
                            FROM  CoreCmsBillDelivery
                            WHERE createTime >= '" + dt + @"'  
                            GROUP BY CONVERT(varchar(100),createTime, 23)";
            }
            else if (dbTypeString == DbType.MySql.ToString())
            {
                sqlStr = @"SELECT  count(1) AS nums,date(createTime)  AS day
                            FROM  CoreCmsBillDelivery
                            WHERE createTime >= '" + dt + @"'  
                            GROUP BY date(createTime)";
            }

            if (string.IsNullOrEmpty(sqlStr))
            {
                return null;
            }
            return await DbClient.SqlQueryable<StatisticsOut>(sqlStr).ToListAsync();
        }

    }
}
