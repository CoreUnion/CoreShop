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
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 团购调用接口数据
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {

        private readonly IHttpContextUser _user;
        private readonly ICoreCmsPromotionServices _coreCmsPromotionServices;
        private ICoreCmsGoodsServices _goodsServices;


        /// <summary>
        /// 构造函数
        /// </summary>
        public GroupController(IHttpContextUser user, ICoreCmsPromotionServices coreCmsPromotionServices, ICoreCmsGoodsServices goodsServices)
        {
            _user = user;
            _coreCmsPromotionServices = coreCmsPromotionServices;
            _goodsServices = goodsServices;
        }


        //公共接口====================================================================================================

        #region 获取秒杀团购列表===========================================================
        /// <summary>
        /// 获取秒杀团购列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetList([FromBody] FMGroupGetListPost entity)
        {
            var jm = await _coreCmsPromotionServices.GetGroupList(entity.type, _user.ID, entity.status, entity.page, entity.limit);

            return jm;
        }
        #endregion

        #region 获取秒杀团购详情===========================================================
        /// <summary>
        /// 获取秒杀团购详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetGoodsDetial([FromBody] FMGetGoodsDetial entity)
        {
            var jm = await _coreCmsPromotionServices.GetGroupDetail(entity.id, 0, "group", entity.groupId);
            return jm;
        }
        #endregion

        //验证接口====================================================================================================


    }
}
