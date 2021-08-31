/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

namespace CoreCms.Net.Model.ViewModels.DTO
{
    /// <summary>
    /// </summary>
    public class reshipGoods
    {
        /// <summary>
        ///     售后商品数量，包含申请中和审核通过的
        /// </summary>
        public int reshipNums { get; set; } = 0;

        /// <summary>
        ///     已发货的商品进行退货的数量
        /// </summary>
        public int reshipedNums { get; set; } = 0;
    }
}