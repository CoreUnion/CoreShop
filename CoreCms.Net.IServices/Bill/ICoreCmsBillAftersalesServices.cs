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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using Newtonsoft.Json.Linq;
using SqlSugar;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     退货单表 服务工厂接口
    /// </summary>
    public interface ICoreCmsBillAftersalesServices : IBaseServices<CoreCmsBillAftersales>
    {
        /// <summary>
        ///     根据订单号查询已经售后的内容
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="aftersaleLevel">取售后单的时候，售后单的等级，0：待审核的和审核通过的售后单，1未审核的，2审核通过的</param>
        /// <returns></returns>
        WebApiCallBack OrderToAftersales(string orderId, int aftersaleLevel = 0);


        /// <summary>
        ///     统计用户的售后数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<int> GetUserAfterSalesNum(int userId, int status);


        /// <summary>
        ///     创建售后单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId">发起售后的订单</param>
        /// <param name="type">是否收到退货，1未收到退货，不会创建退货单，2收到退货，会创建退货单,只有未发货的商品才能选择未收到货，只有已发货的才能选择已收到货</param>
        /// <param name="items">如果是退款退货，退货的明细 以 [[order_item_id=>nums]]的二维数组形式传值</param>
        /// <param name="images"></param>
        /// <param name="reason">售后理由</param>
        /// <param name="refund">退款金额，只在退款退货的时候用，如果是退款，直接就是订单金额</param>
        /// <returns></returns>
        Task<WebApiCallBack> ToAdd(int userId, string orderId, int type, JArray items, string[] images, string reason,
            decimal refund);

        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        Task<IPageList<CoreCmsBillAftersales>> QueryPageAsync(Expression<Func<CoreCmsBillAftersales, bool>> predicate,
            Expression<Func<CoreCmsBillAftersales, object>> orderByExpression, OrderByType orderByType,
            int pageIndex = 1,
            int pageSize = 20);

        /// <summary>
        ///     获取单个数据
        /// </summary>
        /// <param name="reshipId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CoreCmsBillAftersales> GetInfo(string aftersalesId, int userId);


        /// <summary>
        ///     后端进行审核的时候，前置操作，1取出页面的数据，2在提交过来的表单的时候，进行校验
        /// </summary>
        /// <param name="aftersalesId"></param>
        /// <returns></returns>
        Task<WebApiCallBack> PreAudit(string aftersalesId);


        /// <summary>
        ///     Audit平台审核通过或者审核不通过
        ///     如果审核通过了，是退款单的话，自动生成退款单，并做订单完成状态，如果是退货的话，自动生成退款单和退货单，如果
        /// </summary>
        /// <param name="aftersalesId"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="refund"></param>
        /// <param name="mark"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<WebApiCallBack> Audit(string aftersalesId, int status, int type, decimal refund, string mark,
            JArray items);
    }
}