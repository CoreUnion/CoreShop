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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 广告api控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdvertController : ControllerBase
    {

        private IHttpContextUser _user;
        private readonly ICoreCmsArticleServices _articleServices;
        private readonly ICoreCmsAdvertPositionServices _advertPositionServices;
        private readonly ICoreCmsAdvertisementServices _advertisementServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="user"></param>
        /// <param name="articleServices"></param>
        /// <param name="advertPositionServices"></param>
        /// <param name="advertisementServices"></param>
        public AdvertController(IHttpContextUser user
            , ICoreCmsArticleServices articleServices
            , ICoreCmsAdvertPositionServices advertPositionServices
            , ICoreCmsAdvertisementServices advertisementServices
            )
        {
            _user = user;
            _articleServices = articleServices;
            _advertPositionServices = advertPositionServices;
            _advertisementServices = advertisementServices;
        }

        #region 获取广告列表=============================================================================
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetAdvertList([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var list = await _advertisementServices.QueryPageAsync(p => p.code == entity.where, p => p.createTime, OrderByType.Desc,
                entity.page, entity.limit);
            jm.status = true;
            jm.data = list;

            return jm;

        }
        #endregion

        #region 获取广告位置信息=============================================================================
        /// <summary>
        /// 获取广告位置信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetPositionList([FromBody] WxAdvert entity)
        {
            var jm = new WebApiCallBack();

            var position = await _advertPositionServices.QueryListByClauseAsync(p => p.isEnable && p.code == entity.codes);
            if (!position.Any())
            {
                return jm;
            }
            var ids = position.Select(p => p.id).ToList();
            var isement = await _advertisementServices.QueryListByClauseAsync(p => ids.Contains(p.positionId));

            Dictionary<string, List<CoreCmsAdvertisement>> list = new Dictionary<string, List<CoreCmsAdvertisement>>();
            list.Add(entity.codes, isement);

            jm.status = true;
            jm.data = list;

            return jm;

        }
        #endregion


    }
}
