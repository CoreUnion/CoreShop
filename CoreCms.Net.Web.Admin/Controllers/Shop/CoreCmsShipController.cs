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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     配送方式表
    /// </summary>
    [Description("配送方式表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsShipController : ControllerBase
    {
        private readonly ICoreCmsShipServices _coreCmsShipServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsLogisticsServices _logisticsServices;
        private readonly ICoreCmsAreaServices _areaServices;

        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsShipController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsShipServices coreCmsShipServices
            , ICoreCmsLogisticsServices logisticsServices, ICoreCmsAreaServices areaServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsShipServices = coreCmsShipServices;
            _logisticsServices = logisticsServices;
            _areaServices = areaServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsShip/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsShip>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsShip, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "isCashOnDelivery":
                    orderEx = p => p.isCashOnDelivery;
                    break;
                case "firstUnit":
                    orderEx = p => p.firstUnit;
                    break;
                case "continueUnit":
                    orderEx = p => p.continueUnit;
                    break;
                case "isdefaultAreaFee":
                    orderEx = p => p.isdefaultAreaFee;
                    break;
                case "areaType":
                    orderEx = p => p.areaType;
                    break;
                case "firstunitPrice":
                    orderEx = p => p.firstunitPrice;
                    break;
                case "continueunitPrice":
                    orderEx = p => p.continueunitPrice;
                    break;
                case "exp":
                    orderEx = p => p.exp;
                    break;
                case "logiName":
                    orderEx = p => p.logiName;
                    break;
                case "logiCode":
                    orderEx = p => p.logiCode;
                    break;
                case "isDefault":
                    orderEx = p => p.isDefault;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "status":
                    orderEx = p => p.status;
                    break;
                case "isfreePostage":
                    orderEx = p => p.isfreePostage;
                    break;
                case "areaFee":
                    orderEx = p => p.areaFee;
                    break;
                case "goodsMoney":
                    orderEx = p => p.goodsMoney;
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
            //配送方式名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //是否货到付款 bit
            var isCashOnDelivery = Request.Form["isCashOnDelivery"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isCashOnDelivery) && isCashOnDelivery.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isCashOnDelivery);
            else if (!string.IsNullOrEmpty(isCashOnDelivery) && isCashOnDelivery.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isCashOnDelivery == false);
            //首重 int
            var firstUnit = Request.Form["firstUnit"].FirstOrDefault().ObjectToInt(0);
            if (firstUnit > 0) @where = @where.And(p => p.firstUnit == firstUnit);
            //续重 int
            var continueUnit = Request.Form["continueUnit"].FirstOrDefault().ObjectToInt(0);
            if (continueUnit > 0) @where = @where.And(p => p.continueUnit == continueUnit);
            //是否按地区设置配送费用是否启用默认配送费用 bit
            var isdefaultAreaFee = Request.Form["isdefaultAreaFee"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isdefaultAreaFee) && isdefaultAreaFee.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isdefaultAreaFee);
            else if (!string.IsNullOrEmpty(isdefaultAreaFee) && isdefaultAreaFee.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isdefaultAreaFee == false);
            //地区类型 int
            var areaType = Request.Form["areaType"].FirstOrDefault().ObjectToInt(0);
            if (areaType > 0) @where = @where.And(p => p.areaType == areaType);
            //配送费用计算表达式 nvarchar
            var exp = Request.Form["exp"].FirstOrDefault();
            if (!string.IsNullOrEmpty(exp)) @where = @where.And(p => p.exp.Contains(exp));
            //物流公司名称 nvarchar
            var logiName = Request.Form["logiName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiName)) @where = @where.And(p => p.logiName.Contains(logiName));
            //物流公司编码 nvarchar
            var logiCode = Request.Form["logiCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(logiCode)) @where = @where.And(p => p.logiCode.Contains(logiCode));
            //是否默认 bit
            var isDefault = Request.Form["isDefault"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isDefault) && isDefault.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isDefault);
            else if (!string.IsNullOrEmpty(isDefault) && isDefault.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isDefault == false);
            //配送方式排序 越小越靠前 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0) @where = @where.And(p => p.sort == sort);
            //状态 1=正常 2=停用 int
            var status = Request.Form["status"].FirstOrDefault().ObjectToInt(0);
            if (status > 0) @where = @where.And(p => p.status == status);
            //是否包邮 bit
            var isfreePostage = Request.Form["isfreePostage"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isfreePostage) && isfreePostage.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isfreePostage);
            else if (!string.IsNullOrEmpty(isfreePostage) && isfreePostage.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isfreePostage == false);
            //地区配送费用 nvarchar
            var areaFee = Request.Form["areaFee"].FirstOrDefault();
            if (!string.IsNullOrEmpty(areaFee)) @where = @where.And(p => p.areaFee.Contains(areaFee));
            //获取数据
            var list = await _coreCmsShipServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }

        #endregion

        #region 首页数据============================================================

        // POST: Api/CoreCmsShip/GetIndex
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

        // POST: Api/CoreCmsShip/GetCreate
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

            var shipUnit = EnumHelper.EnumToList<GlobalEnumVars.ShipUnit>();
            var logistics = await _logisticsServices.QueryListByClauseAsync(p => 1 == 1, p => p.sort, OrderByType.Asc);
            jm.data = new
            {
                shipUnit,
                logistics,
            };
            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsShip/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsShip entity)
        {
            var jm = await _coreCmsShipServices.InsertAsync(entity);
            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsShip/GetEdit
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

            var model = await _coreCmsShipServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;

            var shipUnit = EnumHelper.EnumToList<GlobalEnumVars.ShipUnit>();
            var logistics = await _logisticsServices.QueryListByClauseAsync(p => true, p => p.sort, OrderByType.Asc);

            if (!string.IsNullOrEmpty(model.areaFee))
            {
                model.areaFeeObj = JsonConvert.DeserializeObject(model.areaFee);
            }
            jm.data = new
            {
                shipUnit,
                logistics,
                model
            };
            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Api/CoreCmsShip/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsShip entity)
        {
            var jm = await _coreCmsShipServices.UpdateAsync(entity);
            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsShip/DoDelete/10
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

            var model = await _coreCmsShipServices.ExistsAsync(p => p.id == entity.id, true);
            if (!model)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            jm = await _coreCmsShipServices.DeleteByIdAsync(entity.id);

            return jm;

        }

        #endregion

        #region 设置是否货到付款============================================================

        // POST: Api/CoreCmsShip/DoSetisCashOnDelivery/10
        /// <summary>
        ///     设置是否货到付款
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否货到付款")]
        public async Task<AdminUiCallBack> DoSetisCashOnDelivery([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsShipServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isCashOnDelivery = (bool)entity.data;

            jm = await _coreCmsShipServices.UpdateAsync(oldModel);

            return jm;
        }

        #endregion

        #region 设置是否按地区设置配送费用============================================================

        // POST: Api/CoreCmsShip/DoSetisdefaultAreaFee/10
        /// <summary>
        ///     设置是否按地区设置配送费用
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否按地区设置配送费用")]
        public async Task<AdminUiCallBack> DoSetisdefaultAreaFee([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsShipServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isdefaultAreaFee = (bool)entity.data;

            jm = await _coreCmsShipServices.UpdateAsync(oldModel);

            return jm;
        }

        #endregion

        #region 设置是否默认============================================================

        // POST: Api/CoreCmsShip/DoSetisDefault/10
        /// <summary>
        ///     设置是否默认
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否默认")]
        public async Task<AdminUiCallBack> DoSetisDefault([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsShipServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isDefault = (bool)entity.data;

            jm = await _coreCmsShipServices.UpdateAsync(oldModel);

            return jm;
        }

        #endregion

        #region 设置是否包邮============================================================

        // POST: Api/CoreCmsShip/DoSetisfreePostage/10
        /// <summary>
        ///     设置是否包邮
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否包邮")]
        public async Task<AdminUiCallBack> DoSetisfreePostage([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsShipServices.QueryByIdAsync(entity.id, false);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isfreePostage = (bool)entity.data;

            jm = await _coreCmsShipServices.UpdateAsync(oldModel);

            return jm;
        }

        #endregion
    }
}