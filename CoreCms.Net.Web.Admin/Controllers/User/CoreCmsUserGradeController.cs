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
    ///     用户等级表
    /// </summary>
    [Description("用户等级表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsUserGradeController : ControllerBase
    {
        private readonly ICoreCmsUserGradeServices _coreCmsUserGradeServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsUserGradeController(IWebHostEnvironment webHostEnvironment,
            ICoreCmsUserGradeServices coreCmsUserGradeServices, ICoreCmsUserServices userServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsUserGradeServices = coreCmsUserGradeServices;
            _userServices = userServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsUserGrade/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsUserGrade>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsUserGrade, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "title":
                    orderEx = p => p.title;
                    break;
                case "isDefault":
                    orderEx = p => p.isDefault;
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

            //id int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //标题 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title)) @where = @where.And(p => p.title.Contains(title));
            //是否默认 bit
            var isDefault = Request.Form["isDefault"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDefault) && isDefault.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isDefault);
            else if (!string.IsNullOrEmpty(isDefault) && isDefault.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isDefault == false);
            //获取数据
            var list = await _coreCmsUserGradeServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsUserGrade/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public AdminUiCallBack GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack
            {
                code = 0
            };
            return jm;
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/CoreCmsUserGrade/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public AdminUiCallBack GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack
            {
                code = 0
            };
            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsUserGrade/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsUserGrade entity)
        {
            var jm = new AdminUiCallBack();

            var id = await _coreCmsUserGradeServices.InsertAsync(entity);
            var bl = id > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            //其他处理
            if (bl && entity.isDefault)
            {
                Expression<Func<CoreCmsUserGrade, bool>> predicate = p => p.id != id;
                await _coreCmsUserGradeServices.UpdateAsync(it => new CoreCmsUserGrade { isDefault = false },
                    predicate);
            }

            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsUserGrade/GetEdit
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

            var model = await _coreCmsUserGradeServices.QueryByIdAsync(entity.id);
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

        // POST: Admins/CoreCmsUserGrade/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsUserGrade entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsUserGradeServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var oldDf = oldModel.isDefault;

            if (oldDf && entity.isDefault == false)
            {
                jm.msg = "请保留一个为默认等级";
                return jm;
            }

            //事物处理过程开始
            oldModel.id = entity.id;
            oldModel.title = entity.title;
            oldModel.isDefault = entity.isDefault;
            //事物处理过程结束
            var bl = await _coreCmsUserGradeServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            //其他处理
            if (bl && entity.isDefault)
            {
                Expression<Func<CoreCmsUserGrade, bool>> predicate = p => p.id != entity.id;
                await _coreCmsUserGradeServices.UpdateAsync(it => new CoreCmsUserGrade { isDefault = false },
                    predicate);
            }

            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsUserGrade/DoDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsUserGradeServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var isHave = await _userServices.ExistsAsync(p => p.grade == model.id);
            if (isHave)
            {
                jm.msg = "存在下级关联数据,禁止删除";
                return jm;
            }

            var isDefault = await _coreCmsUserGradeServices.ExistsAsync(p => p.isDefault && p.id != entity.id);
            if (isDefault == false)
            {
                jm.msg = "请先设置其他选项为默认";
                return jm;
            }

            var bl = await _coreCmsUserGradeServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/CoreCmsUserGrade/GetDetails/10
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

            var model = await _coreCmsUserGradeServices.QueryByIdAsync(entity.id);
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

        #region 设置是否默认============================================================

        // POST: Api/CoreCmsUserGrade/DoSetisDefault/10
        /// <summary>
        ///     设置是否默认
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否默认")]
        public async Task<AdminUiCallBack> DoSetisDefault([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsUserGradeServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isDefault = entity.data;

            if (entity.data == false)
            {
                var isHave = await _coreCmsUserGradeServices.ExistsAsync(p => p.isDefault && p.id != entity.id);
                if (isHave == false)
                {
                    jm.msg = "请保持一个默认设置";
                    return jm;
                }
            }

            var bl = await _coreCmsUserGradeServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            //其他处理
            if (bl && oldModel.isDefault)
            {
                Expression<Func<CoreCmsUserGrade, bool>> predicate = p => p.id != entity.id;
                await _coreCmsUserGradeServices.UpdateAsync(it => new CoreCmsUserGrade { isDefault = false },
                    predicate);
            }

            return jm;
        }

        #endregion
    }
}