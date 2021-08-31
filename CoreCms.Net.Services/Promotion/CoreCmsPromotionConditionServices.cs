/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 促销条件表 接口实现
    /// </summary>
    public class CoreCmsPromotionConditionServices : BaseServices<CoreCmsPromotionCondition>, ICoreCmsPromotionConditionServices
    {
        private readonly ICoreCmsPromotionConditionRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoreCmsGoodsCategoryServices _goodsCategoryServices;

        private readonly IServiceProvider _serviceProvider;


        public CoreCmsPromotionConditionServices(IUnitOfWork unitOfWork
            , ICoreCmsPromotionConditionRepository dal
            , ICoreCmsGoodsCategoryServices goodsCategoryServices
            , IServiceProvider serviceProvider
            )
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _goodsCategoryServices = goodsCategoryServices;
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// 检查是否满足条件
        /// </summary>
        /// <param name="conditionInfo"></param>
        /// <param name="cart"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        public async Task<bool> check(CoreCmsPromotionCondition conditionInfo, CartDto cart,
            CoreCmsPromotion promotionInfo)
        {
            if (string.IsNullOrEmpty(conditionInfo.parameters)) return false;

            var getPromotionConditionType = SystemSettingDictionary.GetPromotionConditionType();
            var codeModel = getPromotionConditionType.Find(p => p.sKey == conditionInfo.code);
            if (codeModel != null)
            {
                //如果是订单促销就直接去判断促销条件，如果是商品促销，就循环订单明细
                JObject parameters = (JObject)JsonConvert.DeserializeObject(conditionInfo.parameters);

                if (codeModel.sValue == "goods")
                {
                    var key = false;
                    foreach (var item in cart.list)
                    {
                        var type = 0;
                        //判断是哪个规则，并且确认是否符合
                        switch (conditionInfo.code)
                        {
                            case "GOODS_ALL":
                                type = condition_GOODS_ALL(parameters,
                                    (int)item.products.goodsId, item.nums);
                                break;
                            case "GOODS_IDS":
                                type = condition_GoodsIdS(parameters,
                                    (int)item.products.goodsId, item.nums);
                                break;
                            case "GOODS_CATS":
                                type = await condition_GOODS_CATS(parameters, (int)item.products.goodsId, item.nums);
                                break;
                            case "GOODS_BRANDS":
                                type = await condition_GOODS_BRANDS(parameters,
                                    (int)item.products.goodsId, item.nums);
                                break;
                            default:
                                type = 0;
                                break;
                        }
                        if (type > 0)
                        {
                            if (item.products.promotionList.ContainsKey(promotionInfo.id))
                            {
                                item.products.promotionList[promotionInfo.id].name = promotionInfo.name;
                                item.products.promotionList[promotionInfo.id].type = type;
                            }
                            else
                            {
                                item.products.promotionList.Add(promotionInfo.id, new WxNameTypeDto()
                                {
                                    name = promotionInfo.name,
                                    type = type
                                });
                            }

                        }
                        //只有选中的商品才算促销
                        if (item.isSelect)
                        {
                            if (!key)
                            {
                                if (type == 2)
                                {
                                    key = true;//针对某一条商品促销条件，循环购物车的所有商品，只要有一条满足要求就，算，就返回true
                                }
                            }
                        }
                    }
                    return key;
                }
                else if (codeModel.sValue == "order")
                {
                    var type = condition_ORDER_FULL(parameters, cart);
                    if (type > 0)
                    {
                        if (cart.promotionList.ContainsKey(promotionInfo.id))
                        {
                            cart.promotionList[promotionInfo.id].name = promotionInfo.name;
                            cart.promotionList[promotionInfo.id].type = type;
                        }
                        //else
                        //{
                        //    cart.promotionList.Add(type, new WxNameTypeDto()
                        //    {
                        //        name = promotionInfo.name,
                        //        type = type
                        //    });
                        //}
                    }
                    if (type == 2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (codeModel.sValue == "user")
                {
                    var type = await condition_USER_GRADE(parameters, cart.userId);
                    if (type == 2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            return false;
        }


        /// <summary>
        /// 在促销结果中，如果是商品促销结果，调用此方法，判断商品是否符合需求
        /// </summary>
        /// <param name="promotionId"></param>
        /// <param name="goodsId"></param>
        /// <param name="nums"></param>
        /// <returns></returns>
        public async Task<int> goods_check(int promotionId, int goodsId, int nums = 1)
        {
            var conditionInfos = await _dal.QueryListByClauseAsync(p => p.promotionId == promotionId);
            var getPromotionConditionType = SystemSettingDictionary.GetPromotionConditionType();

            foreach (var item in conditionInfos)
            {
                var codeModel = getPromotionConditionType.Find(p => p.sKey == item.code);
                if (codeModel != null && codeModel.sValue == "goods")
                {
                    JObject parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                    var type = 0;
                    //判断是哪个规则，并且确认是否符合
                    switch (item.code)
                    {
                        case "GOODS_ALL":
                            type = condition_GOODS_ALL(parameters, goodsId, nums);
                            break;
                        case "GOODS_IDS":
                            type = condition_GoodsIdS(parameters, goodsId, nums);
                            break;
                        case "GOODS_CATS":
                            type = await condition_GOODS_CATS(parameters, goodsId, nums);
                            break;
                        case "GOODS_BRANDS":
                            type = await condition_GOODS_BRANDS(parameters, goodsId, nums);
                            break;
                        default:
                            type = 0;
                            break;
                    }
                    if (type != 2)
                    {
                        return type;
                    }
                }
            }
            return 2;
        }


        /// <summary>
        /// 因为计算过促销条件后啊，前面有些是满足条件的，所以，他们的type是2，后面有不满足条件的时候呢，要把前面满足条件的回滚成不满足条件的
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        public CartDto PromotionFalse(CartDto cart, CoreCmsPromotion promotionInfo)
        {
            switch (promotionInfo.type)
            {
                case (int)GlobalEnumVars.PromotionType.Promotion:
                    //订单促销回滚
                    if (cart.promotionList.ContainsKey(promotionInfo.id))
                    {
                        cart.promotionList[promotionInfo.id].name = promotionInfo.name;
                        cart.promotionList[promotionInfo.id].type = 1;
                    }
                    //商品回滚
                    foreach (var item in cart.list.Where(item => item.products.promotionList.ContainsKey(promotionInfo.id)))
                    {
                        item.products.promotionList[promotionInfo.id].name = promotionInfo.name;
                        item.products.promotionList[promotionInfo.id].type = 1;
                    }
                    break;
            }
            return cart;
        }

        /// <summary>
        /// 订单满XX金额时满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="cart"></param>
        /// <returns></returns>
        public int condition_ORDER_FULL(JObject parameters, CartDto cart)
        {
            if (!parameters.ContainsKey("money")) return 1;
            var objMoney = Convert.ToDecimal(parameters["money"]);
            return cart.amount >= objMoney ? 2 : 1;
        }

        /// <summary>
        /// 所有商品满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="goodsId">商品序列</param>
        /// <param name="nums">数量</param>
        /// <returns></returns>
        public int condition_GOODS_ALL(JObject parameters, int goodsId, int nums)
        {
            return 2;
        }

        /// <summary>
        /// 指定某些商品满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="goodsId">商品序列</param>
        /// <param name="nums">数量</param>
        /// <returns></returns>
        public int condition_GoodsIdS(JObject parameters, int goodsId, int nums)
        {
            if (!parameters.ContainsKey("goodsId") || !parameters.ContainsKey("nums")) return 0;

            var objNums = Convert.ToInt32(parameters["nums"]);

            var goodsIds = CommonHelper.StringToIntArray(parameters["goodsId"].ObjectToString());

            return goodsIds.Any() && goodsIds.Contains(goodsId) ? nums >= objNums ? 2 : 1 : 0;
        }


        /// <summary>
        /// 指定商品分类满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="goodsId">商品序列</param>
        /// <param name="nums">数量</param>
        /// <returns></returns>
        public async Task<int> condition_GOODS_CATS(JObject parameters, int goodsId, int nums)
        {

            using (var container = _serviceProvider.CreateScope())
            {
                var goodsServices = container.ServiceProvider.GetService<ICoreCmsGoodsServices>();

                if (!parameters.ContainsKey("catId") || !parameters.ContainsKey("nums")) return 0;
                var objCatId = parameters["catId"].ObjectToInt();
                var objNums = parameters["nums"].ObjectToInt();
                var goodsModel = await goodsServices.QueryByIdAsync(goodsId);

                if (goodsModel == null)
                {
                    return 0;
                }

                return await _goodsCategoryServices.IsChild(objCatId, goodsModel.goodsCategoryId)
                    ? nums >= objNums ? 2 : 1
                    : 0;
            }
        }

        /// <summary>
        /// 指定商品品牌满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="goodsId">商品序列</param>
        /// <param name="nums">数量</param>
        /// <returns></returns>
        public async Task<int> condition_GOODS_BRANDS(JObject parameters, int goodsId, int nums)
        {
            using (var container = _serviceProvider.CreateScope())
            {
                var goodsServices = container.ServiceProvider.GetService<ICoreCmsGoodsServices>();

                if (!parameters.ContainsKey("brandId") || !parameters.ContainsKey("nums")) return 0;
                var objBrandId = parameters["brandId"].ObjectToInt(0);
                var objNums = parameters["nums"].ObjectToInt(0);

                var goodsModel = await goodsServices.QueryByIdAsync(goodsId);
                if (goodsModel == null)
                {
                    return 0;
                }
                return goodsModel.brandId == objBrandId ? nums >= objNums ? 2 : 1 : 0;
            }
        }


        /// <summary>
        /// 指定用户等级满足条件
        /// </summary>
        /// <param name="parameters">参数对象</param>
        /// <param name="userId">用户序列</param>
        /// <returns></returns>
        public async Task<int> condition_USER_GRADE(JObject parameters, int userId)
        {
            using (var container = _serviceProvider.CreateScope())
            {
                var userServices = container.ServiceProvider.GetService<ICoreCmsUserServices>();

                if (!parameters.ContainsKey("grades")) return 0;
                var userInfo = await userServices.QueryByIdAsync(userId);
                if (userInfo == null)
                {
                    return 0;
                }
                var arr = CommonHelper.StringToIntArray(parameters["grades"].ObjectToString());
                if (arr.Contains(userInfo.grade))
                {
                    return 2;
                }
                return 0;
            }
        }
    }
}
