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
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 店铺店员关联表 接口实现
    /// </summary>
    public class CoreCmsClerkServices : BaseServices<CoreCmsClerk>, ICoreCmsClerkServices
    {
        private readonly ICoreCmsClerkRepository _dal;
        private readonly ICoreCmsUserServices _userServices;
        private readonly ICoreCmsSettingServices _settingServices;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsClerkServices(IUnitOfWork unitOfWork, ICoreCmsClerkRepository dal
            , ICoreCmsUserServices userServices
            , ICoreCmsSettingServices settingServices)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _userServices = userServices;
            _settingServices = settingServices;
        }

        /// <summary>
        /// 判断是不是店员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> IsClerk(int userId)
        {
            var jm = new WebApiCallBack();

            var allConfigs = await _settingServices.GetConfigDictionaries();

            var storeSwitch = CommonHelper.GetConfigDictionary(allConfigs, SystemSettingConstVars.StoreSwitch).ObjectToInt(2);
            if (storeSwitch == 1)
            {
                var bl = await base.ExistsAsync(p => p.userId == userId);
                jm.status = true;
                jm.data = bl;
                jm.msg = bl ? "是店员" : "不是店员";
            }
            else
            {
                jm.status = true;
                jm.data = false;
                jm.msg = "未开启到店自提";
            }

            return jm;
        }

        /// <summary>
        ///     获取门店关联用户分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<IPageList<StoreClerkDto>> QueryStoreClerkDtoPageAsync(Expression<Func<StoreClerkDto, bool>> predicate,
            Expression<Func<StoreClerkDto, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryStoreClerkDtoPageAsync(predicate, orderByExpression, orderByType, pageIndex,
                pageSize, blUseNoLock);
        }




        /// <summary>
        ///     获取单个门店用户数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public async Task<StoreClerkDto> QueryStoreClerkDtoByClauseAsync(Expression<Func<StoreClerkDto, bool>> predicate, bool blUseNoLock = false)
        {
            return await _dal.QueryStoreClerkDtoByClauseAsync(predicate, blUseNoLock);
        }

    }
}
