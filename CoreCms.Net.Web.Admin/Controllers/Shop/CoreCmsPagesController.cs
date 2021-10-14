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
using Newtonsoft.Json;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     页面设计
    /// </summary>
    [Description("页面设计")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsPagesController : ControllerBase
    {
        private readonly ICoreCmsArticleTypeServices _articleTypeServices;
        private readonly ICoreCmsBrandServices _brandServices;
        private readonly ICoreCmsPagesServices _coreCmsPagesServices;
        private readonly ICoreCmsGoodsCategoryServices _goodsCategoryServices;
        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly ICoreCmsPagesItemsServices _pagesItemsServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsPagesServices"></param>
        /// <param name="brandServices"></param>
        /// <param name="goodsServices"></param>
        /// <param name="pagesItemsServices"></param>
        /// <param name="goodsCategoryServices"></param>
        /// <param name="articleTypeServices"></param>
        public CoreCmsPagesController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsPagesServices coreCmsPagesServices
            , ICoreCmsBrandServices brandServices
            , ICoreCmsGoodsServices goodsServices
            , ICoreCmsPagesItemsServices pagesItemsServices
            , ICoreCmsGoodsCategoryServices goodsCategoryServices
            , ICoreCmsArticleTypeServices articleTypeServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsPagesServices = coreCmsPagesServices;
            _brandServices = brandServices;
            _goodsServices = goodsServices;
            _pagesItemsServices = pagesItemsServices;
            _goodsCategoryServices = goodsCategoryServices;
            _articleTypeServices = articleTypeServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsPages/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsPages>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsPages, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "code":
                    orderEx = p => p.code;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "description":
                    orderEx = p => p.description;
                    break;
                case "layout":
                    orderEx = p => p.layout;
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

            // int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0) @where = @where.And(p => p.id == id);
            //可视化区域编码 nvarchar
            var code = Request.Form["code"].FirstOrDefault();
            if (!string.IsNullOrEmpty(code)) @where = @where.And(p => p.code.Contains(code));
            //可编辑区域名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //描述 nvarchar
            var description = Request.Form["description"].FirstOrDefault();
            if (!string.IsNullOrEmpty(description)) @where = @where.And(p => p.description.Contains(description));
            //布局样式编码，1，手机端 int
            var layout = Request.Form["layout"].FirstOrDefault().ObjectToInt(0);
            if (layout > 0) @where = @where.And(p => p.layout == layout);
            //1手机端，2PC端 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0) @where = @where.And(p => p.type == type);
            //获取数据
            var list = await _coreCmsPagesServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsPages/GetIndex
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

        // POST: Api/CoreCmsPages/GetCreate
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
            var pagesLayout = EnumHelper.EnumToList<GlobalEnumVars.PagesLayout>();
            jm.data = new
            {
                pagesLayout,
            };

            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsPages/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsPages entity)
        {
            var jm = await _coreCmsPagesServices.InsertAsync(entity);
            return jm;
        }

        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsPages/GetEdit
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

            var model = await _coreCmsPagesServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;

            var pagesLayout = EnumHelper.EnumToList<GlobalEnumVars.PagesLayout>();

            jm.data = new
            {
                model,
                pagesLayout,
            };

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Api/CoreCmsPages/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsPages entity)
        {
            var jm = await _coreCmsPagesServices.UpdateAsync(entity);

            return jm;
        }
        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsPages/DoDelete/10
        /// <summary>
        ///     单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = await _coreCmsPagesServices.DeleteByIdAsync(entity.id);
            return jm;

        }

        #endregion

        #region 复制数据============================================================

        // POST: Api/CoreCmsPages/Copy/10
        /// <summary>
        ///     复制数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("复制数据")]
        public async Task<AdminUiCallBack> DoCopy([FromBody] FMIntId entity)
        {
            var jm = await _coreCmsPagesServices.CopyByIdAsync(entity.id);
            return jm;

        }

        #endregion


        #region 版面设计============================================================

        // POST: Api/CoreCmsPages/GetEdit
        /// <summary>
        ///     版面设计
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("版面设计")]
        public async Task<AdminUiCallBack> GetDesign([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPagesServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;
            //获取品牌数据
            var brandsData = await _brandServices.QueryAsync();
            var brands = brandsData.Select(p => new { p.id, p.name }).ToList();
            //获取分类数据
            var categories = await _goodsCategoryServices.QueryListByClauseAsync(p => p.isShow, p => p.sort,
                OrderByType.Asc);
            //获取文章分类数据
            var articleTypes = await _articleTypeServices.QueryAsync();

            jm.data = new
            {
                model,
                pageCode = model.code,
                brandList = brands,
                categories = GoodsHelper.GetTree(categories),
                articleTypes
            };

            return jm;
        }

        #endregion

        #region 版面设计提交

        // POST: Api/CoreCmsPages/Edit
        /// <summary>
        ///     版面设计提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("版面设计提交")]
        public async Task<AdminUiCallBack> DoDesign([FromBody] FmPagesUpdate entity)
        {
            var jm = await _coreCmsPagesServices.UpdateDesignAsync(entity);
            return jm;
        }

        #endregion

        #region 获取已存储数据

        // POST: Api/CoreCmsPages/GetPageData
        /// <summary>
        ///     获取已存储数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("获取已存储数据")]
        public async Task<AdminUiCallBack> GetPageData([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPagesServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;
            var pageItems = await _pagesItemsServices.QueryListByClauseAsync(p => p.pageCode == model.code, p => p.sort, OrderByType.Asc);

            var noObjArr = new string[] { "textarea" };


            var pageConfig = pageItems.Select(p => new
            {
                type = p.widgetCode,
                value = noObjArr.Contains(p.widgetCode) ? p.parameters : new JsonSerializer().Deserialize(new JsonTextReader(new StringReader(p.parameters)))
            }).ToList();
            jm.data = new
            {
                pageConfig
            };

            return jm;
        }

        #endregion

        #region 获取文章类别
        // POST: Api/CoreCmsPages/GetArticleTypes
        /// <summary>
        ///     获取文章类别
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("获取文章类别")]
        public async Task<AdminUiCallBack> GetArticleTypes([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPagesServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;
            //获取文章分类数据
            var articleTypes = await _articleTypeServices.QueryAsync();
            jm.data = new
            {
                articleTypes
            };

            return jm;
        }

        #endregion

    }
}