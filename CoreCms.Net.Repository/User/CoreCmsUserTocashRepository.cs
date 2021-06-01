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
    ///     用户提现记录表 接口实现
    /// </summary>
    public class CoreCmsUserTocashRepository : BaseRepository<CoreCmsUserTocash>, ICoreCmsUserTocashRepository
    {
        public CoreCmsUserTocashRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
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
        public new async Task<IPageList<CoreCmsUserTocash>> QueryPageAsync(
            Expression<Func<CoreCmsUserTocash, bool>> predicate,
            Expression<Func<CoreCmsUserTocash, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsUserTocash> page;
            if (blUseNoLock)
                page = await DbClient.Queryable<CoreCmsUserTocash, CoreCmsUser>((p, sc) => new JoinQueryInfos(
                        JoinType.Left, p.userId == sc.id))
                    .Select((p, sc) => new CoreCmsUserTocash
                    {
                        id = p.id,
                        userId = p.userId,
                        money = p.money,
                        bankName = p.bankName,
                        bankCode = p.bankCode,
                        bankAreaId = p.bankAreaId,
                        accountBank = p.accountBank,
                        accountName = p.accountName,
                        cardNumber = p.cardNumber,
                        withdrawals = p.withdrawals,
                        status = p.status,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        userNickName = sc.nickName
                    })
                    .MergeTable()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            else
                page = await DbClient.Queryable<CoreCmsUserTocash, CoreCmsUser>((p, sc) => new JoinQueryInfos(
                        JoinType.Left, p.userId == sc.id))
                    .Select((p, sc) => new CoreCmsUserTocash
                    {
                        id = p.id,
                        userId = p.userId,
                        money = p.money,
                        bankName = p.bankName,
                        bankCode = p.bankCode,
                        bankAreaId = p.bankAreaId,
                        accountBank = p.accountBank,
                        accountName = p.accountName,
                        cardNumber = p.cardNumber,
                        withdrawals = p.withdrawals,
                        status = p.status,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        userNickName = sc.nickName
                    })
                    .MergeTable()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);


            var list = new PageList<CoreCmsUserTocash>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion
    }
}