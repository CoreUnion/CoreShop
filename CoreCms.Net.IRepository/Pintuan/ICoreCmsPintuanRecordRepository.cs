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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     拼团记录表 工厂接口
    /// </summary>
    public interface ICoreCmsPinTuanRecordRepository : IBaseRepository<CoreCmsPinTuanRecord>
    {
        /// <summary>
        ///     生成订单的时候，增加信息
        /// </summary>
        /// <param name="order">订单数据</param>
        /// <param name="items">货品列表</param>
        /// <param name="teamId">团队序列</param>
        /// <returns></returns>
        Task<WebApiCallBack> OrderAdd(CoreCmsOrder order, List<CoreCmsOrderItem> items, int teamId = 0);

        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        new Task<IPageList<CoreCmsPinTuanRecord>> QueryPageAsync(
            Expression<Func<CoreCmsPinTuanRecord, bool>> predicate,
            Expression<Func<CoreCmsPinTuanRecord, object>> orderByExpression, OrderByType orderByType,
            int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);
    }
}