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
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using SqlSugar;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     代理货品池 接口实现
    /// </summary>
    public class CoreCmsAgentProductsServices : BaseServices<CoreCmsAgentProducts>, ICoreCmsAgentProductsServices
    {
        private readonly ICoreCmsAgentProductsRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public CoreCmsAgentProductsServices(IUnitOfWork unitOfWork, ICoreCmsAgentProductsRepository dal)
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
        public new async Task<IPageList<CoreCmsAgentProducts>> QueryPageAsync(
            Expression<Func<CoreCmsAgentProducts, bool>> predicate,
            Expression<Func<CoreCmsAgentProducts, object>> orderByExpression, OrderByType orderByType,
            int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize,
                blUseNoLock);
        }

        #endregion
    }
}