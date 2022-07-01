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
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 银行卡信息 接口实现
    /// </summary>
    public class CoreCmsUserBankCardServices : BaseServices<CoreCmsUserBankCard>, ICoreCmsUserBankCardServices
    {
        private readonly ICoreCmsUserBankCardRepository _dal;
        private readonly ICoreCmsAreaServices _areaServices;
        private readonly ICoreCmsSettingServices _settingServices;


        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsUserBankCardServices(IUnitOfWork unitOfWork, ICoreCmsUserBankCardRepository dal, ICoreCmsAreaServices areaServices, ICoreCmsSettingServices settingServices)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _areaServices = areaServices;
            _settingServices = settingServices;
        }


        /// <summary>
        /// 我的银行卡列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> AddBankCards(CoreCmsUserBankCard entity)
        {
            var jm = new WebApiCallBack();

            jm.otherData = entity;
            if (string.IsNullOrEmpty(entity.cardNumber))
            {
                jm.msg = "请输入银行卡号"; return jm;
            }
            if (entity.cardNumber.Length < 16 || entity.cardNumber.Length > 19)
            {
                jm.msg = "请输入16-19位银行卡号"; return jm;
            }
            if (string.IsNullOrEmpty(entity.accountName))
            {
                jm.msg = "请输入开户账户名"; return jm;
            }
            if (string.IsNullOrEmpty(entity.bankName))
            {
                jm.msg = "请输入银行名称"; return jm;
            }
            if (string.IsNullOrEmpty(entity.accountBank))
            {
                jm.msg = "请输入开户行名称"; return jm;
            }
            var card = await _dal.QueryByClauseAsync(p => p.userId == entity.userId && p.cardNumber == entity.cardNumber);
            if (card != null)
            {
                jm.msg = "该卡片已经添加"; return jm;
            }

            var model = new CoreCmsUserBankCard();
            model.userId = entity.userId;
            model.bankName = entity.bankName;
            model.bankCode = entity.bankCode;
            model.bankAreaId = entity.bankAreaId;
            model.accountBank = entity.accountBank;
            model.accountName = entity.accountName;
            model.cardNumber = entity.cardNumber;
            model.cardType = entity.cardType;
            model.isdefault = entity.isdefault;
            model.createTime = DateTime.Now;

            if (model.isdefault == true)
            {
                // 如果要添加默认 先判断是否有默认卡
                var def = await _dal.ExistsAsync(p => p.userId == entity.userId && p.isdefault == true);
                if (def)
                {

                    await _dal.UpdateAsync(p => new CoreCmsUserBankCard() { isdefault = false }, p => p.isdefault == true && p.userId == entity.userId);
                    await _dal.InsertAsync(model);
                    jm.status = true;
                    jm.msg = "保存成功";

                }
                else
                {
                    await _dal.InsertAsync(model);
                    jm.status = true;
                    jm.msg = "保存成功";
                }
            }
            else
            {
                await _dal.InsertAsync(model);
                jm.status = true;
                jm.msg = "保存成功";
            }

            return jm;
        }



        /// <summary>
        /// 我的银行卡列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetMyBankcardsList(int userId)
        {
            var jm = new WebApiCallBack();

            jm.status = true;
            var res = await _dal.QueryListByClauseAsync(p => p.userId == userId);
            if (res != null && res.Any())
            {
                foreach (var item in res)
                {
                    var areas =await _areaServices.GetAreaFullName(item.bankAreaId); ;
                    item.bankAreaName = areas.status ? areas.data.ToString() : "";
                    item.cardNumber = UserHelper.BankCardNoFormat(item.cardNumber, 4, 4, '*');
                    item.cardTypeName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BankType>(item.cardType);
                    item.bankLogo = BankConst.BankLogoUrl + item.bankCode;
                }
            }
            jm.data = res;

            return jm;
        }


        /// <summary>
        /// 删除银行卡
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Removebankcard(int id, int userId)
        {
            var jm = new WebApiCallBack();

            // 先判断该银行卡是否存在
            var data = await _dal.QueryByClauseAsync(p => p.id == id && p.userId == userId);
            if (data != null)
            {
                // 如果要删除的是默认的卡
                if (data.isdefault)
                {
                    // 查询是否有其他银行卡
                    var otherData = await _dal.QueryByClauseAsync(p => p.id != id && p.userId == userId);
                    if (otherData != null)
                    {
                        otherData.isdefault = true;
                        await _dal.UpdateAsync(otherData);
                        await _dal.DeleteAsync(data);
                        jm.status = true;
                        jm.msg = "删除成功";
                    }
                    else
                    {
                        jm.status = await _dal.DeleteAsync(data);
                        jm.msg = jm.status ? "删除成功" : "删除失败";
                    }
                }
                else
                {
                    jm.status = await _dal.DeleteByIdAsync(id);
                    jm.msg = jm.status ? "删除成功" : "删除失败";
                }
            }
            else
            {
                jm.msg = "该卡片不存在";
            }

            return jm;
        }


        /// <summary>
        /// 获取用户默认银行卡信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetDefaultBankCard(int userId)
        {
            var jm = new WebApiCallBack();

            var defCard = await _dal.QueryByClauseAsync(p => p.userId == userId && p.isdefault == true);
            if (defCard != null)
            {
                var areas = await _areaServices.GetAreaFullName(defCard.bankAreaId); ;
                defCard.bankAreaName = areas.status ? areas.data.ToString() : "";
                defCard.cardNumber = UserHelper.BankCardNoFormat(defCard.cardNumber, 4, 4, '*');
                defCard.cardTypeName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BankType>(defCard.cardType);
                defCard.bankLogo = BankConst.BankLogoUrl + defCard.bankCode;
                jm.data = defCard;
            }
            else
            {
                var card = await _dal.QueryByClauseAsync(p => p.userId == userId, p => p.createTime, OrderByType.Asc);
                if (card != null)
                {
                    var areas = await _areaServices.GetAreaFullName(card.bankAreaId); ;
                    card.bankAreaName = areas.status ? areas.data.ToString() : "";
                    card.cardNumber = UserHelper.BankCardNoFormat(card.cardNumber, 4, 4, '*');
                    card.cardTypeName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BankType>(card.cardType);
                    card.bankLogo = BankConst.BankLogoUrl + card.bankCode;
                    jm.data = card;
                }
            }

            jm.status = jm.data != null;
            jm.msg = jm.data != null ? "获取成功" : "无数据获取";

            return jm;
        }

        /// <summary>
        /// 获取银行卡组织信息
        /// </summary>
        /// <param name="cardCode"></param>
        /// <returns></returns>
        public WebApiCallBack BankCardsOrganization(string cardCode)
        {
            var jm = new WebApiCallBack();

            var url = "https://ccdcapi.alipay.com/validateAndCacheCardInfo.json?_input_charset=utf-8&cardNo=" + cardCode + "&cardBinCheck=true";
            var res = HttpHelper.PostSend(url, "");
            var resObj = JObject.Parse(res);
            if (!resObj.ContainsKey("validated"))
            {
                jm.msg = GlobalErrorCodeVars.Code11021;
                jm.code = 11021;
                return jm;
            }

            if (!resObj["validated"].ObjectToBool())
            {
                jm.msg = GlobalErrorCodeVars.Code11021;
                jm.code = 11021;
                return jm;
            }
            else
            {
                var name = EnumHelper.GetEnumDescriptionByKey<GlobalEnumVars.BankList>(resObj["bank"].ObjectToString());
                var type = (int)GlobalEnumVars.BankType.BankTypeDc;
                var typeName = string.Empty;
                switch (resObj["cardType"].ObjectToString())
                {
                    case "DC":
                        type = (int)GlobalEnumVars.BankType.BankTypeDc;
                        typeName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BankType>((int)GlobalEnumVars.BankType.BankTypeDc);
                        break;

                    case "CC":
                        type = (int)GlobalEnumVars.BankType.BankTypeCc;
                        typeName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BankType>((int)GlobalEnumVars.BankType.BankTypeCc);
                        break;
                }
                var bankCode = resObj["bank"].ObjectToString();
                jm.status = true;
                jm.data = new
                {
                    name,
                    type,
                    typeName,
                    bankCode
                };
            }

            return jm;
        }


        /// <summary>
        /// 设置默认的银行卡
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> SetDefault(int userId, int id)
        {
            var jm = new WebApiCallBack();

            var card = await _dal.QueryByClauseAsync(p => p.userId == userId && p.id == id);
            if (card != null)
            {
                await _dal.UpdateAsync(it => new CoreCmsUserBankCard() { isdefault = true }, p => p.id == card.id);
                await _dal.UpdateAsync(it => new CoreCmsUserBankCard() { isdefault = false }, p => p.id != card.id && p.userId == userId);
                jm.status = true;
                jm.msg = "保存成功";
            }
            else
            {
                jm.msg = "该银行卡不存在";
            }
            jm.status = true;

            return jm;
        }

        /// <summary>
        /// 获取银行卡信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetBankcardInfo(int userId, int id)
        {
            var jm = new WebApiCallBack();

            var card = await _dal.QueryByClauseAsync(p => p.userId == userId && p.id == id);
            if (card != null)
            {
                card.bankLogo = BankConst.BankLogoUrl + card.bankCode;
                card.cardNumber = UserHelper.BankCardNoFormat(card.cardNumber, 4, 4, '*');
                card.cardTypeName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BankType>(card.cardType);

                jm.status = true;
                jm.msg = "获取成功";
                jm.data = card;
            }
            else
            {
                jm.status = false;
                jm.msg = "该银行卡不存在";
            }

            return jm;
        }

    }
}
