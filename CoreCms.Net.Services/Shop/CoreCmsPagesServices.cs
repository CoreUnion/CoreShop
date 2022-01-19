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
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 单页 接口实现
    /// </summary>
    public class CoreCmsPagesServices : BaseServices<CoreCmsPages>, ICoreCmsPagesServices
    {
        private readonly ICoreCmsPagesRepository _dal;
        private readonly ICoreCmsPagesItemsRepository _pagesItemsRepository;
        private readonly ICoreCmsPromotionServices _promotionServices;
        private readonly ICoreCmsNoticeServices _noticeServices;
        private readonly ICoreCmsGoodsCategoryServices _goodsCategoryServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly ICoreCmsArticleServices _articleServices;
        private readonly ICoreCmsPromotionConditionServices _promotionConditionServices;
        private readonly ICoreCmsPinTuanRuleServices _pinTuanRuleServices;
        private readonly ICoreCmsServicesServices _servicesServices;


        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsPagesServices(IUnitOfWork unitOfWork
            , ICoreCmsPagesRepository dal
            , ICoreCmsPagesItemsRepository pagesItemsRepository, ICoreCmsPromotionServices promotionServices, ICoreCmsNoticeServices noticeServices, ICoreCmsGoodsCategoryServices goodsCategoryServices, ICoreCmsSettingServices settingServices, ICoreCmsGoodsServices goodsServices, ICoreCmsArticleServices articleServices, ICoreCmsPromotionConditionServices promotionConditionServices, ICoreCmsPinTuanRuleServices pinTuanRuleServices, ICoreCmsServicesServices servicesServices)
        {
            this._dal = dal;
            _pagesItemsRepository = pagesItemsRepository;
            _promotionServices = promotionServices;
            _noticeServices = noticeServices;
            _goodsCategoryServices = goodsCategoryServices;
            _settingServices = settingServices;
            _goodsServices = goodsServices;
            _articleServices = articleServices;
            _promotionConditionServices = promotionConditionServices;
            _pinTuanRuleServices = pinTuanRuleServices;
            _servicesServices = servicesServices;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsPages entity)
        {
            return await _dal.InsertAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsPages entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> DeleteByIdAsync(int id)
        {
            return await _dal.DeleteByIdAsync(id);
        }

        /// <summary>
        /// 更新设计
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateDesignAsync(FmPagesUpdate entity)
        {
            return await _dal.UpdateDesignAsync(entity);
        }


        /// <summary>
        /// 复制一个同样的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> CopyByIdAsync(int id)
        {
            return await _dal.CopyByIdAsync(id);
        }


        #region 获取首页数据
        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetPageConfig(string code)
        {

            var jm = new WebApiCallBack();

            var wherePage = PredicateBuilder.True<CoreCmsPages>();

            wherePage = code == "mobile_home" ? wherePage.And(p => p.type == 1) : wherePage.And(p => p.code == code);

            var model = await _dal.QueryByClauseAsync(wherePage);
            if (model == null)
            {
                return jm;
            }
            jm.status = true;
            var items = await _pagesItemsRepository.QueryListByClauseAsync(p => p.pageCode == model.code, p => p.sort, OrderByType.Asc);

            var itemsDto = new List<PagesItemsDto>();
            foreach (var item in items)
            {
                var dto = new PagesItemsDto();
                dto.id = item.id;
                dto.widgetCode = item.widgetCode;
                dto.pageCode = item.pageCode;
                dto.positionId = item.positionId;
                dto.sort = item.sort;

                item.parameters = item.parameters.Replace("/images/empty-banner.png", "/static/images/common/empty-banner.png");


                if (item.widgetCode == "search") //搜索
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else if (item.widgetCode == "tabBar")
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else if (item.widgetCode == "notice")
                {
                    JObject parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);

                    if (parameters != null && parameters.ContainsKey("type") && parameters["type"].ToString() == "auto")
                    {
                        var list = await _noticeServices.QueryListAsync(p => p.isDel == false, p => p.createTime, OrderByType.Desc, 1, 20);
                        if (list != null && list.Any())
                        {
                            JArray result = JArray.FromObject(list);
                            parameters.Remove("list");
                            parameters.Add("list", result);
                        }
                    }
                    else if (parameters != null && parameters.ContainsKey("type") && parameters["type"].ToString() == "choose")
                    {

                        var where = PredicateBuilder.True<CoreCmsNotice>();

                        var orderBy = string.Empty;
                        var noticeIdsStr = string.Empty;
                        if (parameters != null && parameters.ContainsKey("list"))
                        {
                            JArray result = JArray.Parse(parameters["list"].ToString());
                            var noticeIds = new List<int>();
                            foreach (var ss in result)  //查找某个字段与值
                            {
                                var noticeId = ((JObject)ss)["id"].ObjectToInt(0);
                                if (noticeId > 0)
                                {
                                    noticeIds.Add(noticeId);
                                }
                            }
                            where = where.And(p => noticeIds.Contains(p.id));
                            if (noticeIds.Any())
                            {
                                noticeIdsStr = string.Join(",", noticeIdsStr);
                                //按照固定的序列id进行排序
                                if (AppSettingsConstVars.DbDbType == DbType.SqlServer.ToString())
                                {
                                    orderBy = " CHARINDEX(RTRIM(CAST(id as NCHAR)),'" + noticeIdsStr + "') ";
                                }
                                else if (AppSettingsConstVars.DbDbType == DbType.MySql.ToString())
                                {
                                    orderBy = " find_in_set(id,'" + noticeIdsStr + "') ";
                                }
                            }
                        }
                        var notices = await _noticeServices.QueryListByClauseAsync(where, orderBy);
                        if (notices != null && notices.Any())
                        {
                            JArray result = JArray.FromObject(notices);
                            parameters.Remove("list");
                            parameters.Add("list", result);
                        }
                        else
                        {
                            parameters.Remove("list");
                            parameters.Add("list", new JArray());
                        }
                    }

                    dto.parameters = parameters;
                }
                else if (item.widgetCode == "imgSlide")
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else if (item.widgetCode == "coupon")
                {
                    JObject parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);

                    if (parameters != null && parameters.ContainsKey("limit") && parameters["limit"].ObjectToInt(0) != 0)
                    {
                        var list = await _promotionServices.ReceiveCouponList(parameters["limit"].ObjectToInt(0));
                        if (list != null && list.Any())
                        {
                            JArray result = JArray.FromObject(list);
                            parameters.Remove("list");
                            parameters.Add("list", result);
                        }
                    }

                    dto.parameters = parameters;
                }
                else if (item.widgetCode == "blank")
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else if (item.widgetCode == "textarea")
                {
                    JObject parameters = new JObject();
                    parameters["value"] = item.parameters;
                    dto.parameters = parameters;
                }
                else if (item.widgetCode == "video")
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else if (item.widgetCode == "imgWindow")
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else if (item.widgetCode == "imgSingle")
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else if (item.widgetCode == "goodTabBar")
                {
                    JObject parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);

                    if (parameters != null && parameters.ContainsKey("list"))
                    {
                        var list = JArray.Parse(parameters["list"].ToString());
                        var newList = new JArray();
                        foreach (var jToken in list)
                        {
                            var child = (JObject)jToken;
                            var where = PredicateBuilder.True<CoreCmsGoods>();
                            where = where.And(p => p.isDel == false);
                            where = where.And(p => p.isMarketable == true);

                            if (child != null && child.ContainsKey("type") && child["type"].ToString() == "auto")
                            {
                                //商品分类,同时取所有子分类
                                if (child.ContainsKey("classifyId") && child["classifyId"].ObjectToInt(0) > 0)
                                {
                                    var classifyId = child["classifyId"].ObjectToInt(0);
                                    var childCats = await _goodsCategoryServices.QueryListByClauseAsync(p => p.parentId == classifyId);
                                    var catIds = childCats != null && childCats.Any()
                                        ? childCats.Select(p => p.id).ToList()
                                        : new List<int>();
                                    catIds.Add(classifyId);

                                    where = where.And(p => catIds.Contains(p.goodsCategoryId));
                                    //扩展分类 CoreCmsGoodsCategory
                                }
                                //品牌筛选
                                if (child.ContainsKey("brandId") && child["brandId"].ObjectToInt(0) > 0)
                                {
                                    var brandId = child["brandId"].ObjectToInt(0);
                                    where = where.And(p => p.brandId == brandId);
                                }

                                var limit = 0;
                                if (child.ContainsKey("limit") && child["limit"].ObjectToInt(0) > 0)
                                {
                                    limit = child["limit"].ObjectToInt(0);
                                }
                                limit = limit > 0 ? limit : 10;

                                var goods = await _goodsServices.QueryListByClauseAsync(where, limit, p => p.createTime, OrderByType.Desc, true);
                                if (goods != null && goods.Any())
                                {
                                    JArray result = JArray.FromObject(goods);
                                    child.Remove("list");
                                    child.Add("list", result);
                                }
                                else
                                {
                                    child.Remove("list");
                                    child.Add("list", new JArray());
                                }
                            }
                            else
                            {
                                var orderBy = string.Empty;
                                string goodidsStr;
                                if (child != null && child.ContainsKey("list"))
                                {
                                    JArray result = JArray.Parse(child["list"].ToString());
                                    var goodids = new List<int>();
                                    foreach (var ss in result)  //查找某个字段与值
                                    {
                                        var goodid = ((JObject)ss)["id"].ObjectToInt(0);
                                        if (goodid > 0)
                                        {
                                            goodids.Add(goodid);
                                        }
                                    }
                                    where = where.And(p => goodids.Contains(p.id));
                                    if (goodids.Any())
                                    {
                                        goodidsStr = string.Join(",", goodids);
                                        //按照id序列打乱后的顺序排序
                                        if (AppSettingsConstVars.DbDbType == DbType.SqlServer.ToString())
                                        {
                                            orderBy = " CHARINDEX(RTRIM(CAST(id as NCHAR)),'" + goodidsStr + "') ";
                                        }
                                        else if (AppSettingsConstVars.DbDbType == DbType.MySql.ToString())
                                        {
                                            orderBy = " find_in_set(id,'" + goodidsStr + "') ";
                                        }
                                    }
                                }
                                var goods = await _goodsServices.QueryListByClauseAsync(where, orderBy);
                                if (goods != null && goods.Any())
                                {
                                    JArray result = JArray.FromObject(goods);
                                    child.Remove("list");
                                    child.Add("list", result);
                                }
                                else
                                {
                                    child.Remove("list");
                                    child.Add("list", new JArray());
                                }
                            }
                            newList.Add(child);
                        }

                        if (newList != null && newList.Any())
                        {
                            parameters.Remove("list");
                            parameters.Add("list", newList);
                        }
                    }
                    dto.parameters = parameters;
                }

                else if (item.widgetCode == "goods")
                {
                    JObject parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);

                    var where = PredicateBuilder.True<CoreCmsGoods>();
                    where = where.And(p => p.isDel == false);
                    where = where.And(p => p.isMarketable == true);
                    if (parameters != null && parameters.ContainsKey("type") && parameters["type"].ToString() == "auto")
                    {
                        //商品分类,同时取所有子分类
                        if (parameters.ContainsKey("classifyId") && parameters["classifyId"].ObjectToInt(0) > 0)
                        {
                            var classifyId = parameters["classifyId"].ObjectToInt(0);
                            var childCats = await _goodsCategoryServices.QueryListByClauseAsync(p => p.parentId == classifyId);
                            var catIds = childCats != null && childCats.Any()
                                ? childCats.Select(p => p.id).ToList()
                                : new List<int>();
                            catIds.Add(classifyId);

                            where = where.And(p => catIds.Contains(p.goodsCategoryId));
                            //扩展分类 CoreCmsGoodsCategory
                        }
                        //品牌筛选
                        if (parameters.ContainsKey("brandId") && parameters["brandId"].ObjectToInt(0) > 0)
                        {
                            var brandId = parameters["brandId"].ObjectToInt(0);
                            where = where.And(p => p.brandId == brandId);
                        }

                        var limit = 0;
                        if (parameters.ContainsKey("limit") && parameters["limit"].ObjectToInt(0) > 0)
                        {
                            limit = parameters["limit"].ObjectToInt(0);
                        }
                        limit = limit > 0 ? limit : 10;

                        var goods = await _goodsServices.QueryPageAsync(where, " sort desc,id desc ", 1, limit, true);
                        if (goods != null && goods.Any())
                        {
                            JArray result = JArray.FromObject(goods);
                            parameters.Remove("list");
                            parameters.Add("list", result);
                        }
                        else
                        {
                            parameters.Remove("list");
                            parameters.Add("list", new JArray());
                        }
                    }
                    else
                    {
                        var orderBy = string.Empty;
                        var goodidsStr = string.Empty;
                        if (parameters != null && parameters.ContainsKey("list"))
                        {
                            JArray result = JArray.Parse(parameters["list"].ToString());
                            var goodids = new List<int>();
                            foreach (var ss in result)  //查找某个字段与值
                            {
                                var goodid = ((JObject)ss)["id"].ObjectToInt(0);
                                if (goodid > 0)
                                {
                                    goodids.Add(goodid);
                                }
                            }
                            where = where.And(p => goodids.Contains(p.id));
                            if (goodids.Any())
                            {
                                goodidsStr = string.Join(",", goodids);
                                //按照id序列打乱后的顺序排序
                                if (AppSettingsConstVars.DbDbType == DbType.SqlServer.ToString())
                                {
                                    orderBy = " CHARINDEX(RTRIM(CAST(id as NCHAR)),'" + goodidsStr + "') ";
                                }
                                else if (AppSettingsConstVars.DbDbType == DbType.MySql.ToString())
                                {
                                    orderBy = " find_in_set(id,'" + goodidsStr + "') ";
                                }
                            }
                        }
                        var goods = await _goodsServices.QueryListByClauseAsync(where, orderBy);
                        if (goods != null && goods.Any())
                        {
                            JArray result = JArray.FromObject(goods);
                            parameters.Remove("list");
                            parameters.Add("list", result);
                        }
                        else
                        {
                            parameters.Remove("list");
                            parameters.Add("list", new JArray());
                        }
                    }

                    dto.parameters = parameters;

                }
                else if (item.widgetCode == "article")
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else if (item.widgetCode == "articleClassify")
                {
                    JObject parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);

                    if (parameters != null)
                    {
                        if (parameters.ContainsKey("articleClassifyId") && parameters["articleClassifyId"].ObjectToInt(0) > 0)
                        {
                            var articleClassifyId = parameters["articleClassifyId"].ObjectToInt(0);
                            var limit = parameters["limit"].ObjectToInt(0);
                            limit = limit > 0 ? limit : 20;
                            var list = await _articleServices.QueryPageAsync(p => p.typeId == articleClassifyId && p.isPub == true,
                                p => p.createTime, OrderByType.Desc, 1, limit);
                            if (list != null && list.Any())
                            {
                                JArray result = JArray.FromObject(list);
                                parameters.Remove("list");
                                parameters.Add("list", result);
                            }
                        }
                    }

                    dto.parameters = parameters;
                }
                else if (item.widgetCode == "navBar")
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else if (item.widgetCode == "groupPurchase")
                {
                    JObject parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);

                    if (parameters != null && parameters.ContainsKey("list"))
                    {
                        JArray result = JArray.Parse(parameters["list"].ToString());
                        var newReslut = new JArray();
                        foreach (var jToken in result)
                        {
                            var ss = (JObject)jToken;
                            if (ss.ContainsKey("id"))
                            {
                                //判断拼团状态
                                var dt = DateTime.Now;

                                var promotionId = ((JObject)ss)["id"].ObjectToInt(0);
                                if (promotionId > 0)
                                {
                                    var promotionModel = await _promotionServices.QueryByClauseAsync(p => p.id == promotionId && p.isEnable == true && p.startTime <= dt && p.endTime > dt);
                                    if (promotionModel != null)
                                    {
                                        var condition = await _promotionConditionServices.QueryByClauseAsync(p => p.promotionId == promotionId);
                                        if (condition != null)
                                        {
                                            var obj = (JObject)JsonConvert.DeserializeObject(condition.parameters);
                                            if (obj.ContainsKey("goodsId") && obj["goodsId"].ObjectToInt(0) > 0)
                                            {
                                                var goodsId = obj["goodsId"].ObjectToInt(0);
                                                var goods = await _promotionServices.GetGroupDetail(goodsId, 0, "group", promotionId);
                                                if (goods.status)
                                                {
                                                    var goodJson = JsonConvert.SerializeObject(goods.data);
                                                    ((JObject)ss).Add("goods", JToken.Parse(goodJson));
                                                }
                                            }
                                        }

                                        var startStatus = 1;
                                        int lastTime = 0;
                                        bool isOverdue = false;

                                        if (promotionModel.startTime > dt)
                                        {
                                            startStatus = (int)GlobalEnumVars.PinTuanRuleStatus.notBegun;

                                            TimeSpan ts = promotionModel.startTime.Subtract(dt);
                                            lastTime = (int)ts.TotalSeconds;
                                            isOverdue = lastTime > 0;
                                        }
                                        else if (promotionModel.startTime <= dt && promotionModel.endTime > dt)
                                        {
                                            startStatus = (int)GlobalEnumVars.PinTuanRuleStatus.begin;

                                            TimeSpan ts = promotionModel.endTime.Subtract(dt);
                                            lastTime = (int)ts.TotalSeconds;
                                            isOverdue = lastTime > 0;
                                        }
                                        else
                                        {
                                            startStatus = (int)GlobalEnumVars.PinTuanRuleStatus.haveExpired;
                                        }

                                        ((JObject)ss).Add("startStatus", startStatus);
                                        ((JObject)ss).Add("lastTime", lastTime);
                                        ((JObject)ss).Add("isOverdue", isOverdue);

                                        newReslut.Add(ss);
                                    }
                                }
                            }
                        }
                        parameters.Remove("list");
                        parameters.Add("list", newReslut);
                    }

                    dto.parameters = parameters;
                }
                else if (item.widgetCode == "record")
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else if (item.widgetCode == "pinTuan")
                {
                    JObject parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);

                    if (parameters != null && parameters.ContainsKey("list"))
                    {
                        JArray result = JArray.Parse(parameters["list"].ToString());
                        var newReslut = new JArray();
                        foreach (JObject ss in result)
                        {
                            if (ss.ContainsKey("goodsId"))
                            {
                                var goodsId = ((JObject)ss)["goodsId"].ObjectToInt(0);
                                if (goodsId > 0)
                                {
                                    var goodsInfo = await _pinTuanRuleServices.GetPinTuanInfo(goodsId);
                                    if (goodsInfo != null)
                                    {
                                        var pinTuanStartStatus = 1;
                                        int lastTime = 0;
                                        bool isOverdue = false;
                                        //判断拼团状态
                                        var dt = DateTime.Now;

                                        if (goodsInfo.startTime > dt)
                                        {
                                            pinTuanStartStatus = (int)GlobalEnumVars.PinTuanRuleStatus.notBegun;

                                            TimeSpan ts = goodsInfo.startTime.Subtract(dt);
                                            lastTime = (int)ts.TotalSeconds;
                                            isOverdue = lastTime > 0;
                                        }
                                        else if (goodsInfo.startTime <= dt && goodsInfo.endTime > dt)
                                        {
                                            pinTuanStartStatus = (int)GlobalEnumVars.PinTuanRuleStatus.begin;

                                            TimeSpan ts = goodsInfo.endTime.Subtract(dt);
                                            lastTime = (int)ts.TotalSeconds;
                                            isOverdue = lastTime > 0;
                                        }
                                        else
                                        {
                                            pinTuanStartStatus = (int)GlobalEnumVars.PinTuanRuleStatus.haveExpired;
                                        }

                                        decimal pinTuanPrice = goodsInfo.goodsPrice - goodsInfo.discountAmount;
                                        if (pinTuanPrice < 0) pinTuanPrice = 0;



                                        var obj = new JObject();
                                        ((JObject)obj).Add("pinTuanStartStatus", pinTuanStartStatus);
                                        ((JObject)obj).Add("lastTime", lastTime);
                                        ((JObject)obj).Add("isOverdue", isOverdue);
                                        ((JObject)obj).Add("pinTuanPrice", pinTuanPrice);

                                        ((JObject)obj).Add("createTime", goodsInfo.createTime);
                                        ((JObject)obj).Add("discountAmount", goodsInfo.discountAmount);
                                        ((JObject)obj).Add("endTime", goodsInfo.endTime);
                                        ((JObject)obj).Add("goodsId", goodsInfo.goodsId);
                                        ((JObject)obj).Add("goodsImage", goodsInfo.goodsImage);
                                        ((JObject)obj).Add("goodsName", goodsInfo.goodsName);
                                        ((JObject)obj).Add("goodsPrice", goodsInfo.goodsPrice);
                                        ((JObject)obj).Add("id", goodsInfo.id);
                                        ((JObject)obj).Add("isStatusOpen", goodsInfo.isStatusOpen);
                                        ((JObject)obj).Add("maxGoodsNums", goodsInfo.maxGoodsNums);
                                        ((JObject)obj).Add("maxNums", goodsInfo.maxNums);
                                        ((JObject)obj).Add("name", goodsInfo.name);
                                        ((JObject)obj).Add("peopleNumber", goodsInfo.peopleNumber);
                                        ((JObject)obj).Add("significantInterval", goodsInfo.significantInterval);
                                        ((JObject)obj).Add("sort", goodsInfo.sort);
                                        ((JObject)obj).Add("startTime", goodsInfo.startTime);
                                        ((JObject)obj).Add("updateTime", goodsInfo.updateTime);

                                        //((JObject)ss).Add("goodsInfo", JToken.FromObject(goodsInfo));
                                        newReslut.Add(obj);
                                    }
                                }
                            }
                        }
                        parameters.Remove("list");
                        parameters.Add("list", newReslut);
                    }

                    dto.parameters = parameters;

                }
                else if (item.widgetCode == "service")
                {
                    JObject parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);

                    if (parameters != null && parameters.ContainsKey("list"))
                    {
                        JArray result = JArray.Parse(parameters["list"].ToString());
                        foreach (JObject ss in result)
                        {
                            if (ss.ContainsKey("id"))
                            {
                                var id = ((JObject)ss)["id"].ObjectToInt(0);
                                if (id > 0)
                                {
                                    var serviceInfo = await _servicesServices.QueryByIdAsync(id);
                                    if (serviceInfo != null)
                                    {
                                        var pinTuanStartStatus = 1;
                                        int lastTime = 0;
                                        bool isOverdue = false;
                                        //判断拼团状态
                                        var dt = DateTime.Now;

                                        if (serviceInfo.startTime > dt)
                                        {
                                            pinTuanStartStatus = (int)GlobalEnumVars.ServicesOpenStatus.notBegun;
                                            TimeSpan ts = serviceInfo.startTime.Subtract(dt);
                                            lastTime = (int)ts.TotalSeconds;
                                            isOverdue = lastTime > 0;
                                        }
                                        else if (serviceInfo.startTime <= dt && serviceInfo.endTime > dt)
                                        {
                                            pinTuanStartStatus = (int)GlobalEnumVars.ServicesOpenStatus.begin;

                                            TimeSpan ts = serviceInfo.endTime.Subtract(dt);
                                            lastTime = (int)ts.TotalSeconds;
                                            isOverdue = lastTime > 0;
                                        }
                                        else
                                        {
                                            pinTuanStartStatus = (int)GlobalEnumVars.ServicesOpenStatus.haveExpired;
                                        }
                                        ((JObject)ss).Add("pinTuanStartStatus", pinTuanStartStatus);
                                        ((JObject)ss).Add("lastTime", lastTime);
                                        ((JObject)ss).Add("isOverdue", isOverdue);
                                    }
                                }
                            }


                        }
                        parameters.Remove("list");
                        parameters.Add("list", result);
                    }

                    dto.parameters = parameters;
                }
                else if (item.widgetCode == "adpop")
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else if (item.widgetCode == "topImgSlide")
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                else
                {
                    dto.parameters = (JObject)JsonConvert.DeserializeObject(item.parameters);
                }
                itemsDto.Add(dto);
            }

            jm.data = new
            {
                model.code,
                desc = model.description,
                model.id,
                model.layout,
                model.name,
                model.type,
                items = itemsDto
            };

            return jm;
        }

        #endregion




    }
}
