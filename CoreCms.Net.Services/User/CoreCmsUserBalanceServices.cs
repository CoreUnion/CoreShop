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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 用户余额表 接口实现
    /// </summary>
    public class CoreCmsUserBalanceServices : BaseServices<CoreCmsUserBalance>, ICoreCmsUserBalanceServices
    {
        private readonly ICoreCmsUserBalanceRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;

        public CoreCmsUserBalanceServices(IUnitOfWork unitOfWork, ICoreCmsUserBalanceRepository dal,
            IServiceProvider serviceProvider
            )
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 余额变动记录
        /// </summary>
        /// <param name="userId">当前用户id,当是店铺的时候，取店铺创始人的userId</param>
        /// <param name="type">类型</param>
        /// <param name="money">金额，永远是正的</param>
        /// <param name="sourceId">资源id</param>
        /// <param name="cateMoney">服务费金额 (提现)</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Change(int userId, int type, decimal money, string sourceId = "", decimal cateMoney = 0)
        {
            using var container = _serviceProvider.CreateScope();
            var userServices = container.ServiceProvider.GetService<ICoreCmsUserServices>();

            var jm = new WebApiCallBack();

            if (money != 0)
            {
                //取用户实际余额
                var userInfo = await userServices.QueryByIdAsync(userId);
                if (userInfo == null)
                {
                    jm.data = jm.code = 11004;
                    jm.msg = GlobalErrorCodeVars.Code11004;
                    return jm;
                }
                //取描述，并简单校验
                var res = UserHelper.GetMemo(type, money, cateMoney);
                if (string.IsNullOrEmpty(res))
                {
                    return jm;
                }
                var memo = res;
                if (type != (int)GlobalEnumVars.UserBalanceSourceTypes.Admin)
                {
                    //后台充值或调不改绝对值

                }
                //如果是减余额的操作，还是加余额操作
                if (type == (int)GlobalEnumVars.UserBalanceSourceTypes.Pay || type == (int)GlobalEnumVars.UserBalanceSourceTypes.Tocash)
                {
                    money = -money - cateMoney;
                }
                if (type != (int)GlobalEnumVars.UserBalanceSourceTypes.Service)
                {
                    //后台充值或调不改绝对值

                }


                var balance = userInfo.balance + money;
                if (balance < 0)
                {
                    jm.data = jm.code = 11007;
                    jm.msg = GlobalErrorCodeVars.Code11007;
                    return jm;
                }
                var balanceModel = new CoreCmsUserBalance();
                balanceModel.userId = userId;
                balanceModel.type = type;
                balanceModel.money = money;
                balanceModel.balance = balance;
                balanceModel.sourceId = sourceId;
                balanceModel.memo = memo;
                balanceModel.createTime = DateTime.Now;
                //增加记录
                var balanceModelId = await _dal.InsertAsync(balanceModel);
                balanceModel.id = balanceModelId;
                //更新用户数据
                await userServices.UpdateAsync(p => new CoreCmsUser() { balance = balance }, p => p.id == userId);

                jm.data = balanceModel;

            }
            jm.status = true;

            return jm;
        }


        /// <summary>
        /// 获取用户的邀请佣金
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<decimal> GetInviteCommission(int userId)
        {
            var type = (int)GlobalEnumVars.UserBalanceSourceTypes.Distribution;
            var money = await _dal.GetSumAsync(p => p.userId == userId && p.type == type, p => p.money);
            return money;
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
        public new async Task<IPageList<CoreCmsUserBalance>> QueryPageAsync(Expression<Func<CoreCmsUserBalance, bool>> predicate,
            Expression<Func<CoreCmsUserBalance, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion

    }
}
