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
using CoreCms.Net.Model.ViewModels.DTO;

namespace CoreCms.Net.IServices
{
    /// <summary>
    ///     地区表 服务工厂接口
    /// </summary>
    public interface ICoreCmsAreaServices : IBaseServices<CoreCmsArea>
    {
        /// <summary>
        ///     获取所有省市区信息
        /// </summary>
        /// <returns></returns>
        Task<List<AreaTreeDto>> GetTreeArea(List<PostAreasTreeNode> checkedAreases, int parentId = 0,
            int currentChecked = 0);


        /// <summary>
        ///     组装地区数据
        /// </summary>
        List<AreaTreeDto> resolve2(List<CoreCmsArea> allDatas, int parentId, List<PostAreasTreeNode> checkedAreases,
            int currentChecked = 0);


        /// <summary>
        ///     获取最终地区ID
        /// </summary>
        /// <param name="provinceName">省</param>
        /// <param name="cityName">市</param>
        /// <param name="countyName">县</param>
        /// <param name="postalCode">邮编</param>
        /// <returns></returns>
        Task<int> GetThreeAreaId(string provinceName, string cityName, string countyName, string postalCode);

        /// <summary>
        ///     根据areaId获取三级区域名称
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="cacheAreas"></param>
        /// <returns></returns>
        Task<WebApiCallBack> GetAreaFullName(int areaId, List<CoreCmsArea> cacheAreas = null);

        /// <summary>
        ///     根据id来返回省市区信息，如果没有查到，就返回省的列表
        /// </summary>
        List<CoreCmsArea> GetArea(List<CoreCmsArea> cacheAreas, int id = 0);


        /// <summary>
        ///     获取最终地区ID
        /// </summary>
        /// <param name="provinceName">省</param>
        /// <param name="cityName">市</param>
        /// <param name="countyName">县</param>
        /// <param name="postalCode">邮编</param>
        /// <returns></returns>
        Task<WebApiCallBack> GetAreaId(string provinceName, string cityName, string countyName, string postalCode);

        #region 重写增删改查操作===========================================================

        /// <summary>
        ///     重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> InsertAsync(CoreCmsArea entity);

        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(CoreCmsArea entity);

        /// <summary>
        ///     重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        new Task<AdminUiCallBack> UpdateAsync(List<CoreCmsArea> entity);

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


        #region 获取缓存的所有数据==========================================================

        /// <summary>
        ///     获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<CoreCmsArea>> GetCaChe();

        /// <summary>
        ///     更新cache
        /// </summary>
        Task<List<CoreCmsArea>> UpdateCaChe();

        #endregion
    }
}