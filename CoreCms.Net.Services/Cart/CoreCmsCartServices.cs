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
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 购物车表 接口实现
    /// </summary>
    public class CoreCmsCartServices : BaseServices<CoreCmsCart>, ICoreCmsCartServices
    {
        private readonly ICoreCmsCartRepository _dal;

        private readonly ICoreCmsGoodsCollectionServices _goodsCollectionServices;
        private readonly ICoreCmsPinTuanRuleServices _pinTuanRuleServices;
        private readonly ICoreCmsShipServices _shipServices;
        private readonly ICoreCmsPromotionServices _promotionServices;
        private readonly ICoreCmsPromotionConditionServices _promotionConditionServices;
        private readonly ICoreCmsPromotionResultServices _promotionResultServices;
        private readonly ICoreCmsCouponServices _couponServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICoreCmsProductsServices _productsServices;
        private readonly ICoreCmsPinTuanGoodsServices _pinTuanGoodsServices;
        private readonly ICoreCmsPinTuanRecordServices _pinTuanRecordServices;
        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly ICoreCmsGoodsCategoryServices _goodsCategoryServices;

        public CoreCmsCartServices(
            ICoreCmsCartRepository dal
            , IServiceProvider serviceProvider
            , ICoreCmsGoodsCollectionServices goodsCollectionServices
            , ICoreCmsPinTuanRuleServices pinTuanRuleServices
            , ICoreCmsShipServices shipServices
            , ICoreCmsPromotionServices promotionServices
            , ICoreCmsCouponServices couponServices
            , ICoreCmsUserServices userServices
            , ICoreCmsSettingServices settingServices
            , ICoreCmsProductsServices productsServices
            , ICoreCmsPinTuanGoodsServices pinTuanGoodsServices, ICoreCmsPromotionConditionServices promotionConditionServices, ICoreCmsGoodsServices goodsServices, ICoreCmsGoodsCategoryServices goodsCategoryServices, ICoreCmsPromotionResultServices promotionResultServices, ICoreCmsPinTuanRecordServices pinTuanRecordServices)
        {
            this._dal = dal;
            base.BaseDal = dal;

            _serviceProvider = serviceProvider;
            _goodsCollectionServices = goodsCollectionServices;
            _pinTuanRuleServices = pinTuanRuleServices;
            _shipServices = shipServices;
            _promotionServices = promotionServices;
            _couponServices = couponServices;
            _userServices = userServices;
            _settingServices = settingServices;
            _productsServices = productsServices;
            _pinTuanGoodsServices = pinTuanGoodsServices;
            _promotionConditionServices = promotionConditionServices;
            _goodsServices = goodsServices;
            _goodsCategoryServices = goodsCategoryServices;
            _promotionResultServices = promotionResultServices;
            _pinTuanRecordServices = pinTuanRecordServices;
        }

        #region 设置购物车商品数量====================================================

        /// <summary>
        /// 设置购物车商品数量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nums"></param>
        /// <param name="userId"></param>
        /// <param name="numType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SetCartNum(int id, int nums, int userId, int numType, int type = 1)
        {
            var jm = new WebApiCallBack();
            if (nums <= 0)
            {
                jm.msg = "商品数量必须为正整数";
                return jm;
            }
            if (userId == 0)
            {
                jm.msg = "用户信息获取失败";
                return jm;
            }
            if (id == 0)
            {
                jm.msg = "请提交要设置的信息";
                return jm;
            }
            var cartModel = await _dal.QueryByClauseAsync(p => p.userId == userId && p.productId == id);
            if (cartModel == null)
            {
                jm.msg = "获取购物车数据失败";
                return jm;
            }
            var outData = await Add(userId, cartModel.productId, nums, numType, type);
            jm.status = outData.status;
            jm.msg = jm.status ? GlobalConstVars.SetDataSuccess : GlobalConstVars.SetDataFailure;
            jm.otherData = outData;

            return jm;
        }


        #endregion

        #region  重写删除指定ID集合的数据(批量删除)
        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> DeleteByIdsAsync(int id, int userId)
        {
            var jm = new WebApiCallBack();

            if (userId == 0)
            {
                jm.msg = "用户信息获取失败";
                return jm;
            }
            if (id <= 0)
            {
                jm.msg = "请提交要删除的货品";
                return jm;
            }
            jm.status = await _dal.DeleteAsync(p => p.id == id && p.userId == userId);
            jm.msg = jm.status ? "删除成功" : "删除失败";

            return jm;
        }
        #endregion

        #region 添加单个货品到购物车
        /// <summary>
        /// 添加单个货品到购物车
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="productId">货品序号</param>
        /// <param name="nums">数量</param>
        /// <param name="numType">数量类型/1是直接增加/2是赋值</param>
        /// <param name="cartTypes">1普通购物还是2团购秒杀3团购模式4秒杀模式6砍价模式7赠品</param>
        /// <param name="objectId">关联对象类型</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Add(int userId, int productId, int nums, int numType, int cartTypes = 1, int objectId = 0)
        {
            var jm = new WebApiCallBack();

            using var container = _serviceProvider.CreateScope();

            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();
            var productsServices = container.ServiceProvider.GetService<ICoreCmsProductsServices>();
            var goodsServices = container.ServiceProvider.GetService<ICoreCmsGoodsServices>();

            //获取数据 
            if (nums <= 0)
            {
                jm.msg = "请选择货品数量";
                return jm;
            }
            if (productId <= 0)
            {
                jm.msg = "请选择货品";
                return jm;
            }
            //获取货品信息
            var products = await productsServices.GetProductInfo(productId, false, userId); //第二个参数是不算促销信息,否则促销信息就算重复了
            if (products == null)
            {
                jm.msg = "获取货品信息失败";
                return jm;
            }
            //判断是否下架
            var isMarketable = await productsServices.GetShelfStatus(productId);
            if (isMarketable == false)
            {
                jm.msg = "货品已下架";
                return jm;
            }
            //剩余库存可购判定
            var canBuyNum = products.stock;
            //获取是否存在记录
            var catInfo = await _dal.QueryByClauseAsync(p => p.userId == userId && p.productId == productId && p.objectId == objectId);

            //根据购物车存储类型匹配数据
            switch (cartTypes)
            {
                case (int)GlobalEnumVars.OrderType.Common:

                    break;
                case (int)GlobalEnumVars.OrderType.PinTuan:
                    numType = (int)GlobalEnumVars.OrderType.PinTuan;
                    //拼团模式去判断是否开启拼团，是否存在
                    var callBack = await AddCartHavePinTuan(products.id, userId, nums, objectId);
                    if (callBack.status == false)
                    {
                        return callBack;
                    }
                    //此人的购物车中的所有购物车拼团商品都删掉，因为立即购买也是要加入购物车的，所以需要清空之前历史的加入过购物车的商品
                    await _dal.DeleteAsync(p => p.type == (int)GlobalEnumVars.OrderType.PinTuan && p.userId == userId);
                    catInfo = null;
                    break;
                case (int)GlobalEnumVars.OrderType.Group or (int)GlobalEnumVars.OrderType.Skill:
                    //标准模式不需要做什么判断
                    //判断商品是否做团购秒杀
                    if (goodsServices.IsInGroup((int)products.goodsId, out var promotionsModel, objectId) == true)
                    {
                        jm.msg = "进入判断商品是否做团购秒杀";

                        var typeIds = new int[] { (int)GlobalEnumVars.OrderType.Group, (int)GlobalEnumVars.OrderType.Skill };
                        //此人的购物车中的所有购物车拼团商品都删掉，因为立即购买也是要加入购物车的，所以需要清空之前历史的加入过购物车的商品
                        await _dal.DeleteAsync(p => typeIds.Contains(p.type) && p.productId == products.id && p.userId == userId);
                        catInfo = null;

                        var checkOrder = orderServices.FindLimitOrder(products.id, userId, promotionsModel.startTime, promotionsModel.endTime);
                        if (promotionsModel.maxGoodsNums > 0)
                        {
                            if (checkOrder.TotalOrders + nums > promotionsModel.maxGoodsNums)
                            {
                                jm.data = 15610;
                                jm.msg = GlobalErrorCodeVars.Code15610;
                                return jm;
                            }
                        }
                        if (promotionsModel.maxNums > 0)
                        {
                            if (checkOrder.TotalUserOrders > promotionsModel.maxNums)
                            {
                                jm.data = 15611;
                                jm.msg = GlobalErrorCodeVars.Code15611;
                                return jm;
                            }
                        }

                    }
                    break;
                case (int)GlobalEnumVars.OrderType.Bargain:

                    break;

                case (int)GlobalEnumVars.OrderType.Solitaire:

                    break;
                default:
                    jm.data = 10000;
                    return jm;
            }

            if (catInfo == null)
            {
                if (nums > canBuyNum)
                {
                    jm.msg = "库存不足";
                    return jm;
                }

                catInfo = new CoreCmsCart
                {
                    userId = userId,
                    productId = productId,
                    nums = nums,
                    type = cartTypes,
                    objectId = objectId
                };
                var outId = await _dal.InsertAsync(catInfo);
                jm.status = outId > 0;
                jm.data = outId;
            }
            else
            {
                if (numType == 1)
                {
                    catInfo.nums = nums + catInfo.nums;
                    if (catInfo.nums > canBuyNum)
                    {
                        jm.msg = "库存不足";
                        return jm;
                    }
                }
                else
                {
                    catInfo.nums = nums;
                }
                jm.status = await _dal.UpdateAsync(catInfo);
                jm.data = catInfo.id;
            }
            jm.msg = jm.status ? "添加成功" : "添加失败";

            return jm;
        }

        #endregion

        #region 在加入购物车的时候，判断是否有参加拼团的商品

        /// <summary>
        /// 在加入购物车的时候，判断是否有参加拼团的商品
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userId">用户序列</param>
        /// <param name="nums">加入购物车数量</param>
        /// <param name="teamId">团队序列</param>
        public async Task<WebApiCallBack> AddCartHavePinTuan(int productId, int userId = 0, int nums = 1, int teamId = 0)
        {
            var jm = new WebApiCallBack();
            var products = await _productsServices.QueryByIdAsync(productId);
            if (products == null)
            {
                jm.data = 10000;
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }
            var pinTuanGoods = await _pinTuanGoodsServices.QueryByClauseAsync(p => p.goodsId == products.goodsId);
            if (pinTuanGoods == null)
            {
                jm.data = 10000;
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }
            var pinTuanRule = await _pinTuanRuleServices.QueryByClauseAsync(p => p.id == pinTuanGoods.ruleId);
            if (pinTuanRule == null)
            {
                jm.data = 10000;
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }

            if (pinTuanRule.startTime > DateTime.Now)
            {
                jm.data = 15601;
                jm.msg = GlobalErrorCodeVars.Code15601;
                return jm;
            }
            if (pinTuanRule.endTime < DateTime.Now)
            {
                jm.data = 15602;
                jm.msg = GlobalErrorCodeVars.Code15602;
                return jm;
            }
            //查询是否存在已经开团，并且自己是队长的拼团
            var havaGroup = await _pinTuanRecordServices.ExistsAsync(p =>
                p.id == p.teamId
                && p.userId == userId
                && p.goodsId == products.goodsId
                && p.teamId == teamId
                && p.status == (int)GlobalEnumVars.PinTuanRecordStatus.InProgress);
            if (havaGroup)
            {
                jm.data = 15613;
                jm.msg = GlobalErrorCodeVars.Code15613;
                return jm;
            }

            using var container = _serviceProvider.CreateScope();
            var orderRepository = container.ServiceProvider.GetService<ICoreCmsOrderRepository>();
            var checkOrder = orderRepository.FindLimitOrder(products.id, userId, pinTuanRule.startTime, pinTuanRule.endTime, (int)GlobalEnumVars.OrderType.PinTuan);
            if (pinTuanRule.maxGoodsNums > 0)
            {
                if (checkOrder.TotalOrders + nums > pinTuanRule.maxGoodsNums)
                {
                    jm.data = 15610;
                    jm.msg = GlobalErrorCodeVars.Code15610;
                    return jm;
                }
            }
            if (pinTuanRule.maxNums > 0)
            {
                if (checkOrder.TotalUserOrders > pinTuanRule.maxNums)
                {
                    jm.data = 15611;
                    jm.msg = GlobalErrorCodeVars.Code15611;
                    return jm;
                }
            }

            jm.status = true;
            return jm;
        }


        #endregion

        #region 获取购物车原始列表(未核算)

        /// <summary>
        /// 获取购物车原始列表(未核算)
        /// </summary>
        /// <param name="userId">用户序号</param>
        /// <param name="ids">已选择货号</param>
        /// <param name="type">购物车类型/同订单类型</param>
        /// <param name="objectId">关联非订单类型数据序列</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetCartDtoData(int userId, int[] ids = null, int type = 1, int objectId = 0)
        {
            var jm = new WebApiCallBack() { methodDescription = "获取购物车原始列表(未核算)" };

            //强制过滤一遍，防止出现可以造假数据
            await _dal.DeleteAsync(p => p.userId == userId && p.nums <= 0);

            using var container = _serviceProvider.CreateScope();
            var productsService = container.ServiceProvider.GetService<ICoreCmsProductsServices>();
            var goodsServices = container.ServiceProvider.GetService<ICoreCmsGoodsServices>();


            var carts = await _dal.QueryListByClauseAsync(p => p.userId == userId && p.type == type, p => p.id, OrderByType.Asc);
            var cartDto = new CartDto { userId = userId, type = type };

            foreach (var item in carts)
            {
                var cartProducts = new CartProducts();
                //如果没有此商品，就在购物车里删掉
                var productInfo = await productsService.GetProductInfo(item.productId, false, userId);
                if (productInfo == null)
                {
                    await _dal.DeleteAsync(item);
                    continue;
                }
                //商品下架，就从购物车里面删除
                var ps = await productsService.GetShelfStatus(item.productId);
                if (ps == false)
                {
                    await _dal.DeleteAsync(item);
                    continue;
                }
                //商品金额设置为0，就从购物车里面删除
                if (productInfo.price <= 0)
                {
                    await _dal.DeleteAsync(item);
                    continue;
                }
                //获取重量
                var goodsWeight = await goodsServices.GetWeight(item.productId);

                //开始赋值
                cartProducts.id = item.id;
                cartProducts.userId = userId;
                cartProducts.productId = item.productId;
                cartProducts.nums = item.nums;
                cartProducts.type = item.type;
                cartProducts.weight = goodsWeight;
                cartProducts.products = productInfo;
                //如果传过来了购物车数据，就算指定的购物车的数据，否则，就算全部购物车的数据
                if (ids != null && ids.Any() && ids.Contains(item.id))
                {
                    cartProducts.isSelect = true;
                }
                else
                {
                    cartProducts.isSelect = false;
                }
                //判断商品是否已收藏
                cartProducts.isCollection = await _goodsCollectionServices.Check(userId, (int)cartProducts.products.goodsId);

                cartDto.list.Add(cartProducts);
            }
            //如果不同的购物车类型，可能会做一些不同的操作。
            switch (type)
            {
                case (int)GlobalEnumVars.OrderType.Common:
                    //标准模式不需要修改订单数据和商品数据
                    break;
                case (int)GlobalEnumVars.OrderType.PinTuan:
                    //拼团模式走拼团价，去修改商品价格
                    var result = _pinTuanRuleServices.PinTuanInfo(cartDto.list);
                    if (result.status)
                    {
                        cartDto.list = result.data as List<CartProducts>;
                    }
                    else
                    {
                        return result;
                    }
                    break;
                case (int)GlobalEnumVars.OrderType.Group:
                    //团购模式不需要修改订单数据和商品数据
                    break;
                case (int)GlobalEnumVars.OrderType.Skill:
                    //秒杀模式不需要修改订单数据和商品数据
                    break;
                case (int)GlobalEnumVars.OrderType.Bargain:
                    //砍价模式

                    break;
                case (int)GlobalEnumVars.OrderType.Solitaire:
                    //接龙模式，去获取接龙商品价格。
                    break;

                default:
                    jm.msg = GlobalErrorCodeVars.Code10000;
                    return jm;
            }

            jm.status = true;
            jm.data = cartDto;
            jm.msg = GlobalConstVars.GetDataSuccess;

            return jm;
        }

        #endregion

        #region 获取处理后的购物车信息

        /// <summary>
        /// 获取处理后的购物车信息
        /// </summary>
        /// <param name="userId">用户序列</param>
        /// <param name="ids">选中的购物车商品</param>
        /// <param name="orderType">订单类型</param>
        /// <param name="areaId">收货地址id</param>
        /// <param name="point">消费的积分</param>
        /// <param name="couponCode">优惠券码</param>
        /// <param name="freeFreight">是否免运费</param>
        /// <param name="deliveryType">关联上面的是否免运费/1=快递配送（要去算运费）生成订单记录快递方式  2=门店自提（不需要计算运费）生成订单记录门店自提信息</param>
        /// <param name="objectId">关联非普通订单营销类型序列</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetCartInfos(int userId, int[] ids, int orderType, int areaId, int point, string couponCode, bool freeFreight = false, int deliveryType = (int)GlobalEnumVars.OrderReceiptType.Logistics, int objectId = 0)
        {
            var jm = new WebApiCallBack() { methodDescription = "获取处理后的购物车信息" };
            var cartDto = new CartDto(); //必须初始化
            var cartDtoData = await GetCartDtoData(userId, ids, orderType, objectId);
            if (!cartDtoData.status)
            {
                jm.msg = "1";
                return cartDtoData;
            }
            cartDto = cartDtoData.data as CartDto;

            jm.msg = "2";

            //算订单总金额
            foreach (var item in cartDto.list)
            {
                jm.msg = "3";
                //库存不足不计算金额不可以选择
                if (item.nums > item.products.stock)
                {
                    item.isSelect = false;
                }
                //单条商品总价
                item.products.amount = Math.Round(item.nums * (decimal)item.products.price, 2);

                if (item.isSelect)
                {
                    //算订单总商品价格
                    cartDto.goodsAmount = Math.Round(cartDto.goodsAmount + item.products.amount, 2);
                    //算订单总价格
                    cartDto.amount = Math.Round(cartDto.amount + item.products.amount, 2);
                    //计算总重量
                    cartDto.weight = Math.Round(cartDto.weight + Math.Round(item.weight * item.nums, 2), 2);
                }
            }

            //门店订单，强制无运费
            if (deliveryType == (int)GlobalEnumVars.OrderReceiptType.IntraCityService || deliveryType == (int)GlobalEnumVars.OrderReceiptType.SelfDelivery)
            {
                freeFreight = true;
            }
            //运费判断
            if (CartFreight(cartDto, areaId, freeFreight) == false)
            {
                jm.data = cartDto;
                jm.msg = "运费判断";
                return jm;
            }
            //接下来算订单促销金额,有些模式不需要计算促销信息，这里就增加判断
            if (orderType == (int)GlobalEnumVars.OrderType.Common)
            {
                jm.data = await _promotionServices.ToPromotion(cartDto);
                jm.msg = "订单促销金额计算";
            }
            else if ((orderType == (int)GlobalEnumVars.OrderType.Group || orderType == (int)GlobalEnumVars.OrderType.Skill) && objectId > 0)
            {
                //团购秒杀默认时间过期后，不可以下单
                var dt = DateTime.Now;
                var promotionInfo = await _promotionServices.QueryByClauseAsync(p => p.startTime < dt && p.endTime > dt && p.id == objectId);

                var checkRes = await _promotionServices.SetPromotion(promotionInfo, cartDto);
                if (checkRes == false)
                {
                    jm.msg = GlobalErrorCodeVars.Code15600;
                    return jm;
                }
            }
            else if (orderType == (int)GlobalEnumVars.OrderType.PinTuan)
            {
                jm.data = await _promotionServices.ToPromotion(cartDto);
                jm.msg = "拼团也计算促销信息";
            }
            //使用优惠券，判断优惠券是否可用
            var bl = await CartCoupon(cartDto, couponCode);
            if (bl == false)
            {
                jm.status = false;
                jm.data = cartDto.error.data;
                jm.msg = cartDto.error.msg;
                return jm;
            }
            //使用积分
            var pointDto = await CartPoint(cartDto, userId, point);
            if (pointDto.status == false)
            {
                jm.status = false;
                jm.msg = pointDto.msg;
                return jm;
            }

            jm.status = true;
            jm.data = cartDto;
            jm.msg = "4";

            return jm;
        }

        #endregion

        #region 算运费
        /// <summary>
        /// 算运费
        /// </summary>
        /// <param name="cartDto">购物车信息</param>
        /// <param name="areaId">收货地址id</param>
        /// <param name="freeFreight">是否包邮，默认false</param>
        /// <returns></returns>
        public bool CartFreight(CartDto cartDto, int areaId, bool freeFreight = false)
        {
            if (freeFreight == false)
            {
                if (areaId > 0)
                {
                    cartDto.costFreight = _shipServices.GetShipCost(areaId, cartDto.weight, cartDto.goodsAmount);
                    cartDto.amount = Math.Round(cartDto.amount + cartDto.costFreight, 2);
                }
            }
            return true;
        }

        #endregion

        #region 购物车中使用优惠券

        /// <summary>
        /// 购物车中使用优惠券
        /// </summary>
        /// <param name="cartDto"></param>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        public async Task<bool> CartCoupon(CartDto cartDto, string couponCode)
        {
            if (!string.IsNullOrEmpty(couponCode))
            {
                var arr = couponCode.Split(",");
                //判断优惠券是否可用
                var couponInfo = await _couponServices.CodeToInfo(arr, true);
                if (couponInfo.status == false)
                {
                    cartDto.error = couponInfo;
                    return false;
                }
                //判断优惠券是否符合规格
                var res = await _promotionServices.ToCoupon(cartDto, couponInfo.data as List<CoreCmsPromotion>);
                if (res.status == false)
                {
                    cartDto.error = res;
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 购物车中使用积分

        /// <summary>
        /// 购物车中使用积分
        /// </summary>
        /// <param name="cartDto"></param>
        /// <param name="userId"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> CartPoint(CartDto cartDto, int userId, int point)
        {
            var jm = new WebApiCallBack() { status = true };
            if (point > 0)
            {
                var getUserPointDto = await _userServices.GetUserPoint(userId, 0);
                if (getUserPointDto.point < point)
                {
                    jm.status = false;
                    jm.msg = "积分不足，无法使用积分";
                    return jm;
                }
                //判断积分值多少钱
                //计算可用积分

                var allConfigs = await _settingServices.GetConfigDictionaries();

                var ordersPointProportion = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OrdersPointProportion).ObjectToInt(10);//订单积分使用比例
                var maxPointDeductedMoney = Math.Round(cartDto.amount * ordersPointProportion / 100, 2); //最大积分抵扣的钱
                var pointDiscountedProportion = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.PointDiscountedProportion).ObjectToInt(100); //积分兑换比例
                var pointDeductedMoney = point / pointDiscountedProportion; //积分可以抵扣的钱

                if (maxPointDeductedMoney < pointDeductedMoney)
                {
                    jm.status = false;
                    jm.msg = "积分超过订单可使用的积分数量";
                    return jm;
                }
                cartDto.point = point;
                cartDto.pointExchangeMoney = pointDeductedMoney;
                cartDto.amount -= pointDeductedMoney;
            }
            jm.data = cartDto;
            return jm;
        }

        #endregion

        #region 获取购物车用户数据总数
        /// <summary>
        ///     获取购物车用户数据总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetCountAsync(int userId)
        {
            return await _dal.GetCountAsync(userId);
        }

        #endregion


        #region 根据提交的数据判断哪些购物券可以使用
        /// <summary>
        /// 根据提交的数据判断哪些购物券可以使用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetCartAvailableCoupon(int userId, int[] ids = null)
        {
            var jm = new WebApiCallBack();
            var dt = DateTime.Now;
            var resultData = new List<CoreCmsCoupon>();

            //取用户数据，如果用户没登录，都没意义。
            var user = await _userServices.QueryByClauseAsync(p => p.id == userId);
            if (user == null)
            {
                return jm;
            }
            //先取购物车数据，如果购物车都没存购货品数据，优惠券就没意义
            //获取货品数据
            var carts = await _dal.QueryListByClauseAsync(p => p.userId == userId && p.type == (int)GlobalEnumVars.OrderType.Common);
            if (!carts.Any())
            {
                return jm;
            }
            var productIds = carts.Select(p => p.productId).ToList();
            var products = await _productsServices.QueryListByClauseAsync(p => productIds.Contains(p.id));
            if (!products.Any())
            {
                return jm;
            }
            var cartProducts = new List<CartProducts>();
            var goodIds = new List<int>();
            foreach (var item in carts)
            {
                var cp = new CartProducts { products = products.Find(p => p.id == item.productId) };
                if (cp.products == null)
                {
                    continue;
                }
                //开始赋值
                cp.id = item.id;
                cp.userId = userId;
                cp.productId = item.productId;
                cp.nums = item.nums;
                cp.type = item.type;
                //如果传过来了购物车数据，就算指定的购物车的数据，否则，就算全部购物车的数据
                if (ids != null && ids.Any())
                {
                    if (ids.Contains(item.id))
                    {
                        goodIds.Add(cp.products.goodsId);
                        cartProducts.Add(cp);
                    }
                    continue;
                }
                goodIds.Add(cp.products.goodsId);
                cartProducts.Add(cp);
            }
            //如果获取赛选后没了，就返回无
            if (!cartProducts.Any()) return jm;
            //获取商品数据
            var goods = await _goodsServices.QueryListByClauseAsync(p => goodIds.Contains(p.id));
            //如果商品都被已下架或者删除，也没了。
            if (!goods.Any()) return jm;

            cartProducts.ForEach(p =>
            {
                p.good = goods.Find(g => g.id == p.products.goodsId);
            });


            //获取我的可用优惠券
            var wherec = PredicateBuilder.True<CoreCmsCoupon>();
            wherec = wherec.And(p => p.userId == userId);
            wherec = wherec.And(p => p.isUsed == false);
            wherec = wherec.And(p => p.endTime > dt);
            var userCoupon = await _couponServices.QueryPageMapperAsync(wherec, p => p.createTime, OrderByType.Desc, false);
            if (!userCoupon.Any())
            {
                return jm;
            }

            //获取所有商品分类
            var goodsCategories = await _goodsCategoryServices.GetCaChe();

            foreach (var coupon in userCoupon)
            {
                if (coupon.promotion == null)
                {
                    continue;
                }

                //先判断优惠券是否可以使用
                //判断规则是否开启
                if (coupon.promotion.isEnable == false) continue;
                //判断优惠券规则是否到达开始时间
                if (coupon.startTime > dt) continue;
                //判断优惠券规则是否已经到结束时间了，也就是是否过期了
                if (coupon.endTime < dt) continue;

                //再来判断是否符合规格
                //判断是哪个规则，并且确认是否符合|只要有一个规则不满足，就失效。
                var type = 0;
                foreach (var pc in coupon.conditions)
                {
                    type = 0;
                    JObject parameters = (JObject)JsonConvert.DeserializeObject(pc.parameters);
                    if (parameters == null) break;
                    var objNums = 0;
                    switch (pc.code)
                    {
                        case "GOODS_ALL":
                            //只要购物车有商品就支持，因为是购物车付款调用接口，所以不存在无商品情况
                            type = 2;
                            break;
                        case "GOODS_IDS":
                            //指定某些商品满足条件
                            if (!parameters.ContainsKey("goodsId") || !parameters.ContainsKey("nums")) break;
                            objNums = parameters["nums"].ObjectToInt(0);
                            var goodsIds = CommonHelper.StringToIntArray(parameters["goodsId"].ObjectToString());
                            //只要有一个商品支持，此优惠券就可以使用。
                            if (goodsIds.Any())
                            {
                                foreach (var p in cartProducts)
                                {
                                    if (!goodsIds.Contains(p.products.goodsId))
                                    {
                                        continue;
                                    }
                                    if (p.nums >= objNums)
                                    {
                                        type = 2;
                                        break;
                                    }
                                }
                            }
                            break;
                        case "GOODS_CATS":
                            //指定商品是否满足分类
                            if (!parameters.ContainsKey("catId") || !parameters.ContainsKey("nums")) break;
                            var objCatId = parameters["catId"].ObjectToInt(0);
                            objNums = parameters["nums"].ObjectToInt(0);

                            foreach (var product in cartProducts)
                            {
                                type = _goodsCategoryServices.IsHave(goodsCategories, objCatId, product.good.goodsCategoryId) ? product.nums >= objNums ? 2 : 1
                                    : 0;
                                if (type == 2)
                                {
                                    break;
                                }
                            }
                            break;
                        case "GOODS_BRANDS":
                            //指定商品品牌满足条件
                            if (!parameters.ContainsKey("brandId") || !parameters.ContainsKey("nums")) break;
                            var objBrandId = parameters["brandId"].ObjectToInt(0);
                            objNums = parameters["nums"].ObjectToInt(0);

                            foreach (var product in cartProducts)
                            {
                                type = product.good.brandId == objBrandId ? product.nums >= objNums ? 2 : 1 : 0;
                                if (type == 2)
                                {
                                    break;
                                }
                            }
                            break;
                        case "ORDER_FULL":
                            //订单满XX金额满足条件
                            if (!parameters.ContainsKey("money")) break;
                            var objMoney = parameters["money"].ObjectToDecimal();
                            //算订单总商品价格
                            decimal goodsAmount = 0;
                            foreach (var product in cartProducts)
                            {
                                var money = product.products.price * product.nums;
                                goodsAmount += Math.Round(goodsAmount + money, 2);
                            }
                            if (goodsAmount >= objMoney)
                            {
                                type = 2;
                            }
                            break;
                        case "USER_GRADE":
                            //用户符合指定等级
                            if (!parameters.ContainsKey("grades")) break;
                            var arr = CommonHelper.StringToIntArray(parameters["grades"].ObjectToString());
                            if (arr.Contains(user.grade))
                            {
                                type = 2;
                            }
                            break;
                        default:
                            break;
                    }
                    //不满足条件，直接跳过
                    if (type != 2)
                    {
                        break;
                    }
                }

                if (type == 2)
                {
                    resultData.Add(coupon);
                }
            }

            //结果集拼装
            var resutlList = new List<GetMyCouponResultDto>();
            if (resultData != null && resultData.Any())
            {
                foreach (var item in resultData)
                {
                    var expression1 = string.Empty;
                    var expression2 = string.Empty;

                    foreach (var condition in item.conditions)
                    {
                        expression1 += PromotionHelper.GetConditionMsg(condition.code, condition.parameters);
                    }
                    foreach (var result in item.results)
                    {
                        expression2 += PromotionHelper.GetResultMsg(result.code, result.parameters);
                    }

                    var dto = new GetMyCouponResultDto
                    {
                        couponCode = item.couponCode,
                        promotionId = item.promotionId,
                        isUsed = item.isUsed,
                        userId = item.userId,
                        usedId = item.usedId,
                        createTime = item.createTime,
                        updateTime = item.updateTime,
                        couponName = item.promotion.name,
                        expression1 = expression1,
                        expression2 = expression2,
                        isExpire = item.endTime > dt,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        stime = item.startTime.ToString("yyyy-MM-dd"),
                        etime = item.endTime.ToString("yyyy-MM-dd")
                    };

                    resutlList.Add(dto);
                }
            }
            jm.status = true;
            jm.data = new
            {
                list = resutlList,
                changeData = resultData,
                cartProducts,
                user
            };
            //jm.otherData = userCoupon;
            return jm;
        }

        #endregion

    }
}
