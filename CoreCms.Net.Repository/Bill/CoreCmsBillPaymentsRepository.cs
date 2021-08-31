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
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 支付单表 接口实现
    /// </summary>
    public class CoreCmsBillPaymentsRepository : BaseRepository<CoreCmsBillPayments>, ICoreCmsBillPaymentsRepository
    {

        public CoreCmsBillPaymentsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }


        /// <summary>
        /// 支付单7天统计
        /// </summary>
        /// <returns></returns>
        public async Task<List<StatisticsOut>> Statistics()
        {
            var dt = DateTime.Now.AddDays(-8).ToString("yyyy-MM-dd 00:00:00");

            var sqlStr = string.Empty;
            string dbTypeString = AppSettingsConstVars.DbDbType;
            if (dbTypeString == DbType.SqlServer.ToString())
            {
                sqlStr = @"SELECT  count(1) AS nums,CONVERT(varchar(100),createTime, 23)  AS day
                            FROM  CoreCmsBillPayments
                            WHERE createTime >= '" + dt + @"' AND status = " + (int)GlobalEnumVars.BillPaymentsStatus.Payed + @" AND type=" + (int)GlobalEnumVars.BillPaymentsType.Order + @" 
                            GROUP BY CONVERT(varchar(100),createTime, 23)";
            }
            else if (dbTypeString == DbType.MySql.ToString())
            {
                sqlStr = @"SELECT  count(1) AS nums,date(createTime) AS day
                            FROM  CoreCmsBillPayments
                            WHERE createTime >= '" + dt + @"' AND status = " + (int)GlobalEnumVars.BillPaymentsStatus.Payed + @" AND type=" + (int)GlobalEnumVars.BillPaymentsType.Order + @" 
                            GROUP BY date(createTime)";
            }

            if (string.IsNullOrEmpty(sqlStr))
            {
                return null;
            }

            var list = await DbClient.SqlQueryable<StatisticsOut>(sqlStr).ToListAsync();
            return list;
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
        public new async Task<IPageList<CoreCmsBillPayments>> QueryPageAsync(Expression<Func<CoreCmsBillPayments, bool>> predicate,
            Expression<Func<CoreCmsBillPayments, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsBillPayments> page;
            if (blUseNoLock)
                page = await DbClient.Queryable<CoreCmsBillPayments, CoreCmsUser>((p, sc) => new JoinQueryInfos(
                         JoinType.Left, p.userId == sc.id))
                    .Select((p, sc) => new CoreCmsBillPayments
                    {
                        paymentId = p.paymentId,
                        money = p.money,
                        userId = p.userId,
                        type = p.type,
                        status = p.status,
                        paymentCode = p.paymentCode,
                        ip = p.ip,
                        parameters = p.parameters,
                        payedMsg = p.payedMsg,
                        tradeNo = p.tradeNo,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        userNickName = sc.nickName
                    })
                    .MergeTable()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            else
                page = await DbClient.Queryable<CoreCmsBillPayments, CoreCmsUser>((p, sc) => new JoinQueryInfos(
                        JoinType.Left, p.userId == sc.id))
                    .Select((p, sc) => new CoreCmsBillPayments
                    {
                        paymentId = p.paymentId,
                        money = p.money,
                        userId = p.userId,
                        type = p.type,
                        status = p.status,
                        paymentCode = p.paymentCode,
                        ip = p.ip,
                        parameters = p.parameters,
                        payedMsg = p.payedMsg,
                        tradeNo = p.tradeNo,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        userNickName = sc.nickName
                    })
                    .MergeTable()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);
            var list = new PageList<CoreCmsBillPayments>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion


    }
}
