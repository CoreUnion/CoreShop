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

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     拼团记录表 服务工厂接口
    /// </summary>
    public interface ICoreCmsPinTuanRecordServices : IBaseServices<CoreCmsPinTuanRecord>
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
        ///     取得商品的所有拼团记录
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="goodsId"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetRecord(int ruleId, int goodsId, int status = 0);


        /// <summary>
        ///     获取拼团团队人数
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetTeamList(int teamId, string orderId);


        /// <summary>
        ///     自动取消到时间的团（定时任务用）
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> AutoCanclePinTuanOrder();


        #region 重写根据条件查询分页数据

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

        #endregion
    }
}