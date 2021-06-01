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
    ///     提货单表 服务工厂接口
    /// </summary>
    public interface ICoreCmsBillLadingServices : IBaseServices<CoreCmsBillLading>
    {
        /// <summary>
        ///     添加提货单
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> AddData(string orderId, int storeId, string name, string mobile);


        /// <summary>
        ///     核销提货单
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> LadingOperating(string[] ids, int userId = 0);


        /// <summary>
        ///     获取店铺提货单列表
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetStoreLadingList(int userId, int page, int limit);


        /// <summary>
        ///     删除提货单(软删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> LadingDelete(string id, int userId = 0);

        /// <summary>
        ///     获取提货单详情
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetInfo(string id, int userId = 0);
    }
}