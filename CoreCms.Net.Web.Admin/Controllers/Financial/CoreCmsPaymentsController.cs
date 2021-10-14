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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     支付方式表
    /// </summary>
    [Description("支付方式表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsPaymentsController : ControllerBase
    {
        private readonly ICoreCmsPaymentsServices _coreCmsPaymentsServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsPaymentsServices"></param>
        public CoreCmsPaymentsController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsPaymentsServices coreCmsPaymentsServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsPaymentsServices = coreCmsPaymentsServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsPayments/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsPayments>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsPayments, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "code":
                    orderEx = p => p.code;
                    break;
                case "isOnline":
                    orderEx = p => p.isOnline;
                    break;
                case "parameters":
                    orderEx = p => p.parameters;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "memo":
                    orderEx = p => p.memo;
                    break;
                case "isEnable":
                    orderEx = p => p.isEnable;
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

            // int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //支付类型名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //支付类型编码 nvarchar
            var code = Request.Form["code"].FirstOrDefault();
            if (!string.IsNullOrEmpty(code)) @where = @where.And(p => p.code.Contains(code));
            //是否线上支付 bit
            var isOnline = Request.Form["isOnline"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isOnline) && isOnline.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isOnline);
            else if (!string.IsNullOrEmpty(isOnline) && isOnline.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isOnline == false);
            //参数 nvarchar
            var parameters = Request.Form["parameters"].FirstOrDefault();
            if (!string.IsNullOrEmpty(parameters)) @where = @where.And(p => p.parameters.Contains(parameters));
            //排序 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0) @where = @where.And(p => p.sort == sort);
            //方式描述 nvarchar
            var memo = Request.Form["memo"].FirstOrDefault();
            if (!string.IsNullOrEmpty(memo)) @where = @where.And(p => p.memo.Contains(memo));
            //是否启用 bit
            var isEnable = Request.Form["isEnable"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isEnable) && isEnable.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isEnable);
            else if (!string.IsNullOrEmpty(isEnable) && isEnable.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isEnable == false);
            //获取数据
            var list = await _coreCmsPaymentsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsPayments/GetIndex
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
            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsPayments/GetEdit
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

            var model = await _coreCmsPaymentsServices.QueryByIdAsync(entity.id);
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

        // POST: Api/CoreCmsPayments/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsPayments entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsPaymentsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            //事物处理过程开始
            oldModel.parameters = entity.parameters;
            oldModel.sort = entity.sort;
            oldModel.isEnable = entity.isEnable;
            //事物处理过程结束

            var bl = await _coreCmsPaymentsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 设置是否线上支付============================================================

        // POST: Api/CoreCmsPayments/DoSetisOnline/10
        /// <summary>
        ///     设置是否线上支付
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否线上支付")]
        public async Task<AdminUiCallBack> DoSetisOnline([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsPaymentsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isOnline = entity.data;

            var bl = await _coreCmsPaymentsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 设置是否启用============================================================

        // POST: Api/CoreCmsPayments/DoSetisEnable/10
        /// <summary>
        ///     设置是否启用
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否启用")]
        public async Task<AdminUiCallBack> DoSetisEnable([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsPaymentsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isEnable = entity.data;

            var bl = await _coreCmsPaymentsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion
    }
}