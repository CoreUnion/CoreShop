/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     商品分类 服务工厂接口
    /// </summary>
    public interface ICoreCmsGoodsCategoryServices : IBaseServices<CoreCmsGoodsCategory>
    {
        #region 获取缓存的所有数据==========================================================

        /// <summary>
        ///     获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<CoreCmsGoodsCategory>> GetCaChe();

        #endregion


        /// <summary>
        ///     判断商品分类下面是否有某一个商品分类
        /// </summary>
        /// <param name="catParentId"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        Task<bool> IsChild(int catParentId, int catId);


        /// <summary>
        ///     判断是否含有子类
        /// </summary>
        /// <param name="list"></param>
        /// <param name="catParentId"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        bool IsHave(List<CoreCmsGoodsCategory> list, int catParentId, int catId);

        #region 重写增删改查操作===========================================================

        /// <summary>
        ///     事务重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> InsertAsync(CoreCmsGoodsCategory entity);


        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(CoreCmsGoodsCategory entity);


        /// <summary>
        ///     重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(List<CoreCmsGoodsCategory> entity);


        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> DeleteByIdAsync(object id);


        /// <summary>
        ///     重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids);

        #endregion
    }
}