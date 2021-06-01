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
    ///     退货单表 服务工厂接口
    /// </summary>
    public interface ICoreCmsBillReshipServices : IBaseServices<CoreCmsBillReship>
    {
        /// <summary>
        ///     创建退货单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="aftersalesId"></param>
        /// <param name="aftersalesItems"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ToAdd(int userId, string orderId, string aftersalesId,
            List<CoreCmsBillAftersalesItem> aftersalesItems);


        /// <summary>
        ///     获取单个数据带导航
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        Task<CoreCmsBillReship> GetDetails(Expression<Func<CoreCmsBillReship, bool>> predicate,
            Expression<Func<CoreCmsBillReship, object>> orderByExpression, OrderByType orderByType);


        #region 重写根据条件查询分页数据

        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        Task<IPageList<CoreCmsBillReship>> QueryPageAsync(
            Expression<Func<CoreCmsBillReship, bool>> predicate,
            Expression<Func<CoreCmsBillReship, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20);

        #endregion
    }
}