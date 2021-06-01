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
    ///     用户提现记录表 服务工厂接口
    /// </summary>
    public interface ICoreCmsUserTocashServices : IBaseServices<CoreCmsUserTocash>
    {
        /// <summary>
        ///     提现申请
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> Tocash(int userId, decimal money, int bankCardsId);

        /// <summary>
        ///     获取用户提现列表记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<WebApiCallBack> UserToCashList(int userId = 0, int page = 1, int limit = 10, int status = 0);


        /// <summary>
        ///     提现审核
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<WebApiCallBack> Examine(int id = 0, int status = 0);


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
        new Task<IPageList<CoreCmsUserTocash>> QueryPageAsync(
            Expression<Func<CoreCmsUserTocash, bool>> predicate,
            Expression<Func<CoreCmsUserTocash, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        #endregion
    }
}