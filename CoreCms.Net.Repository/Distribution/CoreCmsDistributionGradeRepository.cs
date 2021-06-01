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
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 分销商等级设置表 接口实现
    /// </summary>
    public class CoreCmsDistributionGradeRepository : BaseRepository<CoreCmsDistributionGrade>, ICoreCmsDistributionGradeRepository
    {
        public CoreCmsDistributionGradeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsDistributionGrade entity)
        {
            var jm = new AdminUiCallBack();

            if (await DbClient.Queryable<CoreCmsDistributionGrade>().AnyAsync(p => p.sortId == entity.sortId))
            {
                jm.msg = "存在相同等级排序,请更换!";
                return jm;
            }

            var id = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync();
            var bl = id > 0;

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            if (bl)
            {
                if (entity.isDefault == true)
                {
                    await DbClient.Updateable<CoreCmsDistributionGrade>().SetColumns(p => p.isDefault == false).Where(p => p.isDefault == true && p.id != id).ExecuteCommandAsync();
                }
                await UpdateCaChe();
            }
            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsDistributionGrade entity)
        {
            var jm = new AdminUiCallBack();

            if (await DbClient.Queryable<CoreCmsDistributionGrade>().AnyAsync(p => p.sortId == entity.sortId && entity.id != p.id))
            {
                jm.msg = "存在相同等级排序,请更换!";
                return jm;
            }

            if (entity.isDefault == false)
            {
                var otherHave = await DbClient.Queryable<CoreCmsDistributionGrade>().AnyAsync(p => p.isDefault == true && p.id != entity.id);
                if (otherHave == false)
                {
                    jm.msg = "请保持一个默认分销等级";
                    return jm;
                }
            }

            var oldModel = await DbClient.Queryable<CoreCmsDistributionGrade>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            //oldModel.id = entity.id;
            oldModel.name = entity.name;
            oldModel.isDefault = entity.isDefault;
            oldModel.isAutoUpGrade = entity.isAutoUpGrade;
            oldModel.sortId = entity.sortId;
            oldModel.description = entity.description;

            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                //其他处理
                if (entity.isDefault)
                {
                    await DbClient.Updateable<CoreCmsDistributionGrade>().SetColumns(it => it.isDefault == false).Where(p => p.isDefault == true && p.id != entity.id).ExecuteCommandAsync();
                }
                await UpdateCaChe();
            }

            return jm;
        }


        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> DeleteByIdAsync(int id)
        {
            var jm = new AdminUiCallBack();

            if (await DbClient.Queryable<CoreCmsDistribution>().AnyAsync(p => p.gradeId == id))
            {
                jm.msg = "存在关联的分销用户数据,禁止删除";
                return jm;
            }
            var bl = await DbClient.Deleteable<CoreCmsDistributionGrade>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await DbClient.Deleteable<CoreCmsDistributionCondition>().Where(p => p.gradeId == id).ExecuteCommandAsync();
                await DbClient.Deleteable<CoreCmsDistributionResult>().Where(p => p.gradeId == id).ExecuteCommandAsync();
                await UpdateCaChe();
            }

            return jm;
        }


        #endregion

        #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoreCmsDistributionGrade>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<CoreCmsDistributionGrade>>(GlobalConstVars.CacheCoreCmsDistributionGrade);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<CoreCmsDistributionGrade>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<CoreCmsDistributionGrade>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheCoreCmsDistributionGrade, list);
            return list;
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
        public new async Task<IPageList<CoreCmsDistributionGrade>> QueryPageAsync(Expression<Func<CoreCmsDistributionGrade, bool>> predicate,
            Expression<Func<CoreCmsDistributionGrade, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsDistributionGrade> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsDistributionGrade>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsDistributionGrade
                {
                    id = p.id,
                    name = p.name,
                    isDefault = p.isDefault,
                    isAutoUpGrade = p.isAutoUpGrade,
                    sortId = p.sortId,
                    description = p.description,

                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsDistributionGrade>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsDistributionGrade
                {
                    id = p.id,
                    name = p.name,
                    isDefault = p.isDefault,
                    isAutoUpGrade = p.isAutoUpGrade,
                    sortId = p.sortId,
                    description = p.description,

                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsDistributionGrade>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion
    }
}
