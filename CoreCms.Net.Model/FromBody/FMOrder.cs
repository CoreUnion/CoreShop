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

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    ///     后端编辑订单提交参数
    /// </summary>
    public class AdminEditOrderPost
    {
        public string orderId { get; set; }
        public int editType { get; set; } = 1;
        public int storeId { get; set; } = 0;
        public int shipAreaId { get; set; } = 0;
        public string shipName { get; set; }
        public string shipMobile { get; set; }
        public string shipAddress { get; set; }
        public decimal orderAmount { get; set; } = 0;
    }

    /// <summary>
    ///     后端订单发货提交参数
    /// </summary>
    public class AdminOrderShipPost
    {
        public string orderId { get; set; }
        public string logiCode { get; set; }
        public string logiNo { get; set; }
        public Dictionary<int, int> items { get; set; }
        public int storeId { get; set; } = 0;
        public string shipName { get; set; }
        public string shipMobile { get; set; }
        public string shipAddress { get; set; }
        public int shipAreaId { get; set; } = 0;
        public string memo { get; set; }
    }


    /// <summary>
    ///     后端订单手动支付提交参数
    /// </summary>
    public class AdminOrderDoPayPost
    {
        /// <summary>
        ///     订单编号
        /// </summary>
        public string orderId { get; set; }

        /// <summary>
        ///     支付类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        ///     支付类型编码
        /// </summary>
        public string paymentCode { get; set; }
    }

    /// <summary>
    ///     前台物流查询接口提交参数
    /// </summary>
    public class FMApiLogisticsByApiPost
    {
        /// <summary>
        ///     快递公司编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        ///     物流单号
        /// </summary>
        public string no { get; set; }


        /// <summary>
        ///     手机号码
        /// </summary>
        public string mobile { get; set; }
    }
}