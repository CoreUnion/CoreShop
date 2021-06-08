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
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.Extensions.DependencyInjection;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 用户订阅提醒状态 接口实现
    /// </summary>
    public class CoreCmsUserWeChatMsgSubscriptionSwitchServices : BaseServices<CoreCmsUserWeChatMsgSubscriptionSwitch>, ICoreCmsUserWeChatMsgSubscriptionSwitchServices
    {
        private readonly ICoreCmsUserWeChatMsgSubscriptionSwitchRepository _dal;
        private readonly IServiceProvider _serviceProvider;

        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsUserWeChatMsgSubscriptionSwitchServices(IUnitOfWork unitOfWork, ICoreCmsUserWeChatMsgSubscriptionSwitchRepository dal, IServiceProvider serviceProvider)
        {
            this._dal = dal;
            _serviceProvider = serviceProvider;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }



        /// <summary>
        /// 获取用户是否订阅
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> IsTip(int userId)
        {
            var jm = new WebApiCallBack { data = true, otherData = false };

            using var container = _serviceProvider.CreateScope();

            var templateServices = container.ServiceProvider.GetService<ICoreCmsUserWeChatMsgTemplateServices>();
            var subscriptionServices = container.ServiceProvider.GetService<ICoreCmsUserWeChatMsgSubscriptionServices>();

            var setting = await templateServices.QueryAsync();
            var flag = false;
            foreach (var item in setting)
            {
                if (!string.IsNullOrEmpty(item.templateId))
                {
                    flag = true;
                    jm.otherData = true;
                    break;
                }
            }
            if (flag)
            {
                var res = await _dal.QueryByClauseAsync(p => p.userId == userId);
                if (res != null)
                {
                    if (res.isSwitch) jm.data = false;
                }
                else
                {
                    var count = await subscriptionServices.GetCountAsync(p => p.userId == userId);
                    if (count == setting.Count) jm.data = false;
                }
            }
            else
            {
                jm.data = false;
            }
            jm.status = true;
            jm.msg = "获取成功";

            return jm;
        }




        /// <summary>
        /// 获取用户是否关闭订阅
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CloseTip(int userId)
        {
            var jm = new WebApiCallBack();

            var res = await _dal.QueryByClauseAsync(p => p.userId == userId);
            if (res != null)
            {
                await _dal.UpdateAsync(
                    p => new CoreCmsUserWeChatMsgSubscriptionSwitch() { isSwitch = true },
                    p => p.userId == userId);
            }
            else
            {
                var st = new CoreCmsUserWeChatMsgSubscriptionSwitch();
                st.isSwitch = true;
                st.userId = userId;
                await _dal.InsertAsync(st);
            }


            jm.status = true;
            jm.msg = "关闭成功";

            jm.otherData = true; //是否关闭订阅

            return jm;
        }
    }
}
