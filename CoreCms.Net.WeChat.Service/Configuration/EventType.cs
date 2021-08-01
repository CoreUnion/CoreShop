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
    ///     常见消息类型
    /// </summary>
    public static class EventType
    {

        #region 公众号类型

        /// <summary>
        /// 关注
        /// </summary>
        public const string Subscribe = "subscribe";
        /// <summary>
        /// 取消订阅
        /// </summary>
        public const string Unsubscribe = "unsubscribe";
        /// <summary>
        /// 上报地理位置事件
        /// 用户同意上报地理位置后，每次进入公众号会话时，都会在进入时上报地理位置，或在进入会话后每5秒上报一次地理位置，公众号可以在公众平台网站中修改以上设置。上报地理位置时，微信会将上报地理位置事件推送到开发者填写的URL。
        /// </summary>
        public const string Localtion = "LOCATION";

        /// <summary>
        /// 自定义菜单事件-用户点击自定义菜单后，微信会把点击事件推送给开发者，请注意，点击菜单弹出子菜单，不会产生上报。
        /// </summary>
        public const string Click = "CLICK";


        #endregion


        #region 小程序



        #endregion

        #region 自定义交易组件

        

        #endregion

        /// <summary>
        /// 图片消息
        /// </summary>
        public const string Image = "image";




    }
}