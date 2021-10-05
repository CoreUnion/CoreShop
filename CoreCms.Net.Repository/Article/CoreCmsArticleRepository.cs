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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Utility.Helper;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 文章表 接口实现
    /// </summary>
    public class CoreCmsArticleRepository : BaseRepository<CoreCmsArticle>, ICoreCmsArticleRepository
    {
        public CoreCmsArticleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        #region  获取指定id 的文章详情
        /// <summary>
        /// 获取指定id 的文章详情
        /// </summary>
        /// <param name="id">序列</param>
        public async Task<CoreCmsArticle> ArticleDetail(int id)
        {
            var article = await DbClient.Queryable<CoreCmsArticle>().Where(p => p.id == id && p.isPub == true)
                .FirstAsync();
            if (article == null)
            {
                return null;
            }
            article.contentBody = CommonHelper.ClearHtml(article.contentBody, new[] { "width", "height" });
            article.contentBody = article.contentBody.Replace("<img", "<img style='max-width: 100%'");
            article.contentBody = article.contentBody.Replace("oembed url=", "video  width=\"100%\"  controls=\"controls\" src=");
            article.contentBody = article.contentBody.Replace("/oembed", "/video");

            if (article.typeId > 0)
            {
                article.articleType = await DbClient.Queryable<CoreCmsArticleType>().InSingleAsync(article.typeId);
            }
            //上一篇
            article.upArticle = await DbClient.Queryable<CoreCmsArticle>().Where(p => p.id < article.id && p.isPub == true).Select(p => new CoreCmsArticle
            {
                id = p.id,
                title = p.title,
                brief = p.brief,
                coverImage = p.coverImage,
                typeId = p.typeId,
                sort = p.sort,
                isPub = p.isPub,
                isDel = p.isDel,
                pv = p.pv,
                createTime = p.createTime,
                updateTime = p.updateTime
            }).FirstAsync();
            //下一篇
            article.downArticle = await DbClient.Queryable<CoreCmsArticle>().Where(p => p.id > article.id && p.isPub == true).Select(p => new CoreCmsArticle
            {
                id = p.id,
                title = p.title,
                brief = p.brief,
                coverImage = p.coverImage,
                typeId = p.typeId,
                sort = p.sort,
                isPub = p.isPub,
                isDel = p.isDel,
                pv = p.pv,
                createTime = p.createTime,
                updateTime = p.updateTime
            }).FirstAsync();

            await DbClient.Updateable<CoreCmsArticle>().SetColumns(p => p.pv == (p.pv + 1)).Where(p => p.id == article.id).ExecuteCommandAsync();

            return article;
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
        /// <returns></returns>
        public async Task<IPageList<CoreCmsArticle>> QueryPageAsync(Expression<Func<CoreCmsArticle, bool>> predicate,
            Expression<Func<CoreCmsArticle, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            RefAsync<int> totalCount = 0;
            var page = await DbClient.Queryable<CoreCmsArticle>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsArticle
                {
                    id = p.id,
                    title = p.title,
                    brief = p.brief,
                    coverImage = p.coverImage,
                    typeId = p.typeId,
                    sort = p.sort,
                    isPub = p.isPub,
                    isDel = p.isDel,
                    pv = p.pv,
                    createTime = p.createTime,
                    updateTime = p.updateTime
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            var list = new PageList<CoreCmsArticle>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
