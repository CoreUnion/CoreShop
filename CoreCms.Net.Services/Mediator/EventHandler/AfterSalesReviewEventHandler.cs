/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-08-13 23:57:23
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Threading;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using MediatR;

namespace CoreCms.Net.Services.Mediator
{
    /// <summary>
    /// 消息
    /// </summary>
    public class AfterSalesReviewCommand : IRequest<string>
    {
        public string AfterSalesId { get; set; }
    }

    /// <summary>
    /// 处理器-售后审核通过后
    /// </summary>
    public class AfterSalesReviewEventHandler : IRequestHandler<AfterSalesReviewCommand, string>
    {
        private readonly ICoreCmsBillAftersalesServices _aftersalesServices;
        private readonly ICoreCmsDistributionOrderServices _distributionOrderServices;
        private readonly ICoreCmsAgentOrderServices _agentOrderServices;

        public AfterSalesReviewEventHandler(ICoreCmsBillAftersalesServices aftersalesServices, ICoreCmsDistributionOrderServices distributionOrderServices, ICoreCmsAgentOrderServices agentOrderServices)
        {
            _aftersalesServices = aftersalesServices;
            _distributionOrderServices = distributionOrderServices;
            _agentOrderServices = agentOrderServices;
        }

        public async Task<string> Handle(AfterSalesReviewCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.AfterSalesId))
            {
                return await Task.FromResult("审核单编号获取失败");
            }

            var info = await _aftersalesServices.QueryByClauseAsync(p => p.aftersalesId == request.AfterSalesId);
            if (info != null)
            {
                await _distributionOrderServices.CancleOrderByOrderId(info.orderId);
                await _agentOrderServices.CancleOrderByOrderId(info.orderId);
            }
            return "true";
        }
    }

}
