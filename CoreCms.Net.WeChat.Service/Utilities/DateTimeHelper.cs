/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/7/29 11:06:40
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCms.Net.WeChat.Service.Utilities
{
    /// <summary>微信日期处理帮助类</summary>
    public class DateTimeHelper
    {
        /// <summary>Unix起始时间</summary>
        public static readonly DateTimeOffset BaseTime = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        /// <summary>转换微信DateTime时间到C#时间</summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromXml(long dateTimeFromXml) => DateTimeHelper.GetDateTimeOffsetFromXml(dateTimeFromXml).LocalDateTime;

        /// <summary>转换微信DateTime时间到C#时间</summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromXml(string dateTimeFromXml) => DateTimeHelper.GetDateTimeFromXml(long.Parse(dateTimeFromXml));

        /// <summary>转换微信DateTimeOffset时间到C#时间</summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTimeOffset GetDateTimeOffsetFromXml(long dateTimeFromXml) => DateTimeHelper.BaseTime.AddSeconds((double)dateTimeFromXml).ToLocalTime();

        /// <summary>转换微信DateTimeOffset时间到C#时间</summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTimeOffset GetDateTimeOffsetFromXml(string dateTimeFromXml) => (DateTimeOffset)DateTimeHelper.GetDateTimeFromXml(long.Parse(dateTimeFromXml));

        /// <summary>获取微信DateTime（UNIX时间戳）</summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        [Obsolete("请使用 GetUnixDateTime(dateTime) 方法")]
        public static long GetWeixinDateTime(DateTime dateTime) => DateTimeHelper.GetUnixDateTime(dateTime);

        /// <summary>获取Unix时间戳</summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetUnixDateTime(DateTimeOffset dateTime) => (long)(dateTime - DateTimeHelper.BaseTime).TotalSeconds;

        /// <summary>获取Unix时间戳</summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetUnixDateTime(DateTime dateTime) => (long)((DateTimeOffset)dateTime.ToUniversalTime() - DateTimeHelper.BaseTime).TotalSeconds;
    }
}
