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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     用户余额表 服务工厂接口
    /// </summary>
    public interface ICoreCmsUserBalanceServices : IBaseServices<CoreCmsUserBalance>
    {
        /// <summary>
        ///     余额变动记录
        /// </summary>
        /// <param name="userId">当前用户id,当是店铺的时候，取店铺创始人的userId</param>
        /// <param name="type">类型</param>
        /// <param name="money">金额，永远是正的</param>
        /// <param name="sourceId">资源id</param>
        /// <param name="cateMoney">服务费金额 (提现)</param>
        /// <returns></returns>
        Task<WebApiCallBack> Change(int userId, int type, decimal money, string sourceId = "", decimal cateMoney = 0);


        /// <summary>
        ///     获取用户的邀请佣金
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<decimal> GetInviteCommission(int userId);


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
        new Task<IPageList<CoreCmsUserBalance>> QueryPageAsync(
            Expression<Func<CoreCmsUserBalance, bool>> predicate,
            Expression<Func<CoreCmsUserBalance, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        #endregion
    }
}