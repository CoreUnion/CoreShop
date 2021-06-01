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
    ///     根据where查询条件和order排序获取列表
    /// </summary>
    public class FMPageByWhereOrder
    {
        /// <summary>
        ///     当前页码
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        ///     每页数据量
        /// </summary>
        public int limit { get; set; } = 10;

        /// <summary>
        ///     排序
        /// </summary>
        public string order { get; set; }

        /// <summary>
        ///     判断条件
        /// </summary>
        public string where { get; set; }
    }


    /// <summary>
    ///     根据int类型id加where查询条件和order排序获取列表(一般用于直接id分页)
    /// </summary>
    public class FMPageByIntId
    {
        public object otherData { get; set; }


        public int id { get; set; }


        /// <summary>
        ///     当前页码
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        ///     每页数据量
        /// </summary>
        public int limit { get; set; } = 10;

        /// <summary>
        ///     排序
        /// </summary>
        public string order { get; set; }

        /// <summary>
        ///     判断条件
        /// </summary>
        public string where { get; set; }
    }

    /// <summary>
    ///     根据String类型id加where查询条件和order排序获取列表(一般用于直接id分页)
    /// </summary>
    public class FMPageByStringId
    {
        public string id { get; set; }


        /// <summary>
        ///     当前页码
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        ///     每页数据量
        /// </summary>
        public int limit { get; set; } = 10;

        /// <summary>
        ///     排序
        /// </summary>
        public string order { get; set; }

        /// <summary>
        ///     判断条件
        /// </summary>
        public string where { get; set; }
    }
}