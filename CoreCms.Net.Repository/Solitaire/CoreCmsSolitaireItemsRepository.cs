/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/6/19 23:42:28
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 接龙活动商品表 接口实现
    /// </summary>
    public class CoreCmsSolitaireItemsRepository : BaseRepository<CoreCmsSolitaireItems>, ICoreCmsSolitaireItemsRepository
    {
        public CoreCmsSolitaireItemsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsSolitaireItems entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync() > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsSolitaireItems entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await DbClient.Queryable<CoreCmsSolitaireItems>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.solitaireId = entity.solitaireId;
            oldModel.goodId = entity.goodId;
            oldModel.productId = entity.productId;
            oldModel.price = entity.price;
            oldModel.activityStock = entity.activityStock;
            oldModel.oneCanBuy = entity.oneCanBuy;
            oldModel.isDelete = entity.isDelete;

            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsSolitaireItems> entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsSolitaireItems>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsSolitaireItems>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        #endregion

        #region 重写根据条件查询列表数据
        /// <summary>
        ///     重写根据条件查询列表数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<List<CoreCmsSolitaireItems>> QueryListByClauseAsync(Expression<Func<CoreCmsSolitaireItems, bool>> predicate,
            Expression<Func<CoreCmsSolitaireItems, object>> orderByExpression, OrderByType orderByType, bool blUseNoLock = false)
        {
            List<CoreCmsSolitaireItems> list;
            if (blUseNoLock)
            {    
                list = await DbClient.Queryable<CoreCmsSolitaireItems>()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate).Select(p => new CoreCmsSolitaireItems
                    {
                        id = p.id,
                        solitaireId = p.solitaireId,
                        goodId = p.goodId,
                        productId = p.productId,
                        price = p.price,
                        activityStock = p.activityStock,
                        oneCanBuy = p.oneCanBuy,
                        sortId = p.sortId,
                        isDelete = p.isDelete,
                    })
                    .With(SqlWith.NoLock)
                    .MergeTable()
                    .Mapper(it => it.productObj, it => it.productId)
                    .Mapper(it => it.goodObj, it => it.goodId)
                    .ToListAsync();
            }
            else
            {
                list = await DbClient.Queryable<CoreCmsSolitaireItems>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsSolitaireItems
                {
                    id = p.id,
                    solitaireId = p.solitaireId,
                    goodId = p.goodId,
                    productId = p.productId,
                    price = p.price,
                    activityStock = p.activityStock,
                    oneCanBuy = p.oneCanBuy,
                    sortId = p.sortId,
                    isDelete = p.isDelete,

                })
                .Mapper(it => it.productObj, it => it.productId)
                .Mapper(it => it.goodObj, it => it.goodId)
                .ToListAsync();
            }
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
        public new async Task<IPageList<CoreCmsSolitaireItems>> QueryPageAsync(Expression<Func<CoreCmsSolitaireItems, bool>> predicate,
            Expression<Func<CoreCmsSolitaireItems, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsSolitaireItems> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsSolitaireItems>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsSolitaireItems
                {
                    id = p.id,
                    solitaireId = p.solitaireId,
                    goodId = p.goodId,
                    productId = p.productId,
                    price = p.price,
                    activityStock = p.activityStock,
                    oneCanBuy = p.oneCanBuy,
                    sortId = p.sortId,
                    isDelete = p.isDelete,

                })
                .With(SqlWith.NoLock)
                .Mapper(it => it.productObj, it => it.productId)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsSolitaireItems>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsSolitaireItems
                {
                    id = p.id,
                    solitaireId = p.solitaireId,
                    goodId = p.goodId,
                    productId = p.productId,
                    price = p.price,
                    activityStock = p.activityStock,
                    oneCanBuy = p.oneCanBuy,
                    sortId = p.sortId,
                    isDelete = p.isDelete,

                })
                .Mapper(it => it.productObj, it => it.productId)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsSolitaireItems>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
