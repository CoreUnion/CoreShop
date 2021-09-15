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
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 发票表 接口实现
    /// </summary>
    public class CoreCmsInvoiceServices : BaseServices<CoreCmsInvoice>, ICoreCmsInvoiceServices
    {
        private readonly ICoreCmsInvoiceRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsInvoiceServices(IUnitOfWork unitOfWork, ICoreCmsInvoiceRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 获取订单的发票信息
        /// </summary>
        /// <param name="orderId">订单字符串编号</param>
        /// <returns></returns>
        public async Task<WebApiCallBack> GetOrderInvoiceInfo(string orderId)
        {
            var jm = new WebApiCallBack();

            var model = await base.QueryByClauseAsync(p => p.sourceId == orderId && p.category == (int)GlobalEnumVars.OrderTaxCategory.Order);
            jm.status = model != null;
            jm.data = model;
            jm.msg = jm.status ? GlobalConstVars.GetDataSuccess : GlobalConstVars.GetDataFailure;

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
        public new async Task<IPageList<CoreCmsInvoice>> QueryPageAsync(Expression<Func<CoreCmsInvoice, bool>> predicate,
            Expression<Func<CoreCmsInvoice, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion


    }
}
