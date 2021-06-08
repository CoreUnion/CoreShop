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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 商品类型 接口实现
    /// </summary>
    public class CoreCmsGoodsTypeServices : BaseServices<CoreCmsGoodsType>, ICoreCmsGoodsTypeServices
    {
        private readonly ICoreCmsGoodsTypeRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsGoodsTypeServices(IUnitOfWork unitOfWork, ICoreCmsGoodsTypeRepository dal)
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
        public async Task<AdminUiCallBack> InsertAsync(FmGoodsTypeInsert entity)
        {
            return await _dal.InsertAsync(entity);
        }


        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> DeleteByIdAsync(int id)
        {
            return await _dal.DeleteByIdAsync(id);

        }

        /// <summary>
        /// 更新参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateParametersAsync(FMUpdateArrayIntDataByIntId entity)
        {
            return await _dal.UpdateParametersAsync(entity);
        }

        /// <summary>
        /// 更新属性
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> UpdateTypesAsync(FMUpdateArrayIntDataByIntId entity)
        {
            return await _dal.UpdateTypesAsync(entity);
        }

      
    }
}
