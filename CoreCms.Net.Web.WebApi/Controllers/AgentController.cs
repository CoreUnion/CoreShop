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
using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.Configuration;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 代理请求接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private IHttpContextUser _user;
        private readonly ICoreCmsAgentServices _agentServices;
        private readonly ICoreCmsAgentOrderServices _agentOrderServices;
        private readonly ICoreCmsAgentGoodsServices _agentGoodsServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsGoodsServices _goodsServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="user"></param>
        /// <param name="agentServices"></param>
        /// <param name="settingServices"></param>
        /// <param name="agentOrderServices"></param>
        /// <param name="userServices"></param>
        /// <param name="goodsServices"></param>
        /// <param name="agentGoodsServices"></param>
        public AgentController(IHttpContextUser user, ICoreCmsAgentServices agentServices, ICoreCmsSettingServices settingServices, ICoreCmsAgentOrderServices agentOrderServices, ICoreCmsUserServices userServices, ICoreCmsGoodsServices goodsServices, ICoreCmsAgentGoodsServices agentGoodsServices)
        {
            _user = user;
            _agentServices = agentServices;
            _settingServices = settingServices;
            _agentOrderServices = agentOrderServices;
            _userServices = userServices;
            _goodsServices = goodsServices;
            _agentGoodsServices = agentGoodsServices;
        }

        //公共接口====================================================================================================

        #region 获取店铺信息
        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetStoreInfo([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            if (entity.id == 0)
            {
                jm.msg = "店铺信息丢失";
                return jm;
            }
            var store = UserHelper.GetUserIdByShareCode(entity.id);
            if (store <= 0)
            {
                jm.msg = "店铺信息丢失";
                return jm;
            }
            jm = await _agentServices.GetStore(store);
            return jm;

        }
        #endregion


        #region 根据查询条件获取分页数据============================================================
        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetGoodsPageList([FromBody] FMPageByWhereOrder entity)
        {
            var jm = new WebApiCallBack();

            var where = PredicateBuilder.True<CoreCmsGoods>();
            where = where.And(p => p.isDel == false);
            where = where.And(p => p.isMarketable == true);

            var className = string.Empty;
            if (!string.IsNullOrEmpty(entity.where))
            {
                var obj = JsonConvert.DeserializeAnonymousType(entity.where, new
                {
                    priceFrom = "",
                    priceTo = "",
                    catId = "",
                    brandId = "",
                    labelId = "",
                    searchName = "",
                });

                if (!string.IsNullOrEmpty(obj.priceFrom))
                {
                    var priceF = obj.priceFrom.ObjectToDouble(0);
                    if (priceF >= 0)
                    {
                        var f = Convert.ToDecimal(priceF);
                        where = where.And(p => p.price >= f);
                    }
                }
                if (!string.IsNullOrEmpty(obj.priceTo))
                {
                    var priceT = obj.priceTo.ObjectToDouble(0);
                    if (priceT >= 0)
                    {
                        var f = Convert.ToDecimal(priceT);
                        where = where.And(p => p.price <= f);
                    }
                }
                if (!string.IsNullOrEmpty(obj.brandId))
                {
                    var brandId = obj.brandId.ObjectToInt(0);
                    if (brandId >= 0)
                    {
                        where = where.And(p => p.brandId == brandId);
                    }
                }
                if (!string.IsNullOrEmpty(obj.labelId))
                {
                    var brandId = obj.brandId.ObjectToInt(0);
                    if (brandId >= 0)
                    {
                        where = where.And(p => p.brandId == brandId);
                    }
                }
                if (!string.IsNullOrEmpty(obj.searchName))
                {
                    where = where.And(p => p.name.Contains(obj.searchName));
                }
            }

            var orderBy = " isRecommend desc,isHot desc";
            if (!string.IsNullOrEmpty(entity.order))
            {
                orderBy += "," + entity.order;
            }

            var list = await _goodsServices.QueryAgentGoodsPageAsync(where, orderBy, entity.page, entity.limit, false);
            if (list.Any())
            {
                foreach (var goods in list)
                {
                    goods.images = !string.IsNullOrEmpty(goods.images) ? goods.images.Split(",")[0] : "/static/images/common/empty.png";
                }
            }

            //返回数据
            jm.status = true;
            jm.data = new
            {
                list,
                className,
                entity.page,
                list.TotalCount,
                list.TotalPages,
                entity.limit,
                entity.where,
                entity.order,
            };
            jm.msg = "数据调用成功!";

            return jm;
        }
        #endregion



        //验证接口====================================================================================================

        #region 查询用户是否可以成为代理商
        /// <summary>
        /// 查询用户是否可以成为代理商
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> Info()
        {
            var jm = await _agentServices.GetInfo(_user.ID);
            return jm;

        }
        #endregion

        #region 申请成为代理商接口
        /// <summary>
        /// 申请成为代理商接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> ApplyAgent([FromBody] FMAgentApply entity)
        {
            var jm = new WebApiCallBack();

            if (entity.agreement != "on")
            {
                jm.msg = "请勾选代理商协议";
                return jm;
            }
            var iData = new CoreCmsAgent();
            iData.mobile = entity.mobile;
            iData.name = entity.name;
            iData.weixin = entity.weixin;
            iData.qq = entity.qq;
            jm = await _agentServices.AddData(iData, _user.ID);

            return jm;

        }
        #endregion

        #region 获取我的下级用户数量
        /// <summary>
        /// 获取我的下级用户数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetTeamSum()
        {
            var jm = new WebApiCallBack();

            //发展人数
            var first = await _userServices.QueryChildCountAsync(_user.ID, 1);
            //订单数
            var second = await _agentOrderServices.GetCountAsync(p => p.userId == _user.ID);

            //当月发展人数
            var monthFirst = await _userServices.QueryChildCountAsync(_user.ID, 1, true);

            DateTime dt = DateTime.Now;
            //本月第一天时间      
            DateTime dtFirst = dt.AddDays(1 - (dt.Day));
            dtFirst = new DateTime(dtFirst.Year, dtFirst.Month, dtFirst.Day, 0, 0, 0);
            //获得某年某月的天数    
            int year = dt.Date.Year;
            int month = dt.Date.Month;
            int dayCount = DateTime.DaysInMonth(year, month);
            //本月最后一天时间    
            DateTime dtLast = dtFirst.AddDays(dayCount - 1);

            var monthSecond = await _agentOrderServices.GetCountAsync(p => p.userId == _user.ID && p.createTime > dtFirst && p.createTime < dtLast, true);

            jm.status = true;
            jm.data = new
            {
                count = first + second,
                first,
                second,
                monthCount = monthFirst + monthSecond,
                monthFirst,
                monthSecond
            };

            return jm;
        }

        #endregion

        #region 获取我的订单统计
        /// <summary>
        /// 获取我的订单统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetOrderSum()
        {
            var jm = new WebApiCallBack();

            DateTime dt = DateTime.Now;
            //本月第一天时间      
            DateTime dtFirst = dt.AddDays(1 - (dt.Day));
            dtFirst = new DateTime(dtFirst.Year, dtFirst.Month, dtFirst.Day, 0, 0, 0);
            //获得某年某月的天数    
            int dayCount = DateTime.DaysInMonth(dt.Date.Year, dt.Date.Month);
            //本月最后一天时间    
            DateTime dtLast = dtFirst.AddDays(dayCount - 1);


            //全部订单
            var allOrder = await _agentOrderServices.GetCountAsync(p => p.userId == _user.ID, true);
            //代购订单
            var procurementServiceOrder = await _agentOrderServices.GetCountAsync(p => p.userId == _user.ID && p.buyUserId == _user.ID, true);
            //推广订单
            var customerOrder = await _agentOrderServices.GetCountAsync(p => p.userId == _user.ID && p.buyUserId != _user.ID, true);
            //本月订单
            var monthOrder = await _agentOrderServices.GetCountAsync(p => p.userId == _user.ID && p.createTime > dtFirst && p.createTime < dtLast, true);


            //全部订单金额
            var allOrderMoney = await _agentOrderServices.GetSumAsync(p => p.userId == _user.ID, p => p.amount, true);
            //代购订单金额
            var procurementServiceOrderMoney = await _agentOrderServices.GetSumAsync(p => p.userId == _user.ID && p.buyUserId == _user.ID, p => p.amount, true);
            //推广订单金额
            var customerOrderMoney = await _agentOrderServices.GetSumAsync(p => p.userId == _user.ID && p.buyUserId != _user.ID, p => p.amount, true);
            //本月订单金额
            var monthOrderMoney = await _agentOrderServices.GetSumAsync(p => p.userId == _user.ID && p.createTime > dtFirst && p.createTime < dtLast, p => p.amount, true);

            jm.status = true;
            jm.data = new
            {
                allOrder,
                procurementServiceOrder,
                customerOrder,
                monthOrder,
                allOrderMoney,
                procurementServiceOrderMoney,
                customerOrderMoney,
                monthOrderMoney
            };

            return jm;
        }

        #endregion

        #region 我推广的订单
        /// <summary>
        /// 我推广的订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> MyOrder([FromBody] FMPageByIntId entity)
        {
            var jm = await _agentServices.GetMyOrderList(_user.ID, entity.page, entity.limit, entity.id);
            return jm;
        }
        #endregion

        #region 店铺设置
        /// <summary>
        /// 店铺设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> SetStore([FromBody] FMSetAgentStorePost entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.storeName))
            {
                jm.msg = "请填写店铺名称";
                return jm;
            }
            if (string.IsNullOrEmpty(entity.storeLogo))
            {
                jm.msg = "请上传店铺logo";
                return jm;
            }
            if (string.IsNullOrEmpty(entity.storeBanner))
            {
                jm.msg = "请上传店铺banner";
                return jm;
            }

            var info = await _agentServices.QueryByClauseAsync(p => p.userId == _user.ID);
            if (info != null)
            {
                info.storeLogo = entity.storeLogo;
                info.storeBanner = entity.storeBanner;
                info.storeDesc = entity.storeDesc;
                info.storeName = entity.storeName;
                await _agentServices.UpdateAsync(info);
            }
            jm.status = true;
            jm.msg = "保存成功";

            return jm;

        }
        #endregion

        #region 获取代理商排行
        /// <summary>
        /// 获取代理商排行
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetAgentRanking([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var list = await _agentServices.QueryRankingPageAsync(entity.page, entity.limit);

            jm.status = true;
            jm.data = new
            {
                data = list,
                list.HasNextPage,
                list.HasPreviousPage,
                list.PageIndex,
                list.PageSize,
                list.TotalPages,
                list.TotalCount,

            };

            return jm;
        }

        #endregion
    }
}
