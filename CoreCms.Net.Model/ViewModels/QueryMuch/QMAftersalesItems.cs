/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/


namespace CoreCms.Net.Model.ViewModels.QueryMuch
{
    /// <summary>
    ///     根据订单号查询已经售后的内容.算退货商品明细
    /// </summary>
    public class QMAftersalesItems
    {
        public int orderItemsId { get; set; }
        public int nums { get; set; }
        public int status { get; set; }
        public int type { get; set; }
    }
}