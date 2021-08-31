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
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Helper;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 退货单表 接口实现
    /// </summary>
    public class CoreCmsBillAftersalesRepository : BaseRepository<CoreCmsBillAftersales>, ICoreCmsBillAftersalesRepository
    {
        public CoreCmsBillAftersalesRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 根据条件查询分页数据
        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsBillAftersales>> QueryPageAsync(Expression<Func<CoreCmsBillAftersales, bool>> predicate,
            Expression<Func<CoreCmsBillAftersales, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            RefAsync<int> totalCount = 0;
            var page = await DbClient.Queryable<CoreCmsBillAftersales, CoreCmsUser>((p, sc) => new JoinQueryInfos(
                     JoinType.Left, p.userId == sc.id))
                .Select((p, sc) => new CoreCmsBillAftersales
                {
                    aftersalesId = p.aftersalesId,
                    orderId = p.orderId,
                    userId = p.userId,
                    type = p.type,
                    refundAmount = p.refundAmount,
                    status = p.status,
                    reason = p.reason,
                    mark = p.mark,
                    createTime = p.createTime,
                    updateTime = p.updateTime,
                    userNickName = sc.nickName
                })
                .MergeTable()
                .Mapper(p => p.items, p => p.items.First().aftersalesId)
                .Mapper(p => p.images, p => p.images.First().aftersalesId)
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .ToPageListAsync(pageIndex, pageSize, totalCount);

            if (page.Any())
            {
                var billAftersalesStatus = EnumHelper.EnumToList<GlobalEnumVars.BillAftersalesStatus>();
                foreach (var item in page)
                {
                    var statusModel = billAftersalesStatus.Find(p => p.value == item.status);
                    if (statusModel != null) item.statusName = statusModel.description;
                }
            }


            var list = new PageList<CoreCmsBillAftersales>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

        #region 获取单个数据

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="aftersalesId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CoreCmsBillAftersales> GetInfo(string aftersalesId, int userId = 0)
        {
            var model = userId > 0
                ? await DbClient.Queryable<CoreCmsBillAftersales>()
                    .Where(p => p.aftersalesId == aftersalesId && p.userId == userId).FirstAsync()
                : await DbClient.Queryable<CoreCmsBillAftersales>()
                    .Where(p => p.aftersalesId == aftersalesId).FirstAsync();

            if (model != null)
            {
                model.order = await DbClient.Queryable<CoreCmsOrder>().Where(p => p.orderId == model.orderId).FirstAsync();
                model.images = await DbClient.Queryable<CoreCmsBillAftersalesImages>().Where(p => p.aftersalesId == aftersalesId).OrderBy(p => p.sortId).ToListAsync();
                model.items = await DbClient.Queryable<CoreCmsBillAftersalesItem>().Where(p => p.aftersalesId == aftersalesId).OrderBy(p => p.createTime).ToListAsync();
                model.billRefund = await DbClient.Queryable<CoreCmsBillRefund>().Where(p => p.aftersalesId == aftersalesId).FirstAsync();
                model.billReship = await DbClient.Queryable<CoreCmsBillReship>().Where(p => p.aftersalesId == aftersalesId).FirstAsync();
            }
            return model;
        }
        #endregion


    }
}
