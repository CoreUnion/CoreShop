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
using CoreCms.Net.Model.ViewModels.DTO;
using SqlSugar;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     拼团规则表 服务工厂接口
    /// </summary>
    public interface ICoreCmsPinTuanRuleServices : IBaseServices<CoreCmsPinTuanRule>
    {
        /// <summary>
        ///     取购物车数据的时候，更新价格
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        WebApiCallBack PinTuanInfo(List<CartProducts> list);


        /// <summary>
        ///     接口上获取拼团所有商品
        /// </summary>
        /// <returns></returns>
        Task<WebApiCallBack> GetPinTuanList(int id = 0, int userId = 0);


        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        Task<IPageList<TagPinTuanResult>> QueryTagPinTuanPageAsync(Expression<Func<TagPinTuanResult, bool>> predicate,
            Expression<Func<TagPinTuanResult, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20);


        /// <summary>
        ///     根据商品id获取拼团规则信息
        /// </summary>
        /// <param name="goodId"></param>
        /// <returns></returns>
        Task<TagPinTuanResult> GetPinTuanInfo(int goodId);
    }
}