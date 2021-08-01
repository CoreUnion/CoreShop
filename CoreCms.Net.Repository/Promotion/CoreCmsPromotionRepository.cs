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
using CoreCms.Net.Utility.Extensions;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 促销表 接口实现
    /// </summary>
    public class CoreCmsPromotionRepository : BaseRepository<CoreCmsPromotion>, ICoreCmsPromotionRepository
    {
        public CoreCmsPromotionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }


        //判断商品是否参加团购
        /// <summary>
        /// 判断商品是否参加团购
        /// </summary>
        /// <param name="goodId">商品序列</param>
        /// <param name="promotionId">关联促销信息</param>
        /// <returns></returns>
        public bool IsInGroup(int goodId, out int promotionId)
        {
            promotionId = 0;
            if (goodId == 0)
            {
                return false;
            }
            var dt = DateTime.Now;
            var goodIds = "\"" + goodId + "\"";

            var model = DbClient.Queryable<CoreCmsPromotion, CoreCmsPromotionCondition>(
                    (pro, ccpc) => new object[] {
                        JoinType.Inner, pro.id == ccpc.promotionId
                    }
                )
                .Where((pro, ccpc) => pro.isEnable == true && pro.isDel == false)
                //.Where((pro, ccpc) => pro.startTime < dt && pro.endTime > dt)
                //.Where((pro, ccpc) => ccpc.parameters.Contains("%\"" + goodId + "\"%"))
                .Where((pro, ccpc) => ccpc.parameters.Contains(goodIds))
                .Where((pro, ccpc) => pro.type == (int)GlobalEnumVars.PromotionType.Group ||
                                      pro.type == (int)GlobalEnumVars.PromotionType.Seckill)
                .Select((pro, ccpc) => ccpc)
                .First();

            if (model != null)
            {
                promotionId = model.promotionId.ObjectToInt();
            }
            return model != null;
        }


        #region 查询查了并获取导航下级数据

        /// <summary>
        ///     查询查了并获取导航下级数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="isToPage">是否分页</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsPromotion>> QueryPageAndChildsAsync(Expression<Func<CoreCmsPromotion, bool>> predicate,
            Expression<Func<CoreCmsPromotion, object>> orderByExpression, OrderByType orderByType, bool isToPage = false, int pageIndex = 1,
            int pageSize = 20)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsPromotion> page;
            if (isToPage)
            {
                page = await DbClient.Queryable<CoreCmsPromotion>()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate).Select(p => new CoreCmsPromotion
                    {
                        id = p.id,
                        name = p.name,
                        type = p.type,
                        sort = p.sort,
                        parameters = p.parameters,
                        maxNums = p.maxNums,
                        maxGoodsNums = p.maxGoodsNums,
                        maxRecevieNums = p.maxRecevieNums,
                        startTime = p.startTime,
                        endTime = p.endTime,
                        isExclusive = p.isExclusive,
                        isAutoReceive = p.isAutoReceive,
                        isEnable = p.isEnable,
                        isDel = p.isDel,
                        getNumber = SqlFunc.Subqueryable<CoreCmsCoupon>().Where(o => o.promotionId == p.id).Count()
                    })
                    .Mapper(p => p.promotionCondition, p => p.promotionCondition.First().promotionId)
                    .Mapper(p => p.promotionResult, p => p.promotionResult.First().promotionId)
                    .ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsPromotion>()
                    .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                    .WhereIF(predicate != null, predicate)
                    .Take(pageSize)
                    .Select(p => new CoreCmsPromotion
                    {
                        id = p.id,
                        name = p.name,
                        type = p.type,
                        sort = p.sort,
                        parameters = p.parameters,
                        maxNums = p.maxNums,
                        maxGoodsNums = p.maxGoodsNums,
                        maxRecevieNums = p.maxRecevieNums,
                        startTime = p.startTime,
                        endTime = p.endTime,
                        isExclusive = p.isExclusive,
                        isAutoReceive = p.isAutoReceive,
                        isEnable = p.isEnable,
                        isDel = p.isDel,
                        getNumber = SqlFunc.Subqueryable<CoreCmsCoupon>().Where(o => o.promotionId == p.id).Count()
                    })
                    .Mapper(p => p.promotionCondition, p => p.promotionCondition.First().promotionId)
                    .Mapper(p => p.promotionResult, p => p.promotionResult.First().promotionId)
                    .ToListAsync();
            }

            var list = new PageList<CoreCmsPromotion>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion


    }
}
