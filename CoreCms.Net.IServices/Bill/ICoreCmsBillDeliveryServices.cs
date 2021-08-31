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
    ///     发货单表 服务工厂接口
    /// </summary>
    public interface ICoreCmsBillDeliveryServices : IBaseServices<CoreCmsBillDelivery>
    {

        /// <summary>
        ///     批量发货，可以支持多个订单合并发货，单个订单拆分发货等。
        /// </summary>
        /// <param name="orderId">英文逗号分隔的订单号</param>
        /// <param name="logiCode">物流公司编码</param>
        /// <param name="logiNo">物流单号</param>
        /// <param name="items">发货明细</param>
        /// <param name="storeId">店铺收货地址</param>
        /// <param name="shipName">收货人姓名</param>
        /// <param name="shipMobile">收货人电话</param>
        /// <param name="shipAreaId">省市区id</param>
        /// <param name="shipAddress">收货地址</param>
        /// <param name="memo">发货描述</param>
        /// <returns></returns>
        Task<WebApiCallBack> BatchShip(string[] orderId, string logiCode, string logiNo, Dictionary<int, int> items,
            int storeId = 0, string shipName = "", string shipMobile = "", int shipAreaId = 0, string shipAddress = "",
            string memo = "");


        /// <summary>
        ///     发货，单个订单发货
        /// </summary>
        /// <param name="orderId">英文逗号分隔的订单号</param>
        /// <param name="logiCode">物流公司编码</param>
        /// <param name="logiNo">物流单号</param>
        /// <param name="items">发货明细</param>
        /// <param name="storeId">店铺收货地址</param>
        /// <param name="shipName">收货人姓名</param>
        /// <param name="shipMobile">收货人电话</param>
        /// <param name="shipAreaId">省市区id</param>
        /// <param name="shipAddress">收货地址</param>
        /// <param name="memo">发货描述</param>
        /// <returns></returns>
        Task<WebApiCallBack> Ship(string orderId, string logiCode, string logiNo, Dictionary<int, int> items,
            int storeId = 0, string shipName = "", string shipMobile = "", int shipAreaId = 0, string shipAddress = "",
            string memo = "");




        /// <summary>
        ///     物流信息查询根据快递编码和单号查询(快递100)未使用
        /// </summary>
        /// <param name="code">查询的快递公司的编码， 一律用小写字母（如：yuantong）</param>
        /// <param name="no">查询的快递单号， 单号的最大长度是32个字符</param>
        /// <returns></returns>
        Task<WebApiCallBack> GetLogistic(string code, string no);


        /// <summary>
        ///     发货单统计7天统计
        /// </summary>
        /// <returns></returns>
        Task<List<StatisticsOut>> Statistics();
    }
}