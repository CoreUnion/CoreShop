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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.Model.ViewModels.DTO
{
    /// <summary>
    ///     购物车返回列表实体
    /// </summary>
    public class CartDto
    {
        public int userId { get; set; } = 0;

        /// <summary>
        ///     商品总金额
        /// </summary>
        public decimal goodsAmount { get; set; }

        /// <summary>
        ///     总金额
        /// </summary>
        public decimal amount { get; set; }

        /// <summary>
        ///     订单促销金额
        ///     单纯的订单促销的金额
        /// </summary>
        public decimal orderPromotionMoney { get; set; } = 0;

        /// <summary>
        ///     商品促销金额
        ///     所有的商品促销的总计
        /// </summary>
        public decimal goodsPromotionMoney { get; set; } = 0;

        /// <summary>
        ///     优惠券优惠金额
        /// </summary>
        public decimal couponPromotionMoney { get; set; } = 0;

        /// <summary>
        ///     促销列表
        /// </summary>
        public Dictionary<int, WxNameTypeDto> promotionList { get; set; } = new();

        /// <summary>
        ///     运费
        /// </summary>
        public decimal costFreight { get; set; } = 0;

        /// <summary>
        ///     商品总重
        /// </summary>
        public decimal weight { get; set; } = 0;

        /// <summary>
        ///     优惠券
        /// </summary>
        public List<string> coupon { get; set; } = new();

        /// <summary>
        ///     购物车类型
        /// </summary>
        public int type { get; set; } = 1;


        /// <summary>
        ///     积分
        /// </summary>
        public int point { get; set; } = 0;

        /// <summary>
        ///     积分可以抵扣多少金额
        /// </summary>
        public int pointExchangeMoney { get; set; } = 0;

        public List<CartProducts> list { get; set; } = new();


        /// <summary>
        ///     消息回调
        /// </summary>
        public WebApiCallBack error { get; set; } = new();
    }

    public class CartProducts
    {
        public int id { get; set; } = 0;
        public int userId { get; set; } = 0;
        public int productId { get; set; } = 0;
        public int nums { get; set; } = 1;
        public bool isCollection { get; set; } = false;
        public bool isSelect { get; set; } = false;
        public int type { get; set; } = 1;
        public decimal weight { get; set; } = 0;

        public CoreCmsProducts products { get; set; } = new();

        public CoreCmsGoods good { get; set; }
    }

    /// <summary>
    ///     设置购物车商品数量
    /// </summary>
    public class FMSetCartNum
    {
        public int id { get; set; } = 0;
        public int nums { get; set; } = 1;
    }
}