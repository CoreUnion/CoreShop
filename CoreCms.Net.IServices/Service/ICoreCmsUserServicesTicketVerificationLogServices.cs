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
    ///     服务券核验日志 服务工厂接口
    /// </summary>
    public interface
        ICoreCmsUserServicesTicketVerificationLogServices : IBaseServices<CoreCmsUserServicesTicketVerificationLog>
    {
        /// <summary>
        ///     店铺核销的服务券列表
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetVerificationLogs(int userId, int page, int limit);


        /// <summary>
        ///     删除服务券核销单(软删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> LogDelete(int id, int userId = 0);
    }
}