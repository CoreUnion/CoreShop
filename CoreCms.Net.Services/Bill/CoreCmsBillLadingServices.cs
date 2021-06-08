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
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Helper;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 提货单表 接口实现
    /// </summary>
    public class CoreCmsBillLadingServices : BaseServices<CoreCmsBillLading>, ICoreCmsBillLadingServices
    {
        private readonly ICoreCmsBillLadingRepository _dal;
        private readonly ICoreCmsClerkRepository _clerkRepository;
        private readonly ICoreCmsStoreRepository _storeRepository;
        private readonly ICoreCmsOrderItemRepository _orderItemRepository;
        private readonly ICoreCmsUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsBillLadingServices(IUnitOfWork unitOfWork, ICoreCmsBillLadingRepository dal, ICoreCmsClerkRepository clerkRepository, ICoreCmsStoreRepository storeRepository, ICoreCmsOrderItemRepository orderItemRepository, ICoreCmsUserRepository userRepository)
        {
            this._dal = dal;
            _clerkRepository = clerkRepository;
            _storeRepository = storeRepository;
            _orderItemRepository = orderItemRepository;
            _userRepository = userRepository;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 添加提货单
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> AddData(string orderId, int storeId, string name, string mobile)
        {
            return await _dal.AddData(orderId, storeId, name, mobile);
        }


        /// <summary>
        /// 核销提货单
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> LadingOperating(string[] ids, int userId = 0)
        {
            var jm = new AdminUiCallBack();

            var list = await _dal.QueryListByClauseAsync(p => ids.Contains(p.id));
            if (list.Any())
            {
                foreach (var item in list)
                {
                    item.clerkId = userId;
                    item.pickUpTime = DateTime.Now;
                    item.status = true;
                }

                var outChanges = await _dal.UpdateAsync(list);
                jm.code = outChanges ? 0 : 1;
                jm.msg = outChanges ? "操作成功" : "操作失败";
            }
            else
            {
                jm.msg = "没有可提货的订单";
            }

            return jm;
        }


        /// <summary>
        /// 获取店铺提货单列表
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetStoreLadingList(int userId, int page, int limit)
        {
            var jm = new WebApiCallBack();

            var clerks = await _clerkRepository.QueryListByClauseAsync(p => p.userId == userId);
            var storeIds = clerks.Select(p => p.storeId).ToList();
            var ladingList = await _dal.QueryPageAsync(p => storeIds.Contains(p.storeId) && p.isDel == false, p => p.status, OrderByType.Asc, page, limit);


            jm.status = true;
            jm.msg = "获取成功";
            jm.data = ladingList;
            jm.otherData = new
            {
                ladingList.TotalCount,
                ladingList.TotalPages
            };

            if (ladingList.Any())
            {
                var storeModel = await _storeRepository.QueryAsync();

                foreach (var item in ladingList)
                {
                    item.orderItems = await _orderItemRepository.QueryListByClauseAsync(p => p.orderId == item.orderId);
                    item.storeName = storeModel.FirstOrDefault(p => p.id == item.storeId)?.storeName;
                    var statusInt = item.status
                        ? (int)GlobalEnumVars.BillLadingStatus.Recharge
                        : (int)GlobalEnumVars.BillLadingStatus.Order;
                    item.statusName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillLadingStatus>(statusInt);
                }
            }

            return jm;
        }


        /// <summary>
        /// 删除提货单(软删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> LadingDelete(string id, int userId = 0)
        {
            var jm = new WebApiCallBack();

            var model = await _dal.QueryByClauseAsync(p => p.id == id);
            if (model != null)
            {
                if (model.status == false)
                {
                    jm.msg = "未提货的提货单不能删除";
                    return jm;
                }
                if (userId > 0)
                {
                    var clerks = await _clerkRepository.ExistsAsync(p => p.userId == userId && p.storeId == model.storeId);
                    if (!clerks)
                    {
                        jm.msg = "你无权删除该提货单";
                        return jm;
                    }
                }
                model.isDel = true;
                model.updateTime = DateTime.Now;
                var bl = await _dal.UpdateAsync(model);

                jm.status = bl;
                jm.msg = bl ? "删除成功" : "删除失败";
            }
            else
            {
                jm.msg = "未找到提货单";
            }

            return jm;
        }

        /// <summary>
        /// 获取提货单详情
        /// </summary>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetInfo(string id, int userId = 0)
        {
            var jm = new WebApiCallBack();

            var list = await _dal.QueryListByClauseAsync(p => p.id == id || p.orderId == id || p.mobile == id);
            var data = new List<CoreCmsBillLading>();
            if (list != null)
            {
                if (userId > 0)
                {
                    var clerks = await _clerkRepository.QueryListByClauseAsync(p => p.userId == userId);
                    if (clerks != null && clerks.Any())
                    {
                        var storeIds = clerks.Select(p => p.storeId).ToList();
                        foreach (var item in list)
                        {
                            if (storeIds.Contains(item.storeId))
                            {
                                data.Add(item);
                            }
                        }
                    }
                }
                foreach (var item in data)
                {
                    var statusInt = item.status
                        ? (int)GlobalEnumVars.BillLadingStatus.Recharge
                        : (int)GlobalEnumVars.BillLadingStatus.Order;
                    item.statusName = EnumHelper.GetEnumDescriptionByValue<GlobalEnumVars.BillLadingStatus>(statusInt);
                    if (item.clerkId > 0)
                    {
                        var userInfo = await _userRepository.QueryByClauseAsync(p => p.id == userId);
                        if (userInfo != null)
                        {
                            item.clerkIdName = !string.IsNullOrEmpty(userInfo.nickName)
                                ? userInfo.nickName + "(" + userInfo.mobile + ")"
                                : UserHelper.FormatMobile(userInfo.mobile) + "(" + userInfo.mobile + ")";
                        }
                    }
                    else
                    {
                        item.clerkIdName = item.status ? "(后台管理员)" : "";
                    }
                    item.orderItems = await _orderItemRepository.QueryListByClauseAsync(p => p.orderId == item.orderId);
                }

                jm.status = true;
                jm.msg = "获取成功";
                jm.data = data;
            }
            else
            {
                jm.msg = "提货单不存在";
            }

            return jm;
        }



    }
}
