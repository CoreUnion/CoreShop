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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 用户提现记录表 接口实现
    /// </summary>
    public class CoreCmsUserTocashServices : BaseServices<CoreCmsUserTocash>, ICoreCmsUserTocashServices
    {
        private readonly ICoreCmsUserTocashRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsUserBalanceServices _userBalanceServices;



        public CoreCmsUserTocashServices(IUnitOfWork unitOfWork, ICoreCmsUserTocashRepository dal,
            IServiceProvider serviceProvider, ICoreCmsSettingServices settingServices, ICoreCmsUserServices userServices, ICoreCmsUserBalanceServices userBalanceServices)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
            _settingServices = settingServices;
            _userServices = userServices;
            _userBalanceServices = userBalanceServices;
        }

        /// <summary>
        /// 提现申请
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> Tocash(int userId, decimal money, int bankCardsId)
        {
            var jm = new WebApiCallBack();

            using var container = _serviceProvider.CreateScope();

            var settingServices = container.ServiceProvider.GetService<ICoreCmsSettingServices>();
            var userServices = container.ServiceProvider.GetService<ICoreCmsUserServices>();
            var userBankCardServices = container.ServiceProvider.GetService<ICoreCmsUserBankCardServices>();
            var balanceServices = container.ServiceProvider.GetService<ICoreCmsUserBalanceServices>();

            var allConfigs = await settingServices.GetConfigDictionaries();

            //最小提现金额
            var tocashMoneyLow = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.TocashMoneyLow).ObjectToDecimal((decimal)0.01);
            //每日提现上线
            var tocashMoneyLimit = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.TocashMoneyLimit).ObjectToDecimal(0);
            //提现手续费
            var tocashMoneyRate = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.TocashMoneyRate).ObjectToDecimal(0);
            //最低提现金额
            if (money < tocashMoneyLow)
            {
                jm.msg = "提现最低不能少于" + tocashMoneyLow + "元";
                return jm;
            }
            //每日提现上限，默认0不限制
            if (tocashMoneyLimit > 0)
            {
                var dt = DateTime.Now;
                var starTime = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
                var endTime = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
                //判断历史提现金额  
                var todayMoney = await _dal.GetSumAsync(p => p.createTime >= starTime && p.createTime <= endTime && p.userId == userId, p => p.money);
                todayMoney = todayMoney + money; //历史今天提现加上本次提现;
                if (todayMoney > tocashMoneyLimit)
                {
                    jm.msg = "每日提现不能超过" + tocashMoneyLimit + "元";
                    return jm;
                }
            }
            var userInfo = await userServices.QueryByIdAsync(userId);
            if (userInfo == null)
            {
                jm.msg = GlobalErrorCodeVars.Code11004;
                return jm;
            }
            if (money > userInfo.balance)
            {
                jm.msg = GlobalErrorCodeVars.Code11015;
                return jm;
            }
            // 计算提现服务费(金额)
            var cateMoney = money * (tocashMoneyRate / 100);
            if (cateMoney + money > userInfo.balance)
            {
                jm.msg = GlobalErrorCodeVars.Code11015;
                return jm;
            }
            //获取银行卡信息
            var bankcardsInfo = await userBankCardServices.QueryByClauseAsync(p => p.userId == userId && p.id == bankCardsId);
            if (bankcardsInfo == null)
            {
                jm.msg = GlobalErrorCodeVars.Code11016;
                return jm;
            }
            var cashModel = new CoreCmsUserTocash();
            cashModel.userId = userId;
            cashModel.money = money;
            cashModel.bankName = bankcardsInfo.bankName;
            cashModel.bankCode = bankcardsInfo.bankCode;
            cashModel.bankAreaId = bankcardsInfo.bankAreaId;
            cashModel.accountBank = bankcardsInfo.accountBank;
            cashModel.accountName = bankcardsInfo.accountName;
            cashModel.cardNumber = bankcardsInfo.cardNumber;
            cashModel.status = (int)GlobalEnumVars.UserTocashTypes.待审核;
            cashModel.withdrawals = cateMoney;
            cashModel.createTime = DateTime.Now;

            var res = await _dal.InsertAsync(cashModel);
            if (res > 0)
            {
                var change = await balanceServices.Change(userId, (int)GlobalEnumVars.UserBalanceSourceTypes.Tocash, money, res.ToString(), cateMoney);
                jm.status = change.status;
                jm.msg = jm.status ? "提现申请成功" : "提现申请失败";
                jm.data = change.data;
                jm.otherData = change.otherData;
            }
            else
            {
                jm.msg = "提现申请失败";
            }

            return jm;
        }


        /// <summary>
        /// 获取用户提现列表记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> UserToCashList(int userId = 0, int page = 1, int limit = 10, int status = 0)
        {
            var jm = new WebApiCallBack();

            var where = PredicateBuilder.True<CoreCmsUserTocash>();
            if (status > 0)
            {
                where = where.And(p => p.status == status);
            }
            if (userId > 0)
            {
                where = where.And(p => p.userId == userId);
            }
            var list = await _dal.QueryPageAsync(where, p => p.createTime, OrderByType.Desc, page, limit);
            if (list.Any())
            {
                foreach (var item in list)
                {
                    item.statusName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.UserTocashTypes>(item.status);
                    item.cardNumber = UserHelper.BankCardNoFormat(item.cardNumber);
                }
            }

            jm.status = true;
            jm.data = list;
            jm.otherData = new
            {
                list.TotalPages
            };

            return jm;


        }


        /// <summary>
        /// 提现审核
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Examine(int id = 0, int status = 0)
        {
            var jm = new WebApiCallBack();

            var info = await _dal.QueryByClauseAsync(p =>
                p.id == id && p.status == (int)GlobalEnumVars.UserTocashTypes.待审核);
            if (info == null)
            {
                jm.msg = "没有此记录或不是待审核状态";
                return jm;
            }

            if (status > 0)
            {
                var bl = await _dal.UpdateAsync(p => new CoreCmsUserTocash() { status = status, updateTime = DateTime.Now }, p =>
                         p.id == id && p.status == (int)GlobalEnumVars.UserTocashTypes.待审核);
                jm.status = bl;
                jm.data = status;
                if (bl)
                {
                    //失败给用户退钱到余额
                    if (status == (int)GlobalEnumVars.UserTocashTypes.提现失败)
                    {
                        var toCashInfo = await _dal.QueryByIdAsync(id);

                        // 提现金额 加 服务费返还
                        var newMoney = toCashInfo.money + toCashInfo.withdrawals;
                        var up = await _userServices.UpdateAsync(p => new CoreCmsUser() { balance = p.balance + newMoney }, p => p.id == toCashInfo.userId);
                        if (up)
                        {
                            //添加记录
                            var user = await _userServices.QueryByIdAsync(toCashInfo.userId);

                            var balance = new CoreCmsUserBalance();
                            balance.type = (int)GlobalEnumVars.UserBalanceSourceTypes.Tocash;
                            balance.userId = toCashInfo.userId;
                            balance.balance = user.balance;
                            balance.createTime = DateTime.Now;
                            balance.memo = UserHelper.GetMemo(balance.type, toCashInfo.money);
                            balance.money = newMoney;
                            balance.sourceId = id.ToString();

                            await _userBalanceServices.InsertAsync(balance);
                        }
                    }
                }
            }
            else
            {
                jm.msg = GlobalErrorCodeVars.Code10000;
                jm.status = false;
            }

            return jm;


        }


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
        public new async Task<IPageList<CoreCmsUserTocash>> QueryPageAsync(Expression<Func<CoreCmsUserTocash, bool>> predicate,
            Expression<Func<CoreCmsUserTocash, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion

    }
}
