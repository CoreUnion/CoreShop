/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/14 16:27:23
 *        Description: 暂无
 ***********************************************************************/


using Newtonsoft.Json.Linq;

namespace CoreCms.Net.WeChat.Service.Models
{
    /// <summary>
    ///     处理器-微信模板消息【小程序，公众号都走这里】
    /// </summary>
    public class SendWxTemplateMessage
    {
        /// <summary>
        ///     用户序列
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        ///     类型
        /// </summary>
        public string code { get; set; }

        /// <summary>
        ///     传递数据
        /// </summary>
        public JObject parameters { get; set; }
    }
}