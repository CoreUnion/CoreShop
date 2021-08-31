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
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 拼团规则表 接口实现
    /// </summary>
    public class CoreCmsPinTuanRuleServices : BaseServices<CoreCmsPinTuanRule>, ICoreCmsPinTuanRuleServices
    {
        private readonly ICoreCmsPinTuanRuleRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;

        public CoreCmsPinTuanRuleServices(IUnitOfWork unitOfWork, ICoreCmsPinTuanRuleRepository dal,
            IServiceProvider serviceProvider
            )
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// 取购物车数据的时候，更新价格
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public WebApiCallBack PinTuanInfo(List<CartProducts> list)
        {
            return _dal.PinTuanInfo(list);
        }


        /// <summary>
        /// 接口上获取拼团所有商品
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetPinTuanList(int id = 0, int userId = 0)
        {
            var jm = new WebApiCallBack();

            using var container = _serviceProvider.CreateScope();
            var pinTuanGoodsServices = container.ServiceProvider.GetService<ICoreCmsPinTuanGoodsServices>();
            var pinTuanRuleServices = container.ServiceProvider.GetService<ICoreCmsPinTuanRuleServices>();

            var dt = DateTime.Now;
            var where = PredicateBuilder.True<CoreCmsPinTuanRule>();
            @where = @where.And(p => p.startTime < dt);
            @where = @where.And(p => p.endTime > dt);
            if (id != 0)
            {
                @where = @where.And(p => p.id == id);
            }

            var list = await pinTuanRuleServices.QueryListByClauseAsync(@where, p => p.sort, OrderByType.Asc);
            var goodIds = new List<int>();
            if (list != null && list.Any())
            {
                var ruleIds = list.Select(p => p.id).ToList();
                var goods = await pinTuanGoodsServices.QueryListByClauseAsync(p => ruleIds.Contains(p.ruleId));
                if (goods != null && goods.Any())
                {
                    goodIds = goods.Select(p => p.goodsId).ToList();
                }
            }

            if (goodIds.Any())
            {
                var goods = new List<CoreCmsGoods>();
                foreach (var goodId in goodIds)
                {
                    var g = await pinTuanGoodsServices.GetGoodsInfo(goodId, userId);
                    if (g != null)
                    {
                        goods.Add(g);
                    }
                }
                jm.data = goods;
            }

            jm.status = true;
            return jm;
        }


        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <returns></returns>
        public async Task<IPageList<TagPinTuanResult>> QueryTagPinTuanPageAsync(Expression<Func<TagPinTuanResult, bool>> predicate,
            Expression<Func<TagPinTuanResult, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1, int pageSize = 20)
        {
            return await _dal.QueryTagPinTuanPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize);
        }



        /// <summary>
        ///     根据商品id获取拼团规则信息
        /// </summary>
        /// <param name="goodId"></param>
        /// <returns></returns>

        public async Task<TagPinTuanResult> GetPinTuanInfo(int goodId)
        {
            return await _dal.GetPinTuanInfo(goodId);
        }
    }
}
