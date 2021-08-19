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
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 订单明细表 接口实现
    /// </summary>
    public class CoreCmsOrderItemServices : BaseServices<CoreCmsOrderItem>, ICoreCmsOrderItemServices
    {
        private readonly ICoreCmsOrderItemRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;

        public CoreCmsOrderItemServices(IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider,
            ICoreCmsOrderItemRepository dal)
        {
            _serviceProvider = serviceProvider;
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 发货数量
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="item">发货明细</param>
        /// <returns></returns>
        public async Task<bool> ship(string orderId, Dictionary<int, int> item)
        {
            using (var container = _serviceProvider.CreateScope())
            {
                var goodsRepository = container.ServiceProvider.GetService<ICoreCmsGoodsRepository>();

                var isOver = true;     //是否发完了，true发完了，false未发完
                var list = await base.QueryListByClauseAsync(p => p.orderId == orderId);
                foreach (var child in list)
                {
                    if (item.ContainsKey(child.productId))
                    {
                        var maxNum = child.nums - child.sendNums; //还需要减掉已发数量

                        //还需要减掉已退的数量
                        var reshipNums = _dal.GetaftersalesNums(orderId, child.sn);
                        maxNum = maxNum - reshipNums;

                        if (item[child.productId] > maxNum)  //如果发超了怎么办
                        {
                            throw new System.Exception(orderId + "的" + child.sn + "发超了");
                        }

                        if (isOver && item[child.productId] < maxNum)  //判断是否订单发完了，有一个没发完，就是未发完
                        {
                            isOver = false;
                        }

                        var updateSendNums = item[child.productId] + child.sendNums;
                        await _dal.UpdateAsync(p => new CoreCmsOrderItem() { sendNums = updateSendNums },
                            p => p.id == child.id);

                        //发货后，减库存
                        goodsRepository.ChangeStock(child.productId, GlobalEnumVars.OrderChangeStockType.send.ToString(), item[child.productId]);

                        item.Remove(child.productId);
                    }
                }
                //如果没发完，也报错
                if (item.Count > 0)
                {
                    throw new System.Exception("发货明细里包含订单之外的商品");
                }
                return isOver;
            }


        }

    }
}
