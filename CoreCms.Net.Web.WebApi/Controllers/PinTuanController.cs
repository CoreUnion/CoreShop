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
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 拼团接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PinTuanController : ControllerBase
    {

        private readonly IHttpContextUser _user;
        private readonly ICoreCmsPinTuanGoodsServices _pinTuanGoodsServices;
        private readonly ICoreCmsPinTuanRuleServices _pinTuanRuleServices;
        private readonly ICoreCmsProductsServices _productsServices;
        private readonly ICoreCmsPinTuanRecordServices _pinTuanRecordServices;
        private readonly ICoreCmsGoodsServices _goodsServices;


        /// <summary>
        /// 构造函数
        /// </summary>
        public PinTuanController(IHttpContextUser user
            , ICoreCmsPinTuanGoodsServices pinTuanGoodsServices
            , ICoreCmsPinTuanRuleServices pinTuanRuleServices
            , ICoreCmsProductsServices productsServices
            , ICoreCmsPinTuanRecordServices pinTuanRecordServices, ICoreCmsGoodsServices goodsServices)
        {
            _user = user;
            _pinTuanGoodsServices = pinTuanGoodsServices;
            _pinTuanRuleServices = pinTuanRuleServices;
            _productsServices = productsServices;
            _pinTuanRecordServices = pinTuanRecordServices;
            _goodsServices = goodsServices;
        }


        #region 拼团列表
        /// <summary>
        /// 拼团列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetList([FromBody] FMIntId entity)
        {
            WebApiCallBack jm;

            var userId = 0;
            if (_user != null)
            {
                userId = _user.ID;
            }
            var id = 0;
            if (entity.id > 0)
            {
                id = entity.id;
            }
            jm = await _pinTuanRuleServices.GetPinTuanList(id, userId);
            return jm;

        }

        #endregion

        #region 获取拼团商品信息
        /// <summary>
        /// 获取拼团商品信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetGoodsInfo([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            var userId = 0;
            if (_user != null)
            {
                userId = _user.ID;
            }
            var pinTuanStatus = entity.data.ObjectToInt(1);

            jm.status = true;
            jm.msg = "获取详情成功";
            jm.data = await _pinTuanGoodsServices.GetGoodsInfo(entity.id, userId, pinTuanStatus);

            return jm;

        }

        #endregion

        #region 获取货品信息
        /// <summary>
        /// 获取货品信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetProductInfo([FromBody] FMGetProductInfo entity)
        {
            var jm = new WebApiCallBack();

            var products = await _productsServices.GetProductInfo(entity.id, false, 0, entity.type);
            if (products == null)
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }
            //把拼团的一些属性等加上
            var info = await _pinTuanRuleServices.QueryMuchFirstAsync<CoreCmsPinTuanRule, CoreCmsPinTuanGoods, CoreCmsPinTuanRule>(
                        (join1, join2) => new object[] { JoinType.Left, join1.id == join2.ruleId },
                        (join1, join2) => join1, (join1, join2) => join2.goodsId == products.goodsId);

            if (info == null)
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                return jm;
            }
            products.pinTuanRule = info;
            jm.status = true;
            jm.data = products;
            return jm;

        }

        #endregion

        #region 根据订单id取拼团信息，用在订单详情页
        /// <summary>
        /// 根据订单id取拼团信息，用在订单详情页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetPinTuanTeam([FromBody] FMGetPinTuanTeamPost entity)
        {
            var jm = new WebApiCallBack();

            if (string.IsNullOrEmpty(entity.orderId) && entity.teamId == 0)
            {
                jm.msg = GlobalErrorCodeVars.Code15606;
                return jm;
            }
            jm = await _pinTuanRecordServices.GetTeamList(entity.teamId, entity.orderId);

            return jm;
        }

        #endregion

    }
}
