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
    ///     促销条件表 服务工厂接口
    /// </summary>
    public interface ICoreCmsPromotionConditionServices : IBaseServices<CoreCmsPromotionCondition>
    {
        /// <summary>
        ///     检查是否满足条件
        /// </summary>
        /// <param name="conditionInfo"></param>
        /// <param name="cart"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        Task<bool> check(CoreCmsPromotionCondition conditionInfo, CartDto cart, CoreCmsPromotion promotionInfo);

        /// <summary>
        ///     在促销结果中，如果是商品促销结果，调用此方法，判断商品是否符合需求
        /// </summary>
        /// <param name="promotionId"></param>
        /// <param name="goodsId"></param>
        /// <param name="nums"></param>
        /// <returns></returns>
        Task<int> goods_check(int promotionId, int goodsId, int nums = 1);


        /// <summary>
        ///     因为计算过促销条件后啊，前面有些是满足条件的，所以，他们的type是2，后面有不满足条件的时候呢，要把前面满足条件的回滚成不满足条件的
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        CartDto PromotionFalse(CartDto cart, CoreCmsPromotion promotionInfo);

        /// <summary>
        ///     订单满XX金额时满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="cart"></param>
        /// <returns></returns>
        int condition_ORDER_FULL(JObject parameters, CartDto cart);


        /// <summary>
        ///     所有商品满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="goodsId">商品序列</param>
        /// <param name="nums">数量</param>
        /// <returns></returns>
        int condition_GOODS_ALL(JObject parameters, int goodsId, int nums);


        /// <summary>
        ///     指定某些商品满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="goodsId">商品序列</param>
        /// <param name="nums">数量</param>
        /// <returns></returns>
        int condition_GoodsIdS(JObject parameters, int goodsId, int nums);

        /// <summary>
        ///     指定商品分类满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="goodsId">商品序列</param>
        /// <param name="nums">数量</param>
        /// <returns></returns>
        Task<int> condition_GOODS_CATS(JObject parameters, int goodsId, int nums);

        /// <summary>
        ///     指定商品品牌满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="goodsId">商品序列</param>
        /// <param name="nums">数量</param>
        /// <returns></returns>
        Task<int> condition_GOODS_BRANDS(JObject parameters, int goodsId, int nums);

        /// <summary>
        ///     指定用户等级满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="userId">用户序列</param>
        /// <returns></returns>
        Task<int> condition_USER_GRADE(JObject parameters, int userId);
    }
}