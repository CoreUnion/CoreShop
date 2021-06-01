/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     订单明细表
    /// </summary>
    public partial class CoreCmsOrderItem
    {
        /// <summary>
        ///     退货商品数量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int reshipNums { get; set; } = 0;

        /// <summary>
        ///     已发货的退货商品
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int reshipedNums { get; set; } = 0;


        /// <summary>
        ///     当前退货数量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int atPresentReshipNums { get; set; } = 0;

        [SugarColumn(IsIgnore = true)] public object promotionObj { get; set; }
    }
}