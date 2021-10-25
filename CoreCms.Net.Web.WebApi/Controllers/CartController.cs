/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using CoreCms.Net.Auth.HttpContextUser;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Model.ViewModels.DTO;
using CoreCms.Net.Utility.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreCms.Net.Web.WebApi.Controllers
{
    /// <summary>
    /// 购物车操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IHttpContextUser _user;
        private readonly ICoreCmsCartServices _cartServices;


        /// <summary>
        /// 构造函数
        /// </summary>
        public CartController(IHttpContextUser user, ICoreCmsCartServices cartServices)
        {
            _user = user;
            _cartServices = cartServices;
        }

        //公共接口====================================================================================================

        //验证接口====================================================================================================

        #region 添加单个货品到购物车

        /// <summary>
        /// 添加单个货品到购物车
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> AddCart([FromBody] FMCartAdd entity)
        {
            var jm = await _cartServices.Add(_user.ID, entity.ProductId, entity.Nums, entity.type, entity.cartType, entity.objectId);
            return jm;
        }

        #endregion 添加单个货品到购物车

        #region 获取购物车列表======================================================================

        /// <summary>
        /// 获取购物车列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetList([FromBody] FMCartGetList entity)
        {
            var ids = CommonHelper.StringToIntArray(entity.ids);
            //判断免费运费
            var freeFreight = entity.receiptType != 1;
            //获取数据
            var jm = await _cartServices.GetCartInfos(_user.ID, ids, entity.type, entity.areaId, entity.point, entity.couponCode, freeFreight, entity.receiptType, entity.objectId);

            return jm;
        }

        #endregion 获取购物车列表======================================================================

        #region 删除购物车信息

        /// <summary>
        /// 获取购物车列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> DoDelete([FromBody] FMIntId entity)
        {
            var jm = new WebApiCallBack();

            if (entity.id <= 0)
            {
                jm.msg = "请提交要删除的货品";
                return jm;
            }
            jm = await _cartServices.DeleteByIdsAsync(entity.id, _user.ID);

            return jm;
        }

        #endregion 删除购物车信息

        #region 设置购物车商品数量

        /// <summary>
        /// 设置购物车商品数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> SetCartNum([FromBody] FMSetCartNum entity)
        {
            var jm = await _cartServices.SetCartNum(entity.id, entity.nums, _user.ID, 2, 1);
            return jm;
        }

        #endregion 设置购物车商品数量

        #region 根据提交的数据判断哪些购物券可以使用==================================================

        /// <summary>
        /// 根据提交的数据判断哪些购物券可以使用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<WebApiCallBack> GetCartAvailableCoupon([FromBody] FMCouponForUserCouponPost entity)
        {
            var ids = CommonHelper.StringToIntArray(entity.ids);
            var jm = await _cartServices.GetCartAvailableCoupon(_user.ID, ids);
            return jm;
        }

        #endregion 根据提交的数据判断哪些购物券可以使用==================================================
    }
}