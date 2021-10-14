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
using System.IO;
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
    ///     表单
    /// </summary>
    [Description("表单")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsFormController : ControllerBase
    {
        private readonly ICoreCmsFormServices _coreCmsFormServices;
        private readonly ICoreCmsFormItemServices _formItemServices;
        private readonly ICoreCmsFormSubmitServices _formSubmitServices;
        private readonly IWebHostEnvironment _webHostEnvironment;



        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsFormController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsFormServices coreCmsFormServices, ICoreCmsFormItemServices formItemServices, ICoreCmsFormSubmitServices formSubmitServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsFormServices = coreCmsFormServices;
            _formItemServices = formItemServices;
            _formSubmitServices = formSubmitServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsForm/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsForm>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsForm, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "type":
                    orderEx = p => p.type;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "description":
                    orderEx = p => p.description;
                    break;
                case "headType":
                    orderEx = p => p.headType;
                    break;
                case "headTypeValue":
                    orderEx = p => p.headTypeValue;
                    break;
                case "headTypeVideo":
                    orderEx = p => p.headTypeVideo;
                    break;
                case "buttonName":
                    orderEx = p => p.buttonName;
                    break;
                case "buttonColor":
                    orderEx = p => p.buttonColor;
                    break;
                case "isLogin":
                    orderEx = p => p.isLogin;
                    break;
                case "times":
                    orderEx = p => p.times;
                    break;
                case "qrcode":
                    orderEx = p => p.qrcode;
                    break;
                case "returnMsg":
                    orderEx = p => p.returnMsg;
                    break;
                case "endDateTime":
                    orderEx = p => p.endDateTime;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
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
            if (id > 0) where = where.And(p => p.id == id);
            //表单名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) where = where.And(p => p.name.Contains(name));
            //表单类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) where = where.And(p => p.type == type);
            //表单排序 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0) where = where.And(p => p.sort == sort);
            //表单描述 nvarchar
            var description = Request.Form["description"].FirstOrDefault();
            if (!string.IsNullOrEmpty(description)) where = where.And(p => p.description.Contains(description));
            //表头类型 int
            var headType = Request.Form["headType"].FirstOrDefault().ObjectToInt(0);
            if (headType > 0) where = where.And(p => p.headType == headType);
            //表单头值 nvarchar
            var headTypeValue = Request.Form["headTypeValue"].FirstOrDefault();
            if (!string.IsNullOrEmpty(headTypeValue)) where = where.And(p => p.headTypeValue.Contains(headTypeValue));
            //表单视频 nvarchar
            var headTypeVideo = Request.Form["headTypeVideo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(headTypeVideo)) where = where.And(p => p.headTypeVideo.Contains(headTypeVideo));
            //表单提交按钮名称 nvarchar
            var buttonName = Request.Form["buttonName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(buttonName)) where = where.And(p => p.buttonName.Contains(buttonName));
            //表单按钮颜色 nvarchar
            var buttonColor = Request.Form["buttonColor"].FirstOrDefault();
            if (!string.IsNullOrEmpty(buttonColor)) where = where.And(p => p.buttonColor.Contains(buttonColor));
            //是否需要登录 bit
            var isLogin = Request.Form["isLogin"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isLogin) && isLogin.ToLowerInvariant() == "true")
                where = where.And(p => p.isLogin);
            else if (!string.IsNullOrEmpty(isLogin) && isLogin.ToLowerInvariant() == "false")
                where = where.And(p => p.isLogin == false);
            //可提交次数 int
            var times = Request.Form["times"].FirstOrDefault().ObjectToInt(0);
            if (times > 0) where = where.And(p => p.times == times);
            //二维码图片地址 nvarchar
            var qrcode = Request.Form["qrcode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(qrcode)) where = where.And(p => p.qrcode.Contains(qrcode));
            //提交后提示语 nvarchar
            var returnMsg = Request.Form["returnMsg"].FirstOrDefault();
            if (!string.IsNullOrEmpty(returnMsg)) where = where.And(p => p.returnMsg.Contains(returnMsg));
            //结束时间 datetime
            var endDateTime = Request.Form["endDateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(endDateTime))
            {
                if (endDateTime.Contains("到"))
                {
                    var dts = endDateTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.endDateTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.endDateTime < dtEnd);
                }
                else
                {
                    var dt = endDateTime.ObjectToDate();
                    where = where.And(p => p.endDateTime > dt);
                }
            }

            //创建时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                if (createTime.Contains("到"))
                {
                    var dts = createTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.createTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.createTime < dtEnd);
                }
                else
                {
                    var dt = createTime.ObjectToDate();
                    where = where.And(p => p.createTime > dt);
                }
            }

            //更新时间 datetime
            var updateTime = Request.Form["updateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updateTime))
            {
                if (updateTime.Contains("到"))
                {
                    var dts = updateTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.updateTime < dtEnd);
                }
                else
                {
                    var dt = updateTime.ObjectToDate();
                    where = where.And(p => p.updateTime > dt);
                }
            }

            //获取数据
            var list = await _coreCmsFormServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsForm/GetIndex
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

            var formTypes = EnumHelper.EnumToList<GlobalEnumVars.FormTypes>();
            jm.data = new
            {
                formTypes
            };

            return jm;
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/CoreCmsForm/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public AdminUiCallBack GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var formTypes = EnumHelper.EnumToList<GlobalEnumVars.FormTypes>();
            var formHeadTypes = EnumHelper.EnumToList<GlobalEnumVars.FormHeadTypes>();
            var formFieldTypes = EnumHelper.EnumToList<GlobalEnumVars.FormFieldTypes>();
            var formValidationTypes = EnumHelper.EnumToList<GlobalEnumVars.FormValidationTypes>();

            jm.data = new
            {
                formTypes,
                formHeadTypes,
                formFieldTypes,
                formValidationTypes
            };

            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsForm/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] FMForm entity)
        {
            var jm = await _coreCmsFormServices.InsertAsync(entity);
            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsForm/GetEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsFormServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;

            var formTypes = EnumHelper.EnumToList<GlobalEnumVars.FormTypes>();
            var formHeadTypes = EnumHelper.EnumToList<GlobalEnumVars.FormHeadTypes>();
            var formFieldTypes = EnumHelper.EnumToList<GlobalEnumVars.FormFieldTypes>();
            var formValidationTypes = EnumHelper.EnumToList<GlobalEnumVars.FormValidationTypes>();
            var items = await _formItemServices.QueryListByClauseAsync(p => p.formId == model.id, p => p.sort, OrderByType.Asc);
            jm.data = new
            {
                formTypes,
                formHeadTypes,
                formFieldTypes,
                formValidationTypes,
                model,
                items
            };

            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Api/CoreCmsForm/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] FMForm entity)
        {
            var jm = new AdminUiCallBack();

            //事物处理过程结束
            jm = await _coreCmsFormServices.UpdateAsync(entity);

            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsForm/DoDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = await _coreCmsFormServices.DeleteByIdAsync(entity.id);

            return jm;

        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/CoreCmsForm/GetDetails/10
        /// <summary>
        ///     预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsFormServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;
            jm.data = model;

            var totalSubmit = await _formSubmitServices.GetCountAsync(p => p.formId == entity.id);
            var sumSubmit = await _formSubmitServices.GetSumAsync(p => p.formId == entity.id, p => p.money);

            var formData = await _formSubmitServices.GetStatisticsByFormid(entity.id, 7);
            jm.data = new
            {
                model,
                formData.data,
                formData.day,
                totalSubmit,
                sumSubmit
            };

            return jm;
        }

        #endregion

        #region 设置是否需要登录============================================================

        // POST: Api/CoreCmsForm/DoSetisLogin/10
        /// <summary>
        ///     设置是否需要登录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否需要登录")]
        public async Task<AdminUiCallBack> DoSetisLogin([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsFormServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isLogin = entity.data;

            var bl = await _coreCmsFormServices.UpdateAsync(p => new CoreCmsForm() { isLogin = oldModel.isLogin }, p => p.id == oldModel.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? "设置成功" : "设置失败";

            return jm;
        }

        #endregion

    }
}