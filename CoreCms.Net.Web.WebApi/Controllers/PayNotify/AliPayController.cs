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
using System.Xml;
using Essensoft.Paylink.Alipay;
using Essensoft.Paylink.Alipay.Notify;
using Essensoft.Paylink.Alipay.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreCms.Net.Web.WebApi.Controllers.PayNotify
{
    /// <summary>
    /// 支付宝异步通知
    /// </summary>
    [Route("Notify/[controller]/[action]")]
    public class AliPayController : ControllerBase
    {
        private readonly IAlipayNotifyClient _client;
        private readonly IOptions<AlipayOptions> _optionsAccessor;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client"></param>
        /// <param name="optionsAccessor"></param>
        public AliPayController(IAlipayNotifyClient client, IOptions<AlipayOptions> optionsAccessor)
        {
            _client = client;
            _optionsAccessor = optionsAccessor;
        }

        /// <summary>
        /// 应用网关
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Gateway()
        {
            try
            {
                var service = Request.Form["service"].ToString();
                switch (service)
                {
                    // 激活开发者模式
                    case "alipay.service.check":
                        {
                            var options = _optionsAccessor.Value;

                            // 获取参数
                            var parameters =await _client.GetParametersAsync(Request);
                            var sign = parameters["sign"];
                            parameters.Remove("sign");

                            var signContent = AlipaySignature.GetSignContent(parameters);

                            // 验签
                            //var isSuccess = AlipaySignature.RSACheckContent(signContent, sign, options.AlipayPublicKey, options.SignType);
                            // 验签
                            var isSuccess = AlipaySignature.RSACheckContent(signContent, sign, options.AlipayPublicKey, options.SignType);

                            // 组XML响应内容
                            //var response = MakeVerifyGWResponse(isSuccess, options.AlipayPublicKey, options.AppPrivateKey, options.SignType);
                            var response = MakeVerifyGwResponse(isSuccess, options.AlipayPublicKey, options.AppPrivateKey, options.SignType);


                            return Content(response, "text/xml");
                        }
                }

                var msg_method = Request.Form["msg_method"].ToString();
                switch (msg_method)
                {
                    // 资金单据状态变更通知
                    case "alipay.fund.trans.order.changed":
                        {
                            var notify = await _client.CertificateExecuteAsync<AlipayFundTransOrderChangedNotify>(Request, _optionsAccessor.Value);
                            return AlipayNotifyResult.Success;
                        }
                    // 第三方应用授权取消消息
                    case "alipay.open.auth.appauth.cancelled":
                        {
                            var notify = await _client.CertificateExecuteAsync<AlipayOpenAuthAppauthCancelledNotify>(Request, _optionsAccessor.Value);
                            return AlipayNotifyResult.Success;
                        }
                    // 用户授权取消消息
                    case "alipay.open.auth.userauth.cancelled":
                        {
                            var notify = await _client.CertificateExecuteAsync<AlipayOpenAuthUserauthCancelledNotify>(Request, _optionsAccessor.Value);
                            return AlipayNotifyResult.Success;
                        }
                    // 小程序审核通过通知
                    case "alipay.open.mini.version.audit.passed":
                        {
                            var notify = await _client.CertificateExecuteAsync<AlipayOpenMiniVersionAuditPassedNotify>(Request, _optionsAccessor.Value);
                            return AlipayNotifyResult.Success;
                        }
                    // 用户授权取消消息
                    case "alipay.open.mini.version.audit.rejected":
                        {
                            var notify = await _client.CertificateExecuteAsync<AlipayOpenMiniVersionAuditRejectedNotify>(Request, _optionsAccessor.Value);
                            return AlipayNotifyResult.Success;
                        }
                    // 收单资金结算到银行账户，结算退票的异步通知
                    case "alipay.trade.settle.dishonoured":
                        {
                            var notify = await _client.CertificateExecuteAsync<AlipayTradeSettleDishonouredNotify>(Request, _optionsAccessor.Value);
                            return AlipayNotifyResult.Success;
                        }
                    // 收单资金结算到银行账户，结算失败的异步通知
                    case "alipay.trade.settle.fail":
                        {
                            var notify = await _client.CertificateExecuteAsync<AlipayTradeSettleFailNotify>(Request, _optionsAccessor.Value);
                            return AlipayNotifyResult.Success;
                        }
                    // 收单资金结算到银行账户，结算成功的异步通知
                    case "alipay.trade.settle.success":
                        {
                            var notify = await _client.CertificateExecuteAsync<AlipayTradeSettleSuccessNotify>(Request, _optionsAccessor.Value);
                            return AlipayNotifyResult.Success;
                        }
                }

                return NoContent();
            }
            catch
            {
                return NoContent();
            }
        }

        /// <summary>
        /// 扫码支付异步通知
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Precreate()
        {
            try
            {
                var notify = await _client.CertificateExecuteAsync<AlipayTradePrecreateNotify>(Request, _optionsAccessor.Value);
                if (notify.TradeStatus == AlipayTradeStatus.Success)
                {
                    Console.WriteLine("OutTradeNo: " + notify.OutTradeNo);

                    return AlipayNotifyResult.Success;
                }
                return NoContent();
            }
            catch
            {
                return NoContent();
            }
        }

        /// <summary>
        /// APP支付异步通知
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AppPay()
        {
            try
            {
                var notify = await _client.CertificateExecuteAsync<AlipayTradeAppPayNotify>(Request, _optionsAccessor.Value);
                if (notify.TradeStatus == AlipayTradeStatus.Success)
                {
                    Console.WriteLine("OutTradeNo: " + notify.OutTradeNo);

                    return AlipayNotifyResult.Success;
                }
                return NoContent();
            }
            catch
            {
                return NoContent();
            }
        }

        /// <summary>
        /// 电脑网站支付异步通知
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PagePay()
        {
            try
            {
                var notify = await _client.CertificateExecuteAsync<AlipayTradePagePayNotify>(Request, _optionsAccessor.Value);
                if (notify.TradeStatus == AlipayTradeStatus.Success)
                {
                    Console.WriteLine("OutTradeNo: " + notify.OutTradeNo);

                    return AlipayNotifyResult.Success;
                }
                return NoContent();
            }
            catch
            {
                return NoContent();
            }
        }

        /// <summary>
        /// 手机网站支付异步通知
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> WapPay()
        {
            try
            {
                var notify = await _client.CertificateExecuteAsync<AlipayTradeWapPayNotify>(Request, _optionsAccessor.Value);
                if (notify.TradeStatus == AlipayTradeStatus.Success)
                {
                    Console.WriteLine("OutTradeNo: " + notify.OutTradeNo);

                    return AlipayNotifyResult.Success;
                }
                return NoContent();
            }
            catch
            {
                return NoContent();
            }
        }

        private static string MakeVerifyGwResponse(bool isSuccess, string certPublicKey, string appPrivateKey, string signType)
        {
            var xmlDoc = new XmlDocument(); //创建实例
            var xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "GBK", null);
            xmlDoc.AppendChild(xmldecl);

            var xmlElem = xmlDoc.CreateElement("alipay"); //新建元素
            xmlDoc.AppendChild(xmlElem); //添加元素

            var alipay = xmlDoc.SelectSingleNode("alipay");

            var response = xmlDoc.CreateElement("response");
            var success = xmlDoc.CreateElement("success");
            if (isSuccess)
            {
                success.InnerText = "true";//设置文本节点
                response.AppendChild(success);//添加到<Node>节点中
            }
            else
            {
                success.InnerText = "false";//设置文本节点
                response.AppendChild(success);//添加到<Node>节点中
                var err = xmlDoc.CreateElement("error_code");
                err.InnerText = "VERIFY_FAILED";
                response.AppendChild(err);
            }

            var bizContent = xmlDoc.CreateElement("biz_content");
            bizContent.InnerText = certPublicKey;
            response.AppendChild(bizContent);

            alipay.AppendChild(response);

            var sign = xmlDoc.CreateElement("sign");
            //sign.InnerText = AlipaySignature.RSASignContent(response.InnerXml, appPrivateKey, signType);
            sign.InnerText = AlipaySignature.RSASignContent(response.InnerXml, appPrivateKey, signType);

            alipay.AppendChild(sign);

            var sign_type = xmlDoc.CreateElement("sign_type");
            sign_type.InnerText = signType;
            alipay.AppendChild(sign_type);

            return xmlDoc.InnerXml;
        }




    }
}