/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/28 20:42:38
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     微信授权交互 工厂接口
    /// </summary>
    public interface IWeChatAccessTokenRepository : IBaseRepository<WeChatAccessToken>
    {
    }
}