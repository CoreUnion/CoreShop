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
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 商品浏览记录表 接口实现
    /// </summary>
    public class CoreCmsGoodsBrowsingServices : BaseServices<CoreCmsGoodsBrowsing>, ICoreCmsGoodsBrowsingServices
    {
        private readonly ICoreCmsGoodsBrowsingRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsGoodsBrowsingServices(IUnitOfWork unitOfWork, ICoreCmsGoodsBrowsingRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsGoodsBrowsing>> QueryPageAsync(Expression<Func<CoreCmsGoodsBrowsing, bool>> predicate,
            Expression<Func<CoreCmsGoodsBrowsing, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize);
        }

    }
}
