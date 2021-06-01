/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-02-26 0:57:51
 *        Description: 暂无
 ***********************************************************************/


using CoreCms.Net.Configuration;
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.Utility.Helper
{
    /// <summary>
    /// 短信相关帮助类
    /// </summary>
    public class SmsHelper
    {
        /// <summary>
        /// 根据消息分类和传输参数,获取要发送的内容
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="parameters">参数类型</param>
        /// <returns></returns>
        public static string GetTemp(string type, JObject parameters)
        {
            var msg = string.Empty;
            if (type == GlobalEnumVars.SmsMessageTypes.Reg.ToString())
            {
                // 账户注册
                var code = string.Empty;
                if (parameters.ContainsKey("code"))
                {
                    code = parameters["code"]?.ToString();
                }
                msg = "您正在注册账号，验证码是" + code + "，请勿告诉他人。";
            }
            else if (type == GlobalEnumVars.SmsMessageTypes.Login.ToString())
            {
                // 账户登录
                var code = string.Empty;
                if (parameters.ContainsKey("code"))
                {
                    code = parameters["code"]?.ToString();
                }
                msg = "您正在登陆账号，验证码是" + code + "，请勿告诉他人。";
            }
            else if (type == GlobalEnumVars.SmsMessageTypes.Veri.ToString())
            {
                // 验证验证码
                var code = string.Empty;
                if (parameters.ContainsKey("code"))
                {
                    code = parameters["code"]?.ToString();
                }
                msg = "您的验证码是" + code + "，请勿告诉他人。";
            }
            else if (type == GlobalEnumVars.PlatformMessageTypes.CreateOrder.ToString())
            {
                // 订单创建
                msg = "恭喜您，订单创建成功，祝您购物愉快。";
            }
            else if (type == GlobalEnumVars.PlatformMessageTypes.OrderPayed.ToString())
            {
                // 订单支付通知买家
                msg = "恭喜您，订单支付成功，祝您购物愉快。";
            }
            else if (type == GlobalEnumVars.PlatformMessageTypes.RemindOrderPay.ToString())
            {
                // 未支付催单
                msg = "您的订单还有1个小时就要取消了，请及时进行支付。";
            }
            else if (type == GlobalEnumVars.PlatformMessageTypes.DeliveryNotice.ToString())
            {
                // 订单发货
                msg = "您好，您的订单已经发货。";
            }
            else if (type == GlobalEnumVars.PlatformMessageTypes.AfterSalesPass.ToString())
            {
                // 售后审核通过
                msg = "您好，您的售后已经通过。";
            }
            else if (type == GlobalEnumVars.PlatformMessageTypes.RefundSuccess.ToString())
            {
                // 退款已处理
                msg = "用户您好，您的退款已经处理，请确认。";
            }
            else if (type == GlobalEnumVars.PlatformMessageTypes.SellerOrderNotice.ToString())
            {
                // 订单支付通知卖家
                msg = "您有新的订单了，请及时处理。";
            }
            else if (type == GlobalEnumVars.PlatformMessageTypes.Common.ToString())
            {
                //通用类型
                var tpl = string.Empty;
                if (parameters.ContainsKey("tpl"))
                {
                    tpl = parameters["tpl"]?.ToString();
                }
                msg = tpl;
            }
            return msg;
        }

        /// <summary>
        /// //记录哪里需要发送消息，统一处理
        /// </summary>
        public static void SendMessage()
        {
            //记录哪里需要发送消息，统一处理
        }
    }
}
