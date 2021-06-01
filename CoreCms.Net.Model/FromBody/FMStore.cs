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
    public class FMStoreClerkCURDPost
    {
        public int id { get; set; } = 0;

        public int storeId { get; set; }
        public string phone { get; set; }
    }


    /// <summary>
    ///     根据int类型id加where查询条件和order排序获取列表(一般用于直接id分页)
    /// </summary>
    public class FMGetStoreQueryPageByCoordinate
    {
        public decimal longitude { get; set; } = 0;

        public decimal latitude { get; set; } = 0;


        public string key { get; set; }


        /// <summary>
        ///     当前页码
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        ///     每页数据量
        /// </summary>
        public int limit { get; set; } = 10;
    }
}