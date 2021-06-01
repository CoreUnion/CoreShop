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
using CoreCms.Net.Utility.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 拼团记录表 接口实现
    /// </summary>
    public class CoreCmsPinTuanRecordRepository : BaseRepository<CoreCmsPinTuanRecord>, ICoreCmsPinTuanRecordRepository
    {
        public CoreCmsPinTuanRecordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// 生成订单的时候，增加信息
        /// </summary>
        /// <param name="order">订单数据</param>
        /// <param name="items">商品列表</param>
        /// <param name="teamId">团队序列</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> OrderAdd(CoreCmsOrder order, List<CoreCmsOrderItem> items, int teamId = 0)
        {
            var res = new WebApiCallBack() { methodDescription = "拼团生成订单的时候，增加信息" };

            var orderItem = items.FirstOrDefault();

            if (teamId > 0)
            {
                //参加别人拼团
                var info = await DbClient.Queryable<CoreCmsPinTuanRecord>().FirstAsync(p => p.id == teamId && p.closeTime > DateTime.Now &&
                    p.status == (int)GlobalEnumVars.PinTuanRecordStatus.InProgress);
                if (info == null)
                {
                    res.status = false;
                    res.data = 15607;
                    res.msg = GlobalErrorCodeVars.Code15607;
                    return res;
                }
                //判断订单商品是否是要参加的拼团的商品
                if (info.goodsId != orderItem.goodsId)
                {
                    res.status = false;
                    res.data = 15608;
                    res.msg = GlobalErrorCodeVars.Code15608;
                    return res;
                }

                var model = new CoreCmsPinTuanRecord();
                model.teamId = teamId;
                model.userId = order.userId;
                model.ruleId = info.ruleId;
                model.status = (int)GlobalEnumVars.PinTuanRecordStatus.InProgress;
                model.orderId = order.orderId;
                model.goodsId = (int)orderItem.goodsId;


                await DbClient.Insertable(model).ExecuteReturnIdentityAsync();

                //判断团是否满了，如果满了，就更新状态
                if (!string.IsNullOrEmpty(info.parameters))
                {
                    var parametersObj = (JObject)JsonConvert.DeserializeObject(info.parameters);
                    if (parametersObj != null && parametersObj.ContainsKey("peopleNumber"))
                    {
                        var countPeopleNumber = await DbClient.Queryable<CoreCmsPinTuanRecord>().CountAsync(p => p.teamId == teamId);
                        var peopleNumber = parametersObj["peopleNumber"].ObjectToInt(0);
                        if (peopleNumber > 0 && countPeopleNumber >= peopleNumber)
                        {
                            await DbClient.Updateable<CoreCmsPinTuanRecord>()
                                .SetColumns(p => p.status == (int)GlobalEnumVars.PinTuanRecordStatus.Succeed)
                                .Where(p => p.teamId == teamId).ExecuteCommandAsync();
                        }
                    }
                }
            }
            else
            {
                // 自己创建拼团
                //取得规则id
                var pinfo = await DbClient.Queryable<CoreCmsPinTuanRule, CoreCmsPinTuanGoods>((role, goods) => new object[]
                {
                    JoinType.Inner, role.id == goods.ruleId
                }).Where((role, goods) => role.isStatusOpen == true && goods.goodsId == orderItem.goodsId).FirstAsync();

                if (pinfo == null)
                {
                    res.status = false;
                    res.data = 10000;
                    res.msg = GlobalErrorCodeVars.Code10000;
                    return res;
                }

                var model = new CoreCmsPinTuanRecord();
                //model.teamId = teamId;
                model.userId = order.userId;
                model.ruleId = pinfo.id;
                model.status = (int)GlobalEnumVars.PinTuanRecordStatus.InProgress;
                model.orderId = order.orderId;
                model.goodsId = (int)orderItem.goodsId;

                //冗余拼团人数，拼团结束时间字段
                model.createTime = DateTime.Now;
                model.closeTime = DateTime.Now.AddMinutes(pinfo.significantInterval);
                //附加参数
                var parameters = new { peopleNumber = pinfo.peopleNumber };
                model.parameters = JsonConvert.SerializeObject(parameters);
                model.teamId = 0;
                var outId = await DbClient.Insertable(model).ExecuteReturnIdentityAsync();
                if (outId > 0)
                {
                    await DbClient.Updateable<CoreCmsPinTuanRecord>().SetColumns(p => p.teamId == outId).Where(p => p.id == outId).ExecuteCommandAsync();
                }
            }
            res.status = true;
            return res;
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
        public new async Task<IPageList<CoreCmsPinTuanRecord>> QueryPageAsync(Expression<Func<CoreCmsPinTuanRecord, bool>> predicate,
            Expression<Func<CoreCmsPinTuanRecord, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsPinTuanRecord> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsPinTuanRecord, CoreCmsUser, CoreCmsPinTuanRule, CoreCmsGoods>((p, sc, ptr, good) => new JoinQueryInfos(
                        JoinType.Left, p.userId == sc.id, JoinType.Left, p.ruleId == ptr.id, JoinType.Left, p.goodsId == good.id))
                .Select((p, sc, ptr, good) => new CoreCmsPinTuanRecord
                {
                    id = p.id,
                    teamId = p.teamId,
                    userId = p.userId,
                    ruleId = p.ruleId,
                    goodsId = p.goodsId,
                    status = p.status,
                    orderId = p.orderId,
                    parameters = p.parameters,
                    closeTime = p.closeTime,
                    createTime = p.createTime,
                    updateTime = p.updateTime,
                    userAvatar = sc.avatarImage,
                    nickName = sc.nickName,
                    ruleName = ptr.name,
                    goodName = good.name
                })
                .MergeTable()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsPinTuanRecord, CoreCmsUser, CoreCmsPinTuanRule, CoreCmsGoods>((p, sc, ptr, good) => new JoinQueryInfos(
                        JoinType.Left, p.userId == sc.id, JoinType.Left, p.ruleId == ptr.id, JoinType.Left, p.goodsId == good.id))
                    .Select((p, sc, ptr, good) => new CoreCmsPinTuanRecord
                    {
                        id = p.id,
                        teamId = p.teamId,
                        userId = p.userId,
                        ruleId = p.ruleId,
                        goodsId = p.goodsId,
                        status = p.status,
                        orderId = p.orderId,
                        parameters = p.parameters,
                        closeTime = p.closeTime,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        userAvatar = sc.avatarImage,
                        nickName = sc.nickName,
                        ruleName = ptr.name,
                        goodName = good.name
                    })
                .MergeTable()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsPinTuanRecord>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
