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
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     代理商品池 接口实现
    /// </summary>
    public class CoreCmsAgentGoodsServices : BaseServices<CoreCmsAgentGoods>, ICoreCmsAgentGoodsServices
    {
        private readonly ICoreCmsAgentGoodsRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public CoreCmsAgentGoodsServices(IUnitOfWork unitOfWork, ICoreCmsAgentGoodsRepository dal)
        {
            _dal = dal;
            BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


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
        public new async Task<IPageList<CoreCmsAgentGoods>> QueryPageAsync(
            Expression<Func<CoreCmsAgentGoods, bool>> predicate,
            Expression<Func<CoreCmsAgentGoods, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize,
                blUseNoLock);
        }

        #endregion

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        ///     重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <param name="products"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> InsertAsync(CoreCmsAgentGoods entity, List<CoreCmsAgentProducts> products)
        {
            return await _dal.InsertAsync(entity, products);
        }

        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateAsync(CoreCmsAgentGoods entity, List<CoreCmsAgentProducts> products)
        {
            return await _dal.UpdateAsync(entity, products);
        }

        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsAgentGoods> entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> DeleteByIdAsync(int id)
        {
            return await _dal.DeleteByIdAsync(id);
        }

        /// <summary>
        ///     重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            return await _dal.DeleteByIdsAsync(ids);
        }

        #endregion
    }
}