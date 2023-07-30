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
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 优惠券接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CouponController : ControllerBase
    {

        private readonly IHttpContextUser _user;
        private readonly ICoreCmsCouponServices _couponServices;
        private readonly ICoreCmsPromotionServices _promotionServices;
        private readonly IUnitOfWork _unionOfWork;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="user"></param>
        /// <param name="couponServices"></param>
        /// <param name="promotionServices"></param>
        /// <param name="unionOfWork"></param>
        public CouponController(IHttpContextUser user
            , ICoreCmsCouponServices couponServices, ICoreCmsPromotionServices promotionServices, IUnitOfWork unionOfWork)
        {
            _user = user;
            _couponServices = couponServices;
            _promotionServices = promotionServices;
            _unionOfWork = unionOfWork;
        }

        //公共接口====================================================================================================

        #region 获取 可领取的优惠券==================================================
        /// <summary>
        /// 获取 可领取的优惠券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[Authorize]
        public async Task<WebApiCallBack> CouponList([FromBody] FMCouponForUserCouponPost entity)
        {
            var jm = new WebApiCallBack() { msg = "获取失败" };

            var list = await _promotionServices.GetReceiveCouponList(entity.page, entity.limit);
            jm.status = true;
            jm.data = list;
            jm.msg = "获取成功";
            jm.otherData = new
            {
                totalCount = 0,
                totalPages = 0,
            };
            if (list != null && list.Any())
            {
                jm.data = list;
                jm.otherData = new
                {
                    list.TotalCount,
                    list.TotalPages
                };
            }
            return jm;
        }
        #endregion

        //验证接口====================================================================================================

        #region 获取优惠券 详情==================================================
        /// <summary>
        /// 获取优惠券 详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> CouponDetail([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack() { msg = "获取失败" };

            if (entity.id == 0)
            {
                jm.status = false;
                jm.msg = GlobalErrorCodeVars.Code15006;
                return jm;
            }

            var promotionModel = await _promotionServices.QueryByClauseAsync(p => p.id == entity.id);
            if (promotionModel != null)
            {
                jm.status = true;
                jm.data = promotionModel;
                jm.msg = "获取成功";
            }
            return jm;

        }
        #endregion

        #region 获取用户已领取的优惠券==================================================
        /// <summary>
        /// 获取用户已领取的优惠券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> UserCoupon([FromBody] FMCouponForUserCouponPost entity)
        {
            var jm = await _couponServices.GetMyCoupon(_user.ID, 0, entity.display, entity.page, entity.limit);
            return jm;
        }
        #endregion

        #region 用户领取优惠券==================================================
        /// <summary>
        /// 用户领取优惠券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetCoupon([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            if (entity.id == 0)
            {
                jm.msg = GlobalErrorCodeVars.Code15006;
                return jm;
            }
            
            try
            {
                _unionOfWork.BeginTran();


                //判断优惠券是否可以领取?
                var promotionModel = await _promotionServices.ReceiveCoupon(entity.id);
                if (promotionModel.status == false)
                {
                    _unionOfWork.RollbackTran();
                    return promotionModel;
                }

                
                
                var promotion = (CoreCmsPromotion)promotionModel.data;
                if (promotion == null)
                {
                    _unionOfWork.RollbackTran();
                    jm.msg = GlobalErrorCodeVars.Code15019;
                    return jm;
                }

                if (promotion.maxNums > 0)
                {
                    //判断用户是否已领取?领取次数
                    var couponResult = await _couponServices.GetMyCoupon(_user.ID, entity.id, "all", 1, 9999);
                    if (couponResult.status && couponResult.code >= promotion.maxNums)
                    {
                        _unionOfWork.RollbackTran();
                        jm.msg = GlobalErrorCodeVars.Code15018;
                        return jm;
                    }
                }
                
                jm = await _couponServices.AddData(_user.ID, entity.id, promotion);

                _unionOfWork.CommitTran();

                jm.otherData = promotionModel;

            }
            catch (Exception e)
            {
                _unionOfWork.RollbackTran();
                jm.msg = GlobalErrorCodeVars.Code10000;
            }
            
            return jm;
        }
        #endregion

        #region 用户输入code领取优惠券==================================================
        /// <summary>
        /// 用户输入code领取优惠券
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetCouponKey([FromBody] FMCouponForGetCouponKeyPost entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.key))
            {
                jm.msg = GlobalErrorCodeVars.Code15006;
                return jm;
            }

            var coupon = await _couponServices.QueryByClauseAsync(p => p.couponCode == entity.key);
            if (coupon == null || coupon.promotionId <= 0)
            {
                jm.msg = GlobalErrorCodeVars.Code15009;
                return jm;
            }

            //判断优惠券是否可以领取?
            var promotionModel = await _promotionServices.ReceiveCoupon(coupon.promotionId);
            if (promotionModel.status == false)
            {
                return promotionModel;
            }
            //判断用户是否已领取?
            if (promotionModel.data is CoreCmsPromotion { maxNums: > 0 } info)
            {
                //判断用户是否已领取?领取次数
                var couponResult = await _couponServices.GetMyCoupon(_user.ID, coupon.promotionId, "all", 1, 9999);
                if (couponResult.status && couponResult.code > info.maxNums)
                {
                    jm.msg = GlobalErrorCodeVars.Code15018;
                    return jm;
                }
            }
            //
            jm = await _couponServices.ReceiveCoupon(_user.ID, entity.key);

            return jm;
        }
        #endregion

    }
}