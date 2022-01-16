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
using Yitter.IdGenerator;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    ///     商品表
    /// </summary>
    [Description("商品表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize(Permissions.Name)]
    public class CoreCmsGoodsController : ControllerBase
    {
        private readonly ICoreCmsBrandServices _brandServices;
        private readonly ICoreCmsGoodsCategoryExtendServices _categoryExtendServices;
        private readonly ICoreCmsGoodsCategoryServices _coreCmsGoodsCategoryServices;
        private readonly ICoreCmsGoodsServices _coreCmsGoodsServices;
        private readonly ICoreCmsGoodsGradeServices _goodsGradeServices;
        private readonly ICoreCmsGoodsParamsServices _goodsParamsServices;
        private readonly ICoreCmsGoodsTypeSpecServices _goodsTypeSpecServices;
        private readonly ICoreCmsLabelServices _labelServices;
        private readonly ICoreCmsProductsServices _productsServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsGoodsTypeSpecServices _typeSpecServices;
        private readonly ICoreCmsGoodsTypeSpecValueServices _typeSpecValueServices;
        private readonly ICoreCmsUserGradeServices _userGradeServices;
        private readonly ICoreCmsProductsDistributionServices _productsDistributionServices;
        private readonly IWebHostEnvironment _webHostEnvironment;


        /// <summary>
        ///     构造函数
        /// </summary>
        public CoreCmsGoodsController(IWebHostEnvironment webHostEnvironment
            , ICoreCmsGoodsServices coreCmsGoodsServices
            , ICoreCmsSettingServices settingServices
            , ICoreCmsBrandServices brandServices
            , ICoreCmsGoodsCategoryServices coreCmsGoodsCategoryServices
            , ICoreCmsUserGradeServices userGradeServices
            , ICoreCmsGoodsParamsServices goodsParamsServices
            , ICoreCmsGoodsTypeSpecServices typeSpecServices
            , ICoreCmsGoodsTypeSpecValueServices typeSpecValueServices
            , ICoreCmsGoodsGradeServices goodsGradeServices
            , ICoreCmsProductsServices productsServices
            , ICoreCmsGoodsCategoryExtendServices categoryExtendServices
            , ICoreCmsLabelServices labelServices
            , ICoreCmsProductsDistributionServices productsDistributionServices, ICoreCmsGoodsTypeSpecServices goodsTypeSpecServices)
        {
            _webHostEnvironment = webHostEnvironment;
            _coreCmsGoodsServices = coreCmsGoodsServices;
            _settingServices = settingServices;
            _brandServices = brandServices;
            _coreCmsGoodsCategoryServices = coreCmsGoodsCategoryServices;
            _userGradeServices = userGradeServices;
            _goodsParamsServices = goodsParamsServices;
            _typeSpecServices = typeSpecServices;
            _typeSpecValueServices = typeSpecValueServices;
            _goodsGradeServices = goodsGradeServices;
            _productsServices = productsServices;
            _categoryExtendServices = categoryExtendServices;
            _labelServices = labelServices;
            _productsDistributionServices = productsDistributionServices;
            _goodsTypeSpecServices = goodsTypeSpecServices;
        }

        #region 获取列表============================================================

        // POST: Api/CoreCmsGoods/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsGoods>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();
            Expression<Func<CoreCmsGoods, object>> orderEx;
            switch (orderField)
            {
                case "id":
                    orderEx = p => p.id;
                    break;
                case "bn":
                    orderEx = p => p.bn;
                    break;
                case "name":
                    orderEx = p => p.name;
                    break;
                case "brief":
                    orderEx = p => p.brief;
                    break;
                case "price":
                    orderEx = p => p.price;
                    break;
                case "costprice":
                    orderEx = p => p.costprice;
                    break;
                case "mktprice":
                    orderEx = p => p.mktprice;
                    break;
                case "images":
                    orderEx = p => p.images;
                    break;
                case "goodsCategoryId":
                    orderEx = p => p.goodsCategoryId;
                    break;
                case "goodsTypeId":
                    orderEx = p => p.goodsTypeId;
                    break;
                case "brandId":
                    orderEx = p => p.brandId;
                    break;
                case "isNomalVirtual":
                    orderEx = p => p.isNomalVirtual;
                    break;
                case "isMarketable":
                    orderEx = p => p.isMarketable;
                    break;
                case "stock":
                    orderEx = p => p.stock;  //这里的
                    break;
                case "freezeStock":
                    orderEx = p => p.freezeStock; //这里的
                    break;
                case "weight":
                    orderEx = p => p.weight; //这里的
                    break;
                case "unit":
                    orderEx = p => p.unit;
                    break;
                case "intro":
                    orderEx = p => p.intro;
                    break;
                case "spesDesc":
                    orderEx = p => p.spesDesc;
                    break;
                case "parameters":
                    orderEx = p => p.parameters;
                    break;
                case "commentsCount":
                    orderEx = p => p.commentsCount;
                    break;
                case "viewCount":
                    orderEx = p => p.viewCount;
                    break;
                case "buyCount":
                    orderEx = p => p.buyCount;
                    break;
                case "uptime":
                    orderEx = p => p.uptime;
                    break;
                case "downtime":
                    orderEx = p => p.downtime;
                    break;
                case "sort":
                    orderEx = p => p.sort;
                    break;
                case "labelIds":
                    orderEx = p => p.labelIds;
                    break;
                case "newSpec":
                    orderEx = p => p.newSpec;
                    break;
                case "createTime":
                    orderEx = p => p.createTime;
                    break;
                case "updateTime":
                    orderEx = p => p.updateTime;
                    break;
                case "isRecommend":
                    orderEx = p => p.isRecommend;
                    break;
                case "isHot":
                    orderEx = p => p.isHot;
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

            //商品编码 nvarchar
            var bn = Request.Form["bn"].FirstOrDefault();
            if (!string.IsNullOrEmpty(bn)) @where = @where.And(p => p.bn.Contains(bn));
            //商品名称 nvarchar
            var name = Request.Form["name"].FirstOrDefault();
            if (!string.IsNullOrEmpty(name)) @where = @where.And(p => p.name.Contains(name));
            //商品分类ID 关联category.id int
            var selectTreeSelectNodeId = Request.Form["selectTree_select_nodeId"].FirstOrDefault().ObjectToInt(0);
            if (selectTreeSelectNodeId > 0) @where = @where.And(p => p.goodsCategoryId == selectTreeSelectNodeId);
            //商品分类ID 关联category.id int
            var goodsCategoryId = Request.Form["goodsCategoryId"].FirstOrDefault().ObjectToInt(0);
            if (goodsCategoryId > 0) @where = @where.And(p => p.goodsCategoryId == goodsCategoryId);
            //商品类别ID 关联goods_type.id int
            var goodsTypeId = Request.Form["goodsTypeId"].FirstOrDefault().ObjectToInt(0);
            if (goodsTypeId > 0) @where = @where.And(p => p.goodsTypeId == goodsTypeId);
            //品牌ID 关联brand.id int
            var brandId = Request.Form["brandId"].FirstOrDefault().ObjectToInt(0);
            if (brandId > 0) @where = @where.And(p => p.brandId == brandId);
            //是否上架 bit
            var isMarketable = Request.Form["isMarketable"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isMarketable) && isMarketable.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isMarketable);
            else if (!string.IsNullOrEmpty(isMarketable) && isMarketable.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isMarketable == false);
            //是否推荐 bit
            var isRecommend = Request.Form["isRecommend"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isRecommend) && isRecommend.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isRecommend);
            else if (!string.IsNullOrEmpty(isRecommend) && isRecommend.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isRecommend == false);
            //是否热门 bit
            var isHot = Request.Form["isHot"].FirstOrDefault();
            if (!string.IsNullOrEmpty(isHot) && isHot.ToLowerInvariant() == "true")
                @where = @where.And(p => p.isHot);
            else if (!string.IsNullOrEmpty(isHot) && isHot.ToLowerInvariant() == "false")
                @where = @where.And(p => p.isHot == false);
            //是否报警
            var warn = Request.Form["warn"].FirstOrDefault();
            if (!string.IsNullOrEmpty(warn) && warn.ToLowerInvariant() == "true")
            {
                //获取库存
                var allConfigs = await _settingServices.GetConfigDictionaries();
                var kc = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.GoodsStocksWarn);
                if (kc != null)
                {
                    var kcNumer = kc.ObjectToInt();
                    where = where.And(p => p.stock <= kcNumer && p.isDel == false && p.isMarketable);
                }
                else
                {
                    where = where.And(p => p.stock <= 0 && p.isDel == false && p.isMarketable);
                }
            }

            where = where.And(p => p.isDel == false);

            //获取数据
            var list = await _coreCmsGoodsServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize);

            if (list != null && list.Any())
            {
                var labels = await _labelServices.QueryAsync();

                foreach (var item in list)
                    if (!string.IsNullOrEmpty(item.labelIds))
                    {
                        var ids = CommonHelper.StringToIntArray(item.labelIds);
                        item.labels = labels.Where(p => ids.Contains(p.id)).ToList();
                    }
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

        // POST: Api/CoreCmsGoods/GetIndex
        /// <summary>
        ///     首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public async Task<AdminUiCallBack> GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };

            var totalGoods = await _coreCmsGoodsServices.GetCountAsync(p => p.id > 0 && p.isDel == false);
            var totalMarketableUp = await _coreCmsGoodsServices.GetCountAsync(p => p.isMarketable && p.isDel == false);
            var totalMarketableDown =
                await _coreCmsGoodsServices.GetCountAsync(p => p.isMarketable == false && p.isDel == false);

            //获取库存
            var allConfigs = await _settingServices.GetConfigDictionaries();

            var kc = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.GoodsStocksWarn);
            var totalWarn = 0;
            if (kc != null)
            {
                var kcNumer = kc.ObjectToInt();
                totalWarn = await _coreCmsGoodsServices.GetCountAsync(p =>
                    p.stock <= kcNumer && p.isDel == false && p.isMarketable);
            }
            else
            {
                totalWarn = await _coreCmsGoodsServices.GetCountAsync(p =>
                    p.stock <= 0 && p.isDel == false && p.isMarketable);
            }

            //获取商品分类
            var categories = await _coreCmsGoodsCategoryServices.QueryListByClauseAsync(p => p.isShow, p => p.sort,
                OrderByType.Asc);
            //获取品牌
            var brands = await _brandServices.QueryAsync();

            //获取商品分销方式
            var productsDistributionType = EnumHelper.EnumToList<GlobalEnumVars.ProductsDistributionType>();

            jm.data = new
            {
                totalGoods,
                totalMarketableUp,
                totalMarketableDown,
                totalWarn,
                categories = GoodsHelper.GetTree(categories),
                categoriesAll = categories,
                brands,
                productsDistributionType
            };

            return jm;
        }

        #endregion

        #region 创建数据============================================================

        // POST: Api/CoreCmsGoods/GetCreate
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

            //获取商品分类
            var categories = await _coreCmsGoodsCategoryServices.QueryListByClauseAsync(p => p.isShow, p => p.sort, OrderByType.Asc);

            //获取参数列表
            var paramsList = await _goodsParamsServices.QueryListByClauseAsync(p => p.id > 0, p => p.id, OrderByType.Desc, true);
            //获取SKU列表
            var skuList = await _goodsTypeSpecServices.QueryListByClauseAsync(p => p.id > 0, p => p.id, OrderByType.Desc, true);


            //获取品牌
            var brands = await _brandServices.QueryListByClauseAsync(p => p.id > 0 && p.isShow == true, p => p.id, OrderByType.Desc, true);
            //获取用户等级
            var userGrade = await _userGradeServices.QueryAsync();


            //获取商品分销enum
            var productsDistributionType = EnumHelper.EnumToList<GlobalEnumVars.ProductsDistributionType>();

            jm.data = new
            {
                categories = GoodsHelper.GetTree(categories, false),
                brands,
                userGrade,
                productsDistributionType,
                paramsList,
                skuList
            };

            return jm;
        }

        #endregion

        #region 创建提交============================================================

        // POST: Api/CoreCmsGoods/DoCreate
        /// <summary>
        ///     创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody] FMGoodsInsertModel entity)
        {
            var jm = await _coreCmsGoodsServices.InsertAsync(entity);
            return jm;
        }

        #endregion

        #region 编辑数据============================================================

        // POST: Api/CoreCmsGoods/GetEdit
        /// <summary>
        ///     编辑数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit(int id)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsGoodsServices.QueryByIdAsync(id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                jm.data = id;
                return jm;
            }

            jm.code = 0;

            //获取商品分类
            var categories = await _coreCmsGoodsCategoryServices.GetCaChe();
            categories = categories.Where(p => p.isShow == true).ToList();

            //获取用户等级
            var userGrade = await _userGradeServices.QueryAsync();
            //用户价格体系
            var goodsGrades = await _goodsGradeServices.QueryListByClauseAsync(p => p.goodsId == model.id);
            //货品信息
            var products =
                await _productsServices.QueryListByClauseAsync(p => p.goodsId == model.id && p.isDel == false);
            //扩展信息
            var categoryExtend = await _categoryExtendServices.QueryListByClauseAsync(p => p.goodsId == model.id);
            //获取商品分销enum
            var productsDistributionType = EnumHelper.EnumToList<GlobalEnumVars.ProductsDistributionType>();


            //获取参数列表
            var paramsList = await _goodsParamsServices.QueryListByClauseAsync(p => p.id > 0, p => p.id, OrderByType.Desc, true);
            //获取SKU列表
            var skuList = await _goodsTypeSpecServices.QueryListByClauseAsync(p => p.id > 0, p => p.id, OrderByType.Desc, true);

            //获取品牌
            var brands = await _brandServices.QueryListByClauseAsync(p => p.id > 0 && p.isShow == true, p => p.id, OrderByType.Desc, true);


            if (products != null && products.Any())
            {
                var pIds = products.Select(p => p.id).ToList();
                if (pIds.Any())
                {
                    // 获取商品分销明细
                    var pds = await _productsDistributionServices.QueryListByClauseAsync(p => pIds.Contains(p.productsId), p => p.id, OrderByType.Asc);
                    products.ForEach(p =>
                    {
                        foreach (var o in pds.Where(o => o.productsId == p.id))
                        {
                            p.levelOne = o.levelOne;
                            p.levelTwo = o.levelTwo;
                            p.levelThree = o.levelThree;
                        }
                    });
                    jm.otherData = pds;
                }

            }

            //获取参数信息
            var goodsTypeSpec = new List<CoreCmsGoodsTypeSpec>();
            var goodsParams = new List<CoreCmsGoodsParams>();

            //获取参数
            if (!string.IsNullOrEmpty(model.goodsParamsIds))
            {
                var paramsIds = Utility.Helper.CommonHelper.StringToIntArray(model.goodsParamsIds);
                goodsParams = await _goodsParamsServices.QueryListByClauseAsync(p => paramsIds.Contains(p.id));
            }

            //获取属性
            if (!string.IsNullOrEmpty(model.goodsSkuIds))
            {
                var specIds = Utility.Helper.CommonHelper.StringToIntArray(model.goodsSkuIds);
                var typeSpecs = await _typeSpecServices.QueryListByClauseAsync(p => specIds.Contains(p.id));
                var typeSpecValues = await _typeSpecValueServices.QueryListByClauseAsync(p => specIds.Contains(p.specId));
                typeSpecs.ForEach(p =>
                {
                    p.specValues = typeSpecValues.Where(o => o.specId == p.id).ToList();
                });
                goodsTypeSpec = typeSpecs;
            }

            jm.data = new
            {
                model,
                categories = GoodsHelper.GetTree(categories, false),
                brands,
                userGrade,
                goodsGrades,
                products,
                categoryExtend,
                goodsTypeSpec,
                goodsParams,
                productsDistributionType,
                paramsList,
                skuList
            };


            return jm;
        }

        #endregion

        #region 编辑提交============================================================

        // POST: Admins/CoreCmsGoods/Edit
        /// <summary>
        ///     编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody] FMGoodsInsertModel entity)
        {
            var jm = await _coreCmsGoodsServices.UpdateAsync(entity);
            return jm;
        }

        #endregion

        #region 删除数据============================================================

        // POST: Api/CoreCmsGoods/DoDelete/10
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

            var model = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = GlobalConstVars.DataisNo;
                return jm;
            }

            model.isDel = true;
            //假删除
            var bl = await _coreCmsGoodsServices.UpdateAsync(model);
            if (bl)
            {
                await _productsServices.UpdateAsync(p => new CoreCmsProducts() { isDel = true },
                    p => p.goodsId == model.id);
            }
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            return jm;

        }

        #endregion

        #region 预览数据============================================================

        // POST: Api/CoreCmsGoods/GetDetails/10
        /// <summary>
        ///     预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody] FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                jm.data = entity.id;
                return jm;
            }

            jm.code = 0;

            //获取商品分类
            var categories = await _coreCmsGoodsCategoryServices.GetCaChe();
            categories = categories.Where(p => p.isShow == true).ToList();

            //获取用户等级
            var userGrade = await _userGradeServices.QueryAsync();
            //用户价格体系
            var goodsGrades = await _goodsGradeServices.QueryListByClauseAsync(p => p.goodsId == model.id);
            //货品信息
            var products =
                await _productsServices.QueryListByClauseAsync(p => p.goodsId == model.id && p.isDel == false);
            //扩展信息
            var categoryExtend = await _categoryExtendServices.QueryListByClauseAsync(p => p.goodsId == model.id);
            //获取商品分销enum
            var productsDistributionType = EnumHelper.EnumToList<GlobalEnumVars.ProductsDistributionType>();


            //获取参数列表
            var paramsList = await _goodsParamsServices.QueryListByClauseAsync(p => p.id > 0, p => p.id, OrderByType.Desc, true);
            //获取SKU列表
            var skuList = await _goodsTypeSpecServices.QueryListByClauseAsync(p => p.id > 0, p => p.id, OrderByType.Desc, true);

            //获取品牌
            var brands = await _brandServices.QueryListByClauseAsync(p => p.id > 0 && p.isShow == true, p => p.id, OrderByType.Desc, true);


            if (products != null && products.Any())
            {
                var pIds = products.Select(p => p.id).ToList();
                if (pIds.Any())
                {
                    // 获取商品分销明细
                    var pds = await _productsDistributionServices.QueryListByClauseAsync(p => pIds.Contains(p.productsId), p => p.id, OrderByType.Asc);
                    products.ForEach(p =>
                    {
                        foreach (var o in pds.Where(o => o.productsId == p.id))
                        {
                            p.levelOne = o.levelOne;
                            p.levelTwo = o.levelTwo;
                            p.levelThree = o.levelThree;
                        }
                    });
                    jm.otherData = pds;
                }

            }

            //获取参数信息
            var goodsTypeSpec = new List<CoreCmsGoodsTypeSpec>();
            var goodsParams = new List<CoreCmsGoodsParams>();

            //获取参数
            if (!string.IsNullOrEmpty(model.goodsParamsIds))
            {
                var paramsIds = Utility.Helper.CommonHelper.StringToIntArray(model.goodsParamsIds);
                goodsParams = await _goodsParamsServices.QueryListByClauseAsync(p => paramsIds.Contains(p.id));
            }

            //获取属性
            if (!string.IsNullOrEmpty(model.goodsSkuIds))
            {
                var specIds = Utility.Helper.CommonHelper.StringToIntArray(model.goodsSkuIds);
                var typeSpecs = await _typeSpecServices.QueryListByClauseAsync(p => specIds.Contains(p.id));
                var typeSpecValues = await _typeSpecValueServices.QueryListByClauseAsync(p => specIds.Contains(p.specId));
                typeSpecs.ForEach(p =>
                {
                    p.specValues = typeSpecValues.Where(o => o.specId == p.id).ToList();
                });
                goodsTypeSpec = typeSpecs;
            }

            jm.data = new
            {
                model,
                categories = GoodsHelper.GetTree(categories, false),
                brands,
                userGrade,
                goodsGrades,
                products,
                categoryExtend,
                goodsTypeSpec,
                goodsParams,
                productsDistributionType,
                paramsList,
                skuList
            };


            return jm;
        }

        #endregion

        #region 设置是否虚拟商品============================================================

        // POST: Api/CoreCmsGoods/DoSetisNomalVirtual/10
        /// <summary>
        ///     设置是否虚拟商品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否虚拟商品")]
        public async Task<AdminUiCallBack> DoSetisNomalVirtual([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isNomalVirtual = entity.data;

            var bl = await _coreCmsGoodsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 设置是否上架============================================================

        // POST: Api/CoreCmsGoods/DoSetisMarketable/10
        /// <summary>
        ///     设置是否上架
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否上架")]
        public async Task<AdminUiCallBack> DoSetisMarketable([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isMarketable = entity.data;

            var bl = await _coreCmsGoodsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 设置是否推荐============================================================

        // POST: Api/CoreCmsGoods/DoSetisRecommend/10
        /// <summary>
        ///     设置是否推荐
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否推荐")]
        public async Task<AdminUiCallBack> DoSetisRecommend([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isRecommend = entity.data;

            var bl = await _coreCmsGoodsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 设置是否热门============================================================

        // POST: Api/CoreCmsGoods/DoSetisHot/10
        /// <summary>
        ///     设置是否热门
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否热门")]
        public async Task<AdminUiCallBack> DoSetisHot([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isHot = entity.data;

            var bl = await _coreCmsGoodsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 设置是否删除============================================================

        // POST: Api/CoreCmsGoods/DoSetisDel/10
        /// <summary>
        ///     设置是否删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置是否删除")]
        public async Task<AdminUiCallBack> DoSetisDel([FromBody] FMUpdateBoolDataByIntId entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await _coreCmsGoodsServices.QueryByIdAsync(entity.id);
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            oldModel.isDel = entity.data;

            var bl = await _coreCmsGoodsServices.UpdateAsync(oldModel);
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 获取商品SKU明细============================================================

        // POST: Api/CoreCmsGoods/GetSkuDetail/10
        /// <summary>
        ///     获取商品SKU明细
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("获取商品SKU明细")]
        public async Task<AdminUiCallBack> GetSkuDetail([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.id.Length <= 0)
            {
                jm.msg = "请提交SKU模型";
                return jm;
            }

            var typeSpecs = await _typeSpecServices.QueryListByClauseAsync(p => entity.id.Contains(p.id));
            var typeSpecValues = await _typeSpecValueServices.QueryListByClauseAsync(p => entity.id.Contains(p.specId));
            typeSpecs.ForEach(p => { p.specValues = typeSpecValues.Where(o => o.specId == p.id).ToList(); });

            jm.data = new { goodsTypeSpec = typeSpecs };

            var bl = true;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            return jm;
        }

        #endregion

        #region 根据属性值生成货品html===================================================

        // POST: Api/CoreCmsGoods/GetSpecHtml
        /// <summary>
        ///     根据属性值生成货品html
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("根据属性值生成货品html")]
        public AdminUiCallBack GetSpecHtml([FromBody] FmSerializeArray model)
        {
            var jm = new AdminUiCallBack();

            var list = model.entity;
            if (!list.Any())
            {
                jm.msg = "提交数据获取失败";
                return jm;
            }

            var newSpecs = list.Where(p => p.name.Contains("newSpec.")).ToList();
            foreach (var t in newSpecs)
            {
                t.value = t.value.Trim();
                if (GoodsHelper.FilterChar(t.value) == false) continue;
                jm.msg = "自定义规格不符合支持规则格式";
                return jm;
            }

            var oldProducts = list.Where(p => p.name.Contains("product[spesDesc][]")).Select(p => p.value).ToList();

            //if (entity.value.GroupBy(n => n).Any(c => c.Count() > 1))
            //{
            //    jm.msg = "属性值不允许有相同"; return jm;
            //}

            jm.msg = "数据获取成功";

            //修改后的所有属性数据
            var newSpecStr = string.Empty;
            var newSpec = list.Where(p => p.name.Contains("newSpec")).ToList();
            if (newSpec.Any())
            {
                newSpec.ForEach(p =>
                {
                    p.name = p.name.Replace("newSpec.", "");
                    newSpecStr += p.name + ":" + p.value + "|";
                });
                newSpecStr = newSpecStr.Remove(newSpecStr.Length - 1, 1);
            }

            //选择后已经修改的属性数据
            var selectSpecStr = string.Empty;
            var selectSpec = list.Where(p => p.name.Contains("selectSpec")).ToList();
            if (selectSpec.Any())
            {
                selectSpec.ForEach(p =>
                {
                    p.name = p.name.Replace("selectSpec.", "");
                    selectSpecStr += p.name + ":" + p.value + "|";
                });
                selectSpecStr = selectSpecStr.Remove(selectSpecStr.Length - 1, 1);
            }

            if (selectSpec.Any())
            {
                var newSelectSpec = selectSpec;
                newSelectSpec.ForEach(p =>
                {
                    var id = p.name.Split(".")[0];
                    var name = p.name.Split(".")[1];
                    p.name = name;

                    var newValue = newSpec.Find(p => p.name == id);
                    p.value = newValue.value;
                });

                //生成商品规格sku
                var arrayList = newSelectSpec.Select((x, i) => new { Index = i, Value = x })
                    .GroupBy(x => x.Value.name)
                    .Select(x => x.Select(v => v.Value).ToList())
                    .ToList();
                var sku = new string[arrayList.Count][];
                var arrayListIndex = 0;
                arrayList.ForEach(p =>
                {
                    var arr = new string[p.Count];
                    for (var index = 0; index < p.Count; index++) arr[index] = p[index].name + ":" + p[index].value;
                    sku[arrayListIndex] = arr;
                    arrayListIndex++;
                });
                var skuArray = SkuHelper.process(sku);
                //生成货品列表
                var products = new List<CoreCmsProducts>();
                var count = 0;
                foreach (var array in skuArray)
                {
                    if (oldProducts.Any() && oldProducts.Contains(array))
                    {
                        continue;
                    }
                    var obj = new CoreCmsProducts();

                    if (count == 0) obj.isDefalut = true;
                    //var sn = list.Find(p => p.name == "goods[sn]");
                    obj.sn = "SN" + YitIdHelper.NextId();
                    var price = list.Find(p => p.name == "goods[price]");
                    obj.price = price != null && !string.IsNullOrEmpty(price.value)
                        ? Convert.ToDecimal(price.value)
                        : 0;
                    var costprice = list.Find(p => p.name == "goods[costprice]");
                    obj.costprice = costprice != null && !string.IsNullOrEmpty(costprice.value)
                        ? Convert.ToDecimal(costprice.value)
                        : 0;
                    var mktprice = list.Find(p => p.name == "goods[mktprice]");
                    obj.mktprice = mktprice != null && !string.IsNullOrEmpty(mktprice.value)
                        ? Convert.ToDecimal(mktprice.value)
                        : 0;
                    var stock = list.Find(p => p.name == "goods[stock]");
                    obj.stock = stock != null && !string.IsNullOrEmpty(stock.value)
                        ? Convert.ToInt16(stock.value)
                        : 9999;
                    obj.spesDesc = array;
                    products.Add(obj);

                    count++;
                }

                jm.data = new
                {
                    products,
                    arrayList,
                    skuArray,
                    sku,
                    newSpecStr,
                    selectSpecStr
                };
            }

            var bl = true;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;


            return jm;
        }

        #endregion

        #region 批量删除============================================================

        // POST: Api/CoreCmsGoods/DoBatchDelete/10,11,20
        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<AdminUiCallBack> DoBatchDelete([FromBody] FMArrayIntIds entity)
        {
            var jm = await _coreCmsGoodsServices.DeleteByIdsAsync(entity.id);
            return jm;
        }

        #endregion

        #region 批量上架============================================================

        // POST: Api/CoreCmsGoods/DoBatchMarketableUp/10,11,20
        /// <summary>
        ///     批量上架
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量上架")]
        public async Task<AdminUiCallBack> DoBatchMarketableUp([FromBody] FMArrayIntIds entity)
        {
            var jm = await _coreCmsGoodsServices.DoBatchMarketableUp(entity.id);
            return jm;
        }

        #endregion

        #region 批量下架============================================================

        // POST: Api/CoreCmsGoods/DoBatchMarketableDown/10,11,20
        /// <summary>
        ///     批量下架
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量下架")]
        public async Task<AdminUiCallBack> DoBatchMarketableDown([FromBody] FMArrayIntIds entity)
        {
            var jm = await _coreCmsGoodsServices.DoBatchMarketableDown(entity.id);
            return jm;
        }

        #endregion


        #region 批量修改价格===========================================================

        // POST: Api/CoreCmsGoods/GetBatchModifyPrice
        /// <summary>
        ///     批量修改价格
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量修改价格")]
        public AdminUiCallBack GetBatchModifyPrice([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var priceType = EnumHelper.EnumToList<GlobalEnumVars.PriceType>();
            var userGrade = _userGradeServices.Query();
            if (userGrade.Any())
                userGrade.ForEach(p =>
                {
                    priceType.Add(new EnumEntity
                    {
                        description = p.title,
                        title = "grade_price_" + p.id,
                        value = 10000 + p.id
                    });
                });
            jm.code = 0;
            jm.data = new
            {
                entity,
                priceType
            };

            return jm;
        }


        // POST: Api/CoreCmsGoods/DoBatchModifyPrice
        /// <summary>
        ///     批量修改价格提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量修改价格提交")]
        public async Task<AdminUiCallBack> DoBatchModifyPrice([FromBody] FmBatchModifyPrice entity)
        {
            var jm = await _coreCmsGoodsServices.DoBatchModifyPrice(entity);
            return jm;
        }

        #endregion


        #region 批量修改库存===========================================================

        // POST: Api/CoreCmsGoods/GetBatchModifyStock
        /// <summary>
        ///     批量修改库存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量修改库存")]
        public AdminUiCallBack GetBatchModifyStock([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            jm.code = 0;
            jm.data = new
            {
                entity
            };

            return jm;
        }


        // POST: Api/CoreCmsGoods/DoBatchModifyStock
        /// <summary>
        ///     批量修改库存提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量修改库存提交")]
        public async Task<AdminUiCallBack> DoBatchModifyStock([FromBody] FmBatchModifyStock entity)
        {
            var jm = await _coreCmsGoodsServices.DoBatchModifyStock(entity);
            return jm;
        }

        #endregion

        #region 设置标签============================================================

        // POST: Api/CoreCmsGoods/GetLabel
        /// <summary>
        ///     设置标签
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置标签")]
        public async Task<AdminUiCallBack> GetLabel([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();
            var model = await _labelServices.QueryAsync();
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            jm.code = 0;
            jm.data = new
            {
                labels = model,
                ids = entity
            };

            return jm;
        }

        // POST: Admins/CoreCmsGoods/DoSetLabel
        /// <summary>
        ///     设置标签提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("设置标签提交")]
        public async Task<AdminUiCallBack> DoSetLabel([FromBody] FmSetLabel entity)
        {
            var jm = await _coreCmsGoodsServices.DoSetLabel(entity);
            return jm;
        }

        #endregion


        #region 去除标签============================================================

        // POST: Api/CoreCmsGoods/GetDeleteLabel
        /// <summary>
        ///     去除标签
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("去除标签")]
        public async Task<AdminUiCallBack> GetDeleteLabel([FromBody] FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _coreCmsGoodsServices.QueryByIDsAsync(entity.id);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }

            var labelIds = model.Select(p => p.labelIds).ToList();
            var r = new List<int>();
            labelIds.ForEach(p =>
            {
                var arr = CommonHelper.StringToIntArray(p);
                r.AddRange(arr);
            });

            var labels = _labelServices.QueryListByClause(p => r.Contains(p.id));

            jm.code = 0;
            jm.data = new
            {
                labels,
                ids = entity
            };


            return jm;
        }

        // POST: Admins/CoreCmsGoods/DoDeleteLabel
        /// <summary>
        ///     去除标签提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("去除标签提交")]
        public async Task<AdminUiCallBack> DoDeleteLabel([FromBody] FmSetLabel entity)
        {
            var jm = await _coreCmsGoodsServices.DoDeleteLabel(entity);
            return jm;
        }

        #endregion
    }
}