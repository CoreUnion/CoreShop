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
    /// 代理商订单记录表 接口实现
    /// </summary>
    public class CoreCmsAgentOrderRepository : BaseRepository<CoreCmsAgentOrder>, ICoreCmsAgentOrderRepository
    {
        public CoreCmsAgentOrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsAgentOrder entity)
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
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsAgentOrder entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await DbClient.Queryable<CoreCmsAgentOrder>().In(entity.id).SingleAsync();
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
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsAgentOrder> entity)
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

            var bl = await DbClient.Deleteable<CoreCmsAgentOrder>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<CoreCmsAgentOrder>().In(ids).ExecuteCommandHasChangeAsync();
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
        public new async Task<IPageList<CoreCmsAgentOrder>> QueryPageAsync(Expression<Func<CoreCmsAgentOrder, bool>> predicate,
            Expression<Func<CoreCmsAgentOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsAgentOrder> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsAgentOrder, CoreCmsOrder, CoreCmsUser, CoreCmsUser>((dOrder, cOrder, cUser, pUser) => new object[] {
                        JoinType.Inner,dOrder.orderId==cOrder.orderId,
                        JoinType.Inner,dOrder.buyUserId==cUser.id,
                        JoinType.Inner,dOrder.userId==pUser.id
                    })
                .Select((dOrder, cOrder, cUser, pUser) => new CoreCmsAgentOrder
                {
                    id = dOrder.id,
                    userId = dOrder.userId,
                    buyUserId = dOrder.buyUserId,
                    orderId = dOrder.orderId,
                    amount = dOrder.amount,
                    isSettlement = dOrder.isSettlement,
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
                page = await DbClient.Queryable<CoreCmsAgentOrder, CoreCmsOrder, CoreCmsUser, CoreCmsUser>((dOrder, cOrder, cUser, pUser) => new object[] {
                        JoinType.Inner,dOrder.orderId==cOrder.orderId,
                        JoinType.Inner,dOrder.buyUserId==cUser.id,
                        JoinType.Inner,dOrder.userId==pUser.id
                    })
                    .Select((dOrder, cOrder, cUser, pUser) => new CoreCmsAgentOrder
                    {
                        id = dOrder.id,
                        userId = dOrder.userId,
                        buyUserId = dOrder.buyUserId,
                        orderId = dOrder.orderId,
                        amount = dOrder.amount,
                        isSettlement = dOrder.isSettlement,
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
            var list = new PageList<CoreCmsAgentOrder>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion


    }
}
