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
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 门店调用接口数据
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IHttpContextUser _user;
        private readonly ICoreCmsStoreServices _storeServices;
        private readonly ICoreCmsClerkServices _clerkServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsBillLadingServices _billLadingServices;
        private readonly ICoreCmsOrderServices _orderServices;


        /// <summary>
        /// 构造函数
        /// </summary>
        public StoreController(IHttpContextUser user
            , ICoreCmsStoreServices storeServices
            , ICoreCmsClerkServices clerkServices
            , ICoreCmsSettingServices settingServices
            , ICoreCmsBillLadingServices billLadingServices, ICoreCmsOrderServices orderServices)
        {
            _user = user;
            _storeServices = storeServices;
            _clerkServices = clerkServices;
            _settingServices = settingServices;
            _billLadingServices = billLadingServices;
            _orderServices = orderServices;
        }

        //公共接口======================================================================================================

        #region 获取默认的门店
        /// <summary>
        /// 获取默认的门店
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetDefaultStore()
        {
            var jm = new WebApiCallBack();

            var ship = await _storeServices.QueryByClauseAsync(p => p.isDefault == true);
            jm.status = true;
            jm.data = ship;

            return jm;
        }
        #endregion

        #region 获取门店列表数据
        /// <summary>
        /// 获取门店列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetStoreList([FromBody] FMGetStoreQueryPageByCoordinate entity)
        {
            var jm = new WebApiCallBack();
            try
            {
                var where = PredicateBuilder.True<CoreCmsStore>();

                if (!string.IsNullOrEmpty(entity.key))
                {
                    where = where.And(p => p.storeName.Contains(entity.key));
                }

                jm.status = true;

                var data = await _storeServices.QueryPageAsyncByCoordinate(where, p => p.distance, OrderByType.Asc, entity.page, entity.limit, entity.latitude, entity.longitude);

                foreach (var item in data)
                {
                    if (item.distance > 0)
                    {
                        if (item.distance > 1000)
                        {
                            item.distanceStr = Math.Round(item.distance / 1000, 2) + "km";
                        }
                        else
                        {
                            item.distanceStr = Math.Round(item.distance, 2) + "m";
                        }
                    }
                    else
                    {
                        item.distanceStr = "未知";
                    }
                }
                jm.data = data;
                jm.otherData = new
                {
                    totalCount = data.TotalCount,
                    totalPages = data.TotalPages,
                };
            }
            catch (Exception e)
            {

                jm.msg = GlobalConstVars.DataHandleEx;
                jm.data = e.ToString();
            }
            return jm;
        }
        #endregion

        #region 获取推荐关键词
        /// <summary>
        /// 获取推荐关键词
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetRecommendKeys()
        {
            var jm = new WebApiCallBack();

            var allConfigs = await _settingServices.GetConfigDictionaries();
            var recommendKeysStr = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.RecommendKeys);
            jm.status = true;
            jm.msg = "获取成功";
            jm.data = !string.IsNullOrEmpty(recommendKeysStr) ? recommendKeysStr.Split("|") : new string[] { };

            return jm;
        }
        #endregion

        #region 判断是否开启门店自提
        /// <summary>
        /// 判断是否开启门店自提
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetStoreSwitch()
        {
            var jm = new WebApiCallBack { status = true, msg = "获取成功" };

            var allConfigs = await _settingServices.GetConfigDictionaries();
            jm.data = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.StoreSwitch).ObjectToInt(2); ;
            return jm;
        }
        #endregion

        #region 根据序列获取门店数据
        /// <summary>
        /// 根据序列获取门店数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetStoreById([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack
            {
                status = true,
                msg = "获取成功",
                data = await _storeServices.QueryByClauseAsync(p => p.id == entity.id)
            };
            return jm;
        }
        #endregion

        //验证接口======================================================================================================

        #region 判断访问用户是否是店员
        /// <summary>
        /// 判断访问用户是否是店员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> IsClerk()
        {
            var jm = await _clerkServices.IsClerk(_user.ID);
            return jm;
        }
        #endregion

        #region 根据用户序列获取门店数据
        /// <summary>
        /// 根据用户序列获取门店数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetStoreByUserId()
        {
            var jm = new WebApiCallBack
            {
                status = true,
                msg = "获取成功",
                data = await _storeServices.GetStoreByUserId(_user.ID)
            };
            return jm;
        }
        #endregion

        #region 获取个人订单列表

        /// <summary>
        /// 获取个人订单列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetOrderPageByMerchant([FromBody] GetOrderPageByMerchantPost entity)
        {
            var jm = new WebApiCallBack();

            var store = await _storeServices.GetStoreByUserId(_user.ID);
            if (store != null)
            {
                jm = await _orderServices.GetOrderPageByMerchant(entity.dateType, entity.date, entity.status, entity.storeId, entity.page, entity.limit);
            }
            else
            {
                jm.status = false;
                jm.msg = "你不是店员";
            }

            return jm;
        }

        #endregion

        #region 搜索订单

        /// <summary>
        /// 搜索订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetOrderPageByMerchantSearch([FromBody] GetOrderPageByMerchantSearcgPost entity)
        {
            var jm = new WebApiCallBack();

            var store = await _storeServices.GetStoreByUserId(_user.ID);
            if (store != null)
            {
                jm = await _orderServices.GetOrderPageByMerchantSearch(entity.keyword, entity.status, entity.receiptType, entity.storeId, entity.page, entity.limit);
            }
            else
            {
                jm.status = false;
                jm.msg = "你不是店员";
            }

            return jm;
        }

        #endregion


        #region 店铺提货单列表
        /// <summary>
        /// 店铺提货单列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> StoreLadingList([FromBody] FMPageByIntId entity)
        {
            var jm = await _billLadingServices.GetStoreLadingList(_user.ID, entity.page, entity.limit);
            return jm;
        }
        #endregion

        #region 删除提货单数据
        /// <summary>
        /// 删除提货单数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> LadingDelete([FromBody] FMStringId entity)
        {
            var jm = await _billLadingServices.LadingDelete(entity.id, _user.ID);
            return jm;
        }
        #endregion

        #region 获取单个提货单详情
        /// <summary>
        /// 获取单个提货单详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> LadingInfo([FromBody] FMStringId entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.id))
            {
                jm.msg = "请提交查询数据关键词";
                return jm;
            }
            jm = await _billLadingServices.GetInfo(entity.id, _user.ID);

            return jm;
        }
        #endregion

        #region 核销订单
        /// <summary>
        /// 核销订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> Lading([FromBody] FMStringId entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.id))
            {
                jm.msg = "请提交查询数据关键词";
                return jm;
            }
            var array = entity.id.Split(",");
            var result = await _billLadingServices.LadingOperating(array, _user.ID);
            jm.status = result.code == 0;
            jm.msg = result.msg;

            return jm;
        }
        #endregion


    }
}