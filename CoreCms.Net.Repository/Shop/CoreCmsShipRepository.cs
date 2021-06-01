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
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Loging;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Utility.Extensions;
using CoreCms.Net.Utility.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 配送方式表 接口实现
    /// </summary>
    public class CoreCmsShipRepository : BaseRepository<CoreCmsShip>, ICoreCmsShipRepository
    {
        public CoreCmsShipRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsShip entity)
        {
            var jm = new AdminUiCallBack();

            var logistics = await DbClient.Queryable<CoreCmsLogistics>().FirstAsync(p => p.logiCode == entity.logiCode);
            if (logistics != null)
            {
                entity.logiName = logistics.logiName;
            }
            var id = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync();
            var bl = id > 0;

            if (bl && entity.isDefault == true)
            {
                await DbClient.Updateable<CoreCmsShip>().SetColumns(p => p.isDefault == false).Where(p => p.isDefault == true && p.id != id).ExecuteCommandAsync();
            }

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;

            return jm;
        }


        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsShip entity)
        {
            var jm = new AdminUiCallBack();

            if (entity.isDefault == false)
            {
                var otherHave = await DbClient.Queryable<CoreCmsShip>().AnyAsync(p => p.isDefault == true && p.id != entity.id);
                if (otherHave == false)
                {
                    jm.msg = "请保持一个默认配送方式";
                    return jm;

                }
            }

            var oldModel = await DbClient.Queryable<CoreCmsShip>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            
            //事物处理过程开始
            //oldModel.id = entity.id;
            oldModel.name = entity.name;
            oldModel.isCashOnDelivery = entity.isCashOnDelivery;
            oldModel.firstUnit = entity.firstUnit;
            oldModel.continueUnit = entity.continueUnit;
            oldModel.isdefaultAreaFee = entity.isdefaultAreaFee;
            oldModel.areaType = entity.areaType;
            oldModel.firstunitPrice = entity.firstunitPrice;
            oldModel.continueunitPrice = entity.continueunitPrice;
            oldModel.exp = entity.exp;
            oldModel.logiName = entity.logiName;
            oldModel.logiCode = entity.logiCode;
            oldModel.isDefault = entity.isDefault;
            oldModel.sort = entity.sort;
            oldModel.status = entity.status;
            oldModel.isfreePostage = entity.isfreePostage;
            oldModel.areaFee = entity.areaFee;
            oldModel.goodsMoney = entity.goodsMoney;

            var logistics = await DbClient.Queryable<CoreCmsLogistics>().FirstAsync(p => p.logiCode == oldModel.logiCode);
            if (logistics != null)
            {
                oldModel.logiName = logistics.logiName;
            }

            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            if (bl && entity.isDefault == true)
            {
                await DbClient.Updateable<CoreCmsShip>().SetColumns(p => p.isDefault == false).Where(p => p.isDefault == true && p.id != oldModel.id).ExecuteCommandAsync();
            }
            return jm;
        }


        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack> DeleteByIdAsync(int id)
        {
            var jm = new AdminUiCallBack();

            var isDefault = await DbClient.Queryable<CoreCmsShip>().AnyAsync(p => p.isDefault == true && p.id == id);
            if (isDefault)
            {
                jm.msg = "默认方式禁止删除";
            }
            var bl = await DbClient.Deleteable<CoreCmsShip>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;

            return jm;
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
            var jm = new AdminUiCallBack();

            if (isDefault == false)
            {
                var otherHave = await DbClient.Queryable<CoreCmsShip>().AnyAsync(p => p.isDefault == true && p.id != id);
                if (otherHave == false)
                {
                    jm.msg = "请保持一个默认配送方式";
                    return jm;
                }
            }
            //事物处理过程结束
            var bl = await DbClient.Updateable<CoreCmsShip>().SetColumns(p => p.isDefault == isDefault).Where(p => p.id == id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;

            if (bl && isDefault == true)
            {
                await DbClient.Updateable<CoreCmsShip>().SetColumns(p => p.isDefault == false).Where(p => p.isDefault == true && p.id != id).ExecuteCommandAsync();
            }
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
        public new async Task<IPageList<CoreCmsShip>> QueryPageAsync(Expression<Func<CoreCmsShip, bool>> predicate,
            Expression<Func<CoreCmsShip, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsShip> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsShip>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsShip
                {
                    id = p.id,
                    name = p.name,
                    isCashOnDelivery = p.isCashOnDelivery,
                    firstUnit = p.firstUnit,
                    continueUnit = p.continueUnit,
                    isdefaultAreaFee = p.isdefaultAreaFee,
                    areaType = p.areaType,
                    firstunitPrice = p.firstunitPrice,
                    continueunitPrice = p.continueunitPrice,
                    exp = p.exp,
                    logiName = p.logiName,
                    logiCode = p.logiCode,
                    isDefault = p.isDefault,
                    sort = p.sort,
                    status = p.status,
                    isfreePostage = p.isfreePostage,
                    areaFee = p.areaFee,
                    goodsMoney = p.goodsMoney,

                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsShip>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsShip
                {
                    id = p.id,
                    name = p.name,
                    isCashOnDelivery = p.isCashOnDelivery,
                    firstUnit = p.firstUnit,
                    continueUnit = p.continueUnit,
                    isdefaultAreaFee = p.isdefaultAreaFee,
                    areaType = p.areaType,
                    firstunitPrice = p.firstunitPrice,
                    continueunitPrice = p.continueunitPrice,
                    exp = p.exp,
                    logiName = p.logiName,
                    logiCode = p.logiCode,
                    isDefault = p.isDefault,
                    sort = p.sort,
                    status = p.status,
                    isfreePostage = p.isfreePostage,
                    areaFee = p.areaFee,
                    goodsMoney = p.goodsMoney,

                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsShip>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion


    }
}
