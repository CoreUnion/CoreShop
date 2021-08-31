/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/6/14 23:17:57
 *        Description: 暂无
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
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
    /// 接龙活动表 接口实现
    /// </summary>
    public class CoreCmsSolitaireRepository : BaseRepository<CoreCmsSolitaire>, ICoreCmsSolitaireRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoreCmsSolitaireRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsSolitaire entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.status != (int)GlobalEnumVars.SolitaireStatus.Close && entity.status != (int)GlobalEnumVars.SolitaireStatus.Open)
            {
                jm.msg = "请设置活动状态";
                return jm;
            }
            if (entity.endTime < entity.startTime)
            {
                jm.msg = "活动开始时间不能大于结束时间";
                return jm;
            }
            if (entity.items == null || entity.items.Count <= 0)
            {
                jm.msg = "请设置商品sku";
                return jm;
            }
            var bl = false;
            try
            {
                _unitOfWork.BeginTran();

                entity.createTime = DateTime.Now;

                var id = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync();
                if (id > 0)
                {
                    entity.items.ForEach(p =>
                    {
                        p.solitaireId = id;
                    });
                }
                await DbClient.Insertable(entity.items).ExecuteCommandAsync();

                _unitOfWork.CommitTran();
                bl = true;
            }
            catch (Exception e)
            {
                bl = false;
                _unitOfWork.RollbackTran();
                jm.msg = GlobalConstVars.DataHandleEx;
                jm.data = e;
            }

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsSolitaire entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.status != (int)GlobalEnumVars.SolitaireStatus.Close && entity.status != (int)GlobalEnumVars.SolitaireStatus.Open)
            {
                jm.msg = "请设置活动状态";
                return jm;
            }
            if (entity.endTime < entity.startTime)
            {
                jm.msg = "活动开始时间不能大于结束时间";
                return jm;
            }
            if (entity.items == null || entity.items.Count <= 0)
            {
                jm.msg = "请设置商品sku";
                return jm;
            }

            var oldModel = await DbClient.Queryable<CoreCmsSolitaire>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var bl = false;
            try
            {
                _unitOfWork.BeginTran();

                //事物处理过程开始
                //oldModel.id = entity.id;
                oldModel.title = entity.title;
                oldModel.contentBody = entity.contentBody;
                oldModel.startTime = entity.startTime;
                oldModel.endTime = entity.endTime;
                oldModel.startBuyPrice = entity.startBuyPrice;
                oldModel.minDeliveryPrice = entity.minDeliveryPrice;
                oldModel.isShow = entity.isShow;
                oldModel.status = entity.status;
                oldModel.thumbnail = entity.thumbnail;

                //oldModel.isDelete = entity.isDelete;
                //oldModel.createTime = entity.createTime;

                await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();

                //获取数据库存在的数据
                var oldList = await DbClient.Queryable<CoreCmsSolitaireItems>().Where(p => p.solitaireId == oldModel.id && p.isDelete == false).ToListAsync();
                var oldProductIds = oldList.Select(p => p.productId).ToList();

                //获取提交的数据有货品序列
                var newProductIds = entity.items.Select(p => p.productId).ToList();

                //标记已经不存在新数据里面的货品数据为假删除状态
                await DbClient.Updateable<CoreCmsSolitaireItems>(p => p.isDelete == true)
                    .Where(p => !newProductIds.Contains(p.productId) && p.solitaireId == oldModel.id && p.isDelete == false)
                    .ExecuteCommandAsync();

                //获取老数据并进行更新
                var oldItems = oldList.Where(p => newProductIds.Contains(p.productId)).ToList();
                if (oldItems.Any())
                {
                    oldItems.ForEach(o =>
                    {
                        var newOne = entity.items.Find(p => p.productId == o.productId);
                        o.price = newOne.price;
                        o.activityStock = newOne.activityStock;
                        o.oneCanBuy = newOne.oneCanBuy;
                        o.sortId = newOne.sortId;
                    });
                    await DbClient.Updateable(oldItems).ExecuteCommandAsync();
                }

                //获取新数据并进行增加
                var newItems = entity.items.Where(p => !oldProductIds.Contains(p.productId)).ToList();
                if (newItems.Any())
                {
                    var newList = new List<CoreCmsSolitaireItems>();
                    newItems.ForEach(p =>
                    {
                        newList.Add(new CoreCmsSolitaireItems()
                        {
                            solitaireId = oldModel.id,
                            goodId = p.goodId,
                            productId = p.productId,
                            price = p.price,
                            activityStock = p.activityStock,
                            oneCanBuy = p.oneCanBuy,
                            sortId = p.sortId,
                            isDelete = false,
                        });
                    });
                    await DbClient.Insertable(newList).ExecuteCommandAsync();
                }
                //事物处理过程结束
                _unitOfWork.CommitTran();
                bl = true;

            }
            catch (Exception e)
            {
                bl = false;
                _unitOfWork.RollbackTran();
                jm.msg = GlobalConstVars.DataHandleEx;
                jm.data = e;
            }
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsSolitaire> entity)
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

            var bl = await DbClient.Deleteable<CoreCmsSolitaire>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<CoreCmsSolitaire>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
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
        public new async Task<IPageList<CoreCmsSolitaire>> QueryPageAsync(Expression<Func<CoreCmsSolitaire, bool>> predicate,
            Expression<Func<CoreCmsSolitaire, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsSolitaire> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsSolitaire>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsSolitaire
                {
                    id = p.id,
                    title = p.title,
                    //contentBody = p.contentBody,
                    startTime = p.startTime,
                    endTime = p.endTime,
                    startBuyPrice = p.startBuyPrice,
                    minDeliveryPrice = p.minDeliveryPrice,
                    isShow = p.isShow,
                    status = p.status,
                    isDelete = p.isDelete,
                    createTime = p.createTime,

                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsSolitaire>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsSolitaire
                {
                    id = p.id,
                    title = p.title,
                    //contentBody = p.contentBody,
                    startTime = p.startTime,
                    endTime = p.endTime,
                    startBuyPrice = p.startBuyPrice,
                    minDeliveryPrice = p.minDeliveryPrice,
                    isShow = p.isShow,
                    status = p.status,
                    isDelete = p.isDelete,
                    createTime = p.createTime,

                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsSolitaire>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
