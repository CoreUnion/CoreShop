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
using SqlSugar;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     促销表 工厂接口
    /// </summary>
    public interface ICoreCmsPromotionRepository : IBaseRepository<CoreCmsPromotion>
    {
        //判断商品是否参加团购
        /// <summary>
        ///     判断商品是否参加团购
        /// </summary>
        /// <param name="goodId">商品序列</param>
        /// <param name="promotionId">关联促销信息</param>
        /// <returns></returns>
        public bool IsInGroup(int goodId, out int promotionId);


        /// <summary>
        ///     查询查了并获取导航下级数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="isToPage">是否分页</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        Task<IPageList<CoreCmsPromotion>> QueryPageAndChildsAsync(
            Expression<Func<CoreCmsPromotion, bool>> predicate,
            Expression<Func<CoreCmsPromotion, object>> orderByExpression, OrderByType orderByType,
            bool isToPage = false, int pageIndex = 1,
            int pageSize = 20);
    }
}