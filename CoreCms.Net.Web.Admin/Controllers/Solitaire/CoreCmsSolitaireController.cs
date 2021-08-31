/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/6/14 23:17:57
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
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.ViewModels.UI;
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
    /// 接龙活动表
    ///</summary>
    [Description("接龙活动表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize]
    public class CoreCmsSolitaireController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsSolitaireServices _solitaireServices;
        private readonly ICoreCmsSolitaireItemsServices _itemsServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsSolitaireController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsSolitaireServices solitaireServices, ICoreCmsSolitaireItemsServices itemsServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _solitaireServices = solitaireServices;
            _itemsServices = itemsServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsSolitaire/GetPageList
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<JsonResult> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsSolitaire>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<CoreCmsSolitaire, object>> orderEx = orderField switch
            {
                "id" => p => p.id,
                "title" => p => p.title,
                "contentBody" => p => p.contentBody,
                "startTime" => p => p.startTime,
                "endTime" => p => p.endTime,
                "startBuyPrice" => p => p.startBuyPrice,
                "minDeliveryPrice" => p => p.minDeliveryPrice,
                "isShow" => p => p.isShow,
                "status" => p => p.status,
                "isDelete" => p => p.isDelete,
                "createTime" => p => p.createTime,
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
            //活动标题 nvarchar
            var title = Request.Form["title"].FirstOrDefault();
            if (!string.IsNullOrEmpty(title))
            {
                where = where.And(p => p.title.Contains(title));
            }
            //活动内容 nvarchar
            var contentBody = Request.Form["contentBody"].FirstOrDefault();
            if (!string.IsNullOrEmpty(contentBody))
            {
                where = where.And(p => p.contentBody.Contains(contentBody));
            }
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
                    where = where.And(p => p.startTime < dtEnd);
                }
                else
                {
                    var dt = startTime.ObjectToDate();
                    where = where.And(p => p.startTime > dt);
                }
            }
            //结束时间 datetime
            var endTime = Request.Form["endTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(endTime))
            {
                if (endTime.Contains("到"))
                {
                    var dts = endTime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.endTime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.endTime < dtEnd);
                }
                else
                {
                    var dt = endTime.ObjectToDate();
                    where = where.And(p => p.endTime > dt);
                }
            }
            //是否显示 bit
            var isShow = Request.Form["isShow"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isShow) && isShow.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isShow == true);
            }
            else if (!string.IsNullOrEmpty(isShow) && isShow.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isShow == false);
            }
            //活动状态 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0)
            {
                where = where.And(p => p.status == status);
            }
            //标注删除 bit
            var isDelete = Request.Form["isDelete"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isDelete == true);
            }
            else if (!string.IsNullOrEmpty(isDelete) && isDelete.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isDelete == false);
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
            var list = await _solitaireServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return Json(jm);
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsSolitaire/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public JsonResult GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            var solitaireStatus = EnumHelper.EnumToList<GlobalEnumVars.SolitaireStatus>();
            jm.data = new
            {
                solitaireStatus
            };
            return Json(jm);
        }
        #endregion

        #region 创建数据============================================================
        // POST: Api/CoreCmsSolitaire/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public JsonResult GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var solitaireStatus = EnumHelper.EnumToList<GlobalEnumVars.SolitaireStatus>();
            jm.data = new
            {
                solitaireStatus
            };

            return Json(jm);
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsSolitaire/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<JsonResult> DoCreate([FromBody] CoreCmsSolitaire entity)
        {
            var jm = await _solitaireServices.InsertAsync(entity);
            return Json(jm);
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsSolitaire/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<JsonResult> GetEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _solitaireServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return Json(jm);
            }

            var items = await _itemsServices.QueryListByClauseAsync(p => p.solitaireId == model.id && p.isDelete == false, p => p.sortId, OrderByType.Asc);
            var goodsId = items.Select(p => p.productId).ToList();
            var solitaireStatus = EnumHelper.EnumToList<GlobalEnumVars.SolitaireStatus>();
            jm.data = new
            {
                model,
                items,
                goodsId,
                solitaireStatus
            };
            jm.code = 0;

            return Json(jm);
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Api/CoreCmsSolitaire/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<JsonResult> DoEdit([FromBody] CoreCmsSolitaire entity)
        {
            var jm = await _solitaireServices.UpdateAsync(entity);
            jm.otherData = entity;
            return Json(jm);
        }
        #endregion




        #region 删除数据============================================================
        // POST: Api/CoreCmsSolitaire/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<JsonResult> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _solitaireServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return Json(jm);
            }
            //jm = await _coreCmsSolitaireServices.DeleteByIdAsync(entity.id);
            var bl = await _solitaireServices.UpdateAsync(p => new CoreCmsSolitaire() { isDelete = true }, p => p.id == oldModel.id);

            return Json(jm);
        }
        #endregion

        #region 设置是否显示============================================================
        // POST: Api/CoreCmsSolitaire/DoSetisShow/10
        /// <summary>
        /// 设置是否显示
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否显示")]
        public async Task<JsonResult> DoSetisShow([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _solitaireServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return Json(jm);
            }
            oldModel.isShow = (bool)entity.data;

            var bl = await _solitaireServices.UpdateAsync(p => new CoreCmsSolitaire() { isShow = oldModel.isShow }, p => p.id == oldModel.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return Json(jm);
        }
        #endregion

        #region 设置标注删除============================================================
        // POST: Api/CoreCmsSolitaire/DoSetisDelete/10
        /// <summary>
        /// 设置标注删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置标注删除")]
        public async Task<JsonResult> DoSetisDelete([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _solitaireServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return Json(jm);
            }
            oldModel.isDelete = (bool)entity.data;

            var bl = await _solitaireServices.UpdateAsync(p => new CoreCmsSolitaire() { isDelete = oldModel.isDelete }, p => p.id == oldModel.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return Json(jm);
        }
        #endregion


    }
}
