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
using CoreCms.Net.Services;
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
    /// 代理商品池
    ///</summary>
    [Description("代理商品池")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsAgentGoodsController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsAgentGoodsServices _agentGoodsServices;
        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly ICoreCmsAgentGradeServices _agentGradeServices;
        private readonly ICoreCmsAgentProductsServices _agentProductsServices;
        private readonly ICoreCmsProductsServices _productsServices;


        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsAgentGoodsController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsAgentGoodsServices agentGoodsServices, ICoreCmsGoodsServices goodsServices, ICoreCmsAgentGradeServices agentGradeServices, ICoreCmsAgentProductsServices agentProductsServices, ICoreCmsProductsServices productsServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _agentGoodsServices = agentGoodsServices;
            _goodsServices = goodsServices;
            _agentGradeServices = agentGradeServices;
            _agentProductsServices = agentProductsServices;
            _productsServices = productsServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsAgentGoods/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsAgentGoods>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<CoreCmsAgentGoods, object>> orderEx = orderField switch
            {
                "id" => p => p.id,
                "goodId" => p => p.goodId,
                "sortId" => p => p.sortId,
                "isEnable" => p => p.isEnable,
                "createTime" => p => p.createTime,
                "updateTime" => p => p.updateTime,
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

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //商品序列 int
            var goodId = Request.Form["goodId"].FirstOrDefault().ObjectToInt(0);
            if (goodId > 0)
            {
                where = where.And(p => p.goodId == goodId);
            }
            //排序 int
            var sortId = Request.Form["sortId"].FirstOrDefault().ObjectToInt(0);
            if (sortId > 0)
            {
                where = where.And(p => p.sortId == sortId);
            }
            //是否启用 bit
            var isEnable = Request.Form["isEnable"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isEnable) && isEnable.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isEnable == true);
            }
            else if (!string.IsNullOrEmpty(isEnable) && isEnable.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isEnable == false);
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
            //最后更新时间 datetime
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
            var list = await _agentGoodsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsAgentGoods/GetIndex
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
            return jm;
        }
        #endregion

        #region 创建数据============================================================
        // POST: Api/CoreCmsAgentGoods/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public async Task<AdminUiCallBack> GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var goods = await _goodsServices.QueryEnumEntityAsync();
            var agentGrade = await _agentGradeServices.GetCaChe();
            jm.data = new
            {
                goods,
                agentGrade
            };

            return jm;
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsAgentGoods/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] FMCreateAgentGood entity)
        {
            var jm = new AdminUiCallBack();
            jm = await _agentGoodsServices.InsertAsync(entity.good, entity.products);
            jm.data = entity;

            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsAgentGoods/GetEdit
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

            var model = await _agentGoodsServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;

            var goods = await _goodsServices.QueryEnumEntityAsync();
            var good = await _goodsServices.QueryByClauseAsync(p => p.id == model.goodId);
            var products = await _productsServices.QueryListByClauseAsync(p => p.goodsId == model.goodId && p.isDel == false);
            var agentGrade = await _agentGradeServices.GetCaChe();
            var agentProducts = await _agentProductsServices.QueryListByClauseAsync(p => p.goodId == model.goodId, p => p.id, OrderByType.Asc);

            var allNew = false;
            if (agentProducts.Any() && products.Any())
            {
                var pIds = products.Select(p => p.id).ToList();
                var apIds = agentProducts.Select(p => p.productId).ToList();
                var sameArr = pIds.Intersect(apIds).ToArray();
                allNew = sameArr.Length <= 0;
            }

            jm.data = new
            {
                model,
                goods,
                good,
                agentGrade,
                agentProducts,
                products,
                allNew
            };

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Api/CoreCmsAgentGoods/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] FMCreateAgentGood entity)
        {
            var jm = await _agentGoodsServices.UpdateAsync(entity.good, entity.products);
            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsAgentGoods/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = await _agentGoodsServices.DeleteByIdAsync(entity.id);

            return jm;
        }
        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsAgentGoods/GetDetails/10
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _agentGoodsServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;

            var goods = await _goodsServices.QueryByClauseAsync(p => p.id == model.goodId);
            var products = await _productsServices.QueryListByClauseAsync(p => p.goodsId == model.goodId && p.isDel == false);
            var agentGrade = await _agentGradeServices.GetCaChe();
            var agentProducts = await _agentProductsServices.QueryListByClauseAsync(p => p.goodId == model.goodId, p => p.id, OrderByType.Asc);

            jm.data = new
            {
                model,
                goods,
                agentGrade,
                agentProducts,
                products
            };

            return jm;
        }
        #endregion

        #region 设置是否启用============================================================
        // POST: Api/CoreCmsAgentGoods/DoSetisEnable/10
        /// <summary>
        /// 设置是否启用
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否启用")]
        public async Task<AdminUiCallBack> DoSetisEnable([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _agentGoodsServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isEnable = (bool)entity.data;

            var bl = await _agentGoodsServices.UpdateAsync(p => new CoreCmsAgentGoods() { isEnable = oldModel.isEnable }, p => p.id == oldModel.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion


    }
}
