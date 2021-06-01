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
    /// 退货单表 接口实现
    /// </summary>
    public class CoreCmsBillReshipRepository : BaseRepository<CoreCmsBillReship>, ICoreCmsBillReshipRepository
    {
        public CoreCmsBillReshipRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 获取单个数据带导航
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public async Task<CoreCmsBillReship> GetDetails(Expression<Func<CoreCmsBillReship, bool>> predicate,
            Expression<Func<CoreCmsBillReship, object>> orderByExpression, OrderByType orderByType)
        {
            var model = await DbClient.Queryable<CoreCmsBillReship, CoreCmsUser>((p, sc) => new JoinQueryInfos(
                    JoinType.Left, p.userId == sc.id))
                .Select((p, sc) => new CoreCmsBillReship
                {
                    reshipId = p.reshipId,
                    orderId = p.orderId,
                    aftersalesId = p.aftersalesId,
                    userId = p.userId,
                    logiCode = p.logiCode,
                    logiNo = p.logiNo,
                    status = p.status,
                    memo = p.memo,
                    createTime = p.createTime,
                    updateTime = p.updateTime,
                    userNickName = sc.nickName
                })
                .MergeTable()
                .Mapper(p => p.items, p => p.reshipId)
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .FirstAsync();

            return model;
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
        /// <returns></returns>
        public async Task<IPageList<CoreCmsBillReship>> QueryPageAsync(Expression<Func<CoreCmsBillReship, bool>> predicate,
            Expression<Func<CoreCmsBillReship, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsBillReship> page = await DbClient.Queryable<CoreCmsBillReship, CoreCmsUser>((p, sc) => new JoinQueryInfos(
                     JoinType.Left, p.userId == sc.id))
               .Select((p, sc) => new CoreCmsBillReship
               {
                   reshipId = p.reshipId,
                   orderId = p.orderId,
                   aftersalesId = p.aftersalesId,
                   userId = p.userId,
                   logiCode = p.logiCode,
                   logiNo = p.logiNo,
                   status = p.status,
                   memo = p.memo,
                   createTime = p.createTime,
                   updateTime = p.updateTime,
                   userNickName = sc.nickName
               })
               .MergeTable()
               .Mapper(p => p.items, p => p.reshipId)
               .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
               .WhereIF(predicate != null, predicate)
                .ToPageListAsync(pageIndex, pageSize, totalCount);

            var list = new PageList<CoreCmsBillReship>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
