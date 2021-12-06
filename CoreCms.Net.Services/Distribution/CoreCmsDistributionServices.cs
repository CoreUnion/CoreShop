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
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Model.ViewModels.DTO.Distribution;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Newtonsoft.Json.Linq;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 分销商表 接口实现
    /// </summary>
    public class CoreCmsDistributionServices : BaseServices<CoreCmsDistribution>, ICoreCmsDistributionServices
    {
        private readonly ICoreCmsDistributionRepository _dal;
        private readonly ICoreCmsDistributionOrderRepository _distributionOrderRepository;
        private readonly ICoreCmsDistributionGradeRepository _distributionGradeRepository;
        private readonly ICoreCmsDistributionResultRepository _distributionResultRepository;
        private readonly ICoreCmsDistributionConditionServices _coreCmsDistributionConditionServices;
        private readonly ICoreCmsUserRepository _userRepository;
        private readonly ICoreCmsGoodsRepository _goodsRepository;
        private readonly ICoreCmsUserGradeRepository _userGradeRepository;
        private readonly ICoreCmsOrderRepository _orderRepository;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsDistributionServices(IUnitOfWork unitOfWork, ICoreCmsDistributionRepository dal, ICoreCmsDistributionOrderRepository distributionOrderRepository, ICoreCmsUserRepository userRepository, ICoreCmsGoodsRepository goodsRepository, ICoreCmsUserGradeRepository userGradeRepository, ICoreCmsSettingServices settingServices, ICoreCmsOrderRepository orderRepository, ICoreCmsDistributionGradeRepository distributionGradeRepository, ICoreCmsDistributionResultRepository distributionResultRepository, ICoreCmsDistributionConditionServices coreCmsDistributionConditionServices)
        {
            this._dal = dal;
            _distributionOrderRepository = distributionOrderRepository;
            _userRepository = userRepository;
            _goodsRepository = goodsRepository;
            _userGradeRepository = userGradeRepository;
            _settingServices = settingServices;
            _orderRepository = orderRepository;
            _distributionGradeRepository = distributionGradeRepository;
            _distributionResultRepository = distributionResultRepository;
            _coreCmsDistributionConditionServices = coreCmsDistributionConditionServices;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        #region 获取分销商信息
        /// <summary>
        /// 获取分销商信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="checkStatus">是否检查满足条件</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetInfo(int userId, bool checkStatus = false)
        {
            var jm = new WebApiCallBack();

            var allConfigs = await _settingServices.GetConfigDictionaries();

            var info = await _dal.QueryByClauseAsync(p => p.userId == userId);
            if (info != null && info.verifyStatus == (int)GlobalEnumVars.DistributionVerifyStatus.VerifyYes)
            {
                //总金额
                info.TotalSettlementAmount = await _distributionOrderRepository.GetSumAsync(
                    p => p.isSettlement != (int)GlobalEnumVars.DistributionOrderSettlementStatus.SettlementCancel && p.userId == userId,
                    p => p.amount);
                //已结算金额
                info.SettlementAmount = await _distributionOrderRepository.GetSumAsync(
                    p => p.isSettlement == (int)GlobalEnumVars.DistributionOrderSettlementStatus.SettlementYes && p.userId == userId,
                    p => p.amount);
                //冻结金额
                info.FreezeAmount = await _distributionOrderRepository.GetSumAsync(
                    p => p.isSettlement == (int)GlobalEnumVars.DistributionOrderSettlementStatus.SettlementNo && p.userId == userId,
                    p => p.amount);
                var dt = DateTime.Now;
                //本月第一天时间
                DateTime dtFirst = dt.AddDays(1 - (dt.Day));
                dtFirst = new DateTime(dtFirst.Year, dtFirst.Month, dtFirst.Day, 0, 0, 0);
                //获得某年某月的天数
                int year = dt.Year;
                int month = dt.Month;
                int dayCount = DateTime.DaysInMonth(year, month);
                DateTime dtLast = dtFirst.AddDays(dayCount - 1);
                //本月订单数
                info.CurrentMonthOrder = await _distributionOrderRepository.GetCountAsync(p =>
                    p.createTime >= dtFirst && p.createTime < dtLast && p.userId == userId);

                info.Store = UserHelper.GetShareCodeByUserId(userId).ToString();

                //本日开始结束时间
                var day = dt.Day;
                var dayStart = new DateTime(year, month, day, 0, 0, 0);
                var datEnd = new DateTime(year, month, day, 23, 59, 59);
                //今日收益
                info.TodayFreezeAmount = await _distributionOrderRepository.GetSumAsync(
                    p => p.createTime > dayStart && p.createTime <= datEnd && p.userId == userId,
                    p => p.amount);
                //今日订单
                info.TodayOrder = await _distributionOrderRepository.GetCountAsync(
                    p => p.createTime > dayStart && p.createTime <= datEnd && p.userId == userId);
                //今日会员
                info.TodayUser = await _userRepository.GetCountAsync(p =>
                    p.parentId == userId && p.createTime > dayStart && p.createTime <= datEnd);
            }
            else if (info == null)
            {
                info = new CoreCmsDistribution();
            }
            info.TotalGoods = await _goodsRepository.GetCountAsync(p => p.isMarketable == true);
            if (info.gradeId > 0)
            {
                var userGrade = await _userGradeRepository.QueryByIdAsync(info.gradeId);
                if (userGrade != null)
                {
                    info.GradeName = userGrade.title;
                }
            }
            //检查是否满足条件
            if ((checkStatus == true && info.id == 0) || info.verifyStatus != (int)GlobalEnumVars.DistributionVerifyStatus.VerifyYes)
            {
                info.NeedApply = true; //是否需要申请
                info.ConditionMsg = "您的条件已满足。（点击申请）";

                //无需审核,但是需要满足条件
                var distributionType = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.DistributionType).ObjectToInt(0);
                if (distributionType > 0 && distributionType == 3)
                {
                    info.NeedApply = false;
                    info.ConditionStatus = false;//条件状态
                    info.ConditionProgress = 0;

                    //满足条件，直接成为会员
                    await CheckCondition(allConfigs, info, userId);
                    if (info.ConditionStatus == true && info.ConditionProgress == 100)
                    {
                        //添加用户
                        var userModel = await _userRepository.QueryByIdAsync(userId);
                        if (userModel != null)
                        {
                            var iData = new CoreCmsDistribution();
                            iData.name = !string.IsNullOrEmpty(userModel.nickName)
                                ? userModel.nickName
                                : userModel.mobile;
                            iData.verifyStatus = (int)GlobalEnumVars.DistributionVerifyStatus.VerifyYes;
                            iData.verifyTime = DateTime.Now;

                            await AddData(iData, userId);
                            info.ConditionProgress = 100;
                            info.ConditionStatus = true;//条件状态
                        }
                    }
                }
                else
                {
                    //无条件，但是需要审核
                    if (distributionType > 0 && distributionType == 1)
                    {
                        info.NeedApply = true;
                        info.ConditionProgress = 100;
                        info.ConditionStatus = true;//条件状态
                        info.ConditionMsg = "您的条件已满足。（前往申请）";
                    }
                    else if (distributionType > 0 && distributionType == 2)
                    {
                        await CheckCondition(allConfigs, info, userId);
                    }
                }
            }

            jm.msg = "获取成功";
            jm.status = true;
            jm.data = info;

            return jm;
        }
        #endregion

        #region 添加用户信息
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="iData"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> AddData(CoreCmsDistribution iData, int userId)
        {
            var jm = new WebApiCallBack();
            if (string.IsNullOrEmpty(iData.mobile))
            {
                jm.msg = "请填写手机号";
                return jm;
            }
            if (CommonHelper.IsMobile(iData.mobile) == false)
            {
                jm.msg = "请填写正确的手机号";
                return jm;
            }
            if (string.IsNullOrEmpty(iData.name))
            {
                jm.msg = "请填写您的姓名";
                return jm;
            }
            iData.userId = userId;
            var info = await _dal.QueryByClauseAsync(p => p.userId == userId);
            if (info != null)
            {
                jm.status = false;
                jm.msg = "您已申请，请勿重复提交";
                return jm;
            }
            //默认等级处理
            if (iData.gradeId == 0)
            {
                var disGradeModel = await _distributionGradeRepository.QueryByClauseAsync(p => p.isDefault == true);
                if (disGradeModel != null)
                {
                    iData.gradeId = disGradeModel.id;
                }
            }

            var allConfigs = await _settingServices.GetConfigDictionaries();
            var distributionType = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.DistributionType).ObjectToInt(0);
            if (distributionType == (int)GlobalEnumVars.DistributionConditionType.NoReview)
            {
                iData.verifyStatus = (int)GlobalEnumVars.DistributionVerifyStatus.VerifyYes;
            }
            else
            {
                iData.verifyStatus = (int)GlobalEnumVars.DistributionVerifyStatus.VerifyWait;
            }

            iData.isDelete = false;
            iData.createTime = DateTime.Now;

            //判断是否存在
            var bl = await _dal.InsertAsync(iData) > 0;
            jm.status = bl;
            jm.msg = bl ? "申请成功" : "申请失败";

            return jm;
        }
        #endregion

        #region 检查是否可以成为分销商
        //检查是否可以成为分销商
        public async Task CheckCondition(Dictionary<string, DictionaryKeyValues> allConfigs, CoreCmsDistribution info, int userId = 0)
        {
            //判断消费
            info.ConditionStatus = false;//条件状态
            info.ConditionProgress = 0;
            //获取成为分销商条件
            //var DistributionType = CommonHelper.GetConfigDictionary(allConfigs, GlobalSettingConstVars.DistributionType).ObjectToInt(0);
            var distributionMoney = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.DistributionMoney).ObjectToInt(0);
            //支付金额
            var payed = await _orderRepository.GetSumAsync(
                p => p.payStatus == (int)GlobalEnumVars.OrderPayStatus.Yes && p.userId == userId, p => p.payedAmount);
            if (payed < distributionMoney && distributionMoney > 0)
            {
                info.ConditionMsg = "您的消费额度未满足" + distributionMoney + "元无法申请，快去下单吧~";
            }
            else
            {
                info.ConditionProgress = 50;
                //判断是否需要购买商品
                var distributionGoods = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.DistributionGoods).ObjectToInt(0);
                if (distributionGoods > 0 && distributionGoods == 1)
                {
                    info.ConditionProgress = info.ConditionProgress + 50;
                    info.ConditionStatus = true;//条件状态
                    info.ConditionMsg = "您的条件已满足，马上申请。";
                }
                else
                {
                    //任意商品
                    if (distributionGoods > 0 && distributionGoods == 2)
                    {
                        var orderCount = await _orderRepository.GetCountAsync(p =>
                            p.userId == userId && p.payStatus == (int)GlobalEnumVars.OrderPayStatus.Yes);
                        if (orderCount > 1)
                        {
                            info.ConditionProgress = info.ConditionProgress + 50;
                            info.ConditionStatus = true;//条件状态
                            info.ConditionMsg = "您的条件已满足，马上申请。";
                        }
                        else
                        {
                            info.ConditionMsg = "您的条件未满足，请任意购买一件商品即可成为分销商。";
                        }
                    }
                    else if (distributionGoods > 0 && distributionGoods == 3) //购买指定商品
                    {
                        //判断是否购买指定商品
                        var distributionGoodsId = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.DistributionGoodsId).ObjectToInt(0);
                        var orderNum = _orderRepository.GetOrderNum(userId, distributionGoodsId);
                        if (orderNum >= 1)
                        {
                            info.ConditionProgress = info.ConditionProgress + 50;
                            info.ConditionStatus = true;//条件状态
                            info.ConditionMsg = "您的条件已满足，马上申请。";
                        }
                        else
                        {
                            var goodsInfo = await _goodsRepository.QueryByIdAsync(distributionGoodsId);
                            if (goodsInfo != null)
                            {
                                info.ConditionMsg = "您的条件未满足，请购买指定的【" + goodsInfo.name + "】商品即可成为分销商。";
                            }
                            else
                            {
                                info.ConditionMsg = "您的条件未满足，请购买指定的商品即可成为分销商。";

                            }
                        }

                    }

                }
            }
        }

        #endregion

        #region 获取我的推广订单

        /// <summary>
        /// 获取我的推广订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="typeId">类型</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetMyOrderList(int userId, int page, int limit = 10, int typeId = 0)
        {
            var jm = new WebApiCallBack();

            jm.status = true;
            jm.msg = "获取成功";
            var pageList = await _dal.QueryOrderPageAsync(userId, page, limit, typeId);
            jm.data = pageList;
            jm.code = pageList.TotalCount;

            return jm;
        }
        #endregion

        #region 获取店铺信息
        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetStore(int userId)
        {
            var jm = new WebApiCallBack();

            jm.status = true;
            var info = await _dal.QueryByClauseAsync(p => p.userId == userId);
            if (info != null)
            {
                info.TotalGoods = await _goodsRepository.GetCountAsync(p => p.isMarketable == true && p.isDel == false);
                jm.data = new
                {
                    info.createTime,
                    info.name,
                    info.storeBanner,
                    info.storeDesc,
                    info.storeLogo,
                    info.storeName,
                    info.TotalGoods
                };
            }

            return jm;
        }
        #endregion

        #region 获取当前用户返佣设置
        /// <summary>
        /// 获取当前用户返佣设置
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetGradeAndCommission(int userId)
        {
            var jm = new WebApiCallBack();

            var info = await _dal.QueryByClauseAsync(p => p.userId == userId);
            if (info == null)
            {
                jm.msg = "不是分销商的，不返利";
                return jm; //不是分销商的，不返利。
            }
            var allConfigs = await _settingServices.GetConfigDictionaries();
            var commissionType = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.CommissionType).ObjectToInt(0);
            var dto = new DistributionDto();
            if (info.gradeId > 0)
            {
                var res = await _distributionResultRepository.QueryListByClauseAsync(p => p.gradeId == info.gradeId);
                if (res != null && res.Any())
                {
                    dto.grade_id = info.gradeId;
                    res.ForEach(p =>
                    {
                        var obj = JObject.Parse(p.parameters);
                        if (p.code == "COMMISSION_1")
                        {
                            dto.commission_1 = new commission()
                            {
                                type = obj["commissionType"].ObjectToInt(0),
                                discount = obj["discount"].ObjectToDecimal(0)
                            };
                        }
                        else if (p.code == "COMMISSION_2")
                        {
                            dto.commission_2 = new commission()
                            {
                                type = obj["commissionType"].ObjectToInt(0),
                                discount = obj["discount"].ObjectToDecimal(0)
                            };

                        }
                        else if (p.code == "COMMISSION_3")
                        {
                            dto.commission_3 = new commission()
                            {
                                type = obj["commissionType"].ObjectToInt(0),
                                discount = obj["discount"].ObjectToDecimal(0)
                            };

                        }
                    });
                }
                else
                {
                    dto.grade_id = 0;
                    dto.commission_1 = new commission()
                    {
                        type = commissionType,
                        discount = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.CommissionFirst).ObjectToDecimal(0)
                    };
                    dto.commission_2 = new commission()
                    {
                        type = commissionType,
                        discount = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.CommissionSecond).ObjectToDecimal(0)
                    };
                    dto.commission_3 = new commission()
                    {
                        type = commissionType,
                        discount = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.CommissionThird).ObjectToDecimal(0)
                    };
                }

            }
            else
            {
                dto.grade_id = 0;
                dto.commission_1 = new commission()
                {
                    type = commissionType,
                    discount = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.CommissionFirst).ObjectToDecimal(0)
                };
                dto.commission_2 = new commission()
                {
                    type = commissionType,
                    discount = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.CommissionSecond).ObjectToDecimal(0)
                };
                dto.commission_3 = new commission()
                {
                    type = commissionType,
                    discount = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.CommissionThird).ObjectToDecimal(0)
                };
            }

            dto.DistributionLevel = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.DistributionLevel).ObjectToInt(0);
            jm.status = true;
            jm.data = dto;
            jm.msg = "获取成功";

            return jm;
        }
        #endregion

        #region 检查当前用户是否可以升级(暂存，有问题)

        /// <summary>
        /// 检查当前用户是否可以升级(暂存，有问题)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> CheckUpdate(int userId)
        {
            var jm = new AdminUiCallBack();

            var info = await _dal.QueryByClauseAsync(p => p.userId == userId);
            if (info != null && info.gradeId > 0)
            {
                //找下有没有可以升级的分销商等级
                var grade = await _distributionGradeRepository.QueryByClauseAsync(p => p.id > info.gradeId && p.isAutoUpGrade == true);
                if (grade != null)
                {
                    var conditionList = await _coreCmsDistributionConditionServices.QueryListByClauseAsync(p => p.gradeId == grade.id);
                    //循环所有条件，判断是否可以升级
                    var condition = true;//默认满足升级
                    foreach (var item in conditionList)
                    {
                        var method = "condition_" + item.code;
                        //暂存，有问题
                    }

                    if (condition)
                    {
                        await _dal.UpdateAsync(p => new CoreCmsDistribution() { gradeId = grade.id },
                            p => p.userId == userId);
                        jm.msg = "升级成功";
                    }
                    else
                    {
                        jm.msg = "条件暂不满足，无法升级";
                    }
                }
            }
            return jm;
        }
        #endregion



        /// <summary>
        ///     获取代理商排行
        /// </summary>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        public async Task<IPageList<DistributionRankingDTO>> QueryRankingPageAsync(int pageIndex = 1, int pageSize = 20)
        {
            return await _dal.QueryRankingPageAsync(pageIndex, pageSize);
        }

    }
}
