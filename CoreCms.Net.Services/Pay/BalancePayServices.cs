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
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     线下支付 接口实现
    /// </summary>
    public class BalancePayServices : BaseServices<CoreCmsSetting>, IBalancePayServices
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICoreCmsUserBalanceServices _userBalanceServices;

        public BalancePayServices(IWeChatPayRepository dal
            , IServiceProvider serviceProvider, ICoreCmsUserBalanceServices userBalanceServices)
        {
            BaseDal = dal;
            _serviceProvider = serviceProvider;
            _userBalanceServices = userBalanceServices;
        }

        /// <summary>
        ///     发起支付
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> PubPay(CoreCmsBillPayments entity)
        {
            using (var container = _serviceProvider.CreateScope())
            {
                var userBalanceServices = container.ServiceProvider.GetService<ICoreCmsUserBalanceServices>();
                var billPaymentsServices = container.ServiceProvider.GetService<ICoreCmsBillPaymentsServices>();

                var jm = new WebApiCallBack();

                var result = await userBalanceServices.Change(entity.userId,
                    (int)GlobalEnumVars.UserBalanceSourceTypes.Pay, entity.money, entity.paymentId);
                if (!result.status)
                {
                    jm.msg = result.msg;
                    return jm;
                }

                //改变支付单状态
                var billPaymentInfo = await billPaymentsServices.QueryByIdAsync(entity.paymentId);
                if (billPaymentInfo == null)
                {
                    jm.msg = GlobalErrorCodeVars.Code10056;
                    jm.data = 10056;
                    return jm;
                }

                var userBalance = result.data as CoreCmsUserBalance;
                var resultBillPayment = await billPaymentsServices.ToUpdate(entity.paymentId,
                    (int)GlobalEnumVars.BillPaymentsStatus.Payed, "balancepay", entity.money, userBalance.memo,
                    userBalance.id.ToString());
                if (resultBillPayment.status)
                {
                    jm.msg = resultBillPayment.msg;
                    jm.status = true;
                    jm.data = entity;
                }
                else
                {
                    jm.msg = resultBillPayment.msg;
                }

                return jm;
            }
        }

        /// <summary>
        ///     用户余额退款
        /// </summary>
        /// <param name="refundInfo">退款单数据</param>
        /// <param name="paymentInfo">支付单数据</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Refund(CoreCmsBillRefund refundInfo, CoreCmsBillPayments paymentInfo)
        {
            var jm = new WebApiCallBack();

            if (refundInfo.money == 0)
            {
                jm.status = true;
                jm.msg = "退款成功";
                jm.data = new
                {
                    ReturnCode = "SUCCESS"
                };
                return jm;
            }

            var res = await _userBalanceServices.Change(paymentInfo.userId,
                (int)GlobalEnumVars.UserBalanceSourceTypes.Refund, refundInfo.money, paymentInfo.paymentId);
            if (res.status == false) return jm;
            jm.status = true;
            jm.data = res;
            jm.msg = "退款成功";

            return jm;
        }
    }
}