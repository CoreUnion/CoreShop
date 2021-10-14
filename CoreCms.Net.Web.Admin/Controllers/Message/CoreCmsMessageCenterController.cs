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
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     消息配置表
    /// </summary>
    [Description("消息配置表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsMessageCenterController : ControllerBase
    {
        private readonly ICoreCmsMessageCenterServices _coreCmsMessageCenterServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsMessageCenterServices"></param>
        public CoreCmsMessageCenterController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsMessageCenterServices coreCmsMessageCenterServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsMessageCenterServices = coreCmsMessageCenterServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsMessageCenter/GetPageList
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsMessageCenter>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsMessageCenter, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "code":
                    orderEx = p => p.code;
                    break;
                case "isSms":
                    orderEx = p => p.isSms;
                    break;
                case "isMessage":
                    orderEx = p => p.isMessage;
                    break;
                case "isWxTempletMessage":
                    orderEx = p => p.isWxTempletMessage;
                    break;
                default:
                    orderEx = p => p.id;
                    break;
            }

            //设置排序方式
            var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
            var orderBy = orderDirection switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //编码 nvarchar
            var code = Request.Form["code"].FirstOrDefault();
            if (!string.IsNullOrEmpty(code)) @where = @where.And(p => p.code.Contains(code));
            //启用短信 bit
            var isSms = Request.Form["isSms"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isSms) && isSms.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isSms);
            else if (!string.IsNullOrEmpty(isSms) && isSms.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isSms == false);
            //启用站内消息 bit
            var isMessage = Request.Form["isMessage"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isMessage) && isMessage.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isMessage);
            else if (!string.IsNullOrEmpty(isMessage) && isMessage.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isMessage == false);
            //启用微信模板消息 bit
            var isWxTempletMessage = Request.Form["isWxTempletMessage"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isWxTempletMessage) && isWxTempletMessage.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isWxTempletMessage);
            else if (!string.IsNullOrEmpty(isWxTempletMessage) && isWxTempletMessage.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isWxTempletMessage == false);
            //获取数据
            var list = await _coreCmsMessageCenterServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent,
                pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsMessageCenter/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public AdminUiCallBack GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var platformMessageTypes = EnumHelper.EnumToList<GlobalEnumVars.PlatformMessageTypes>();
            jm.data = new
            {
                platformMessageTypes
            };


            return jm;
        }

        #endregion

        #region 设置启用短信============================================================

        // POST: Api/CoreCmsMessageCenter/DoSetisSms/10
        /// <summary>
        ///     设置启用短信
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置启用短信")]
        public async Task<AdminUiCallBack> DoSetisSms([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsMessageCenterServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isSms = entity.data;

            var bl = await _coreCmsMessageCenterServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;


            return jm;
        }

        #endregion

        #region 设置启用站内消息============================================================

        // POST: Api/CoreCmsMessageCenter/DoSetisMessage/10
        /// <summary>
        ///     设置启用站内消息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置启用站内消息")]
        public async Task<AdminUiCallBack> DoSetisMessage([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsMessageCenterServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isMessage = entity.data;

            var bl = await _coreCmsMessageCenterServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;


            return jm;
        }

        #endregion

        #region 设置启用微信模板消息============================================================

        // POST: Api/CoreCmsMessageCenter/DoSetisWxTempletMessage/10
        /// <summary>
        ///     设置启用微信模板消息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置启用微信模板消息")]
        public async Task<AdminUiCallBack> DoSetisWxTempletMessage([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsMessageCenterServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isWxTempletMessage = entity.data;

            var bl = await _coreCmsMessageCenterServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;


            return jm;
        }

        #endregion
    }
}