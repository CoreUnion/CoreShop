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
using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    ///     分销请求接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DistributionController : ControllerBase
    {
        private readonly ICoreCmsDistributionOrderServices _distributionOrderServices;
        private readonly ICoreCmsDistributionServices _distributionServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly IHttpContextUser _user;

        /// <summary>
        ///     构造函数
        /// </summary>
        public DistributionController(IHttpContextUser user, ICoreCmsDistributionServices distributionServices,
            ICoreCmsSettingServices settingServices, ICoreCmsUserServices userServices,
            ICoreCmsDistributionOrderServices distributionOrderServices)
        {
            _user = user;
            _distributionServices = distributionServices;
            _settingServices = settingServices;
            _userServices = userServices;
            _distributionOrderServices = distributionOrderServices;
        }

        //公共接口====================================================================================================

        #region 获取店铺信息

        /// <summary>
        ///     获取店铺信息
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

            jm = await _distributionServices.GetStore(store);
            return jm;
        }

        #endregion

        //验证接口====================================================================================================

        #region 查询用户是否可以成为分销商

        /// <summary>
        ///     查询用户是否可以成为分销商
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> Info()
        {
            var jm = await _distributionServices.GetInfo(_user.ID, true);
            return jm;
        }

        #endregion

        #region 申请成为分销商接口

        /// <summary>
        ///     申请成为分销商接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> ApplyDistribution([FromBody] FMDistributionApply entity)
        {
            var jm = new WebApiCallBack();

            if (entity.agreement != "on")
            {
                jm.msg = "请勾选分销协议";
                return jm;
            }

            var iData = new CoreCmsDistribution();
            iData.mobile = entity.mobile;
            iData.name = entity.name;
            iData.weixin = entity.weixin;
            iData.qq = entity.qq;
            jm = await _distributionServices.AddData(iData, _user.ID);

            return jm;
        }

        #endregion

        #region 我推广的订单

        /// <summary>
        ///     我推广的订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> MyOrder([FromBody] FMPageByIntId entity)
        {
            var jm = await _distributionServices.GetMyOrderList(_user.ID, entity.page, entity.limit, entity.id);
            return jm;
        }

        #endregion

        #region 店铺设置

        /// <summary>
        ///     店铺设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> SetStore([FromBody] FMSetDistributionStorePost entity)
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

            var info = await _distributionServices.QueryByClauseAsync(p => p.userId == _user.ID);
            if (info != null)
            {
                info.storeLogo = entity.storeLogo;
                info.storeBanner = entity.storeBanner;
                info.storeDesc = entity.storeDesc;
                info.storeName = entity.storeName;
                await _distributionServices.UpdateAsync(info);
            }

            jm.status = true;
            jm.msg = "保存成功";

            return jm;
        }

        #endregion

        #region 获取我的订单统计

        /// <summary>
        ///     获取我的订单统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetOrderSum()
        {
            var jm = new WebApiCallBack();

            //全部订单
            var allOrder = await _distributionOrderServices.QueryChildOrderCountAsync(_user.ID, 0);
            //一级订单
            var firstOrder = await _distributionOrderServices.QueryChildOrderCountAsync(_user.ID);
            //二级订单
            var secondOrder = await _distributionOrderServices.QueryChildOrderCountAsync(_user.ID, 2);
            //本月订单
            var monthOrder = await _distributionOrderServices.QueryChildOrderCountAsync(_user.ID, 0, true);

            //全部订单金额
            var allOrderMoney = await _distributionOrderServices.QueryChildOrderMoneySumAsync(_user.ID, 0);
            //代购订单金额
            var firstOrderMoney = await _distributionOrderServices.QueryChildOrderMoneySumAsync(_user.ID);
            //推广订单金额
            var secondOrderMoney = await _distributionOrderServices.QueryChildOrderMoneySumAsync(_user.ID, 2);
            //本月订单金额
            var monthOrderMoney = await _distributionOrderServices.QueryChildOrderMoneySumAsync(_user.ID, 0, true);


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

            return jm;
        }

        #endregion

        #region 获取我的下级用户数量

        /// <summary>
        ///     获取我的下级用户数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetTeamSum()
        {
            var jm = new WebApiCallBack();

            //一级统计人数
            var first = await _userServices.QueryChildCountAsync(_user.ID);
            //二级发展人数
            var second = await _userServices.QueryChildCountAsync(_user.ID, 2);

            //当月发展一级人数
            var monthFirst = await _userServices.QueryChildCountAsync(_user.ID, 1, true);
            //当月发展二级分数
            var monthSecond = await _userServices.QueryChildCountAsync(_user.ID, 2, true);

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

        #region 获取分销商排行

        /// <summary>
        ///     获取分销商排行
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetDistributionRanking([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var list = await _distributionServices.QueryRankingPageAsync(entity.page, entity.limit);

            jm.status = true;
            jm.data = new
            {
                data = list,
                list.HasNextPage,
                list.HasPreviousPage,
                list.PageIndex,
                list.PageSize,
                list.TotalPages,
                list.TotalCount
            };

            return jm;
        }

        #endregion
    }
}