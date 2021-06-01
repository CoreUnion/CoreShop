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
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     消息发送表 服务工厂接口
    /// </summary>
    public interface ICoreCmsMessageServices : IBaseServices<CoreCmsMessage>
    {
        /// <summary>
        ///     站内消息
        /// </summary>
        /// <param name="userId">接受者id</param>
        /// <param name="code">模板编码</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        Task<WebApiCallBack> Send(int userId, string code, JObject parameters);

        /// <summary>
        ///     消息查看，更新已读状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebApiCallBack> info(int userId, int id);


        /// <summary>
        ///     判断是否有新消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> HasNew(int userId);
    }
}