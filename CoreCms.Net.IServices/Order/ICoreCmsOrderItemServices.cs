/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     订单明细表 服务工厂接口
    /// </summary>
    public interface ICoreCmsOrderItemServices : IBaseServices<CoreCmsOrderItem>
    {
        /// <summary>
        ///     发货数量
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="item">发货明细</param>
        /// <returns></returns>
        Task<bool> ship(string orderId, Dictionary<int, int> item);
    }
}