/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.DTO;
using Newtonsoft.Json.Linq;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     促销结果表 服务工厂接口
    /// </summary>
    public interface ICoreCmsPromotionResultServices : IBaseServices<CoreCmsPromotionResult>
    {
        /// <summary>
        ///     去计算结果
        /// </summary>
        /// <param name="resultInfo"></param>
        /// <param name="cart"></param>
        /// <param name="promotionInfo"></param>
        Task<bool> toResult(CoreCmsPromotionResult resultInfo, CartDto cart, CoreCmsPromotion promotionInfo);


        /// <summary>
        ///     订单减固定金额
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cart"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        bool result_ORDER_REDUCE(JObject parameters, CartDto cart, CoreCmsPromotion promotionInfo);

        /// <summary>
        ///     订单打X折
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cart"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        bool result_ORDER_DISCOUNT(JObject parameters, CartDto cart, CoreCmsPromotion promotionInfo);


        /// <summary>
        ///     指定商品减固定金额
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cartProducts"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        decimal result_GOODS_REDUCE(JObject parameters, CartProducts cartProducts, CoreCmsPromotion promotionInfo);

        /// <summary>
        ///     指定商品打X折
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cartProducts"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        decimal result_GOODS_DISCOUNT(JObject parameters, CartProducts cartProducts, CoreCmsPromotion promotionInfo);


        /// <summary>
        ///     商品一口价
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cartProducts"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        decimal result_GOODS_ONE_PRICE(JObject parameters, CartProducts cartProducts, CoreCmsPromotion promotionInfo);
    }
}