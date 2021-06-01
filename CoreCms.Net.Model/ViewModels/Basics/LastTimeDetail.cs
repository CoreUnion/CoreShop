/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

namespace CoreCms.Net.Model.ViewModels.Basics
{
    /// <summary>
    ///     剩余时间
    /// </summary>
    public class LastTimeDetail
    {
        /// <summary>
        ///     日
        /// </summary>
        public int day { get; set; } = 0;

        /// <summary>
        ///     时
        /// </summary>
        public int hour { get; set; } = 0;

        /// <summary>
        ///     分
        /// </summary>
        public int minute { get; set; } = 0;

        /// <summary>
        ///     秒
        /// </summary>
        public int second { get; set; } = 0;
    }
}