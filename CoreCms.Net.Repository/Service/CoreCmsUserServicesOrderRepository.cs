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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    ///     服务购买表 接口实现
    /// </summary>
    public class CoreCmsUserServicesOrderRepository : BaseRepository<CoreCmsUserServicesOrder>,
        ICoreCmsUserServicesOrderRepository
    {
        public CoreCmsUserServicesOrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
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
        public new async Task<IPageList<CoreCmsUserServicesOrder>> QueryPageAsync(Expression<Func<CoreCmsUserServicesOrder, bool>> predicate,
            Expression<Func<CoreCmsUserServicesOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsUserServicesOrder> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsUserServicesOrder, CoreCmsUser, CoreCmsServices>((sOrder, sUser, sService) => new JoinQueryInfos(
                         JoinType.Left, sOrder.userId == sUser.id, JoinType.Left, sOrder.servicesId == sService.id))
                .Select((sOrder, sUser, sService) => new CoreCmsUserServicesOrder
                {
                    id = sOrder.id,
                    serviceOrderId = sOrder.serviceOrderId,
                    userId = sOrder.userId,
                    servicesId = sOrder.servicesId,
                    isPay = sOrder.isPay,
                    payTime = sOrder.payTime,
                    paymentId = sOrder.paymentId,
                    status = sOrder.status,
                    createTime = sOrder.createTime,
                    servicesEndTime = sOrder.servicesEndTime,
                    serviceTitle = sService.title,
                    userName = sUser.nickName
                })
                    .With(SqlWith.NoLock)
                    .MergeTable()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsUserServicesOrder, CoreCmsUser, CoreCmsServices>((sOrder, sUser, sService) => new JoinQueryInfos(
                        JoinType.Left, sOrder.userId == sUser.id, JoinType.Left, sOrder.servicesId == sService.id))
                    .Select((sOrder, sUser, sService) => new CoreCmsUserServicesOrder
                    {
                        id = sOrder.id,
                        serviceOrderId = sOrder.serviceOrderId,
                        userId = sOrder.userId,
                        servicesId = sOrder.servicesId,
                        isPay = sOrder.isPay,
                        payTime = sOrder.payTime,
                        paymentId = sOrder.paymentId,
                        status = sOrder.status,
                        createTime = sOrder.createTime,
                        servicesEndTime = sOrder.servicesEndTime,
                        serviceTitle = sService.title,
                        userName = sUser.nickName
                    })
                    .MergeTable()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsUserServicesOrder>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion


    }
}