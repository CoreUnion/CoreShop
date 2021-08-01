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
using CoreCms.Net.Caching.AutoMate.RedisCache;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 消息配置表 接口实现
    /// </summary>
    public class CoreCmsMessageCenterServices : BaseServices<CoreCmsMessageCenter>, ICoreCmsMessageCenterServices
    {
        private readonly ICoreCmsMessageCenterRepository _dal;

        private readonly IServiceProvider _serviceProvider;
        private readonly IRedisOperationRepository _redisOperationRepository;

        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsMessageCenterServices(IUnitOfWork unitOfWork, ICoreCmsMessageCenterRepository dal, IServiceProvider serviceProvider, ISysTaskLogServices taskLogServices, IRedisOperationRepository redisOperationRepository)
        {
            this._dal = dal;
            _serviceProvider = serviceProvider;
            _redisOperationRepository = redisOperationRepository;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 商家发送信息助手
        /// </summary>
        /// <param name="userId">接受者id</param>
        /// <param name="code">模板编码</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SendMessage(int userId, string code, JObject parameters)
        {
            try
            {
                var jm = new WebApiCallBack();

                using var container = _serviceProvider.CreateScope();

                var userServices = container.ServiceProvider.GetService<ICoreCmsUserServices>();
                var settingServices = container.ServiceProvider.GetService<ICoreCmsSettingServices>();
                var smsServices = container.ServiceProvider.GetService<ICoreCmsSmsServices>();
                var messageServices = container.ServiceProvider.GetService<ICoreCmsMessageServices>();
                var allConfigs = await settingServices.GetConfigDictionaries();

                var config = await _dal.QueryByClauseAsync(p => p.code == code);
                if (config == null)
                {
                    jm.msg = GlobalErrorCodeVars.Code10100;
                    return jm;
                }
                if (config.isSms)
                {
                    //判断短信是否够,如果够，就去发
                    var user = await userServices.QueryByClauseAsync(p => p.id == userId);
                    if (user != null && !string.IsNullOrEmpty(user.mobile))
                    {
                        var mobile = user.mobile;
                        //判断是否平台通知
                        if (code == GlobalEnumVars.PlatformMessageTypes.SellerOrderNotice.ToString())
                        {
                            mobile = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ShopMobile);
                        }
                        //发货时，短信通知用发货人的
                        if (code == GlobalEnumVars.PlatformMessageTypes.DeliveryNotice.ToString())
                        {
                            if (parameters.ContainsKey("shipMobile"))
                            {
                                mobile = parameters["shipMobile"].ObjectToString();
                            }
                        }
                        if (!string.IsNullOrEmpty(mobile))
                        {
                            await smsServices.Send(mobile, code, parameters);
                        }
                    }
                }
                //站内消息
                if (config.isMessage && code != GlobalEnumVars.PlatformMessageTypes.SellerOrderNotice.ToString())
                {
                    await messageServices.Send(userId, code, parameters);
                }
                //微信模板消息【小程序，公众号都走这里】
                if (config.isWxTempletMessage &&
                    (code == GlobalEnumVars.PlatformMessageTypes.OrderPayed.ToString() || code == GlobalEnumVars.PlatformMessageTypes.DeliveryNotice.ToString() || code == GlobalEnumVars.PlatformMessageTypes.RemindOrderPay.ToString()))
                {
                    var @params = new JObject();
                    @params.Add("parameters", parameters);

                    var data = new
                    {
                        userId,
                        code,
                        parameters = @params
                    };

                    //队列推送消息
                    await _redisOperationRepository.ListLeftPushAsync(RedisMessageQueueKey.SendWxTemplateMessage, JsonConvert.SerializeObject(data));
                }
                jm.status = true;
                return jm;
            }
            catch (Exception ex)
            {
                NLogUtil.WriteAll(LogLevel.Trace, LogType.RefundResultNotification, "商家发送信息助手", JsonConvert.SerializeObject(ex));
                throw;
            }
        }

    }
}
