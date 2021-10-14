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
using NPOI.HSSF.UserModel;
using SqlSugar;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 促销表
    ///</summary>
    [Description("促销表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsPromotionController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsPromotionServices _coreCmsPromotionServices;
        private readonly ICoreCmsPromotionConditionServices _promotionConditionServices;
        private readonly ICoreCmsPromotionResultServices _promotionResultServices;
        private readonly ICoreCmsGoodsCategoryServices _coreCmsGoodsCategoryServices;
        private readonly ICoreCmsBrandServices _coreCmsBrandServices;
        private readonly ICoreCmsUserGradeServices _coreCmsUserGradeServices;
        private readonly ICoreCmsPromotionConditionServices _coreCmsPromotionConditionServices;
        private readonly ICoreCmsPromotionResultServices _coreCmsPromotionResultServices;
        private readonly ICoreCmsCouponServices _coreCmsCouponServices;
        private readonly ICoreCmsGoodsServices _goodsServices;


        ///  <summary>
        ///  构造函数
        /// </summary>
        public CoreCmsPromotionController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsPromotionServices coreCmsPromotionServices
            , ICoreCmsPromotionConditionServices promotionConditionServices
            , ICoreCmsPromotionResultServices promotionResultServices
            , ICoreCmsGoodsCategoryServices coreCmsGoodsCategoryServices
            , ICoreCmsBrandServices coreCmsBrandServices
            , ICoreCmsUserGradeServices coreCmsUserGradeServices
            , ICoreCmsPromotionConditionServices coreCmsPromotionConditionServices
            , ICoreCmsPromotionResultServices coreCmsPromotionResultServices
            , ICoreCmsCouponServices coreCmsCouponServices, ICoreCmsGoodsServices goodsServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsPromotionServices = coreCmsPromotionServices;
            _promotionConditionServices = promotionConditionServices;
            _promotionResultServices = promotionResultServices;
            _coreCmsGoodsCategoryServices = coreCmsGoodsCategoryServices;
            _coreCmsBrandServices = coreCmsBrandServices;
            _coreCmsUserGradeServices = coreCmsUserGradeServices;
            _coreCmsPromotionConditionServices = coreCmsPromotionConditionServices;
            _coreCmsPromotionResultServices = coreCmsPromotionResultServices;
            _coreCmsCouponServices = coreCmsCouponServices;
            _goodsServices = goodsServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsPromotion/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsPromotion>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsPromotion, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "type":
                    orderEx = p => p.type;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "parameters":
                    orderEx = p => p.parameters;
                    break;
                case "startTime":
                    orderEx = p => p.startTime;
                    break;
                case "endTime":
                    orderEx = p => p.endTime;
                    break;
                case "isExclusive":
                    orderEx = p => p.isExclusive;
                    break;
                case "isAutoReceive":
                    orderEx = p => p.isAutoReceive;
                    break;
                case "isEnable":
                    orderEx = p => p.isEnable;
                    break;
                case "isDel":
                    orderEx = p => p.isDel;
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
            var types = Request.Form["types"].FirstOrDefault().ObjectToInt(0);
            if (types == 1)
            {
                where = where.And(p => p.type == (int)GlobalEnumVars.PromotionType.Promotion);
            }
            else if (types == 2)
            {
                where = where.And(p => p.type == (int)GlobalEnumVars.PromotionType.Coupon);
            }
            else if (types == 3)
            {
                where = where.And(p => p.type == (int)GlobalEnumVars.PromotionType.Group || p.type == (int)GlobalEnumVars.PromotionType.Seckill);
            }
            //促销名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name))
            {
                where = where.And(p => p.name.Contains(name));
            }
            //排序 int
            var sort = Request.Form["sort"].FirstOrDefault().ObjectToInt(0);
            if (sort > 0)
            {
                where = where.And(p => p.sort == sort);
            }
            //其它参数 nvarchar
            var parameters = Request.Form["parameters"].FirstOrDefault();
            if (!string.IsNullOrEmpty(parameters))
            {
                where = where.And(p => p.parameters.Contains(parameters));
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
            //是否排他 bit
            var isExclusive = Request.Form["isExclusive"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isExclusive) && isExclusive.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isExclusive == true);
            }
            else if (!string.IsNullOrEmpty(isExclusive) && isExclusive.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isExclusive == false);
            }

            //是否开启 bit
            var isEnable = Request.Form["isEnable"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isEnable) && isEnable.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isEnable == true);
            }
            else if (!string.IsNullOrEmpty(isEnable) && isEnable.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isEnable == false);
            }
            where = where.And(p => p.isDel == false);
            //获取数据
            var list = await _coreCmsPromotionServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);

            if (list.Any() && types == 3)
            {

            }


            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsPromotion/GetIndex
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
            var promotionType = EnumHelper.EnumToList<GlobalEnumVars.PromotionType>();
            jm.data = new
            {
                promotionType
            };
            return jm;
        }
        #endregion

        #region 创建数据============================================================
        // POST: Api/CoreCmsPromotion/GetCreate
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

        #region 创建提交============================================================
        // POST: Api/CoreCmsPromotion/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] CoreCmsPromotion entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.startTime >= entity.endTime)
            {
                jm.msg = "开始时间必须小于结束时间";
                return jm;
            }

            var bl = await _coreCmsPromotionServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = (bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure);

            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsPromotion/GetEdit
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

            var model = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;

            var conditions = await _promotionConditionServices.QueryListByClauseAsync(p => p.promotionId == model.id);
            var results = await _promotionResultServices.QueryListByClauseAsync(p => p.promotionId == model.id);

            //获取促销添加参数类型字典
            var promotionConditionTypes = SystemSettingDictionary.GetPromotionConditionType();

            //获取促销添加结果类型字典
            var promotionResultTypes = SystemSettingDictionary.GetPromotionResultType();

            jm.data = new
            {
                model,
                conditions,
                results,
                promotionConditionTypes,
                promotionResultTypes
            };

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Admins/CoreCmsPromotion/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] CoreCmsPromotion entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.startTime >= entity.endTime)
            {
                jm.msg = "开始时间必须小于结束时间";
                return jm;
            }

            var oldModel = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            oldModel.name = entity.name;
            oldModel.type = entity.type;
            oldModel.sort = entity.sort;
            oldModel.parameters = entity.parameters;
            oldModel.startTime = entity.startTime;
            oldModel.endTime = entity.endTime;

            oldModel.maxNums = entity.maxNums > 0 ? entity.maxNums : 0;
            oldModel.maxGoodsNums = entity.maxGoodsNums > 0 ? entity.maxGoodsNums : 0;
            oldModel.maxRecevieNums = entity.maxRecevieNums > 0 ? entity.maxRecevieNums : 0;

            oldModel.effectiveDays = entity.effectiveDays;
            oldModel.effectiveHours = entity.effectiveHours;

            if (entity.type == (int)GlobalEnumVars.PromotionType.Promotion)
            {
                oldModel.isExclusive = entity.isExclusive;
            }

            if (entity.type == (int)GlobalEnumVars.PromotionType.Coupon)
            {
                oldModel.isAutoReceive = entity.isAutoReceive;

                if (oldModel.effectiveDays == 0 && oldModel.effectiveHours == 0)
                {
                    jm.msg = "优惠券有效时间不能为0";
                    return jm;
                }
            }

            oldModel.isEnable = entity.isEnable;

            if (oldModel.type == (int)GlobalEnumVars.PromotionType.Group || oldModel.type == (int)GlobalEnumVars.PromotionType.Seckill)
            {
                await _coreCmsPromotionConditionServices.DeleteAsync(p => p.promotionId == oldModel.id && p.code == "GOODS_IDS");
                var coreCmsPromotionResult = new CoreCmsPromotionCondition();
                coreCmsPromotionResult.promotionId = oldModel.id;
                coreCmsPromotionResult.code = "GOODS_IDS";
                coreCmsPromotionResult.parameters = entity.parameters;
                await _coreCmsPromotionConditionServices.InsertAsync(coreCmsPromotionResult);
            }

            //事物处理过程结束
            var bl = await _coreCmsPromotionServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsPromotion/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            model.isDel = true;
            var bl = await _coreCmsPromotionServices.UpdateAsync(model);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }
        #endregion


        #region 设置是否排他============================================================
        // POST: Api/CoreCmsPromotion/DoSetisExclusive/10
        /// <summary>
        /// 设置是否排他
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否排他")]
        public async Task<AdminUiCallBack> DoSetisExclusive([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isExclusive = (bool)entity.data;

            var bl = await _coreCmsPromotionServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 设置是否开启============================================================
        // POST: Api/CoreCmsPromotion/DoSetisEnable/10
        /// <summary>
        /// 设置是否开启
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否开启")]
        public async Task<AdminUiCallBack> DoSetisEnable([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isEnable = (bool)entity.data;

            var bl = await _coreCmsPromotionServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        //促销条件===============================================================================

        #region 添加促销条件===================================================
        // POST: Api/CoreCmsPromotion/GetConditionCreate
        /// <summary>
        /// 添加促销条件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("添加促销条件")]
        public async Task<AdminUiCallBack> GetConditionCreate([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            //返回数据
            jm.code = 0;

            var conditionCodes = SystemSettingDictionary.GetPromotionConditionType();
            //获取商品分类
            var categories = await _coreCmsGoodsCategoryServices.QueryListByClauseAsync(p => p.isShow == true, p => p.sort,
                OrderByType.Asc);
            //获取商品品牌
            var brands = await _coreCmsBrandServices.QueryListByClauseAsync(p => p.isShow == true);
            //获取用户等级
            var grades = await _coreCmsUserGradeServices.QueryAsync();

            jm.data = new
            {
                conditionCodes,
                promotionModel = model,
                categories = GoodsHelper.GetTree(categories, false),
                brands,
                grades
            };
            return jm;

        }



        #endregion

        #region 添加促销条件提交===================================================================
        // POST: Api/CoreCmsPromotion/DoConditionCreate
        /// <summary>
        /// 添加促销条件提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("添加促销条件提交")]
        public async Task<AdminUiCallBack> DoConditionCreate([FromBody] CoreCmsPromotionCondition entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await _coreCmsPromotionConditionServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = (bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure);

            return jm;
        }
        #endregion

        #region 获取促销条件============================================================
        // POST: Api/CoreCmsPromotion/GetConditionList
        /// <summary>
        /// 获取促销条件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取促销条件")]
        public async Task<AdminUiCallBack> GetConditionList()
        {
            var jm = new AdminUiCallBack();
            var where = PredicateBuilder.True<CoreCmsPromotionCondition>();

            //促销ID int
            var promotionId = Request.Form["promotionId"].FirstOrDefault().ObjectToInt(0);
            if (promotionId > 0)
            {
                where = where.And(p => p.promotionId == promotionId);
            }
            //获取数据
            var list = await _coreCmsPromotionConditionServices.QueryListByClauseAsync(p =>
                p.promotionId == promotionId);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 删除促销条件============================================================
        // POST: Api/CoreCmsPromotion/DoConditionDelete/10
        /// <summary>
        /// 删除促销条件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("删除促销条件")]
        public async Task<AdminUiCallBack> DoConditionDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPromotionConditionServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            var bl = await _coreCmsPromotionConditionServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }
        #endregion

        #region 编辑促销数据============================================================
        // POST: Api/CoreCmsPromotion/GetConditionEdit
        /// <summary>
        /// 编辑促销数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑促销数据")]
        public async Task<AdminUiCallBack> GetConditionEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPromotionConditionServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;

            var conditionCodes = SystemSettingDictionary.GetPromotionConditionType();
            //获取商品分类
            var categories = await _coreCmsGoodsCategoryServices.QueryListByClauseAsync(p => p.isShow == true, p => p.sort,
                OrderByType.Asc);
            //获取商品品牌
            var brands = await _coreCmsBrandServices.QueryListByClauseAsync(p => p.isShow == true);
            //获取用户等级
            var grades = await _coreCmsUserGradeServices.QueryAsync();

            jm.data = new
            {
                conditionCodes,
                promotionModel = model,
                categories = GoodsHelper.GetTree(categories, false),
                brands,
                grades
            };

            return jm;
        }
        #endregion

        #region 编辑促销提交============================================================
        // POST: Api/CoreCmsPromotion/DoConditionEdit
        /// <summary>
        /// 编辑促销提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑促销提交")]
        public async Task<AdminUiCallBack> DoConditionEdit([FromBody] CoreCmsPromotionCondition entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsPromotionConditionServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            //oldModel.id = entity.id;
            oldModel.promotionId = entity.promotionId;
            oldModel.code = entity.code;
            oldModel.parameters = entity.parameters;

            //事物处理过程结束
            var bl = await _coreCmsPromotionConditionServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        //促销结果===============================================================================

        #region 获取促销结果列表============================================================
        // POST: Api/CoreCmsPromotion/GetResultList
        /// <summary>
        /// 获取促销结果列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取促销结果列表")]
        public async Task<AdminUiCallBack> GetResultList()
        {
            var jm = new AdminUiCallBack();
            var where = PredicateBuilder.True<CoreCmsPromotionResult>();

            //促销ID int
            var promotionId = Request.Form["promotionId"].FirstOrDefault().ObjectToInt(0);
            if (promotionId > 0)
            {
                where = where.And(p => p.promotionId == promotionId);
            }

            //获取数据
            var list = await _coreCmsPromotionResultServices.QueryListByClauseAsync(where);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 添加促销结果===================================================
        // POST: Api/CoreCmsPromotion/GetResultCreate
        /// <summary>
        /// 添加促销结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("添加促销结果")]
        public async Task<AdminUiCallBack> GetResultCreate([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPromotionServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            var resultCount = await _coreCmsPromotionResultServices.GetCountAsync(p => p.promotionId == entity.id);
            if (resultCount >= 1)
            {
                jm.msg = GlobalErrorCodeVars.Code15016;
                return jm;
            }

            //返回数据
            jm.code = 0;
            var resultCodes = SystemSettingDictionary.GetPromotionResultType();
            jm.data = new
            {
                resultCodes,
                promotionModel = model
            };
            return jm;

        }
        #endregion

        #region 创建促销结果提交============================================================
        // POST: Api/CoreCmsPromotion/DoResultCreate
        /// <summary>
        /// 创建促销结果提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建促销结果提交")]
        public async Task<AdminUiCallBack> DoResultCreate([FromBody] CoreCmsPromotionResult entity)
        {
            var jm = new AdminUiCallBack();

            var resultCount = await _coreCmsPromotionResultServices.GetCountAsync(p => p.promotionId == entity.promotionId);
            if (resultCount >= 1)
            {
                jm.msg = GlobalErrorCodeVars.Code15016;
                return jm;
            }

            var bl = await _coreCmsPromotionResultServices.InsertAsync(entity) > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = (bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure);

            return jm;
        }
        #endregion

        #region 编辑促销结果============================================================
        // POST: Api/CoreCmsPromotion/GetResultEdit
        /// <summary>
        /// 编辑促销结果
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑促销结果")]
        public async Task<AdminUiCallBack> GetResultEdit([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPromotionResultServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;
            var resultCodes = SystemSettingDictionary.GetPromotionResultType();
            jm.data = new
            {
                resultCodes,
                promotionModel = model
            };

            return jm;
        }
        #endregion

        #region 编辑促销结果提交============================================================
        // POST: Api/CoreCmsPromotion/DoResultEdit
        /// <summary>
        /// 编辑促销结果提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑促销结果提交")]
        public async Task<AdminUiCallBack> DoResultEdit([FromBody] CoreCmsPromotionResult entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsPromotionResultServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            //事物处理过程开始
            //oldModel.id = entity.id;
            oldModel.promotionId = entity.promotionId;
            oldModel.code = entity.code;
            oldModel.parameters = entity.parameters;

            //事物处理过程结束
            var bl = await _coreCmsPromotionResultServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 删除促销结果数据============================================================
        // POST: Api/CoreCmsPromotion/DoResultDelete/10
        /// <summary>
        /// 单选促销结果删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选促销结果删除")]
        public async Task<AdminUiCallBack> DoResultDelete([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsPromotionResultServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }
            var bl = await _coreCmsPromotionResultServices.DeleteByIdAsync(entity.id);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }
        #endregion

        //优惠券码===============================================================================

        #region 获取优惠券码列表============================================================
        // POST: Api/CoreCmsPromotion/GetCouponPageList
        /// <summary>
        /// 获取优惠券码列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取优惠券码列表")]
        public async Task<AdminUiCallBack> GetCouponPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsCoupon>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsCoupon, object>> orderEx;
            switch (orderField)
            {
                case "couponCode":
                    orderEx = p => p.couponCode;
                    break;
                case "promotionId":
                    orderEx = p => p.promotionId;
                    break;
                case "isUsed":
                    orderEx = p => p.isUsed;
                    break;
                case "userId":
                    orderEx = p => p.userId;
                    break;
                case "usedId":
                    orderEx = p => p.usedId;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                default:
                    orderEx = p => p.createTime;
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
            //优惠券编码 nvarchar
            var couponCode = Request.Form["couponCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(couponCode))
            {
                where = where.And(p => p.couponCode.Contains(couponCode));
            }
            //优惠券序列 int
            var promotionId = Request.Form["promotionId"].FirstOrDefault().ObjectToInt(0);
            if (promotionId > 0)
            {
                where = where.And(p => p.promotionId == promotionId);
            }
            //是否使用 bit
            var isUsed = Request.Form["isUsed"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isUsed) && isUsed.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isUsed == true);
            }
            else if (!string.IsNullOrEmpty(isUsed) && isUsed.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isUsed == false);
            }
            //领取者 int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            //使用者 int
            var usedId = Request.Form["usedId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(usedId))
            {
                where = where.And(p => p.usedId.Contains(usedId));
            }
            //生成时间 datetime
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
            var list = await _coreCmsCouponServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 优惠券码首页数据============================================================
        // POST: Api/CoreCmsPromotion/GetCouponIndex
        /// <summary>
        /// 优惠券码首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("优惠券码首页数据")]
        public AdminUiCallBack GetCouponIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return jm;
        }
        #endregion

        #region 设置是否使用============================================================
        // POST: Api/CoreCmsPromotion/DoSetCouponisUsed/10
        /// <summary>
        /// 设置是否使用
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否使用")]
        public async Task<AdminUiCallBack> DoSetCouponisUsed([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsCouponServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            oldModel.isUsed = (bool)entity.data;

            var bl = await _coreCmsCouponServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }
        #endregion

        #region 选择导出============================================================
        // POST: Api/CoreCmsPromotion/SelectCouponExportExcel/10
        /// <summary>
        /// 选择导出
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("选择导出")]
        public async Task<AdminUiCallBack> SelectCouponExportExcel([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _coreCmsCouponServices.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("序列");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("优惠券编码");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("优惠券id");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("是否使用");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("谁领取了");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("被谁用了");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("创建时间");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("更新时间");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);

            headerRow.Height = 30 * 20;
            var commonCellStyle = ExcelHelper.GetCommonStyle(book);

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listModel.Count; i++)
            {
                var rowTemp = mySheet.CreateRow(i + 1);

                var rowTemp0 = rowTemp.CreateCell(0);
                rowTemp0.SetCellValue(listModel[i].id.ToString());
                rowTemp0.CellStyle = commonCellStyle;

                var rowTemp1 = rowTemp.CreateCell(1);
                rowTemp1.SetCellValue(listModel[i].couponCode.ToString());
                rowTemp1.CellStyle = commonCellStyle;

                var rowTemp2 = rowTemp.CreateCell(2);
                rowTemp2.SetCellValue(listModel[i].promotionId.ToString());
                rowTemp2.CellStyle = commonCellStyle;

                var rowTemp3 = rowTemp.CreateCell(3);
                rowTemp3.SetCellValue(listModel[i].isUsed.ToString());
                rowTemp3.CellStyle = commonCellStyle;

                var rowTemp4 = rowTemp.CreateCell(4);
                rowTemp4.SetCellValue(listModel[i].userId.ToString());
                rowTemp4.CellStyle = commonCellStyle;

                var rowTemp5 = rowTemp.CreateCell(5);
                rowTemp5.SetCellValue(listModel[i].usedId.ToString());
                rowTemp5.CellStyle = commonCellStyle;

                var rowTemp6 = rowTemp.CreateCell(6);
                rowTemp6.SetCellValue(listModel[i].createTime.ToString());
                rowTemp6.CellStyle = commonCellStyle;

                var rowTemp7 = rowTemp.CreateCell(7);
                rowTemp7.SetCellValue(listModel[i].updateTime.ToString());
                rowTemp7.CellStyle = commonCellStyle;

            }
            // 导出excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsCoupon导出(选择结果).xls";
            string filePath = webRootPath + tpath;
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;


            return jm;
        }
        #endregion

        #region 查询导出============================================================
        // POST: Api/CoreCmsPromotion/QueryCouponExportExcel/10
        /// <summary>
        /// 查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryCouponExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsCoupon>();
            //查询筛选

            //序列 int
            var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
            //优惠券编码 nvarchar
            var couponCode = Request.Form["couponCode"].FirstOrDefault();
            if (!string.IsNullOrEmpty(couponCode))
            {
                where = where.And(p => p.couponCode.Contains(couponCode));
            }
            //优惠券id int
            var promotionId = Request.Form["promotionId"].FirstOrDefault().ObjectToInt(0);
            if (promotionId > 0)
            {
                where = where.And(p => p.promotionId == promotionId);
            }
            //是否使用 bit
            var isUsed = Request.Form["isUsed"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isUsed) && isUsed.ToLowerInvariant() == "true")
            {
                where = where.And(p => p.isUsed == true);
            }
            else if (!string.IsNullOrEmpty(isUsed) && isUsed.ToLowerInvariant() == "false")
            {
                where = where.And(p => p.isUsed == false);
            }
            //谁领取了 int
            var userId = Request.Form["userId"].FirstOrDefault().ObjectToInt(0);
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            //使用者 int
            var usedId = Request.Form["usedId"].FirstOrDefault();
            if (!string.IsNullOrEmpty(usedId))
            {
                where = where.And(p => p.usedId.Contains(usedId));
            }
            //创建时间 datetime
            var createTime = Request.Form["createTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createTime))
            {
                var dt = createTime.ObjectToDate();
                where = where.And(p => p.createTime > dt);
            }
            //更新时间 datetime
            var updateTime = Request.Form["updateTime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updateTime))
            {
                var dt = updateTime.ObjectToDate();
                where = where.And(p => p.updateTime > dt);
            }
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _coreCmsCouponServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("序列");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("优惠券编码");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("优惠券id");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("是否使用");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("谁领取了");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("被谁用了");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("创建时间");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("更新时间");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);


            headerRow.Height = 30 * 20;
            var commonCellStyle = ExcelHelper.GetCommonStyle(book);

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listModel.Count; i++)
            {
                var rowTemp = mySheet.CreateRow(i + 1);


                var rowTemp0 = rowTemp.CreateCell(0);
                rowTemp0.SetCellValue(listModel[i].id.ToString());
                rowTemp0.CellStyle = commonCellStyle;



                var rowTemp1 = rowTemp.CreateCell(1);
                rowTemp1.SetCellValue(listModel[i].couponCode.ToString());
                rowTemp1.CellStyle = commonCellStyle;



                var rowTemp2 = rowTemp.CreateCell(2);
                rowTemp2.SetCellValue(listModel[i].promotionId.ToString());
                rowTemp2.CellStyle = commonCellStyle;



                var rowTemp3 = rowTemp.CreateCell(3);
                rowTemp3.SetCellValue(listModel[i].isUsed.ToString());
                rowTemp3.CellStyle = commonCellStyle;



                var rowTemp4 = rowTemp.CreateCell(4);
                rowTemp4.SetCellValue(listModel[i].userId.ToString());
                rowTemp4.CellStyle = commonCellStyle;



                var rowTemp5 = rowTemp.CreateCell(5);
                rowTemp5.SetCellValue(listModel[i].usedId.ToString());
                rowTemp5.CellStyle = commonCellStyle;



                var rowTemp6 = rowTemp.CreateCell(6);
                rowTemp6.SetCellValue(listModel[i].createTime.ToString());
                rowTemp6.CellStyle = commonCellStyle;



                var rowTemp7 = rowTemp.CreateCell(7);
                rowTemp7.SetCellValue(listModel[i].updateTime.ToString());
                rowTemp7.CellStyle = commonCellStyle;


            }
            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsCoupon导出(查询结果).xls";
            string filePath = webRootPath + tpath;
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return jm;
        }
        #endregion

    }
}
