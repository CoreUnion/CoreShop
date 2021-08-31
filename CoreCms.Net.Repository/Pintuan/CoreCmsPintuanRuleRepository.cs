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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 拼团规则表 接口实现
    /// </summary>
    public class CoreCmsPinTuanRuleRepository : BaseRepository<CoreCmsPinTuanRule>, ICoreCmsPinTuanRuleRepository
    {
        public CoreCmsPinTuanRuleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region 取购物车数据的时候，更新价格
        /// <summary>
        /// 取购物车数据的时候，更新价格
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public WebApiCallBack PinTuanInfo(List<CartProducts> list)
        {
            var res = new WebApiCallBack();
            foreach (var item in list)
            {
                var ruleModel = DbClient.Queryable<CoreCmsPinTuanGoods, CoreCmsPinTuanRule>(
                        (pinTuanGoods, pinTuanRule) => new object[]
                        {
                            JoinType.Inner, pinTuanGoods.ruleId == pinTuanRule.id
                        }).Where((pinTuanGoods, pinTuanRule) =>
                        pinTuanGoods.goodsId == item.products.goodsId && pinTuanRule.isStatusOpen == true)
                    .Select((pinTuanGoods, pinTuanRule) => pinTuanRule).First();
                if (ruleModel == null)
                {
                    res.data = 15603;
                    res.msg = GlobalErrorCodeVars.Code15603;
                    return res;
                }
                var dt = DateTime.Now;
                if (ruleModel.startTime > dt)
                {
                    res.data = 15601;
                    res.msg = GlobalErrorCodeVars.Code15601;
                    return res;
                }
                if (ruleModel.endTime < dt)
                {
                    res.data = 15602;
                    res.msg = GlobalErrorCodeVars.Code15602;
                    return res;
                }
                item.products.price = item.products.price - ruleModel.discountAmount;
                if (item.products.price < 0)
                {
                    res.data = 15612;
                    res.msg = GlobalErrorCodeVars.Code15612;
                    return res;
                }
            }
            res.status = true;
            res.data = list;
            return res;
        }

        #endregion

        #region 根据条件查询分页数据

        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        public async Task<IPageList<TagPinTuanResult>> QueryTagPinTuanPageAsync(Expression<Func<TagPinTuanResult, bool>> predicate,
            Expression<Func<TagPinTuanResult, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1, int pageSize = 20)
        {
            RefAsync<int> totalCount = 0;
            var page = await DbClient.Queryable<CoreCmsPinTuanGoods, CoreCmsPinTuanRule, CoreCmsGoods>((pgModel, prModel, goodModel) => new object[] {
                JoinType.Inner,pgModel.ruleId==prModel.id,
                JoinType.Inner,pgModel.goodsId==goodModel.id
            }).Select((pgModel, prModel, goodModel) => new TagPinTuanResult
            {
                id = prModel.id,
                name = prModel.name,
                startTime = prModel.startTime,
                endTime = prModel.endTime,
                peopleNumber = prModel.peopleNumber,
                significantInterval = prModel.significantInterval,
                discountAmount = prModel.discountAmount,
                maxNums = prModel.maxNums,
                maxGoodsNums = prModel.maxGoodsNums,
                sort = prModel.sort,
                isStatusOpen = prModel.isStatusOpen,
                createTime = prModel.createTime,
                updateTime = prModel.updateTime,
                goodsId = pgModel.goodsId,
                goodsName = goodModel.name,
                goodsImages = goodModel.images,
                goodsImage = goodModel.image,
            }).MergeTable()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate)
                .With(SqlWith.Null).ToPageListAsync(pageIndex, pageSize, totalCount);

            var list = new PageList<TagPinTuanResult>(page, pageIndex, pageSize, totalCount);
            return list;
        }
        #endregion

        #region 根据商品id获取拼团规则信息
        /// <summary>
        ///     根据商品id获取拼团规则信息
        /// </summary>
        /// <param name="goodId"></param>
        /// <returns></returns>
        public async Task<TagPinTuanResult> GetPinTuanInfo(int goodId)
        {
            var dt = DateTime.Now;
            var reuslt = await DbClient.Queryable<CoreCmsPinTuanGoods, CoreCmsPinTuanRule, CoreCmsGoods, CoreCmsProducts>(
                (pgModel, prModel, goodModel, productsModel) => new object[]
                {
                    JoinType.Inner, pgModel.ruleId == prModel.id,
                    JoinType.Inner, pgModel.goodsId == goodModel.id,
                    JoinType.Left, goodModel.id == productsModel.goodsId
                })
                .Where((pgModel, prModel, goodModel, productsModel) => prModel.isStatusOpen == true && pgModel.goodsId == goodId && prModel.endTime > dt && productsModel.isDefalut == true && productsModel.isDel == false)
                .Select((pgModel, prModel, goodModel, productsModel) => new TagPinTuanResult
                {
                    id = prModel.id,
                    name = prModel.name,
                    startTime = prModel.startTime,
                    endTime = prModel.endTime,
                    peopleNumber = prModel.peopleNumber,
                    significantInterval = prModel.significantInterval,
                    discountAmount = prModel.discountAmount,
                    maxNums = prModel.maxNums,
                    maxGoodsNums = prModel.maxGoodsNums,
                    sort = prModel.sort,
                    isStatusOpen = prModel.isStatusOpen,
                    createTime = prModel.createTime,
                    updateTime = prModel.updateTime,
                    goodsId = pgModel.goodsId,
                    goodsName = goodModel.name,
                    goodsImages = goodModel.images,
                    goodsPrice = productsModel.price,
                    goodsImage = goodModel.image,

                }).FirstAsync();
            return reuslt;
        }

        #endregion

    }
}
