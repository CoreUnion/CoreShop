/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/10 22:41:46
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using InitQ.Abstractions;
using InitQ.Attributes;
using Newtonsoft.Json;
using SqlSugar;

namespace CoreCms.Net.RedisMQ.Subscribe
{
    /// <summary>
    /// 用户相关队列操作
    /// </summary>
    public class UserSubscribe : IRedisSubscribe
    {

        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsOrderServices _orderServices;
        private readonly ICoreCmsBillRefundServices _billRefundServices;
        private readonly ICoreCmsUserGradeServices _userGradeServices;


        public UserSubscribe(ICoreCmsUserServices userServices, ICoreCmsOrderServices orderServices, ICoreCmsBillRefundServices billRefundServices, ICoreCmsUserGradeServices userGradeServices)
        {
            _userServices = userServices;
            _orderServices = orderServices;
            _billRefundServices = billRefundServices;
            _userGradeServices = userGradeServices;
        }

        /// <summary>
        /// 订单完成-用户升级处理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Subscribe(RedisMessageQueueKey.UserUpGrade)]
        private async Task UserUpGradeHandler(string msg)
        {
            try
            {
                var orderModel = JsonConvert.DeserializeObject<CoreCmsOrder>(msg);

                if (orderModel == null)
                {
                    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "订单完成-用户升级处理", "订单数据获取失败");
                    return;
                }

                var userInfo = await _userServices.QueryPageAsync(p => p.id == orderModel.userId);
                if (userInfo == null)
                {
                    NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "订单完成-用户升级处理", "用户数据获取失败");
                    return;
                }

                //订单支付的金额
                var payedMoney = await _orderServices.GetSumAsync(
                    p => p.payStatus != (int)GlobalEnumVars.OrderAllStatusType.ALL_PENDING_PAYMENT && p.userId == orderModel.userId,
                    p => p.orderAmount);

                //订单退款金额
                var refundMoney = await _billRefundServices.GetSumAsync(
                    p => p.type == (int)GlobalEnumVars.BillRefundType.Order && p.userId == orderModel.userId &&
                         p.status != (int)GlobalEnumVars.BillRefundStatus.STATUS_REFUND, p => p.money);

                var money = payedMoney - refundMoney;

                //取所有用户等级信息

                var userGradeModel = await _userGradeServices.QueryListByClauseAsync(p => p.id > 0, p => p.id, OrderByType.Asc);

                //var id = 0;

                foreach (var item in userGradeModel)
                {

                }

                NLogUtil.WriteAll(NLog.LogLevel.Info, LogType.RedisMessageQueue, "订单完成-用户升级处理", msg);
            }
            catch (Exception ex)
            {
                NLogUtil.WriteAll(NLog.LogLevel.Error, LogType.RedisMessageQueue, "订单完成-用户升级处理", msg, ex);
                throw;
            }
            await Task.CompletedTask;
        }


    }
}
