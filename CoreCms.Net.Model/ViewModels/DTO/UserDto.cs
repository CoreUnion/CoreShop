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
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    /// <summary>
    ///     获取用户积分返回
    /// </summary>
    public class GetUserPointResult
    {
        public int availablePoint { get; set; } = 0;
        public int pointExchangeMoney { get; set; } = 0;

        public int @switch { get; set; } = 1;

        public int point { get; set; } = 0;
    }

    /// <summary>
    ///     获取用户积分提交
    /// </summary>
    public class GetUserPointPost
    {
        /// <summary>
        ///     订单金额
        /// </summary>
        public decimal orderMoney { get; set; }
    }

    /// <summary>
    ///     保存用户地址提交数据
    /// </summary>
    public class SaveUserShipPost
    {
        public int id { get; set; } = 0;
        public string address { get; set; }
        public int areaId { get; set; }
        public int isDefault { get; set; } = 2;
        public string mobile { get; set; }
        public string name { get; set; }
    }

    /// <summary>
    ///     用户获取区域id提交实体
    /// </summary>
    public class GetAreaIdPost
    {
        /// <summary>
        ///     县
        /// </summary>
        public string cityName { get; set; }

        /// <summary>
        ///     市/区
        /// </summary>
        public string countyName { get; set; }

        /// <summary>
        ///     省
        /// </summary>
        public string provinceName { get; set; }

        /// <summary>
        ///     邮编
        /// </summary>
        public string postalCode { get; set; }
    }

    /// <summary>
    ///     支付提交实体
    /// </summary>
    public class PayPost
    {
        /// <summary>
        ///     订单号
        /// </summary>
        public string ids { get; set; }

        /// <summary>
        ///     支付方式
        /// </summary>
        public string payment_code { get; set; }

        /// <summary>
        ///     订单类型 1商品订单 2充值订单 5服务订单
        /// </summary>
        public int payment_type { get; set; } = 0;

        /// <summary>
        ///     附加参数
        /// </summary>
        public JObject @params { get; set; }
    }

    /// <summary>
    ///     订单评价提交数据
    /// </summary>
    public class OrderEvaluatePost
    {
        /// <summary>
        ///     订单编号
        /// </summary>
        public string orderId { get; set; }

        /// <summary>
        ///     评价子项
        /// </summary>
        public List<OrderEvaluatePostItems> items { get; set; }
    }

    public class OrderEvaluatePostItems
    {
        /// <summary>
        ///     图集
        /// </summary>
        public string[] images { get; set; }

        /// <summary>
        ///     关联订单明细编号
        /// </summary>
        public int orderItemId { get; set; }

        /// <summary>
        ///     分值(0-5数字)
        /// </summary>
        public int score { get; set; }

        /// <summary>
        ///     评价文字
        /// </summary>
        public string textarea { get; set; }
    }
}