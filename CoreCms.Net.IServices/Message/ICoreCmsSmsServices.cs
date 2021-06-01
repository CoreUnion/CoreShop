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
using CoreCms.Net.Model.ViewModels.Sms;
using CoreCms.Net.Model.ViewModels.UI;
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     短信发送日志 服务工厂接口
    /// </summary>
    public interface ICoreCmsSmsServices : IBaseServices<CoreCmsSms>
    {
        /// <summary>
        ///     发送短信（验证码）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<WebApiCallBack> DoSendSms(string type, string mobile);

        /// <summary>
        ///     发送短信统一方法
        /// </summary>
        /// <param name="mobile">接受者手机号码</param>
        /// <param name="code">模板编码</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        Task<WebApiCallBack> Send(string mobile, string code, JObject parameters);

        /// <summary>
        ///     接口通道发送短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="contentBody"></param>
        /// <param name="smsOptions"></param>
        string SendSms(string mobile, string contentBody, SMSOptions smsOptions);

        /// <summary>
        ///     校验短信验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="verCode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<bool> Check(string phone, string verCode, string code);
    }
}