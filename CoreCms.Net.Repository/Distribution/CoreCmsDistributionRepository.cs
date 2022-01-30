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
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.ViewModels.DTO.Agent;
using CoreCms.Net.Model.ViewModels.DTO.Distribution;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 分销商表 接口实现
    /// </summary>
    public class CoreCmsDistributionRepository : BaseRepository<CoreCmsDistribution>, ICoreCmsDistributionRepository
    {
        public CoreCmsDistributionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        #region 根据条件查询分页数据

        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="typeId">类型</param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsDistributionOrder>> QueryOrderPageAsync(int userId, int pageIndex = 1, int pageSize = 20, int typeId = 0)
        {
            RefAsync<int> totalCount = 0;
            var page = await DbClient.Queryable<CoreCmsDistributionOrder, CoreCmsOrder, CoreCmsUser>((dOrder, cOrder, userInfo) => new object[] {
                    JoinType.Inner,dOrder.orderId==cOrder.orderId,JoinType.Inner,dOrder.buyUserId==userInfo.id
                })
                .Where((dOrder, cOrder, userInfo) => dOrder.userId == userId)
                .Select((dOrder, cOrder, userInfo) => new CoreCmsDistributionOrder()
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
                    buyUserNickName = userInfo.nickName
                })
                .With(SqlWith.NoLock)
                .MergeTable()
                .WhereIF(typeId > 0, p => p.isSettlement == typeId)
                .OrderBy(dOrder => dOrder.id, OrderByType.Desc)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            var list = new PageList<CoreCmsDistributionOrder>(page, pageIndex, pageSize, totalCount);
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
        public async Task<IPageList<DistributionRankingDTO>> QueryRankingPageAsync(int pageIndex = 1, int pageSize = 20)
        {

            RefAsync<int> totalCount = 0;
            var page = await DbClient.Queryable<CoreCmsDistribution>()
                .Select(p => new DistributionRankingDTO()
                {
                    id = p.userId,
                    nickname = p.name,
                    createtime = p.createTime,
                    totalInCome = SqlFunc.Subqueryable<CoreCmsDistributionOrder>().Where(o => o.userId == p.userId && p.isDelete == false && p.verifyStatus == (int)GlobalEnumVars.DistributionOrderSettlementStatus.SettlementYes).Sum(o => o.amount),
                    orderCount = SqlFunc.Subqueryable<CoreCmsDistributionOrder>().Where(o => o.userId == p.userId && p.isDelete == false && p.verifyStatus == (int)GlobalEnumVars.DistributionOrderSettlementStatus.SettlementYes).Count()
                })
                .With(SqlWith.NoLock)
                .MergeTable()
                .OrderBy(dOrder => dOrder.totalInCome, OrderByType.Desc)
                .ToPageListAsync(pageIndex, pageSize, totalCount);

            var list = new PageList<DistributionRankingDTO>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion



    }
}
