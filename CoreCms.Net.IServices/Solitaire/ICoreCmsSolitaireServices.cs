/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/6/14 23:17:57
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
    /// 接龙活动表 服务工厂接口
    /// </summary>
    public interface ICoreCmsSolitaireServices : IBaseServices<CoreCmsSolitaire>
    {
        #region 重写增删改查操作===========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> InsertAsync(CoreCmsSolitaire entity);

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(CoreCmsSolitaire entity);

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(List<CoreCmsSolitaire> entity);

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> DeleteByIdAsync(object id);

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids);

        #endregion


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
        new Task<IPageList<CoreCmsSolitaire>> QueryPageAsync(
            Expression<Func<CoreCmsSolitaire, bool>> predicate,
            Expression<Func<CoreCmsSolitaire, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);
        #endregion
    }
}
