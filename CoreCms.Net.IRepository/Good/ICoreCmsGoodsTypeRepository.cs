using System.Threading.Tasks;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;

namespace CoreCms.Net.IRepository
{
    /// <summary>
    ///     商品类型 工厂接口
    /// </summary>
    public interface ICoreCmsGoodsTypeRepository : IBaseRepository<CoreCmsGoodsType>
    {
        /// <summary>
        ///     事务重写异步插入方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> InsertAsync(FmGoodsTypeInsert entity);

        /// <summary>
        ///     重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> DeleteByIdAsync(int id);

        /// <summary>
        ///     更新参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateParametersAsync(FMUpdateArrayIntDataByIntId entity);

        /// <summary>
        ///     更新属性
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<AdminUiCallBack> UpdateTypesAsync(FMUpdateArrayIntDataByIntId entity);
    }
}