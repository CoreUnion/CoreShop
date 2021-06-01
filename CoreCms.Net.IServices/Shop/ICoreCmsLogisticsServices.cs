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
    ///     物流公司表 服务工厂接口
    /// </summary>
    public interface ICoreCmsLogisticsServices : IBaseServices<CoreCmsLogistics>
    {
        /// <summary>
        ///     根据物流编码取物流名称等信息
        /// </summary>
        /// <param name="logiCode">物流编码</param>
        /// <returns></returns>
        Task<WebApiCallBack> GetLogiInfo(string logiCode);


        /// <summary>
        ///     通过接口
        /// </summary>
        Task<AdminUiCallBack> DoUpdateCompany();


        /// <summary>
        ///     通过接口获取快递信息
        /// </summary>
        /// <param name="com">来源</param>
        /// <param name="number">编号</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        Task<WebApiCallBack> ExpressPoll(string com, string number, string phone);
    }
}