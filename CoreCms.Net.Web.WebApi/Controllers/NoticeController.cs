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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 公告控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NoticeController : ControllerBase
    {
        private IHttpContextUser _user;
        private ICoreCmsNoticeServices _noticeServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="user"></param>
        /// <param name="noticeServices"></param>
        public NoticeController(IHttpContextUser user, ICoreCmsNoticeServices noticeServices)
        {
            _user = user;
            _noticeServices = noticeServices;
        }


        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> NoticeList([FromBody] FMPageByIntId entity)
        {
            var jm = new WebApiCallBack();

            var list = await _noticeServices.QueryPageAsync(p => p.isDel == false, p => p.createTime, OrderByType.Desc,
                entity.page, entity.limit);
            jm.status = true;
            jm.data = list;

            return jm;

        }

        #endregion



        /// <summary>
        /// 获取单个公告内容
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> NoticeInfo([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            var model = await _noticeServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "数据获取失败";
                return jm;
            }
            jm.status = true;
            jm.data = model;
            return jm;
        }

    }
}
