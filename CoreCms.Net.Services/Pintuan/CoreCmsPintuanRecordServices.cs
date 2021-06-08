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
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 拼团记录表 接口实现
    /// </summary>
    public class CoreCmsPinTuanRecordServices : BaseServices<CoreCmsPinTuanRecord>, ICoreCmsPinTuanRecordServices
    {
        private readonly ICoreCmsPinTuanRecordRepository _dal;
        private readonly ICoreCmsPinTuanRuleRepository _pinTuanRuleRepository;
        private readonly ICoreCmsUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISysTaskLogServices _taskLogServices;


        public CoreCmsPinTuanRecordServices(IUnitOfWork unitOfWork
            , ICoreCmsPinTuanRecordRepository dal
            , ICoreCmsPinTuanRuleRepository pinTuanRuleRepository
            , ICoreCmsUserRepository userRepository, IServiceProvider serviceProvider, ISysTaskLogServices taskLogServices)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _pinTuanRuleRepository = pinTuanRuleRepository;
            _userRepository = userRepository;
            _serviceProvider = serviceProvider;
            _taskLogServices = taskLogServices;
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
            return await _dal.OrderAdd(order, items, teamId);
        }


        /// <summary>
        /// 取得商品的所有拼团记录
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="goodsId"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetRecord(int ruleId, int goodsId, int status = 0)
        {
            var jm = new WebApiCallBack();

            var pinfo = await _pinTuanRuleRepository.QueryByIdAsync(ruleId);
            if (pinfo == null)
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }
            var dt = DateTime.Now;
            var where = PredicateBuilder.True<CoreCmsPinTuanRecord>();
            if (status != 0)
            {
                where = where.And(p => p.status == status);
                if (status == 1) //如果取的是当前正在进行的团的话，这里取还没有结束的团记录，
                {
                    where = where.And(p => p.closeTime > dt);
                }
            }

            where = where.And(p => p.ruleId == ruleId && p.goodsId == goodsId && p.id == p.teamId);

            var model = await _dal.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            var resultModel = new List<CoreCmsPinTuanRecord>();
            if (model != null && model.Any())
            {
                var userIds = model.Select(p => p.userId).ToList();
                var users = await _userRepository.QueryListByClauseAsync(p => userIds.Contains(p.id));

                foreach (var item in model)
                {
                    var user = users.Find(p => p.id == item.userId);
                    item.userAvatar = user != null ? user.avatarImage : "";
                    item.nickName = user != null ? !string.IsNullOrEmpty(user.nickName) ? user.nickName : UserHelper.FormatMobile(user.mobile) : "";
                    //获取拼团团队记录
                    var teams = await _dal.QueryListByClauseAsync(p => p.teamId == item.teamId);
                    if (teams.Any())
                    {
                        var teamsUserIds = teams.Select(p => p.userId).ToList();
                        var teamsUsers = await _userRepository.QueryListByClauseAsync(p => teamsUserIds.Contains(p.id));

                        var teamsUserInfo = new List<PinTuanRecordTeam>();
                        foreach (var cmsUser in teamsUsers)
                        {
                            teamsUserInfo.Add(new PinTuanRecordTeam()
                            {
                                nickName = !string.IsNullOrEmpty(cmsUser.nickName) ? cmsUser.nickName : UserHelper.FormatMobile(cmsUser.mobile),
                                userAvatar = cmsUser.avatarImage
                            });
                        }
                        item.teams = teamsUserInfo;
                    }
                    //计算还剩几个人拼成功
                    item.teamNums = item.teams.Count;
                    if (item.teamNums < pinfo.peopleNumber)
                    {
                        item.teamNums = pinfo.peopleNumber - item.teamNums;
                    }
                    else
                    {
                        continue;
                        //model.Remove(item);
                    }
                    TimeSpan ts = item.closeTime.Subtract(dt);
                    item.lastTime = (int)ts.TotalSeconds;

                    resultModel.Add(item);
                }
            }
            jm.status = true;
            jm.msg = "获取成功";
            jm.data = resultModel;

            return jm;
        }

        /// <summary>
        /// 获取拼团团队人数
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetTeamList(int teamId, string orderId)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(orderId) && teamId == 0)
            {
                jm.msg = GlobalErrorCodeVars.Code15606;
                return jm;
            }

            if (teamId == 0)
            {
                var info = await _dal.QueryByClauseAsync(p => p.orderId == orderId);
                if (info == null)
                {
                    jm.msg = "没有找到拼团记录";
                    return jm;
                }

                teamId = info.teamId;
            }

            //根据teamId取发起团的信息
            var firstTeam = await _dal.QueryByIdAsync(teamId);
            if (firstTeam == null)
            {
                jm.msg = GlobalErrorCodeVars.Code15609;
                return jm;
            }

            var dt = DateTime.Now;
            TimeSpan ts = firstTeam.closeTime.Subtract(dt);
            firstTeam.lastTime = (int)ts.TotalSeconds;
            firstTeam.isOverdue = firstTeam.lastTime > 0;

            var user = await _userRepository.QueryByIdAsync(firstTeam.userId);
            firstTeam.userAvatar = user != null ? user.avatarImage : "";
            firstTeam.nickName = user != null ? !string.IsNullOrEmpty(user.nickName) ? user.nickName : UserHelper.FormatMobile(user.mobile) : "";
            //获取拼团团队记录
            var teams = await _dal.QueryListByClauseAsync(p => p.teamId == teamId);
            if (teams.Any())
            {
                var teamsUserIds = teams.Select(p => p.userId).ToList();
                var teamsUsers = await _userRepository.QueryListByClauseAsync(p => teamsUserIds.Contains(p.id));

                var teamsUserInfo = new List<PinTuanRecordTeam>();
                teams.ForEach(p =>
                {
                    var tmUser = new PinTuanRecordTeam();
                    var user = teamsUsers.FirstOrDefault(o => o.id == p.userId);
                    tmUser.nickName = user != null && !string.IsNullOrEmpty(user.nickName) ? user.nickName : UserHelper.FormatMobile(user.mobile);
                    tmUser.userAvatar = !string.IsNullOrEmpty(user.avatarImage) ? user.avatarImage : "";
                    tmUser.recordId = p.id;
                    tmUser.userId = p.userId;
                    tmUser.teamId = p.teamId;

                    teamsUserInfo.Add(tmUser);
                });
                firstTeam.teams = teamsUserInfo;
            }
            //计算还剩几个人拼成功
            firstTeam.teamNums = firstTeam.teams.Count;
            var parameters = JObject.Parse(firstTeam.parameters);
            var peopleNumber = 0;
            if (parameters.ContainsKey("peopleNumber"))
            {
                peopleNumber = parameters["peopleNumber"].ObjectToInt(0);
            }
            if (firstTeam.teamNums < peopleNumber)
            {
                firstTeam.teamNums = peopleNumber - firstTeam.teamNums;
            }
            else
            {
                firstTeam.teamNums = 0;
            }
            firstTeam.peopleNumber = peopleNumber;

            jm.status = true;
            jm.msg = "获取成功";
            jm.data = firstTeam;

            return jm;
        }


        /// <summary>
        /// 自动取消到时间的团
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> AutoCanclePinTuanOrder()
        {
            using var container = _serviceProvider.CreateScope();

            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();
            var billRefundServices = container.ServiceProvider.GetService<ICoreCmsBillRefundServices>();

            var jm = new WebApiCallBack();

            var dt = DateTime.Now;
            //获取主开团数据
            var list = await _dal.QueryListByClauseAsync(p => p.closeTime < dt && p.status == (int)GlobalEnumVars.PinTuanRecordStatus.InProgress && p.id == p.teamId);
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    //获取开团数据
                    var teamList = await _dal.QueryListByClauseAsync(p => p.teamId == item.id);
                    //更新开团失败数据
                    await _dal.UpdateAsync(
                        p => new CoreCmsPinTuanRecord() { status = (int)GlobalEnumVars.PinTuanRecordStatus.Defeated },
                        p => p.teamId == item.id);

                    if (teamList == null || !teamList.Any()) continue;
                    {
                        //给这个订单作废，如果有支付，并退款
                        var orderId = teamList.Select(p => p.orderId).ToArray();
                        //await CancelOrder(ids);
                        //拼团订单取消，如果已支付自动退款，如果未支付，作废
                        foreach (var id in orderId)
                        {
                            var orderResult = await orderServices.GetOrderInfoByOrderId(id);
                            if (orderResult.status == false)
                            {
                                continue;
                            }
                            var orderInfo = orderResult.data as CoreCmsOrder;
                            if (orderInfo == null)
                            {
                                continue;
                            }
                            if (orderInfo.shipStatus == (int)GlobalEnumVars.OrderShipStatus.Yes)
                            {
                                //如果已经发货了，就不管了，手动退款吧
                                continue;
                            }

                            if (orderInfo.payStatus == (int)GlobalEnumVars.OrderPayStatus.No)
                            {
                                //未支付
                                var noPayedIds = new[] { orderInfo.orderId };
                                await orderServices.CancelOrder(noPayedIds);
                            }
                            else
                            {
                                //已支付，生成退款单，并直接退款，之后，更改订单状态
                                var res = await billRefundServices.ToAdd(orderInfo.userId, orderInfo.orderId, 1,
                                    orderInfo.payedAmount, "");
                                if (res.status == false)
                                {
                                    continue;
                                }
                                var refundInfo = await billRefundServices.QueryByClauseAsync(p =>
                                    p.sourceId == orderInfo.orderId &&
                                    p.status == (int)GlobalEnumVars.BillRefundType.Order && p.type == 1);
                                if (refundInfo == null)
                                {
                                    //没有找到退款单
                                    continue;
                                }
                                //去退款
                                var toRefundResult = await billRefundServices.ToRefund(refundInfo.refundId, (int)GlobalEnumVars.BillRefundStatus.STATUS_REFUND);

                                //插入退款日志
                                var log = new SysTaskLog
                                {
                                    createTime = DateTime.Now,
                                    isSuccess = toRefundResult.status,
                                    name = "定时任务取消拼团订单退款日志",
                                    parameters = JsonConvert.SerializeObject(toRefundResult)
                                };
                                await _taskLogServices.InsertAsync(log);


                                //更新订单状态为已退款已完成
                                await orderServices.UpdateAsync(p => new CoreCmsOrder()
                                {
                                    payStatus = (int)GlobalEnumVars.OrderPayStatus.Refunded,
                                    status = (int)GlobalEnumVars.OrderStatus.Complete
                                }, p => p.orderId == orderInfo.orderId);
                            }
                        }
                    }
                }
            }

            jm.status = true;

            //插入日志
            var model = new SysTaskLog
            {
                createTime = DateTime.Now,
                isSuccess = jm.status,
                name = "自动取消到时间的团",
                parameters = JsonConvert.SerializeObject(jm)
            };
            await _taskLogServices.InsertAsync(model);


            return jm;
        }


        //拼团订单取消，如果已支付自动退款，如果未支付，作废（方法直接并入上个方法内）
        private async Task<bool> CancelOrder(string[] orderId)
        {
            using var container = _serviceProvider.CreateScope();
            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();
            var billRefundServices = container.ServiceProvider.GetService<ICoreCmsBillRefundServices>();

            foreach (var id in orderId)
            {
                var orderResult = await orderServices.GetOrderInfoByOrderId(id);
                if (orderResult.status == false)
                {
                    continue;
                }
                var orderInfo = orderResult.data as CoreCmsOrder;
                if (orderInfo == null)
                {
                    continue;
                }
                if (orderInfo.shipStatus == (int)GlobalEnumVars.OrderShipStatus.Yes)
                {
                    //如果已经发货了，就不管了，手动退款吧
                    continue;
                }

                if (orderInfo.payStatus == (int)GlobalEnumVars.OrderPayStatus.No)
                {
                    //未支付
                    var ids = new[] { orderInfo.orderId };
                    await orderServices.CancelOrder(ids);
                }
                else
                {
                    //已支付，生成退款单，并直接退款，之后，更改订单状态
                    var res = await billRefundServices.ToAdd(orderInfo.userId, orderInfo.orderId, 1, orderInfo.payedAmount, "");
                    if (res.status == false)
                    {
                        continue;
                    }
                    var refundInfo = await billRefundServices.QueryByClauseAsync(p =>
                        p.sourceId == orderInfo.orderId &&
                        p.status == (int)GlobalEnumVars.BillRefundType.Order && p.type == 1);
                    if (refundInfo == null)
                    {
                        //没有找到退款单
                        continue;
                    }
                    //去退款
                    await billRefundServices.ToRefund(refundInfo.refundId, (int)GlobalEnumVars.BillRefundStatus.STATUS_REFUND);

                    //更新订单状态为已退款已完成
                    await orderServices.UpdateAsync(p => new CoreCmsOrder()
                    {
                        payStatus = (int)GlobalEnumVars.OrderPayStatus.Refunded,
                        status = (int)GlobalEnumVars.OrderStatus.Complete
                    }, p => p.orderId == orderInfo.orderId);
                }
            }

            return true;
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
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion

    }
}
