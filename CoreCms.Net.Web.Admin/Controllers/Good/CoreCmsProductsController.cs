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
    /// 货品表
    ///</summary>
    [Description("货品表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsProductsController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsProductsServices _coreCmsProductsServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsProductsController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsProductsServices coreCmsProductsServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsProductsServices = coreCmsProductsServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsProducts/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsProducts>();
            where = where.And(p => p.isDel == false);


            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<CoreCmsProducts, object>> orderEx = orderField switch
            {
                "id" => p => p.id,
                "goodsId" => p => p.goodsId,
                "barcode" => p => p.barcode,
                "sn" => p => p.sn,
                "price" => p => p.price,
                "costprice" => p => p.costprice,
                "mktprice" => p => p.mktprice,
                "marketable" => p => p.marketable,
                "weight" => p => p.weight,
                "stock" => p => p.stock,
                "freezeStock" => p => p.freezeStock,
                "spesDesc" => p => p.spesDesc,
                "isDefalut" => p => p.isDefalut,
                "images" => p => p.images,
                "isDel" => p => p.isDel,
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


            //货品条码 nvarchar
            var barcode = Request.Form["barcode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(barcode))
            {
                where = where.And(p => p.barcode.Contains(barcode));
            }
            //商品编码 nvarchar
            var sn = Request.Form["sn"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sn))
            {
                where = where.And(p => p.sn.Contains(sn));
            }
            //规格值 nvarchar
            var spesDesc = Request.Form["spesDesc"].FirstOrDefault();
            if (!string.IsNullOrEmpty(spesDesc))
            {
                where = where.And(p => p.spesDesc.Contains(spesDesc));
            }
            //规格值 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name))
            {
                where = where.And(p => p.name.Contains(name));
            }

            //获取数据
            var list = await _coreCmsProductsServices.QueryDetailPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsProducts/GetIndex
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


        #region 预览数据============================================================
        // POST: Api/CoreCmsProducts/GetDetails/10
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

            var model = await _coreCmsProductsServices.QueryByClauseAsync(p => p.id == entity.id);
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

        #region 设置库存============================================================
        // POST: Api/CoreCmsProducts/DoSetStock/10
        /// <summary>
        /// 设置库存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置库存")]
        public async Task<AdminUiCallBack> DoSetStock([FromBody] FMUpdateIntegerDataByIntId entity)
        {
            var jm = await _coreCmsProductsServices.EditStock(entity.id, entity.data);

            return jm;
        }
        #endregion


    }
}
