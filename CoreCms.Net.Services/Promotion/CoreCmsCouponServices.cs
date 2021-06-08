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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 优惠券表 接口实现
    /// </summary>
    public class CoreCmsCouponServices : BaseServices<CoreCmsCoupon>, ICoreCmsCouponServices
    {
        private readonly ICoreCmsCouponRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;

        public CoreCmsCouponServices(IUnitOfWork unitOfWork, ICoreCmsCouponRepository dal, IServiceProvider serviceProvider)
        {
            this._dal = dal;
            _serviceProvider = serviceProvider;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 根据优惠券编码取优惠券的信息,并判断是否可用
        /// </summary>
        /// <param name="code"></param>
        /// <param name="check"></param>
        public async Task<WebApiCallBack> CodeToInfo(string[] code, bool check = false)
        {
            return await _dal.ToInfo(code, check);
        }

        /// <summary>
        /// 删除核销多个优惠券
        /// </summary>
        /// <param name="couponCode">优惠券码</param>
        /// <param name="orderId">使用序列</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> UsedMultipleCoupon(string[] couponCode, string orderId)
        {
            var res = new WebApiCallBack() { methodDescription = "删除核销多个优惠券" };
            //判断优惠券码能否有效
            var resCodeToInfo = await CodeToInfo(couponCode, true);
            if (resCodeToInfo.status == false)
            {
                return resCodeToInfo;
            }
            var dt = DateTime.Now;
            var doHasChange = await _dal.UpdateAsync(p => new CoreCmsCoupon() { isUsed = true, usedId = orderId, updateTime = dt },
                p => p.isUsed == false && couponCode.Contains(p.couponCode));
            if (doHasChange)
            {
                res.status = true;
                res.msg = "核销使用优惠券成功";
                res.data = couponCode;
            }
            else
            {
                res.status = false;
                res.msg = "核销使用优惠券失败";
                res.data = couponCode;
            }
            return res;
        }

        /// <summary>
        /// 获取 我的优惠券
        /// </summary>
        /// <param name="userId">用户序列</param>
        /// <param name="promotionId">促销序列</param>
        /// <param name="display">优惠券状态编码</param>
        /// <param name="page">页码</param>
        /// <param name="limit">数量</param>
        public async Task<WebApiCallBack> GetMyCoupon(int userId, int promotionId = 0, string display = "all", int page = 1, int limit = 10)
        {

            return await _dal.GetMyCoupon(userId, promotionId, display, page, limit);
        }

        /// <summary>
        /// 用户领取优惠券 插入数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="promotionId"></param>
        /// <param name="promotion"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> AddData(int userId, int promotionId, CoreCmsPromotion promotion)
        {
            var jm = new WebApiCallBack();

            var dtTime = DateTime.Now;
            var eTime = DateTime.Now;

            if (promotion.effectiveDays > 0)
            {
                eTime = eTime.AddDays(promotion.effectiveDays);
            }
            if (promotion.effectiveHours > 0)
            {
                eTime = eTime.AddHours(promotion.effectiveHours);
            }
            var coupon = new CoreCmsCoupon
            {
                couponCode = GeneratePromotionCode()[0],
                promotionId = promotion.id,
                isUsed = false,
                userId = userId,
                createTime = dtTime,
                startTime = dtTime,
                endTime = eTime,
                remark = "接口领取"
            };

            var bl = await _dal.InsertAsync(coupon) > 0;
            jm.status = bl;
            jm.msg = bl ? "领取成功" : "领取失败";

            return jm;
        }


        /// <summary>
        /// 通过优惠券号领取优惠券
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ReceiveCoupon(int userId, string couponCode)
        {
            using var container = _serviceProvider.CreateScope();
            var promotionServices = container.ServiceProvider.GetService<ICoreCmsPromotionServices>();

            var jm = new WebApiCallBack();

            var coupon = await _dal.QueryByClauseAsync(p => p.couponCode == couponCode);
            if (coupon == null)
            {
                jm.msg = "该优惠券不存在";
                return jm;
            }
            if (!string.IsNullOrEmpty(coupon.usedId))
            {
                jm.msg = "该优惠券已被使用";
                return jm;
            }
            if (coupon.userId > 0)
            {
                jm.msg = "该优惠券已被其他人领取";
                return jm;
            }

            coupon.userId = userId;
            var bl = await _dal.UpdateAsync(p => new CoreCmsCoupon() { userId = userId }, p => p.id == coupon.id);
            if (bl)
            {
                var promotion = await promotionServices.QueryByIdAsync(coupon.promotionId);
                if (promotion != null)
                {
                    coupon.couponName = promotion.name;
                }
            }
            jm.status = bl;
            jm.msg = bl ? "领取成功" : "领取失败";
            jm.data = coupon;

            return jm;
        }


        /// <summary>
        /// 生成优惠券code 方法
        /// </summary>
        /// <param name="noOfCodes">定义一个int类型的参数 用来确定生成多少个优惠码</param>
        /// <param name="excludeCodesArray">定义一个exclude_codes_array类型的数组</param>
        /// <param name="codeLength">定义一个code_length的参数来确定优惠码的长度</param>
        /// <returns></returns>
        public List<string> GeneratePromotionCode(int noOfCodes = 1, List<string> excludeCodesArray = null, int codeLength = 10)
        {
            char[] constant =
            {
                '0','1','2','3','4','5','6','7','8','9',
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
                'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
            };
            var promotionCodes = new List<string>();  //这个数组用来接收生成的优惠码
            Random rd = new Random();
            for (int i = 0; i < noOfCodes; i++)
            {
                var code = "";
                for (int j = 0; j < codeLength; j++)
                {
                    code += constant[rd.Next(62)];
                }
                //如果生成的6位随机数不再我们定义的$promotion_codes函数里面
                if (!promotionCodes.Contains(code))
                {
                    if (excludeCodesArray != null && excludeCodesArray.Any())
                    {
                        if (!excludeCodesArray.Contains(code))
                        {
                            promotionCodes.Add(code);//将优惠码赋值给数组
                        }
                        else
                        {
                            i--;
                        }
                    }
                    else
                    {
                        promotionCodes.Add(code);//将优惠码赋值给数组
                    }
                }
                else
                {
                    i--;
                }
            }
            return promotionCodes;
        }


        #region 根据条件查询分页数据及导航数据

        /// <summary>
        ///     根据条件查询分页数据及导航数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="isToPage">是否分页</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsCoupon>> QueryPageMapperAsync(Expression<Func<CoreCmsCoupon, bool>> predicate,
            Expression<Func<CoreCmsCoupon, object>> orderByExpression, OrderByType orderByType, bool isToPage = false, int pageIndex = 1,
            int pageSize = 20)
        {

            return await _dal.QueryPageMapperAsync(predicate, orderByExpression, orderByType, isToPage, pageIndex, pageSize);
        }

        #endregion


        /// <summary>
        ///     重写数据并获取相关
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <returns></returns>
        public async Task<List<CoreCmsCoupon>> QueryWithAboutAsync(Expression<Func<CoreCmsCoupon, bool>> predicate)
        {
            return await _dal.QueryWithAboutAsync(predicate);
        }


        #region 重写根据条件查询分页数据
        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<IPageList<CoreCmsCoupon>> QueryPageAsync(Expression<Func<CoreCmsCoupon, bool>> predicate,
            Expression<Func<CoreCmsCoupon, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion

        /// <summary>
        /// 获取 我的优惠券可用数量
        /// </summary>
        /// <param name="userId">用户序列</param>
        public async Task<int> GetMyCouponCount(int userId)
        {
            return await _dal.GetMyCouponCount(userId);
        }



        #region 优惠券返还
        /// <summary>
        ///  优惠券返还
        /// </summary>
        /// <param name="couponCodes">优惠券数组</param>
        public async Task<WebApiCallBack> CancelReturnCoupon(string couponCodes)
        {
            var jm = new WebApiCallBack();
            jm.code = 0;
            var bl = false;
            var ids = couponCodes.Split(",");
            var list = await _dal.QueryListByClauseAsync(p => ids.Contains(p.couponCode));
            if (list != null && list.Any())
            {
                var newList = new List<CoreCmsCoupon>();
                list.ForEach(p =>
                {
                    var eTime = p.endTime.AddMinutes(1);
                    newList.Add(new CoreCmsCoupon()
                    {
                        couponCode = GeneratePromotionCode()[0],
                        promotionId = p.promotionId,
                        isUsed = false,
                        userId = p.userId,
                        createTime = DateTime.Now,
                        remark = "优惠券返还",
                        startTime = p.startTime,
                        endTime = eTime
                    });
                });
                bl = await _dal.InsertAsync(newList) > 0;
                jm.status = bl;
                jm.msg = bl ? "返还成功" : "返还失败";
            }
            return jm;
        }
        #endregion


    }
}
