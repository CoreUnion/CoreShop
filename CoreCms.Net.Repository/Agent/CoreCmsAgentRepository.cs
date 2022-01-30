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
using CoreCms.Net.Model.ViewModels.DTO.Agent;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 代理商表 接口实现
    /// </summary>
    public class CoreCmsAgentRepository : BaseRepository<CoreCmsAgent>, ICoreCmsAgentRepository
    {
        public CoreCmsAgentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
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
        public new async Task<IPageList<CoreCmsAgent>> QueryPageAsync(Expression<Func<CoreCmsAgent, bool>> predicate,
            Expression<Func<CoreCmsAgent, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsAgent> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsAgent>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsAgent
                {
                    id = p.id,
                    userId = p.userId,
                    name = p.name,
                    gradeId = p.gradeId,
                    mobile = p.mobile,
                    weixin = p.weixin,
                    qq = p.qq,
                    storeName = p.storeName,
                    storeLogo = p.storeLogo,
                    storeBanner = p.storeBanner,
                    storeDesc = p.storeDesc,
                    verifyStatus = p.verifyStatus,
                    createTime = p.createTime,
                    updateTime = p.updateTime,
                    verifyTime = p.verifyTime,
                    isDelete = p.isDelete,
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsAgent>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsAgent
                {
                    id = p.id,
                    userId = p.userId,
                    name = p.name,
                    gradeId = p.gradeId,
                    mobile = p.mobile,
                    weixin = p.weixin,
                    qq = p.qq,
                    storeName = p.storeName,
                    storeLogo = p.storeLogo,
                    storeBanner = p.storeBanner,
                    storeDesc = p.storeDesc,
                    verifyStatus = p.verifyStatus,
                    createTime = p.createTime,
                    updateTime = p.updateTime,
                    verifyTime = p.verifyTime,
                    isDelete = p.isDelete,

                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsAgent>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion



        #region 根据条件查询分页数据

        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsAgentOrder>> QueryOrderPageAsync(int userId, int pageIndex = 1, int pageSize = 20, int typeId = 0)
        {
            RefAsync<int> totalCount = 0;
            var page = await DbClient.Queryable<CoreCmsAgentOrder, CoreCmsOrder, CoreCmsUser>((dOrder, cOrder, userInfo) => new object[] {
                    JoinType.Inner,dOrder.orderId==cOrder.orderId,JoinType.Inner,dOrder.buyUserId==userInfo.id
                })
                .Where((dOrder, cOrder, userInfo) => dOrder.userId == userId)
                .Select((dOrder, cOrder, userInfo) => new CoreCmsAgentOrder()
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
                    buyUserNickName = userInfo.nickName
                })
                .With(SqlWith.NoLock)
                .MergeTable()
                .WhereIF(typeId > 0, p => p.isSettlement == typeId)
                .OrderBy(dOrder => dOrder.id, OrderByType.Desc)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            var list = new PageList<CoreCmsAgentOrder>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion


        #region 获取代理商排行

        /// <summary>
        ///     获取代理商排行
        /// </summary>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        public async Task<IPageList<AgentRankingDTO>> QueryRankingPageAsync(int pageIndex = 1, int pageSize = 20)
        {

            RefAsync<int> totalCount = 0;
            var page = await DbClient.Queryable<CoreCmsAgent>()
                .Select(p => new AgentRankingDTO()
                {
                    id = p.userId,
                    nickname = p.name,
                    createtime = p.createTime,
                    totalInCome = SqlFunc.Subqueryable<CoreCmsAgentOrder>().Where(o => o.userId == p.userId && p.isDelete == false && p.verifyStatus == (int)GlobalEnumVars.AgentOrderSettlementStatus.SettlementYes).Sum(o => o.amount),
                    orderCount = SqlFunc.Subqueryable<CoreCmsAgentOrder>().Where(o => o.userId == p.userId && p.isDelete == false && p.verifyStatus == (int)GlobalEnumVars.AgentOrderSettlementStatus.SettlementYes).Count()
                })
                .With(SqlWith.NoLock)
                .MergeTable()
                .OrderBy(dOrder => dOrder.totalInCome, OrderByType.Desc)
                .ToPageListAsync(pageIndex, pageSize, totalCount);

            var list = new PageList<AgentRankingDTO>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion




    }
}
