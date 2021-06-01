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
    ///     分享通用接口
    /// </summary>
    public interface ICoreCmsShareServices : IBaseServices<CoreCmsSetting>
    {
        /// <summary>
        ///     二维码分享
        /// </summary>
        /// <param name="client"></param>
        /// <param name="page"></param>
        /// <param name="userShareCode"></param>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<WebApiCallBack> QrShare(int client, int page, int userShareCode, string url, JObject parameter);


        /// <summary>
        ///     页面分享
        /// </summary>
        /// <param name="client"></param>
        /// <param name="page"></param>
        /// <param name="userShareCode"></param>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        WebApiCallBack UrlShare(int client, int page, int userShareCode, string url, JObject parameter);


        /// <summary>
        ///     海报分享
        /// </summary>
        /// <param name="client"></param>
        /// <param name="page"></param>
        /// <param name="userShareCode"></param>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<WebApiCallBack> PosterShare(int client, int page, int userShareCode, string url, JObject parameter);


        /// <summary>
        ///     小程序二维码，和业务没关系
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetQrCode(string scene, string page = "pages/share/jump");


        /// <summary>
        ///     url参数解密
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        WebApiCallBack de_url(string code);
    }
}