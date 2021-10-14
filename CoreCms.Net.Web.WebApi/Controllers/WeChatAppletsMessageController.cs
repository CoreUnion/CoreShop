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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 微信小程序消息订阅接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeChatAppletsMessageController : ControllerBase
    {
        private readonly IHttpContextUser _user;
        private readonly ICoreCmsUserWeChatMsgTemplateServices _userWeChatMsgTemplateServices;
        private readonly ICoreCmsUserWeChatMsgSubscriptionSwitchServices _userWeChatMsgSubscriptionSwitchServices;
        private readonly ICoreCmsUserWeChatMsgSubscriptionServices _userWeChatMsgSubscriptionServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WeChatAppletsMessageController(IHttpContextUser user, ICoreCmsUserWeChatMsgTemplateServices userWeChatMsgTemplateServices, ICoreCmsUserWeChatMsgSubscriptionSwitchServices userWeChatMsgSubscriptionSwitchServices, ICoreCmsUserWeChatMsgSubscriptionServices userWeChatMsgSubscriptionServices)
        {
            _user = user;
            _userWeChatMsgTemplateServices = userWeChatMsgTemplateServices;
            _userWeChatMsgSubscriptionSwitchServices = userWeChatMsgSubscriptionSwitchServices;
            _userWeChatMsgSubscriptionServices = userWeChatMsgSubscriptionServices;
        }

        #region 获取用户是否订阅
        /// <summary>
        /// 获取用户是否订阅
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> IsTip()
        {
            var jm = await _userWeChatMsgSubscriptionSwitchServices.IsTip(_user.ID);
            return jm;
        }
        #endregion

        #region 用户取消订阅
        /// <summary>
        /// 用户取消订阅
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> CloseTip()
        {
            var jm = await _userWeChatMsgSubscriptionSwitchServices.CloseTip(_user.ID);
            return jm;
        }

        #endregion

        #region 获取订阅模板
        /// <summary>
        /// 获取订阅模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> Tmpl()
        {
            var jm = await _userWeChatMsgSubscriptionServices.tmpl(_user.ID);

            return jm;
        }

        #endregion

        #region 设置订阅信息
        /// <summary>
        /// 设置订阅信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> SetTip([FromBody] SetWeChatAppletsMessageTip entity)
        {
            var jm = await _userWeChatMsgSubscriptionServices.SetTip(_user.ID, entity.templateId, entity.status);
            return jm;
        }
        #endregion

    }
}
