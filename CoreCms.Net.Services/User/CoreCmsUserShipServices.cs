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
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 用户地址表 接口实现
    /// </summary>
    public class CoreCmsUserShipServices : BaseServices<CoreCmsUserShip>, ICoreCmsUserShipServices
    {
        private readonly ICoreCmsUserShipRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsUserShipServices(IUnitOfWork unitOfWork, ICoreCmsUserShipRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<WebApiCallBack> InsertAsync(CoreCmsUserShip entity)
        {
            return await _dal.InsertAsync(entity);
        }



        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsUserShip entity)
        {
            return await _dal.UpdateAsync(entity);
        }
    }
}
