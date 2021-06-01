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
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     单页 服务工厂接口
    /// </summary>
    public interface ICoreCmsPagesServices : IBaseServices<CoreCmsPages>
    {
        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateAsync(FmPagesUpdate entity);


        /// <summary>
        ///     获取首页数据
        /// </summary>
        /// <param name="code">查询编码</param>
        /// <returns></returns>
        Task<WebApiCallBack> GetPageConfig(string code);
    }
}