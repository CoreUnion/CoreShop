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
using System.Threading.Tasks;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.Echarts;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     报表 服务工厂接口
    /// </summary>
    public interface ICoreCmsReportsServices : IBaseServices<GetOrdersReportsDbSelectOut>
    {
        /// <summary>
        ///     订单报表
        /// </summary>
        /// <param name="num">数量</param>
        /// <param name="where">查询条件</param>
        /// <param name="section">查询值</param>
        /// <param name="sTime">开始时间</param>
        /// <param name="joinVal">筛选字段createTime/paymentTime </param>
        /// <returns></returns>
        List<GetOrdersReportsDbSelectOut> GetOrderMark(int num, string where, int section, DateTime sTime,
            string joinVal);

        /// <summary>
        ///     支付单报表
        /// </summary>
        /// <param name="num">数量</param>
        /// <param name="where">查询条件</param>
        /// <param name="section">查询值</param>
        /// <param name="sTime">开始时间</param>
        /// <param name="joinVal">筛选字段createTime/paymentTime </param>
        /// <returns></returns>
        List<GetOrdersReportsDbSelectOut> GetPaymentsMark(int num, string where, int section, DateTime sTime,
            string joinVal);

        /// <summary>
        ///     退款单报表
        /// </summary>
        /// <param name="num">数量</param>
        /// <param name="where">查询条件</param>
        /// <param name="section">查询值</param>
        /// <param name="sTime">开始时间</param>
        /// <param name="joinVal">筛选字段createTime/paymentTime </param>
        /// <returns></returns>
        List<GetOrdersReportsDbSelectOut> GetRefundMark(int num, string where, int section, DateTime sTime,
            string joinVal);

        /// <summary>
        ///     用户提现报表
        /// </summary>
        /// <param name="num">数量</param>
        /// <param name="where">查询条件</param>
        /// <param name="section">查询值</param>
        /// <param name="sTime">开始时间</param>
        /// <param name="joinVal">筛选字段createTime/paymentTime </param>
        /// <returns></returns>
        List<GetOrdersReportsDbSelectOut> GetTocashMark(int num, string where, int section, DateTime sTime,
            string joinVal);

        /// <summary>
        ///     获取订单销量查询返回结果
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="filter"></param>
        /// <param name="filterSed"></param>
        /// <param name="thesort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IPageList<GoodsSalesVolume>> GetGoodsSalesVolumes(string start, string end, string filter,
            string filterSed,
            string thesort, int pageIndex = 1, int pageSize = 5000);

        /// <summary>
        ///     获取商品收藏查询返回结果
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="thesort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IPageList<GoodsCollection>> GetGoodsCollections(string start, string end, string thesort,
            int pageIndex = 1, int pageSize = 5000);
    }
}