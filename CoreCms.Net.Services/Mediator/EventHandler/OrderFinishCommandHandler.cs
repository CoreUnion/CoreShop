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
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using MediatR;

namespace CoreCms.Net.Services.Mediator
{
    /// <summary>
    /// 订单完成时，结算该订单
    /// </summary>
    public class OrderFinishCommand : IRequest<WebApiCallBack>
    {
        public string OrderId { get; set; }
    }

    /// <summary>
    /// 处理器-订单完成后
    /// </summary>
    public class OrderFinishCommandHandler : IRequestHandler<OrderFinishCommand, WebApiCallBack>
    {
        private readonly ICoreCmsDistributionOrderServices _distributionOrderServices;
        private readonly ICoreCmsAgentOrderServices _agentOrderServices;

        public OrderFinishCommandHandler(ICoreCmsDistributionOrderServices distributionOrderServices, ICoreCmsAgentOrderServices agentOrderServices)
        {
            _distributionOrderServices = distributionOrderServices;
            _agentOrderServices = agentOrderServices;
        }

        public async Task<WebApiCallBack> Handle(OrderFinishCommand request, CancellationToken cancellationToken)
        {
            var jm = new WebApiCallBack();
            if (string.IsNullOrEmpty(request.OrderId))
            {
                jm.msg = "订单编号获取失败";
                return await Task.FromResult(jm);
            }

            await _distributionOrderServices.FinishOrder(request.OrderId);
            await _agentOrderServices.FinishOrder(request.OrderId);



            return await Task.FromResult(jm);
        }
    }

}
