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
using CoreCms.Net.Loging;
using CoreCms.Net.Model.ViewModels.UI;
using NLog;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 代理商品池 接口实现
    /// </summary>
    public class CoreCmsAgentGoodsRepository : BaseRepository<CoreCmsAgentGoods>, ICoreCmsAgentGoodsRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsAgentGoodsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <param name="products"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> InsertAsync(CoreCmsAgentGoods entity, List<CoreCmsAgentProducts> products)
        {
            var jm = new AdminUiCallBack();

            try
            {
                var isHave = await DbClient.Queryable<CoreCmsAgentGoods>().AnyAsync(p => p.goodId == entity.goodId);
                if (isHave)
                {
                    jm.msg = "此商品已录入代理池";
                    return jm;
                }

                var good = await DbClient.Queryable<CoreCmsGoods>().FirstAsync(p => p.id == entity.goodId);
                if (good == null)
                {
                    jm.msg = "商品不存在";
                    return jm;
                }

                _unitOfWork.BeginTran();

                entity.createTime = DateTime.Now;
                entity.goodRefreshTime = good.updateTime;

                var id = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync();
                if (id <= 0)
                {
                    _unitOfWork.RollbackTran();
                    jm.msg = GlobalConstVars.DataHandleEx;
                    return jm;
                }
                products.ForEach(p =>
                {
                    p.agentGoodsId = id;
                    p.createTime = DateTime.Now;
                    p.isDel = false;
                    p.goodId = entity.goodId;
                });

                var bl = await DbClient.Insertable(products).ExecuteCommandAsync() > 0;

                _unitOfWork.CommitTran();

                jm.code = bl ? 0 : 1;
                jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTran();
                jm.msg = GlobalConstVars.DataHandleEx;
                jm.data = e;
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateAsync(CoreCmsAgentGoods entity, List<CoreCmsAgentProducts> products)
        {
            var jm = new AdminUiCallBack();

            try
            {
                var isHave = await DbClient.Queryable<CoreCmsAgentGoods>().AnyAsync(p => p.goodId == entity.goodId && p.id != entity.id);
                if (isHave)
                {
                    jm.msg = "此商品已录入代理池";
                    return jm;
                }

                var good = await DbClient.Queryable<CoreCmsGoods>().FirstAsync(p => p.id == entity.goodId);
                if (good == null)
                {
                    jm.msg = "商品不存在";
                    return jm;
                }


                var oldModel = await DbClient.Queryable<CoreCmsAgentGoods>().FirstAsync(p => p.id == entity.id);
                if (oldModel == null)
                {
                    jm.msg = "编辑数据不存在";
                    return jm;
                }
                _unitOfWork.BeginTran();

                oldModel.updateTime = DateTime.Now;
                oldModel.goodId = entity.goodId;
                oldModel.sortId = entity.sortId;
                oldModel.isEnable = entity.isEnable;
                oldModel.goodRefreshTime = good.updateTime;

                products.ForEach(p =>
                {
                    p.agentGoodsId = oldModel.id;
                    p.createTime = DateTime.Now;
                    p.isDel = false;
                    p.goodId = entity.goodId;
                });

                //数据处理
                await DbClient.Updateable(oldModel).ExecuteCommandAsync();

                await DbClient.Deleteable<CoreCmsAgentProducts>(p => p.agentGoodsId == oldModel.id).ExecuteCommandHasChangeAsync();

                await DbClient.Insertable(products).ExecuteCommandAsync();

                _unitOfWork.CommitTran();

                jm.code = 0;
                jm.msg = GlobalConstVars.EditSuccess;

            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTran();
                jm.msg = GlobalConstVars.DataHandleEx;
                jm.data = e;
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsAgentGoods> entity)
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
        public async Task<AdminUiCallBack> DeleteByIdAsync(int id)
        {
            var jm = new AdminUiCallBack();
            try
            {
                var model = await DbClient.Queryable<CoreCmsAgentGoods>().FirstAsync(p => p.id == id);
                if (model == null)
                {
                    jm.msg = GlobalConstVars.DataisNo;
                    return jm;
                }

                _unitOfWork.BeginTran();
                var bl = await DbClient.Deleteable<CoreCmsAgentGoods>(id).ExecuteCommandHasChangeAsync();
                if (bl)
                {
                    await DbClient.Deleteable<CoreCmsAgentProducts>(p => p.agentGoodsId == model.id).ExecuteCommandHasChangeAsync();
                }
                _unitOfWork.CommitTran();

                jm.code = bl ? 0 : 1;
                jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTran();
                NLogUtil.WriteAll(LogLevel.Error, LogType.Web, "删除代理池商品", "删除报错 ", e);
            }

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

            var bl = await DbClient.Deleteable<CoreCmsAgentGoods>().In(ids).ExecuteCommandHasChangeAsync();
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
        public new async Task<IPageList<CoreCmsAgentGoods>> QueryPageAsync(Expression<Func<CoreCmsAgentGoods, bool>> predicate,
            Expression<Func<CoreCmsAgentGoods, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsAgentGoods> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsAgentGoods, CoreCmsGoods>((ag, cg) => new JoinQueryInfos(JoinType.Left, ag.goodId == cg.id))
                .Select((ag, cg) => new CoreCmsAgentGoods
                {
                    id = ag.id,
                    goodId = ag.goodId,
                    goodRefreshTime = ag.goodRefreshTime,
                    sortId = ag.sortId,
                    isEnable = ag.isEnable,
                    createTime = ag.createTime,
                    updateTime = ag.updateTime,
                    goodName = cg.name,
                    goodImage = cg.image,
                    goodUpdateTime = cg.updateTime
                })
                .With(SqlWith.NoLock)
                .MergeTable()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsAgentGoods, CoreCmsGoods>((ag, cg) => new JoinQueryInfos(JoinType.Left, ag.goodId == cg.id))
                    .Select((ag, cg) => new CoreCmsAgentGoods
                    {
                        id = ag.id,
                        goodId = ag.goodId,
                        goodRefreshTime = ag.goodRefreshTime,
                        sortId = ag.sortId,
                        isEnable = ag.isEnable,
                        createTime = ag.createTime,
                        updateTime = ag.updateTime,
                        goodName = cg.name,
                        goodImage = cg.image,
                        goodUpdateTime = cg.updateTime
                    })
                    .MergeTable()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsAgentGoods>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
