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
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.IServices;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;


namespace CoreCms.Net.Services
{
    /// <summary>
    /// 配送方式表 接口实现
    /// </summary>
    public class CoreCmsShipServices : BaseServices<CoreCmsShip>, ICoreCmsShipServices
    {
        private readonly ICoreCmsShipRepository _dal;
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsShipServices(IUnitOfWork unitOfWork, ICoreCmsShipRepository dal)
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
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsShip entity)
        {
            return await _dal.InsertAsync(entity);
        }

        /// <summary>
        /// 重写异步更新方法方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsShip entity)
        {
            return await _dal.UpdateAsync(entity);
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

        #endregion


        /// <summary>
        /// 设置是否默认
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> SetIsDefault(int id, bool isDefault)
        {
            return await _dal.SetIsDefault(id, isDefault);
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
        public new async Task<IPageList<CoreCmsShip>> QueryPageAsync(Expression<Func<CoreCmsShip, bool>> predicate,
            Expression<Func<CoreCmsShip, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            return await _dal.QueryPageAsync(predicate, orderByExpression, orderByType, pageIndex, pageSize, blUseNoLock);
        }
        #endregion

        /// <summary>
        /// 获取配送费用
        /// </summary>
        /// <param name="areaId">地区id</param>
        /// <param name="weight">重量,单位g</param>
        /// <param name="totalmoney">商品总价</param>
        /// <returns></returns>
        public decimal GetShipCost(int areaId = 0, decimal weight = 0, decimal totalmoney = 0)
        {
            decimal postfee = 0;

            var idStr = areaId.ToString();
            //先判断是否子地区满足条件
            var def = base.QueryByClause(p =>
                 p.status == (int)GlobalEnumVars.ShipStatus.Yes &&
                 p.areaType == (int)GlobalEnumVars.ShipAreaType.Part && p.areaFee.Contains(idStr));
            //没有子地区取默认
            if (def == null)
            {
                def = base.QueryByClause(p => p.isDefault == true && p.status == (int)GlobalEnumVars.ShipStatus.Yes);
            }
            //没有默认取启用状态
            if (def == null)
            {
                def = base.QueryByClause(p => p.status == (int)GlobalEnumVars.ShipStatus.Yes);
                if (def == null)
                {//没有配送方式，返回0
                    return postfee;
                }
            }
            //是否包邮
            if (def.isfreePostage == true)
            {
                return postfee;
            }

            if (def.areaType == (int)GlobalEnumVars.ShipAreaType.Part)
            {
                var areaFee = (JArray)JsonConvert.DeserializeObject(def.areaFee);
                if (areaFee != null && areaFee.Count > 0)
                {
                    var isIn = false;
                    foreach (var jToken in areaFee)
                    {
                        var item = (JObject)jToken;
                        //if (item.Property("area") == null) continue;
                        var area = item["area"].ObjectToString();
                        var firstunitAreaPrice = item["firstunitAreaPrice"].ObjectToInt(0);
                        if (!string.IsNullOrEmpty(area))
                        {
                            var areaArr = CommonHelper.StringToIntArray(area);
                            if (areaArr.Contains(areaId))
                            {
                                isIn = true;
                                var total = calculate_fee(def, weight, totalmoney, firstunitAreaPrice);
                                postfee = Math.Round(total, 2);
                                break;
                            }
                        }
                    }
                    if (!isIn)
                    {
                        var total = calculate_fee(def, weight, totalmoney, 0);
                        postfee = Math.Round(total, 2);
                    }
                }
                else
                {
                    var total = calculate_fee(def, weight, totalmoney, 0);
                    postfee = Math.Round(total, 2);
                }
            }
            else
            {
                var total = calculate_fee(def, weight, totalmoney, 0);
                postfee = Math.Round(total, 2);
            }
            return postfee;
        }

        /// <summary>
        /// 计算运费
        /// </summary>
        /// <param name="ship">配送方式内容</param>
        /// <param name="weight">订单总重</param>
        /// <param name="totalmoney">商品总价</param>
        /// <param name="firstunitAreaPrice"></param>
        /// <returns></returns>
        public decimal calculate_fee(CoreCmsShip ship, decimal weight, decimal totalmoney = 0, decimal firstunitAreaPrice = 0)
        {
            //满多少免运费
            if (ship.goodsMoney > 0 && totalmoney >= ship.goodsMoney)
            {
                return 0;
            }

            if (weight > 0 && weight > ship.firstUnit)
            {
                decimal shipMoney = ship.firstunitPrice + (Math.Ceiling(Math.Abs(weight - ship.firstUnit) / ship.continueUnit) * ship.continueunitPrice);
                return shipMoney;
            }
            else
            {
                if (ship.firstunitPrice > 0)
                {
                    return ship.firstunitPrice;
                }
                else
                {
                    return firstunitAreaPrice;
                }
            }
        }

        /// <summary>
        /// 根据地区获取配送方式
        /// </summary>
        public CoreCmsShip GetShip(int areaId = 0)
        {
            var idStr = areaId.ToString();
            //先判断是否子地区满足条件
            var def = base.QueryByClause(p =>
                p.status == (int)GlobalEnumVars.ShipStatus.Yes &&
                p.areaType == (int)GlobalEnumVars.ShipAreaType.Part && p.areaFee.Contains(idStr));
            //没有子地区取默认
            if (def == null)
            {
                def = base.QueryByClause(p => p.isDefault == true && p.status == (int)GlobalEnumVars.ShipStatus.Yes);
            }
            //没有默认取启用状态
            if (def == null)
            {
                def = base.QueryByClause(p => p.status == (int)GlobalEnumVars.ShipStatus.Yes);
                return def;
            }
            return def;
        }

    }
}
