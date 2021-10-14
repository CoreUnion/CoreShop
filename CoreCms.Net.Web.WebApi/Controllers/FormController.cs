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
using CoreCms.Net.IServices;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 表单接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private readonly ICoreCmsFormServices _formServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formServices"></param>
        public FormController(ICoreCmsFormServices formServices)
        {
            _formServices = formServices;
        }


        #region 万能表单/获取活动商品详情=============================================================================
        /// <summary>
        /// 万能表单/获取活动商品详情
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> GetFormDetial([FromBody] FmGetForm entity)
        {
            var jm = await _formServices.GetFormInfo(entity.id, entity.token);
            return jm;
        }
        #endregion


        #region 万能表单/提交表单=============================================================================
        /// <summary>
        /// 万能表单/提交表单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> AddSubmit([FromBody] FmAddSubmit entity)
        {
            var jm = await _formServices.AddSubmit(entity);

            jm.otherData = entity;

            return jm;
        }
        #endregion


    }
}
