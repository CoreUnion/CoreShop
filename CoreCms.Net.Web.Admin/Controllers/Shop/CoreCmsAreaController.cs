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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     地区表
    /// </summary>
    [Description("地区表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsAreaController : ControllerBase
    {
        private readonly ICoreCmsAreaServices _coreCmsAreaServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsAreaServices"></param>
        public CoreCmsAreaController(IWebHostEnvironment webHostEnvironment, ICoreCmsAreaServices coreCmsAreaServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsAreaServices = coreCmsAreaServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsArea/GetAllList
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetAllList()
        {
            var jm = new AdminUiCallBack();

            //获取数据
            var list = await _coreCmsAreaServices.GetCaChe();
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion


        #region 获取列表============================================================

        // POST: Api/CoreCmsArea/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsArea>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsArea, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "parentId":
                    orderEx = p => p.parentId;
                    break;
                case "depth":
                    orderEx = p => p.depth;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "postalCode":
                    orderEx = p => p.postalCode;
                    break;
                case "sort":
                    orderEx = p => p.sort;
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

            //地区ID int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //父级ID int
            var parentId = Request.Form["parentId"].FirstOrDefault().ObjectToInt(0);
            if (parentId > 0) @where = @where.And(p => p.parentId == parentId);
            //地区深度 int
            var depth = Request.Form["depth"].FirstOrDefault().ObjectToInt(0);
            if (depth > 0) @where = @where.And(p => p.depth == depth);
            //地区名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //邮编 nvarchar
            var postalCode = Request.Form["postalCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(postalCode)) @where = @where.And(p => p.postalCode.Contains(postalCode));
            //地区排序 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0) @where = @where.And(p => p.sort == sort);
            //获取数据
            var list = await _coreCmsAreaServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsArea/GetIndex
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

        // POST: Api/CoreCmsArea/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public async Task<AdminUiCallBack> GetCreate([FromBody] FMIntId entity)
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var parentModel = await _coreCmsAreaServices.QueryByClauseAsync(p => p.id == entity.id);
            jm.data = parentModel;

            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsArea/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsArea entity)
        {
            var jm = await _coreCmsAreaServices.InsertAsync(entity);
            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsArea/GetEdit
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

            var model = await _coreCmsAreaServices.QueryByIdAsync(entity.id);
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

        // POST: Admins/CoreCmsArea/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsArea entity)
        {
            var jm = await _coreCmsAreaServices.UpdateAsync(entity);
            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsArea/DoDelete/10
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

            var bl = await _coreCmsAreaServices.ExistsAsync(p => p.parentId == entity.id);
            if (bl)
            {
                jm.msg = GlobalConstVars.DeleteIsHaveChildren;
                return jm;
            }

            jm = await _coreCmsAreaServices.DeleteByIdAsync(entity.id);
            return jm;

        }

        #endregion

    }
}