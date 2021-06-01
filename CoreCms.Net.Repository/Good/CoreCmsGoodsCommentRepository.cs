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
using System.Linq;
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
    /// 商品评价表 接口实现
    /// </summary>
    public class CoreCmsGoodsCommentRepository : BaseRepository<CoreCmsGoodsComment>, ICoreCmsGoodsCommentRepository
    {
        public CoreCmsGoodsCommentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsGoodsComment entity)
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
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsGoodsComment entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await DbClient.Queryable<CoreCmsGoodsComment>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.commentId = entity.commentId;
            oldModel.score = entity.score;
            oldModel.userId = entity.userId;
            oldModel.goodsId = entity.goodsId;
            oldModel.orderId = entity.orderId;
            oldModel.addon = entity.addon;
            oldModel.images = entity.images;
            oldModel.contentBody = entity.contentBody;
            oldModel.sellerContent = entity.sellerContent;
            oldModel.isDisplay = entity.isDisplay;
            oldModel.createTime = entity.createTime;

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
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsGoodsComment> entity)
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

            var bl = await DbClient.Deleteable<CoreCmsGoodsComment>(id).ExecuteCommandHasChangeAsync();
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

            var bl = await DbClient.Deleteable<CoreCmsGoodsComment>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        #endregion


        /// <summary>
        /// 商家回复评价
        /// </summary>
        /// <param name="id">序列</param>
        /// <param name="sellerContent">回复内容</param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> Reply(int id,string sellerContent)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await DbClient.Queryable<CoreCmsGoodsComment>().In(id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.sellerContent = sellerContent;

            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }



        /// <summary>
        /// 获取单个详情数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<CoreCmsGoodsComment> DetailsByIdAsync(Expression<Func<CoreCmsGoodsComment, bool>> predicate,
            Expression<Func<CoreCmsGoodsComment, object>> orderByExpression, OrderByType orderByType)
        {
            var model = await DbClient.Queryable<CoreCmsGoodsComment, CoreCmsUser, CoreCmsGoods>((p, cUser, cGood) => new JoinQueryInfos(
                    JoinType.Left, p.userId == cUser.id,
                    JoinType.Left, p.goodsId == cGood.id
                ))
                .Select((p, cUser, cGood) => new CoreCmsGoodsComment
                {
                    id = p.id,
                    commentId = p.commentId,
                    score = p.score,
                    userId = p.userId,
                    goodsId = p.goodsId,
                    orderId = p.orderId,
                    addon = p.addon,
                    images = p.images,
                    contentBody = p.contentBody,
                    sellerContent = p.sellerContent,
                    isDisplay = p.isDisplay,
                    createTime = p.createTime,
                    avatarImage = cUser.avatarImage,
                    nickName = cUser.nickName,
                    mobile = cUser.mobile,
                    goodName = cGood.name,
                })
                .MergeTable()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .Where(predicate)
                .FirstAsync();

            if (model != null)
            {
                model.imagesArr = !string.IsNullOrEmpty(model.images) ? model.images.Split(",") : new String[0];
            }

            return model;
        }


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
        public new async Task<IPageList<CoreCmsGoodsComment>> QueryPageAsync(Expression<Func<CoreCmsGoodsComment, bool>> predicate,
            Expression<Func<CoreCmsGoodsComment, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;

            List<CoreCmsGoodsComment> page = await DbClient.Queryable<CoreCmsGoodsComment, CoreCmsUser, CoreCmsGoods>((p, cUser, cGood) => new JoinQueryInfos(
                    JoinType.Left, p.userId == cUser.id,
                    JoinType.Left, p.goodsId == cGood.id
                ))
                .Select((p, cUser, cGood) => new CoreCmsGoodsComment
                {
                    id = p.id,
                    commentId = p.commentId,
                    score = p.score,
                    userId = p.userId,
                    goodsId = p.goodsId,
                    orderId = p.orderId,
                    addon = p.addon,
                    images = p.images,
                    contentBody = p.contentBody,
                    sellerContent = p.sellerContent,
                    isDisplay = p.isDisplay,
                    createTime = p.createTime,
                    avatarImage = cUser.avatarImage,
                    nickName = cUser.nickName,
                    mobile = cUser.mobile,
                    goodName = cGood.name,
                })
                .MergeTable()//将上面的操作变成一个表 mergetable
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .Where(predicate)
                .ToPageListAsync(pageIndex, pageSize, totalCount);

            if (page.Any())
            {
                foreach (var item in page)
                {
                    item.imagesArr = !string.IsNullOrEmpty(item.images) ? item.images.Split(",") : new String[0];
                }
            }


            var list = new PageList<CoreCmsGoodsComment>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
