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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.IServices;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 代理商等级设置表
    ///</summary>
    [Description("代理商等级设置表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsAgentGradeController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsAgentGradeServices _coreCmsAgentGradeServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsAgentGradeController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsAgentGradeServices coreCmsAgentGradeServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsAgentGradeServices = coreCmsAgentGradeServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsAgentGrade/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsAgentGrade>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<CoreCmsAgentGrade, object>> orderEx = orderField switch
            {
                "id" => p => p.id,
                "name" => p => p.name,
                "isDefault" => p => p.isDefault,
                "isAutoUpGrade" => p => p.isAutoUpGrade,
                "defaultSalesPriceType" => p => p.defaultSalesPriceType,
                "defaultSalesPriceNumber" => p => p.defaultSalesPriceNumber,
                "sortId" => p => p.sortId,
                "description" => p => p.description,
                _ => p => p.id
            };

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
            //价格加成方式 int
            var defaultSalesPriceType = Request.Form["defaultSalesPriceType"].FirstOrDefault().ObjectToInt(0);
            if (defaultSalesPriceType > 0)
            {
                where = where.And(p => p.defaultSalesPriceType == defaultSalesPriceType);
            }
            //价格加成值 int
            var defaultSalesPriceNumber = Request.Form["defaultSalesPriceNumber"].FirstOrDefault().ObjectToInt(0);
            if (defaultSalesPriceNumber > 0)
            {
                where = where.And(p => p.defaultSalesPriceNumber == defaultSalesPriceNumber);
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
            var list = await _coreCmsAgentGradeServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsAgentGrade/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public AdminUiCallBack GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var agentDefaultSalesPriceType = EnumHelper.EnumToList<GlobalEnumVars.AgentDefaultSalesPriceType>();
            jm.data = new
            {
                agentDefaultSalesPriceType
            };


            return jm;
        }
        #endregion

        #region 创建数据============================================================
        // POST: Api/CoreCmsAgentGrade/GetCreate
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
            var agentDefaultSalesPriceType = EnumHelper.EnumToList<GlobalEnumVars.AgentDefaultSalesPriceType>();
            jm.data = new
            {
                agentDefaultSalesPriceType
            };
            return jm;
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsAgentGrade/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsAgentGrade entity)
        {
            var jm = await _coreCmsAgentGradeServices.InsertAsync(entity);
            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsAgentGrade/GetEdit
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

            var model = await _coreCmsAgentGradeServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;
            var agentDefaultSalesPriceType = EnumHelper.EnumToList<GlobalEnumVars.AgentDefaultSalesPriceType>();
            jm.data = new
            {
                model,
                agentDefaultSalesPriceType
            };

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Api/CoreCmsAgentGrade/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsAgentGrade entity)
        {
            var jm = await _coreCmsAgentGradeServices.UpdateAsync(entity);
            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsAgentGrade/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsAgentGradeServices.ExistsAsync(p => p.id == entity.id, true);
            if (!model)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            jm = await _coreCmsAgentGradeServices.DeleteByIdAsync(entity.id);

            return jm;
        }
        #endregion

        #region 设置是否默认等级============================================================
        // POST: Api/CoreCmsAgentGrade/DoSetisDefault/10
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

            var oldModel = await _coreCmsAgentGradeServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isDefault = (bool)entity.data;

            jm = await _coreCmsAgentGradeServices.UpdateAsync(oldModel);

            return jm;
        }
        #endregion

        #region 设置是否自动升级============================================================
        // POST: Api/CoreCmsAgentGrade/DoSetisAutoUpGrade/10
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

            var oldModel = await _coreCmsAgentGradeServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isAutoUpGrade = (bool)entity.data;

            jm = await _coreCmsAgentGradeServices.UpdateAsync(oldModel);

            return jm;
        }
        #endregion


    }
}
