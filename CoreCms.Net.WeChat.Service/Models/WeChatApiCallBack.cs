/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

namespace CoreCms.Net.WeChat.Service.Models
{
    /// <summary>
    ///     微信接口回调Json实体
    /// </summary>
    public class WeChatApiCallBack
    {
        /// <summary>
        ///     提交数据
        /// </summary>
        public object OtherData { get; set; } = null;

        /// <summary>
        ///     状态码
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        ///     信息说明。
        /// </summary>
        public string Msg { get; set; } = "响应成功";

        /// <summary>
        ///     返回数据
        /// </summary>
        public string Data { get; set; } = "success";
    }
}