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
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 服务券核验日志 接口实现
    /// </summary>
    public class CoreCmsUserServicesTicketVerificationLogServices : BaseServices<CoreCmsUserServicesTicketVerificationLog>, ICoreCmsUserServicesTicketVerificationLogServices
    {
        private readonly ICoreCmsUserServicesTicketVerificationLogRepository _dal;
        private readonly ICoreCmsServicesServices _servicesServices;
        private readonly ICoreCmsUserServicesTicketServices _userServicesTicketServices;
        private readonly ICoreCmsClerkServices _clerkServices;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsUserServicesTicketVerificationLogServices(IUnitOfWork unitOfWork, ICoreCmsUserServicesTicketVerificationLogRepository dal, ICoreCmsClerkServices clerkServices, ICoreCmsServicesServices servicesServices, ICoreCmsUserServicesTicketServices userServicesTicketServices)
        {
            this._dal = dal;
            _clerkServices = clerkServices;
            _servicesServices = servicesServices;
            _userServicesTicketServices = userServicesTicketServices;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 店铺核销的服务券列表
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetVerificationLogs(int userId, int page, int limit)
        {
            var jm = new WebApiCallBack();

            var clerk = await _clerkServices.QueryByClauseAsync(p => p.userId == userId);
            if (clerk == null)
            {
                jm.msg = "未查询到用户门店";
                return jm;
            }

            var logs = await _dal.QueryPageAsync(p => p.storeId == clerk.storeId && p.isDel == false, p => p.verificationTime, OrderByType.Desc, page, limit);

            if (logs != null && logs.Any())
            {
                var ticketIds = logs.Select(p => p.ticketId).ToList();
                var serviceIds = logs.Select(p => p.serviceId).ToList();

                var servicesModel = await _servicesServices.QueryListByClauseAsync(p => serviceIds.Contains(p.id));
                var ticketsMdoel = await _userServicesTicketServices.QueryListByClauseAsync(p => ticketIds.Contains(p.id));

                foreach (var item in logs)
                {
                    item.ticket = ticketsMdoel.Find(p => p.id == item.ticketId);
                    item.service = servicesModel.Find(p => p.id == item.serviceId);
                }
                jm.status = true;
                jm.msg = "获取成功";
            }

            jm.data = logs;
            jm.otherData = new
            {
                logs.TotalPages,
                logs.TotalCount
            };

            return jm;
        }

        /// <summary>
        /// 删除服务券核销单(软删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> LogDelete(int id, int userId = 0)
        {
            var jm = new WebApiCallBack();

            var model = await _dal.QueryByClauseAsync(p => p.id == id);
            if (model != null)
            {
                if (userId > 0)
                {
                    var clerks = await _clerkServices.ExistsAsync(p => p.userId == userId && p.storeId == model.storeId);
                    if (!clerks)
                    {
                        jm.msg = "你无权删除该服务券核销单";
                        return jm;
                    }
                }
                model.isDel = true;
                var bl = await _dal.UpdateAsync(model);

                jm.status = bl;
                jm.msg = bl ? "删除成功" : "删除失败";
            }
            else
            {
                jm.msg = "未找到服务券核销单";
            }

            return jm;
        }

    }
}
