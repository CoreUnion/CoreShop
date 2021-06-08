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
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 退货单表 接口实现
    /// </summary>
    public class CoreCmsBillReshipServices : BaseServices<CoreCmsBillReship>, ICoreCmsBillReshipServices
    {
        private readonly ICoreCmsBillReshipRepository _dal;
        private readonly ICoreCmsBillReshipItemRepository _billReshipItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsBillReshipServices(IUnitOfWork unitOfWork
            , ICoreCmsBillReshipRepository dal
            , ICoreCmsBillReshipItemRepository billReshipItemRepository
            )
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
            _billReshipItemRepository = billReshipItemRepository;
        }

        /// <summary>
        /// 创建退货单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="aftersalesId"></param>
        /// <param name="aftersalesItems"></param>
        /// <returns></returns>
        public async Task<WebApiCallBack> ToAdd(int userId, string orderId, string aftersalesId, List<CoreCmsBillAftersalesItem> aftersalesItems)
        {
            var jm = new WebApiCallBack();

            if (aftersalesItems == null || aftersalesItems.Count <= 0)
            {
                jm.msg = GlobalErrorCodeVars.Code13209;
                jm.data = jm.code = 13209;
                return jm;
            }

            var model = new CoreCmsBillReship();
            model.reshipId = CommonHelper.GetSerialNumberType((int)GlobalEnumVars.SerialNumberType.退货单编号);
            model.orderId = orderId;
            model.aftersalesId = aftersalesId;
            model.userId = userId;
            model.status = (int)GlobalEnumVars.BillReshipStatus.待退货;
            model.createTime = DateTime.Now;

            await _dal.InsertAsync(model);

            var list = new List<CoreCmsBillReshipItem>();
            foreach (var item in aftersalesItems)
            {
                var reshipItem = new CoreCmsBillReshipItem();
                reshipItem.reshipId = model.reshipId;
                reshipItem.orderItemsId = item.orderItemsId;
                reshipItem.goodsId = item.goodsId;
                reshipItem.productId = item.productId;
                reshipItem.sn = item.sn;
                reshipItem.bn = item.bn;
                reshipItem.name = item.name;
                reshipItem.imageUrl = item.imageUrl;
                reshipItem.nums = item.nums;
                reshipItem.addon = item.addon;
                reshipItem.createTime = DateTime.Now;
                list.Add(reshipItem);
                //保存退货单明细
            }

            await _billReshipItemRepository.InsertAsync(list);

            jm.status = true;
            jm.data = model;

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
        public async Task<IPageList<CoreCmsBillReship>> QueryPageAsync(Expression<Func<CoreCmsBillReship, bool>> predicate,
            Expression<Func<CoreCmsBillReship, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize);
        }
        #endregion



        #region 获取单个数据带导航
        /// <summary>
        /// 获取单个数据带导航
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public async Task<CoreCmsBillReship> GetDetails(Expression<Func<CoreCmsBillReship, bool>> predicate,
            Expression<Func<CoreCmsBillReship, object>> orderByExpression, OrderByType orderByType)
        {
            return await _dal.GetDetails(predicate, orderByExpression, orderByType);
        }
        #endregion


    }
}
