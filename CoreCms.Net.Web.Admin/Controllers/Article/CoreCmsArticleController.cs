/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.Configuration;
using CoreCms.Net.Filter;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     文章表
    /// </summary>
    [Description("文章表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsArticleController : ControllerBase
    {
        private readonly ICoreCmsArticleServices _coreCmsArticleServices;
        private readonly ICoreCmsArticleTypeServices _coreCmsArticleTypeServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsArticleServices"></param>
        /// <param name="coreCmsArticleTypeServices"></param>
        public CoreCmsArticleController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsArticleServices coreCmsArticleServices
            , ICoreCmsArticleTypeServices coreCmsArticleTypeServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsArticleServices = coreCmsArticleServices;
            _coreCmsArticleTypeServices = coreCmsArticleTypeServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsArticle/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsArticle>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsArticle, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;

                case "title":
                    orderEx = p => p.title;
                    break;

                case "brief":
                    orderEx = p => p.brief;
                    break;

                case "coverImage":
                    orderEx = p => p.coverImage;
                    break;

                case "contentBody":
                    orderEx = p => p.contentBody;
                    break;

                case "typeId":
                    orderEx = p => p.typeId;
                    break;

                case "sort":
                    orderEx = p => p.sort;
                    break;

                case "isPub":
                    orderEx = p => p.isPub;
                    break;

                case "isDel":
                    orderEx = p => p.isDel;
                    break;

                case "pv":
                    orderEx = p => p.pv;
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
            if (id > 0) @where = @where.And(p => p.id == id);
            //标题 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title)) @where = @where.And(p => p.title.Contains(title));
            //简介 nvarchar
            var brief = Request.Form["brief"].FirstOrDefault();
            if (!string.IsNullOrEmpty(brief)) @where = @where.And(p => p.brief.Contains(brief));
            //封面图 nvarchar
            var coverImage = Request.Form["coverImage"].FirstOrDefault();
            if (!string.IsNullOrEmpty(coverImage)) @where = @where.And(p => p.coverImage.Contains(coverImage));
            //文章内容 nvarchar
            var contentBody = Request.Form["contentBody"].FirstOrDefault();
            if (!string.IsNullOrEmpty(contentBody)) @where = @where.And(p => p.contentBody.Contains(contentBody));
            //分类id int
            var typeId = Request.Form["typeId"].FirstOrDefault().ObjectToInt(0);
            if (typeId > 0) @where = @where.And(p => p.typeId == typeId);
            //排序 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0) @where = @where.And(p => p.sort == sort);
            //是否发布 bit
            var isPub = Request.Form["isPub"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isPub) && isPub.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isPub);
            else if (!string.IsNullOrEmpty(isPub) && isPub.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isPub == false);
            //是否删除 bit
            var isDel = Request.Form["isDel"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDel) && isDel.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isDel == true);
            else if (!string.IsNullOrEmpty(isDel) && isDel.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isDel == false);
            //访问量 int
            var pv = Request.Form["pv"].FirstOrDefault().ObjectToInt(0);
            if (pv > 0) @where = @where.And(p => p.pv == pv);
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
            var list = await _coreCmsArticleServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion 获取列表============================================================

        #region 首页数据============================================================

        // POST: Api/CoreCmsArticle/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public async Task<AdminUiCallBack> GetIndex()
        {
            //返回数据

            var categories = await _coreCmsArticleTypeServices.QueryAsync();

            var jm = new AdminUiCallBack { code = 0 };
            jm.data = new
            {
                categories
            };
            return jm;
        }

        #endregion 首页数据============================================================

        #region 创建数据============================================================

        // POST: Api/CoreCmsArticle/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public async Task<AdminUiCallBack> GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var categories = await _coreCmsArticleTypeServices.QueryAsync();
            jm.data = new
            {
                categories
            };

            return jm;
        }

        #endregion 创建数据============================================================

        #region 创建提交============================================================

        // POST: Api/CoreCmsArticle/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsArticle entity)
        {
            var jm = new AdminUiCallBack();

            entity.createTime = DateTime.Now;

            var bl = await _coreCmsArticleServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        #endregion 创建提交============================================================

        #region 编辑数据============================================================

        // POST: Api/CoreCmsArticle/GetEdit
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

            var model = await _coreCmsArticleServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;

            var categories = await _coreCmsArticleTypeServices.QueryAsync();
            jm.data = new
            {
                categories,
                model
            };

            return jm;
        }

        #endregion 编辑数据============================================================

        #region 编辑提交============================================================

        // POST: Admins/CoreCmsArticle/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsArticle entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsArticleServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            //事物处理过程开始
            //oldModel.id = entity.id;
            oldModel.title = entity.title;
            oldModel.brief = entity.brief;
            oldModel.coverImage = entity.coverImage;
            oldModel.contentBody = entity.contentBody;
            oldModel.typeId = entity.typeId;
            oldModel.sort = entity.sort;
            oldModel.isPub = entity.isPub;
            oldModel.isDel = entity.isDel;
            oldModel.pv = entity.pv;
            //oldModel.createTime = entity.createTime;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await _coreCmsArticleServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion 编辑提交============================================================

        #region 删除数据============================================================

        // POST: Api/CoreCmsArticle/DoDelete/10
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

            var model = await _coreCmsArticleServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var bl = await _coreCmsArticleServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;
        }

        #endregion 删除数据============================================================

        #region 设置是否发布============================================================

        // POST: Api/CoreCmsArticle/DoSetisPub/10
        /// <summary>
        ///     设置是否发布
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否发布")]
        public async Task<AdminUiCallBack> DoSetisPub([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsArticleServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isPub = entity.data;

            var bl = await _coreCmsArticleServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion 设置是否发布============================================================

        #region 设置是否删除============================================================

        // POST: Api/CoreCmsArticle/DoSetisDel/10
        /// <summary>
        ///     设置是否删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否删除")]
        public async Task<AdminUiCallBack> DoSetisDel([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsArticleServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isDel = entity.data;

            var bl = await _coreCmsArticleServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion 设置是否删除============================================================
    }
}