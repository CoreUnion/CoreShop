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
using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using Essensoft.Paylink.WeChatPay;
using Essensoft.Paylink.WeChatPay.V2;
using Essensoft.Paylink.WeChatPay.V2.Request;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     微信支付 接口实现
    /// </summary>
    public class WeChatPayServices : BaseServices<CoreCmsSetting>, IWeChatPayServices
    {
        private readonly IWeChatPayClient _client;
        private readonly IOptions<WeChatPayOptions> _optionsAccessor;
        private readonly IHttpContextUser _user;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsUserWeChatInfoServices _userWeChatInfoServices;

        public WeChatPayServices(IHttpContextUser user
            , IWeChatPayClient client
            , IOptions<WeChatPayOptions> optionsAccessor
            , ICoreCmsUserServices userServices
            , ICoreCmsUserWeChatInfoServices userWeChatInfoServices
        )
        {

            _client = client;
            _optionsAccessor = optionsAccessor;
            _user = user;
            _userServices = userServices;
            _userWeChatInfoServices = userWeChatInfoServices;
        }

        /// <summary>
        ///     发起支付
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> PubPay(CoreCmsBillPayments entity)
        {
            var jm = new WebApiCallBack();

            var weChatPayUrl = AppSettingsConstVars.PayCallBackWeChatPayUrl;
            if (string.IsNullOrEmpty(weChatPayUrl))
            {
                jm.msg = "未获取到配置的通知地址";
                return jm;
            }

            var tradeType = GlobalEnumVars.WeiChatPayTradeType.JSAPI.ToString();
            if (!string.IsNullOrEmpty(entity.parameters))
            {
                var jobj = (JObject)JsonConvert.DeserializeObject(entity.parameters);
                if (jobj != null && jobj.ContainsKey("trade_type"))
                    tradeType = GetTradeType(jobj["trade_type"].ObjectToString());
            }

            var openId = string.Empty;
            if (tradeType == GlobalEnumVars.WeiChatPayTradeType.JSAPI.ToString())
            {
                var userAccount = await _userServices.QueryByIdAsync(_user.ID);
                if (userAccount == null)
                {
                    jm.msg = "用户账户获取失败";
                    return jm;
                }

                if (userAccount.userWx <= 0)
                {
                    jm.msg = "账户关联微信用户信息获取失败";
                    return jm;
                }

                var user = await _userWeChatInfoServices.QueryByClauseAsync(p => p.id == userAccount.userWx);
                if (user == null)
                {
                    jm.msg = "微信用户信息获取失败";
                    return jm;
                }

                openId = user.openid;
            }

            var request = new WeChatPayUnifiedOrderRequest
            {
                Body = entity.payTitle.Length > 50 ? entity.payTitle[..50] : entity.payTitle,
                OutTradeNo = entity.paymentId,
                TotalFee = Convert.ToInt32(entity.money * 100),
                SpBillCreateIp = entity.ip,
                NotifyUrl = weChatPayUrl,
                TradeType = tradeType,
                OpenId = openId
            };

            var response = await _client.ExecuteAsync(request, _optionsAccessor.Value);
            if (response.ReturnCode == WeChatPayCode.Success && response.ResultCode == WeChatPayCode.Success)
            {
                var req = new WeChatPayJsApiSdkRequest
                {
                    Package = "prepay_id=" + response.PrepayId
                };

                var parameter = await _client.ExecuteAsync(req, _optionsAccessor.Value);
                // 将参数(parameter)给 公众号前端 让他在微信内H5调起支付(https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7&index=6)
                parameter.Add("paymentId", entity.paymentId);

                jm.status = true;
                jm.msg = "支付成功";
                jm.data = parameter;
                jm.otherData = response;
            }
            else
            {
                jm.status = false;
                jm.msg = "微信建立支付请求失败";
                jm.otherData = response;
            }

            return jm;
        }

        /// <summary>
        ///     用户退款
        /// </summary>
        /// <param name="refundInfo">退款单数据</param>
        /// <param name="paymentInfo">支付单数据</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Refund(CoreCmsBillRefund refundInfo, CoreCmsBillPayments paymentInfo)
        {
            var jm = new WebApiCallBack();

            var weChatRefundUrl = AppSettingsConstVars.PayCallBackWeChatRefundUrl;
            if (string.IsNullOrEmpty(weChatRefundUrl))
            {
                jm.msg = "未获取到配置的通知地址";
                return jm;
            }

            var request = new WeChatPayRefundRequest
            {
                OutRefundNo = refundInfo.refundId,
                TransactionId = paymentInfo.tradeNo,
                OutTradeNo = paymentInfo.paymentId,
                TotalFee = Convert.ToInt32(paymentInfo.money * 100),
                RefundFee = Convert.ToInt32(refundInfo.money * 100),
                NotifyUrl = weChatRefundUrl
            };
            var response = await _client.ExecuteAsync(request, _optionsAccessor.Value);

            if (response.ReturnCode == WeChatPayCode.Success && response.ResultCode == WeChatPayCode.Success)
            {
                jm.status = true;
                jm.msg = "退款成功";
                jm.data = response;
            }
            else
            {
                jm.status = false;
                jm.msg = "退款失败";
                jm.data = response;
            }

            return jm;
        }

        private static string GetTradeType(string tradeType)
        {
            if (tradeType != GlobalEnumVars.WeiChatPayTradeType.JSAPI.ToString() &&
                tradeType != GlobalEnumVars.WeiChatPayTradeType.JSAPI_OFFICIAL.ToString() &&
                tradeType != GlobalEnumVars.WeiChatPayTradeType.NATIVE.ToString() &&
                tradeType != GlobalEnumVars.WeiChatPayTradeType.APP.ToString() &&
                tradeType != GlobalEnumVars.WeiChatPayTradeType.MWEB.ToString()
            )
                return "JSAPI";
            if (tradeType == GlobalEnumVars.WeiChatPayTradeType.JSAPI_OFFICIAL.ToString())
                return "JSAPI";
            return tradeType;
        }
    }
}