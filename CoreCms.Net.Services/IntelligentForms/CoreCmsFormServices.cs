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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Auth;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 表单 接口实现
    /// </summary>
    public class CoreCmsFormServices : BaseServices<CoreCmsForm>, ICoreCmsFormServices
    {
        private readonly ICoreCmsFormRepository _dal;
        private readonly ICoreCmsFormItemRepository _itemRepository;
        private readonly ICoreCmsFormSubmitServices _formSubmitServices;
        private readonly ICoreCmsFormSubmitDetailServices _formSubmitDetailServices;
        private readonly ICoreCmsProductsServices _productsServices;
        private readonly ICoreCmsAreaServices _areaServices;

        private IHttpContextAccessor _httpContextAccessor;

        private readonly ICoreCmsGoodsServices _goodsServices;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsFormServices(IUnitOfWork unitOfWork, ICoreCmsFormRepository dal, ICoreCmsFormItemRepository itemRepository, ICoreCmsGoodsServices goodsServices, ICoreCmsFormSubmitServices formSubmitServices, IHttpContextAccessor httpContextAccessor, ICoreCmsAreaServices areaServices, ICoreCmsFormSubmitDetailServices formSubmitDetailServices, ICoreCmsProductsServices productsServices)
        {
            this._dal = dal;
            _itemRepository = itemRepository;
            _goodsServices = goodsServices;
            _formSubmitServices = formSubmitServices;
            _httpContextAccessor = httpContextAccessor;
            _areaServices = areaServices;
            _formSubmitDetailServices = formSubmitDetailServices;
            _productsServices = productsServices;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> InsertAsync(FMForm entity)
        {
            return await _dal.InsertAsync(entity);
        }


        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateAsync(FMForm entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsForm entity)
        {
            return await _dal.InsertAsync(entity);
        }


        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsForm> entity)
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
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            return await _dal.DeleteByIdsAsync(ids);
        }

        #endregion

        #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoreCmsForm>> GetCaChe()
        {
            return await _dal.GetCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<CoreCmsForm>> UpdateCaChe()
        {
            return await _dal.UpdateCaChe();
        }

        #endregion

        #region 重写根据条件查询分页数据
        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<IPageList<CoreCmsForm>> QueryPageAsync(Expression<Func<CoreCmsForm, bool>> predicate,
            Expression<Func<CoreCmsForm, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion


        /// <summary>
        /// 获取form表单详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <param name="noToken"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetFormInfo(int id, string token, bool noToken = false)
        {
            var jm = new WebApiCallBack();

            if (id == 0)
            {
                jm.msg = GlobalErrorCodeVars.Code10051;
                jm.status = false;
                return jm;
            }
            var formModel = await _dal.QueryByIdAsync(id);
            if (formModel == null)
            {
                jm.msg = GlobalErrorCodeVars.Code18008;
                return jm;
            }

            var dt = DateTime.Now;
            //检查过期时间
            if (formModel.endDateTime < dt && noToken == false)
            {
                jm.msg = GlobalErrorCodeVars.Code18002;
                jm.data = new
                {
                    isExpires = true,
                    needLogin = false
                };
                return jm;
            }

            //如果是后台，则不验证token
            if (noToken)
            {
                formModel.isLogin = false;
            }
            if (formModel.isLogin && string.IsNullOrEmpty(token))
            {
                jm.msg = GlobalErrorCodeVars.Code14006;
                jm.data = new
                {
                    needLogin = true,
                    isExpires = false,
                };
                return jm;
            }
            else if (formModel.isLogin && !string.IsNullOrEmpty(token))
            {
                var userId = TokenHelper.GetUserIdBySecurityToken(token);
                jm.otherData = userId;
                if (userId <= 0)
                {
                    jm.msg = GlobalErrorCodeVars.Code14006;
                    jm.data = new
                    {
                        needLogin = true,
                        isExpires = false,
                    };
                    return jm;
                }
            }


            var items = await _itemRepository.QueryListByClauseAsync(p => p.formId == formModel.id, p => p.sort, OrderByType.Asc);
            foreach (var item in items)
            {
                if (item.type == GlobalEnumVars.FormFieldTypes.goods.ToString())
                {
                    if (noToken == false)
                    {
                        var goodId = Convert.ToInt32(item.value);
                        if (goodId > 0)
                        {
                            item.good = await _goodsServices.GetGoodsDetial(goodId);
                        }
                    }
                }
                else if (item.type == GlobalEnumVars.FormFieldTypes.radio.ToString())
                {
                    item.radioValue = item.value.Split(",").ToList();
                }
                else if (item.type == GlobalEnumVars.FormFieldTypes.checbox.ToString())
                {
                    var valueArray = item.value.Split(",");
                    var defaultArray = item.defaultValue.Split(",");

                    var tempCheckbox = new List<TempCheckbox>();
                    foreach (var v in valueArray)
                    {
                        var t = new TempCheckbox { @checked = defaultArray.Contains(v), value = v };
                        tempCheckbox.Add(t);
                    }
                    item.checkboxValue = tempCheckbox;
                }
                else if (item.type == GlobalEnumVars.FormFieldTypes.date.ToString())
                {
                    //没有值的时候设置下默认值
                    if (string.IsNullOrEmpty(item.defaultValue))
                    {
                        item.defaultValue = dt.ToString("yyyy-MM-hh");
                    }
                }
                else if (item.type == GlobalEnumVars.FormFieldTypes.time.ToString())
                {
                    //没有值的时候设置下默认值
                    if (string.IsNullOrEmpty(item.defaultValue))
                    {
                        item.defaultValue = dt.ToString("HH:mm");
                    }
                }
            }
            formModel.Items = items;


            jm.status = true;
            jm.code = 0;
            jm.data = formModel;
            jm.msg = "获取成功";

            return jm;
        }


        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> AddSubmit(FmAddSubmit entity)
        {
            var jm = new WebApiCallBack();
            jm.msg = GlobalErrorCodeVars.Code18001;

            var formModel = await _dal.QueryByIdAsync(entity.id);
            if (formModel == null)
            {
                jm.msg = GlobalErrorCodeVars.Code18008;
                return jm;
            }

            if (formModel.isLogin && string.IsNullOrEmpty(entity.token))
            {
                jm.msg = GlobalErrorCodeVars.Code14006;
                jm.data = new
                {
                    needLogin = true
                };
                return jm;
            }

            var dt = DateTime.Now;
            //检查过期时间
            if (formModel.endDateTime < dt)
            {
                jm.msg = GlobalErrorCodeVars.Code18002;
                return jm;
            }

            decimal money = 0;

            //todo 金额促销
            //付款码
            if (formModel.type == (int)GlobalEnumVars.FormTypes.付款码)
            {
                var items = await _itemRepository.QueryListByClauseAsync(p => p.formId == formModel.id && p.type == GlobalEnumVars.FormFieldTypes.money.ToString());
                if (items.Any())
                {
                    items.ForEach(p =>
                    {
                        var post = entity.data.Find(o => o.key == p.id);
                        money += (decimal)(post?.value.ObjectToFloat(0) ?? 0);
                    });
                }
            }

            var userId = 0;
            if (!string.IsNullOrEmpty(entity.token))
            {
                userId = TokenHelper.GetUserIdBySecurityToken(entity.token);
            }
            //判断提交次数
            if (formModel.times > 0 && !string.IsNullOrEmpty(entity.token))
            {
                if (userId <= 0)
                {
                    jm.msg = GlobalErrorCodeVars.Code18012;
                    return jm;
                }

                var count = await _formSubmitServices.GetCountAsync(p => p.userId == userId && p.formId == formModel.id);
                if (count >= formModel.times)
                {
                    jm.msg = GlobalErrorCodeVars.Code18003;
                    return jm;
                }
            }

            try
            {
                _unitOfWork.BeginTran();
                var formSubmitModel = new CoreCmsFormSubmit();
                formSubmitModel.formId = formModel.id;
                formSubmitModel.formName = formModel.name;
                formSubmitModel.userId = userId;
                formSubmitModel.money = money;
                formSubmitModel.payStatus = false;
                formSubmitModel.status = false;
                formSubmitModel.createTime = DateTime.Now;
                formSubmitModel.ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress != null ? _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString() : "127.0.0.1";

                var formSubmitId = await _formSubmitServices.InsertReturnIdentityAsync(formSubmitModel);

                if (formSubmitId <= 0)
                {
                    _unitOfWork.RollbackTran();
                    jm.msg = GlobalErrorCodeVars.Code18005;
                    return jm;
                }

                if (entity.data.Count > 0)
                {
                    //根据提交的数据取字段列表（已废弃）
                    //var formitemIds = entity.data.Select(p => p.key).ToList();
                    //var formItems = await _itemRepository.QueryListByClauseAsync(p => formitemIds.Contains(p.id));

                    //查询表单展示字段
                    var formItems = await _itemRepository.QueryListByClauseAsync(p => p.formId == formModel.id, p => p.sort, OrderByType.Asc);

                    var sDetails = new List<CoreCmsFormSubmitDetail>();

                    //遍历表单字段项
                    foreach (var item in formItems)
                    {
                        //获取提交的哪些
                        var postItem = entity.data.Find(p => p.key == item.id);
                        if (postItem == null)
                        {
                            _unitOfWork.RollbackTran();
                            jm.msg = item.name + GlobalErrorCodeVars.Code18020;
                            return jm;
                        }
                        string value = postItem.value;

                        //验证数据格式
                        if (FormHelper.ValidateField(item.validationType, postItem.value))
                        {
                            _unitOfWork.RollbackTran();
                        }
                        //如果必填但数据为空。
                        if (item.required && string.IsNullOrEmpty(postItem.value))
                        {
                            _unitOfWork.RollbackTran();
                            jm.msg = GlobalErrorCodeVars.Code18006 + item.name;
                            return jm;
                        }
                        //验证地区
                        if (item.type == GlobalEnumVars.FormFieldTypes.area.ToString() && !string.IsNullOrEmpty(postItem.value))
                        {
                            //var arr = postItem.value.Split(" ");

                            //var countyName = arr[2];
                            //var cityName = arr[1];
                            //var provinceName = arr[0];

                            //var areaId = await _areaServices.GetThreeAreaId(provinceName, cityName, countyName, "");
                            //value = areaId.ToString();

                            value = postItem.value;
                        }

                        if (item.type == GlobalEnumVars.FormFieldTypes.goods.ToString() && !string.IsNullOrEmpty(postItem.value))
                        {
                            var goods = JsonConvert.DeserializeObject<List<FmAddSubmitItemGoods>>(postItem.value);
                            if (goods.Any())
                            {
                                foreach (var good in goods)
                                {
                                    var product = await _productsServices.GetProductInfo(good.productId, false, 0);
                                    if (product == null)
                                    {
                                        _unitOfWork.RollbackTran();
                                        jm.msg = GlobalErrorCodeVars.Code12501;
                                        return jm;
                                    }

                                    var formItemName = !string.IsNullOrEmpty(product.spesDesc)
                                        ? product.spesDesc + "/" + product.sn
                                        : product.sn;

                                    money += product.price * good.nums;
                                    var sDetail = new CoreCmsFormSubmitDetail();
                                    sDetail.submitId = formSubmitId;
                                    sDetail.formId = formModel.id;
                                    sDetail.formItemId = item.id;
                                    sDetail.formItemName = formItemName;
                                    sDetail.formItemValue = good.nums.ToString();
                                    sDetails.Add(sDetail);
                                }
                            }
                        }
                        else
                        {
                            var sDetail = new CoreCmsFormSubmitDetail();
                            sDetail.submitId = formSubmitId;
                            sDetail.formId = formModel.id;
                            sDetail.formItemId = item.id;
                            sDetail.formItemName = item.name;
                            sDetail.formItemValue = value;
                            sDetails.Add(sDetail);
                        }
                    }
                    var sDetailResult = await _formSubmitDetailServices.InsertAsync(sDetails);
                    if (sDetailResult <= 0)
                    {
                        _unitOfWork.RollbackTran();
                        jm.msg = GlobalErrorCodeVars.Code18007;
                        return jm;
                    }
                }

                if (formModel.type == (int)GlobalEnumVars.FormTypes.订单)
                {
                    //订单类型时，更新提交表单金额
                    await _formSubmitServices.UpdateAsync(p => new CoreCmsFormSubmit() { money = money }, p => p.id == formSubmitId);
                }

                jm.data = new
                {
                    formSubmitId,
                    money
                };

                _unitOfWork.CommitTran();

                jm.status = true;
                jm.msg = "提交成功";

            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTran();
                jm.status = false;
                jm.msg = "操作异常";
                jm.data = e;
            }

            return jm;
        }


    }
}
