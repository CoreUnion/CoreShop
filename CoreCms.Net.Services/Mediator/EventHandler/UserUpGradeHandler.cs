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
using SqlSugar;

namespace CoreCms.Net.Services.Mediator
{
    /// <summary>
    /// 订单支付成功后，用户升级处理
    /// </summary>
    public class UserUpGradeCommand : IRequest<WebApiCallBack>
    {
        public CoreCmsOrder Order { get; set; }
    }

    /// <summary>
    /// 处理器-订单完成后
    /// </summary>
    public class UserUpGradeHandler : IRequestHandler<UserUpGradeCommand, WebApiCallBack>
    {
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsOrderServices _orderServices;
        private readonly ICoreCmsBillRefundServices _billRefundServices;
        private readonly ICoreCmsUserGradeServices _userGradeServices;


        public UserUpGradeHandler(ICoreCmsUserServices userServices, ICoreCmsOrderServices orderServices, ICoreCmsBillRefundServices billRefundServices, ICoreCmsUserGradeServices userGradeServices)
        {
            _userServices = userServices;
            _orderServices = orderServices;
            _billRefundServices = billRefundServices;
            _userGradeServices = userGradeServices;
        }

        public async Task<WebApiCallBack> Handle(UserUpGradeCommand request, CancellationToken cancellationToken)
        {
            var jm = new WebApiCallBack();
            if (request.Order == null)
            {
                jm.msg = "订单数据获取失败";
                return await Task.FromResult(jm);
            }

            var order = request.Order;
            var userInfo = await _userServices.QueryPageAsync(p => p.id == order.userId);
            if (userInfo == null)
            {
                jm.msg = "用户数据获取失败";
                return await Task.FromResult(jm);
            }

            //订单支付的金额
            var payedMoney = await _orderServices.GetSumAsync(
                p => p.payStatus != (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_PAYMENT && p.userId == order.userId,
                p => p.orderAmount);

            //订单退款金额
            var refundMoney = await _billRefundServices.GetSumAsync(
                p => p.type == (int)GlobalEnumVars.BillRefundType.Order && p.userId == order.userId &&
                     p.status != (int)GlobalEnumVars.BillRefundStatus.STATUS_REFUND, p => p.money);

            var money = payedMoney - refundMoney;

            //取所有用户等级信息

            var userGradeModel =
                await _userGradeServices.QueryListByClauseAsync(p => p.id > 0, p => p.id, OrderByType.Asc);

            //var id = 0;

            foreach (var item in userGradeModel)
            {

            }



            return await Task.FromResult(jm);
        }
    }

}
