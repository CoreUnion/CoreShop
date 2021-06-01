/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     订单明细表 工厂接口
    /// </summary>
    public interface ICoreCmsOrderItemRepository : IBaseRepository<CoreCmsOrderItem>
    {
        /// <summary>
        ///     算订单的商品退了多少个(未发货的退货数量，已发货的退货不算)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        int GetaftersalesNums(string orderId, string sn);
    }
}