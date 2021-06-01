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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 促销活动记录表 接口实现
    /// </summary>
    public class CoreCmsPromotionRecordServices : BaseServices<CoreCmsPromotionRecord>, ICoreCmsPromotionRecordServices
    {
        private readonly ICoreCmsPromotionRecordRepository _dal;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IServiceProvider _serviceProvider;



        public CoreCmsPromotionRecordServices(IUnitOfWork unitOfWork, ICoreCmsPromotionRecordRepository dal, IServiceProvider serviceProvider)
        {
            this._dal = dal;
            _serviceProvider = serviceProvider;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        #region 生成订单的时候，增加信息

        /// <summary>
        /// 生成订单的时候，增加信息
        /// </summary>
        /// <param name="order">订单数据</param>
        /// <param name="items">货品列表</param>
        /// <param name="groupId">秒杀团购序列</param>
        /// <param name="orderType">购物车类型</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> OrderAdd(CoreCmsOrder order, List<CoreCmsOrderItem> items, int groupId,
            int orderType)
        {
            var jm = new WebApiCallBack();

            using var container = _serviceProvider.CreateScope();

            var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();
            var goodsServices = container.ServiceProvider.GetService<ICoreCmsGoodsServices>();


            var orderItem = items.FirstOrDefault();

            //判断商品是否做团购秒杀
            if (goodsServices.IsInGroup((int)orderItem.goodsId, out var promotionsModel, groupId) == true)
            {
                jm.msg = "团购秒杀机制验证失败";

                var checkOrder = orderServices.FindLimitOrder(orderItem.productId, order.userId, promotionsModel.startTime, promotionsModel.endTime, orderType);
                if (promotionsModel.maxGoodsNums > 0)
                {
                    if (checkOrder.TotalOrders + 1 > promotionsModel.maxGoodsNums)
                    {
                        jm.data = 15610;
                        jm.msg = GlobalErrorCodeVars.Code15610;
                        return jm;
                    }
                }
                if (promotionsModel.maxNums > 0)
                {
                    if (checkOrder.TotalUserOrders > promotionsModel.maxNums)
                    {
                        jm.data = 15611;
                        jm.msg = GlobalErrorCodeVars.Code15611;
                        return jm;
                    }
                }

            }

            var model = new CoreCmsPromotionRecord();
            model.promotionId = groupId;
            model.userId = order.userId;
            model.goodsId = orderItem.goodsId;
            model.productId = orderItem.productId;
            model.orderId = order.orderId;
            model.type = orderType;

            var res = await _dal.InsertAsync(model) > 0;

            if (res == false)
            {
                jm.data = 10004;
                jm.msg = GlobalErrorCodeVars.Code10004;
                return jm;
            }

            jm.status = true;
            jm.msg = "团购秒杀机制验证通过";


            return jm;
        }

        #endregion
    }
}
