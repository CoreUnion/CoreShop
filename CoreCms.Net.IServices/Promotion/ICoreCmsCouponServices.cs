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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     优惠券表 服务工厂接口
    /// </summary>
    public interface ICoreCmsCouponServices : IBaseServices<CoreCmsCoupon>
    {
        /// <summary>
        ///     根据优惠券编码取优惠券的信息,并判断是否可用
        /// </summary>
        /// <param name="code"></param>
        /// <param name="check"></param>
        Task<WebApiCallBack> CodeToInfo(string[] code, bool check = false);


        /// <summary>
        ///     删除核销多个优惠券
        /// </summary>
        /// <param name="couponCode">优惠券码</param>
        /// <param name="orderId">使用序列</param>
        /// <returns></returns>
        Task<WebApiCallBack> UsedMultipleCoupon(string[] couponCode, string orderId);


        /// <summary>
        ///     获取 我的优惠券
        /// </summary>
        /// <param name="userId">用户序列</param>
        /// <param name="promotionId">促销序列</param>
        /// <param name="display">优惠券状态编码</param>
        /// <param name="page">页码</param>
        /// <param name="limit">数量</param>
        /// <returns></returns>
        Task<WebApiCallBack> GetMyCoupon(int userId, int promotionId = 0, string display = "all", int page = 1,
            int limit = 10);


        /// <summary>
        ///     用户领取优惠券 插入数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="promotionId"></param>
        /// <param name="promotion"></param>
        /// <returns></returns>
        Task<WebApiCallBack> AddData(int userId, int promotionId, CoreCmsPromotion promotion);

        /// <summary>
        ///     通过优惠券号领取优惠券
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        Task<WebApiCallBack> ReceiveCoupon(int userId, string couponCode);

        /// <summary>
        ///     生成优惠券code 方法
        /// </summary>
        /// <param name="noOfCodes">定义一个int类型的参数 用来确定生成多少个优惠码</param>
        /// <param name="excludeCodesArray">定义一个exclude_codes_array类型的数组</param>
        /// <param name="codeLength">定义一个code_length的参数来确定优惠码的长度</param>
        /// <returns></returns>
        List<string> GeneratePromotionCode(int noOfCodes = 1, List<string> excludeCodesArray = null,
            int codeLength = 10);


        /// <summary>
        ///     根据条件查询分页数据及导航数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="isToPage">是否分页</param>
        /// <returns></returns>
        Task<IPageList<CoreCmsCoupon>> QueryPageMapperAsync(
            Expression<Func<CoreCmsCoupon, bool>> predicate,
            Expression<Func<CoreCmsCoupon, object>> orderByExpression, OrderByType orderByType, bool isToPage = false,
            int pageIndex = 1,
            int pageSize = 20);


        /// <summary>
        ///     重写数据并获取相关
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        Task<List<CoreCmsCoupon>> QueryWithAboutAsync(Expression<Func<CoreCmsCoupon, bool>> predicate);


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
        new Task<IPageList<CoreCmsCoupon>> QueryPageAsync(
            Expression<Func<CoreCmsCoupon, bool>> predicate,
            Expression<Func<CoreCmsCoupon, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false);

        #endregion


        /// <summary>
        ///     获取 我的优惠券可用数量
        /// </summary>
        /// <param name="userId">用户序列</param>
        Task<int> GetMyCouponCount(int userId);


        #region 优惠券返还

        /// <summary>
        ///     优惠券返还
        /// </summary>
        /// <param name="couponCodes">优惠券数组</param>
        Task<WebApiCallBack> CancelReturnCoupon(string couponCodes);

        #endregion
    }
}