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
        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 快递编码
        /// </summary>
        public string logiCode { get; set; }
        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string logiNo { get; set; }
        /// <summary>
        /// 直播物流编码 
        /// </summary>
        public string deliveryCompanyId { get; set; }

        public Dictionary<int, int> items { get; set; }
        /// <summary>
        /// 门店编码
        /// </summary>
        public int storeId { get; set; } = 0;
        /// <summary>
        /// 收货姓名
        /// </summary>
        public string shipName { get; set; }
        /// <summary>
        /// 收货手机
        /// </summary>
        public string shipMobile { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string shipAddress { get; set; }
        /// <summary>
        /// 收货区域编码
        /// </summary>
        public int shipAreaId { get; set; } = 0;
        /// <summary>
        /// 备注
        /// </summary>
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