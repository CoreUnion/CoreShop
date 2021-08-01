using System;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Essensoft.Paylink.WeChatPay.V2;
using Essensoft.Paylink.WeChatPay.V2.Notify;
using InitQ.Abstractions;
using InitQ.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoreCms.Net.RedisMQ.Subscribe
{
    /// <summary>
    /// 订单完结后走代理或分销商提成处理
    /// </summary>
    public class OrderAgentOrDistributionSubscribe : IRedisSubscribe
    {
        private readonly ICoreCmsBillPaymentsServices _billPaymentsServices;

        private readonly ICoreCmsDistributionOrderServices _distributionOrderServices;
        private readonly ICoreCmsDistributionServices _distributionServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsAgentOrderServices _agentOrderServices;


        public OrderAgentOrDistributionSubscribe(ICoreCmsBillPaymentsServices billPaymentsServices, ICoreCmsDistributionOrderServices distributionOrderServices, ICoreCmsDistributionServices distributionServices, ICoreCmsSettingServices settingServices, ICoreCmsUserServices userServices, ICoreCmsAgentOrderServices agentOrderServices)
        {
            _billPaymentsServices = billPaymentsServices;
            _distributionOrderServices = distributionOrderServices;
            _distributionServices = distributionServices;
            _settingServices = settingServices;
            _userServices = userServices;
            _agentOrderServices = agentOrderServices;
        }

        /// <summary>
        /// 订单完结后走代理或分销商提成处理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Subscribe(RedisMessageQueueKey.OrderAgentOrDistribution)]

        private async Task OrderAgentOrDistribution(string msg)
        {
            try
            {
                var order = JsonConvert.DeserializeObject<CoreCmsOrder>(msg);
                if (order != null)
                {
                    var jm = await _agentOrderServices.AddData(order);

                    //判断是走代理还是走分销
                    if (jm.status == true)
                    {
                        NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "订单完结后走代理结佣", JsonConvert.SerializeObject(jm));
                        return;
                    }
                    else
                    {
                        await _distributionOrderServices.AddData(order); //添加分享关联订单日志
                        //判断是否可以成为分销商
                        //先判断是否已经是经销商了。
                        bool check = await _distributionServices.ExistsAsync(p => p.userId == order.userId);
                        var allConfigs = await _settingServices.GetConfigDictionaries();
                        var distributionType = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.DistributionType).ObjectToInt(0);
                        if (distributionType == 3)  //无需审核，但是要满足提交
                        {
                            var info = new CoreCmsDistribution();
                            //判断是否分销商
                            if (check == false)
                            {
                                await _distributionServices.CheckCondition(allConfigs, info, order.userId);
                                if (info.ConditionStatus == true && info.ConditionProgress == 100)
                                {
                                    //添加用户
                                    var user = await _userServices.QueryByClauseAsync(p => p.id == order.userId);
                                    var iData = new CoreCmsDistribution();
                                    iData.userId = order.userId;
                                    iData.mobile = user.mobile;
                                    iData.name = !string.IsNullOrEmpty(user.nickName) ? user.nickName : user.mobile;
                                    iData.verifyStatus = (int)GlobalEnumVars.DistributionVerifyStatus.VerifyYes;
                                    iData.verifyTime = DateTime.Now;

                                    await _distributionServices.AddData(iData, order.userId);
                                }
                            }
                        }
                        //已经是经销商的判断是否可以升级
                        if (check)
                        {
                            await _distributionServices.CheckUpdate(order.userId);
                        }
                        jm.status = true;
                        jm.msg = "分销成功";
                        NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "订单完结后走分销结佣", JsonConvert.SerializeObject(jm));
                        return;
                    }
                }
                else
                {
                    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "订单完结结佣", "订单获取失败");
                }
            }
            catch (Exception ex)
            {
                NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "订单完结结佣", msg, ex);
                throw;
            }
            await Task.CompletedTask;
        }


    }
}
