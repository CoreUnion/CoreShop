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
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 促销结果表 接口实现
    /// </summary>
    public class CoreCmsPromotionResultServices : BaseServices<CoreCmsPromotionResult>, ICoreCmsPromotionResultServices
    {
        private readonly ICoreCmsPromotionResultRepository _dal;

        private ICoreCmsPromotionConditionServices _promotionConditionServices;


        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsPromotionResultServices(IUnitOfWork unitOfWork, ICoreCmsPromotionResultRepository dal, ICoreCmsPromotionConditionServices promotionConditionServices)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _promotionConditionServices = promotionConditionServices;
        }



        //去计算结果
        public async Task<bool> toResult(CoreCmsPromotionResult resultInfo, CartDto cart,
            CoreCmsPromotion promotionInfo)
        {
            if (string.IsNullOrEmpty(resultInfo.parameters)) return false;

            var resultType = SystemSettingDictionary.GetPromotionResultType();
            var resultModel = resultType.Find(p => p.sKey == resultInfo.code);
            if (resultModel != null)
            {
                JObject parameters = (JObject)JsonConvert.DeserializeObject(resultInfo.parameters);
                //如果是订单促销就直接去判断促销条件，如果是商品促销，就循环订单明细
                if (resultModel.sValue == "goods")
                {
                    var isUsed = false;
                    foreach (var item in cart.list)
                    {
                        var type = await _promotionConditionServices.goods_check(promotionInfo.id, (int)item.products.goodsId, item.nums);
                        if (type == 2)
                        {
                            //到这里就说明此商品信息满足促销商品促销信息的条件，去计算结果
                            //注意，在明细上面，就不细分促销的种类了，都放到一个上面，在订单上面才细分
                            decimal promotionModel = 0;
                            if (isUsed == false)
                            {
                                switch (resultInfo.code)
                                {
                                    case "GOODS_REDUCE":
                                        promotionModel = result_GOODS_REDUCE(parameters, item, promotionInfo);
                                        break;
                                    case "GOODS_DISCOUNT":
                                        promotionModel = result_GOODS_DISCOUNT(parameters, item, promotionInfo);
                                        break;
                                    case "GOODS_ONE_PRICE":
                                        promotionModel = result_GOODS_ONE_PRICE(parameters, item, promotionInfo);
                                        break;
                                    case "GOODS_HALF_PRICE": //todo 指定商品每第几件减指定金额
                                        promotionModel = result_GOODS_HALF_PRICE(parameters, item, promotionInfo);
                                        break;
                                    default:
                                        promotionModel = 0;
                                        break;
                                }
                            }

                            if (item.isSelect)
                            {
                                switch (promotionInfo.type)
                                {
                                    case (int)GlobalEnumVars.PromotionType.Promotion:
                                        //设置总的商品促销金额
                                        cart.goodsPromotionMoney = Math.Round(cart.goodsPromotionMoney + promotionModel, 2);
                                        //设置总的价格
                                        cart.amount = Math.Round(cart.amount - promotionModel, 2);
                                        break;
                                    case (int)GlobalEnumVars.PromotionType.Coupon:
                                        if (isUsed)
                                        {
                                            item.products.promotionList.Remove(promotionInfo.id);
                                        }
                                        else
                                        {
                                            //优惠券促销金额
                                            cart.couponPromotionMoney = Math.Round(cart.couponPromotionMoney + promotionModel, 2);
                                            //设置总的价格
                                            cart.amount = Math.Round(cart.amount - promotionModel, 2);
                                            //跳出下级处理
                                            isUsed = true;
                                        }
                                        break;
                                    case (int)GlobalEnumVars.PromotionType.Group:
                                        //团购
                                        cart.goodsPromotionMoney = Math.Round(cart.goodsPromotionMoney + promotionModel, 2);
                                        //设置总的价格
                                        cart.amount = Math.Round(cart.amount - promotionModel, 2);
                                        break;
                                    case (int)GlobalEnumVars.PromotionType.Seckill:
                                        //秒杀
                                        cart.goodsPromotionMoney = Math.Round(cart.goodsPromotionMoney + promotionModel, 2);
                                        //设置总的价格
                                        cart.amount = Math.Round(cart.amount - promotionModel, 2);
                                        break;
                                }
                            }

                        }
                    }
                    //商品促销可能做的比较狠，导致订单价格为负数了，这里判断一下，如果订单价格小于0了，就是0了
                    cart.amount = cart.amount > 0 ? cart.amount : 0;
                }
                else
                {
                    if (resultInfo.code == "ORDER_DISCOUNT")
                    {
                        result_ORDER_DISCOUNT(parameters, cart, promotionInfo);
                    }
                    else if (resultInfo.code == "ORDER_REDUCE")
                    {
                        result_ORDER_REDUCE(parameters, cart, promotionInfo);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 订单减固定金额
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cart"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        public bool result_ORDER_REDUCE(JObject parameters, CartDto cart, CoreCmsPromotion promotionInfo)
        {
            if (!parameters.ContainsKey("money")) return true;

            //判断极端情况，减的太多，超过购物车的总金额了，那么就最多减到0
            if (cart.amount < (decimal)parameters["money"])
            {
                parameters["money"] = cart.amount;
            }
            //总价格修改
            cart.amount -= (decimal)parameters["money"];
            switch (promotionInfo.type)
            {
                case (int)GlobalEnumVars.PromotionType.Promotion:
                    //总促销修改
                    cart.orderPromotionMoney += (decimal)parameters["money"];
                    //设置促销列表
                    if (cart.promotionList.ContainsKey(promotionInfo.id))
                    {
                        cart.promotionList[promotionInfo.id].name = promotionInfo.name;
                        cart.promotionList[promotionInfo.id].type = 2;
                    }
                    else
                    {
                        cart.promotionList.Add(promotionInfo.id, new WxNameTypeDto() { name = promotionInfo.name, type = 2 });
                    }
                    break;

                case (int)GlobalEnumVars.PromotionType.Coupon:
                    //优惠券促销金额
                    cart.couponPromotionMoney += (decimal)parameters["money"];
                    break;
            }
            return true;
        }

        /// <summary>
        /// 订单打X折
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cart"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        public bool result_ORDER_DISCOUNT(JObject parameters, CartDto cart, CoreCmsPromotion promotionInfo)
        {
            //if (parameters.Property("discount") == null) return true;
            //var objDiscount = Convert.ToInt32(parameters["discount"]);

            if (!parameters.ContainsKey("discount")) return true;
            var objDiscount = parameters["discount"].ObjectToDecimal(0);

            //判断参数是否设置的正确
            if (objDiscount >= 10 || objDiscount <= 0)
            {
                return true;
            }
            var orderAmount = cart.amount;
            //总价格修改
            cart.amount = Math.Round(Math.Round(Math.Round(cart.amount * objDiscount, 3) * 10, 2) / 100, 2);
            switch (promotionInfo.type)
            {
                case (int)GlobalEnumVars.PromotionType.Promotion:
                    //总促销修改
                    cart.orderPromotionMoney = Math.Round(cart.orderPromotionMoney + Math.Round(orderAmount - cart.amount, 2), 2);
                    //设置促销列表
                    if (cart.promotionList.ContainsKey(promotionInfo.id))
                    {
                        cart.promotionList[promotionInfo.id].name = promotionInfo.name;
                        cart.promotionList[promotionInfo.id].type = 2;

                    }
                    else
                    {
                        cart.promotionList.Add(promotionInfo.id, new WxNameTypeDto() { name = promotionInfo.name, type = 2 });
                    }
                    break;

                case (int)GlobalEnumVars.PromotionType.Coupon:
                    //优惠券促销金额
                    cart.couponPromotionMoney = Math.Round(cart.couponPromotionMoney + Math.Round(orderAmount - cart.amount, 2), 2);
                    break;
            }
            return true;
        }

        /// <summary>
        /// 指定商品减固定金额
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cartProducts"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        public decimal result_GOODS_REDUCE(JObject parameters, CartProducts cartProducts, CoreCmsPromotion promotionInfo)
        {
            if (!parameters.ContainsKey("money")) return 0;
            var objMoney = parameters["money"].ObjectToDecimal(0);

            decimal promotionMoney = 0;
            //判断极端情况，减的太多，超过商品单价了，那么就最多减到0
            if (cartProducts.products.price < objMoney)
            {
                objMoney = cartProducts.products.price;
            }
            cartProducts.products.price = Math.Round(cartProducts.products.price - objMoney, 2);
            //此次商品促销一共优惠了多少钱
            promotionMoney = promotionInfo.type == (int)GlobalEnumVars.PromotionType.Coupon ? objMoney : Math.Round(cartProducts.nums * objMoney, 2);
            //设置商品优惠总金额
            cartProducts.products.promotionAmount = Math.Round(cartProducts.products.promotionAmount + objMoney, 2);
            //设置商品的实际销售金额（单品）
            cartProducts.products.amount = Math.Round(cartProducts.products.amount - promotionMoney, 2);
            return promotionMoney;
        }

        /// <summary>
        /// 指定商品打X折
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cartProducts"></param>
        /// <param name="promotionInfo"></param>
        /// <returns></returns>
        public decimal result_GOODS_DISCOUNT(JObject parameters, CartProducts cartProducts, CoreCmsPromotion promotionInfo)
        {
            if (!parameters.ContainsKey("discount")) return 0;
            var objDiscount = parameters["discount"].ObjectToDecimal(0);

            decimal promotionMoney = 0;
            decimal goodsPrice = cartProducts.products.price;
            cartProducts.products.price = Math.Round(Math.Round(Math.Round(cartProducts.products.price * objDiscount, 3) * 10, 2) / 100, 2);
            //单品优惠的金额
            var pmoney = Math.Round(goodsPrice - cartProducts.products.price, 2);
            promotionMoney = promotionInfo.type == (int)GlobalEnumVars.PromotionType.Coupon ? pmoney : Math.Round(cartProducts.nums * pmoney, 2);
            //设置商品优惠总金额
            cartProducts.products.promotionAmount = Math.Round(cartProducts.products.promotionAmount + promotionMoney, 2);
            //设置商品的实际销售总金额
            cartProducts.products.amount = Math.Round(cartProducts.products.amount - promotionMoney, 2);

            return promotionMoney;
        }

        //商品一口价
        public decimal result_GOODS_ONE_PRICE(JObject parameters, CartProducts cartProducts, CoreCmsPromotion promotionInfo)
        {
            if (!parameters.ContainsKey("money")) return 0;
            var objMoney = parameters["money"].ObjectToDecimal(0);

            //如果一口价比商品价格高，那么就不执行了
            decimal promotionMoney = 0;
            if (cartProducts.products.price <= objMoney)
            {
                return promotionMoney;
            }
            var goodsPrice = (decimal)cartProducts.products.price;
            cartProducts.products.price = Math.Round(objMoney, 2);
            //单品优惠的金额
            var pmoney = Math.Round(goodsPrice - cartProducts.products.price, 2);
            promotionMoney = promotionInfo.type == (int)GlobalEnumVars.PromotionType.Coupon ? pmoney : Math.Round(cartProducts.nums * pmoney, 2);
            //设置商品优惠总金额
            cartProducts.products.promotionAmount = Math.Round(cartProducts.products.promotionAmount + promotionMoney, 2);
            //设置商品的实际销售总金额
            cartProducts.products.amount = Math.Round(cartProducts.products.amount - promotionMoney, 2);
            return promotionMoney;
        }

        //商品第N个半价
        public decimal result_GOODS_HALF_PRICE(JObject parameters, CartProducts cartProducts, CoreCmsPromotion promotionInfo)
        {
            if (!parameters.ContainsKey("money")) return 0;
            var objMoney = parameters["money"].ObjectToDecimal(0);

            //如果一口价比商品价格高，那么就不执行了
            decimal promotionMoney = 0;
            if (cartProducts.products.price <= objMoney)
            {
                return promotionMoney;
            }
            //第几个优惠
            var num = parameters["num"].ObjectToInt(0);
            //购买的数量
            var buyNum = cartProducts.nums;
            //取整数，保证满足了，才优惠  ，比如设置 原价 10 第2个 减少5，购买5个产品的时候，实际只优惠2个产品的价格
            var promotionNum = buyNum / num;
            var pmoney = Math.Round((decimal)promotionNum * objMoney / buyNum, 2);  //单品优惠的金额 
            var goodsPrice = (decimal)cartProducts.products.price;
            cartProducts.products.price = goodsPrice - pmoney; //优惠后的平均价格 
            promotionMoney = Math.Round(cartProducts.nums * pmoney, 2);//优惠金额  
            //设置商品优惠总金额
            cartProducts.products.promotionAmount = Math.Round(cartProducts.products.promotionAmount + promotionMoney, 2);
            //设置商品的实际销售总金额
            cartProducts.products.amount = Math.Round(cartProducts.products.amount - promotionMoney, 2);
            return promotionMoney;
        }

    }
}
