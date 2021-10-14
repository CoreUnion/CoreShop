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
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     拼团规则表
    /// </summary>
    [Description("拼团规则表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsPinTuanRuleController : ControllerBase
    {
        private readonly ICoreCmsGoodsServices _coreCmsGoodsServices;
        private readonly ICoreCmsPinTuanGoodsServices _coreCmsPinTuanGoodsServices;
        private readonly ICoreCmsPinTuanRuleServices _coreCmsPinTuanRuleServices;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="coreCmsPinTuanRuleServices"></param>
        /// <param name="coreCmsGoodsServices"></param>
        /// <param name="coreCmsPinTuanGoodsServices"></param>
        public CoreCmsPinTuanRuleController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsPinTuanRuleServices coreCmsPinTuanRuleServices
            , ICoreCmsGoodsServices coreCmsGoodsServices
            , ICoreCmsPinTuanGoodsServices coreCmsPinTuanGoodsServices
        )
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsPinTuanRuleServices = coreCmsPinTuanRuleServices;
            _coreCmsGoodsServices = coreCmsGoodsServices;
            _coreCmsPinTuanGoodsServices = coreCmsPinTuanGoodsServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsPinTuanRule/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsPinTuanRule>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsPinTuanRule, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "startTime":
                    orderEx = p => p.startTime;
                    break;
                case "endTime":
                    orderEx = p => p.endTime;
                    break;
                case "peopleNumber":
                    orderEx = p => p.peopleNumber;
                    break;
                case "significantInterval":
                    orderEx = p => p.significantInterval;
                    break;
                case "discountAmount":
                    orderEx = p => p.discountAmount;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "isStatusOpen":
                    orderEx = p => p.isStatusOpen;
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
            //活动名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //开始时间 datetime
            var startTime = Request.Form["startTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(startTime))
            {
                if (startTime.Contains("到"))
                {
                    var dts = startTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.startTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.endTime < dtEnd);
                }
                else
                {
                    var dt = startTime.ObjectToDate();
                    where = where.And(p => p.startTime > dt);
                }
            }


            //是否开启 bit
            var isStatusOpen = Request.Form["isStatusOpen"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isStatusOpen) && isStatusOpen.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isStatusOpen);
            else if (!string.IsNullOrEmpty(isStatusOpen) && isStatusOpen.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isStatusOpen == false);
            //获取数据
            var list = await _coreCmsPinTuanRuleServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsPinTuanRule/GetIndex
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

        // POST: Api/CoreCmsPinTuanRule/GetCreate
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
            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsPinTuanRule/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsPinTuanRule entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.peopleNumber < 2 || entity.peopleNumber > 10)
            {
                jm.msg = "拼团人数只允许2至10人";
                return jm;
            }

            entity.createTime = DateTime.Now;
            var id = await _coreCmsPinTuanRuleServices.InsertAsync(entity);
            var bl = id > 0;
            if (bl && entity.goods.Any())
            {
                var list = new List<CoreCmsPinTuanGoods>();
                foreach (var good in entity.goods)
                    list.Add(new CoreCmsPinTuanGoods
                    {
                        goodsId = good,
                        ruleId = id
                    });
                await _coreCmsPinTuanGoodsServices.InsertAsync(list);
            }

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsPinTuanRule/GetEdit
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

            var model = await _coreCmsPinTuanRuleServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;

            var pinTuanGoods = await _coreCmsPinTuanGoodsServices.QueryListByClauseAsync(p => p.ruleId == model.id);
            var Ids = pinTuanGoods.Select(p => p.goodsId).ToArray();
            var goods = await _coreCmsGoodsServices.QueryListByClauseAsync(p => Ids.Contains(p.id));
            jm.data = new
            {
                model,
                goods
            };

            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Admins/CoreCmsPinTuanRule/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsPinTuanRule entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.peopleNumber < 2 || entity.peopleNumber > 10)
            {
                jm.msg = "拼团人数只允许2至10人";
                return jm;
            }

            var oldModel = await _coreCmsPinTuanRuleServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            //事物处理过程开始
            oldModel.name = entity.name;
            oldModel.startTime = entity.startTime;
            oldModel.endTime = entity.endTime;
            oldModel.peopleNumber = entity.peopleNumber;
            oldModel.significantInterval = entity.significantInterval;
            oldModel.discountAmount = entity.discountAmount;
            oldModel.sort = entity.sort;
            oldModel.isStatusOpen = entity.isStatusOpen;
            oldModel.maxGoodsNums = entity.maxGoodsNums;
            oldModel.maxNums = entity.maxNums;
            oldModel.updateTime = DateTime.Now;

            //事物处理过程结束
            var bl = await _coreCmsPinTuanRuleServices.UpdateAsync(oldModel);
            if (bl && entity.goods.Any())
            {
                await _coreCmsPinTuanGoodsServices.DeleteAsync(p => p.ruleId == oldModel.id);
                var list = new List<CoreCmsPinTuanGoods>();
                foreach (var good in entity.goods)
                    list.Add(new CoreCmsPinTuanGoods
                    {
                        goodsId = good,
                        ruleId = oldModel.id
                    });
                await _coreCmsPinTuanGoodsServices.InsertAsync(list);
            }

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;


            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsPinTuanRule/DoDelete/10
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

            var model = await _coreCmsPinTuanRuleServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var bl = await _coreCmsPinTuanRuleServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }

        #endregion

        #region 设置是否开启============================================================

        // POST: Api/CoreCmsPinTuanRule/DoSetisStatusOpen/10
        /// <summary>
        ///     设置是否开启
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否开启")]
        public async Task<AdminUiCallBack> DoSetisStatusOpen([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsPinTuanRuleServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isStatusOpen = entity.data;

            var bl = await _coreCmsPinTuanRuleServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion
    }
}