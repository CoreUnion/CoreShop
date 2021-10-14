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
    /// 库存操作详情表
    ///</summary>
    [Description("库存操作详情表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsStockLogController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsStockLogServices _coreCmsStockLogServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsStockLogController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsStockLogServices coreCmsStockLogServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsStockLogServices = coreCmsStockLogServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsStockLog/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsStockLog>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<CoreCmsStockLog, object>> orderEx = orderField switch
            {
                "id" => p => p.id,
                "stockId" => p => p.stockId,
                "productId" => p => p.productId,
                "goodsId" => p => p.goodsId,
                "nums" => p => p.nums,
                "sn" => p => p.sn,
                "bn" => p => p.bn,
                "goodsName" => p => p.goodsName,
                "spesDesc" => p => p.spesDesc,
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
            //库存单号 nvarchar
            var stockId = Request.Form["stockId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(stockId))
            {
                where = where.And(p => p.stockId.Contains(stockId));
            }
            //货品序列 int
            var productId = Request.Form["productId"].FirstOrDefault().ObjectToInt(0);
            if (productId > 0)
            {
                where = where.And(p => p.productId == productId);
            }
            //商品序列 int
            var goodsId = Request.Form["goodsId"].FirstOrDefault().ObjectToInt(0);
            if (goodsId > 0)
            {
                where = where.And(p => p.goodsId == goodsId);
            }
            //数量 int
            var nums = Request.Form["nums"].FirstOrDefault().ObjectToInt(0);
            if (nums > 0)
            {
                where = where.And(p => p.nums == nums);
            }
            //货品编码 nvarchar
            var sn = Request.Form["sn"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sn))
            {
                where = where.And(p => p.sn.Contains(sn));
            }
            //商品编码 nvarchar
            var bn = Request.Form["bn"].FirstOrDefault();
            if (!string.IsNullOrEmpty(bn))
            {
                where = where.And(p => p.bn.Contains(bn));
            }
            //商品名称 nvarchar
            var goodsName = Request.Form["goodsName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(goodsName))
            {
                where = where.And(p => p.goodsName.Contains(goodsName));
            }
            //货品明细序列号存储 nvarchar
            var spesDesc = Request.Form["spesDesc"].FirstOrDefault();
            if (!string.IsNullOrEmpty(spesDesc))
            {
                where = where.And(p => p.spesDesc.Contains(spesDesc));
            }
            //操作类型 int
            var type = Request.Form["type"].FirstOrDefault().ObjectToInt(0);
            if (type > 0)
            {
                where = where.And(p => p.type == type);
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


            //获取数据
            var list = await _coreCmsStockLogServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsStockLog/GetIndex
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

            var stockType = EnumHelper.EnumToList<GlobalEnumVars.StockType>();
            jm.data = new
            {
                stockType
            };

            return jm;
        }
        #endregion

        #region 创建数据============================================================
        // POST: Api/CoreCmsStockLog/GetCreate
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

    }
}
