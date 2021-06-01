/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

namespace CoreCms.Net.Model.FromBody
{
    /// <summary>
    ///     优惠券相关提交验证实体
    /// </summary>
    public class FMCouponForUserCouponPost
    {
        public int page { get; set; } = 1;
        public int limit { get; set; } = 100;
        public string display { get; set; } = "all";


        /// <summary>
        ///     购物车数据
        /// </summary>
        public string ids { get; set; } = null;
    }


    /// <summary>
    ///     优惠券相关提交验证实体
    /// </summary>
    public class FMCouponForGetCouponKeyPost
    {
        public string key { get; set; }
    }


    /// <summary>
    ///     优惠券相关提交验证实体
    /// </summary>
    public class FMCouponForCouponListPost
    {
        public int limit { get; set; } = 3;
    }
}