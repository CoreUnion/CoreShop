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
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Helper;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 优惠券码表 接口实现
    /// </summary>
    public class CoreCmsCouponRepository : BaseRepository<CoreCmsCoupon>, ICoreCmsCouponRepository
    {


        public CoreCmsCouponRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        #region 根据优惠券编码取优惠券的信息,并判断是否可用

        /// <summary>
        /// 根据优惠券编码取优惠券的信息,并判断是否可用
        /// </summary>
        /// <param name="code"></param>
        /// <param name="check"></param>
        public async Task<WebApiCallBack> ToInfo(string[] code, bool check = false)
        {
            var jm = new WebApiCallBack();
            var outData = new List<CoreCmsPromotion>();

            foreach (var codeStr in code)
            {
                var model =await DbClient.Queryable<CoreCmsCoupon, CoreCmsPromotion>((coupon, prommotion) => new object[]
                    {
                        JoinType.Inner, coupon.promotionId == prommotion.id
                    }).Where((coupon, prommotion) => coupon.couponCode == codeStr)
                    .Select((coupon, prommotion) => new
                    {
                        coupon,
                        prommotion
                    }).FirstAsync();
                ;
                if (model != null)
                {
                    if (check)
                    {
                        //判断规则是否开启
                        if (model.prommotion.isEnable == false)
                        {
                            jm.data = 15012;
                            jm.msg = GlobalErrorCodeVars.Code15012;
                            return jm;
                        }
                        //判断优惠券规则是否到达开始时间
                        var dt = DateTime.Now;
                        if (model.coupon.startTime > dt)
                        {
                            jm.data = 15010;
                            jm.msg = GlobalErrorCodeVars.Code15010;
                            return jm;
                        }
                        //判断优惠券规则是否已经到结束时间了，也就是是否过期了
                        if (model.coupon.endTime < dt)
                        {
                            jm.data = 15011;
                            jm.msg = GlobalErrorCodeVars.Code15011;
                            return jm;
                        }
                        //判断是否已经使用过了
                        if (model.coupon.isUsed)
                        {
                            jm.data = 15013;
                            jm.msg = GlobalErrorCodeVars.Code15013;
                            return jm;
                        }
                        //判断此类优惠券是否已经使用过,防止一类优惠券使用多张
                        if (outData.Exists(p => p.id == model.coupon.promotionId))
                        {
                            jm.data = 15015;
                            jm.msg = GlobalErrorCodeVars.Code15015;
                            return jm;
                        }
                    }
                    outData.Add(model.prommotion);
                }
                else
                {
                    jm.data = 15009;
                    jm.msg = GlobalErrorCodeVars.Code15009;
                    return jm;
                }
            }
            jm.status = true;
            jm.data = outData;
            return jm;
        }


        #endregion

        #region 获取 我的优惠券
        /// <summary>
        /// 获取 我的优惠券
        /// </summary>
        /// <param name="userId">用户序列</param>
        /// <param name="promotionId">促销序列</param>
        /// <param name="display">优惠券状态编码</param>
        /// <param name="page">页码</param>
        /// <param name="limit">数量</param>
        public async Task<WebApiCallBack> GetMyCoupon(int userId, int promotionId = 0, string display = "", int page = 1, int limit = 10)
        {
            var jm = new WebApiCallBack();
            jm.code = 0;

            RefAsync<int> totalCount = 0;
            var dt = DateTime.Now;
            var listData = await DbClient.Queryable<CoreCmsCoupon, CoreCmsPromotion>((coupon, promotion) => new object[]
                 {
                        JoinType.Inner, coupon.promotionId == promotion.id
                 })
                .Where((coupon, promotion) => coupon.userId == userId)
                .Where((coupon, promotion) => promotion.isDel == false)
                .Where((coupon, promotion) => promotion.type == (int)GlobalEnumVars.PromotionType.Coupon)
                .WhereIF(display == GlobalEnumVars.CouponIsUsedStatusText.noUsed.ToString(), (coupon, promotion) => coupon.isUsed == false && coupon.endTime >= dt)
                .WhereIF(display == GlobalEnumVars.CouponIsUsedStatusText.yesUsed.ToString(), (coupon, promotion) => coupon.isUsed == true)
                .WhereIF(display == GlobalEnumVars.CouponIsUsedStatusText.invalid.ToString(), (coupon, promotion) => coupon.endTime < dt)
                .WhereIF(promotionId > 0, (coupon, promotion) => coupon.promotionId == promotionId)
                .WhereIF(promotionId == 0, (coupon, promotion) => promotion.isEnable == true)
                .Select((coupon, promotion) => new CoreCmsCoupon
                {
                    couponCode = coupon.couponCode,
                    promotionId = coupon.promotionId,
                    isUsed = coupon.isUsed,
                    userId = coupon.userId,
                    usedId = coupon.usedId,
                    createTime = coupon.createTime,
                    updateTime = coupon.updateTime,
                    couponName = promotion.name,
                    startTime = coupon.startTime,
                    endTime = coupon.endTime,

                }).MergeTable()
                .Mapper(p => p.conditions, p => p.promotionId)
                .Mapper(p => p.results, p => p.promotionId)
                .OrderBy(p => p.createTime, OrderByType.Desc)
                .With(SqlWith.Null)
                .ToPageListAsync(page, limit, totalCount);

            var totalPages = totalCount.Value / limit;
            if (totalPages == 0) { totalPages++; }
            if (totalPages % limit > 0)
                totalPages++;

            var resutlList = new List<GetMyCouponResultDto>();
            if (listData != null && listData.Any())
            {
                foreach (var item in listData)
                {
                    //var pcondition = await DbClient.Queryable<CoreCmsPromotionCondition>().Where(p => p.promotionId == item.promotionId).ToListAsync();
                    //var presult = await DbClient.Queryable<CoreCmsPromotionResult>().Where(p => p.promotionId == item.promotionId).ToListAsync();
                    var expression1 = string.Empty;
                    var expression2 = string.Empty;

                    var dto = new GetMyCouponResultDto();

                    foreach (var condition in item.conditions)
                    {
                        var str = PromotionHelper.GetConditionMsg(condition.code, condition.parameters);
                        expression1 += str;
                        dto.conditions.Add(str);
                    }
                    foreach (var result in item.results)
                    {
                        var str = PromotionHelper.GetResultMsg(result.code, result.parameters);
                        expression2 += str;
                        dto.results.Add(str);
                    }

                    dto.couponCode = item.couponCode;
                    dto.promotionId = item.promotionId;
                    dto.isUsed = item.isUsed;
                    dto.userId = item.userId;
                    dto.usedId = item.usedId;
                    dto.createTime = item.createTime;
                    dto.updateTime = item.updateTime;
                    dto.couponName = item.couponName;

                    dto.expression1 = expression1;
                    dto.expression2 = expression2;

                    dto.isExpire = dt > item.endTime;
                    dto.startTime = item.startTime;
                    dto.endTime = item.endTime;
                    dto.stime = item.startTime.ToString("yyyy-MM-dd");
                    dto.etime = item.endTime.ToString("yyyy-MM-dd");

                    resutlList.Add(dto);
                }
            }
            jm.status = true;
            jm.msg = "获取成功";
            jm.data = new
            {
                list = resutlList,
                count = totalCount.Value,
                display,
                page = totalPages
            };
            jm.code = totalCount.Value;

            return jm;
        }
        #endregion

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
            RefAsync<int> totalCount = 0;
            List<CoreCmsCoupon> page;
            if (isToPage)
            {
                page = await DbClient.Queryable<CoreCmsCoupon>()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate).Select(p => new CoreCmsCoupon
                    {
                        id = p.id,
                        couponCode = p.couponCode,
                        promotionId = p.promotionId,
                        isUsed = p.isUsed,
                        userId = p.userId,
                        usedId = p.usedId,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        startTime = p.startTime,
                        endTime = p.endTime,
                    })
                    .Mapper(p => p.promotion, p => p.promotionId)
                    .Mapper(p => p.conditions, p => p.conditions.First().promotionId)
                    .Mapper(p => p.results, p => p.results.First().promotionId)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsCoupon>()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .Take(pageSize)
                    .Select(p => new CoreCmsCoupon
                    {
                        id = p.id,
                        couponCode = p.couponCode,
                        promotionId = p.promotionId,
                        isUsed = p.isUsed,
                        userId = p.userId,
                        usedId = p.usedId,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        startTime = p.startTime,
                        endTime = p.endTime,
                    })
                    .Mapper(p => p.promotion, p => p.promotionId)
                    .Mapper(p => p.conditions, p => p.conditions.First().promotionId)
                    .Mapper(p => p.results, p => p.results.First().promotionId)
                    .ToListAsync();
            }
            var list = new PageList<CoreCmsCoupon>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

        #region 重写数据并获取相关
        /// <summary>
        ///     重写数据并获取相关
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <returns></returns>
        public async Task<List<CoreCmsCoupon>> QueryWithAboutAsync(Expression<Func<CoreCmsCoupon, bool>> predicate)
        {
            var page = await DbClient.Queryable<CoreCmsCoupon, CoreCmsUser, CoreCmsPromotion>((p, sUser, sPromotion) => new JoinQueryInfos(
                    JoinType.Left, p.userId == sUser.id,
                    JoinType.Left, p.promotionId == sPromotion.id))
                .Select((p, sUser, sPromotion) => new CoreCmsCoupon
                {
                    id = p.id,
                    couponCode = p.couponCode,
                    promotionId = p.promotionId,
                    isUsed = p.isUsed,
                    userId = p.userId,
                    usedId = p.usedId,
                    createTime = p.createTime,
                    updateTime = p.updateTime,
                    userNickName = sUser.nickName,
                    couponName = sPromotion.name,
                    startTime = p.startTime,
                    endTime = p.endTime,
                })
                .MergeTable()
                .WhereIF(predicate != null, predicate)
                .ToListAsync();
            return page;
        }

        #endregion

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
            RefAsync<int> totalCount = 0;
            var page = blUseNoLock
                ? await DbClient.Queryable<CoreCmsCoupon, CoreCmsUser, CoreCmsPromotion>((p, sUser, sPromotion) => new JoinQueryInfos(
                         JoinType.Left, p.userId == sUser.id,
                         JoinType.Left, p.promotionId == sPromotion.id))
                .Select((p, sUser, sPromotion) => new CoreCmsCoupon
                {
                    id = p.id,
                    couponCode = p.couponCode,
                    promotionId = p.promotionId,
                    isUsed = p.isUsed,
                    userId = p.userId,
                    usedId = p.usedId,
                    createTime = p.createTime,
                    updateTime = p.updateTime,
                    userNickName = sUser.nickName,
                    couponName = sPromotion.name,
                    startTime = p.startTime,
                    endTime = p.endTime,
                })
                .MergeTable()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount)
                :
                await DbClient.Queryable<CoreCmsCoupon, CoreCmsUser, CoreCmsPromotion>((p, sUser, sPromotion) => new JoinQueryInfos(
                        JoinType.Left, p.userId == sUser.id,
                        JoinType.Left, p.promotionId == sPromotion.id))
                    .Select((p, sUser, sPromotion) => new CoreCmsCoupon
                    {
                        id = p.id,
                        couponCode = p.couponCode,
                        promotionId = p.promotionId,
                        isUsed = p.isUsed,
                        userId = p.userId,
                        usedId = p.usedId,
                        createTime = p.createTime,
                        updateTime = p.updateTime,
                        userNickName = sUser.nickName,
                        couponName = sPromotion.name,
                        startTime = p.startTime,
                        endTime = p.endTime,
                    })
                .MergeTable()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            var list = new PageList<CoreCmsCoupon>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

        #region 获取 我的优惠券可用数量
        /// <summary>
        /// 获取 我的优惠券可用数量
        /// </summary>
        /// <param name="userId">用户序列</param>
        public async Task<int> GetMyCouponCount(int userId)
        {
            var jm = new WebApiCallBack();
            jm.code = 0;

            var dt = DateTime.Now;
            var count = await DbClient.Queryable<CoreCmsCoupon, CoreCmsPromotion>((coupon, promotion) => new object[]
                 {
                        JoinType.Inner, coupon.promotionId == promotion.id
                 })
                .Where((coupon, promotion) => coupon.userId == userId)
                .Where((coupon, promotion) => promotion.isDel == false)
                .Where((coupon, promotion) => coupon.isUsed == false)
                .Where((coupon, promotion) => promotion.type == (int)GlobalEnumVars.PromotionType.Coupon)
                .Where((coupon, promotion) => coupon.endTime > dt)
                .CountAsync();
            return count;
        }
        #endregion

    }
}
