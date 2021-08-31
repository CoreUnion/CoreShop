/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO.Distribution;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     分销商表 服务工厂接口
    /// </summary>
    public interface ICoreCmsDistributionServices : IBaseServices<CoreCmsDistribution>
    {
        /// <summary>
        ///     获取分销商信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="checkStatus">是否检查满足条件</param>
        /// <returns></returns>
        Task<WebApiCallBack> GetInfo(int userId, bool checkStatus = false);

        /// <summary>
        ///     添加用户信息
        /// </summary>
        /// <param name="iData"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> AddData(CoreCmsDistribution iData, int userId);


        /// <summary>
        ///     获取我的推广订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetMyOrderList(int userId, int page, int limit = 10, int typeId = 0);

        /// <summary>
        ///     获取店铺信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetStore(int userId);


        /// <summary>
        ///     获取当前用户返佣设置
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetGradeAndCommission(int userId);


        //检查是否可以成为分销商
        Task CheckCondition(Dictionary<string, DictionaryKeyValues> allConfigs, CoreCmsDistribution info,
            int userId = 0);


        /// <summary>
        ///     检查当前用户是否可以升级(暂存，有问题)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> CheckUpdate(int userId);

        /// <summary>
        ///     获取代理商排行
        /// </summary>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        Task<IPageList<DistributionRankingDTO>> QueryRankingPageAsync(int pageIndex = 1, int pageSize = 20);
    }
}