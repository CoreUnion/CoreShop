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
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 微信订阅消息存储表 接口实现
    /// </summary>
    public class CoreCmsUserWeChatMsgSubscriptionServices : BaseServices<CoreCmsUserWeChatMsgSubscription>, ICoreCmsUserWeChatMsgSubscriptionServices
    {
        private readonly ICoreCmsUserWeChatMsgSubscriptionRepository _dal;
        private readonly IServiceProvider _serviceProvider;

        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsUserWeChatMsgSubscriptionServices(IUnitOfWork unitOfWork, ICoreCmsUserWeChatMsgSubscriptionRepository dal, IServiceProvider serviceProvider)
        {
            this._dal = dal;
            _serviceProvider = serviceProvider;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 获取模板信息
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> tmpl(int userId)
        {
            var jm = new WebApiCallBack();

            using var container = _serviceProvider.CreateScope();

            var templateServices = container.ServiceProvider.GetService<ICoreCmsUserWeChatMsgTemplateServices>();

            //支付通知|发货通知
            var arr = new string[] { "pay", "ship", "cancel" };

            var list = await templateServices.QueryListByClauseAsync(p => arr.Contains(p.templateTitle), p => p.id, OrderByType.Asc);
            var arrTitle = list.Select(p => p.templateId).ToList();

            jm.status = true;
            jm.data = arrTitle;
            jm.msg = "获取成功";

            return jm;
        }


        /// <summary>
        /// 设置订阅状态
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> SetTip(int userId, string templateId, string status)
        {
            var jm = new WebApiCallBack();

            using var container = _serviceProvider.CreateScope();

            var templateServices = container.ServiceProvider.GetService<ICoreCmsUserWeChatMsgTemplateServices>();
            var subscriptionServices = container.ServiceProvider.GetService<ICoreCmsUserWeChatMsgSubscriptionServices>();

            var setting = await templateServices.QueryAsync();
            var type = "";

            if (setting.Any())
            {
                foreach (var item in setting)
                {
                    if (item.templateId == templateId)
                    {
                        type = item.templateTitle;
                        break;
                    }
                }
            }
            var count = await subscriptionServices.GetCountAsync(p => p.userId == userId && p.type == type);

            if (status == "accept")
            {
                if (count < 1)
                {
                    var sub = new CoreCmsUserWeChatMsgSubscription();
                    sub.userId = userId;
                    sub.templateId = templateId;
                    sub.type = type;
                    await subscriptionServices.InsertAsync(sub);
                }
                else
                {
                    await subscriptionServices.UpdateAsync(
                        p => new CoreCmsUserWeChatMsgSubscription() { templateId = templateId },
                        p => p.userId == userId && p.type == type);
                }
            }
            else
            {
                if (count > 0)
                {
                    await subscriptionServices.DeleteAsync(p => p.userId == userId && p.type == type);
                }
            }
            jm.status = true;
            jm.msg = "设置订阅状态成功";

            return jm;
        }

    }
}
