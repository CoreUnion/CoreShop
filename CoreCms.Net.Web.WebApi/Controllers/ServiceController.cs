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
using System.Threading.Tasks;
using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 服务卡控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ICoreCmsServicesServices _servicesServices;
        private readonly ICoreCmsUserServicesOrderServices _userServicesOrderServices;
        private readonly ICoreCmsUserServicesTicketServices _userServicesTicketServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsUserServicesTicketVerificationLogServices _ticketVerificationLogServices;
        private readonly ICoreCmsClerkServices _clerkServices;
        private readonly ICoreCmsStoreServices _storeServices;
        private readonly ICoreCmsUserGradeServices _userGradeServices;


        private readonly IHttpContextUser _user;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="servicesServices"></param>
        /// <param name="user"></param>
        /// <param name="userServicesOrderServices"></param>
        /// <param name="userServicesTicketServices"></param>
        /// <param name="userServices"></param>
        /// <param name="clerkServices"></param>
        /// <param name="ticketVerificationLogServices"></param>
        /// <param name="storeServices"></param>
        /// <param name="userGradeServices"></param>
        public ServiceController(ICoreCmsServicesServices servicesServices, IHttpContextUser user, ICoreCmsUserServicesOrderServices userServicesOrderServices, ICoreCmsUserServicesTicketServices userServicesTicketServices, ICoreCmsUserServices userServices, ICoreCmsClerkServices clerkServices, ICoreCmsUserServicesTicketVerificationLogServices ticketVerificationLogServices, ICoreCmsStoreServices storeServices, ICoreCmsUserGradeServices userGradeServices)
        {
            _servicesServices = servicesServices;
            _user = user;
            _userServicesOrderServices = userServicesOrderServices;
            _userServicesTicketServices = userServicesTicketServices;
            _userServices = userServices;
            _clerkServices = clerkServices;
            _ticketVerificationLogServices = ticketVerificationLogServices;
            _storeServices = storeServices;
            _userGradeServices = userGradeServices;
        }


        #region 取得服务卡列表信息
        /// <summary>
        /// 取得服务卡列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public async Task<WebApiCallBack> GetPageList([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var dt = DateTime.Now;
            var where = PredicateBuilder.True<CoreCmsServices>();

            where = where.And(p => p.status == (int)GlobalEnumVars.ServicesStatus.Shelve);
            where = where.And(p => p.amount > 0);
            where = where.And(p => p.startTime < dt && p.endTime > dt);

            var list = await _servicesServices.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, entity.page, entity.limit);

            if (list.Any())
            {
                var storesAll = await _storeServices.QueryAsync();
                var userGradesAll = await _userGradeServices.QueryAsync();

                foreach (var data in list)
                {
                    TimeSpan ts = data.endTime.Subtract(dt);
                    data.timestamp = (int)ts.TotalSeconds;

                    if (!string.IsNullOrEmpty(data.consumableStore))
                    {
                        var consumableStoreStr = CommonHelper.GetCaptureInterceptedText(data.consumableStore, ",");
                        var consumableStoreIds = CommonHelper.StringToIntArray(consumableStoreStr);
                        if (consumableStoreIds.Any())
                        {
                            var stores = storesAll.Where(p => consumableStoreIds.Contains(p.id)).ToList();
                            data.consumableStores = stores.Select(p => p.storeName).ToList();
                        }
                    }

                    if (!string.IsNullOrEmpty(data.allowedMembership))
                    {
                        var allowedMembershipStr = CommonHelper.GetCaptureInterceptedText(data.allowedMembership, ",");
                        var allowedMembershipIds = CommonHelper.StringToIntArray(allowedMembershipStr);
                        if (allowedMembershipIds.Any())
                        {
                            var userGrades = userGradesAll.Where(p => allowedMembershipIds.Contains(p.id)).ToList();
                            data.allowedMemberships = userGrades.Select(p => p.title).ToList();
                        }
                    }
                }
            }

            jm.status = true;
            jm.data = new
            {
                list = list,
                count = list.TotalCount,
            };
            return jm;

        }

        #endregion

        #region 获取服务卡详情
        /// <summary>
        /// 获取服务卡详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public async Task<WebApiCallBack> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            var data = await _servicesServices.QueryByClauseAsync(p => p.id == entity.id);

            if (data != null)
            {
                var dt = DateTime.Now;
                TimeSpan ts = data.endTime.Subtract(dt);
                data.timestamp = (int)ts.TotalSeconds;

                if (!string.IsNullOrEmpty(data.consumableStore))
                {
                    var consumableStoreStr = CommonHelper.GetCaptureInterceptedText(data.consumableStore, ",");
                    var consumableStoreIds = CommonHelper.StringToIntArray(consumableStoreStr);
                    if (consumableStoreIds.Any())
                    {
                        var stores = await _storeServices.QueryListByClauseAsync(p => consumableStoreIds.Contains(p.id));
                        data.consumableStores = stores.Select(p => p.storeName).ToList();
                    }
                }

                if (!string.IsNullOrEmpty(data.allowedMembership))
                {
                    var allowedMembershipStr = CommonHelper.GetCaptureInterceptedText(data.allowedMembership, ",");
                    var allowedMembershipIds = CommonHelper.StringToIntArray(allowedMembershipStr);
                    if (allowedMembershipIds.Any())
                    {
                        var userGrades = await _userGradeServices.QueryListByClauseAsync(p => allowedMembershipIds.Contains(p.id));
                        data.allowedMemberships = userGrades.Select(p => p.title).ToList();
                    }
                }
            }

            jm.status = true;
            jm.data = data;
            return jm;

        }

        #endregion

        //验证接口====================================================================================================

        #region 添加服务订单
        /// <summary>
        /// 取得服务卡列表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> AddServiceOrder([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            var data = await _servicesServices.QueryByClauseAsync(p => p.id == entity.id);

            if (data == null)
            {
                jm.msg = "服务数据获取失败";
                return jm;
            }

            var user = await _userServices.QueryByIdAsync(_user.ID);
            if (user == null)
            {
                jm.msg = "用户数据获取失败";
                return jm;
            }

            if (!data.allowedMembership.Contains("," + user.grade + ","))
            {
                jm.msg = "您所在的用户级别不支持购买";
                return jm;
            }

            var order = new CoreCmsUserServicesOrder();
            order.serviceOrderId = CommonHelper.GetSerialNumberType((int)GlobalEnumVars.SerialNumberType.服务订单编号);
            order.userId = _user.ID;
            order.servicesId = entity.id;
            order.isPay = false;
            order.status = (int)GlobalEnumVars.ServicesOrderStatus.正常;
            order.createTime = DateTime.Now;

            var bl = await _userServicesOrderServices.InsertAsync(order) > 0;

            jm.status = bl;
            jm.data = order.serviceOrderId;
            return jm;

        }



        #endregion

        #region 店铺核销的服务券列表
        /// <summary>
        /// 店铺核销的服务券列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> VerificationPageList([FromBody] FMPageByIntId entity)
        {
            var jm = await _ticketVerificationLogServices.GetVerificationLogs(_user.ID, entity.page, entity.limit);
            return jm;
        }
        #endregion

        #region 软删除服务券核销单数据
        /// <summary>
        /// 软删除服务券核销单数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> LogDelete([FromBody] FMIntId entity)
        {
            var jm = await _ticketVerificationLogServices.LogDelete(entity.id, _user.ID);
            return jm;
        }
        #endregion


        #region 获取单个提货单详情
        /// <summary>
        /// 获取单个提货单详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetTicketInfo([FromBody] FMStringId entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.id))
            {
                jm.msg = "请提交查询数据关键词";
                return jm;
            }

            var ticket = await _userServicesTicketServices.QueryByClauseAsync(p => p.redeemCode == entity.id);
            if (ticket == null)
            {
                jm.msg = "未查询到服务券";
                return jm;
            }

            ticket.statusStr = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.ServicesTicketStatus>(ticket.status);

            var service = await _servicesServices.QueryByClauseAsync(p => p.id == ticket.serviceId);
            var serviceOrder =
                await _userServicesOrderServices.QueryByClauseAsync(p => p.serviceOrderId == ticket.serviceOrderId);

            jm.status = true;
            jm.data = new
            {
                ticket,
                service,
                serviceOrder
            };

            return jm;
        }
        #endregion

        #region 核销服务券
        /// <summary>
        /// 核销服务券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> VerificationTicket([FromBody] FMStringId entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.id))
            {
                jm.msg = "请提交查询数据关键词";
                return jm;
            }
            var ticket = await _userServicesTicketServices.QueryByClauseAsync(p => p.redeemCode == entity.id);
            if (ticket == null)
            {
                jm.msg = "未查询到服务券";
                return jm;
            }

            if (ticket.status != (int)GlobalEnumVars.ServicesTicketStatus.Normal)
            {
                jm.msg = "服务券状态不支持核销";
                return jm;
            }

            var service = await _servicesServices.QueryByIdAsync(ticket.serviceId);
            if (service == null)
            {
                jm.msg = "服务项目获取失败";
                return jm;
            }

            var user = await _userServices.QueryByIdAsync(_user.ID);
            if (user == null)
            {
                jm.msg = "未获取到审核权限";
                return jm;
            }

            var clerk = await _clerkServices.QueryByClauseAsync(p => p.userId == user.id);
            if (clerk == null)
            {
                jm.msg = "非门店店员无权限核验";
                return jm;
            }

            if (!service.consumableStore.Contains("," + clerk.storeId + ","))
            {
                jm.msg = "您所在的门店无权核销此券";
                return jm;
            }

            //开始更新数据
            var log = new CoreCmsUserServicesTicketVerificationLog
            {
                storeId = clerk.storeId,
                verificationUserId = _user.ID,
                ticketId = ticket.id,
                ticketRedeemCode = ticket.redeemCode,
                verificationTime = DateTime.Now,
                serviceId = ticket.serviceId,
                isDel = false
            };

            ticket.status = (int)GlobalEnumVars.ServicesTicketStatus.Verification;
            ticket.verificationTime = DateTime.Now;
            ticket.isVerification = true;
            var up = await _userServicesTicketServices.UpdateAsync(ticket);
            var bl = false;
            if (up)
            {
                bl = await _ticketVerificationLogServices.InsertAsync(log) > 0;
            }
            jm.status = up && bl;
            jm.msg = jm.status ? "核销成功" : "核销失败";

            return jm;
        }
        #endregion


    }
}
