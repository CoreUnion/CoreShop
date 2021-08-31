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
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     促销表 服务工厂接口
    /// </summary>
    public interface ICoreCmsPromotionServices : IBaseServices<CoreCmsPromotion>
    {
        /// <summary>
        ///     购物车的数据传过来，然后去算促销
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<CartDto> ToPromotion(CartDto cart, int type = (int) GlobalEnumVars.PromotionType.Promotion);

        /// <summary>
        ///     购物车的数据传过来，然后去算优惠券
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="promotions"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ToCoupon(CartDto cart, List<CoreCmsPromotion> promotions);

        /// <summary>
        ///     根据促销信息，去计算购物车的促销情况
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="cartModel"></param>
        /// <returns></returns>
        Task<bool> SetPromotion(CoreCmsPromotion promotion, CartDto cartModel);

        /// <summary>
        ///     获取团购列表数据
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetGroupList(int type, int userId, int status, int pageIndex, int pageSize);

        /// <summary>
        ///     获取团购/秒杀商品详情
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetGroupDetail(int goodId = 0, int userId = 0, string type = "group", int groupId = 0);

        /// <summary>
        ///     获取可领取的优惠券
        /// </summary>
        /// <param name="limit">数量</param>
        /// <returns></returns>
        Task<List<CoreCmsPromotion>> ReceiveCouponList(int limit = 3);


        /// <summary>
        ///     获取可领取的优惠券（分页）
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">数量</param>
        /// <returns></returns>
        Task<IPageList<CoreCmsPromotion>> GetReceiveCouponList(int page = 1, int limit = 10);

        /// <summary>
        ///     获取指定id 的优惠券是否可以领取
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ReceiveCoupon(int promotionId);
    }
}