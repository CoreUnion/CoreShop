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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
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
    ///     拼团记录表
    /// </summary>
    [Description("拼团记录表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsPinTuanRecordController : ControllerBase
    {
        private readonly ICoreCmsPinTuanRecordServices _coreCmsPinTuanRecordServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsPinTuanRecordServices"></param>
        public CoreCmsPinTuanRecordController(IWebHostEnvironment webHostEnvironment,
            ICoreCmsPinTuanRecordServices coreCmsPinTuanRecordServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsPinTuanRecordServices = coreCmsPinTuanRecordServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsPinTuanRecord/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsPinTuanRecord>();
            where = where.And(p => p.id == p.teamId);

            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsPinTuanRecord, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "teamId":
                    orderEx = p => p.teamId;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "ruleId":
                    orderEx = p => p.ruleId;
                    break;
                case "goodsId":
                    orderEx = p => p.goodsId;
                    break;
                case "status":
                    orderEx = p => p.status;
                    break;
                case "orderId":
                    orderEx = p => p.orderId;
                    break;
                case "parameters":
                    orderEx = p => p.parameters;
                    break;
                case "closeTime":
                    orderEx = p => p.closeTime;
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
            //团序列 int
            var teamId = Request.Form["teamId"].FirstOrDefault().ObjectToInt(0);
            if (teamId > 0) @where = @where.And(p => p.teamId == teamId);
            //用户序列 int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0) @where = @where.And(p => p.userId == userId);
            //规则表序列 int
            var ruleId = Request.Form["ruleId"].FirstOrDefault().ObjectToInt(0);
            if (ruleId > 0) @where = @where.And(p => p.ruleId == ruleId);
            //商品序列 int
            var goodsId = Request.Form["goodsId"].FirstOrDefault().ObjectToInt(0);
            if (goodsId > 0) @where = @where.And(p => p.goodsId == goodsId);
            //状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) @where = @where.And(p => p.status == status);
            //订单序列 nvarchar
            var orderId = Request.Form["orderId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(orderId)) @where = @where.And(p => p.orderId.Contains(orderId));
            //拼团人数Json nvarchar
            var parameters = Request.Form["parameters"].FirstOrDefault();
            if (!string.IsNullOrEmpty(parameters)) @where = @where.And(p => p.parameters.Contains(parameters));
            //关闭时间 datetime
            var closeTime = Request.Form["closeTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(closeTime))
            {
                if (closeTime.Contains("到"))
                {
                    var dts = closeTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.closeTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.closeTime < dtEnd);
                }
                else
                {
                    var dt = closeTime.ObjectToDate();
                    where = where.And(p => p.closeTime > dt);
                }
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
            var list = await _coreCmsPinTuanRecordServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent,
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

        // POST: Api/CoreCmsPinTuanRecord/GetIndex
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

            var status = EnumHelper.EnumToList<GlobalEnumVars.PinTuanRecordStatus>();
            jm.data = new
            {
                status
            };
            return jm;
        }

        #endregion
    }
}