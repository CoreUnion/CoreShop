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
    ///     服务消费券 接口实现
    /// </summary>
    public class CoreCmsUserServicesTicketRepository : BaseRepository<CoreCmsUserServicesTicket>,
        ICoreCmsUserServicesTicketRepository
    {
        public CoreCmsUserServicesTicketRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
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
        public new async Task<IPageList<CoreCmsUserServicesTicket>> QueryPageAsync(Expression<Func<CoreCmsUserServicesTicket, bool>> predicate,
            Expression<Func<CoreCmsUserServicesTicket, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsUserServicesTicket> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsUserServicesTicket>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsUserServicesTicket
                {
                    id = p.id,
                    serviceOrderId = p.serviceOrderId,
                    securityCode = p.securityCode,
                    redeemCode = p.redeemCode,
                    serviceId = p.serviceId,
                    userId = p.userId,
                    status = p.status,
                    validityType = p.validityType,
                    validityStartTime = p.validityStartTime,
                    validityEndTime = p.validityEndTime,
                    createTime = p.createTime,
                    isVerification = p.isVerification,
                    verificationTime = p.verificationTime,

                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsUserServicesTicket>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsUserServicesTicket
                {
                    id = p.id,
                    serviceOrderId = p.serviceOrderId,
                    securityCode = p.securityCode,
                    redeemCode = p.redeemCode,
                    serviceId = p.serviceId,
                    userId = p.userId,
                    status = p.status,
                    validityType = p.validityType,
                    validityStartTime = p.validityStartTime,
                    validityEndTime = p.validityEndTime,
                    createTime = p.createTime,
                    isVerification = p.isVerification,
                    verificationTime = p.verificationTime,

                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsUserServicesTicket>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion


    }
}