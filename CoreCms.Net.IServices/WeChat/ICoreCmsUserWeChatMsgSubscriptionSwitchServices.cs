/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     用户订阅提醒状态 服务工厂接口
    /// </summary>
    public interface
        ICoreCmsUserWeChatMsgSubscriptionSwitchServices : IBaseServices<CoreCmsUserWeChatMsgSubscriptionSwitch>
    {
        /// <summary>
        ///     获取用户是否订阅
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> IsTip(int userId);


        /// <summary>
        ///     获取用户是否订阅
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> CloseTip(int userId);
    }
}