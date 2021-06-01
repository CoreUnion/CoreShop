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
    //APi=========================================================================
    /// <summary>
    ///     获取团购列表请求参数
    /// </summary>
    public class FMGroupGetListPost
    {
        /// <summary>
        ///     类型
        /// </summary>
        public int type { get; set; } = 0;

        /// <summary>
        ///     页码
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        ///     分页数量
        /// </summary>
        public int limit { get; set; } = 10;

        /// <summary>
        ///     活动状态
        /// </summary>
        public int status { get; set; } = 0;
    }


    public class FMGetGoodsDetial
    {
        public int id { get; set; }
        public int groupId { get; set; }
    }
}