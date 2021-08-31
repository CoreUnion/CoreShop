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
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using SqlSugar;
using ToolGood.Words;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 商品评价表 接口实现
    /// </summary>
    public class CoreCmsGoodsCommentServices : BaseServices<CoreCmsGoodsComment>, ICoreCmsGoodsCommentServices
    {
        private readonly ICoreCmsGoodsCommentRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly IToolsServices _toolsServices;
        public CoreCmsGoodsCommentServices(IUnitOfWork unitOfWork, ICoreCmsGoodsCommentRepository dal,
            IServiceProvider serviceProvider, IToolsServices toolsServices)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _toolsServices = toolsServices;
        }

        /// <summary>
        /// 添加一条评论
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="items">评价数据</param>
        /// <param name="userId">用户序列</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> AddComment(string orderId, List<OrderEvaluatePostItems> items, int userId)
        {
            var jm = new WebApiCallBack();

            using var container = _serviceProvider.CreateScope();

            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();
            var orderItemServices = container.ServiceProvider.GetService<ICoreCmsOrderItemServices>();
            var goodsServices = container.ServiceProvider.GetService<ICoreCmsGoodsServices>();

            //判断这个订单是否可以评价
            var res = await orderServices.IsOrderComment(orderId, userId);
            if (!res.status)
            {
                //已经评价或者存在问题
                return res;
            }
            var goodComments = new List<CoreCmsGoodsComment>();
            var gid = new List<int>();




            foreach (var item in items)
            {
                //判断此条记录是否是此订单下面的
                var itemInfo = await orderItemServices.QueryByClauseAsync(p => p.id == item.orderItemId && p.orderId == orderId);
                if (itemInfo == null)
                {
                    //说明没有此条记录，就不需要评论了
                    continue;
                }
                var score = 5;
                if (item.score >= 1 && item.score <= 5)
                {
                    score = item.score;
                }
                var images = string.Empty;
                if (item.images.Any())
                {
                    images = string.Join(",", item.images);
                }

                //过滤违规字符串
                item.textarea = await _toolsServices.IllegalWordsReplace(item.textarea);

                var commentModel = new CoreCmsGoodsComment
                {
                    commentId = 0,
                    score = score,
                    userId = userId,
                    goodsId = itemInfo.goodsId,
                    orderId = orderId,
                    images = images,
                    contentBody = item.textarea,
                    addon = itemInfo.addon,
                    isDisplay = false,
                    createTime = DateTime.Now
                };
                goodComments.Add(commentModel);
                gid.Add(itemInfo.goodsId);
            }

            await _dal.InsertAsync(goodComments);
            //商品表更新评论数量
            await goodsServices.UpdateAsync(p => new CoreCmsGoods() { commentsCount = p.commentsCount + 1 },
                p => gid.Contains(p.id));
            //修改评价状态
            await orderServices.UpdateAsync(p => new CoreCmsOrder() { isComment = true }, p => p.orderId == orderId);

            jm.status = true;
            jm.msg = "评价成功";

            return jm;
        }


        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsGoodsComment entity)
        {
            return await _dal.InsertAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsGoodsComment entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsGoodsComment> entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            return await _dal.DeleteByIdAsync(id);
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            return await _dal.DeleteByIdsAsync(ids);
        }

        #endregion

        /// <summary>
        /// 商家回复评价
        /// </summary>
        /// <param name="id">序列</param>
        /// <param name="sellerContent">回复内容</param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> Reply(int id, string sellerContent)
        {
            return await _dal.Reply(id, sellerContent);
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
            return await _dal.DetailsByIdAsync(predicate, orderByExpression, orderByType);
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
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion

    }
}
