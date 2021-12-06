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
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Sms;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 短信发送日志 接口实现
    /// </summary>
    public class CoreCmsSmsServices : BaseServices<CoreCmsSms>, ICoreCmsSmsServices
    {
        private readonly ICoreCmsSmsRepository _dal;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CoreCmsSmsServices(IUnitOfWork unitOfWork
            , ICoreCmsSmsRepository dal
            , IHttpContextAccessor httpContextAccessor, ICoreCmsSettingServices settingServices)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;

            _httpContextAccessor = httpContextAccessor;
            _settingServices = settingServices;
        }




        #region 发送短信（验证码）

        /// <summary>
        /// 发送短信（验证码）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> DoSendSms(string type, string mobile)
        {
            var jm = new WebApiCallBack();
            var smsOptions = await _settingServices.GetSmsOptions();
            if (smsOptions.Enabled == false)
            {
                jm.msg = "短信功能未开启";
                return jm;
            }

            Random rd = new Random();
            int codeNumber = rd.Next(100000, 999999);

            //获取是否存在
            var dt = DateTime.Now;
            var endDt = dt.AddMinutes(10);

            var oldLog = await _dal.QueryByClauseAsync(p => p.code == type && p.mobile == mobile && p.createTime > dt && p.createTime < endDt, p => p.id, OrderByType.Desc);
            if (oldLog == null)
            {
                oldLog = new CoreCmsSms();
                oldLog.code = type;
                oldLog.createTime = DateTime.Now;
                oldLog.mobile = mobile;
                oldLog.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";
                oldLog.isUsed = false;
                var obj = new
                {
                    code = codeNumber
                };
                switch (type)
                {
                    case "login":
                        oldLog.contentBody = "您本次登陆的验证码是：" + codeNumber + "，请不要将验证码泄露给他人！";
                        oldLog.parameters = JsonConvert.SerializeObject(obj);
                        break;
                    default:
                        oldLog.contentBody = "您验证码是：" + codeNumber + "，请不要将验证码泄露给他人！";
                        oldLog.parameters = JsonConvert.SerializeObject(obj);
                        break;
                }
                await _dal.InsertAsync(oldLog);
            }

            var str = SendSms(oldLog.mobile, oldLog.contentBody, smsOptions);
            jm.status = true;
            jm.data = str;
            jm.msg = "短信发送成功";

            return jm;
        }
        #endregion

        #region 校验短信验证码
        /// <summary>
        /// 校验短信验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="verCode"></param>
        /// <param name="code"></param>
        /// <returns></returns>

        public async Task<bool> Check(string phone, string verCode, string code)
        {
            var smsInfo = await _dal.QueryByClauseAsync(p =>
                p.mobile == phone && p.code == code && p.createTime < DateTime.Now && p.isUsed == false, p => p.createTime, OrderByType.Desc);
            if (smsInfo != null)
            {
                var parameters = JObject.Parse(smsInfo.parameters);
                if (parameters.ContainsKey("code"))
                {
                    var dataCode = parameters["code"]?.ToString();
                    if (dataCode != verCode) return false;
                    smsInfo.isUsed = true;
                    await _dal.UpdateAsync(smsInfo);
                    return true;
                }
                return false;
            }
            return false;
        }
        #endregion

        #region 接口通道发送短信

        /// <summary>
        /// 接口通道发送短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="contentBody"></param>
        /// <param name="smsOptions">配置文件</param>
        public string SendSms(string mobile, string contentBody, SMSOptions smsOptions)
        {
            if (smsOptions.Enabled)
            {
                string param = $@"action=send&userid={smsOptions.UserId}&account={smsOptions.Account}&password={smsOptions.Password}&content={"【" + smsOptions.Signature + "】" + contentBody}&mobile={mobile}";
                var str = HttpHelper.PostSend(smsOptions.ApiUrl, param);
                return str;
            }
            else
            {
                return "短信接口未开启";
            }
        }
        #endregion

        #region 发送短信统一方法
        /// <summary>
        /// 发送短信统一方法
        /// </summary>
        /// <param name="mobile">接受者手机号码</param>
        /// <param name="code">模板编码</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Send(string mobile, string code, JObject parameters)
        {
            var jm = new WebApiCallBack();
            var smsOptions = await _settingServices.GetSmsOptions();
            if (smsOptions.Enabled == false)
            {
                jm.msg = "短信功能未开启";
                return jm;
            }

            if (string.IsNullOrEmpty(mobile))
            {
                jm.msg = GlobalErrorCodeVars.Code11051;
                return jm;
            }
            var isUsed = false;
            if (code == GlobalEnumVars.SmsMessageTypes.Reg.ToString() || code == GlobalEnumVars.SmsMessageTypes.Login.ToString() || code == GlobalEnumVars.SmsMessageTypes.Veri.ToString())
            {
                var dt = DateTime.Now;
                var newCreateTime = DateTime.Now.AddSeconds(-60);
                var smsInfo = await _dal.QueryByClauseAsync(p =>
                    p.mobile == mobile && p.code == code && p.createTime < newCreateTime && p.isUsed == false);
                if (smsInfo != null)
                {
                    var ts = dt - smsInfo.createTime;
                    if (ts.Seconds < 60)
                    {
                        jm.msg = "两次发送时间间隔小于60秒";
                        return jm;
                    }
                    parameters = JObject.Parse(smsInfo.parameters); ;
                }
                else
                {
                    Random rd = new Random();
                    int codeNumber = rd.Next(100000, 999999);
                    if (parameters.ContainsKey("code"))
                    {
                        parameters.Remove("code");
                    }
                    parameters.Add("code", codeNumber);
                }
                isUsed = false;
            }
            else
            {
                isUsed = true;
            }


            var str = string.Empty;
            var allConfigs = await _settingServices.GetConfigDictionaries();

            if (code == GlobalEnumVars.SmsMessageTypes.Reg.ToString())
            {
                // 账户注册
                var msg = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SmsTplForReg);
                if (!string.IsNullOrEmpty(msg))
                {
                    var sendCode = string.Empty;
                    if (parameters.ContainsKey("code"))
                    {
                        sendCode = parameters["code"]?.ToString();
                    }
                    str = msg.Replace("{code}", sendCode);
                }
            }
            else if (code == GlobalEnumVars.SmsMessageTypes.Login.ToString())
            {
                // 账户登录
                var msg = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SmsTplForLogin);
                if (!string.IsNullOrEmpty(msg))
                {
                    var sendCode = string.Empty;
                    if (parameters.ContainsKey("code"))
                    {
                        sendCode = parameters["code"]?.ToString();
                    }
                    str = msg.Replace("{code}", sendCode);
                }
            }
            else if (code == GlobalEnumVars.SmsMessageTypes.Veri.ToString())
            {
                // 验证验证码
                var msg = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SmsTplForVeri);
                if (!string.IsNullOrEmpty(msg))
                {
                    var sendCode = string.Empty;
                    if (parameters.ContainsKey("code"))
                    {
                        sendCode = parameters["code"]?.ToString();
                    }
                    str = msg.Replace("{code}", sendCode);
                }
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.CreateOrder.ToString())
            {
                // 订单创建
                var msg = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SmsTplForCreateOrder);
                str = !string.IsNullOrEmpty(msg) ? msg : string.Empty;
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.OrderPayed.ToString())
            {
                // 订单支付通知买家
                var msg = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SmsTplForOrderPayed);
                str = !string.IsNullOrEmpty(msg) ? msg : string.Empty;
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.RemindOrderPay.ToString())
            {
                // 未支付催单
                var msg = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SmsTplForRemindOrderPay);
                str = !string.IsNullOrEmpty(msg) ? msg : string.Empty;
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.DeliveryNotice.ToString())
            {
                // 订单发货
                var msg = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SmsTplForDeliveryNotice);
                str = !string.IsNullOrEmpty(msg) ? msg : string.Empty;
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.AfterSalesPass.ToString())
            {
                // 售后审核通过
                var msg = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SmsTplForAfterSalesPass);
                str = !string.IsNullOrEmpty(msg) ? msg : string.Empty;
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.RefundSuccess.ToString())
            {
                // 退款已处理
                var msg = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SmsTplForRefundSuccess);
                str = !string.IsNullOrEmpty(msg) ? msg : string.Empty;
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.SellerOrderNotice.ToString())
            {
                // 订单支付通知卖家
                var msg = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SmsTplForSellerOrderNotice);
                str = !string.IsNullOrEmpty(msg) ? msg : string.Empty;
            }
            else if (code == GlobalEnumVars.PlatformMessageTypes.Common.ToString())
            {
                //通用类型
                var tpl = string.Empty;
                if (parameters.ContainsKey("tpl"))
                {
                    tpl = parameters["tpl"]?.ToString();
                }
                str = tpl;
                if (!string.IsNullOrEmpty(str))
                {
                    var msg = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SmsTplForCommon);
                    str = !string.IsNullOrEmpty(msg) ? msg : string.Empty;
                }
            }

            if (string.IsNullOrEmpty(str))
            {
                jm.msg = GlobalErrorCodeVars.Code10009;
                return jm;
            }

            var oldLog = new CoreCmsSms();
            oldLog.mobile = mobile;
            oldLog.code = code;
            oldLog.parameters = JsonConvert.SerializeObject(parameters);
            oldLog.contentBody = str;
            oldLog.createTime = DateTime.Now;
            oldLog.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ?
                _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";
            oldLog.isUsed = isUsed;

            await _dal.InsertAsync(oldLog);

            var result = SendSms(oldLog.mobile, oldLog.contentBody, smsOptions);

            jm.status = true;
            jm.msg = "发送成功";
            jm.data = result;

            return jm;
        }

        #endregion


    }
}
