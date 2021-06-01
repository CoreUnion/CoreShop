/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    ///     后台查询报表综合提交参数
    /// </summary>
    public class FMReports
    {
        /// <summary>
        ///     时间返回
        /// </summary>
        public string date { get; set; }

        /// <summary>
        ///     查询时间类型
        /// </summary>
        public int section { get; set; } = 0;

        //页码
        public int page { get; set; } = 1;

        /// <summary>
        ///     分页数量
        /// </summary>
        public int limit { get; set; } = 5000;

        /// <summary>
        ///     排名条件
        /// </summary>
        public string filter { get; set; } = "";

        /// <summary>
        ///     升降序 desc/asc
        /// </summary>
        public string thesort { get; set; } = "";
    }
}