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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.IServices;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 微信小程序消息模板
    ///</summary>
    [Description("微信小程序消息模板")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsUserWeChatMsgTemplateController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsUserWeChatMsgTemplateServices _coreCmsUserWeChatMsgTemplateServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsUserWeChatMsgTemplateController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsUserWeChatMsgTemplateServices coreCmsUserWeChatMsgTemplateServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsUserWeChatMsgTemplateServices = coreCmsUserWeChatMsgTemplateServices;
        }

        #region 首页数据============================================================
        // POST: Api/CoreCmsUserWeChatMsgTemplate/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public async Task<AdminUiCallBack> GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var data = await _coreCmsUserWeChatMsgTemplateServices.QueryAsync();

            var order = data.Find(p => p.templateTitle == GlobalEnumVars.WeChatMsgTemplateType.order.ToString());
            var cancel = data.Find(p => p.templateTitle == GlobalEnumVars.WeChatMsgTemplateType.cancel.ToString());
            var pay = data.Find(p => p.templateTitle == GlobalEnumVars.WeChatMsgTemplateType.pay.ToString());
            var ship = data.Find(p => p.templateTitle == GlobalEnumVars.WeChatMsgTemplateType.ship.ToString());
            var aftersale = data.Find(p => p.templateTitle == GlobalEnumVars.WeChatMsgTemplateType.aftersale.ToString());
            var refund = data.Find(p => p.templateTitle == GlobalEnumVars.WeChatMsgTemplateType.refund.ToString());

            jm.data = new
            {
                order,
                cancel,
                pay,
                ship,
                aftersale,
                refund
            };

            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsUserWeChatMsgTemplate/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsUserWeChatMsgTemplateServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;
            jm.data = model;

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Api/CoreCmsUserWeChatMsgTemplate/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] FMWeChatMsgTemplateEdit entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.list == null || entity.list.Count < 0)
            {
                jm.msg = "未传输讯息";
                return jm;
            }
            //事物处理过程开始
            var data = await _coreCmsUserWeChatMsgTemplateServices.QueryAsync();
            foreach (var item in data)
            {
                var o = entity.list.Find(p => p.templateTitle == item.templateTitle);
                if (o != null)
                {
                    item.templateId = o.templateId.Trim();
                    item.data01 = o.data01.Trim();
                    item.data02 = o.data02.Trim();
                    item.data03 = o.data03.Trim();
                    item.data04 = o.data04.Trim();
                    item.data05 = o.data05.Trim();
                }
            }

            var bl = await _coreCmsUserWeChatMsgTemplateServices.UpdateAsync(data);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            jm.otherData = entity;

            return jm;
        }
        #endregion

    }
}
