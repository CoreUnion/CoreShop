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
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 公告表 接口实现
    /// </summary>
    public class CoreCmsNoticeRepository : BaseRepository<CoreCmsNotice>, ICoreCmsNoticeRepository
    {
        public CoreCmsNoticeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
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
        public async Task<IPageList<CoreCmsNotice>> QueryPageAsync(Expression<Func<CoreCmsNotice, bool>> predicate,
            Expression<Func<CoreCmsNotice, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            RefAsync<int> totalCount = 0;
            var page = await DbClient.Queryable<CoreCmsNotice>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsNotice
                {
                    id = p.id,
                    title = p.title,
                    type = p.type,
                    sort = p.sort,
                    isDel = p.isDel,
                    createTime = p.createTime
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            var list = new PageList<CoreCmsNotice>(page, pageIndex, pageSize, totalCount);
            return list;
        }


        /// <summary>
        ///     获取列表首页用
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<List<CoreCmsNotice>> QueryListAsync(Expression<Func<CoreCmsNotice, bool>> predicate,
            Expression<Func<CoreCmsNotice, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            var list = await DbClient.Queryable<CoreCmsNotice>().OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsNotice
                {
                    id = p.id,
                    title = p.title,
                    type = p.type,
                    sort = p.sort,
                    isDel = p.isDel,
                    createTime = p.createTime
                }).ToPageListAsync(pageIndex, pageSize);
            return list;
        }



    }
}
