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
using System.Linq;
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
    /// 分销商订单记录表 接口实现
    /// </summary>
    public class CoreCmsDistributionOrderRepository : BaseRepository<CoreCmsDistributionOrder>, ICoreCmsDistributionOrderRepository
    {
        public CoreCmsDistributionOrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsDistributionOrder entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync() > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsDistributionOrder entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await DbClient.Queryable<CoreCmsDistributionOrder>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.userId = entity.userId;
            oldModel.buyUserId = entity.buyUserId;
            oldModel.orderId = entity.orderId;
            oldModel.amount = entity.amount;
            oldModel.isSettlement = entity.isSettlement;
            oldModel.level = entity.level;
            oldModel.createTime = entity.createTime;
            oldModel.updateTime = entity.updateTime;
            oldModel.isDelete = entity.isDelete;

            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsDistributionOrder> entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsDistributionOrder>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsDistributionOrder>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

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
        public new async Task<IPageList<CoreCmsDistributionOrder>> QueryPageAsync(Expression<Func<CoreCmsDistributionOrder, bool>> predicate,
            Expression<Func<CoreCmsDistributionOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsDistributionOrder> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsDistributionOrder, CoreCmsOrder, CoreCmsUser, CoreCmsUser>((dOrder, cOrder, cUser, pUser) => new object[] {
                        JoinType.Inner,dOrder.orderId==cOrder.orderId,
                        JoinType.Inner,dOrder.buyUserId==cUser.id,
                        JoinType.Inner,dOrder.userId==pUser.id
                    })
                .Select((dOrder, cOrder, cUser, pUser) => new CoreCmsDistributionOrder
                {
                    id = dOrder.id,
                    userId = dOrder.userId,
                    buyUserId = dOrder.buyUserId,
                    orderId = dOrder.orderId,
                    amount = dOrder.amount,
                    isSettlement = dOrder.isSettlement,
                    level = dOrder.level,
                    createTime = dOrder.createTime,
                    updateTime = dOrder.updateTime,
                    isDelete = dOrder.isDelete,
                    buyUserNickName = cUser.nickName,
                    distributorName = pUser.nickName
                })
                .With(SqlWith.NoLock)
                .MergeTable()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsDistributionOrder, CoreCmsOrder, CoreCmsUser, CoreCmsUser>((dOrder, cOrder, cUser, pUser) => new object[] {
                        JoinType.Inner,dOrder.orderId==cOrder.orderId,
                        JoinType.Inner,dOrder.buyUserId==cUser.id,
                        JoinType.Inner,dOrder.userId==pUser.id
                    })
                    .Select((dOrder, cOrder, cUser, pUser) => new CoreCmsDistributionOrder
                    {
                        id = dOrder.id,
                        userId = dOrder.userId,
                        buyUserId = dOrder.buyUserId,
                        orderId = dOrder.orderId,
                        amount = dOrder.amount,
                        isSettlement = dOrder.isSettlement,
                        level = dOrder.level,
                        createTime = dOrder.createTime,
                        updateTime = dOrder.updateTime,
                        isDelete = dOrder.isDelete,
                        buyUserNickName = cUser.nickName,
                        distributorName = pUser.nickName
                    })
                .MergeTable()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsDistributionOrder>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion


        /// <summary>
        ///     获取下级推广订单数量
        /// </summary>
        /// <param name="parentId">父类序列</param>
        /// <param name="type">1获取1级，其他为2级,0为全部</param>
        /// <param name="thisMonth">显示当月</param>
        /// <returns></returns>
        public async Task<int> QueryChildOrderCountAsync(int parentId, int type = 1, bool thisMonth = false)
        {
            var totalSum = 0;

            DateTime dt = DateTime.Now;
            //本月第一天时间      
            DateTime dtFirst = dt.AddDays(1 - (dt.Day));
            dtFirst = new DateTime(dtFirst.Year, dtFirst.Month, dtFirst.Day, 0, 0, 0);

            //获得某年某月的天数    
            int dayCount = DateTime.DaysInMonth(dt.Date.Year, dt.Date.Month);
            //本月最后一天时间    
            DateTime dtLast = dtFirst.AddDays(dayCount - 1);

            if (type == 1)
            {
                totalSum = await DbClient.Queryable<CoreCmsDistributionOrder, CoreCmsUser>((orders, users) => new JoinQueryInfos(JoinType.Left, orders.buyUserId == users.id))
                    .WhereIF(thisMonth == true, (orders, users) => orders.createTime > dtFirst && orders.createTime < dtLast)
                    .Where((orders, users) => users.parentId == parentId)
                    .With(SqlWith.NoLock)
                    .CountAsync();

                return totalSum;
            }
            else if (type == 2)
            {
                //获取一级列表
                List<int> firstIds = await DbClient.Queryable<CoreCmsUser>().Where(p => p.parentId == parentId).Select(p => p.id).ToListAsync();
                if (firstIds.Count == 0)
                {
                    return totalSum;
                }
                var allIds = await DbClient.Queryable<CoreCmsUser>().Where(p => firstIds.Contains(p.parentId)).Select(p => p.id).ToListAsync();
                if (allIds.Count == 0)
                {
                    return totalSum;
                }
                totalSum = await DbClient.Queryable<CoreCmsDistributionOrder>()
                    .Where(p => allIds.Contains(p.buyUserId))
                    .WhereIF(thisMonth, p => p.createTime > dtFirst && p.createTime < dtLast)
                    .With(SqlWith.NoLock)
                    .CountAsync();

                return totalSum;

            }
            else
            {
                //获取一级列表
                List<int> firstIds = await DbClient.Queryable<CoreCmsUser>().Where(p => p.parentId == parentId).Select(p => p.id).ToListAsync();
                if (firstIds.Count == 0)
                {
                    return totalSum;
                }
                var allIds = await DbClient.Queryable<CoreCmsUser>().Where(p => firstIds.Contains(p.id) || firstIds.Contains(p.parentId)).Select(p => p.id).ToListAsync();
                if (allIds.Count == 0)
                {
                    return totalSum;
                }
                totalSum = await DbClient.Queryable<CoreCmsDistributionOrder>()
                    .Where(p => allIds.Contains(p.buyUserId))
                    .WhereIF(thisMonth, p => p.createTime > dtFirst && p.createTime < dtLast)
                    .With(SqlWith.NoLock)
                    .CountAsync();

                return totalSum;
            }
        }

        /// <summary>
        ///     获取下级推广订单金额
        /// </summary>
        /// <param name="parentId">父类序列</param>
        /// <param name="type">1获取1级，其他为2级，0为全部</param>
        /// <param name="thisMonth">显示当月</param>
        /// <returns></returns>
        public async Task<decimal> QueryChildOrderMoneySumAsync(int parentId, int type = 1, bool thisMonth = false)
        {
            decimal totalSum = 0;

            DateTime dt = DateTime.Now;
            //本月第一天时间      
            DateTime dtFirst = dt.AddDays(1 - (dt.Day));
            dtFirst = new DateTime(dtFirst.Year, dtFirst.Month, dtFirst.Day, 0, 0, 0);
            //获得某年某月的天数    
            int year = dt.Date.Year;
            int month = dt.Date.Month;
            int dayCount = DateTime.DaysInMonth(year, month);
            //本月最后一天时间    
            DateTime dtLast = dtFirst.AddDays(dayCount - 1);

            if (type == 1)
            {
                totalSum = await DbClient.Queryable<CoreCmsDistributionOrder, CoreCmsUser>((orders, users) =>
                        new JoinQueryInfos(JoinType.Left, orders.buyUserId == users.id))
                    .Where((orders, users) => users.parentId == parentId)
                    .WhereIF(thisMonth, (orders, users) => orders.createTime > dtFirst && orders.createTime < dtLast)
                    .With(SqlWith.NoLock)
                    .SumAsync((orders, users) => orders.amount);
                return totalSum;
            }
            else if (type == 2)
            {
                //获取一级列表
                List<int> firstIds = await DbClient.Queryable<CoreCmsUser>().Where(p => p.parentId == parentId).Select(p => p.id).ToListAsync();
                if (firstIds.Count == 0)
                {
                    return totalSum;
                }
                var allIds = await DbClient.Queryable<CoreCmsUser>().Where(p => firstIds.Contains(p.parentId)).Select(p => p.id).ToListAsync();
                if (allIds.Count == 0)
                {
                    return totalSum;
                }
                totalSum = await DbClient.Queryable<CoreCmsDistributionOrder, CoreCmsUser>((orders, users) => new JoinQueryInfos(JoinType.Left, orders.buyUserId == users.id))
                    .Where((orders, users) => allIds.Contains(orders.buyUserId))
                    .WhereIF(thisMonth, (orders, users) => orders.createTime > dtFirst && orders.createTime < dtLast)
                    .With(SqlWith.NoLock)
                    .SumAsync((orders, users) => orders.amount);
                return totalSum;

            }
            else
            {
                //获取全部
                List<int> firstIds = await DbClient.Queryable<CoreCmsUser>().Where(p => p.parentId == parentId).Select(p => p.id).ToListAsync();
                if (firstIds.Count == 0)
                {
                    return totalSum;
                }
                var allIds = await DbClient.Queryable<CoreCmsUser>().Where(p => firstIds.Contains(p.id) || firstIds.Contains(p.parentId)).Select(p => p.id).ToListAsync();
                if (allIds.Count == 0)
                {
                    return totalSum;
                }
                totalSum = await DbClient.Queryable<CoreCmsDistributionOrder, CoreCmsUser>((orders, users) => new JoinQueryInfos(JoinType.Left, orders.buyUserId == users.id))
                    .Where((orders, users) => allIds.Contains(orders.buyUserId))
                    .WhereIF(thisMonth, (orders, users) => orders.createTime > dtFirst && orders.createTime < dtLast)
                    .With(SqlWith.NoLock)
                    .SumAsync((orders, users) => orders.amount);
                return totalSum;
            }
        }


    }
}
