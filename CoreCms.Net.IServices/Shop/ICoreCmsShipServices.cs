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
    ///     配送方式表 服务工厂接口
    /// </summary>
    public interface ICoreCmsShipServices : IBaseServices<CoreCmsShip>
    {
        /// <summary>
        ///     设置是否默认
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> SetIsDefault(int id, bool isDefault);


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
        new Task<IPageList<CoreCmsShip>> QueryPageAsync(
            Expression<Func<CoreCmsShip, bool>> predicate,
            Expression<Func<CoreCmsShip, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        #endregion


        /// <summary>
        ///     获取配送费用
        /// </summary>
        /// <param name="areaId">地区id</param>
        /// <param name="weight">重量,单位g</param>
        /// <param name="totalmoney">商品总价</param>
        /// <returns></returns>
        decimal GetShipCost(int areaId = 0, decimal weight = 0, decimal totalmoney = 0);


        /// <summary>
        ///     计算运费
        /// </summary>
        /// <param name="ship">配送方式内容</param>
        /// <param name="weight">订单总重</param>
        /// <param name="totalmoney">商品总价</param>
        /// <param name="firstunitAreaPrice"></param>
        /// <returns></returns>
        decimal calculate_fee(CoreCmsShip ship, decimal weight, decimal totalmoney = 0, decimal firstunitAreaPrice = 0);

        /// <summary>
        ///     根据地区获取配送方式
        /// </summary>
        CoreCmsShip GetShip(int areaId = 0);

        #region 重写增删改查操作===========================================================

        /// <summary>
        ///     重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> InsertAsync(CoreCmsShip entity);

        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(CoreCmsShip entity);


        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DeleteByIdAsync(int id);

        #endregion
    }
}