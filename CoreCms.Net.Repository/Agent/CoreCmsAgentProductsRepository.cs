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
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 代理货品池 接口实现
    /// </summary>
    public class CoreCmsAgentProductsRepository : BaseRepository<CoreCmsAgentProducts>, ICoreCmsAgentProductsRepository
    {
        public CoreCmsAgentProductsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
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
        public new async Task<IPageList<CoreCmsAgentProducts>> QueryPageAsync(Expression<Func<CoreCmsAgentProducts, bool>> predicate,
            Expression<Func<CoreCmsAgentProducts, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsAgentProducts> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsAgentProducts>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsAgentProducts
                {
                    id = p.id,
                    goodId = p.goodId,
                    productId = p.productId,
                    productCostPrice = p.productCostPrice,
                    productPrice = p.productPrice,
                    agentGradeId = p.agentGradeId,
                    agentGradePrice = p.agentGradePrice,
                    createTime = p.createTime,
                    updateTime = p.updateTime,
                    isDel = p.isDel,

                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsAgentProducts>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsAgentProducts
                {
                    id = p.id,
                    goodId = p.goodId,
                    productId = p.productId,
                    productCostPrice = p.productCostPrice,
                    productPrice = p.productPrice,
                    agentGradeId = p.agentGradeId,
                    agentGradePrice = p.agentGradePrice,
                    createTime = p.createTime,
                    updateTime = p.updateTime,
                    isDel = p.isDel,

                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsAgentProducts>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
