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
    /// 消息
    /// </summary>
    public class OrderPayedCommand : IRequest<WebApiCallBack>
    {
        public CoreCmsOrder order { get; set; }
    }

    /// <summary>
    /// 处理器-订单支付成功后用户是否可升级处理
    /// </summary>
    public class OrderPayedEventHandler : IRequestHandler<OrderPayedCommand, WebApiCallBack>
    {
        private readonly ICoreCmsDistributionOrderServices _distributionOrderServices;
        private readonly ICoreCmsDistributionServices _distributionServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsAgentOrderServices _agentOrderServices;


        public OrderPayedEventHandler(ICoreCmsDistributionOrderServices distributionOrderServices, ICoreCmsDistributionServices distributionServices, ICoreCmsSettingServices settingServices, ICoreCmsUserServices userServices, ICoreCmsAgentOrderServices agentOrderServices)
        {
            _distributionOrderServices = distributionOrderServices;
            _distributionServices = distributionServices;
            _settingServices = settingServices;
            _userServices = userServices;
            _agentOrderServices = agentOrderServices;
        }


        public async Task<WebApiCallBack> Handle(OrderPayedCommand request, CancellationToken cancellationToken)
        {
            var jm = new WebApiCallBack();
            if (request.order == null)
            {
                jm.msg = "订单获取失败";
                return await Task.FromResult(jm);
            }

            var allConfigs = await _settingServices.GetConfigDictionaries();

            jm = await _agentOrderServices.AddData(request.order);

            //判断是走代理还是走分销
            if (jm.status == true)
            {
                return await Task.FromResult(jm);
            }
            else
            {
                await _distributionOrderServices.AddData(request.order); //添加分享关联订单日志
                //判断是否可以成为分销商
                //先判断是否已经是经销商了。
                bool check = await _distributionServices.ExistsAsync(p => p.userId == request.order.userId);
                var distributionType = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.DistributionType).ObjectToInt(0);
                if (distributionType == 3)  //无需审核，但是要满足提交
                {
                    var info = new CoreCmsDistribution();
                    //判断是否分销商
                    if (check == false)
                    {
                        await _distributionServices.CheckCondition(allConfigs, info, request.order.userId);
                        if (info.ConditionStatus == true && info.ConditionProgress == 100)
                        {
                            //添加用户
                            var user = await _userServices.QueryByClauseAsync(p => p.id == request.order.userId);
                            var iData = new CoreCmsDistribution();
                            iData.userId = request.order.userId;
                            iData.mobile = user.mobile;
                            iData.name = !string.IsNullOrEmpty(user.nickName) ? user.nickName : user.mobile;
                            iData.verifyStatus = (int)GlobalEnumVars.DistributionVerifyStatus.VerifyYes;
                            iData.verifyTime = DateTime.Now;

                            await _distributionServices.AddData(iData, request.order.userId);
                        }
                    }
                }
                //已经是经销商的判断是否可以升级
                if (check)
                {
                    await _distributionServices.CheckUpdate(request.order.userId);
                }
                jm.status = true;
                jm.msg = "分销成功";
                return await Task.FromResult(jm);
            }
        }
    }

}
