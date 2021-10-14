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
    ///     广告表
    /// </summary>
    [Description("广告表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsAdvertisementController : ControllerBase
    {
        private readonly ICoreCmsAdvertisementServices _coreCmsAdvertisementServices;
        private readonly ICoreCmsAdvertPositionServices _coreCmsAdvertPositionServices;
        private readonly ICoreCmsArticleServices _coreCmsArticleServices;
        private readonly ICoreCmsArticleTypeServices _coreCmsArticleTypeServices;
        private readonly ICoreCmsGoodsServices _coreCmsGoodsServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsAdvertisementServices"></param>
        /// <param name="coreCmsAdvertPositionServices"></param>
        /// <param name="coreCmsGoodsServices"></param>
        /// <param name="coreCmsArticleServices"></param>
        /// <param name="coreCmsArticleTypeServices"></param>
        public CoreCmsAdvertisementController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsAdvertisementServices coreCmsAdvertisementServices
            , ICoreCmsAdvertPositionServices coreCmsAdvertPositionServices
            , ICoreCmsGoodsServices coreCmsGoodsServices
            , ICoreCmsArticleServices coreCmsArticleServices
            , ICoreCmsArticleTypeServices coreCmsArticleTypeServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsAdvertisementServices = coreCmsAdvertisementServices;
            _coreCmsAdvertPositionServices = coreCmsAdvertPositionServices;
            _coreCmsGoodsServices = coreCmsGoodsServices;
            _coreCmsArticleServices = coreCmsArticleServices;
            _coreCmsArticleTypeServices = coreCmsArticleTypeServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsAdvertisement/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsAdvertisement>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsAdvertisement, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "positionId":
                    orderEx = p => p.positionId;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "imageUrl":
                    orderEx = p => p.imageUrl;
                    break;
                case "val":
                    orderEx = p => p.val;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                case "code":
                    orderEx = p => p.code;
                    break;
                case "type":
                    orderEx = p => p.type;
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
            //位置序列 int
            var positionId = Request.Form["positionId"].FirstOrDefault().ObjectToInt(0);
            if (positionId > 0) @where = @where.And(p => p.positionId == positionId);
            //广告名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //广告图片id nvarchar
            var imageUrl = Request.Form["imageUrl"].FirstOrDefault();
            if (!string.IsNullOrEmpty(imageUrl)) @where = @where.And(p => p.imageUrl.Contains(imageUrl));
            //链接属性值 nvarchar
            var val = Request.Form["val"].FirstOrDefault();
            if (!string.IsNullOrEmpty(val)) @where = @where.And(p => p.val.Contains(val));
            //从小到大 越小越靠前 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0) @where = @where.And(p => p.sort == sort);
            //添加时间 datetime
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

            //广告位置编码 nvarchar
            var code = Request.Form["code"].FirstOrDefault();
            if (!string.IsNullOrEmpty(code)) @where = @where.And(p => p.code.Contains(code));
            //类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) @where = @where.And(p => p.type == type);
            //获取数据
            var list = await _coreCmsAdvertisementServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent,
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

        // POST: Api/CoreCmsAdvertisement/GetIndex
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
            var types = EnumHelper.EnumToList<GlobalEnumVars.AdvertPositionType>();
            jm.data = new
            {
                types
            };
            return jm;
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/CoreCmsAdvertisement/GetCreate
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

            var advertPosition = await _coreCmsAdvertPositionServices.QueryAsync();
            var types = EnumHelper.EnumToList<GlobalEnumVars.AdvertPositionType>();
            jm.data = new
            {
                advertPosition,
                types
            };
            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsAdvertisement/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsAdvertisement entity)
        {
            var jm = new AdminUiCallBack();

            entity.createTime = DateTime.Now;

            var type = await _coreCmsAdvertPositionServices.QueryByIdAsync(entity.positionId);
            if (type != null) entity.code = type.code;
            var bl = await _coreCmsAdvertisementServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsAdvertisement/GetEdit
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

            var model = await _coreCmsAdvertisementServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;
            var advertPosition = await _coreCmsAdvertPositionServices.QueryAsync();
            var types = EnumHelper.EnumToList<GlobalEnumVars.AdvertPositionType>();

            jm.data = new
            {
                model,
                advertPosition,
                types
            };


            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Admins/CoreCmsAdvertisement/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsAdvertisement entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsAdvertisementServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            //事物处理过程开始
            //oldModel.id = entity.id;
            if (oldModel.positionId != entity.positionId)
            {
                oldModel.positionId = entity.positionId;
                var type = _coreCmsAdvertPositionServices.QueryById(entity.positionId);
                if (type != null) oldModel.code = type.code;
            }

            oldModel.name = entity.name;
            oldModel.imageUrl = entity.imageUrl;
            oldModel.val = entity.val;
            oldModel.valDes = entity.valDes;
            oldModel.sort = entity.sort;
            //oldModel.createTime = entity.createTime;
            oldModel.updateTime = DateTime.Now;
            oldModel.type = entity.type;

            //事物处理过程结束
            var bl = await _coreCmsAdvertisementServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsAdvertisement/DoDelete/10
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

            var model = await _coreCmsAdvertisementServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var bl = await _coreCmsAdvertisementServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
        }

        #endregion

        #region 获取商品列表

        // POST: Api/CoreCmsAdvertisement/GetGoods
        /// <summary>
        ///     获取商品列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取商品列表")]
        public async Task<AdminUiCallBack> GetGoods()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsGoods>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsGoods, object>> orderEx = p => p.createTime;
            //设置排序方式
            var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
            var orderBy = orderDirection switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };
            //查询筛选

            //商品编码 nvarchar
            var bn = Request.Form["bn"].FirstOrDefault();
            if (!string.IsNullOrEmpty(bn)) @where = @where.And(p => p.bn.Contains(bn));
            //商品名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            where = where.And(p => p.isDel == false);
            //获取数据
            var list = await _coreCmsGoodsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 获取文章列表

        // POST: Api/CoreCmsAdvertisement/GetArticle
        /// <summary>
        ///     获取文章列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取文章列表")]
        public async Task<AdminUiCallBack> GetArticle()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsArticle>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsArticle, object>> orderEx = p => p.id;
            //设置排序方式
            var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
            var orderBy = orderDirection switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };
            //查询筛选

            //标题 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title)) @where = @where.And(p => p.title.Contains(title));
            where = where.And(p => p.isDel == false);
            //获取数据
            var list = await _coreCmsArticleServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 获取文章分类列表

        // POST: Api/CoreCmsAdvertisement/GetArticleType
        /// <summary>
        ///     获取文章分类列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取文章分类列表")]
        public async Task<AdminUiCallBack> GetArticleType()
        {
            var jm = new AdminUiCallBack();
            //获取数据
            var list = await _coreCmsArticleTypeServices.QueryAsync();
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion
    }
}