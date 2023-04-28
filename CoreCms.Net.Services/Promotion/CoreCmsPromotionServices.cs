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
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 促销表 接口实现
    /// </summary>
    public class CoreCmsPromotionServices : BaseServices<CoreCmsPromotion>, ICoreCmsPromotionServices
    {
        private readonly ICoreCmsPromotionRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICoreCmsPromotionConditionServices _promotionConditionServices;
        private readonly ICoreCmsPromotionResultServices _promotionResultServices;
        private readonly IServiceProvider _serviceProvider;

        private readonly ICoreCmsCouponServices _couponServices;


        public CoreCmsPromotionServices(IUnitOfWork unitOfWork
            , ICoreCmsPromotionRepository dal
            , ICoreCmsPromotionConditionServices promotionConditionServices
            , ICoreCmsPromotionResultServices promotionResultServices
            , IServiceProvider serviceProvider, ICoreCmsCouponServices couponServices)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;

            _promotionConditionServices = promotionConditionServices;
            _promotionResultServices = promotionResultServices;
            _serviceProvider = serviceProvider;
            _couponServices = couponServices;
        }


        #region 购物车的数据传过来，然后去算促销================

        /// <summary>
        /// 购物车的数据传过来，然后去算促销
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<CartDto> ToPromotion(CartDto cart, int type = (int)GlobalEnumVars.PromotionType.Promotion)
        {
            //团购秒杀不会走到这里,团购秒杀直接调用setPromotion方法
            if (type == (int)GlobalEnumVars.PromotionType.Group || type == (int)GlobalEnumVars.PromotionType.Seckill)
            {
                return cart;
            }

            //按照权重取所有已生效的促销列表
            var dt = DateTime.Now;
            var promotions = await _dal.QueryListByClauseAsync(p => p.isEnable == true && p.startTime < dt && p.endTime > dt && p.type == (int)GlobalEnumVars.PromotionType.Promotion && p.isDel == false, p => p.sort, OrderByType.Asc);

            foreach (var item in promotions)
            {
                await SetPromotion(item, cart);
                if (item.isExclusive == true) break;
            }

            return cart;
        }
        #endregion

        #region 购物车的数据传过来，然后去算优惠券

        /// <summary>
        /// 购物车的数据传过来，然后去算优惠券
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="promotions"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ToCoupon(CartDto cart, List<CoreCmsPromotion> promotions)
        {
            var jm = new WebApiCallBack();
            foreach (var item in promotions)
            {
                if (item == null)
                {
                    jm.data = 15014;
                    jm.msg = GlobalErrorCodeVars.Code15014;
                    return jm;
                }
                var bl = await SetPromotion(item, cart);
                if (bl)
                {
                    cart.coupon.Add(item.name);
                }
                else
                {
                    jm.data = 15014;
                    jm.msg = GlobalErrorCodeVars.Code15014;
                    return jm;
                }
            }
            jm.status = true;
            jm.data = cart;
            return jm;
        }

        #endregion

        #region 根据促销信息，去计算购物车的促销情况

        /// <summary>
        /// 根据促销信息，去计算购物车的促销情况
        /// </summary>
        /// <param name="promotion"></param>
        /// <param name="cartModel"></param>
        /// <returns></returns>
        public async Task<bool> SetPromotion(CoreCmsPromotion promotion, CartDto cartModel)
        {
            var promotionConditions = await _promotionConditionServices.QueryListByClauseAsync(p => p.promotionId == promotion.id);
            //循环取出所有的促销条件，有一条不满足，就不行，就返回false
            var key = true;
            foreach (var item in promotionConditions)
            {
                var res = await _promotionConditionServices.check(item, cartModel, promotion);
                if (!key) continue;
                if (!res)
                {
                    //多个促销条件中，如果有一个不满足，整体就不满足，但是为了显示完整的促销标签，还是要运算完所有的促销条件
                    key = false;
                }
            }
            if (key)
            {
                //走到这一步就说明所有的促销条件都符合，那么就去计算结果
                var promotionResults = await _promotionResultServices.QueryListByClauseAsync(p => p.promotionId == promotion.id);
                foreach (var item in promotionResults)
                {
                    await _promotionResultServices.toResult(item, cartModel, promotion);
                }
            }
            else
            {
                //如果不满足需求，就要统一标准，把有些满足条件的（2），变成1
                _promotionConditionServices.PromotionFalse(cartModel, promotion);
            }
            return key;
        }
        #endregion

        #region 获取团购列表数据
        /// <summary>
        /// 获取团购列表数据
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetGroupList(int type, int userId, int status, int pageIndex, int pageSize)
        {

            var jm = new WebApiCallBack { status = true };

            var where = PredicateBuilder.True<CoreCmsPromotion>();
            where = where.And(p => p.isEnable == true && p.isDel == false);
            if (type == (int)GlobalEnumVars.PromotionType.Group)
            {
                where = where.And(p => p.type == (int)GlobalEnumVars.PromotionType.Group);
            }
            else if (type == (int)GlobalEnumVars.PromotionType.Seckill)
            {
                where = where.And(p => p.type == (int)GlobalEnumVars.PromotionType.Seckill);
            }

            var dt = DateTime.Now;

            if (status == (int)GlobalEnumVars.GroupSeckillStatus.Upcoming)
            {
                where = where.And(p => p.startTime > dt);

            }
            else if (status == (int)GlobalEnumVars.GroupSeckillStatus.InProgress)
            {
                where = where.And(p => p.startTime < dt && dt < p.endTime);
            }
            else if (status == (int)GlobalEnumVars.GroupSeckillStatus.Finished)
            {
                where = where.And(p => p.endTime < dt);
            }

            var goods = new List<CoreCmsGoods>();
            var list = await _dal.QueryPageAsync(where, p => p.endTime, OrderByType.Desc, pageIndex, pageSize);
            if (list != null && list.Any())
            {
                foreach (var item in list)
                {
                    var promotionId = item.id;
                    var condition = await _promotionConditionServices.QueryByClauseAsync(p => p.promotionId == promotionId);
                    if (condition != null && condition.parameters.Contains("goodsId"))
                    {
                        JObject parameters = (JObject)JsonConvert.DeserializeObject(condition.parameters);

                        var res = await GetGroupDetail(parameters["goodsId"].ObjectToInt(0), userId, "group", item.id);
                        if (res.status)
                        {
                            var good = res.data as CoreCmsGoods;

                            good.groupId = item.id;
                            good.groupType = item.type;
                            good.groupStatus = item.isEnable;
                            good.groupTime = DateTime.Now;
                            good.groupStartTime = item.startTime;
                            good.groupEndTime = item.endTime;

                            TimeSpan ts = item.endTime.Subtract(dt);
                            good.groupTimestamp = (int)ts.TotalSeconds;

                            goods.Add(good);
                        }
                        else
                        {
                            item.expression1 = res.msg;
                        }
                    }
                }
            }

            jm.data = new
            {
                goods,
                list.TotalCount,
                list.TotalPages,
                list,
                pageIndex,
                pageSize
            };
            return jm;
        }

        #endregion

        #region 获取团购/秒杀商品详情
        /// <summary>
        /// 获取团购/秒杀商品详情
        /// </summary>
        /// <returns></returns>

        public async Task<WebApiCallBack> GetGroupDetail(int goodId = 0, int userId = 0, string type = "group", int groupId = 0)
        {
            using var container = _serviceProvider.CreateScope();

            var goodsServices = container.ServiceProvider.GetService<ICoreCmsGoodsServices>();
            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();

            var jm = new WebApiCallBack() { msg = "关键参数丢失" };

            if (goodId == 0)
            {
                return jm;
            }
            //判断商品是否参加团购
            var isInGroup = _dal.IsInGroup(goodId, out var promotionId);
            if (!isInGroup)
            {
                jm.msg = "商品未参加团购";
                return jm;
            }

            var promotion = await _dal.QueryByClauseAsync(p => p.isDel == false && p.isEnable == true && p.id == promotionId);
            if (promotion == null)
            {
                jm.msg = "无此活动";
                jm.otherData = promotionId;
                return jm;
            }

            var goods = new CoreCmsGoods();
            goods = await goodsServices.GetGoodsDetial(goodId, userId, true, type, groupId);
            if (goods == null)
            {
                jm.msg = "商品不存在";
                return jm;
            }

            if (goods.isMarketable == false)
            {
                jm.msg = "商品已下架";
                return jm;
            }

            //调整前台显示数量
            if (!string.IsNullOrEmpty(promotion.parameters))
            {
                var extendParams = (JObject)JsonConvert.DeserializeObject(promotion.parameters);
                var checkOrder = orderServices.FindLimitOrder(goods.product.id, userId, promotion.startTime, promotion.endTime);

                if (extendParams != null && extendParams.ContainsKey("max_goods_nums") && extendParams["max_goods_nums"].ObjectToInt(0) != 0)
                {
                    var maxGoodsNums = extendParams["max_goods_nums"].ObjectToInt(0);
                    goods.stock = maxGoodsNums;
                    //活动销售件数
                    goods.product.stock = maxGoodsNums - checkOrder.TotalOrders;
                    goods.buyPromotionCount = checkOrder.TotalOrders;
                }
                else
                {
                    goods.buyPromotionCount = checkOrder.TotalOrders;
                }
            }

            var dt = DateTime.Now;

            goods.groupId = promotion.id;
            goods.groupType = promotion.type;
            goods.groupStatus = promotion.isEnable;
            goods.groupTime = dt;
            goods.groupStartTime = promotion.startTime;
            goods.groupEndTime = promotion.endTime;

            TimeSpan ts = promotion.endTime.Subtract(dt);
            goods.groupTimestamp = (int)ts.TotalSeconds;


            //进行促销后要更换原销售价替换原市场价
            var originPrice = Math.Round(goods.product.price + goods.product.promotionAmount, 2);
            goods.product.mktprice = originPrice;
            jm.status = true;
            jm.msg = "数据获取成功";
            jm.data = goods;

            return jm;
        }

        #endregion

        #region 获取可领取的优惠券（不分页）
        /// <summary>
        /// 获取可领取的优惠券（不分页）
        /// </summary>
        /// <param name="limit">数量</param>
        /// <returns></returns>
        public async Task<List<CoreCmsPromotion>> ReceiveCouponList(int limit = 3)
        {
            var where = PredicateBuilder.True<CoreCmsPromotion>();
            where = where.And(p => p.endTime > DateTime.Now); //判断优惠券失效时间 是否可领取
            where = where.And(p => p.isEnable == true); //启用状态
            where = where.And(p => p.type == (int)GlobalEnumVars.PromotionType.Coupon);  //促销 类型
            where = where.And(p => p.isAutoReceive == true);  //自动领取状态
            where = where.And(p => p.isDel == false);  //是否被删除

            var data = await _dal.QueryPageAndChildsAsync(where, p => p.id, OrderByType.Desc, false, 0, limit);

            if (data != null && data.Any())
            {
                foreach (var item in data)
                {

                    var expression1 = "";
                    var expression2 = "";

                    foreach (var condition in item.promotionCondition)
                    {
                        var str = PromotionHelper.GetConditionMsg(condition.code, condition.parameters);
                        expression1 += str;
                        item.conditions.Add(str);
                    }
                    foreach (var result in item.promotionResult)
                    {
                        var str = PromotionHelper.GetResultMsg(result.code, result.parameters);
                        expression2 += str;
                        item.results.Add(str);
                    }
                    item.expression1 = expression1;
                    item.expression2 = expression2;
                }
            }
            return data.ToList();
        }
        #endregion

        #region 获取可领取的优惠券（分页）

        /// <summary>
        /// 获取可领取的优惠券（分页）
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">数量</param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsPromotion>> GetReceiveCouponList(int page = 1, int limit = 10)
        {
            var where = PredicateBuilder.True<CoreCmsPromotion>();
            where = where.And(p => p.endTime > DateTime.Now); //判断优惠券失效时间 是否可领取
            where = where.And(p => p.isEnable == true); //启用状态
            where = where.And(p => p.type == (int)GlobalEnumVars.PromotionType.Coupon);  //促销 类型
            where = where.And(p => p.isAutoReceive == true);  //自动领取状态
            where = where.And(p => p.isDel == false);  //是否被删除

            var data = await _dal.QueryPageAndChildsAsync(where, p => p.id, OrderByType.Desc, true, page, limit);

            if (data != null && data.Any())
            {
                foreach (var item in data)
                {
                    var expression1 = "";
                    var expression2 = "";
                    foreach (var condition in item.promotionCondition)
                    {
                        var str = PromotionHelper.GetConditionMsg(condition.code, condition.parameters);
                        expression1 += str;
                        item.conditions.Add(str);
                    }
                    foreach (var result in item.promotionResult)
                    {
                        var str = PromotionHelper.GetResultMsg(result.code, result.parameters);
                        expression2 += str;
                        item.results.Add(str);
                    }
                    item.expression1 = expression1;
                    item.expression2 = expression2;
                }
            }
            return data;
        }
        #endregion

        #region 获取指定id 的优惠券是否可以领取
        /// <summary>
        /// 获取指定id 的优惠券是否可以领取
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ReceiveCoupon(int promotionId)
        {
            var jm = new WebApiCallBack();

            var where = PredicateBuilder.True<CoreCmsPromotion>();
            where = where.And(p => p.endTime > DateTime.Now); //判断优惠券失效时间 是否可领取
            where = where.And(p => p.isEnable == true); //启用状态
            where = where.And(p => p.type == (int)GlobalEnumVars.PromotionType.Coupon);  //促销 类型
            where = where.And(p => p.isAutoReceive == true);  //自动领取状态
            where = where.And(p => p.id == promotionId);
            where = where.And(p => p.isDel == false);  //是否被删除


            var info = await _dal.QueryByClauseAsync(where,false,true);
            if (info != null)
            {
                jm.data = info;
                //判断最大领取数量
                if (info.maxRecevieNums == 0)
                {
                    jm.status = false;
                    return jm;
                }
                
                var receiveCount = await _couponServices.GetCountAsync(p => p.promotionId == promotionId);
                if (receiveCount >= info.maxRecevieNums)
                {
                    jm.status = false;
                    jm.msg = "该优惠券已被领完，请下次再来！";
                    return jm;
                }
                else
                {
                    jm.status = true;
                    jm.code = receiveCount;
                }

            }
            else
            {
                jm.msg = GlobalErrorCodeVars.Code15007;
            }
            return jm;
        }

        #endregion
    }
}
