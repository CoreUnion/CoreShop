/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-03-18 3:18:36
 *        Description: 暂无
 ***********************************************************************/

namespace CoreCms.Net.Web.WebApi.Models
{
    /// <summary>
    ///     错误返回示例
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        ///     回调序列
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        ///     显示回调序列
        /// </summary>

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}