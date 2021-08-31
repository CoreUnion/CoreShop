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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO.Agent;
using CoreCms.Net.Utility.Helper;
using SqlSugar;

namespace CoreCms.Net.Services
{
    /// <summary>
    ///     代理商表 接口实现
    /// </summary>
    public class CoreCmsAgentServices : BaseServices<CoreCmsAgent>, ICoreCmsAgentServices
    {
        private readonly ICoreCmsAgentGoodsServices _agentGoodsServices;
        private readonly ICoreCmsAgentGradeServices _agentGradeServices;
        private readonly ICoreCmsAgentOrderServices _agentOrderServices;
        private readonly ICoreCmsAgentRepository _dal;
        private readonly ICoreCmsGoodsServices _goodsServices;

        private readonly ICoreCmsSettingServices _settingServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoreCmsUserServices _userServices;


        public CoreCmsAgentServices(IUnitOfWork unitOfWork, ICoreCmsAgentRepository dal,
            ICoreCmsSettingServices settingServices, ICoreCmsAgentOrderServices agentOrderServices,
            ICoreCmsUserServices userServices, ICoreCmsAgentGradeServices agentGradeServices,
            ICoreCmsGoodsServices goodsServices, ICoreCmsAgentGoodsServices agentGoodsServices)
        {
            _dal = dal;
            _settingServices = settingServices;
            _agentOrderServices = agentOrderServices;
            _userServices = userServices;
            _agentGradeServices = agentGradeServices;
            _goodsServices = goodsServices;
            _agentGoodsServices = agentGoodsServices;
            BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        #region 获取分销商信息

        /// <summary>
        ///     获取分销商信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetInfo(int userId)
        {
            var jm = new WebApiCallBack();

            //var allConfigs = await _settingServices.GetConfigDictionaries();

            var info = await _dal.QueryByClauseAsync(p => p.userId == userId);

            if (info is {verifyStatus: (int) GlobalEnumVars.AgentVerifyStatus.VerifyYes})
            {
                //总金额
                info.TotalSettlementAmount = await _agentOrderServices.GetSumAsync(
                    p => p.isSettlement != (int) GlobalEnumVars.AgentOrderSettlementStatus.SettlementCancel &&
                         p.userId == userId, p => p.amount);
                //已结算金额
                info.SettlementAmount = await _agentOrderServices.GetSumAsync(
                    p => p.isSettlement == (int) GlobalEnumVars.AgentOrderSettlementStatus.SettlementYes &&
                         p.userId == userId, p => p.amount);
                //冻结金额
                info.FreezeAmount = await _agentOrderServices.GetSumAsync(
                    p => p.isSettlement == (int) GlobalEnumVars.AgentOrderSettlementStatus.SettlementNo &&
                         p.userId == userId, p => p.amount);

                var dt = DateTime.Now;
                //本月第一天时间
                var dtFirst = dt.AddDays(1 - dt.Day);
                //获得某年某月的天数
                var year = dt.Year;
                var month = dt.Month;
                var dayCount = DateTime.DaysInMonth(year, month);
                var dtLast = dtFirst.AddDays(dayCount - 1);

                //本月订单数
                info.CurrentMonthOrder = await _agentOrderServices.GetCountAsync(p =>
                    p.createTime >= dtFirst && p.createTime < dtLast && p.userId == userId);

                info.Store = UserHelper.GetShareCodeByUserId(userId).ToString();

                //本日开始结束时间
                var day = dt.Day;
                var dayStart = new DateTime(year, month, day, 0, 0, 0);
                var datEnd = new DateTime(year, month, day, 23, 59, 59);

                //今日收益
                info.TodayFreezeAmount = await _agentOrderServices.GetSumAsync(
                    p => p.createTime > dayStart && p.createTime <= datEnd && p.userId == userId, p => p.amount);
                //今日订单
                info.TodayOrder = await _agentOrderServices.GetCountAsync(p =>
                    p.createTime > dayStart && p.createTime <= datEnd && p.userId == userId);
                //今日会员
                info.TodayUser = await _userServices.GetCountAsync(p =>
                    p.parentId == userId && p.createTime > dayStart && p.createTime <= datEnd);
            }

            info ??= new CoreCmsAgent();

            info.TotalGoods = await _agentGoodsServices.GetCountAsync(p => p.isEnable);
            if (info.gradeId > 0)
            {
                var userGrade = await _agentGradeServices.QueryByIdAsync(info.gradeId);
                if (userGrade != null) info.GradeName = userGrade.name;
            }

            jm.msg = "获取成功";
            jm.status = true;
            jm.data = info;

            return jm;
        }

        #endregion

        #region 添加用户信息

        /// <summary>
        ///     添加用户信息
        /// </summary>
        /// <param name="iData"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> AddData(CoreCmsAgent iData, int userId)
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
                var disGradeModel = await _agentGradeServices.QueryByClauseAsync(p => p.isDefault);
                if (disGradeModel != null) iData.gradeId = disGradeModel.id;
            }

            if (iData.verifyStatus == 0) iData.verifyStatus = (int) GlobalEnumVars.DistributionVerifyStatus.VerifyWait;
            iData.isDelete = false;
            iData.createTime = DateTime.Now;

            //判断是否存在
            var bl = await _dal.InsertAsync(iData) > 0;
            jm.status = bl;
            jm.msg = bl ? "申请成功" : "申请失败";

            return jm;
        }

        #endregion

        #region 获取我的推广订单

        /// <summary>
        ///     获取我的推广订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="typeId"></param>
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
        ///     获取店铺信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetStore(int userId)
        {
            var jm = new WebApiCallBack();

            jm.status = true;
            var info = await _dal.QueryByClauseAsync(p => p.userId == userId);
            if (info != null)
                //info.TotalGoods = await _agentGoodsServices.GetCountAsync(p => p.isEnable == true);
                jm.data = new
                {
                    info.createTime,
                    info.name,
                    info.storeBanner,
                    info.storeDesc,
                    info.storeLogo,
                    info.storeName
                    //info.TotalGoods
                };

            return jm;
        }

        #endregion


        /// <summary>
        ///     根据条件查询分页数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        public async Task<IPageList<CoreCmsAgentOrder>> QueryOrderPageAsync(int userId, int pageIndex = 1,
            int pageSize = 20)
        {
            return await _dal.QueryOrderPageAsync(userId, pageIndex, pageSize);
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
        public new async Task<IPageList<CoreCmsAgent>> QueryPageAsync(Expression<Func<CoreCmsAgent, bool>> predicate,
            Expression<Func<CoreCmsAgent, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize,
                blUseNoLock);
        }

        #endregion


        /// <summary>
        ///     获取代理商排行
        /// </summary>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        public async Task<IPageList<AgentRankingDTO>> QueryRankingPageAsync(int pageIndex = 1, int pageSize = 20)
        {
            return await _dal.QueryRankingPageAsync(pageIndex, pageSize);
        }
    }
}