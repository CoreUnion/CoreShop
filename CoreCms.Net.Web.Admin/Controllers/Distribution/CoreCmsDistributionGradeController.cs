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
    ///     分销商等级设置表
    /// </summary>
    [Description("分销商等级设置表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsDistributionGradeController : ControllerBase
    {
        private readonly ICoreCmsDistributionConditionServices _coreCmsDistributionConditionServices;
        private readonly ICoreCmsDistributionGradeServices _coreCmsDistributionGradeServices;
        private readonly ICoreCmsDistributionResultServices _coreCmsDistributionResultServices;
        private readonly ICoreCmsUserGradeServices _coreCmsUserGradeServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsDistributionGradeController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsDistributionGradeServices coreCmsDistributionGradeServices
            , ICoreCmsUserGradeServices coreCmsUserGradeServices
            , ICoreCmsDistributionConditionServices coreCmsDistributionConditionServices
            , ICoreCmsDistributionResultServices coreCmsDistributionResultServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsDistributionGradeServices = coreCmsDistributionGradeServices;
            _coreCmsUserGradeServices = coreCmsUserGradeServices;
            _coreCmsDistributionConditionServices = coreCmsDistributionConditionServices;
            _coreCmsDistributionResultServices = coreCmsDistributionResultServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsDistributionGrade/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsDistributionGrade>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsDistributionGrade, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "isDefault":
                    orderEx = p => p.isDefault;
                    break;
                case "isAutoUpGrade":
                    orderEx = p => p.isAutoUpGrade;
                    break;
                case "sortId":
                    orderEx = p => p.sortId;
                    break;
                case "description":
                    orderEx = p => p.description;
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

            //等级序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //等级名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name))
            {
                where = where.And(p => p.name.Contains(name));
            }
            //是否默认等级 bit
            var isDefault = Request.Form["isDefault"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDefault) && isDefault.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isDefault == true);
            }
            else if (!string.IsNullOrEmpty(isDefault) && isDefault.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isDefault == false);
            }
            //是否自动升级 bit
            var isAutoUpGrade = Request.Form["isAutoUpGrade"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isAutoUpGrade) && isAutoUpGrade.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isAutoUpGrade == true);
            }
            else if (!string.IsNullOrEmpty(isAutoUpGrade) && isAutoUpGrade.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isAutoUpGrade == false);
            }
            //等级排序 int
            var sortId = Request.Form["sortId"].FirstOrDefault().ObjectToInt(0);
            if (sortId > 0)
            {
                where = where.And(p => p.sortId == sortId);
            }
            //等级说明 nvarchar
            var description = Request.Form["description"].FirstOrDefault();
            if (!string.IsNullOrEmpty(description))
            {
                where = where.And(p => p.description.Contains(description));
            }
            //获取数据
            var list = await _coreCmsDistributionGradeServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsDistributionGrade/GetIndex
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

        #region 创建数据============================================================
        // POST: Api/CoreCmsDistributionGrade/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public AdminUiCallBack GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return jm;
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsDistributionGrade/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsDistributionGrade entity)
        {
            var jm = await _coreCmsDistributionGradeServices.InsertAsync(entity);
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

            var model = await _coreCmsDistributionGradeServices.QueryByClauseAsync(p => p.id == entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var distributionConditions = await _coreCmsDistributionConditionServices.QueryListByClauseAsync(p => p.gradeId == model.id);
            var distributionResults = await _coreCmsDistributionResultServices.QueryListByClauseAsync(p => p.gradeId == model.id);

            jm.code = 0;
            jm.data = new
            {
                model,
                distributionConditions,
                distributionResults
            };

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
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsDistributionGrade entity)
        {
            var jm = await _coreCmsDistributionGradeServices.UpdateAsync(entity);
            return jm;
        }

        #endregion

        #region 设置是否默认等级============================================================
        // POST: Api/CoreCmsDistributionGrade/DoSetisDefault/10
        /// <summary>
        /// 设置是否默认等级
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否默认等级")]
        public async Task<AdminUiCallBack> DoSetisDefault([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsDistributionGradeServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isDefault = (bool)entity.data;

            jm = await _coreCmsDistributionGradeServices.UpdateAsync(oldModel);

            return jm;
        }
        #endregion

        #region 设置是否自动升级============================================================
        // POST: Api/CoreCmsDistributionGrade/DoSetisAutoUpGrade/10
        /// <summary>
        /// 设置是否自动升级
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否自动升级")]
        public async Task<AdminUiCallBack> DoSetisAutoUpGrade([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsDistributionGradeServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isAutoUpGrade = (bool)entity.data;

            jm = await _coreCmsDistributionGradeServices.UpdateAsync(oldModel);

            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsDistributionGrade/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = await _coreCmsDistributionGradeServices.DeleteByIdAsync(entity.id);
            return jm;
        }
        #endregion


        //升级条件

        #region 获取列表============================================================

        // POST: Api/CoreCmsDistributionGrade/GetDistributionConditionPageList
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetDistributionConditionPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsDistributionCondition>();

            //查询筛选


            //会员等级Id int
            var gradeId = Request.Form["gradeId"].FirstOrDefault().ObjectToInt(0);
            if (gradeId > 0)
            {
                @where = @where.And(p => p.gradeId == gradeId);
            }

            //获取数据
            var list = await _coreCmsDistributionConditionServices.QueryPageAsync(where, p => p.id, OrderByType.Asc, pageCurrent, pageSize);
            if (list.Any())
            {
                var distributionConditions = EnumHelper.EnumToList<GlobalEnumVars.DistributionConditions>();
                foreach (var condition in list)
                {
                    var cd = distributionConditions.Find(p => p.title == condition.code);
                    condition.name = cd?.description;
                }
            }

            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/CoreCmsDistributionGrade/GetDistributionConditionCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public AdminUiCallBack GetDistributionConditionCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            var distributionConditionsCode = EnumHelper.EnumToList<GlobalEnumVars.DistributionConditionsCode>();

            jm.data = new
            {
                distributionConditionsCode,
            };
            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsDistributionGrade/DoDistributionConditionCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoDistributionConditionCreate([FromBody] CoreCmsDistributionCondition entity)
        {
            var jm = await _coreCmsDistributionConditionServices.InsertAsync(entity);
            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsDistributionGrade/GetDistributionConditionEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetDistributionConditionEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsDistributionConditionServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var distributionConditionsCode = EnumHelper.EnumToList<GlobalEnumVars.DistributionConditionsCode>();

            jm.code = 0;
            jm.data = new
            {
                model,
                distributionConditionsCode,
            };

            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Api/CoreCmsDistributionGrade/DoDistributionConditionEdit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoDistributionConditionEdit([FromBody] CoreCmsDistributionCondition entity)
        {
            var jm = await _coreCmsDistributionConditionServices.UpdateAsync(entity);
            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsDistributionGrade/DoDistributionConditionDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDistributionConditionDelete([FromBody] FMIntId entity)
        {
            var jm = await _coreCmsDistributionConditionServices.DeleteByIdAsync(entity.id);
            return jm;
        }

        #endregion

        //升级结果

        #region 获取列表============================================================

        // POST: Api/CoreCmsDistributionGrade/GetDistributionResultPageList
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetDistributionResultPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsDistributionResult>();

            //查询筛选


            //会员等级Id int
            var gradeId = Request.Form["gradeId"].FirstOrDefault().ObjectToInt(0);
            if (gradeId > 0)
            {
                @where = @where.And(p => p.gradeId == gradeId);
            }

            //获取数据
            var list = await _coreCmsDistributionResultServices.QueryPageAsync(where, p => p.code, OrderByType.Asc, pageCurrent,
                pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/CoreCmsDistributionGrade/GetDistributionResultCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public AdminUiCallBack GetDistributionResultCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };


            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsDistributionGrade/DoDistributionResultCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoDistributionResultCreate([FromBody] CoreCmsDistributionResult entity)
        {
            var jm = await _coreCmsDistributionResultServices.InsertAsync(entity);
            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsDistributionGrade/GetDistributionResultEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetDistributionResultEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsDistributionResultServices.QueryByIdAsync(entity.id);
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

        // POST: Api/CoreCmsDistributionGrade/DoDistributionResultEdit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoDistributionResultEdit([FromBody] CoreCmsDistributionResult entity)
        {
            var jm = await _coreCmsDistributionResultServices.UpdateAsync(entity);
            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsDistributionGrade/DoDistributionResultDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDistributionResultDelete([FromBody] FMIntId entity)
        {
            var jm = await _coreCmsDistributionResultServices.DeleteByIdAsync(entity.id); ;
            return jm;
        }

        #endregion
    }
}