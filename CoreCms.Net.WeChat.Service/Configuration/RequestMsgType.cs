/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/30 14:19:49
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCms.Net.WeChat.Service.Configuration
{
    /// <summary>
    ///     常用常量配置
    /// </summary>
    public static class RequestMsgType
    {
        // 各种消息类型,除了扫带二维码事件
        /// <summary>
        /// 文本消息
        /// </summary>
        public const string Text = "text";

        /// <summary>
        /// 图片消息
        /// </summary>
        public const string Image = "image";

        /// <summary>
        /// 语音消息
        /// </summary>
        public const string Voice = "voice";

        /// <summary>
        /// 视频消息
        /// </summary>
        public const string Video = "video";

        /// <summary>
        /// 小视频消息
        /// </summary>
        public const string ShortVideo = "shortvideo";

        /// <summary>
        /// 地理位置消息
        /// </summary>
        public const string Location = "location";

        /// <summary>
        /// 链接消息
        /// </summary>
        public const string Link = "link";

        /// <summary>
        /// 事件推送消息
        /// </summary>
        public const string MessageEvent = "event";



    }
}