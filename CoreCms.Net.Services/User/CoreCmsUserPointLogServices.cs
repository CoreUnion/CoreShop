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
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 用户积分记录表 接口实现
    /// </summary>
    public class CoreCmsUserPointLogServices : BaseServices<CoreCmsUserPointLog>, ICoreCmsUserPointLogServices
    {
        private readonly ICoreCmsUserPointLogRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;

        public CoreCmsUserPointLogServices(IUnitOfWork unitOfWork, ICoreCmsUserPointLogRepository dal,
            IServiceProvider serviceProvider
            )
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 积分消费日志设置
        /// </summary>
        /// <param name="userId">用户</param>
        /// <param name="num"></param>
        /// <param name="type"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SetPoint(int userId, int num, int type, string remarks)
        {
            using (var container = _serviceProvider.CreateScope())
            {
                var userServices = container.ServiceProvider.GetService<ICoreCmsUserServices>();

                var res = new WebApiCallBack() { methodDescription = "积分消费日志设置" };
                //获取积分账号信息
                var userModel = await userServices.QueryByIdAsync(userId);
                if (userModel == null)
                {
                    res.msg = "更新消费积分日志获取用户信息失败";
                    return res;
                }

                var newPoint = num + userModel.point;
                //积分余额判断
                if (newPoint < 0)
                {
                    res.msg = "积分余额不足";
                    return res;
                }
                //插入记录
                var log = new CoreCmsUserPointLog();
                log.userId = userId;
                log.type = type;
                log.num = num;
                log.balance = newPoint;
                log.remarks = remarks;
                log.createTime = DateTime.Now;

                await _dal.InsertAsync(log);
                await userServices.UpdateAsync(p => new CoreCmsUser() { point = p.point + num }, p => p.id == userModel.id);


                res.status = true;
                res.msg = "积分更改成功";
                return res;
            }
        }

        /// <summary>
        /// 订单完成送积分操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="money"></param>
        /// <param name="orderId"></param>
        public async Task OrderComplete(int userId, decimal money, string orderId)
        {
            using (var container = _serviceProvider.CreateScope())
            {
                var _settingServices = container.ServiceProvider.GetService<ICoreCmsSettingServices>();
                var allConfigs = await _settingServices.GetConfigDictionaries();

                var ordersRewardProportion = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.OrdersRewardProportion).ObjectToInt();
                if (ordersRewardProportion > 0)
                {
                    var point = Convert.ToInt32(money / ordersRewardProportion);
                    await SetPoint(userId, point, (int)GlobalEnumVars.UserPointSourceTypes.PointTypeRebate,
                          "订单：" + orderId + " 积分奖励");

                }
            }
        }


        /// <summary>
        /// 判断今天是否签到
        /// </summary>
        /// <param name="userId"></param>
        public async Task<WebApiCallBack> IsSign(int userId)
        {
            var jm = new WebApiCallBack();
            jm.msg = "今天还没有签到";

            var where = PredicateBuilder.True<CoreCmsUserPointLog>();
            where = where.And(p => p.userId == userId && p.type == (int)GlobalEnumVars.UserPointSourceTypes.PointTypeSign);

            var dt = DateTime.Now;
            var startTime = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            var endTime = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);

            where = where.And(p => p.createTime > startTime && p.createTime < endTime);

            var bl = await _dal.ExistsAsync(where);
            jm.status = bl;
            jm.msg = bl ? "今天已经签到了" : "今天还没有签到";

            return jm;
        }


        /// <summary>
        /// 用户签到
        /// </summary>
        /// <param name="userId"></param>
        public async Task<WebApiCallBack> Sign(int userId)
        {
            using (var container = _serviceProvider.CreateScope())
            {
                var _settingServices = container.ServiceProvider.GetService<ICoreCmsSettingServices>();
                var jm = new WebApiCallBack();

                var res = await IsSign(userId);
                if (res.status)
                {
                    jm.msg = "今天已经签到，无需重复签到";
                }
                //获取店铺签到积分设置
                var allConfigs = await _settingServices.GetConfigDictionaries();

                var signPointType = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SignPointType).ObjectToInt();

                //判断是固定积分计算还是随机积分计算
                var point = 0;
                if (signPointType == (int)GlobalEnumVars.UserPointSignTypes.RandomPoint)
                {
                    //随机计算
                    //获取最小随机值
                    var signRandomMin = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SignRandomMin).ObjectToInt(1);
                    //获取最大随机值
                    var signRandomMax = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SignRandomMax).ObjectToInt(10);
                    Random ran = new Random();
                    point = ran.Next(signRandomMin, signRandomMax);
                }
                else
                {
                    //固定计算
                    //首次签到积分
                    var firstSignPoint = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.FirstSignPoint).ObjectToInt(1);
                    //连续签到追加
                    var continuitySignAdditional = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.ContinuitySignAdditional).ObjectToInt(1);
                    //签到最多积分
                    var signMostPoint = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.SignMostPoint).ObjectToInt(1);

                    //最大连续签到天数
                    var maxContinuityDay = 0;

                    //获取连续签到天数
                    if (continuitySignAdditional > 0)
                    {
                        maxContinuityDay = (signMostPoint - firstSignPoint) / continuitySignAdditional;
                    }
                    else
                    {
                        //连续追加0的话说明每天签到积分都一样多，那么最大连续签到天数就是1天
                        maxContinuityDay = 1;
                    }

                    var day = DateTime.Now.AddDays(-maxContinuityDay);


                    var logs = await _dal.QueryListByClauseAsync(p =>
                         p.userId == userId && p.type == (int)(int)GlobalEnumVars.UserPointSourceTypes.PointTypeSign &&
                         p.createTime > day);

                    var newRes = new List<string>();
                    if (logs != null && logs.Any())
                    {
                        foreach (var item in logs)
                        {
                            var dtStr = item.createTime.ToString("yyyy-MM-dd");
                            if (!newRes.Contains(dtStr))
                            {
                                newRes.Add(dtStr);
                            }
                        }
                    }

                    var intDay = 0; //连续签到天数
                    for (int i = 1; i <= maxContinuityDay; i++)
                    {
                        var now = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd"); ;
                        if (newRes.Contains(now))
                        {
                            intDay++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    //积分
                    point = firstSignPoint + continuitySignAdditional * intDay;
                    point = point > signMostPoint ? signMostPoint : point;
                }
                jm.data = point;
                //插入数据库
                var result = await SetPoint(userId, point, (int)GlobalEnumVars.UserPointSourceTypes.PointTypeSign, "积分签到，获得" + point + "积分");
                jm.msg = result.msg;
                jm.status = result.status;
                return jm;
            }
        }
    }
}
