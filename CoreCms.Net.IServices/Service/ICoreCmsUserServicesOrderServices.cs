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
    ///     服务购买表 服务工厂接口
    /// </summary>
    public interface ICoreCmsUserServicesOrderServices : IBaseServices<CoreCmsUserServicesOrder>
    {
        /// <summary>
        ///     完成服务订单后生成兑换券
        /// </summary>
        /// <param name="serviceOrderId"></param>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> CreateUserServicesTickets(string serviceOrderId, string paymentId);
    }
}