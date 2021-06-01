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
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     促销活动记录表 服务工厂接口
    /// </summary>
    public interface ICoreCmsPromotionRecordServices : IBaseServices<CoreCmsPromotionRecord>
    {
        /// <summary>
        ///     生成订单的时候，增加信息
        /// </summary>
        /// <param name="order">订单数据</param>
        /// <param name="items">货品列表</param>
        /// <param name="groupId">秒杀团购序列</param>
        /// <param name="orderType">购物车类型</param>
        /// <returns></returns>
        Task<WebApiCallBack> OrderAdd(CoreCmsOrder order, List<CoreCmsOrderItem> items, int groupId, int orderType);
    }
}