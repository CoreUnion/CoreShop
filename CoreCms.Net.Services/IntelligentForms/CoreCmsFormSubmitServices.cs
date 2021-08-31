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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 用户对表的提交记录 接口实现
    /// </summary>
    public class CoreCmsFormSubmitServices : BaseServices<CoreCmsFormSubmit>, ICoreCmsFormSubmitServices
    {
        private readonly ICoreCmsFormSubmitRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsFormSubmitServices(IUnitOfWork unitOfWork, ICoreCmsFormSubmitRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        #region 实现重写增删改查操作==========================================================


        /// <summary>
        /// 重写异步插入方法并返回自增值
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public async Task<int> InsertReturnIdentityAsync(CoreCmsFormSubmit entity)
        {
            return await _dal.InsertReturnIdentityAsync(entity);
        }

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsFormSubmit entity)
        {
            return await _dal.InsertAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsFormSubmit entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsFormSubmit> entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
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
        public new async Task<IPageList<CoreCmsFormSubmit>> QueryPageAsync(Expression<Func<CoreCmsFormSubmit, bool>> predicate,
            Expression<Func<CoreCmsFormSubmit, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion


        /// <summary>
        /// 表单支付
        /// </summary>
        /// <param name="id">序列</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> Pay(int id)
        {
            return await _dal.Pay(id);
        }

        /// <summary>
        /// 获取表单的统计数据
        /// </summary>
        /// <param name="formId">表单序列</param>
        /// <param name="day">多少天内的数据</param>
        /// <returns></returns>
        public async Task<FormStatisticsViewDto> GetStatisticsByFormid(int formId, int day)
        {
            return await _dal.GetStatisticsByFormid(formId, day);
        }

    }
}
