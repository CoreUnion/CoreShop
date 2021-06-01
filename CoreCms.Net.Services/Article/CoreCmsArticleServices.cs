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
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using SqlSugar;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     文章表 接口实现
    /// </summary>
    public class CoreCmsArticleServices : BaseServices<CoreCmsArticle>, ICoreCmsArticleServices
    {
        private readonly ICoreCmsArticleRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        public CoreCmsArticleServices(IUnitOfWork unitOfWork, ICoreCmsArticleRepository dal)
        {
            _dal = dal;
            BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        ///     获取指定id 的文章详情
        /// </summary>
        /// <param name="id">序列</param>
        public async Task<CoreCmsArticle> ArticleDetail(int id)
        {
            return await _dal.ArticleDetail(id);
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
        public async Task<IPageList<CoreCmsArticle>> QueryPageAsync(Expression<Func<CoreCmsArticle, bool>> predicate,
            Expression<Func<CoreCmsArticle, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize);
        }
    }
}