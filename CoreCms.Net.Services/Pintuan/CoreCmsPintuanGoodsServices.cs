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
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 拼团商品表 接口实现
    /// </summary>
    public class CoreCmsPinTuanGoodsServices : BaseServices<CoreCmsPinTuanGoods>, ICoreCmsPinTuanGoodsServices
    {
        private readonly ICoreCmsPinTuanGoodsRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;

        public CoreCmsPinTuanGoodsServices(IUnitOfWork unitOfWork, ICoreCmsPinTuanGoodsRepository dal,
            IServiceProvider serviceProvider
            )
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 取拼团的商品信息，增加拼团的一些属性，会显示优惠价
        /// </summary>
        /// <returns></returns>
        public async Task<CoreCmsGoods> GetGoodsInfo(int id, int userId, int pinTuanStatus = 0)
        {
            using (var container = _serviceProvider.CreateScope())
            {
                var goodsServices = container.ServiceProvider.GetService<ICoreCmsGoodsServices>();
                var pinTuanRuleServices = container.ServiceProvider.GetService<ICoreCmsPinTuanRuleServices>();
                var pinTuanRecordServices = container.ServiceProvider.GetService<ICoreCmsPinTuanRecordServices>();
                var orderServices = container.ServiceProvider.GetService<ICoreCmsOrderServices>();

                var goodsInfo = await goodsServices.GetGoodsDetial(id, userId, false);
                if (goodsInfo == null) return null;
                //把拼团的一些属性等加上
                var info =
                    await pinTuanRuleServices
                        .QueryMuchFirstAsync<CoreCmsPinTuanRule, CoreCmsPinTuanGoods, CoreCmsPinTuanRule>(
                            (join1, join2) => new object[] { JoinType.Left, join1.id == join2.ruleId },
                            (join1, join2) => join1, (join1, join2) => join2.goodsId == id && join1.isStatusOpen == true);

                if (info == null) return null;

                goodsInfo.pinTuanRule = info;
                goodsInfo.pinTuanPrice = goodsInfo.price - info.discountAmount;
                if (goodsInfo.pinTuanPrice < 0) goodsInfo.pinTuanPrice = 0;
                //取拼团记录
                goodsInfo.pinTuanRecordNums = await pinTuanRecordServices.GetCountAsync(p => p.ruleId == info.id && p.goodsId == id && p.status == pinTuanStatus);

                //判断拼团状态
                var dt = DateTime.Now;
                if (goodsInfo.pinTuanRule.startTime > dt)
                {
                    goodsInfo.pinTuanRule.pinTuanStartStatus = (int)GlobalEnumVars.PinTuanRuleStatus.notBegun;

                    TimeSpan ts = goodsInfo.pinTuanRule.startTime.Subtract(dt);
                    goodsInfo.pinTuanRule.lastTime = (int)ts.TotalSeconds;
                }
                else if (goodsInfo.pinTuanRule.startTime <= dt && goodsInfo.pinTuanRule.endTime > dt)
                {
                    goodsInfo.pinTuanRule.pinTuanStartStatus = (int)GlobalEnumVars.PinTuanRuleStatus.begin;

                    TimeSpan ts = goodsInfo.pinTuanRule.endTime.Subtract(dt);
                    goodsInfo.pinTuanRule.lastTime = (int)ts.TotalSeconds;
                }
                else
                {
                    goodsInfo.pinTuanRule.pinTuanStartStatus = (int)GlobalEnumVars.PinTuanRuleStatus.haveExpired;
                }
                //拼团记录
                //var re = await pinTuanRecordServices.GetRecord(info.id, goodsInfo.product.goodsId);
                var re = await pinTuanRecordServices.GetRecord(info.id, goodsInfo.id, pinTuanStatus);
                if (re.status)
                {
                    goodsInfo.pinTuanRecord = re.data as List<CoreCmsPinTuanRecord>;
                }

                var checkOrder = orderServices.FindLimitOrder(goodsInfo.product.id, userId, info.startTime, info.endTime, 0);
                if (info.maxGoodsNums > 0)
                {
                    goodsInfo.stock = info.maxGoodsNums;
                    //活动销售件数
                    goodsInfo.product.stock = info.maxGoodsNums - checkOrder.TotalOrders;
                    //goodsInfo.buyCount = info.maxGoodsNums - checkOrder.TotalOrders;
                    goodsInfo.buyPinTuanCount = checkOrder.TotalOrders;
                }
                else
                {
                    //goodsInfo.buyCount = info.maxGoodsNums - checkOrder.TotalOrders;
                    goodsInfo.buyPinTuanCount = checkOrder.TotalOrders;

                }
                return goodsInfo;
            }
        }



    }
}
