/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/
using System.Threading.Tasks;
using AutoMapper;
using CoreCms.Net.Caching.AutoMate.RedisCache;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 演示类
    /// </summary>
    //[DisableCors]
    public class DemoController : Controller
    {
        private readonly IRedisOperationRepository _redisOperationRepository;
        private readonly ICoreCmsAgentServices _agentServices;
        private readonly ICoreCmsDistributionOrderServices _distributionOrderServices;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DemoController(IRedisOperationRepository redisOperationRepository, ICoreCmsAgentServices agentServices, ICoreCmsDistributionOrderServices distributionOrderServices)
        {
            _redisOperationRepository = redisOperationRepository;
            _agentServices = agentServices;
            _distributionOrderServices = distributionOrderServices;
        }

        /// <summary>
        /// 默认首页
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            //var msg = $"这里是一条日志{DateTime.Now}";
            //await _redisOperationRepository.ListLeftPushAsync(RedisMessageQueueKey.LogingQueue, msg);

            //return Content("已结束");

            var jm = new WebApiCallBack();

            //全部订单
            var allOrder = await _distributionOrderServices.QueryChildOrderCountAsync(10, 0);
            //一级订单
            var firstOrder = await _distributionOrderServices.QueryChildOrderCountAsync(10, 1);
            //二级订单
            var secondOrder = await _distributionOrderServices.QueryChildOrderCountAsync(10, 2);
            //本月订单
            var monthOrder = await _distributionOrderServices.QueryChildOrderCountAsync(10, 0, true);

            //全部订单金额
            var allOrderMoney = await _distributionOrderServices.QueryChildOrderMoneySumAsync(10, 0);
            //代购订单金额
            var firstOrderMoney = await _distributionOrderServices.QueryChildOrderMoneySumAsync(10, 1);
            //推广订单金额
            var secondOrderMoney = await _distributionOrderServices.QueryChildOrderMoneySumAsync(10, 2);
            //本月订单金额
            var monthOrderMoney = await _distributionOrderServices.QueryChildOrderMoneySumAsync(10, 0, true);


            jm.status = true;
            jm.data = new
            {
                allOrder,
                firstOrder,
                secondOrder,
                monthOrder,
                allOrderMoney,
                firstOrderMoney,
                secondOrderMoney,
                monthOrderMoney
            };

            return new JsonResult(jm);

        }
    }
}
