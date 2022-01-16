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
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 服务购买表 接口实现
    /// </summary>
    public class CoreCmsUserServicesOrderServices : BaseServices<CoreCmsUserServicesOrder>, ICoreCmsUserServicesOrderServices
    {
        private readonly ICoreCmsUserServicesOrderRepository _dal;
        private readonly IServiceProvider _serviceProvider;


        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsUserServicesOrderServices(IUnitOfWork unitOfWork, ICoreCmsUserServicesOrderRepository dal, IServiceProvider serviceProvider)
        {
            this._dal = dal;
            _serviceProvider = serviceProvider;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 完成服务订单后生成兑换券
        /// </summary>
        /// <param name="serviceOrderId"></param>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CreateUserServicesTickets(string serviceOrderId, string paymentId)
        {
            using var container = _serviceProvider.CreateScope();

            var servicesServices = container.ServiceProvider.GetService<ICoreCmsServicesServices>();
            var userServicesTicketServices = container.ServiceProvider.GetService<ICoreCmsUserServicesTicketServices>();


            var jm = new WebApiCallBack();

            var model = await _dal.QueryByClauseAsync(p => p.serviceOrderId == serviceOrderId);
            if (model == null)
            {
                jm.msg = "订单获取失败";
                return jm;
            }

            var servicesModel = await servicesServices.QueryByClauseAsync(p => p.id == model.servicesId);
            if (servicesModel == null)
            {
                jm.msg = "服务信息获取失败";
                return jm;
            }

            model.isPay = true;
            model.payTime = DateTime.Now;
            model.paymentId = paymentId;
            model.servicesEndTime = servicesModel.validityEndTime;

            var up = await _dal.UpdateAsync(model);
            var bl = false;
            if (up)
            {
                var tickets = new List<CoreCmsUserServicesTicket>();
                for (int i = 0; i < servicesModel.ticketNumber; i++)
                {
                    var tk = new CoreCmsUserServicesTicket();
                    tk.serviceOrderId = serviceOrderId;
                    tk.securityCode = Guid.NewGuid();
                    tk.redeemCode = CommonHelper.GetSerialNumberType((int)GlobalEnumVars.SerialNumberType.服务券兑换码);
                    tk.serviceId = model.servicesId;
                    tk.userId = model.userId;
                    tk.status = (int)GlobalEnumVars.ServicesTicketStatus.Normal;
                    tk.validityType = servicesModel.validityType;
                    tk.validityStartTime = servicesModel.validityStartTime;
                    tk.validityEndTime = servicesModel.validityEndTime;
                    tk.createTime = DateTime.Now;
                    tk.isVerification = false;
                    tickets.Add(tk);
                }
                bl = await userServicesTicketServices.InsertAsync(tickets) > 0;
            }

            jm.status = bl && up;
            jm.msg = bl && up ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
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
        public new async Task<IPageList<CoreCmsUserServicesOrder>> QueryPageAsync(Expression<Func<CoreCmsUserServicesOrder, bool>> predicate,
            Expression<Func<CoreCmsUserServicesOrder, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion

    }
}
