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
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 商品分类 接口实现
    /// </summary>
    public class CoreCmsGoodsCategoryServices : BaseServices<CoreCmsGoodsCategory>, ICoreCmsGoodsCategoryServices
    {
        private readonly ICoreCmsGoodsCategoryRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsGoodsCategoryServices(IUnitOfWork unitOfWork, ICoreCmsGoodsCategoryRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
            _unitOfWork = unitOfWork;
        }


        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsGoodsCategory entity)
        {
            return await _dal.InsertAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsGoodsCategory entity)
        {
            return await _dal.UpdateAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsGoodsCategory> entity)
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

        #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoreCmsGoodsCategory>> GetCaChe()
        {
            return await _dal.GetCaChe();
        }

        #endregion

        #region 判断商品分类下面是否有某一个商品分类
        /// <summary>
        /// 判断商品分类下面是否有某一个商品分类
        /// </summary>
        /// <param name="catParentId"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        public async Task<bool> IsChild(int catParentId, int catId)
        {
            var list = await _dal.GetCaChe();
            var bl = IsHave(list, catParentId, catId);
            return bl;
        }
        #endregion

        #region 判断是否含有子类
        /// <summary>
        /// 判断是否含有子类
        /// </summary>
        /// <param name="list"></param>
        /// <param name="catParentId"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        public bool IsHave(List<CoreCmsGoodsCategory> list, int catParentId, int catId)
        {
            if (catParentId == catId)
            {
                return true;
            }
            if (!list.Exists(p => p.id == catParentId))
            {
                return false;
            }
            var children = list.Where(p => p.parentId == catParentId).ToList();
            foreach (var item in children)
            {
                if (IsHave(list, item.id, catId))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

    }
}
