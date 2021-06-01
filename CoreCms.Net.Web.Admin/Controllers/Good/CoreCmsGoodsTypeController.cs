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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     商品类型
    /// </summary>
    [Description("商品类型")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize]
    public class CoreCmsGoodsTypeController : ControllerBase
    {
        private readonly ICoreCmsGoodsTypeParamsServices _coreCmsGoodsTypeParamsServices;
        private readonly ICoreCmsGoodsTypeServices _coreCmsGoodsTypeServices;
        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly ICoreCmsGoodsParamsServices _paramsServices;
        private readonly ICoreCmsGoodsTypeSpecRelServices _typeSpecRelServices;
        private readonly ICoreCmsGoodsTypeSpecServices _typeSpecServices;

        private readonly ICoreCmsGoodsTypeSpecValueServices _valueServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsGoodsTypeController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsGoodsTypeServices coreCmsGoodsTypeServices
            , ICoreCmsGoodsTypeSpecRelServices typeSpecRelServices
            , ICoreCmsGoodsTypeSpecServices typeSpecServices
            , ICoreCmsGoodsParamsServices paramsServices
            , ICoreCmsGoodsTypeSpecValueServices coreCmsGoodsTypeSpecValueServices
            , ICoreCmsGoodsTypeParamsServices coreCmsGoodsTypeParamsServices, ICoreCmsGoodsServices goodsServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsGoodsTypeServices = coreCmsGoodsTypeServices;
            _typeSpecRelServices = typeSpecRelServices;
            _typeSpecServices = typeSpecServices;
            _paramsServices = paramsServices;
            _valueServices = coreCmsGoodsTypeSpecValueServices;
            _coreCmsGoodsTypeParamsServices = coreCmsGoodsTypeParamsServices;
            _goodsServices = goodsServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsGoodsType/GetPageList
        /// <summary>
        ///     获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsGoodsType>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsGoodsType, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;

                case "name":
                    orderEx = p => p.name;
                    break;

                case "parameters":
                    orderEx = p => p.parameters;
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
            //类型名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //获取数据
            var list = await _coreCmsGoodsTypeServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);

            if (list.Any())
            {
                var ids = list.Select(p => p.id).ToList();
                var goodsTypeSpecRel = await _typeSpecRelServices.QueryListByClauseAsync(p => ids.Contains(p.typeId));
                if (goodsTypeSpecRel.Any())
                {
                    var specIds = goodsTypeSpecRel.Select(p => p.specId).ToArray();
                    var specModels = await _typeSpecServices.QueryByIDsAsync(specIds);
                    if (specModels.Any())
                        foreach (var item in list)
                        {
                            var childInts = goodsTypeSpecRel.Where(p => p.typeId == item.id).Select(p => p.specId)
                                .ToList();
                            item.spec = specModels.Where(p => childInts.Contains(p.id)).OrderBy(p => p.sort).ToList();
                        }
                }

                var parameters =
                    await _coreCmsGoodsTypeParamsServices.QueryListByClauseAsync(p => ids.Contains(p.typeId));
                if (parameters.Any())
                {
                    var parameterIds = parameters.Select(p => p.paramsId).ToList();
                    var parameterModels =
                        await _paramsServices.QueryListByClauseAsync(p => parameterIds.Contains(p.id));
                    if (parameterModels.Any())
                        foreach (var item in list)
                        {
                            var childInts = parameters.Where(p => p.typeId == item.id).Select(p => p.paramsId).ToList();
                            item.parameter = parameterModels.Where(p => childInts.Contains(p.id))
                                .OrderBy(p => p.createTime).ToList();
                        }
                }
            }

            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion 获取列表============================================================

        #region 首页数据============================================================

        // POST: Api/CoreCmsGoodsType/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public JsonResult GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return new JsonResult(jm);
        }

        #endregion 首页数据============================================================

        #region 创建数据============================================================

        // POST: Api/CoreCmsGoodsType/GetCreate
        /// <summary>
        ///     创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public JsonResult GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return new JsonResult(jm);
        }

        #endregion 创建数据============================================================

        #region 创建提交============================================================

        // POST: Api/CoreCmsGoodsType/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] FmGoodsTypeInsert entity)
        {
            var jm = await _coreCmsGoodsTypeServices.InsertAsync(entity);
            return new JsonResult(jm);
        }

        #endregion 创建提交============================================================

        #region 编辑数据============================================================

        // POST: Api/CoreCmsGoodsType/GetEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<JsonResult> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsGoodsTypeServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            jm.code = 0;
            jm.data = model;

            return new JsonResult(jm);
        }

        #endregion 编辑数据============================================================

        #region 编辑提交============================================================

        // POST: Admins/CoreCmsGoodsType/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] CoreCmsGoodsType entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsTypeServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            //事物处理过程开始
            oldModel.name = entity.name;
            //事物处理过程结束

            var bl = await _coreCmsGoodsTypeServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return new JsonResult(jm);
        }

        #endregion 编辑提交============================================================

        #region 删除数据============================================================

        // POST: Api/CoreCmsGoodsType/DoDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<JsonResult> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsGoodsTypeServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return new JsonResult(jm);
            }

            if (await _goodsServices.ExistsAsync(p => p.goodsTypeId == model.id))
            {
                jm.msg = "类型关联商品信息,禁止删除!";
                return new JsonResult(jm);
            }

            jm = await _coreCmsGoodsTypeServices.DeleteByIdAsync(entity.id);
            return new JsonResult(jm);
        }

        #endregion 删除数据============================================================

        #region 获取已有参数列表============================================================

        // POST: Api/CoreCmsGoodsType/GetGoodsParamsPageList
        /// <summary>
        ///     获取已有参数列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取已有参数列表")]
        public async Task<JsonResult> GetGoodsParamsPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsGoodsParams>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsGoodsParams, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;

                case "name":
                    orderEx = p => p.name;
                    break;

                case "value":
                    orderEx = p => p.value;
                    break;

                case "type":
                    orderEx = p => p.type;
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
            //参数名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //参数值 nvarchar
            var value = Request.Form["value"].FirstOrDefault();
            if (!string.IsNullOrEmpty(value)) @where = @where.And(p => p.value.Contains(value));
            //参数类型 nvarchar
            var type = Request.Form["type"].FirstOrDefault();
            if (!string.IsNullOrEmpty(type)) @where = @where.And(p => p.type.Contains(type));
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
            var list = await _paramsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion 获取已有参数列表============================================================

        #region 选择参数面板============================================================

        // POST: Api/CoreCmsGoodsParams/GetGoodParams
        /// <summary>
        ///     选择参数面板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("选择参数面板")]
        public JsonResult GetGoodsParams()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            var goodsParamTypes = EnumHelper.EnumToList<GlobalEnumVars.GoodsParamTypes>();
            jm.data = new
            {
                goodsParamTypes
            };
            return new JsonResult(jm);
        }

        #endregion 选择参数面板============================================================

        #region 获取已有属性列表============================================================

        // POST: Api/CoreCmsGoodsType/GetTypeSpecSpecPageList
        /// <summary>
        ///     获取已有属性列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取已有属性列表")]
        public async Task<JsonResult> GetTypeSpecSpecPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsGoodsTypeSpec>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsGoodsTypeSpec, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;

                case "name":
                    orderEx = p => p.name;
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

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //属性名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //属性排序 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0) @where = @where.And(p => p.sort == sort);
            //获取数据
            var list = await _typeSpecServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);

            if (list.Any())
            {
                var ids = list.Select(p => p.id).ToList();
                var values = await _valueServices.QueryListByClauseAsync(p => ids.Contains(p.specId));
                foreach (var item in list)
                    item.specValues = values.Where(p => p.specId == item.id).OrderBy(p => p.sort).ToList();
            }

            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return new JsonResult(jm);
        }

        #endregion 获取已有属性列表============================================================

        #region 选择属性面板============================================================

        // POST: Api/CoreCmsGoodsType/GetTypeSpecIndex
        /// <summary>
        ///     选择属性面板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("选择属性面板")]
        public JsonResult GetTypeSpecIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return new JsonResult(jm);
        }

        #endregion 选择属性面板============================================================

        #region 编辑参数============================================================

        // POST: Api/CoreCmsGoodsType/GetEditParameters
        /// <summary>
        ///     编辑参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑参数")]
        public async Task<JsonResult> GetEditParameters([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsGoodsTypeServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            jm.code = 0;

            var parameters = await _paramsServices.QueryAsync();
            var typeParametersRel =
                await _coreCmsGoodsTypeParamsServices.QueryListByClauseAsync(p => p.typeId == entity.id);
            jm.data = new
            {
                model,
                parameters,
                typeParametersRel = typeParametersRel.Any()
                    ? typeParametersRel.Select(p => p.paramsId).ToList()
                    : new List<int>()
            };

            return new JsonResult(jm);
        }

        #endregion 编辑参数============================================================

        #region 编辑参数提交============================================================

        // POST: Admins/CoreCmsGoodsType/DoEditParameters
        /// <summary>
        ///     编辑参数提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑参数提交")]
        public async Task<JsonResult> DoEditParameters([FromBody] FMUpdateArrayIntDataByIntId entity)
        {
            var jm = await _coreCmsGoodsTypeServices.UpdateParametersAsync(entity);
            return new JsonResult(jm);
        }

        #endregion 编辑参数提交============================================================

        #region 编辑属性============================================================

        // POST: Api/CoreCmsGoodsType/GetEditTypes
        /// <summary>
        ///     编辑属性
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑属性")]
        public async Task<JsonResult> GetEditTypes([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsGoodsTypeServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return new JsonResult(jm);
            }

            jm.code = 0;

            var goodsTypeSpec = await _typeSpecServices.QueryAsync();
            var goodsTypeSpecRel = await _typeSpecRelServices.QueryListByClauseAsync(p => p.typeId == entity.id);
            jm.data = new
            {
                model,
                goodsTypeSpec,
                goodsTypeSpecRel = goodsTypeSpecRel.Any()
                    ? goodsTypeSpecRel.Select(p => p.specId).ToList()
                    : new List<int>()
            };

            return new JsonResult(jm);
        }

        #endregion 编辑属性============================================================

        #region 编辑属性提交============================================================

        // POST: Admins/CoreCmsGoodsType/DoEditTypes
        /// <summary>
        ///     编辑属性提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑属性提交")]
        public async Task<JsonResult> DoEditTypes([FromBody] FMUpdateArrayIntDataByIntId entity)
        {
            var jm = await _coreCmsGoodsTypeServices.UpdateTypesAsync(entity);
            return new JsonResult(jm);
        }

        #endregion 编辑属性提交============================================================
    }
}