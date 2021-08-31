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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 退款单表 接口实现
    /// </summary>
    public class CoreCmsBillRefundServices : BaseServices<CoreCmsBillRefund>, ICoreCmsBillRefundServices
    {
        private readonly ICoreCmsBillRefundRepository _dal;
        private readonly ICoreCmsBillPaymentsServices _billPaymentsServices;
        private readonly ICoreCmsMessageCenterServices _messageCenterServices;
        private readonly ICoreCmsPaymentsServices _paymentsServices;
        private readonly IBalancePayServices _balancePayServices;
        private readonly IAliPayServices _aliPayServices;
        private readonly IWeChatPayServices _weChatPayServices;


        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsBillRefundServices(IUnitOfWork unitOfWork, ICoreCmsBillRefundRepository dal, ICoreCmsBillPaymentsServices billPaymentsServices, ICoreCmsMessageCenterServices messageCenterServices, ICoreCmsPaymentsServices paymentsServices, IBalancePayServices balancePayServices, IAliPayServices aliPayServices, IWeChatPayServices weChatPayServices)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _billPaymentsServices = billPaymentsServices;
            _messageCenterServices = messageCenterServices;
            _paymentsServices = paymentsServices;
            _balancePayServices = balancePayServices;
            _aliPayServices = aliPayServices;
            _weChatPayServices = weChatPayServices;
        }


        /// <summary>
        /// 创建退款单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sourceId"></param>
        /// <param name="type"></param>
        /// <param name="money"></param>
        /// <param name="aftersalesId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ToAdd(int userId, string sourceId, int type, decimal money, string aftersalesId)
        {
            var jm = new WebApiCallBack();

            if (money == 0)
            {
                jm.data = jm.code = 13208;
                jm.msg = GlobalErrorCodeVars.Code13208;
                return jm;
            }
            //创建退款单
            var billRefund = new CoreCmsBillRefund();
            billRefund.refundId = CommonHelper.GetSerialNumberType((int)GlobalEnumVars.SerialNumberType.退款单编号);
            billRefund.aftersalesId = aftersalesId;
            billRefund.money = money;
            billRefund.userId = userId;
            billRefund.sourceId = sourceId;
            billRefund.type = type;
            //取支付成功的支付单号
            var paymentsInfo = await _billPaymentsServices.QueryByClauseAsync(p => p.sourceId == sourceId && p.type == type && p.status == (int)GlobalEnumVars.BillPaymentsStatus.Payed);
            if (paymentsInfo != null)
            {
                billRefund.paymentCode = paymentsInfo.paymentCode;
                billRefund.tradeNo = paymentsInfo.tradeNo;
            }
            billRefund.status = (int)GlobalEnumVars.BillRefundStatus.STATUS_NOREFUND;
            billRefund.createTime = DateTime.Now;

            await _dal.InsertAsync(billRefund);

            jm.status = true;
            jm.msg = "创建成功";

            return jm;
        }


        /// <summary>
        /// 退款单去退款或者拒绝
        /// </summary>
        /// <param name="refundId">退款单id</param>
        /// <param name="status">2或者4，通过或者拒绝</param>
        /// <param name="paymentCodeStr">退款方式，如果和退款单上的一样，说明没有修改，原路返回，否则只记录状态，不做实际退款,如果为空是原路返回</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ToRefund(string refundId, int status, string paymentCodeStr = "")
        {
            var jm = new WebApiCallBack();

            var info = await _dal.QueryByClauseAsync(p => p.refundId == refundId && p.status == (int)GlobalEnumVars.BillRefundStatus.STATUS_NOREFUND);

            if (info == null)
            {
                jm.status = false;
                jm.msg = GlobalErrorCodeVars.Code13210;
                return jm;
            }

            if (paymentCodeStr == "")
            {
                paymentCodeStr = info.paymentCode;
            }

            if (status == (int)GlobalEnumVars.BillRefundStatus.STATUS_REFUND)
            {
                //退款完成后的钩子
                jm.msg = "退款单退款成功";

                //如果前端传过来的退款方式和退款单上的退款方式一样的话，就说明是原路返回，试着调用支付方式的退款方法,如果不一样的话，就直接做退款单的退款状态为已退款就可以了
                if (paymentCodeStr == info.paymentCode && paymentCodeStr != "offline")
                {
                    jm = await PaymentRefund(refundId);
                }
                else
                {
                    //只修改状态，不做实际退款，实际退款线下去退。
                    await _dal.UpdateAsync(p => new CoreCmsBillRefund()
                    {
                        status = (int)GlobalEnumVars.BillRefundStatus.STATUS_REFUND,
                        paymentCode = paymentCodeStr
                    },
                        p => p.refundId == refundId && p.status == (int)GlobalEnumVars.BillRefundStatus.STATUS_NOREFUND);
                    jm.status = true;
                }

                //退款同意，先发退款消息和钩子，下面原路返回可能失败，但是在业务上相当于退款已经退过了，只是实际的款项可能还没到账
                //发送退款消息
                await _messageCenterServices.SendMessage(info.userId, GlobalEnumVars.PlatformMessageTypes.RefundSuccess.ToString(), JObject.FromObject(info));

                return jm;
            }
            else if (status == (int)GlobalEnumVars.BillRefundStatus.STATUS_REFUSE)
            {
                //退款拒绝
                await _dal.UpdateAsync(
                    p => new CoreCmsBillRefund()
                    {
                        status = status,
                        paymentCode = paymentCodeStr
                    },
                    p => p.refundId == refundId && p.status == (int)GlobalEnumVars.BillRefundStatus.STATUS_NOREFUND);
                jm.status = true;
                jm.msg = "退款单拒绝成功";
            }
            else
            {
                jm.status = false;
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }
            return jm;
        }

        /// <summary>
        /// 如果是在线支付的原路退还，去做退款操作
        /// </summary>
        public async Task<WebApiCallBack> PaymentRefund(string refundId)
        {

            var jm = new WebApiCallBack();

            var info = await _dal.QueryByClauseAsync(p =>
                p.refundId == refundId && p.status != (int)GlobalEnumVars.BillRefundStatus.STATUS_REFUND);
            if (info == null)
            {
                jm.status = false;
                jm.msg = GlobalErrorCodeVars.Code13210;
                return jm;
            }

            //取支付成功的支付单号
            var panyMentsInfo = await _billPaymentsServices.QueryByClauseAsync(p => p.sourceId == info.sourceId && p.type == info.type && p.status == (int)GlobalEnumVars.BillPaymentsStatus.Payed);
            if (panyMentsInfo == null)
            {
                jm.msg = "没有找到支付成功的支付单号";
                return jm;
            }
            if (panyMentsInfo.paymentCode != info.paymentCode)
            {
                jm.msg = "退款单退款方式和支付方式不一样，原路退还失败";
                return jm;
            }
            //取此支付方式的信息
            var paymentsModel = await _paymentsServices.QueryByClauseAsync(p => p.code == info.paymentCode && p.isEnable == true);
            if (paymentsModel == null)
            {
                jm.msg = GlobalErrorCodeVars.Code10050;
                return jm;
            }

            //去退款
            //微信退款
            if (panyMentsInfo.paymentCode == GlobalEnumVars.PaymentsTypes.wechatpay.ToString())
            {
                jm = await _weChatPayServices.Refund(info, panyMentsInfo);
            }
            //支付宝退款
            else if (panyMentsInfo.paymentCode == GlobalEnumVars.PaymentsTypes.alipay.ToString())
            {
                jm.status = false;
                jm.msg = "支付宝退款未开通";
            }
            //余额退款
            else if (panyMentsInfo.paymentCode == GlobalEnumVars.PaymentsTypes.balancepay.ToString())
            {
                jm = await _balancePayServices.Refund(info, panyMentsInfo);
            }

            if (jm.status)
            {
                var res = JsonConvert.SerializeObject(jm.data);
                await _dal.UpdateAsync(p => new CoreCmsBillRefund() { status = (int)GlobalEnumVars.BillRefundStatus.STATUS_REFUND, memo = res }, p => p.refundId == refundId);
            }
            else
            {
                var res = JsonConvert.SerializeObject(jm.data);
                await _dal.UpdateAsync(p => new CoreCmsBillRefund() { status = (int)GlobalEnumVars.BillRefundStatus.STATUS_FAIL, memo = res }, p => p.refundId == refundId);
            }

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
        public new async Task<IPageList<CoreCmsBillRefund>> QueryPageAsync(Expression<Func<CoreCmsBillRefund, bool>> predicate,
            Expression<Func<CoreCmsBillRefund, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion

    }
}
