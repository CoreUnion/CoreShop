/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     发货单表 工厂接口
    /// </summary>
    public interface ICoreCmsBillDeliveryRepository : IBaseRepository<CoreCmsBillDelivery>
    {
        /// <summary>
        ///     发货单统计7天统计
        /// </summary>
        /// <returns></returns>
        Task<List<StatisticsOut>> Statistics();
    }
}